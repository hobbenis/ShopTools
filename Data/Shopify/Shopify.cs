//Neither me nor this project have any affiliation with Shopify.
//This class/namespace/etc is simply named as such because it accesses Shopify's API
//You will need to provide your own API key if you wish to use this.
//As such, you should carefully review this and make sure it will do what you need.

//this file is to contain classes, etc specific to shopify's api

namespace ShopTools.Shopify;

public class Product
{
    public string body_html;
    public DateTime created_at;
    public string handle;
    public long id;
    public List<ProductImage> images;
    public Dictionary<string, object> options;
    public string product_type;
    public DateTime published_at;
    public string published_scope;
    public string status;
    public string tags;
    public string template_suffix;
    public string title;
    public DateTime updated_at;
    public List<ProductVariant> variants;
    public string vendor;
}

public class ProductImage
{
    public DateTime created_at;
    public long id;
    public int position;
    public long product_id;
    public List<long> variant_ids;
    public string src;
    public int width;
    public int height;
    public DateTime updated_at;
}

public class ProductVariant
{
    public string barcode;
    public string compare_at_price;
    public DateTime created_at;
    public string fulfillment_service;
    public double grams;
    public long id;
    public long image_id;
    public long inventory_item_id;
    public string inventory_management;
    public string inventory_policy;
    public int inventory_quantity;
    public int old_inventory_quantity;
    public int inventory_quantity_adjustment;
    public Dictionary<string, string> option;
    public Dictionary<string, Price> presentment_prices;
    public int position;
    public string price;
    public long product_id;
    public bool requires_shipping;
    public string sku;
    public bool taxable;
    public string tax_code;
    public string title;
    public DateTime updated_at;
    public double weight;
    public string weight_unit;
}

public class Price
{
    public string currency_code;
    public double amount;
}

public class ClientDetails
{
  public string accept_language;
  public int browser_height;
  public string browser_ip;
  public int browser_width;
  public string session_hash;
  public string user_agent;
}

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

public class Order
{
public int app_id;
public CustomerAddress billing_address;
public string browser_ip;
public bool buyer_accepts_marketing;
public string cancel_reason;
public DateTime cancelled_at;
public string cart_token;
public string checkout_token;
public ClientDetails client_details;
public DateTime closed_at;
public DateTime created_at;
public string currency;
public string current_total_discounts;
public Dictionary<string, Price> current_total_discounts_set;
public Dictionary<string, Price> current_total_duties_set;
public string current_total_price;
public Dictionary<string, Price> current_total_price_set;
public string current_subtotal_price;
public Dictionary<string, Price> current_subtotal_price_set;
public string current_total_tax;
public Dictionary<string, Price> current_total_tax_set;

public Customer customer;
public string customer_locale;


/*
"discount_applications": {
"discount_applications": [
  {
    "type": "manual",
    "title": "custom discount",
    "description": "customer deserved it",
    "value": "2.0",
    "value_type": "fixed_amount",
    "allocation_method": "across",
    "target_selection": "explicit",
    "target_type": "line_item"
  },
  {
    "type": "script",
    "description": "my scripted discount",
    "value": "5.0",
    "value_type": "fixed_amount",
    "allocation_method": "across",
    "target_selection": "explicit",
    "target_type": "shipping_line"
  },
  {
    "type": "discount_code",
    "code": "SUMMERSALE",
    "value": "10.0",
    "value_type": "fixed_amount",
    "allocation_method": "across",
    "target_selection": "all",
    "target_type": "line_item"
  }
]
},*/



/*
"discount_codes": [
{
  "code": "SPRING30",
  "amount": "30.00",
  "type": "fixed_amount"
}
],*/

