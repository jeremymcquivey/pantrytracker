using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PantryTrackers.Common;
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

        public ObservableCollection<PageTypeGroup<GroceryListItem>> ListItems { get; } =
            new ObservableCollection<PageTypeGroup<GroceryListItem>>();
        
        public Command RefreshDataCommand => _refreshDataCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            // TODO: Eventually, when we support multiple shopping lists, this will be the list requested.
            // For now, though, this is the user Id.
            var listId = await SecureStorage.GetAsync(ClaimKeys.Id);
            await Task.Run(async () =>
            {
                var list = await _groceryList.GetList(listId);

                if(list == null)
                {
                    return;
                }

                var groupedList = list.Where(item => item.Status != GroceryListItemStatus.Archived)
                                      .OrderBy(item => item.Status)
                                      .GroupBy(item => item.Status)
                                      .Select(group => new PageTypeGroup<GroceryListItem>(group)
                                        {
                                            Title = $"{group.Key}",
                                            ShortName = $"{group.Key}",
                                        });

                Device.BeginInvokeOnMainThread(() =>
                {
                    IsNetworkBusy = false;
                    ListItems.Clear();
                    foreach (var group in groupedList)
                    {
                        ListItems.Add(group);
                    }
                });
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

                    var activeGroup = ListItems.FirstOrDefault(group => group.Title == $"{GroceryListItemStatus.Active}");
                    var inactiveGroup = ListItems.FirstOrDefault(group => group.Title == $"{GroceryListItemStatus.Purchased}");
                    var itemIndex = activeGroup?.IndexOf(item);

                    if (activeGroup != default && itemIndex.HasValue)
                    {
                        activeGroup.RemoveAt(itemIndex.Value);
                        if (inactiveGroup == default)
                        {
                            ListItems.Add(new PageTypeGroup<GroceryListItem>(new List<GroceryListItem>())
                            {
                                Title = $"{GroceryListItemStatus.Purchased}",
                                ShortName = $"{GroceryListItemStatus.Purchased}"
                            });
                        }
                        inactiveGroup.Add(newItem);
                    }
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
