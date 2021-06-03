using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PantryTrackers.Common.Security;
using PantryTrackers.Models.GroceryList;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.GroceryList
{
    public class GroceryListMainPageViewModel: ViewModelBase
    {
        private Command _refreshDataCommand;
        private Command<GroceryListItem> _markItemAsPurchasedCommand;

        private GroceryListService _groceryList;
        private INavigationService _navigationService;

        public ObservableCollection<GroceryListItem> ListItems { get; } =
            new ObservableCollection<GroceryListItem>();

        public Command RefreshDataCommand => _refreshDataCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            // TODO: Eventually, when we support multiple shopping lists, this will be the list requested.
            // For now, though, this is the user Id.
            var listId = await SecureStorage.GetAsync(ClaimKeys.Id);
            await Task.Run(async () =>
            {
                var list = await _groceryList.GetList(listId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsNetworkBusy = false;
                });

                if(list == null)
                {
                    return;
                }

                ListItems.Clear();
                foreach(var item in list.Where(item => item.Status == GroceryListItemStatus.Active || item.Status == GroceryListItemStatus.Purchased))
                {
                    ListItems.Add(item);
                }
            });
        }, CanExecute);

        public Command<GroceryListItem> MarkItemAsPurchasedCommand => _markItemAsPurchasedCommand ??= new Command<GroceryListItem>(async(item) =>
        {
            if(item == null)
            {
                return;
            }

            IsNetworkBusy = true;
            var listId = await SecureStorage.GetAsync(ClaimKeys.Id);
            await Task.Run(async () =>
            {
                item.Status = GroceryListItemStatus.Purchased;
                item.PurchaseDate = DateTime.Now;
                var newItem = await _groceryList.SaveItem(listId, item) ?? item;
                
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsNetworkBusy = false;
                    var index = ListItems.IndexOf(item);
                    ListItems.RemoveAt(index);
                    ListItems.Insert(index, newItem);
                });
            });
        }, CanExecute);

        public GroceryListMainPageViewModel(INavigationService navigationService, GroceryListService groceryList) :
               base(navigationService, null)
        {
            _groceryList = groceryList;
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(RefreshDataCommand.CanExecute(null))
            {
                RefreshDataCommand.Execute(null);
            }

            base.OnNavigatedTo(parameters);
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();
            RefreshDataCommand.ChangeCanExecute();
        }
    }
}
