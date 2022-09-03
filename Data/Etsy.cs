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

public class EtsyError
{
    [JsonProperty("error")] public string Error;
    [JsonProperty("error_description")] public string ErrorDescription;
    [JsonProperty("error_uri")] public string ErrorUri;
}
    
public abstract class EtsyObject : BoundObject
{  
    [JsonProperty("error")] public string Error;
    [JsonProperty("error_description")] public string ErrorDescription;
    [JsonProperty("error_uri")] public string ErrorUri;
    
    public string Platform => "https://www.etsy.com/";
    
    protected EtsyConnection MyConnection;

    public virtual void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
    }
}
    
public class EtsyProperty : EtsyObject
{
    [JsonProperty("property_id")] public long PropertyId { get => (long)Get(); set => Set(value); }
    [JsonProperty("property_name")] public string PropertyName { get => (string)Get(); set => Set(value); }
    [JsonProperty("scale_id")] public long ScaleId { get => (long)Get(); set => Set(value); }
    [JsonProperty("scale_name")] public string ScaleName { get => (string)Get(); set => Set(value); }
    [JsonProperty("value_ids")] public BindingList<long> ValueIds { get => (BindingList<long>)Get(); set => Set(value); }
    [JsonProperty("values")] public BindingList<string> Values { get => (BindingList<string>)Get(); set => Set(value); }
}
    
public class EtsyPrice : EtsyObject
{
    [JsonProperty("amount")] public int Amount { get => (int)Get(); set => Set(value); }
    [JsonProperty("divisor")] public int Divisor { get => (int)Get(); set => Set(value); }
    [JsonProperty("currency_code")] public string CurrencyCode { get => (string)Get(); set => Set(value); }
}
    
