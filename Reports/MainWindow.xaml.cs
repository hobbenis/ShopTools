using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShopTools.Etsy;
using Microsoft.VisualBasic;
using System.IO;
using ShopTools.Reports;

namespace ShopTools.Reports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cacheFolder => $@"{appDataFolder}\ShopTools\etsy";
        
        private EConnection myEtsyConn;
        
        private List<Transaction> TransList;
        private ProductionSummary myProductionSummary;
        private List<ProductionSummaryLine> ProdList;
        private List<Listing> Listings;

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
                    myEtsyConn = new EConnection(cacheFolder, myPass);
                    
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
                
                myEtsyConn = new EConnection(cacheFolder, inputPass, inputApiKey, inputShopId);
            }
            
            btnAuth.IsEnabled = true;
            btnUnlock.IsEnabled = false;
            
            myEtsyConn.LoadCachedListings();
            
            myProductionSummary = new ProductionSummary(myEtsyConn, DateTime.Today.AddDays(4));
            lvListings.ItemsSource = myEtsyConn.cachedListings.Values;
        }

        private void tsbAuth_Click(object sender, EventArgs e)
        {
            Auth myAuthDialog = new Auth(myEtsyConn);
            myAuthDialog.Show();
        }

        private void MainWindow_OnClosing(object? thisSender, CancelEventArgs thisE)
        {
            if (!(myEtsyConn == null)) { myEtsyConn.SaveAuth(); }
        }

        private void Log(string message)
        {
            lbStatus.Content = message;
        }

        private void RefreshListings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Warning! If you have many listings this could take quite some time.");
            
            myEtsyConn.RefreshCachedListings();
            
            Log($"{myEtsyConn.cachedListings.Count} listings cached.");
            lvListings.ItemsSource = myEtsyConn.cachedListings.Values;
        }
        
        private void RefreshOrders(object sender, RoutedEventArgs e)
        {
            TransList = myProductionSummary.OpenTransactions().OrderBy(x => x.expected_ship_date).ToList();
            
            Log($"{TransList.Count} orders downloaded.");
            lvOrders.ItemsSource = TransList;
        }  
        
        private void RefreshProductionSummary(object sender, RoutedEventArgs e)
        {
            myProductionSummary.cutoff_date = dpProdCutoff.SelectedDate.Value;
            myProductionSummary.RefreshReceipts();
            
            ProdList = myProductionSummary.OpenTransactionSummaryByListing().OrderBy(x => x.earliest_ship_date).ToList();
            
            Log($"{ProdList.Count} different items required for orders before the cuttoff date.");
            lvProduction.ItemsSource = ProdList;
        }

        private void btnListingDetail_OnClick(object sender, RoutedEventArgs e)
        {
            new ListingDetail((sender as Button).DataContext as Listing).Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new OrderDetail(myEtsyConn.GetShopReceipt(((sender as Button).DataContext as Transaction).receipt_id)).Show();
        }
    }
}