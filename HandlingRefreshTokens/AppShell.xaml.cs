using System;
using System.Collections.Generic;
using HandlingRefreshTokens.ViewModels;
using HandlingRefreshTokens.Views;
using Xamarin.Forms;

namespace HandlingRefreshTokens
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        }

    }
}
