using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PantryTrackers.Common;
using PantryTrackers.Common.Security;
using PantryTrackers.Models;
using PantryTrackers.Models.GroceryList;
using PantryTrackers.Services;
using PantryTrackers.Views.GroceryList;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.GroceryList
{
    public class GroceryListMainPageViewModel: ViewModelBase
    {
        private Command _refreshDataCommand;
        private Command<GroceryListItem> _markItemAsPurchasedCommand;
        private Command<GroceryListItem> _saveNewListEntryCommand;
        private Command<GroceryListItem> _removeItemCommand;
        private Command _addGroceryItemCommand;

        private GroceryListService _groceryList;

        public ObservableCollection<PageTypeGroup<GroceryListItem>> ListItems { get; } =
            new ObservableCollection<PageTypeGroup<GroceryListItem>>();

        public Command AddGroceryItemCommand => _addGroceryItemCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            await NavigationService.NavigateAsync(nameof(AddGroceryListItemPage));
            IsNetworkBusy = false;
        }, CanExecute);

        public Command<GroceryListItem> SaveNewListEntryCommand => _saveNewListEntryCommand ??= new Command<GroceryListItem>(async (newListEntry) =>
        {
            var listId = await SecureStorage.GetAsync(ClaimKeys.Id);
            var newItem = await _groceryList.SaveItem(listId, newListEntry);

            if(newItem?.Id != default)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"{newListEntry.DisplayName} added to list", "OK");
                RefreshDataCommand.Execute(null);
            }

        }, CanExecute);

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

                    if (activeGroup != default && itemIndex.Value >= 0)
                    {
                        activeGroup.RemoveAt(itemIndex.Value);

                        if(activeGroup.Count == 0)
                        {
                            ListItems.Remove(activeGroup);
                        }

                        if (inactiveGroup == default)
                        {
                            ListItems.Add(new PageTypeGroup<GroceryListItem>(new ObservableCollection<GroceryListItem>())
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

        public Command<GroceryListItem> RemoveItemCommand => _removeItemCommand ??= new Command<GroceryListItem>(async (item) =>
        {
            IsNetworkBusy = true;
            var listId = await SecureStorage.GetAsync(ClaimKeys.Id);
            await Task.Run(async () =>
            {
                if(await _groceryList.RemoveItem(listId, item))
                {
                    var group = ListItems.FirstOrDefault(p => p.Contains(item));
                    if(group != default)
                    {
                        group.Remove(item);
                    }
                }

            });

            IsNetworkBusy = false;
        }, CanExecute);

        public GroceryListMainPageViewModel(INavigationService navigationService, GroceryListService groceryList) :
               base(navigationService, null)
        {
            _groceryList = groceryList;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("NewGroceryListEntry") && parameters["NewGroceryListEntry"].GetType() == typeof(GroceryListItem))
            {
                SaveNewListEntryCommand.Execute(parameters["NewGroceryListEntry"]);
            }

            if (parameters.ContainsKey("PantryTransaction") && parameters["PantryTransaction"].GetType() == typeof(PantryTransaction))
            {
                if (SaveNewListEntryCommand.CanExecute(null))
                {
                    var product = parameters["PantryTransaction"] as PantryTransaction;
                    SaveNewListEntryCommand.Execute(new GroceryListItem
                    {
                        ProductId = product.ProductId,
                        DisplayName = product.ProductName,
                        Quantity = 1,
                        Size = 1,
                        Container = product.Container,
                        Status = GroceryListItemStatus.Active,
                        Unit = product.Unit,
                        VarietyId = product.Variety?.Id
                    });
                }
            }

            if (RefreshDataCommand.CanExecute(null))
            {
                RefreshDataCommand.Execute(null);
            }

            base.OnNavigatedTo(parameters);
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();
            RefreshDataCommand.ChangeCanExecute();
            AddGroceryItemCommand.ChangeCanExecute();
            SaveNewListEntryCommand.ChangeCanExecute();
            RemoveItemCommand.ChangeCanExecute();
        }
    }
}
