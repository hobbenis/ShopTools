using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows;
using System.Windows.Media;
using ShopTools.Data.Etsy;
using ShopTools.Data.Market;

namespace ShopTools.Reports;

class AuthScope : INotifyPropertyChanged
{
    private string _Key;
    private bool _Value;
    
    public string Key
    {
        get
        {
            return _Key;
        }
        set
        {
            _Key = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Key"));
        }
    }

    public bool Value
    {
        get
        {
            return _Value;
        }
        set
        {
            _Value = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

public partial class Auth : System.Windows.Window
{
    private IMarketConnection myConn;

    private ObservableCollection<AuthScope> authScopes = new ObservableCollection<AuthScope>()
    {
        new AuthScope() { Key = "listings_r", Value = false },
        new AuthScope() { Key = "shops_r", Value = false },
        new AuthScope() { Key = "transactions_r", Value = false },
    };

    public Auth(IMarketConnection thisConn)
    {
        InitializeComponent();
        myConn = thisConn;
        txtCodeVer.Text = myConn.Auth.GetRandomString(64);
        txtState.Text = myConn.Auth.GetRandomString(24);
        clbScopes.ItemsSource = authScopes;
        authScopes.CollectionChanged += UpdateRequestUrl;
    }

    private void txtRedirect_TextChanged(object sender, EventArgs e)
    {
        NameValueCollection myParams = HttpUtility.ParseQueryString(txtRedirect.Text);
        txtRetState.Text = myParams["state"];
        txtAuthCode.Text = myParams["code"];
    }

    private void txtRetState_TextChanged(object sender, EventArgs e)
    {
        if (txtRetState.Text.Length > 1)
        {
            if (txtRetState.Text.Equals(txtState.Text))
            {
                txtRetState.Background = Brushes.LightGreen;
            }
            else
            {
                txtRetState.Background = Brushes.LightPink;
            }
        }
        else
        {
            txtRetState.Background = Brushes.LightGray;
        }
    }
    
    private void btnCopyAuthReqUrl_Click(object sender, EventArgs e)
    {
        UpdateRequestUrl();
        Clipboard.SetText(txtAuthUrl.Text);
    }
    
    private void btnPasteRedirectUri_Click(object sender, EventArgs e)
    {
        if (!Clipboard.ContainsText()) { return; }
        txtRedirect.Text = Clipboard.GetText();
    }
    
    private void btnSaveTokens_Click(object sender, EventArgs e)
    {
        myConn.AuthBox.access_token = txtAccToken.Text;
        myConn.AuthBox.refresh_token = txtRefToken.Text;
        myConn.SaveAuthData();
    }

    private void btnReqToken_Click(object sender, EventArgs e)
    {
        Debug.Print("Requesting access token...");
        AccessToken myToken = myConn.Auth.RequestAccessToken(txtAuthCode.Text, txtCodeVer.Text);
        if (myToken.Error != null && myToken.Error.Length > 0)
        {
            MessageBox.Show($"Error: {myToken.Error} \n"
                            + $"Description: {myToken.ErrorDescription}\n"
                            + $"URI: {myToken.ErrorUri}\n");
        }
        else
        {
            txtAccToken.Text = myToken.access_token;
            txtRefToken.Text = myToken.refresh_token;
        }
    }

    private void UpdateRequestUrl()
    {
        string[] myScopes = (from AuthScope x 
                in authScopes where x.Value
            select x.Key).ToArray();
        
        txtAuthUrl.Text = myConn.Auth.GetAuthorizationUrl(txtCodeVer.Text, 
            txtState.Text, myScopes);
    }
    
    private void UpdateRequestUrl(object? o, NotifyCollectionChangedEventArgs? n)
    {
        UpdateRequestUrl();
    }

    private void UpdateRequestUrl(object thisSender, RoutedEventArgs thisE)
    {
        UpdateRequestUrl();
    }
}