using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using HandlingRefreshTokens.Models;
using HandlingRefreshTokens.Views;
using HandlingRefreshTokens.Services;

namespace HandlingRefreshTokens.ViewModels
{
    public class PeopleViewModel : BaseViewModel
    {
       

        public ObservableCollection<PersonInfo> Items { get; }
        public Command LoadItemsCommand { get; }
       
        public IPeopleService PoepleService => DependencyService.Get<IPeopleService>();

        public PeopleViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<PersonInfo>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);

           
        }

        private async void ExecuteLoadItemsCommand(object obj)
        {
            IsBusy = true;

            try
            {
                Items.Clear();

                var task1 = PoepleService.GetPeopleAsync();
                var task2 = PoepleService.GetPeopleAsync();
                var task3 = PoepleService.GetPeopleAsync();
                var task4 = PoepleService.GetPeopleAsync();

                foreach (var person in await task2)
                {
                    Items.Add(person);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

      
        public void OnAppearing()
        {
            IsBusy = true;
        }

      
    }
}