//Neither me nor this project have any affiliation with Etsy.
//This class/namespace/etc is simply named as such because it accesses Etsy's API
//You will need to provide your own API key if you wish to use this.
//As such, you should carefully review this and make sure it will do what you need.

//this file is to contain classes, etc that are specific to etsy's api

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RestSharp;
using ShopTools.Data.Market;
using ShopTools.Data.Common;

namespace ShopTools.Data.Etsy;

public enum EtsyScopes
{
    address_r,	    //Read a member's shipping addresses.
    address_w,	    //Update and delete a member's shipping address.
    billing_r,	    //Read a member's Etsy bill charges and payments.
    cart_r,	        //Read the contents of a member’s cart.
    cart_w,	        //Add and remove listings from a member's cart.
    email_r,	    //Read a member's email address
    favorites_r,	//View a member's favorite listings and users.
    favorites_w,	//Add to and remove from a member's favorite listings and users.
    feedback_r,	    //View all details of a member's feedback (including purchase history.)
    listings_d,	    //Delete a member's listings.
    listings_r,	    //Read a member's inactive and expired (i.e., non-public) listings.
    listings_w,	    //Create and edit a member's listings.
    profile_r,	    //Read a member's private profile information.
    profile_w,	    //Update a member's private profile information.
    recommend_r,	//View a member's recommended listings.
    recommend_w,	//Remove a member's recommended listings.
    shops_r,	    //See a member's shop description, messages and sections, even if not (yet) public.
    shops_w,	    //Update a member's shop description, messages and sections.
    transactions_r,	//Read a member's purchase and sales data. This applies to buyers as well as sellers.
    transactions_w,	//Update a member's sales data.
}

public abstract class EtsyObject
{
    [JsonProperty("error")] public string SetError { set { error = value; } }
    [JsonProperty("error_description")] public string SetErrorDescription { set { error_description = value; } }
    [JsonProperty("error_uri")] public string SetErrorUri { set { error_uri = value; } }
    private string error;
    private string error_description;
    private string error_uri;
    [JsonIgnore] public string Error => error;
    [JsonIgnore] public string ErrorDescription => error_description;
    [JsonIgnore] public string ErrorUri => error_uri;
    
    [JsonIgnore]
    public string Platform => "https://www.etsy.com/";
    
    protected EtsyConnection MyConnection;

    public virtual void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
    }
}

public class EtsyProperty : EtsyObject
{
    [JsonProperty("property_id")] public long PropertyId { get; set; }
    [JsonProperty("property_name")] public string PropertyName { get; set; }
    [JsonProperty("scale_id")] public long ScaleId { get; set; }
    [JsonProperty("scale_name")] public string ScaleName { get; set; }
    [JsonProperty("value_ids")] public BindingList<long> ValueIds { get; set; }
    [JsonProperty("values")] public BindingList<string> Values { get; set; }
}

public class EtsyPrice : EtsyObject
{
    [JsonProperty("amount")] public int Amount { get; set; }
    [JsonProperty("divisor")] public int Divisor { get; set; }
    [JsonProperty("currency_code")] public string CurrencyCode { get; set; }
}

public class EtsyTranslation : EtsyObject
{
    [JsonProperty("listing_id")] public long ListingId { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("tags")] public string[] Tags { get; set; }
}

public class EtsyReview : EtsyObject
{
    [JsonProperty("shop_id")] public long ShopId { get; set; }
    [JsonProperty("listing_id")] public long ListingId { get; set; }
    [JsonProperty("transaction_id")] public long TransactionId { get; set; }
    [JsonProperty("buyer_user_id")] public long BuyerUserId { get; set; }
    [JsonProperty("rating")] public int Rating { get; set; }
    [JsonProperty("review")] public string Review { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("image_url_fullxfull")] public string ImageUrlFullxfull { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("update_timestamp")] public int UpdateTimestamp { get; set; }
}

