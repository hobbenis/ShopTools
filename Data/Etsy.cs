//Neither me nor this project have any affiliation with Etsy.
//This class/namespace/etc is simply named as such because it accesses Etsy's API
//You will need to provide your own API key if you wish to use this.
//As such, you should carefully review this and make sure it will do what you need.

using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RestSharp;
using ShopTools.Common;

namespace ShopTools.Etsy
{
    /*address_r	Read a member's shipping addresses.
    address_w	Update and delete a member's shipping address.
    billing_r	Read a member's Etsy bill charges and payments.
    cart_r	Read the contents of a member’s cart.
    cart_w	Add and remove listings from a member's cart.
    email_r	Read a member's email address
    favorites_r	View a member's favorite listings and users.
    favorites_w	Add to and remove from a member's favorite listings and users.
    feedback_r	View all details of a member's feedback (including purchase history.)
    listings_d	Delete a member's listings.
    listings_r	Read a member's inactive and expired (i.e., non-public) listings.
    listings_w	Create and edit a member's listings.
    profile_r	Read a member's private profile information.
    profile_w	Update a member's private profile information.
    recommend_r	View a member's recommended listings.
    recommend_w	Remove a member's recommended listings.
    shops_r	See a member's shop description, messages and sections, even if not (yet) public.
    shops_w	Update a member's shop description, messages and sections.
    transactions_r	Read a member's purchase and sales data. This applies to buyers as well as sellers.
    transactions_w	Update a member's sales data.*/

    public class ProductionSummaryLine
    {
        private EConnection myConn;
        
        public long listing_image_id { get; set; }
        public string sku { get; set; }
        public double qty { get; set; }
        public long listing_id { get; set; }
        public string description { get; set; }
        public string variation_str { get; set; }
        public int earliest_ship_date { get; set; }
        
        public string description_fl => 
            description.Substring(0, Math.Min(description.IndexOf("\n"), description.Length));

        public DateTime earliest_ship_datetime => 
            DateTimeOffset.FromUnixTimeSeconds(earliest_ship_date).ToLocalTime().DateTime;
        
        public string listing_image_thumb_path
        {
            get
            {
                if (myConn != null) { return myConn.GetListingImageThumbPath(listing_id, listing_image_id); }
                else { return null; }
            }
        }
        
        public ProductionSummaryLine(EConnection thisConn)
        {
            myConn = thisConn;
        }
    }
    
    public class ProductionSummary
    {
        private EConnection myConn;
        private List<Receipt> myReceipts;

        public DateTime cutoff_date;
        
        public ProductionSummary(EConnection thisConn, DateTime thisCutOffDate)
        {
            myConn = thisConn;
            cutoff_date = thisCutOffDate;
        }

        public void RefreshReceipts()
        {
            myReceipts = myConn.GetShopReceipts();
        }
        
        public List<Receipt> OpenReceipts()
        {
            if (myReceipts is null)
            {
                RefreshReceipts();
            }
            
            return myReceipts;
        }
        
        public List<Transaction> OpenTransactions()
        {
            List<Transaction> retTransactions = new List<Transaction>();
            
            foreach (Receipt thisReceipt in OpenReceipts())
            {
                foreach (Transaction thisTrans in thisReceipt.transactions)
                {
                    if (thisTrans.expected_ship_datetime > cutoff_date) { continue; }
                    
                    Transaction newTrans = thisTrans;
                    newTrans.SetConnection(myConn);
                    newTrans.buyer_receipt_message = thisReceipt.message_from_buyer;
                    retTransactions.Add(newTrans);
                }
            }
            
            return retTransactions;
        }
        
