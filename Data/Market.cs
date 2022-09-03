//put interfaces and the classes that handle them here

using System.Collections;

namespace ShopTools.Data.Market;

public interface IMarketOrder
{
    public IEnumerable<IMarketOrderLine> OrderLines { get; }
    public IEnumerable<IMarketOrderShipment> OrderShipments { get; }
    public DateTime EarliestExpectedShipDate { get; }
    
    public string WebUrl { get; }
    public string Platform { get; }
}

public interface IMarketOrderLine
{
    public string Sku { get; }
    public double Quantity { get; }
    public string Description { get; }
    public string DescriptionFirstLine { get; }
    public string Variation { get; }
    public DateTime ExpectedShipDate { get; }
    public string Platform { get; }
    public string PlatformListingId { get; }
    public string ImageThumbCachePath { get; }
    public string BuyerMessage { get; }
    public IMarketListing PlatformListing { get; }
    public IMarketOrder PlatformOrder { get; }
}

public interface IMarketOrderShipment
{
    public string Carrier { get; }
    public string TrackingId { get; }
}

public interface IMarketImage
{
    public string ThumbCachePath { get; }
    public string CachePath { get; }
}

public interface IMarketListing
{
    public string Title { get; }
    public string Description { get; }
    public string DescriptionFirstLine { get; }
    public string ImageThumbCachePath { get; }
    public string WebUrl { get; }
}

public interface IMarketConnection
{
    public IEnumerable<IMarketOrder> OpenOrders { get; }
    public IEnumerable<IMarketListing> Listings { get; }
}