public class EtsyShop : EtsyObject
{
    [JsonProperty("shop_id")] public long ShopId { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("shop_name")] public string ShopName { get; set; }
    [JsonProperty("create_date")] public int CreateDate { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("announcement")] public string Announcement { get; set; }
    [JsonProperty("currency_code")] public string CurrencyCode { get; set; }
    [JsonProperty("is_vacation")] public bool IsVacation { get; set; }
    [JsonProperty("vacation_message")] public string VacationMessage { get; set; }
    [JsonProperty("sale_message")] public string SaleMessage { get; set; }
    [JsonProperty("digital_sale_message")] public string DigitalSaleMessage { get; set; }
    [JsonProperty("update_date")] public int UpdateDate { get; set; }
    [JsonProperty("listing_active_count")] public int ListingActiveCount { get; set; }
    [JsonProperty("digital_listing_count")] public int DigitalListingCount { get; set; }
    [JsonProperty("login_name")] public string LoginName { get; set; }
    [JsonProperty("accepts_custom_requests")] public bool AcceptsCustomRequests { get; set; }
    [JsonProperty("policy_welcome")] public string PolicyWelcome { get; set; }
    [JsonProperty("policy_payment")] public string PolicyPayment { get; set; }
    [JsonProperty("policy_shipping")] public string PolicyShipping { get; set; }
    [JsonProperty("policy_refunds")] public string PolicyRefunds { get; set; }
    [JsonProperty("policy_additional")] public string PolicyAdditional { get; set; }
    [JsonProperty("policy_seller_info")] public string PolicySellerInfo { get; set; }
    [JsonProperty("policy_update_date")] public int PolicyUpdateDate { get; set; }
    [JsonProperty("policy_has_private_receipt_info")] public bool PolicyHasPrivateReceiptInfo { get; set; }
    [JsonProperty("has_unstructured_policies")] public bool Policies { get; set; }
    [JsonProperty("policy_privacy")] public string PolicyPrivacy { get; set; }
    [JsonProperty("vacation_autoreply")] public string VacationAutoreply { get; set; }
    [JsonProperty("url")] public string Url { get; set; }
    [JsonProperty("image_url_760x100")] public string ImageUrl760X100 { get; set; }
    [JsonProperty("num_favorers")] public int NumFavorers { get; set; }
    [JsonProperty("languages")] public List<string> Languages { get; set; }
    [JsonProperty("icon_url_fullxfull")] public string IconUrlFullxfull { get; set; }
    [JsonProperty("is_using_structured_policies")] public bool IsUsingStructuredPolicies { get; set; }
    [JsonProperty("has_onboarded_structured_policies")] public bool HasOnboardedStructuredPolicies { get; set; }
    [JsonProperty("include_dispute_form_link")] public bool IncludeDisputeFormLink { get; set; }
    [JsonProperty("is_direct_checkout_onboarded")] public bool IsDirectCheckoutOnboarded { get; set; }
    [JsonProperty("is_etsy_payments_onboarded")] public bool IsEtsyPaymentsOnboarded { get; set; }
    [JsonProperty("is_calculated_eligible")] public bool IsCalculatedEligible { get; set; }
    [JsonProperty("is_opted_in_to_buyer_promise")] public bool IsOptedInToBuyerPromise { get; set; }
    [JsonProperty("is_shop_us_based")] public bool IsShopUsBased { get; set; }
    [JsonProperty("transaction_sold_count")] public int TransactionSoldCount { get; set; }
    [JsonProperty("shipping_from_country_iso")] public string ShippingFromCountryIso { get; set; }
    [JsonProperty("shop_location_country_iso")] public string ShopLocationCountryIso { get; set; }
    [JsonProperty("review_count")] public int ReviewCount { get; set; }
    [JsonProperty("review_average")] public double ReviewAverage { get; set; }
}

public class EtsyShopSection : EtsyObject
{
    [JsonProperty("shop_section_id")] public long ShopSectionId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("rank")] public int Rank { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("active_listing_count")] public int ActiveListingCount { get; set; }
}
    
public class EtsyProductionPartner : EtsyObject
{
    [JsonProperty("production_partner_id")] public long ProductionPartnerId { get; set; }
    [JsonProperty("partner_name")] public string PartnerName { get; set; }
    [JsonProperty("location")] public string Location { get; set; }
}