        public List<ProductionSummaryLine> OpenTransactionSummaryByListing()
        {
            List<ProductionSummaryLine> retSummary = new List<ProductionSummaryLine>();

            foreach (Transaction thisTrans in OpenTransactions())
            {
                foreach (ProductionSummaryLine thisLine in retSummary)
                {
                    if (thisLine.listing_id.Equals(thisTrans.listing_id) &&
                        thisLine.variation_str.Equals(thisTrans.variation_str))
                    {
                        thisLine.qty += thisTrans.quantity;
                        thisLine.earliest_ship_date =
                            Math.Min(thisLine.earliest_ship_date, thisTrans.expected_ship_date);
                        
                        goto nextReceipt;
                    }
                }
                
                retSummary.Add(new ProductionSummaryLine(myConn)
                {
                    listing_id = thisTrans.listing_id,
                    description = thisTrans.description,
                    earliest_ship_date = thisTrans.expected_ship_date,
                    listing_image_id = thisTrans.listing_image_id,
                    sku = thisTrans.sku,
                    qty = thisTrans.quantity,
                    variation_str = thisTrans.variation_str
                });
                
                nextReceipt:
                continue;
            }

            return retSummary;
        }
    }
    
    public class EtsyError
    {
        public string error;
        public string error_description;
        public string error_uri;
    }
    
    public class EtsyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string error;
        public string error_description;
        public string error_uri;
        
        private Dictionary<string, object?> myFields;

        protected EConnection myConn;

        public void SetConnection(EConnection thisConn)
        {
            myConn = thisConn;
        }
        
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
    
    public class Property : EtsyObject
    {
        public long property_id { get => (long)Get(); set => Set(value); }
        public string property_name { get => (string)Get(); set => Set(value); }
        public long scale_id { get => (long)Get(); set => Set(value); }
        public string scale_name { get => (string)Get(); set => Set(value); }
        public BindingList<long> value_ids { get => (BindingList<long>)Get(); set => Set(value); }
        public BindingList<string> values { get => (BindingList<string>)Get(); set => Set(value); }
    }
    
    public class Price : EtsyObject
    {
        public int amount { get => (int)Get(); set => Set(value); }
        public int divisor { get => (int)Get(); set => Set(value); }
        public string currency_code { get => (string)Get(); set => Set(value); }
    }
    
    public class Listing : EtsyObject
    {
        public long listing_id { get => (long)Get(); set => Set(value); }
        public long user_id { get => (long)Get(); set => Set(value); }
        public long shop_id { get => (long)Get(); set => Set(value); }
        public string title { get => (string)Get(); set => Set(value); }
        public string description { get => (string)Get(); set => Set(value); }
        public string state { get => (string)Get(); set => Set(value); }
        public long creation_timestamp { get => (long)Get(); set => Set(value); }
        public long ending_timestamp { get => (long)Get(); set => Set(value); }
        public long original_creation_timestamp { get => (long)Get(); set => Set(value); }
        public long last_modified_timestamp { get => (long)Get(); set => Set(value); }
        public long state_timestamp { get => (long)Get(); set => Set(value); }
        public int quantity { get => (int)Get(); set => Set(value); }
        public long shop_section_id { get => (long)Get(); set => Set(value); }
        public long featured_rank { get => (long)Get(); set => Set(value); }
        public string url { get => (string)Get(); set => Set(value); }
        public int num_favorers { get => (int)Get(); set => Set(value); }
        public bool non_taxable { get => (bool)Get(); set => Set(value); }
        public bool is_customizable { get => (bool)Get(); set => Set(value); }
        public bool is_personalizable { get => (bool)Get(); set => Set(value); }
        public bool personalization_is_required { get => (bool)Get(); set => Set(value); }
        public int personalization_char_count_max { get => (int)Get(); set => Set(value); }
        public string personalization_instructions { get => (string)Get(); set => Set(value); }
        public string listing_type { get => (string)Get(); set => Set(value); }
        public BindingList<string> tags { get => (BindingList<string>)Get(); set => Set(value); }
        public BindingList<string> materials { get => (BindingList<string>)Get(); set => Set(value); }
        public long shipping_profile_id { get => (long)Get(); set => Set(value); }
        public int processing_min { get => (int)Get(); set => Set(value); }
        public int processing_max { get => (int)Get(); set => Set(value); }
        public string who_made { get => (string)Get(); set => Set(value); }
        public string when_made { get => (string)Get(); set => Set(value); }
        public bool is_supply { get => (bool)Get(); set => Set(value); }
        public double item_weight { get => (double)Get(); set => Set(value); }
        public string item_weight_unit { get => (string)Get(); set => Set(value); }
        public double item_length { get => (double)Get(); set => Set(value); }
        public double item_width { get => (double)Get(); set => Set(value); }
        public double item_height { get => (double)Get(); set => Set(value); }
        public string item_dimensions_unit { get => (string)Get(); set => Set(value); }
        public bool is_private { get => (bool)Get(); set => Set(value); }
        public BindingList<string> style { get => (BindingList<string>)Get(); set => Set(value); }
        public string file_data { get => (string)Get(); set => Set(value); }
        public bool has_variations { get => (bool)Get(); set => Set(value); }
        public bool should_auto_renew { get => (bool)Get(); set => Set(value); }
        public string language { get => (string)Get(); set => Set(value); }
        public Price price { get => (Price)Get(); set => Set(value); }
        public long taxonomy_id { get => (long)Get(); set => Set(value); }
        public ShippingProfile shipping_profile { get => (ShippingProfile)Get(); set => Set(value); }
        public User user { get => (User)Get(); set => Set(value); }
        public Shop shop { get => (Shop)Get(); set => Set(value); }
        public BindingList<ListingImage> images{ get => (BindingList<ListingImage>)Get(); set => Set(value); }
        public BindingList<ProductionPartner> production_partners{ get => (BindingList<ProductionPartner>)Get(); set => Set(value); }
        public BindingList<string> skus{ get => (BindingList<string>)Get(); set => Set(value); }
        public BindingList<Translation> translations{ get => (BindingList<Translation>)Get(); set => Set(value); }