  public string email;
  public bool estimated_taxes;
  public string financial_status;
  
/*
"fulfillments": [
{
  "created_at": "2012-03-13T16:09:54-04:00",
  "id": 255858046,
  "order_id": 450789469,
  "status": "failure",
  "tracking_company": "USPS",
  "tracking_number": "1Z2345",
  "updated_at": "2012-05-01T14:22:25-04:00"
}
],
"fulfillment_status": "partial",
"gateway": "shopify_payments",
"id": 450789469,
"landing_site": "http://www.example.com?source=abc",
"line_items": [
{
  "fulfillable_quantity": 1,
  "fulfillment_service": "amazon",
  "fulfillment_status": "fulfilled",
  "grams": 500,
  "id": 669751112,
  "price": "199.99",
  "product_id": 7513594,
  "quantity": 1,
  "requires_shipping": true,
  "sku": "IPOD-342-N",
  "title": "IPod Nano",
  "variant_id": 4264112,
  "variant_title": "Pink",
  "vendor": "Apple",
  "name": "IPod Nano - Pink",
  "gift_card": false,
  "price_set": {
    "shop_money": {
      "amount": "199.99",
      "currency_code": "USD"
    },
    "presentment_money": {
      "amount": "173.30",
      "currency_code": "EUR"
    }
  },
  "properties": [
    {
      "name": "custom engraving",
      "value": "Happy Birthday Mom!"
    }
  ],
  "taxable": true,
  "tax_lines": [
    {
      "title": "HST",
      "price": "25.81",
      "price_set": {
        "shop_money": {
          "amount": "25.81",
          "currency_code": "USD"
        },
        "presentment_money": {
          "amount": "20.15",
          "currency_code": "EUR"
        }
      },
      "channel_liable": true,
      "rate": 0.13
    }
  ],
  "total_discount": "5.00",
  "total_discount_set": {
    "shop_money": {
      "amount": "5.00",
      "currency_code": "USD"
    },
    "presentment_money": {
      "amount": "4.30",
      "currency_code": "EUR"
    }
  },
  "discount_allocations": [
    {
      "amount": "5.00",
      "discount_application_index": 2,
      "amount_set": {
        "shop_money": {
          "amount": "5.00",
          "currency_code": "USD"
        },
        "presentment_money": {
          "amount": "3.96",
          "currency_code": "EUR"
        }
      }
    }
  ],
  "origin_location": {
    "id": 1390592786454,
    "country_code": "CA",
    "province_code": "ON",
    "name": "Apple",
    "address1": "700 West Georgia Street",
    "address2": "1500",
    "city": "Toronto",
    "zip": "V7Y 1G5"
  },
  "duties": [
    {
      "id": "2",
      "harmonized_system_code": "520300",
      "country_code_of_origin": "CA",
      "shop_money": {
        "amount": "164.86",
        "currency_code": "CAD"
      },
      "presentment_money": {
        "amount": "105.31",
        "currency_code": "EUR"
      },
      "tax_lines": [
        {
          "title": "VAT",
          "price": "16.486",
          "rate": 0.1,
          "price_set": {
            "shop_money": {
              "amount": "16.486",
              "currency_code": "CAD"
            },
            "presentment_money": {
              "amount": "10.531",
              "currency_code": "EUR"
            }
          },
          "channel_liable": true
        }
      ],
      "admin_graphql_api_id": "gid://shopify/Duty/2"
    }
  ]
}
],*/

  public long location_id;
  public string name;
  public string note;
  public Dictionary<string, string> note_attributes;

  public int number;

  public int order_number;

