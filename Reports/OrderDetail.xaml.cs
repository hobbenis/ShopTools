using System.Diagnostics;
using System.Windows;
using ShopTools.Etsy;

namespace ShopTools.Reports;

public partial class OrderDetail : System.Windows.Window
{
    private Receipt myReceipt;
    
    public OrderDetail(Receipt thisReceipt)
    {
        InitializeComponent();
        myReceipt = thisReceipt;
    }
    
    private void ButtonBase_OnClick(object thisSender, RoutedEventArgs thisE)
    {
        Process.Start(myReceipt.web_url);
    }
}