        public string first_image_thumb_cache
        {
            get
            {
                if (images != null && images.Count > 0)
                {
                    return myConn.GetListingImageThumbPath(listing_id, images.First().listing_image_id);
                }
                else
                {
                    return string.Empty;
                }
            }    
        }
        
        public string web_url => $"https://www.etsy.com/listing/{listing_id}/";
    }
    
    public class ListingOffering : EtsyObject
    {
        public long offering_id { get => (long)Get(); set => Set(value); }
        public double quantity { get => (double)Get(); set => Set(value); }
        public bool is_enabled { get => (bool)Get(); set => Set(value); }
        public bool is_deleted { get => (bool)Get(); set => Set(value); }
        public Price price { get => (Price)Get(); set => Set(value); }
    }
    
    public class ListingProduct : EtsyObject
    {
        public long product_id { get => (long)Get(); set => Set(value); }
        public string sku { get => (string)Get(); set => Set(value); }
        public bool is_deleted { get => (bool)Get(); set => Set(value); }
        public List<ListingOffering> offerings { get => (List<ListingOffering>)Get(); set => Set(value); }
        public List<Property> property_values { get => (List<Property>)Get(); set => Set(value); }
    }
    
    public class ListingImage : EtsyObject
    {
        public long listing_id { get; set; }
        public long listing_image_id { get; set; }
        public string hex_code { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int hue { get; set; }
        public int saturation { get; set; }
        public int brightness { get; set; }
        public bool is_black_and_white { get; set; }
        public int creation_tsz { get; set; }
        public int rank { get; set; }
        public string url_75x75 { get; set; }
        public string url_170x135 { get; set; }
        public string url_570xN { get; set; }
        public string url_fullxfull { get; set; }
        public int full_height { get; set; }
        public int full_width { get; set; }

        public string cache_thumb_path
        {
            get
            {
                return myConn.GetListingImageThumbPath(listing_id, listing_image_id);
            }
        }
    }
    
