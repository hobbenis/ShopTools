//put interfaces and the classes that handle them here

using System.Collections;
using ShopTools.Data.Common;

namespace ShopTools.Data.Market;

public interface IMarketOrder
{
    public string WebUrl { get; }
    public string Platform { get; }

    public string OrderId { get; }
    public string CustomerName { get; }
    public string City { get; }
    public string State { get; }
    
    public IEnumerable<IMarketOrderLine> OrderLines { get; }
    public IEnumerable<IMarketOrderShipment> OrderShipments { get; }
    public DateTime EarliestExpectedShipDate { get; }
}

public interface IMarketOrderLine
{
    public string Sku { get; }
    public double Quantity { get; }
    public string Description { get; }
    public string DescriptionFirstLine { get; }
    public string Variation { get; }
    public DateTime ExpectedShipDateTime { get; }
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
    public IEnumerable<IMarketImage> ListingImages { get; }
}

public interface IMarketConnection
{
    public void LoadCachedData();
    public void SaveCachedData();
    public void SaveAuthData();

    public void RefreshListingCache();
    public void RefreshOrderCache();

    public OAuth2 Auth { get; }
    public AuthLockBox AuthBox { get; }
    
    public IEnumerable<IMarketOrder> Orders { get; }
    public IEnumerable<IMarketListing> Listings { get; }
}