public class EtsyListing : EtsyObject, IMarketListing
{
    [JsonProperty("listing_id")] public long ListingId { get => (long)Get(); set => Set(value); }
    [JsonProperty("user_id")] public long UserId { get => (long)Get(); set => Set(value); }
    [JsonProperty("shop_id")] public long ShopId { get => (long)Get(); set => Set(value); }
    [JsonProperty("title")] public string Title { get => (string)Get(); set => Set(value); }
    [JsonProperty("description")] public string Description { get => (string)Get(); set => Set(value); }
    [JsonProperty("state")] public string State { get => (string)Get(); set => Set(value); }
    [JsonProperty("creation_timestamp")] public long CreationTimestamp { get => (long)Get(); set => Set(value); }
    [JsonProperty("ending_timestamp")] public long EndingTimestamp { get => (long)Get(); set => Set(value); }
    [JsonProperty("original_creation_timestamp")] public long OriginalCreationTimestamp { get => (long)Get(); set => Set(value); }
    [JsonProperty("last_modified_timestamp")] public long LastModifiedTimestamp { get => (long)Get(); set => Set(value); }
    [JsonProperty("state_timestamp")] public long StateTimestamp { get => (long)Get(); set => Set(value); }
    [JsonProperty("quantity")] public int Quantity { get => (int)Get(); set => Set(value); }
    [JsonProperty("shop_section_id")] public long ShopSectionId { get => (long)Get(); set => Set(value); }
    [JsonProperty("featured_rank")] public long FeaturedRank { get => (long)Get(); set => Set(value); }
    [JsonProperty("url")] public string Url { get => (string)Get(); set => Set(value); }
    [JsonProperty("num_favorers")] public int NumFavorers { get => (int)Get(); set => Set(value); }
    [JsonProperty("non_taxable")] public bool NonTaxable { get => (bool)Get(); set => Set(value); }
    [JsonProperty("is_customizable")] public bool IsCustomizable { get => (bool)Get(); set => Set(value); }
    [JsonProperty("is_personalizable")] public bool IsPersonalizable { get => (bool)Get(); set => Set(value); }
    [JsonProperty("personalization_is_required")] public bool PersonalizationIsRequired { get => (bool)Get(); set => Set(value); }
    [JsonProperty("personalization_char_count_max")] public int PersonalizationCharCountMax { get => (int)Get(); set => Set(value); }
    [JsonProperty("personalization_instructions")] public string PersonalizationInstructions { get => (string)Get(); set => Set(value); }
    [JsonProperty("listing_type")] public string ListingType { get => (string)Get(); set => Set(value); }
    [JsonProperty("tags")] public BindingList<string> Tags { get => (BindingList<string>)Get(); set => Set(value); }
    [JsonProperty("materials")] public BindingList<string> Materials { get => (BindingList<string>)Get(); set => Set(value); }
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get => (long)Get(); set => Set(value); }
    [JsonProperty("processing_min")] public int ProcessingMin { get => (int)Get(); set => Set(value); }
    [JsonProperty("processing_max")] public int ProcessingMax { get => (int)Get(); set => Set(value); }
    [JsonProperty("who_made")] public string WhoMade { get => (string)Get(); set => Set(value); }
    [JsonProperty("when_made")] public string WhenMade { get => (string)Get(); set => Set(value); }
    [JsonProperty("is_supply")] public bool IsSupply { get => (bool)Get(); set => Set(value); }
    [JsonProperty("item_weight")] public double ItemWeight { get => (double)Get(); set => Set(value); }
    [JsonProperty("item_weight_unit")] public string ItemWeightUnit { get => (string)Get(); set => Set(value); }
    [JsonProperty("item_length")] public double ItemLength { get => (double)Get(); set => Set(value); }
    [JsonProperty("item_width")] public double ItemWidth { get => (double)Get(); set => Set(value); }
    [JsonProperty("item_height")] public double ItemHeight { get => (double)Get(); set => Set(value); }
    [JsonProperty("item_dimensions_unit")] public string ItemDimensionsUnit { get => (string)Get(); set => Set(value); }
    [JsonProperty("is_private")] public bool IsPrivate { get => (bool)Get(); set => Set(value); }
    [JsonProperty("style")] public BindingList<string> Style { get => (BindingList<string>)Get(); set => Set(value); }
    [JsonProperty("file_data")] public string FileData { get => (string)Get(); set => Set(value); }
    [JsonProperty("has_variations")] public bool HasVariations { get => (bool)Get(); set => Set(value); }
    [JsonProperty("should_auto_renew")] public bool ShouldAutoRenew { get => (bool)Get(); set => Set(value); }
    [JsonProperty("language")] public string Language { get => (string)Get(); set => Set(value); }
    [JsonProperty("price")] public EtsyPrice Price { get => (EtsyPrice)Get(); set => Set(value); }
    [JsonProperty("taxonomy_id")] public long TaxonomyId { get => (long)Get(); set => Set(value); }
    [JsonProperty("shipping_profile")] public EtsyShippingProfile ShippingProfile { get => (EtsyShippingProfile)Get(); set => Set(value); }
    [JsonProperty("user")] public EtsyUser User { get => (EtsyUser)Get(); set => Set(value); }
    [JsonProperty("images")] public BindingList<EtsyListingImage> Images{ get => (BindingList<EtsyListingImage>)Get(); set => Set(value); }
    [JsonProperty("production_partners")] public BindingList<EtsyProductionPartner> ProductionPartners{ get => (BindingList<EtsyProductionPartner>)Get(); set => Set(value); }
    [JsonProperty("skus")] public BindingList<string> Skus{ get => (BindingList<string>)Get(); set => Set(value); }
    [JsonProperty("translations")] public BindingList<EtsyTranslation> Translations{ get => (BindingList<EtsyTranslation>)Get(); set => Set(value); }
    
    public EtsyShop EtsyShop { get => (EtsyShop)Get(); set => Set(value); }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
        
        if (Price is not null) { Price.SetConnection(thisConn); }
        if (ShippingProfile is not null) { ShippingProfile.SetConnection(thisConn); }
        if (User is not null) { User.SetConnection(thisConn); }

        if (Images is not null) { foreach (var thisObj in Images) { thisObj.SetConnection(thisConn); } }
        if (ProductionPartners is not null) {foreach (var thisObj in ProductionPartners) { thisObj.SetConnection(thisConn); }}
        if (Translations is not null) {foreach (var thisObj in Translations) { thisObj.SetConnection(thisConn); }}
    }

    public string DescriptionFirstLine
    {
        get
        {
            return Description.Substring(0, Math.Min(Description.Length, Description.IndexOf('\n')));
        }
    }

    public string ImageThumbCachePath
    {
        get
        {
            if (Images != null && Images.Count > 0)
            {
                return MyConnection.GetListingImageThumbPath(ListingId, Images.First().ListingImageId);
            }
            else
            {
                return string.Empty;
            }
        }    
    }
        
    public string WebUrl => $"https://www.etsy.com/listing/{ListingId}/";
}
    