public class EtsyUser : EtsyObject
{
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("login_name")] public string LoginName { get; set; }
    [JsonProperty("primary_email")] public string PrimaryEmail { get; set; }
    [JsonProperty("first_name")] public string FirstName { get; set; }
    [JsonProperty("last_name")] public string LastName { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("referred_by_user_id")] public long ReferredByUserId { get; set; }
    [JsonProperty("use_new_inventory_endpoints")] public bool UseNewInventoryEndpoints { get; set; }
    [JsonProperty("is_seller")] public bool IsSeller { get; set; }
    [JsonProperty("bio")] public string Bio { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("birth_month")] public string BirthMonth { get; set; }
    [JsonProperty("birth_day")] public string BirthDay { get; set; }
    [JsonProperty("transaction_buy_count")] public int TransactionBuyCount { get; set; }
    [JsonProperty("transaction_sold_count")] public int TransactionSoldCount { get; set; }
}

public class EtsyUserAddress : EtsyObject
{
    [JsonProperty("user_address_id")] public long UserAddressId { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("first_line")] public string FirstLine { get; set; }
    [JsonProperty("second_line")] public string SecondLine { get; set; }
    [JsonProperty("city")] public string City { get; set; }
    [JsonProperty("state")] public string State { get; set; }
    [JsonProperty("zip")] public string Zip { get; set; }
    [JsonProperty("iso_country_code")] public string IsoCountryCode { get; set; }
    [JsonProperty("country_name")] public string CountryName { get; set; }
    [JsonProperty("is_default_shipping_address")] public bool IsDefaultShippingAddress { get; set; }
}

public class EtsyLedgerEntry : EtsyObject
{
    [JsonProperty("entry_id")] public long EntryId { get; set; }
    [JsonProperty("ledger_id")] public long LedgerId { get; set; }
    [JsonProperty("sequence_number")] public int SequenceNumber { get; set; }
    [JsonProperty("amount")] public double Amount { get; set; }
    [JsonProperty("currency")] public string Currency { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("balance")] public double Balance { get; set; }
    [JsonProperty("create_date")] public int CreateDate { get; set; }
    [JsonProperty("ledger_type")] public string LedgerType { get; set; }
    [JsonProperty("reference_type")] public string ReferenceType { get; set; }
    [JsonProperty("reference_id")] public string ReferenceId { get; set; }
}
    
public class EtsyPayment : EtsyObject
{
    [JsonProperty("payment_id")] public long PaymentId { get; set; }
    [JsonProperty("buyer_user_id")] public long BuyerUserId { get; set; }
    [JsonProperty("shop_id")] public long ShopId { get; set; }
    [JsonProperty("receipt_id")] public long ReceiptId { get; set; }
    [JsonProperty("amount_gross")] public EtsyPrice AmountGross { get; set; }
    [JsonProperty("amount_fees")] public EtsyPrice AmountFees { get; set; }
    [JsonProperty("amount_net")] public EtsyPrice AmountNet { get; set; }
    [JsonProperty("posted_gross")] public EtsyPrice PostedGross { get; set; }
    [JsonProperty("posted_fees")] public EtsyPrice PostedFees { get; set; }
    [JsonProperty("posted_net")] public EtsyPrice PostedNet { get; set; }
    [JsonProperty("adjusted_gross")] public EtsyPrice AdjustedGross { get; set; }
    [JsonProperty("adjusted_fees")] public EtsyPrice AdjustedFees { get; set; }
    [JsonProperty("adjusted_net")] public EtsyPrice AdjustedNet { get; set; }
    [JsonProperty("currency")] public string Currency { get; set; }
    [JsonProperty("shop_currency")] public string ShopCurrency { get; set; }
    [JsonProperty("buyer_currency")] public string BuyerCurrency { get; set; }
    [JsonProperty("shipping_user_id")] public long ShippingUserId { get; set; }
    [JsonProperty("shipping_address_id")] public long ShippingAddressId { get; set; }
    [JsonProperty("billing_address_id")] public long BillingAddressId { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("shipped_timestamp")] public int ShippedTimestamp { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("update_timestamp")] public int UpdateTimestamp { get; set; }
    [JsonProperty("payment_adjustments")] public List<EtsyPaymentAdjustment> PaymentAdjustments { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (PaymentAdjustments is not null)
        {
            foreach (var thisObj in PaymentAdjustments) { thisObj.SetConnection(thisConn); }
        }
        
        if (AmountGross is not null) {AmountGross.SetConnection(thisConn);}
        if (AmountFees is not null) { AmountFees.SetConnection(thisConn);}
        if (AmountNet is not null) {AmountNet.SetConnection(thisConn);}
        if (PostedGross is not null) {PostedGross.SetConnection(thisConn);}
        if (PostedFees is not null) {PostedFees.SetConnection(thisConn);}
        if (PostedNet is not null) {PostedNet.SetConnection(thisConn);}
        if (AdjustedGross is not null) {AdjustedGross.SetConnection(thisConn);}
        if (AdjustedFees is not null) {AdjustedFees.SetConnection(thisConn);}
        if (AdjustedNet is not null) {AdjustedNet.SetConnection(thisConn);}
    }
}

