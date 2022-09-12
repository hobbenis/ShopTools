namespace ShopTools.Shopify;

public class Customer
{
    public bool accepts_marketing;
    public DateTime accepts_marketing_updated_at;
    public List<CustomerAddress> addresses;
    public string currency;
    public DateTime created_at;
    public CustomerAddress default_address;
    public string email;
  
    /*{
    "email_marketing_consent": {
      "state": "subscribed",
      "opt_in_level": "confirmed_opt_in",
      "consent_updated_at": "2022-04-01T11:22:06-04:00"
    },*/

    public string first_name;
    public long id;
    public string last_name;
    public long last_order_id;
  
    /*
    "last_order_name": "#1169",
    "metafield": {
      "key": "new",
      "namespace": "global",
      "value": "newvalue",
      "type": "string"
    },
    "marketing_opt_in_level": "confirmed_opt_in",
    "multipass_identifier": null,
    "note": "Placed an order that had a fraud warning",
    "orders_count": 3,
    "password": "password",
    "password_confirmation": "password_confirmation",
    "phone": "+16135551111",
    "sms_marketing_consent": {
      "state": "subscribed",
      "opt_in_level": "single_opt_in",
      "consent_updated_at": "2021-08-03T15:31:06-04:00",
      "consent_collected_from": "OTHER"
    },
    "state": "disabled",
    "tags": "loyal",
    "tax_exempt": true,
    "tax_exemptions": [
      "CA_STATUS_CARD_EXEMPTION",
      "CA_BC_RESELLER_EXEMPTION"
    ],
    "total_spent": "375.30",
    "updated_at": "2012-08-24T14:01:46-04:00",
    "verified_email": true
  }*/
}

public class CustomerAddress
{
    public string address1;
    public string address2;
    public string city;
    public string? company;
    public string country;
    public string first_name;
    public string last_name;
    public string phone;
    public string province;
    public string zip;
    public string name;
    public string province_code;
    public string country_code;
    public string latitude;
    public string longitude;
}