public class EtsyListingOffering : EtsyObject
{
    [JsonProperty("offering_id")] public long OfferingId { get => (long)Get(); set => Set(value); }
    [JsonProperty("quantity")] public double Quantity { get => (double)Get(); set => Set(value); }
    [JsonProperty("is_enabled")] public bool IsEnabled { get => (bool)Get(); set => Set(value); }
    [JsonProperty("is_deleted")] public bool IsDeleted { get => (bool)Get(); set => Set(value); }
    [JsonProperty("price")] public EtsyPrice Price { get => (EtsyPrice)Get(); set => Set(value); }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
        if (Price is not null) { Price.SetConnection(thisConn); }
    }
}
    
public class EtsyListingProduct : EtsyObject
{
    [JsonProperty("product_id")] public long ProductId { get => (long)Get(); set => Set(value); }
    [JsonProperty("sku")] public string Sku { get => (string)Get(); set => Set(value); }
    [JsonProperty("is_deleted")] public bool IsDeleted { get => (bool)Get(); set => Set(value); }
    [JsonProperty("offerings")] public List<EtsyListingOffering> Offerings { get => (List<EtsyListingOffering>)Get(); set => Set(value); }
    [JsonProperty("property_values")] public List<EtsyProperty> PropertyValues { get => (List<EtsyProperty>)Get(); set => Set(value); }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (Offerings is not null) {foreach (var thisObj in Offerings) { thisObj.SetConnection(thisConn); }}
        if (PropertyValues is not null) {foreach (var thisObj in PropertyValues) { thisObj.SetConnection(thisConn); }}
    }
}
    
public class EtsyListingImage : EtsyObject
{
    [JsonProperty("listing_id")] public long ListingId { get; set; }
    [JsonProperty("listing_image_id")] public long ListingImageId { get; set; }
    [JsonProperty("hex_code")] public string HexCode { get; set; }
    [JsonProperty("red")] public int Red { get; set; }
    [JsonProperty("green")] public int Green { get; set; }
    [JsonProperty("blue")] public int Blue { get; set; }
    [JsonProperty("hue")] public int Hue { get; set; }
    [JsonProperty("saturation")] public int Saturation { get; set; }
    [JsonProperty("brightness")] public int Brightness { get; set; }
    [JsonProperty("is_black_and_white")] public bool IsBlackAndWhite { get; set; }
    [JsonProperty("creation_tsz")] public int CreationTsz { get; set; }
    [JsonProperty("rank")] public int Rank { get; set; }
    [JsonProperty("url_75x75")] public string Url75X75 { get; set; }
    [JsonProperty("url_170x135")] public string Url170X135 { get; set; }
    [JsonProperty("url_570xN")] public string Url570XN { get; set; }
    [JsonProperty("url_fullxfull")] public string UrlFullxfull { get; set; }
    [JsonProperty("full_height")] public int FullHeight { get; set; }
    [JsonProperty("full_width")] public int FullWidth { get; set; }

    public string ImageThumbPath
    {
        get
        {
            if (MyConnection is null) { return null; }
            return MyConnection.GetListingImageThumbPath(ListingId, ListingImageId);
        }
    }
}
    
