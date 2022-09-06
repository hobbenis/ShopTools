using Newtonsoft.Json;
using ShopTools.Data.Market;

namespace ShopTools.Data.Etsy;

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

    public string OrderId => ReceiptId.ToString();
    public string CustomerName => Name;
    
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
