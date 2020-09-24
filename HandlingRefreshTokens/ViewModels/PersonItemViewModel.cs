using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HandlingRefreshTokens.Models;
using Xamarin.Forms;

namespace HandlingRefreshTokens.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class PersonItemViewModel : BaseViewModel
    {
      
        private string description;
        private string id;

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string FullName
        {
            get => description;
            set => SetProperty(ref description, value);
        }


      
    }
}