public class EtsyShippingProfile : EtsyObject
{
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("min_processing_days")] public int MinProcessingDays { get; set; }
    [JsonProperty("max_processing_days")] public int MaxProcessingDays { get; set; }
    [JsonProperty("processing_days_display_label")] public string ProcessingDaysDisplayLabel { get; set; }
    [JsonProperty("origin_country_iso")] public string OriginCountryIso { get; set; }
    [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
    [JsonProperty("shipping_profile_destinations")] public List<EtsyShippingProfileDestination> ShippingProfileDestinations { get; set; }
    [JsonProperty("shipping_profile_upgrades")] public List<EtsyShippingProfileUpgrade> ShippingProfileUpgrades { get; set; }
    [JsonProperty("origin_postal_code")] public string OriginPostalCode { get; set; }
    [JsonProperty("profile_type")] public string ProfileType { get; set; }
    [JsonProperty("domestic_handling_fee")] public double DomesticHandlingFee { get; set; }
    [JsonProperty("international_handling_fee")] public double InternationalHandlingFee { get; set; }

    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (ShippingProfileDestinations is not null)
        {
            foreach (var thisObj in ShippingProfileDestinations) { thisObj.SetConnection(thisConn); }
        }

        if (ShippingProfileUpgrades is not null)
        {
            foreach (var thisObj in ShippingProfileUpgrades) { thisObj.SetConnection(thisConn); }    
        }
    }
}

public class EtsyShippingProfileDestination : EtsyObject
{
    [JsonProperty("shipping_profile_destination_id")] public long ShippingProfileDestinationId { get; set; }
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get; set; }
    [JsonProperty("origin_country_iso")] public string OriginCountryIso { get; set; }
    [JsonProperty("destination_country_iso")] public string DestinationCountryIso { get; set; }
    [JsonProperty("destination_region")] public string DestinationRegion { get; set; }
    [JsonProperty("primary_cost")] public EtsyPrice PrimaryCost { get; set; }
    [JsonProperty("secondary_cost")] public EtsyPrice SecondaryCost { get; set; }
    [JsonProperty("shipping_carrier_id")] public long ShippingCarrierId { get; set; }
    [JsonProperty("mail_class")] public string MailClass { get; set; }
    [JsonProperty("min_delivery_days")] public int MinDeliveryDays { get; set; }
    [JsonProperty("max_delivery_days")] public int MaxDeliveryDays { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (PrimaryCost is not null) { PrimaryCost.SetConnection(thisConn); }
        if (SecondaryCost is not null) { SecondaryCost.SetConnection(thisConn); }
    }
}

public class EtsyShippingProfileUpgrade : EtsyObject
{
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get; set; }
    [JsonProperty("upgrade_id")] public long UpgradeId { get; set; }
    [JsonProperty("upgrade_name")] public string UpgradeName { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("rank")] public int Rank { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("price")] public EtsyPrice Price { get; set; }
    [JsonProperty("secondary_price")] public EtsyPrice SecondaryPrice { get; set; }
    [JsonProperty("shipping_carrier_id")] public long ShippingCarrierId { get; set; }
    [JsonProperty("mail_class")] public string MailClass { get; set; }
    [JsonProperty("min_delivery_days")] public int MinDeliveryDays { get; set; }
    [JsonProperty("max_delivery_days")] public int MaxDeliveryDays { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (Price is not null) { Price.SetConnection(thisConn); }
        if (SecondaryPrice is not null) {SecondaryPrice.SetConnection(thisConn);}
    }
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
    