    public class ShippingProfile : EtsyObject
    {
        public long shipping_profile_id { get; set; }
        public string title { get; set; }
        public long user_id { get; set; }
        public int min_processing_days { get; set; }
        public int max_processing_days { get; set; }
        public string processing_days_display_label { get; set; }
        public string origin_country_iso { get; set; }
        public bool is_deleted { get; set; }
        public List<ShippingProfileDestination> shipping_profile_destinations { get; set; }
        public List<ShippingProfileUpgrade> shipping_profile_upgrades { get; set; }
        public string origin_postal_code { get; set; }
        public string profile_type { get; set; }
        public double domestic_handling_fee { get; set; }
        public double international_handling_fee { get; set; }
    }

    public class ShippingProfileDestination : EtsyObject
    {
        public long shipping_profile_destination_id { get; set; }
        public long shipping_profile_id { get; set; }
        public string origin_country_iso { get; set; }
        public string destination_country_iso { get; set; }
        public string destination_region { get; set; }
        public Price primary_cost { get; set; }
        public Price secondary_cost { get; set; }
        public long shipping_carrier_id { get; set; }
        public string mail_class { get; set; }
        public int min_delivery_days { get; set; }
        public int max_delivery_days { get; set; }
    }

    public class ShippingProfileUpgrade : EtsyObject
    {
        public long shipping_profile_id { get; set; }
        public long upgrade_id { get; set; }
        public string upgrade_name { get; set; }
        public string type { get; set; }
        public int rank { get; set; }
        public string language { get; set; }
        public Price price { get; set; }
        public Price secondary_price { get; set; }
        public long shipping_carrier_id { get; set; }
        public string mail_class { get; set; }
        public int min_delivery_days { get; set; }
        public int max_delivery_days { get; set; }
    }

    public class Translation : EtsyObject
    {
        public long listing_id { get; set; }
        public string language { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
    }

    public class Review : EtsyObject
    {
        public long shop_id { get; set; }
        public long listing_id { get; set; }
        public long transaction_id { get; set; }
        public long buyer_user_id { get; set; }
        public int rating { get; set; }
        public string review { get; set; }
        public string language { get; set; }
        public string image_url_fullxfull { get; set; }
        public int create_timestamp { get; set; }
        public int update_timestamp { get; set; }
    }

    public class Shop : EtsyObject
    {
        public long shop_id { get; set; }
        public long user_id { get; set; }
        public string shop_name { get; set; }
        public int create_date { get; set; }
        public string title { get; set; }
        public string announcement { get; set; }
        public string currency_code { get; set; }
        public bool is_vacation { get; set; }
        public string vacation_message { get; set; }
        public string sale_message { get; set; }
        public string digital_sale_message { get; set; }
        public int update_date { get; set; }
        public int listing_active_count { get; set; }
        public int digital_listing_count { get; set; }
        public string login_name { get; set; }
        public bool accepts_custom_requests { get; set; }
        public string policy_welcome { get; set; }
        public string policy_payment { get; set; }
        public string policy_shipping { get; set; }
        public string policy_refunds { get; set; }
        public string policy_additional { get; set; }
        public string policy_seller_info { get; set; }
        public int policy_update_date { get; set; }
        public bool policy_has_private_receipt_info { get; set; }
        public bool has_unstructured_policies { get; set; }
        public string policy_privacy { get; set; }
        public string vacation_autoreply { get; set; }
        public string url { get; set; }
        public string image_url_760x100 { get; set; }
        public int num_favorers { get; set; }
        public List<string> languages { get; set; }
        public string icon_url_fullxfull { get; set; }
        public bool is_using_structured_policies { get; set; }
        public bool has_onboarded_structured_policies { get; set; }
        public bool include_dispute_form_link { get; set; }
        public bool is_direct_checkout_onboarded { get; set; }
        public bool is_etsy_payments_onboarded { get; set; }
        public bool is_calculated_eligible { get; set; }
        public bool is_opted_in_to_buyer_promise { get; set; }
        public bool is_shop_us_based { get; set; }
        public int transaction_sold_count { get; set; }
        public string shipping_from_country_iso { get; set; }
        public string shop_location_country_iso { get; set; }
        public int review_count { get; set; }
        public double review_average { get; set; }
    }

