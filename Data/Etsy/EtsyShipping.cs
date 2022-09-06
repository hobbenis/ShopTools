using Newtonsoft.Json;

namespace ShopTools.Data.Etsy;

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
