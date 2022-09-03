using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShopTools.Data.Etsy;
using Microsoft.VisualBasic;
using System.IO;
using ShopTools.Data.Market;
using ShopTools.Production;

namespace ShopTools.Reports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cacheFolder => $@"{appDataFolder}\ShopTools\etsy";
        
        private IMarketConnection myEtsyConn;
        
        private ProductionSummary myProductionSummary;

        private int daysAhead = 7;
        
        public MainWindow()
        {
            InitializeComponent();
            
            btnAuth.IsEnabled = false;
            Log("Unlock token file to request data.");

            dpOrdCutoff.SelectedDate = DateTime.Now.AddDays(daysAhead);
            dpProdCutoff.SelectedDate = DateTime.Now.AddDays(daysAhead);
        }

        private void tsbUnlock_Click(object sender, EventArgs e)
        {
            if (!(myEtsyConn == null))
            {
                Log("An authorization file has already been loaded.");
                return;
            }
            
            if (File.Exists(cacheFolder + @"\auth.txt"))
            {
                string myPass = Interaction.InputBox("Enter the password for the auth file.");
                
                try
                {
                    myEtsyConn = new EtsyConnection(cacheFolder, myPass);
                    
                    Log("Authorization data unlocked.");
                }
                catch (Exception myError)
                {
                    Log($"Incorrect password. Error: {myError.Message}");
                    return;
                }
            }
            else
            {
                string inputShopId = Interaction.InputBox("Enter your shop id:");
                if (inputShopId.Length < 1) { return; }
                string inputApiKey = Interaction.InputBox("Enter your api key:");
                if (inputApiKey.Length < 1) { return; }
                string inputPass = Interaction.InputBox("Enter a password to set for the file:");
                if (inputPass.Length < 1) { return; }
                
                myEtsyConn = new EtsyConnection(cacheFolder, inputPass, inputApiKey, inputShopId);
            }
            
            btnAuth.IsEnabled = true;
            btnUnlock.IsEnabled = false;
            
            myEtsyConn.LoadCachedData();
            
            lvListings.ItemsSource = myEtsyConn.Listings;
        }

        private void tsbAuth_Click(object sender, EventArgs e)
        {
            Auth myAuthDialog = new Auth(myEtsyConn);
            myAuthDialog.Show();
        }

        private void MainWindow_OnClosing(object? thisSender, CancelEventArgs thisE)
        {
            if (!(myEtsyConn == null)) { myEtsyConn.SaveAuthData(); }
        }

        private void Log(string message)
        {
            lbStatus.Content = message;
        }

        private void RefreshListings(object sender, RoutedEventArgs e)
        {
            if (myEtsyConn is null) { return; }
            
            MessageBox.Show("Warning! If you have many listings this could take quite some time.");
            
            myEtsyConn.RefreshListingCache();
            
            Log($"{myEtsyConn.Listings.Count()} listings cached.");
            lvListings.ItemsSource = myEtsyConn.Listings;
        }
        
        private void RefreshOrders(object sender, RoutedEventArgs e)
        {
            myEtsyConn.RefreshOrderCache();
            myProductionSummary = new ProductionSummary(myEtsyConn.Orders, dpOrdCutoff.SelectedDate.Value);
            
            lvOrders.ItemsSource = myProductionSummary.OrderLines;
            
            Log($"{lvOrders.Items.Count} orders downloaded.");
        }  
        
        private void RefreshProductionSummary(object sender, RoutedEventArgs e)
        {
            myEtsyConn.RefreshOrderCache();
            myProductionSummary = new ProductionSummary(myEtsyConn.Orders, dpProdCutoff.SelectedDate.Value);
            
            lvProduction.ItemsSource = 
                myProductionSummary.ProductionSummaryLines.OrderBy(x => x.EarliestShipDate).ToList();
            
            Log($"{lvProduction.Items.Count} different items required for orders before the cutoff date.");
        }
        
        private void btnListingDetail_OnClick(object sender, RoutedEventArgs e)
        {
            IMarketListing thisListing = (sender as Button).DataContext as IMarketListing;
            
            new ListingDetail(thisListing).Show();
        }
        
        private void btnTransactionItem_OnClick(object sender, RoutedEventArgs e)
        {
            IMarketOrderLine thisTransaction = (sender as Button).DataContext as IMarketOrderLine;
            
            new OrderDetail(thisTransaction.PlatformOrder).Show();
        }
        
        private void btnProductionItem_OnClick(object sender, RoutedEventArgs e)
        {
            ProductionSummaryLine thisProdLine = (sender as Button).DataContext as ProductionSummaryLine;
            
            foreach (IMarketListing thisListing in thisProdLine.RelatedMarketListings)
            {
                new ListingDetail(thisListing).Show();
            }
        }
    }
}