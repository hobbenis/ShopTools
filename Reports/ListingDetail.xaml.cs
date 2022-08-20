using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using ShopTools.Etsy;

namespace ShopTools.Reports;

public partial class ListingDetail : System.Windows.Window
{
    private Listing myListing;

    public ListingDetail(Listing thisListing)
    {
        myListing = thisListing;

        InitializeComponent();
    }
}