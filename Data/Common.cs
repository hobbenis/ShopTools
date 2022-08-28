using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ShopTools.Data.Etsy;
using Newtonsoft.Json;
using RestSharp;

namespace ShopTools.Data.Common;

public class BoundObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Dictionary<string, object?> myFields;

    protected void Set(object value, [CallerMemberName] string propName = "")
    {
        if (myFields is null)
        {
            myFields = new();
        }
            
        if (myFields.ContainsKey(propName) && myFields[propName].Equals(value))
        {
            return;
        }

        myFields[propName] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    protected object? Get([CallerMemberName] string propName = "")
    {
        if (myFields is null)
        {
            myFields = new();
        }

        if (!myFields.ContainsKey(propName))
        {
            return null;
        }
            
        return myFields[propName];
    }
}

public class oAuthToken
{
    public string code_verifier;
    public string state;
    public string auth_code;
    public string access_token;
    public string refresh_token;
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
    public LockedString shop_id;
    public LockedString api_key;
    public LockedString access_token;
    public LockedString refresh_token;
}

public class AuthLockBox
{
    public long shop_id
    {
        get
        {
            return long.Parse(myAuthSet.shop_id.GetPlainText());
        }
        set
        {
            myAuthSet.shop_id.SetPlainText(value.ToString());
        }
    }
    
    public string api_key => myAuthSet.api_key.GetPlainText();
    
    public string refresh_token
    {
        get { return myAuthSet.refresh_token.GetPlainText(); }
        set { myAuthSet.refresh_token.SetPlainText(value); }
    }

    public string access_token
    {
        get { return myAuthSet.access_token.GetPlainText(); }
        set { myAuthSet.access_token.SetPlainText(value); }
    }
    
    private AuthSet myAuthSet;
    //private byte[] myKey;
    private string myAuthPath;
    
    public AuthLockBox(string thisAuthPath, string thisPass)
    {
        myAuthPath = thisAuthPath;
        
        if (!File.Exists(thisAuthPath))
        {
            throw new Exception("Auth file does not exist.");
        }
        
        myAuthSet = JsonConvert.DeserializeObject<AuthSet>(File.ReadAllText(myAuthPath));

        ContinueInit(thisPass);
    }
    
    public AuthLockBox(string thisAuthPath, string thisPass, string thisApiKey, string thisShopId)
    {
        myAuthPath = thisAuthPath;
        
        myAuthSet = new AuthSet();
        
        myAuthSet.access_token = new LockedString();
        myAuthSet.refresh_token = new LockedString();
        myAuthSet.api_key = new LockedString();
        myAuthSet.shop_id = new LockedString();

        myAuthSet.api_key.SetPlainText(thisApiKey);
        myAuthSet.shop_id.SetPlainText(thisShopId);
        
        ContinueInit(thisPass);
        
        Save();
    }

    private void ContinueInit(string thisPass)
    {
        myAuthSet.access_token.SetPass(thisPass);
        myAuthSet.refresh_token.SetPass(thisPass);
        myAuthSet.api_key.SetPass(thisPass);
        myAuthSet.shop_id.SetPass(thisPass);
    }
    
    public void Save()
    {
        string saveContents = JsonConvert.SerializeObject(myAuthSet, Formatting.Indented);

        Directory.CreateDirectory(Path.GetDirectoryName(myAuthPath));
        
        File.WriteAllText(myAuthPath, saveContents);
    }
}