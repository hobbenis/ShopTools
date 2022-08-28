using System.Diagnostics;
using System.Windows;
using ShopTools.Data.Etsy;

namespace ShopTools.Reports;

public partial class OrderDetail : System.Windows.Window
{
    private EtsyReceipt myReceipt;
    
    public OrderDetail(EtsyReceipt thisReceipt)
    {
        InitializeComponent();
        myReceipt = thisReceipt;
        this.DataContext = myReceipt;
    }
    
    private void ButtonBase_OnClick(object thisSender, RoutedEventArgs thisE)
    {
        Process.Start(myReceipt.WebUrl);
    }
}