public class EtsyPaymentAdjustment : EtsyObject
{
    [JsonProperty("payment_adjustment_id")] public long PaymentAdjustmentId { get; set; }
    [JsonProperty("payment_id")] public long PaymentId { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("is_success")] public bool IsSuccess { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("reason_code")] public string ReasonCode { get; set; }
    [JsonProperty("total_adjustment_amount")] public double TotalAdjustmentAmount { get; set; }
    [JsonProperty("shop_total_adjustment_amount")] public double ShopTotalAdjustmentAmount { get; set; }
    [JsonProperty("buyer_total_adjustment_amount")] public double BuyerTotalAdjustmentAmount { get; set; }
    [JsonProperty("total_fee_adjustment_amount")] public double TotalFeeAdjustmentAmount { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("update_timestamp")] public int UpdateTimestamp { get; set; }
}

public class EtsyVariation : EtsyObject
{
    [JsonProperty("property_id")] public long PropertyId { get; set; }
    [JsonProperty("value_id")] public long ValueId { get; set; }
    [JsonProperty("formatted_name")] public string FormattedName { get; set; }
    [JsonProperty("formatted_value")] public string FormattedValue { get; set; }
}
    
public class Results<TEtsyType> : EtsyObject
{
    public int count { get; set; }
    public List<TEtsyType> results { get; set; }
}

public class AccessToken : EtsyObject
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
}

public class EtsyConnection : IMarketConnection
{
    private string myCacheFolder;
    private string errorsFolder => $@"{myCacheFolder}\errors";
    private string responsesFolder => $@"{myCacheFolder}\responses";
    private string imagesFolder => $@"{myCacheFolder}\images";
    private string thumbsFolder => $@"{imagesFolder}\75x75";
    private string fullImagesFolder => $@"{imagesFolder}\full";
    private string objectsFolder => $@"{myCacheFolder}\objects";
    private string listingsFile => $@"{objectsFolder}\listings.txt";

    private string authFile => $@"{myCacheFolder}\auth.txt";

    private string baseUri = @"https://api.etsy.com/v3/application";
    private string accessUri = @"https://www.etsy.com/oauth/connect";
    private string oauthUri = @"https://api.etsy.com/v3/public/oauth";
    private string code_challenge_method = "S256";
    private string redirect_uri = @"http://localhost:3003/oauth/redirect";

    private AuthLockBox myAuthBox;
    public AuthLockBox AuthBox => myAuthBox;
    
    public long shopId
    {
        get { return myAuthBox.shop_id; }
        set { myAuthBox.shop_id = value; }
    }

    private OAuth2 myAuth;
    public OAuth2 Auth => myAuth;
    
    private RestClient myRest;
    private JsonSerializerSettings myJsonSerializerSettings;
    private WebClient myWeb;

    private EtsyShop _etsyShop;

    public Dictionary<long, string> cachedThumbPaths;
    public Dictionary<long, EtsyListing> cachedListings;
    public Dictionary<long, EtsyReceipt> cachedReceipts;

    public EtsyConnection(string thisCacheFolder, string passWd)
    {
        myCacheFolder = thisCacheFolder;
        myAuthBox = new AuthLockBox(authFile, passWd);

        ContinueInit();
    }
    
    public EtsyConnection(string thisCacheFolder, string passWd, string thisApiKey, string thisShopId)
    {
        myCacheFolder = thisCacheFolder;
        myAuthBox = new AuthLockBox(authFile, passWd, thisApiKey, thisShopId);

        ContinueInit();
    }
    
