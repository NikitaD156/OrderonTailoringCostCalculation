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
        private ComplicatedElement selectedComplicatedElement;

        public ComplicatedElementsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _receiptComplicatedElementService = new ReceiptComplicatedElementService();
            _complicatedElementService = new ComplicatedElementService();
            complicatedElements = new ObservableCollection<ComplicatedElement>();
            _navigationService = navigationService;
            _dialogService = dialogService;
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

        [RelayCommand]
        public async Task LoadAsync()
        {
            await _complicatedElementService.GetItemsAsync();
            ComplicatedElements.Clear();
            foreach (var complicatedElement in _complicatedElementService.ComplicatedElements)
            {
                ComplicatedElements.Add(complicatedElement);
            }
        }
    }
}
