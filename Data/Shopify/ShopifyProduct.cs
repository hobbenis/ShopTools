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