    private void ContinueInit()
    {
        cachedListings = new Dictionary<long, EtsyListing>();
        cachedReceipts = new Dictionary<long, EtsyReceipt>();
        cachedThumbPaths = new Dictionary<long, string>();
        
        Directory.CreateDirectory(errorsFolder);
        Directory.CreateDirectory(responsesFolder);
        Directory.CreateDirectory(imagesFolder);
        Directory.CreateDirectory(objectsFolder);

        myRest = new RestClient(baseUri);
        myRest.AddDefaultHeader("x-api-key", myAuthBox.api_key);

        myWeb = new WebClient();
        myAuth = new OAuth2(myAuthBox.api_key, accessUri, oauthUri, code_challenge_method, redirect_uri);

        myJsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        _etsyShop = GetShop(myAuthBox.shop_id);
        if (_etsyShop == null)
        {
            throw new Exception("Unable to retrieve shop information.");
        }
    }
    
    public string OAuth2Get(RestRequest myReq, bool saveResponse = false, bool attemptReAuth = true)
    {
        myReq.AddHeader("authorization", $"Bearer {myAuthBox.access_token}");

        var myResult = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult();
        
        /*if (!myResult.IsSuccessful)
        {
            return string.Empty;
        }*/
        
        string myContents = myResult.Content;

        EtsyObject myResponse = JsonConvert.DeserializeObject<EtsyShop>(myContents, myJsonSerializerSettings);

        if (myResponse.Error != null)
        {
            SaveError(myResult.StatusCode
                      + "\n" + myContents);
            if (myResponse.Error == "invalid_token" && attemptReAuth)
            {
                RequestRefreshToken();
                return OAuth2Get(myReq, saveResponse, false);
            }
        }

        if (saveResponse)
        {
            SaveResponse(myContents);
        }

        return myContents;
    }
    
    public EtsyShop GetShop(long thisShopId)
    {
        RestRequest myReq = new RestRequest("shops/{shop_id}");
        myReq.AddUrlSegment("shop_id", thisShopId);

        string retContents = OAuth2Get(myReq, true);

        EtsyShop myRetEtsyShop = JsonConvert.DeserializeObject<EtsyShop>(retContents, myJsonSerializerSettings);

        return myRetEtsyShop;
    }
    
    private void SaveError(string errorBlock)
    {
        string errorTime = DateTime.Now.ToString("yyyyMMddHmmss");

        Directory.CreateDirectory(errorsFolder);
        System.IO.File.WriteAllText($@"{errorsFolder}\{errorTime}.txt", errorBlock);
    }
    