    public class ShopSection : EtsyObject
    {
        public long shop_section_id { get; set; }
        public string title { get; set; }
        public int rank { get; set; }
        public long user_id { get; set; }
        public int active_listing_count { get; set; }
    }
    
    public class ProductionPartner : EtsyObject
    {
        public long production_partner_id { get; set; }
        public string partner_name { get; set; }
        public string location { get; set; }
    }

    public class User : EtsyObject
    {
        public long user_id { get; set; }
        public string login_name { get; set; }
        public string primary_email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int create_timestamp { get; set; }
        public long referred_by_user_id { get; set; }
        public bool use_new_inventory_endpoints { get; set; }
        public bool is_seller { get; set; }
        public string bio { get; set; }
        public string gender { get; set; }
        public string birth_month { get; set; }
        public string birth_day { get; set; }
        public int transaction_buy_count { get; set; }
        public int transaction_sold_count { get; set; }
    }

    public class UserAddress : EtsyObject
    {
        public long user_address_id { get; set; }
        public long user_id { get; set; }
        public string name { get; set; }
        public string first_line { get; set; }
        public string second_line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string iso_country_code { get; set; }
        public string country_name { get; set; }
        public bool is_default_shipping_address { get; set; }
    }

    public class LedgerEntry : EtsyObject
    {
        public long entry_id { get; set; }
        public long ledger_id { get; set; }
        public int sequence_number { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public double balance { get; set; }
        public int create_date { get; set; }
        public string ledger_type { get; set; }
        public string reference_type { get; set; }
        public string reference_id { get; set; }
    }
    
    public class Payment : EtsyObject
    {
        public long payment_id { get; set; }
        public long buyer_user_id { get; set; }
        public long shop_id { get; set; }
        public long receipt_id { get; set; }
        public Price amount_gross { get; set; }
        public Price amount_fees { get; set; }
        public Price amount_net { get; set; }
        public Price posted_gross { get; set; }
        public Price posted_fees { get; set; }
        public Price posted_net { get; set; }
        public Price adjusted_gross { get; set; }
        public Price adjusted_fees { get; set; }
        public Price adjusted_net { get; set; }
        public string currency { get; set; }
        public string shop_currency { get; set; }
        public string buyer_currency { get; set; }
        public long shipping_user_id { get; set; }
        public long shipping_address_id { get; set; }
        public long billing_address_id { get; set; }
        public string status { get; set; }
        public int shipped_timestamp { get; set; }
        public int create_timestamp { get; set; }
        public int update_timestamp { get; set; }
        public List<PaymentAdjustment> payment_adjustments { get; set; }
    }

    public class PaymentAdjustment : EtsyObject
    {
        public long payment_adjustment_id { get; set; }
        public long payment_id { get; set; }
        public string status { get; set; }
        public bool is_success { get; set; }
        public long user_id { get; set; }
        public string reason_code { get; set; }
        public double total_adjustment_amount { get; set; }
        public double shop_total_adjustment_amount { get; set; }
        public double buyer_total_adjustment_amount { get; set; }
        public double total_fee_adjustment_amount { get; set; }
        public int create_timestamp { get; set; }
        public int update_timestamp { get; set; }
    }
    
    public class Receipt : EtsyObject
    {
        public long receipt_id { get; set; }
        public int receipt_type { get; set; }
        public long seller_user_id { get; set; }
        public string seller_email { get; set; }
        public long buyer_user_id { get; set; }
        public string buyer_email { get; set; }
        public string name { get; set; }
        public string first_line { get; set; }
        public string second_line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string status { get; set; }
        public string formatted_address { get; set; }
        public string country_iso { get; set; }
        public string payment_method { get; set; }
        public string payment_email { get; set; }
        public string message_from_seller { get; set; }
        public string message_from_buyer { get; set; }
        public string message_from_payment { get; set; }
        public bool is_paid { get; set; }
        public bool is_shipped { get; set; }
        public int create_timestamp { get; set; }
        public int update_timestamp { get; set; }
        public bool is_gift { get; set; }
        public string gift_message { get; set; }
        public Price grandtotal { get; set; }
        public Price subtotal { get; set; }
        public Price total_price { get; set; }
        public Price total_shipping_cost { get; set; }
        public Price total_tax_cost { get; set; }
        public Price total_vat_cost { get; set; }
        public Price discount_amt { get; set; }
        public Price gift_wrap_price { get; set; }
        public List<Shipment> shipments { get; set; }
        public List<Transaction> transactions { get; set; }
        
