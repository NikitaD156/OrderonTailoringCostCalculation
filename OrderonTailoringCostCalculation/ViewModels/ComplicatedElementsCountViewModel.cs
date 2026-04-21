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
    [QueryProperty(nameof(SelectedComplicatedElementID), "complicatedElementID")]
    public partial class ComplicatedElementsCountViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        private readonly IDialogService _dialogService;

        private readonly ComplicatedElementService _complicatedElementService;

        private readonly ReceiptComplicatedElementService _receiptComplicatedElementService;


        [ObservableProperty]
        private int selectedComplicatedElementID;

        [ObservableProperty]
        private string displayComplicatedElementName;

        [ObservableProperty]
        private int currentElementsCount;

        [ObservableProperty]
        private int receiptID;

        public ComplicatedElementsCountViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _receiptComplicatedElementService = new ReceiptComplicatedElementService();
            _complicatedElementService = new ComplicatedElementService();
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        async Task CountSelected()
        {
            if (CurrentElementsCount != 0)
            {
                ReceiptComplicatedElement currentReceiptComplicatedElement = new ReceiptComplicatedElement();
                currentReceiptComplicatedElement.ReceiptID = ReceiptID;
                currentReceiptComplicatedElement.ComplicatedElementID = SelectedComplicatedElementID;
                currentReceiptComplicatedElement.Count = CurrentElementsCount;

                await _receiptComplicatedElementService.SaveItemAsync(currentReceiptComplicatedElement);

                bool saved = true;

                var parameters = new Dictionary<string, object>
                {
                    ["complicatedElementSaved"] = saved
                };
                await _navigationService.GoToAsync("../..", parameters);
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

        async partial void OnSelectedComplicatedElementIDChanged(int value)
        {
            ComplicatedElement selectedElement = await _complicatedElementService.GetItemAsync(value);
            DisplayComplicatedElementName = selectedElement.Name + " " + selectedElement.Description;
        }
    }
}
