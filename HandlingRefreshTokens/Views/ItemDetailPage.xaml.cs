using System.ComponentModel;
using Xamarin.Forms;
using HandlingRefreshTokens.ViewModels;

namespace HandlingRefreshTokens.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new PersonItemViewModel();
        }
    }
}