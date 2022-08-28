using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using System.Net.Mime;
using ShopTools.Data.Etsy;

namespace ShopTools.Reports;

public partial class ListingDetail : System.Windows.Window
{
    private EtsyListing myListing;

    public ListingDetail(EtsyListing thisListing)
    {
        InitializeComponent();
        myListing = thisListing;
        this.Title = myListing.title;
        this.DataContext = myListing;
    }
}