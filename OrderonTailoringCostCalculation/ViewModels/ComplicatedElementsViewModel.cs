using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;
using System.Collections.ObjectModel;

namespace OrderonTailoringCostCalculation.ViewModels
{
    [QueryProperty(nameof(ReceiptID), "receiptID")]
    [QueryProperty(nameof(MultiplierID), "multiplierID")]
    public partial class ComplicatedElementsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private readonly IDialogService _dialogService;

        private readonly ComplicatedElementService _complicatedElementService;

        private readonly ReceiptComplicatedElementService _receiptComplicatedElementService;

        [ObservableProperty]
        private ObservableCollection<ComplicatedElement> complicatedElements;

        [ObservableProperty]
        private int receiptID;

        [ObservableProperty]
        private int multiplierID;

        [ObservableProperty]
        private ComplicatedElement selectedComplicatedElement;

        [ObservableProperty]
        private string searchText;

        public ComplicatedElementsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _receiptComplicatedElementService = new ReceiptComplicatedElementService();
            _complicatedElementService = new ComplicatedElementService();
            complicatedElements = new ObservableCollection<ComplicatedElement>();
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        async partial void OnReceiptIDChanged(int value)
        {
            if(value != 0)
            {
                await LoadAsync();
            }
        }

        async partial void OnMultiplierIDChanged(int value)
        {
            if(value != 0)
            {
                await LoadAsync();
            }
        }

        [RelayCommand]
        async Task SelectedElementChanged(ComplicatedElement value)
        {
            if (value != null)
            {
                ReceiptComplicatedElement currentReceiptComplicatedElement = new ReceiptComplicatedElement();
                currentReceiptComplicatedElement.ReceiptID = ReceiptID;
                currentReceiptComplicatedElement.ComplicatedElementID = value.ID;

                await _receiptComplicatedElementService.SaveItemAsync(currentReceiptComplicatedElement);

                bool saved = true;

                var parameters = new Dictionary<string, object>
                {
                    ["complicatedElementSaved"] = saved
                };
                await _navigationService.GoToAsync("..", parameters);
            }
            else
            {
                bool b = await _dialogService.ShowAlertAsync("Количество Элементов = 0", "Да", "Вернутся к выбору элемента?", "Нет");
                if (b)
                {
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    return;
                }
            }
        }

        async partial void OnSearchTextChanged(string value)
        {
            if(value != null)
            {
                await SearchElement();
            }
        }

        [RelayCommand]
        public async Task SearchElement()
        {
            await _complicatedElementService.GetItemsAsync();
            ComplicatedElements.Clear();
            foreach (var complicatedElement in _complicatedElementService.ComplicatedElements)
            {
                if (complicatedElement.Name.ToLower().Contains(SearchText.ToLower()) || complicatedElement.Description.ToLower().Contains(SearchText.ToLower()))
                {
                    ComplicatedElements.Add(complicatedElement);
                }
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (ReceiptID != 0 && MultiplierID != 0)
            {
                await _complicatedElementService.GetItemsAsync();
                ComplicatedElements.Clear();
                foreach (var complicatedElement in _complicatedElementService.ComplicatedElements)
                {
                    if (MultiplierID == complicatedElement.MultiplierID)
                    {
                        ComplicatedElements.Add(complicatedElement);
                    }
                }
            }
        }
    }
}
