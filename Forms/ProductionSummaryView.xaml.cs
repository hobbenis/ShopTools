using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShopTools.Interfaces;
using ShopTools.Reports;

namespace ShopTools;

public partial class ProductionSummaryView : Window
{
    private ProductionSummaryReport myProductionSummary;

    public ProductionSummaryView(ProductionSummaryReport thisProductionSummary)
    {
        myProductionSummary = thisProductionSummary;
        InitializeComponent();
        
        lvProduction.ItemsSource = 
            myProductionSummary.ProductionSummaryLines.OrderBy(x => x.EarliestShipDate).ToList();
    }

    private void btnProductionItem_OnClick(object sender, RoutedEventArgs e)
    {
        ProductionSummaryReportLine thisProdLine = (sender as Button).DataContext as ProductionSummaryReportLine;
        
        foreach (IMarketListing thisListing in thisProdLine.RelatedMarketListings)
        {
            new ListingDetail(thisListing).Show();
        }
    }
}