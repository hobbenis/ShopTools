using ShopTools.Interfaces;

namespace ShopTools.Reports;

public class ProductionSummaryReportLine
{
    public string Sku { get; set; }
    public double Quantity { get; set; }
    public string Description { get; set; }
    public Dictionary<string, double> Variations { get; set; }
    public DateTime EarliestShipDate { get; set; }
    public List<IMarketListing> RelatedMarketListings { get; set; }
    
    public string PlatformListingId { get; set; }
    public string Platform { get; set; }
    
    public string DescriptionFirstLine => 
        Description.Substring(0, Math.Min(Description.IndexOf("\n"), Description.Length));

    public string ImageThumbCachePath { get; set; }
}
    
public class ProductionSummaryReport
{
    private List<IMarketOrder> myOrders;
    private DateTime myCutoffDate;
        
    public ProductionSummaryReport(IEnumerable<IMarketOrder> theseOrders, DateTime thisCutOffDate)
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

    private List<ProductionSummaryReportLine> mySummaryLines;
    
    private void SummarizeProduction()
    {
        mySummaryLines = new List<ProductionSummaryReportLine>();
        
        foreach (IMarketOrderLine thisTrans in OrderLines)
        {
            foreach (ProductionSummaryReportLine thisProdLine in mySummaryLines)
            {
                if (thisProdLine.PlatformListingId.Equals(thisTrans.PlatformListingId)
                     && thisProdLine.Platform.Equals(thisTrans.Platform))
                {
                    thisProdLine.Quantity += thisTrans.Quantity;
                    thisProdLine.EarliestShipDate = 
                        (new DateTime[] { thisProdLine.EarliestShipDate, thisTrans.ExpectedShipDateTime }).Max();

                    if (thisProdLine.Variations.ContainsKey(thisTrans.Variation))
                    {
                        thisProdLine.Variations[thisTrans.Variation] += thisTrans.Quantity;
                    }
                    else
                    {
                        thisProdLine.Variations[thisTrans.Variation] = thisTrans.Quantity;
                    }
                    
                    if (!thisProdLine.RelatedMarketListings.Contains(thisTrans.PlatformListing))
                    {
                        thisProdLine.RelatedMarketListings.Add(thisTrans.PlatformListing);
                    }
                    
                    goto nextReceipt;
                }
            }
                
            mySummaryLines.Add(new ProductionSummaryReportLine()
            {
                PlatformListingId = thisTrans.PlatformListingId,
                Platform = thisTrans.Platform,
                Description = thisTrans.Description,
                EarliestShipDate = thisTrans.ExpectedShipDateTime,
                Sku = thisTrans.Sku,
                Quantity = thisTrans.Quantity,
                Variations = new Dictionary<string, double>() { { thisTrans.Variation, thisTrans.Quantity } }, 
                ImageThumbCachePath = thisTrans.ImageThumbCachePath,
                RelatedMarketListings = new List<IMarketListing>() { thisTrans.PlatformListing }
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
    
    public IEnumerable<ProductionSummaryReportLine> ProductionSummaryLines
    {
        get
        {
            foreach (ProductionSummaryReportLine thisLine in mySummaryLines)
            {
                yield return thisLine;
            }
        }
    }
}
    