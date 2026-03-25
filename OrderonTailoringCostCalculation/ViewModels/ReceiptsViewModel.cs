using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;

namespace OrderonTailoringCostCalculation.ViewModels
{
    public partial class ReceiptsViewModel : ObservableObject
    {
        private readonly ReceiptService receiptService;

        private readonly INavigationService _navigationService;

        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private ObservableCollection<ReceiptListItemViewModel> receipts;

        [ObservableProperty]
        private ReceiptListItemViewModel selectedReceipt;

        public ReceiptsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            receiptService = new ReceiptService();
            receipts = new ObservableCollection<ReceiptListItemViewModel>();
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            await receiptService.GetItemsAsync();
            Receipts.Clear();
            MinValueGarmentService minValueGarmentService = new MinValueGarmentService();
            foreach (var receipt in receiptService.Receipts)
            {
                MinValueGarment minValueGarment =  await minValueGarmentService.GetItemAsync(receipt.MinValueGarmentID);
                Receipts.Add(new ReceiptListItemViewModel(receipt, minValueGarment.Name));
            }
        }

        [RelayCommand]
        private async Task GoToReceipt()
        {
            await _navigationService.GoToAsync("ReceiptPage");
        }

        [RelayCommand]
        private async Task GoToReceiptWithParam(ReceiptListItemViewModel selectedReceipt)
        {
            if (selectedReceipt == null) 
            {
                await _dialogService.ShowAlertAsync("Ошибка", "Не выбран чек\nПожалуйста, выберите чек из списка или создайте новый", "OK");
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                ["id"] = selectedReceipt.id
            };
            await _navigationService.GoToAsync("ReceiptPage", parameters);
        }
    }
}