public class EtsyReceipt : EtsyObject, IMarketOrder
{
    [JsonProperty("receipt_id")] public long ReceiptId { get; set; }
    [JsonProperty("receipt_type")] public int ReceiptType { get; set; }
    [JsonProperty("seller_user_id")] public long SellerUserId { get; set; }
    [JsonProperty("seller_email")] public string SellerEmail { get; set; }
    [JsonProperty("buyer_user_id")] public long BuyerUserId { get; set; }
    [JsonProperty("buyer_email")] public string BuyerEmail { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("first_line")] public string FirstLine { get; set; }
    [JsonProperty("second_line")] public string SecondLine { get; set; }
    [JsonProperty("city")] public string City { get; set; }
    [JsonProperty("state")] public string State { get; set; }
    [JsonProperty("zip")] public string Zip { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }
    [JsonProperty("country_iso")] public string CountryIso { get; set; }
    [JsonProperty("payment_method")] public string PaymentMethod { get; set; }
    [JsonProperty("payment_email")] public string PaymentEmail { get; set; }
    [JsonProperty("message_from_seller")] public string MessageFromSeller { get; set; }
    [JsonProperty("message_from_buyer")] public string MessageFromBuyer { get; set; }
    [JsonProperty("message_from_payment")] public string MessageFromPayment { get; set; }
    [JsonProperty("is_paid")] public bool IsPaid { get; set; }
    [JsonProperty("is_shipped")] public bool IsShipped { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("update_timestamp")] public int UpdateTimestamp { get; set; }
    [JsonProperty("is_gift")] public bool IsGift { get; set; }
    [JsonProperty("gift_message")] public string GiftMessage { get; set; }
    [JsonProperty("grandtotal")] public EtsyPrice Grandtotal { get; set; }
    [JsonProperty("subtotal")] public EtsyPrice Subtotal { get; set; }
    [JsonProperty("total_price")] public EtsyPrice TotalPrice { get; set; }
    [JsonProperty("total_shipping_cost")] public EtsyPrice TotalShippingCost { get; set; }
    [JsonProperty("total_tax_cost")] public EtsyPrice TotalTaxCost { get; set; }
    [JsonProperty("total_vat_cost")] public EtsyPrice TotalVatCost { get; set; }
    [JsonProperty("discount_amt")] public EtsyPrice DiscountAmt { get; set; }
    [JsonProperty("gift_wrap_price")] public EtsyPrice GiftWrapPrice { get; set; }
    [JsonProperty("shipments")] public List<EtsyShipment> Shipments { get; set; }
    [JsonProperty("transactions")] public List<EtsyTransaction> Transactions { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (Grandtotal is not null) {Grandtotal.SetConnection(thisConn);}
        if (Subtotal is not null) {Subtotal.SetConnection(thisConn);}
        if (TotalPrice is not null) {TotalPrice.SetConnection(thisConn);}
        if (TotalShippingCost is not null) {TotalShippingCost.SetConnection(thisConn);}
        if (TotalTaxCost is not null) {TotalTaxCost.SetConnection(thisConn);}
        if (TotalVatCost is not null) {TotalVatCost.SetConnection(thisConn);}
        if (DiscountAmt is not null) {DiscountAmt.SetConnection(thisConn);}
        if (GiftWrapPrice is not null) {GiftWrapPrice.SetConnection(thisConn);}

        if (Shipments is not null)
        {
            foreach (var thisObj in Shipments) { thisObj.SetConnection(thisConn); }    
        }

        if (Transactions is not null)
        {
            foreach (var thisObj in Transactions) { thisObj.SetConnection(thisConn); }
        }
    }
    
    public IEnumerable<IMarketOrderLine> OrderLines => new List<IMarketOrderLine>(Transactions);
    public IEnumerable<IMarketOrderShipment> OrderShipments => new List<IMarketOrderShipment>(Shipments);
    public string WebUrl => $"https://www.etsy.com/your/orders/?order_id={ReceiptId}";
    
    public DateTime EarliestExpectedShipDate
    {
        get
        {
            return DateTimeOffset.FromUnixTimeSeconds(
                (from x in Transactions select x.ExpectedShipDate).Max()).ToLocalTime().DateTime;
        }
    }
}

public class EtsyShipment : EtsyObject, IMarketOrderShipment
{
    [JsonProperty("receipt_shipping_id")] public long ReceiptShippingId { get; set; }
    [JsonProperty("shipment_notification_timestamp")] public int ShipmentNotificationTimestamp { get; set; }
    [JsonProperty("carrier_name")] public string Carrier { get; set; }
    [JsonProperty("tracking_code")] public string TrackingId { get; set; }
}