  public Dictionary<string, Price> original_total_duties_set;
/*

"payment_details": {
"avs_result_code": "Y",
"credit_card_bin": "453600",
"cvv_result_code": "M",
"credit_card_number": "•••• •••• •••• 4242",
"credit_card_company": "Visa"
},
"payment_terms": {
"amount": 70,
"currency": "CAD",
"payment_terms_name": "NET_30",
"payment_terms_type": "NET",
"due_in_days": 30,
"payment_schedules": [
  {
    "amount": 70,
    "currency": "CAD",
    "issued_at": "2020-07-29T13:02:43-04:00",
    "due_at": "2020-08-29T13:02:43-04:00",
    "completed_at": "null",
    "expected_payment_method": "shopify_payments"
  }
]
},
"payment_gateway_names": [
"authorize_net",
"Cash on Delivery (COD)"
],
"phone": "+557734881234",
"presentment_currency": "CAD",
"processed_at": "2008-01-10T11:00:00-05:00",
"processing_method": "direct",
"referring_site": "http://www.anexample.com",
"refunds": [
{
  "id": 18423447608,
  "order_id": 394481795128,
  "created_at": "2018-03-06T09:35:37-05:00",
  "note": null,
  "user_id": null,
  "processed_at": "2018-03-06T09:35:37-05:00",
  "refund_line_items": [],
  "transactions": [],
  "order_adjustments": []
}
],
"shipping_address": {
"address1": "123 Amoebobacterieae St",
"address2": "",
"city": "Ottawa",
"company": null,
"country": "Canada",
"first_name": "Bob",
"last_name": "Bobsen",
"latitude": "45.41634",
"longitude": "-75.6868",
"phone": "555-625-1199",
"province": "Ontario",
"zip": "K2P0V6",
"name": "Bob Bobsen",
"country_code": "CA",
"province_code": "ON"
},
"shipping_lines": [
{
  "code": "INT.TP",
  "price": "4.00",
  "price_set": {
    "shop_money": {
      "amount": "4.00",
      "currency_code": "USD"
    },
    "presentment_money": {
      "amount": "3.17",
      "currency_code": "EUR"
    }
  },
  "discounted_price": "4.00",
  "discounted_price_set": {
    "shop_money": {
      "amount": "4.00",
      "currency_code": "USD"
    },
    "presentment_money": {
      "amount": "3.17",
      "currency_code": "EUR"
    }
  },
  "source": "canada_post",
  "title": "Small Packet International Air",
  "tax_lines": [],
  "carrier_identifier": "third_party_carrier_identifier",
  "requested_fulfillment_service_id": "third_party_fulfillment_service_id"
}
],
"source_name": "instagram",
"source_identifier": "ORDERID-123",
"source_url": "{URL_to_order}",
"subtotal_price": 398,
"subtotal_price_set": {
"shop_money": {
  "amount": "141.99",
  "currency_code": "CAD"
},
"presentment_money": {
  "amount": "90.95",
  "currency_code": "EUR"
}
},
"tags": "imported, vip",
"tax_lines": [
{
  "price": 11.94,
  "rate": 0.06,
  "title": "State Tax",
  "channel_liable": true
}
],
"taxes_included": false,
"test": true,
"token": "b1946ac92492d2347c6235b4d2611184",
"total_discounts": "0.00",
"total_discounts_set": {
"shop_money": {
  "amount": "0.00",
  "currency_code": "CAD"
},
"presentment_money": {
  "amount": "0.00",
  "currency_code": "EUR"
}
},
"total_line_items_price": "398.00",
"total_line_items_price_set": {
"shop_money": {
  "amount": "141.99",
  "currency_code": "CAD"
},
"presentment_money": {
  "amount": "90.95",
  "currency_code": "EUR"
}
},
"total_outstanding": "5.00",
"total_price": "409.94",
"total_price_set": {
"shop_money": {
  "amount": "164.86",
  "currency_code": "CAD"
},
"presentment_money": {
  "amount": "105.31",
  "currency_code": "EUR"
}
},
"total_shipping_price_set": {
"shop_money": {
  "amount": "30.00",
  "currency_code": "USD"
},
"presentment_money": {
  "amount": "0.00",
  "currency_code": "USD"
}
},
"total_tax": "11.94",
"total_tax_set": {
"shop_money": {
  "amount": "18.87",
  "currency_code": "CAD"
},
"presentment_money": {
  "amount": "11.82",
  "currency_code": "EUR"
}
},
"total_tip_received": "4.87",
"total_weight": 300,
"updated_at": "2012-08-24T14:02:15-04:00",
"user_id": 31522279,
"order_status_url": {
"order_status_url": "https://checkout.shopify.com/112233/checkouts/4207896aad57dfb159/thank_you_token?key=753621327b9e8a64789651bf221dfe35"
}
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

public class Transaction
{
    
}

public class Refund
{
    
}