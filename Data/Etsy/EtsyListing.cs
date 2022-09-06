using System.ComponentModel;
using Newtonsoft.Json;
using ShopTools.Data.Market;

namespace ShopTools.Data.Etsy;

public class EtsyListing : EtsyObject, IMarketListing
{
    [JsonProperty("listing_id")] public long ListingId { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("shop_id")] public long ShopId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("state")] public string State { get; set; }
    [JsonProperty("creation_timestamp")] public long CreationTimestamp { get; set; }
    [JsonProperty("ending_timestamp")] public long EndingTimestamp { get; set; }
    [JsonProperty("original_creation_timestamp")] public long OriginalCreationTimestamp { get; set; }
    [JsonProperty("last_modified_timestamp")] public long LastModifiedTimestamp { get; set; }
    [JsonProperty("state_timestamp")] public long StateTimestamp { get; set; }
    [JsonProperty("quantity")] public int Quantity { get; set; }
    [JsonProperty("shop_section_id")] public long ShopSectionId { get; set; }
    [JsonProperty("featured_rank")] public long FeaturedRank { get; set; }
    [JsonProperty("url")] public string Url { get; set; }
    [JsonProperty("num_favorers")] public int NumFavorers { get; set; }
    [JsonProperty("non_taxable")] public bool NonTaxable { get; set; }
    [JsonProperty("is_customizable")] public bool IsCustomizable { get; set; }
    [JsonProperty("is_personalizable")] public bool IsPersonalizable { get; set; }
    [JsonProperty("personalization_is_required")] public bool PersonalizationIsRequired { get; set; }
    [JsonProperty("personalization_char_count_max")] public int PersonalizationCharCountMax { get; set; }
    [JsonProperty("personalization_instructions")] public string PersonalizationInstructions { get; set; }
    [JsonProperty("listing_type")] public string ListingType { get; set; }
    [JsonProperty("tags")] public BindingList<string> Tags { get; set; }
    [JsonProperty("materials")] public BindingList<string> Materials { get; set; }
    [JsonProperty("shipping_profile_id")] public long ShippingProfileId { get; set; }
    [JsonProperty("processing_min")] public int ProcessingMin  { get; set; }
    [JsonProperty("processing_max")] public int ProcessingMax { get; set; }
    [JsonProperty("who_made")] public string WhoMade { get; set; }
    [JsonProperty("when_made")] public string WhenMade { get; set; }
    [JsonProperty("is_supply")] public bool IsSupply { get; set; }
    [JsonProperty("item_weight")] public double ItemWeight { get; set; }
    [JsonProperty("item_weight_unit")] public string ItemWeightUnit { get; set; }
    [JsonProperty("item_length")] public double ItemLength { get; set; }
    [JsonProperty("item_width")] public double ItemWidth { get; set; }
    [JsonProperty("item_height")] public double ItemHeight { get; set; }
    [JsonProperty("item_dimensions_unit")] public string ItemDimensionsUnit { get; set; }
    [JsonProperty("is_private")] public bool IsPrivate { get; set; }
    [JsonProperty("style")] public BindingList<string> Style { get; set; }
    [JsonProperty("file_data")] public string FileData { get; set; }
    [JsonProperty("has_variations")] public bool HasVariations { get; set; }
    [JsonProperty("should_auto_renew")] public bool ShouldAutoRenew { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("price")] public EtsyPrice Price { get; set; }
    [JsonProperty("taxonomy_id")] public long TaxonomyId { get; set; }
    [JsonProperty("shipping_profile")] public EtsyShippingProfile ShippingProfile { get; set; }
    [JsonProperty("user")] public EtsyUser User { get; set; }
    [JsonProperty("images")] public List<EtsyListingImage> Images { get; set; }
    [JsonProperty("production_partners")] public BindingList<EtsyProductionPartner> ProductionPartners { get; set; }
    [JsonProperty("skus")] public BindingList<string> Skus { get; set; }
    [JsonProperty("translations")] public BindingList<EtsyTranslation> Translations { get; set; }
    
    [JsonIgnore]
    public EtsyShop EtsyShop  { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
        
        if (Price is not null) { Price.SetConnection(thisConn); }
        if (ShippingProfile is not null) { ShippingProfile.SetConnection(thisConn); }
        if (User is not null) { User.SetConnection(thisConn); }

        if (Images is not null) { foreach (var thisObj in Images) { (thisObj as EtsyListingImage).SetConnection(thisConn); } }
        if (ProductionPartners is not null) {foreach (var thisObj in ProductionPartners) { thisObj.SetConnection(thisConn); }}
        if (Translations is not null) {foreach (var thisObj in Translations) { thisObj.SetConnection(thisConn); }}
    }
    
    [JsonIgnore]
    public string DescriptionFirstLine
    {
        get
        {
            if (Description.Contains('\n'))
            {
                return Description.Substring(0, Math.Min(Description.Length, Description.IndexOf('\n')));
            }
            else
            {
                return Description;
            }
        }
    }
    
    [JsonIgnore]
    public string ImageThumbCachePath
    {
        get
        {
            if (Images != null && Images.Count() > 0)
            {
                return Images.First().ThumbCachePath;
            }
            else
            {
                return string.Empty;
            }
        }    
    }
    
    [JsonIgnore]
    public IEnumerable<IMarketImage> ListingImages
    {
        get
        {
            foreach (IMarketImage thisImage in Images)
            {
                yield return thisImage;
            }
        }
    }
    
    [JsonIgnore]
    public string WebUrl => $"https://www.etsy.com/listing/{ListingId}/";
}
    
public class EtsyListingOffering : EtsyObject
{
    [JsonProperty("offering_id")] public long OfferingId { get; set; }
    [JsonProperty("quantity")] public double Quantity { get; set; }
    [JsonProperty("is_enabled")] public bool IsEnabled { get; set; }
    [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
    [JsonProperty("price")] public EtsyPrice Price { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;
        if (Price is not null) { Price.SetConnection(thisConn); }
    }
}
    
public class EtsyListingProduct : EtsyObject
{
    [JsonProperty("product_id")] public long ProductId { get; set; }
    [JsonProperty("sku")] public string Sku { get; set; }
    [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
    [JsonProperty("offerings")] public List<EtsyListingOffering> Offerings { get; set; }
    [JsonProperty("property_values")] public List<EtsyProperty> PropertyValues { get; set; }
    
    public override void SetConnection(EtsyConnection thisConn)
    {
        MyConnection = thisConn;

        if (Offerings is not null) {foreach (var thisObj in Offerings) { thisObj.SetConnection(thisConn); }}
        if (PropertyValues is not null) {foreach (var thisObj in PropertyValues) { thisObj.SetConnection(thisConn); }}
    }
}
    
public class EtsyListingImage : EtsyObject, IMarketImage
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
    
    [JsonIgnore]
    public string ThumbCachePath
    {
        get
        {
            if (MyConnection is null) { return null; }
            return MyConnection.GetListingImageThumbPath(ListingId, ListingImageId);
        }
    }

    public void DownloadToCache()
    {
        
    }
    
    [JsonIgnore]
    public string CachePath
    {
        get
        {
            if (MyConnection is null) { return null; }
            return MyConnection.GetListingImageFullPath(ListingId, ListingImageId);
        }
    }
}
    