public class EtsyTransaction : EtsyObject, IMarketOrderLine
{
    [JsonProperty("transaction_id")] public long TransactionId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("seller_user_id")] public long SellerUserId { get; set; }
    [JsonProperty("buyer_user_id")] public long BuyerUserId { get; set; }
    [JsonProperty("create_timestamp")] public int CreateTimestamp { get; set; }
    [JsonProperty("paid_timestamp")] public int PaidTimestamp { get; set; }
    [JsonProperty("shipped_timestamp")] public int ShippedTimestamp { get; set; }
    [JsonProperty("quantity")] public double Quantity { get; set; }
    [JsonProperty("listing_image_id")] public long ListingImageId { get; set; }
    [JsonProperty("receipt_id")] public long ReceiptId { get; set; }
    [JsonProperty("is_digital")] public bool IsDigital { get; set; }
    [JsonProperty("file_data")] public string FileData { get; set; }
    [JsonProperty("listing_id")] public long ListingId { get; set; }
    [JsonProperty("transaction_type")] public string TransactionType { get; set; }
    [JsonProperty("product_id")] public long ProductId { get; set; }
    [JsonProperty("sku")] public string Sku { get; set; }
    [JsonProperty("price")] public EtsyPrice Price { get; set; }
    [JsonProperty("shipping_cost")] public EtsyPrice ShippingCost { get; set; }
    [JsonProperty("variations")] public List<EtsyVariation> Variations { get; set; }
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get; set; }
    [JsonProperty("min_processing_days")] public int MinProcessingDays { get; set; }
    [JsonProperty("max_processing_days")] public int MaxProcessingDays { get; set; }
    [JsonProperty("shipping_method")] public string ShippingMethod { get; set; }
    [JsonProperty("shipping_upgrade")] public string ShippingUpgrade { get; set; }
    [JsonProperty("expected_ship_date")] public int ExpectedShipDate { get; set; }
    [JsonProperty("buyer_receipt_message")] public string BuyerReceiptMessage { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
        
        if (Price is not null) {Price.SetConnection(thisConn);}
        if (Price is not null) {ShippingCost.SetConnection(thisConn);}

        if (Variations is not null)
        {
            foreach (var thisObj in Variations) { thisObj.SetConnection(thisConn); }    
        }
    }
    
    public DateTime CreatedDate =>
        DateTimeOffset.FromUnixTimeSeconds(CreateTimestamp).ToLocalTime().DateTime;

    public string DescriptionFirstLine
    {
        get { return Description.Substring(0, Math.Min(Description.Length, Description.IndexOf('\n'))); }
    }
    
    public string Variation => string.Join(", ", from v in Variations select v.FormattedValue);
    public DateTime ExpectedShipDateTime => DateTimeOffset.FromUnixTimeSeconds(ExpectedShipDate).ToLocalTime().DateTime;
    public string PlatformListingId => ListingId.ToString();
    public string BuyerMessage => BuyerReceiptMessage;

    public IMarketOrder PlatformOrder
    {
        get
        {
            if (MyConnection is null)
            {
                return null;
            }

            return MyConnection.GetShopReceipt(ReceiptId) as IMarketOrder;
        }
    }

    public IMarketListing PlatformListing
    {
        get
        {
            if (MyConnection is null)
            {
                return null;
            }

            return MyConnection.GetListing(ListingId) as IMarketListing;
        }
    }

    public string ImageThumbCachePath => MyConnection.GetListingImageThumbPath(ListingId, ListingImageId); 
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

    public long shopId
    {
        get { return myAuthBox.shop_id; }
        set { myAuthBox.shop_id = value; }
    }

    public OAuth2 myAuth;
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
    }
    
    public string OAuth2Get(RestRequest myReq, bool saveResponse = false, bool attemptReAuth = true)
    {
        myReq.AddHeader("authorization", $"Bearer {myAuthBox.access_token}");

        var myResult = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult();

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

    public List<EtsyReceipt> GetShopReceipts(bool was_paid = true, bool was_shipped = false, int offset = 0)
    {
        int limit = 25;
        RestRequest myReq = new RestRequest("shops/{shop_id}/receipts");
        myReq.AddUrlSegment("shop_id", _etsyShop.ShopId);
        myReq.AddQueryParameter("was_paid", was_paid);
        myReq.AddQueryParameter("was_shipped", was_shipped);
        myReq.AddQueryParameter("limit", 25);
        myReq.AddQueryParameter("offset", offset);

        string retContents = OAuth2Get(myReq);
        Results<EtsyReceipt> retObj =
            JsonConvert.DeserializeObject<Results<EtsyReceipt>>(retContents, myJsonSerializerSettings);
        
        List<EtsyReceipt> retRecpts = new List<EtsyReceipt>();

        if (retObj.count > 0)
        {
            foreach (EtsyReceipt thisResult in retObj.results)
            {
                thisResult.SetConnection(this);
                
                retRecpts.Add(thisResult);
            }

            if (offset + limit < retObj.count)
            {
                retRecpts.AddRange(GetShopReceipts(was_paid, was_shipped, offset + limit));
            }
        }

        return retRecpts;
    }
    
    public BindingList<EtsyListingImage> GetListingImages(long listing_id)
    {
        RestRequest myReq = new RestRequest("listings/{listing_id}/images");
        myReq.AddUrlSegment("listing_id", listing_id);
        string retContents = OAuth2Get(myReq, true);
        Results<EtsyListingImage> retObj =
            JsonConvert.DeserializeObject<Results<EtsyListingImage>>(retContents, myJsonSerializerSettings);
        return new BindingList<EtsyListingImage>(retObj.results.ToList());
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
        AccessToken reqToken = myAuth.RefreshAccessToken(myAuthBox.refresh_token);

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

        Directory.CreateDirectory(thumbsFolder);
        string cacheUri = $@"{thumbsFolder}\{listing_image_id}";

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

        if (!System.IO.File.Exists(cacheUri))
        {
            EtsyListingImage myImage = GetListingImage(listing_id, listing_image_id);
            myWeb.DownloadFile(myImage.UrlFullxfull, cacheUri);
        }

        return cacheUri;
    }
    
    public void LoadCachedListings()
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

    public void SaveCachedListings()
    {
        string contentsToWrite = JsonConvert.SerializeObject(cachedListings, Formatting.Indented);
        Directory.CreateDirectory(Path.GetDirectoryName(listingsFile));
        File.WriteAllText(listingsFile, contentsToWrite);
    }

    public Results<EtsyListing> GetShopListings(int offset, int limit)
    {
        RestRequest thisReq = new RestRequest(@"shops/{shop_id}/listings", Method.Get);
        thisReq.AddUrlSegment("shop_id", _etsyShop.ShopId);

        thisReq.AddQueryParameter("limit", limit);
        thisReq.AddQueryParameter("offset", offset);
        thisReq.AddQueryParameter("sort_on", "updated");

        string retContents = OAuth2Get(thisReq, true);

        return JsonConvert.DeserializeObject<Results<EtsyListing>>(retContents, myJsonSerializerSettings);
    }

    public void RefreshCachedListings()
    {
        if (cachedListings == null)
        {
            cachedListings = new Dictionary<long, EtsyListing>();
        }

        int limit = 25;
        Results<EtsyListing> myResults = GetShopListings(0, limit);
        int total = myResults.count;

        for (int offset = limit; offset <= total; offset = offset + limit)
        {
            foreach (EtsyListing thisListing in myResults.results)
            {
                if (cachedListings.ContainsKey(thisListing.ListingId))
                {
                    if (thisListing.LastModifiedTimestamp >
                        cachedListings[thisListing.ListingId].LastModifiedTimestamp)
                    {
                        cachedListings[thisListing.ListingId] = thisListing;
                        cachedListings[thisListing.ListingId].Images = GetListingImages(thisListing.ListingId);
                    }
                    else
                    {
                        goto finished_updating;
                    }
                }
                else
                {
                    cachedListings[thisListing.ListingId] = thisListing;
                    cachedListings[thisListing.ListingId].Images = GetListingImages(thisListing.ListingId);
                    cachedListings[thisListing.ListingId].SetConnection(this);
                }
            }

            myResults = GetShopListings(offset, limit);
        }

        finished_updating:
        SaveCachedListings();
    }
    
    public void SetTokens(string thisAccessToken, string thisRefreshToken)
    {
        myAuthBox.access_token = thisAccessToken;
        myAuthBox.refresh_token = thisRefreshToken;
    }

    public void SaveAuth()
    {
        myAuthBox.Save();
    }
    
    public IEnumerable<IMarketOrder> OpenOrders
    {
        get
        {
            foreach (IMarketOrder thisOrder in GetShopReceipts(true, false))
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