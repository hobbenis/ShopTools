using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;
using ShopTools.Etsy;

namespace ShopTools.Common;

public class OAuthToken
{
    [JsonProperty("code_verifier")] public string CodeVerifier { get; set; }
    [JsonProperty("state")] public string State { get; set; }
    [JsonProperty("auth_code")] public string AuthCode { get; set; }
    [JsonProperty("access_token")] public string AccessToken { get; set; }
    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
}

public class OAuth2
{
    private string apiKey;
    private string connectUri;
    private string oauthUri;
    private string code_challenge_method;
    private string redirect_uri;
    
    public OAuth2(string thisApiKey, string thisConnectUri, string thisOauthUri,
        string thisCodeChallengeMethod, string thisRedirectUri)
    {
        this.apiKey = thisApiKey;
        this.connectUri = thisConnectUri;
        this.oauthUri = thisOauthUri;
        this.code_challenge_method = thisCodeChallengeMethod;
        this.redirect_uri = thisRedirectUri;
    }
    
    private JsonSerializerSettings myJsonSerializerSettings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };
    
    public string GetAuthorizationUrl(string myCodeVer, string state, string[] scopes)
    {
        var response_type = "code";
        var encoded_scopes = string.Join("%20", scopes);
        var code_challenge = GetBase64UrlSafe(GetSha256(myCodeVer));
        string code_challenge_method = "S256";
        
        return $"{connectUri}?"
               + $"response_type={response_type}"
               + $"&redirect_uri={redirect_uri}"
               + $"&scope={encoded_scopes}"
               + $"&client_id={apiKey}"
               + $"&state={state}"
               + $"&code_challenge={code_challenge}"
               + $"&code_challenge_method={code_challenge_method}";
    }
    
    public AccessToken RequestAccessToken(string authCode, string codeVer)
    {
        var myReq = new RestRequest("token", Method.Post);

        myReq.AddHeader("Content-Type", "application/json");

        var myParams = new Dictionary<string, string>();

        myParams["grant_type"] = "authorization_code";
        myParams["client_id"] = apiKey;
        myParams["redirect_uri"] = redirect_uri;
        myParams["code"] = authCode;
        myParams["code_verifier"] = codeVer;

        var requestBody = JsonConvert.SerializeObject(myParams);

        myReq.AddBody(requestBody, "application/json");

        using var tokenClient = new RestClient(oauthUri);
        tokenClient.AddDefaultHeader("x-api-key", apiKey);
        
        var retContents = tokenClient.ExecuteAsync(myReq).GetAwaiter().GetResult().Content;
        
        var time = DateTime.Now.ToString("yyyyMMddHmmss");
        
        return JsonConvert.DeserializeObject<AccessToken>(retContents
            , myJsonSerializerSettings);
    }
    
    public AccessToken RefreshAccessToken(string refreshToken)
    {
        var myReq = new RestRequest("token", Method.Post);
        
        myReq.AddHeader("Content-Type", "application/json");
        
        var myParams = new Dictionary<string, string>();
        
        myParams["grant_type"] = "refresh_token";
        myParams["client_id"] = apiKey;
        myParams["refresh_token"] = refreshToken;
        
        var requestBody = JsonConvert.SerializeObject(myParams);
        
        myReq.AddBody(requestBody, "application/json");
        
        using var tokenClient = new RestClient(oauthUri);
        tokenClient.AddDefaultHeader("x-api-key", apiKey);
        
        var retContents = tokenClient.ExecuteAsync(myReq).GetAwaiter().GetResult().Content;
        
        AccessToken retToken = JsonConvert.DeserializeObject<AccessToken>(retContents
            , myJsonSerializerSettings);
        
        return retToken;
    }
    
    private byte[] GetSalt()
    {
        byte[] thisSalt = new byte[8];
    
        using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(thisSalt);
        }
    
        return thisSalt;
    }
    
    public string GetRandomString(int myLength)
    {
        StringBuilder myStringB = new StringBuilder();
        
        for (var i = 0; i < myLength; i = i + 8)
        {
            myStringB.Append(Path.GetRandomFileName().Substring(0, Math.Min(8, myLength - i)));
        }
        
        return myStringB.ToString();
    }

    public byte[] GetSha256(string thisString)
    {
        using var sha256 = SHA256.Create();
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(thisString));
        }
    }
    
    public string GetBase64UrlSafe(byte[] thisData)
    {
        string myEncData = Convert.ToBase64String(thisData);
        string myUrlSafData = Regex.Replace(myEncData, "\\+", "-");
        myUrlSafData = Regex.Replace(myUrlSafData, "\\/", "_");
        myUrlSafData = Regex.Replace(myUrlSafData, "=+$", "");
        return myUrlSafData;
    }
}

public class AuthSet
{
    [JsonProperty("shop_id")] public LockedString ShopId;
    [JsonProperty("api_key")] public LockedString ApiKey;
    [JsonProperty("access_token")] public LockedString AccessToken;
    [JsonProperty("refresh_token")] public LockedString RefreshToken;
}

public class AuthLockBox
{
    public long shop_id
    {
        get
        {
            return long.Parse(myAuthSet.ShopId.Plaintext);
        }
        set
        {
            myAuthSet.ShopId.Plaintext = value.ToString();
        }
    }
    
    public string api_key => myAuthSet.ApiKey.Plaintext;
    
    public string refresh_token
    {
        get { return myAuthSet.RefreshToken.Plaintext; }
        set { myAuthSet.RefreshToken.Plaintext = value; }
    }

    public string access_token
    {
        get { return myAuthSet.AccessToken.Plaintext; }
        set { myAuthSet.AccessToken.Plaintext = value; }
    }
    
    private AuthSet myAuthSet;
    private string myAuthPath;
    
    public AuthLockBox(string thisAuthPath, string thisPass)
    {
        myAuthPath = thisAuthPath;
        
        if (!File.Exists(thisAuthPath))
        {
            throw new Exception("Auth file does not exist.");
        }
        
        myAuthSet = JsonConvert.DeserializeObject<AuthSet>(File.ReadAllText(myAuthPath));

        myAuthSet.AccessToken.Passphrase = thisPass;
        myAuthSet.ApiKey.Passphrase = thisPass;
        myAuthSet.RefreshToken.Passphrase = thisPass;
        myAuthSet.ShopId.Passphrase = thisPass;
    }
    
    public AuthLockBox(string thisAuthPath, string thisPass, string thisApiKey, string thisShopId)
    {
        myAuthPath = thisAuthPath;
        
        myAuthSet = new AuthSet();
        
        myAuthSet.AccessToken = new LockedString(thisPass, string.Empty);
        myAuthSet.RefreshToken = new LockedString(thisPass, string.Empty);
        myAuthSet.ApiKey = new LockedString(thisPass, thisApiKey);
        myAuthSet.ShopId = new LockedString(thisPass, thisShopId);
        
        Save();
    }

    public void Save()
    {
        string saveContents = JsonConvert.SerializeObject(myAuthSet, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            
        });

        Directory.CreateDirectory(Path.GetDirectoryName(myAuthPath));
        
        File.WriteAllText(myAuthPath, saveContents);
    }
}