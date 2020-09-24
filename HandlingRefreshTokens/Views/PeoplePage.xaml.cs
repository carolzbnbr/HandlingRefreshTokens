using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HandlingRefreshTokens.Models;
using HandlingRefreshTokens.Views;
using HandlingRefreshTokens.ViewModels;

namespace HandlingRefreshTokens.Views
{
    public partial class PeoplePage : ContentPage
    {
        PeopleViewModel _viewModel;

        public PeoplePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new PeopleViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}