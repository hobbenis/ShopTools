using ShopTools.Data.Etsy;
using ShopTools.Data.Market;

namespace ShopTools.Production;

public class ProductionSummaryLine
{
    public string Sku { get; set; }
    public double Quantity { get; set; }
    public string Description { get; set; }
    public string Variation { get; set; }
    public DateTime EarliestShipDate { get; set; }

    public string PlatformListingId { get; set; }
    public string Platform { get; set; }
    
    public string DescriptionFirstLine => 
        Description.Substring(0, Math.Min(Description.IndexOf("\n"), Description.Length));

    public string ImageThumbCachePath { get; set; }
}
    
public class ProductionSummary
{
    private List<IMarketOrder> myOrders;
    private DateTime myCutoffDate;
        
    public ProductionSummary(IEnumerable<IMarketOrder> theseOrders, DateTime thisCutOffDate)
    {
        myCutoffDate = thisCutOffDate;
        myOrders = new List<IMarketOrder>();
        AddOrders(theseOrders);
    }
    
    public void AddOrders(IEnumerable<IMarketOrder> theseOrders)
    {
        if (theseOrders is null) { return; }
        
        foreach (IMarketOrder thisOrder in theseOrders)
        {
            if (thisOrder.EarliestExpectedShipDate <= myCutoffDate)
            {
                myOrders.Add(thisOrder);
            }
        }
        
        SummarizeProduction();
    }

    private List<ProductionSummaryLine> mySummaryLines;
    
    private void SummarizeProduction()
    {
        mySummaryLines = new List<ProductionSummaryLine>();
        
        foreach (IMarketOrderLine thisTrans in OrderLines)
        {
            foreach (ProductionSummaryLine thisProdLine in mySummaryLines)
            {
                if ((thisProdLine.PlatformListingId.Equals(thisTrans.PlatformListingId)
                     && thisProdLine.Platform.Equals(thisTrans.Platform)) &&
                    thisProdLine.Variation.Equals(thisTrans.Variation))
                {
                    thisProdLine.Quantity += thisTrans.Quantity;
                    thisProdLine.EarliestShipDate = 
                        (new DateTime[] { thisProdLine.EarliestShipDate, thisTrans.ExpectedShipDate }).Max();

                    goto nextReceipt;
                }
            }
                
            mySummaryLines.Add(new ProductionSummaryLine()
            {
                PlatformListingId = thisTrans.PlatformListingId,
                Platform = thisTrans.Platform,
                Description = thisTrans.Description,
                EarliestShipDate = thisTrans.ExpectedShipDate,
                Sku = thisTrans.Sku,
                Quantity = thisTrans.Quantity,
                Variation = thisTrans.Variation,
                ImageThumbCachePath = thisTrans.ImageThumbCachePath
            });
                
            nextReceipt:
            continue;
        }
    }
    
    public List<IMarketOrder> Orders
    {
        get
        {
            return myOrders;    
        }
    }
        
    public IEnumerable<IMarketOrderLine> OrderLines
    {
        get
        {
            foreach (IMarketOrder thisReceipt in Orders)
            {
                foreach (IMarketOrderLine thisTrans in thisReceipt.OrderLines)
                {
                    yield return thisTrans;
                }
            }    
        }
    }
    
    public IEnumerable<ProductionSummaryLine> ProductionSummaryLines
    {
        get
        {
            foreach (ProductionSummaryLine thisLine in mySummaryLines)
            {
                yield return thisLine;
            }
        }
    }
}
    