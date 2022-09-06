using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using System.Net.Mime;
using ShopTools.Interfaces;

namespace ShopTools.Reports;

public partial class ListingDetail : System.Windows.Window
{
    private IMarketListing myListing;

    public ListingDetail(IMarketListing thisListing)
    {
        InitializeComponent();
        myListing = thisListing;
        this.Title = myListing.Title;
        this.DataContext = myListing;
    }
}