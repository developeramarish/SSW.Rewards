﻿using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SSW.Consulting.PopupPages
{
    public partial class JoinUs : PopupPage
    {
        public JoinUs()
        {
            InitializeComponent();
        }

        private async void FindoutMore_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.ssw.com.au/ssw/Employment/default.aspx", BrowserLaunchMode.SystemPreferred);
        }
    }
}
