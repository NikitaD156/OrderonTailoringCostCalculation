using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;
using OrderonTailoringCostCalculation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static SQLite.SQLite3;

namespace OrderonTailoringCostCalculation.ViewModels
{
    [QueryProperty(nameof(ReceiptID), "id")]
    public partial class ReceiptDetailsViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;

        private readonly IDialogService _dialogService;

        private readonly ReceiptService _receiptService = new();

        private Receipt _currentReceipt;

        [ObservableProperty]
        private MinValueGarment _currentMinValueGarment;

        [ObservableProperty]
        private int receiptID;

        [ObservableProperty]
        private string displayMinValueGarmentName;

        public ReceiptDetailsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        async partial void OnReceiptIDChanged(int value)
        {
            if (value != 0)
            {
                try
                {
                    await LoadReceiptAsync(value);
                }
                catch (Exception ex)
                {
                    await _dialogService.ShowAlertAsync(ex.Source ?? "Ошибка данных", ex.Message, "OK");
                }
            }
            else
            {
                return;
            }
        }

        async partial void OnCurrentMinValueGarmentChanged(MinValueGarment value)
        {
            if (value != null)
            {
                try
                {
                    _currentReceipt.MinValueGarmentID = value.ID;
                    await SaveReceipt();
                }
                catch (Exception ex)
                {
                    await _dialogService.ShowAlertAsync(ex.Source ?? "Ошибка данных", ex.Message, "OK");
                }
            }
            else
            {
                return;
            }
        }

        [RelayCommand]
        public async Task LoadReceiptAsync(int id)
        {
            var allReceipt = await _receiptService.GetReceiptWithGarmentAsync(id);
            if (allReceipt != null)
            {
                _currentReceipt = allReceipt.Receipt;
                DisplayMinValueGarmentName = allReceipt.MinValueGarment?.Name;
                await SaveReceipt();
            }
        }

        [RelayCommand]
        public async Task SaveReceipt()
        {
            if (_currentReceipt != null)
            {
                await _receiptService.SaveItemAsync(_currentReceipt);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Необходима основа", "Для сохранения выберите основу изделия", "OK");
                
            }
        }

        [RelayCommand]
        private async Task GoToGarments()
        {
            
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await _navigationService.GoBackAsync();
        }
    }
}