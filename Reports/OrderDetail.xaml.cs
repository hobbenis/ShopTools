using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ShopTools.Data.Etsy;
using ShopTools.Data.Market;

namespace ShopTools.Reports;

public partial class OrderDetail : System.Windows.Window
{
    private IMarketOrder myReceipt;
    
    public OrderDetail(IMarketOrder thisReceipt)
    {
        InitializeComponent();
        myReceipt = thisReceipt;
        this.DataContext = myReceipt;
    }
    
    private void btnGotoUrl_OnClick(object thisSender, RoutedEventArgs thisE)
    {
        Process.Start(myReceipt.WebUrl);
    }

    private void btnOpenListing_OnClick(object sender, RoutedEventArgs e)
    {
        IMarketOrderLine thisLine = (sender as Button).DataContext as IMarketOrderLine;
            
        new ListingDetail(thisLine.PlatformListing).Show();
    }
}