        public string web_url => $"https://www.etsy.com/your/orders/?order_id={receipt_id}";
    }

    public class Shipment : EtsyObject
    {
        public long receipt_shipping_id { get; set; }
        public int shipment_notification_timestamp { get; set; }
        public string carrier_name { get; set; }
        public string tracking_code { get; set; }
    }

    public class Transaction : EtsyObject
    {
        public long transaction_id { get; set; }
        public string title { get; set; }
        public string description { get; set; } 
        public string p_description => 
            description.Substring(0, Math.Min(description.IndexOf("\n"), description.Length));
        public long seller_user_id { get; set; }
        public long buyer_user_id { get; set; }
        public int create_timestamp { get; set; }
        public int paid_timestamp { get; set; }
        public int shipped_timestamp { get; set; }
        public double quantity { get; set; }
        public long listing_image_id { get; set; }
        public long receipt_id { get; set; }
        public bool is_digital { get; set; }
        public string file_data { get; set; }
        public long listing_id { get; set; }
        public string transaction_type { get; set; }
        public long product_id { get; set; }
        public string sku { get; set; }
        public Price price { get; set; }
        public Price shipping_cost { get; set; }
        public List<Variation> variations { get; set; }
        public string variation_str => string.Join(", ", from v in variations select v.formatted_value);
        public long shipping_profile_id { get; set; }
        public int min_processing_days { get; set; }
        public int max_processing_days { get; set; }
        public string shipping_method { get; set; }
        public string shipping_upgrade { get; set; }
        public int expected_ship_date { get; set; }
        
        public string buyer_receipt_message { get; set; }
        
        public DateTime expected_ship_datetime =>
            DateTimeOffset.FromUnixTimeSeconds(expected_ship_date).ToLocalTime().DateTime;
        
        public DateTime create_datetime =>
            DateTimeOffset.FromUnixTimeSeconds(create_timestamp).ToLocalTime().DateTime;
        
        public string listing_image_thumb_path
        {
            get
            {
                if (myConn != null) { return myConn.GetListingImageThumbPath(listing_id, listing_image_id); }
                else { return null; }
            }
        }
    }

    public class Variation : EtsyObject
    {
        public long property_id { get; set; }
        public long value_id { get; set; }
        public string formatted_name { get; set; }
        public string formatted_value { get; set; }
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
    
    public class EConnection
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
        get
        {
            return myAuthBox.shop_id;
        }
        set
        {
            myAuthBox.shop_id = value;
        }
    }
    
    public OAuth2 myAuth;
    private RestClient myRest;
    private JsonSerializerSettings myJsonSerializerSettings;
    private WebClient myWeb;
    
    private Shop myShop;
    
    public Dictionary<long, string> cachedThumbPaths;
    public Dictionary<long, Listing> cachedListings;
    
    public EConnection(string thisCacheFolder, string passWd)
    {
        myCacheFolder = thisCacheFolder;
        myAuthBox = new AuthLockBox(authFile, passWd);
        
        ContinueInit();
    }
    
    public EConnection(string thisCacheFolder, string passWd, string thisApiKey, string thisShopId)
    {
        myCacheFolder = thisCacheFolder;
        myAuthBox = new AuthLockBox(authFile, passWd, thisApiKey, thisShopId);
        
        ContinueInit();
    }
    
    private void ContinueInit()
    {
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
        
        myShop = GetShop(myAuthBox.shop_id);
    }
    