    private void SaveResponse(string responseBlock)
    {
        string responseTime = DateTime.Now.ToString("yyyyMMddHmmss");

        string responseFormatted =
            JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBlock), Formatting.Indented);

        Directory.CreateDirectory(responsesFolder);
        System.IO.File.WriteAllText($@"{responsesFolder}\{responseTime}.txt", responseFormatted);
    }
    
    public string Ping()
    {
        RestRequest myReq = new RestRequest("openapi-ping", Method.Get);

        RestResponse myResponse = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult();

        return myResponse.Content;
    }
    
    public EtsyListing GetListing(long listingId, bool useCache = true)
    {
        if (useCache && cachedListings.ContainsKey(listingId))
        {
            return cachedListings[listingId];
        }
        
        var myReq = new RestRequest("listings/{listing_id}");
        myReq.AddUrlSegment("listing_id", listingId);

        string retContents = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult().Content;
        SaveResponse(retContents);

        EtsyListing retListing = JsonConvert.DeserializeObject<EtsyListing>(retContents, myJsonSerializerSettings);

        cachedListings[retListing.ListingId] = retListing;
        
        return retListing;
    }
    
    public EtsyReceipt? GetShopReceipt(long receipt_id, bool useCache = true)
    {
        if (useCache && cachedReceipts.ContainsKey(receipt_id))
        {
            return cachedReceipts[receipt_id];
        }
        
        RestRequest myReq = new RestRequest("shops/{shop_id}/receipts/{receipt_id}");
        myReq.AddUrlSegment("shop_id", _etsyShop.ShopId);
        myReq.AddUrlSegment("receipt_id", receipt_id);

        string retContents = OAuth2Get(myReq);
        Results<EtsyReceipt> retObj =
            JsonConvert.DeserializeObject<Results<EtsyReceipt>>(retContents, myJsonSerializerSettings);

        if (retObj != null || retObj.results.Count < 1)
        {
            return null;
        }

        cachedReceipts[retObj.results.First().ReceiptId] = retObj.results.First();
        
        return retObj.results.First();
    }

    public List<EtsyReceipt> GetShopReceipts(bool was_paid = true, bool was_shipped = false, int offset = 0, int limit = 10)
    {
        RestRequest myReq = new RestRequest("shops/{shop_id}/receipts");
        myReq.AddUrlSegment("shop_id", _etsyShop.ShopId);
        myReq.AddQueryParameter("was_paid", was_paid);
        myReq.AddQueryParameter("was_shipped", was_shipped);
        myReq.AddQueryParameter("limit", limit);
        myReq.AddQueryParameter("offset", offset);

        if (cachedReceipts.Values.Count > 0)
        {
            myReq.AddQueryParameter("min_last_modified", 
                (from EtsyReceipt x in cachedReceipts.Values select x.UpdateTimestamp).Max());
        }
        
        string retContents = OAuth2Get(myReq);
        Results<EtsyReceipt> retObj =
            JsonConvert.DeserializeObject<Results<EtsyReceipt>>(retContents, myJsonSerializerSettings);
        
        List<EtsyReceipt> retReceipts = new List<EtsyReceipt>();

        if (retObj.count > 0)
        {
            foreach (EtsyReceipt thisResult in retObj.results)
            {
                thisResult.SetConnection(this);
                
                retReceipts.Add(thisResult);
            }

            if (offset + limit < retObj.count)
            {
                retReceipts.AddRange(GetShopReceipts(was_paid, was_shipped, offset + limit));
            }
        }

        return retReceipts;
    }

    public void RefreshOrderCache()
    {
        int offset = 0;
        int limit = 10;

        while (true)
        {
            var theseReceipts = GetShopReceipts(offset: offset, limit: limit);
                
            foreach (EtsyReceipt thisOrder in theseReceipts)
            {
                if (!cachedReceipts.ContainsKey(thisOrder.ReceiptId) ||
                    thisOrder.UpdateTimestamp > cachedReceipts[thisOrder.ReceiptId].UpdateTimestamp)
                {
                    cachedReceipts[thisOrder.ReceiptId] = thisOrder;
                    cachedReceipts[thisOrder.ReceiptId].SetConnection(this);
                }
                else
                {
                    //stop requesting orders when we've stopped getting updates
                    return;
                }
            }

            if (theseReceipts.Count < limit)
            {
                //return if the most recent group is short -- indicating this is the final set
                break;
            }

            offset += limit;
        }
    }
    
    public List<EtsyListingImage> GetListingImages(long listing_id)
    {
        RestRequest myReq = new RestRequest("listings/{listing_id}/images");
        myReq.AddUrlSegment("listing_id", listing_id);
        string retContents = OAuth2Get(myReq, true);
        Results<EtsyListingImage> retObj =
            JsonConvert.DeserializeObject<Results<EtsyListingImage>>(retContents, myJsonSerializerSettings);
        return new List<EtsyListingImage>(retObj.results.ToList());
    }
    
    public EtsyListingImage GetListingImage(long listing_id, long listing_image_id, bool useCache = true)
    {
        RestRequest myReq = new RestRequest("listings/{listing_id}/images/{listing_image_id}");
        
        myReq.AddUrlSegment("listing_id", listing_id);
        myReq.AddUrlSegment("listing_image_id", listing_image_id);
        
        string retContents = OAuth2Get(myReq, true);
        
        EtsyListingImage retObj =
            JsonConvert.DeserializeObject<EtsyListingImage>(retContents, myJsonSerializerSettings);
        
        return retObj;
    }
    
    public bool RequestRefreshToken()
    {
        AccessToken reqToken = Auth.RefreshAccessToken(myAuthBox.refresh_token);

        if (reqToken.Error != null)
        {
            SaveError($"error: {reqToken.Error} \n " +
                      $"description: {reqToken.ErrorDescription} \n uri: {reqToken.ErrorUri}");
            return false;
        }

        if (myAuthBox.refresh_token.Length > 1 && myAuthBox.access_token.Length > 1)
        {
            myAuthBox.refresh_token = reqToken.refresh_token;
            myAuthBox.access_token = reqToken.access_token;
            myAuthBox.Save();

            return true;
        }

        return false;
    }
    
    public string GetListingImageThumbPath(long listing_id, long listing_image_id, bool useCache = true)
    {
        if (useCache && cachedThumbPaths.ContainsKey(listing_image_id))
        {
            return cachedThumbPaths[listing_image_id];
        }
        
        string cacheUri = $@"{thumbsFolder}\{listing_image_id}";
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(cacheUri));

        if (!File.Exists(cacheUri))
        {
            EtsyListingImage myImage = GetListingImage(listing_id, listing_image_id);
            myWeb.DownloadFile(myImage.Url75X75, cacheUri);
        }

        cachedThumbPaths[listing_image_id] = cacheUri;

        return cachedThumbPaths[listing_image_id];
    }
    
    public string GetListingImageFullPath(long listing_id, long listing_image_id)
    {
        string cacheUri = $@"{fullImagesFolder}\{listing_image_id}";

        return cacheUri;
    }

    public bool DownloadImageToCache(long listing_id, long listing_image_id)
    {
        string cacheUri = GetListingImageFullPath(listing_id, listing_image_id);
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(cacheUri));

        if (!System.IO.File.Exists(cacheUri))
        {
            EtsyListingImage myImage = GetListingImage(listing_id, listing_image_id);
            myWeb.DownloadFile(myImage.UrlFullxfull, cacheUri);
        }

        return System.IO.File.Exists(cacheUri) == true;
    }
    
    public void LoadCachedData()
    {
        if (!File.Exists(listingsFile))
        {
            return;
        }

        if (cachedListings == null)
        {
            cachedListings = new Dictionary<long, EtsyListing>();
        }

        string listingsFileContents = File.ReadAllText(listingsFile);
        cachedListings = JsonConvert.DeserializeObject<Dictionary<long, EtsyListing>>(listingsFileContents);

        foreach (EtsyListing thisListing in cachedListings.Values)
        {
            thisListing.SetConnection(this);
        }
    }
    
    public void SaveCachedData()
    {
        string contentsToWrite = JsonConvert.SerializeObject(cachedListings, Formatting.Indented);
        Directory.CreateDirectory(Path.GetDirectoryName(listingsFile));
        File.WriteAllText(listingsFile, contentsToWrite);
    }
    
    public List<EtsyListing> GetListings(int offset, int limit)
    {
        RestRequest thisReq = new RestRequest(@"shops/{shop_id}/listings", Method.Get);
        thisReq.AddUrlSegment("shop_id", _etsyShop.ShopId);

        thisReq.AddQueryParameter("limit", limit);
        thisReq.AddQueryParameter("offset", offset);
        thisReq.AddQueryParameter("sort_on", "updated");
        thisReq.AddQueryParameter("sort_order", "desc");
        thisReq.AddQueryParameter("includes", "Images");
        
        string retContents = OAuth2Get(thisReq, true);

        return JsonConvert.DeserializeObject<Results<EtsyListing>>(retContents, myJsonSerializerSettings).results;
    }
    
    public void RefreshListingCache()
    {
        if (cachedListings == null)
        {
            cachedListings = new Dictionary<long, EtsyListing>();
        }

        int limit = 10;
        int offset = 0;

        while (true) 
        {
            List<EtsyListing> myResults = GetListings(offset, limit);

            if (myResults is null || myResults.Count < 1)
            {
                break;
            }
            
            foreach (EtsyListing thisListing in myResults)
            {
                if (!cachedListings.ContainsKey(thisListing.ListingId) ||
                    thisListing.LastModifiedTimestamp > cachedListings[thisListing.ListingId].LastModifiedTimestamp)
                {
                    cachedListings[thisListing.ListingId] = thisListing;
                    if (thisListing.Images is null || thisListing.Images.Count < 1)
                    {
                        cachedListings[thisListing.ListingId].Images = GetListingImages(thisListing.ListingId);
                    }
                    cachedListings[thisListing.ListingId].SetConnection(this);
                }
                else
                {
                    goto finished_updating;
                }
            }
            
            if (myResults.Count < limit)
            {
                break;
            }
            
            offset += limit;
        }

        finished_updating:
        SaveCachedData();
    }
    
    public void SaveAuthData()
    {
        myAuthBox.Save();
    }
    
    public IEnumerable<IMarketOrder> Orders
    {
        get
        {
            foreach (IMarketOrder thisOrder in cachedReceipts.Values)
            {
                yield return thisOrder;
            }
        }
    }
    
    public IEnumerable<IMarketListing> Listings
    {
        get
        {
            foreach (IMarketListing thisListing in cachedListings.Values)
            {
                yield return thisListing;
            }
        }
    }
}