    public string Oauth2Get(RestRequest myReq, bool saveResponse = false, bool attemptReAuth = true)
    {
        myReq.AddHeader("authorization", $"Bearer {myAuthBox.access_token}");
        
        var myResult = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult();
        
        string myContents = myResult.Content;

        EtsyObject myResponse = JsonConvert.DeserializeObject<Shop>(myContents, myJsonSerializerSettings);
        
        if (myResponse.error != null)
        {
            SaveError(myResult.StatusCode 
                      + "\n" + myContents);   
            if (myResponse.error == "invalid_token" && attemptReAuth)
            {
                RequestRefreshToken();
                return Oauth2Get(myReq, saveResponse, false);
            }
        }
        
        if (saveResponse) 
        { 
            SaveResponse(myContents); 
        }
        
        return myContents;
    }
    
    public Shop GetShop(long thisShopId)
    {
        RestRequest myReq = new RestRequest("shops/{shop_id}");
        myReq.AddUrlSegment("shop_id", thisShopId);
        
        string retContents = Oauth2Get(myReq, true);
        
        Shop retShop = JsonConvert.DeserializeObject<Shop>(retContents, myJsonSerializerSettings);
        
        return retShop;
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
    
    public Listing RefreshListing(int listingId)
    {
        var myReq = new RestRequest("listings/{listing_id}");
        myReq.AddUrlSegment("listing_id", listingId);

        string retContents = myRest.ExecuteAsync(myReq).GetAwaiter().GetResult().Content;
        SaveResponse(retContents);
        
        return JsonConvert.DeserializeObject<Listing>(retContents, myJsonSerializerSettings);
    }

    public List<Receipt> GetShopReceipts(bool was_paid = true, bool was_shipped = false, int offset = 0)
    {
        int limit = 25;
        RestRequest myReq = new RestRequest("shops/{shop_id}/receipts");
        myReq.AddUrlSegment("shop_id", myShop.shop_id);
        myReq.AddQueryParameter("was_paid", was_paid);
        myReq.AddQueryParameter("was_shipped", was_shipped);
        myReq.AddQueryParameter("limit", 25);
        myReq.AddQueryParameter("offset", offset);
        
        string retContents = Oauth2Get(myReq);
        Results<Receipt> retObj = 
            JsonConvert.DeserializeObject<Results<Receipt>>(retContents, myJsonSerializerSettings);
        
        List<Receipt> retRecpts = new List<Receipt>();
        
        if (retObj.count > 0)
        {
            retRecpts.AddRange(retObj.results.ToList());
            
            if (offset + limit < retObj.count)
            {
                retRecpts.AddRange(GetShopReceipts(was_paid, was_shipped, offset + limit));
            }
        }
        
        return retRecpts;
    }
    
    public Receipt? GetShopReceipt(long receipt_id)
    {
        RestRequest myReq = new RestRequest("shops/{shop_id}/receipts/{receipt_id}");
        myReq.AddUrlSegment("shop_id", myShop.shop_id);
        myReq.AddUrlSegment("receipt_id", receipt_id);
        
        string retContents = Oauth2Get(myReq);
        Results<Receipt> retObj = 
            JsonConvert.DeserializeObject<Results<Receipt>>(retContents, myJsonSerializerSettings);

        if (retObj.results.Count < 1)
        {
            return null; 
        }
        
        return retObj.results.First();
    }
    
    public BindingList<ListingImage> GetListingImages(long listing_id)
    {
        RestRequest myReq = new RestRequest("listings/{listing_id}/images");
        myReq.AddUrlSegment("listing_id", listing_id);
        string retContents = Oauth2Get(myReq, true);
        Results<ListingImage> retObj = 
            JsonConvert.DeserializeObject<Results<ListingImage>>(retContents, myJsonSerializerSettings);
        return new BindingList<ListingImage>(retObj.results.ToList());
    }

    public ListingImage GetListingImage(long listing_id, long listing_image_id)
    {
        RestRequest myReq = new RestRequest("listings/{listing_id}/images/{listing_image_id}");
        myReq.AddUrlSegment("listing_id", listing_id);
        myReq.AddUrlSegment("listing_image_id", listing_image_id);
        string retContents = Oauth2Get(myReq, true);
        ListingImage retObj = 
            JsonConvert.DeserializeObject<ListingImage>(retContents, myJsonSerializerSettings);
        return retObj;
    }
    
    public bool RequestRefreshToken()
    {
        AccessToken reqToken = myAuth.RefreshAccessToken(myAuthBox.refresh_token);
        
        if (reqToken.error != null)
        {
            SaveError($"error: {reqToken.error} \n " +
                      $"description: {reqToken.error_description} \n uri: {reqToken.error_uri}");
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
    
    public string GetListingImageThumbPath(long listing_id, long listing_image_id)
    {
        if (cachedThumbPaths == null)
        {
            cachedThumbPaths = new Dictionary<long, string>();
        }
        
        if (cachedThumbPaths.ContainsKey(listing_image_id))
        {
            return cachedThumbPaths[listing_image_id];
        }

        Directory.CreateDirectory(thumbsFolder);
        string cacheUri = $@"{thumbsFolder}\{listing_image_id}";
        
        if (!System.IO.File.Exists(cacheUri))
        {
            ListingImage myImage = GetListingImage(listing_id, listing_image_id);
            myWeb.DownloadFile(myImage.url_75x75, cacheUri);
        }
        
        cachedThumbPaths[listing_image_id] = cacheUri;
        
        return cachedThumbPaths[listing_image_id];
    }
    
    public string GetListingImageFull(long listing_id, long listing_image_id)
    {
        string cacheUri = $@"{fullImagesFolder}\{listing_image_id}";
        
        if (!System.IO.File.Exists(cacheUri))
        {
            ListingImage myImage = GetListingImage(listing_id, listing_image_id);
            myWeb.DownloadFile(myImage.url_fullxfull, cacheUri);
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
            cachedListings = new Dictionary<long, Listing>();
        }
        
        string listingsFileContents = File.ReadAllText(listingsFile);
        cachedListings = JsonConvert.DeserializeObject<Dictionary<long, Listing>>(listingsFileContents);
        
        foreach (Listing thisListing in cachedListings.Values)
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

    public Results<Listing> GetShopListings(int offset, int limit)
    {
        RestRequest thisReq = new RestRequest(@"shops/{shop_id}/listings", Method.Get);
        thisReq.AddUrlSegment("shop_id", myShop.shop_id);
        
        thisReq.AddQueryParameter("limit", limit);
        thisReq.AddQueryParameter("offset", offset);
        thisReq.AddQueryParameter("sort_on", "updated");
        
        string retContents = Oauth2Get(thisReq, true);

        return JsonConvert.DeserializeObject<Results<Listing>>(retContents, myJsonSerializerSettings);
    }
    
    public void RefreshCachedListings()
    {
        if (cachedListings == null)
        {
            cachedListings = new Dictionary<long, Listing>();
        }
        
        int limit = 25;
        Results<Listing> myResults = GetShopListings(0, limit);
        int total = myResults.count;

        for (int offset = limit; offset <= total; offset = offset + limit)
        {
            foreach (Listing thisListing in myResults.results)
            {
                if (cachedListings.ContainsKey(thisListing.listing_id))
                {
                    if (thisListing.last_modified_timestamp > 
                        cachedListings[thisListing.listing_id].last_modified_timestamp)
                    {
                        cachedListings[thisListing.listing_id] = thisListing;
                        cachedListings[thisListing.listing_id].images = GetListingImages(thisListing.listing_id);
                    }
                    else
                    {
                        goto finished_updating;
                    }
                }
                else
                {
                    cachedListings[thisListing.listing_id] = thisListing;
                    cachedListings[thisListing.listing_id].images = GetListingImages(thisListing.listing_id);
                    cachedListings[thisListing.listing_id].SetConnection(this);
                }
            }

            myResults = GetShopListings(offset, limit);
        }
        
        finished_updating:
        SaveCachedListings();
    }

    public Listing GetListing(long thisListingId)
    {
        if (cachedListings.ContainsKey(thisListingId))
        {
            return cachedListings[thisListingId];
        }

        return null;
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
}

}