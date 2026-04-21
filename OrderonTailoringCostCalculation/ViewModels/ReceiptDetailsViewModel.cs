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
    [QueryProperty(nameof(SelectedMinValueGarmentIDWithSelectedGroup), "garmentIDandGroup")]
    [QueryProperty(nameof(ComplicatedElementSaved), "complicatedElementSaved")]
    [QueryProperty(nameof(DiscountSaved), "discountSaved")]
    public partial class ReceiptDetailsViewModel: ObservableObject
    {
        private readonly INavigationService _navigationService;

        private readonly IDialogService _dialogService;

        private readonly ReceiptService _receiptService = new();

        private readonly ReceiptComplicatedElementService _receiptComplicatedElementService = new();

        private readonly ComplicatedElementService _complicatedElementService = new();

        private readonly ReceiptDiscountService _receiptDiscountService = new();

        private readonly DiscountService _discountService = new();

        private readonly MultiplierService _multiplierService = new();

        [ObservableProperty]
        private ObservableCollection<ComplicatedElement> complicatedElements = new();

        [ObservableProperty]
        private ObservableCollection<Discount> discounts = new();

        [ObservableProperty]
        private ReceiptDetails _receiptDetails;

        [ObservableProperty]
        private bool isComplElementButtonVisable = false;

        [ObservableProperty]
        private Receipt _currentReceipt;

        [ObservableProperty]
        private int receiptID;

        [ObservableProperty]
        private MinValueGarmentIDWithSelectedGroup selectedMinValueGarmentIDWithSelectedGroup;


        [ObservableProperty]
        private string displayMinValueGarmentName;

        [ObservableProperty]
        private bool complicatedElementSaved;

        [ObservableProperty]
        private bool discountSaved;

        public record MaterialSelectedMessage(int garmentID, int materialID);
        
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
        async partial void OnSelectedMinValueGarmentIDWithSelectedGroupChanged(MinValueGarmentIDWithSelectedGroup value)
        {
            if (value != null)
            {
                try
                {
                    if (CurrentReceipt != null)
                    {
                        CurrentReceipt.MinValueGarmentID = value.MinValueGarmentID;
                        CurrentReceipt.MaterialGroupID = value.SelectedGroup;
                        await SaveReceipt();
                    }
                    else
                    {
                        CurrentReceipt = new Receipt();
                        CurrentReceipt.MinValueGarmentID = value.MinValueGarmentID;
                        CurrentReceipt.MaterialGroupID = value.SelectedGroup;
                        await SaveReceipt();
                    }
                    SelectedMinValueGarmentIDWithSelectedGroup.MinValueGarmentID = 0;
                    selectedMinValueGarmentIDWithSelectedGroup.SelectedGroup = 0;
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

        async partial void OnComplicatedElementSavedChanged(bool value)
        {
            if (value)
            {
                try
                {
                    await SaveReceipt();

                    ComplicatedElementSaved = false;
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

        async partial void OnDiscountSavedChanged(bool value)
        {
            if (value)
            {
                try
                {
                    await SaveReceipt();

                    DiscountSaved = false;
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
            ReceiptDetails = await _receiptService.GetReceiptWithGarmentAsync(id);
            if (ReceiptDetails != null)
            {
                CurrentReceipt = ReceiptDetails.Receipt;
                DisplayMinValueGarmentName = ReceiptDetails.MinValueGarment?.Name;
                IsComplElementButtonVisable = CurrentReceipt.MinValueGarmentID == 0 ? false : true;

            }
        }


        [RelayCommand]
        public async Task SaveReceipt()
        {
            if (CurrentReceipt != null)
            {
                ReceiptID = await _receiptService.SaveItemAsync(CurrentReceipt);
                await LoadReceiptAsync(ReceiptID);
                if (ReceiptDetails != null)
                {
                    CurrentReceipt.ConventionalUnitValue = 300;

                    CurrentReceipt.DiscountValue = 0;

                    Multiplier multiplier = await _multiplierService.GetItemAsync(ReceiptDetails.MinValueGarment.MultiplierID); 

                    switch (CurrentReceipt.MaterialGroupID)
                    {
                        case 1:
                            {
                                CurrentReceipt.MinValue = (ReceiptDetails.MinValueGarment.RatioFirst * multiplier.FirstGroup) * CurrentReceipt.ConventionalUnitValue;
                                CurrentReceipt.DiscountValue -= 20;
                                
                            }
                            break;
                        case 2:
                            {
                                CurrentReceipt.MinValue = (ReceiptDetails.MinValueGarment.RatioFirst * multiplier.FirstGroup) * CurrentReceipt.ConventionalUnitValue;
                            }
                            break;
                        case 3:
                            {
                                CurrentReceipt.MinValue = (ReceiptDetails.MinValueGarment.RatioSecond * multiplier.SecondGroup) * CurrentReceipt.ConventionalUnitValue;
                            }
                            break;
                        case 4:
                            {
                                CurrentReceipt.MinValue = (ReceiptDetails.MinValueGarment.RatioThird * multiplier.ThirdGroup) * CurrentReceipt.ConventionalUnitValue;
                            }
                            break;
                        case 5:
                            {
                                CurrentReceipt.MinValue = (ReceiptDetails.MinValueGarment.RatioFourth * multiplier.FourthGroup) * CurrentReceipt.ConventionalUnitValue;
                            }
                            break;
                    }

                    List<ReceiptComplicatedElement> receiptComplicatedElements = await _receiptComplicatedElementService.GetItemsAsync(ReceiptID);
                    if (receiptComplicatedElements != null)
                    {
                        ComplicatedElements.Clear();
                        CurrentReceipt.ComplicatedElementsValue = 0;
                        foreach (var element in receiptComplicatedElements)
                        {
                            ComplicatedElement complicatedElement = await _complicatedElementService.GetItemAsync(element.ComplicatedElementID);
                            ComplicatedElements.Add(complicatedElement);
                            CurrentReceipt.ComplicatedElementsValue += complicatedElement.Merit * element.Count;
                        }
                        CurrentReceipt.ComplicatedElementsValue = CurrentReceipt.ComplicatedElementsValue * CurrentReceipt.ConventionalUnitValue;
                    }

                    CurrentReceipt.TotalValue = CurrentReceipt.MinValue + CurrentReceipt.ComplicatedElementsValue;

                    List<ReceiptDiscount> receiptDiscounts = await _receiptDiscountService.GetItemsAsync(ReceiptID);
                    if (receiptDiscounts != null)
                    {
                        foreach(var discount in receiptDiscounts)
                        {
                            var currentDiscount = await _discountService.GetItemAsync(discount.DiscountID);
                            CurrentReceipt.DiscountValue += currentDiscount.Merit;
                        }
                    }
                    if(CurrentReceipt.DiscountValue != 0)
                    {
                        CurrentReceipt.DiscountValue = ((CurrentReceipt.DiscountValue * -1) * CurrentReceipt.TotalValue) / 100;
                    }

                    CurrentReceipt.TotalValue += CurrentReceipt.DiscountValue;

                    ReceiptID = await _receiptService.SaveItemAsync(CurrentReceipt);
                    await LoadReceiptAsync(ReceiptID);
                }
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if(ReceiptID != 0)
            {
                await LoadReceiptAsync(ReceiptID);
            }
            await SaveReceipt();
        }


        [RelayCommand]
        private async Task SaveAndExit()
        {
            await SaveReceipt();
            await _navigationService.GoToRootAsync();
        }

        [RelayCommand]
        private async Task GoToGarments()
        {
            await _navigationService.GoToAsync("MinValueGarmentsPage");
        }

        [RelayCommand]
        private async Task GoToComplicatedElements()
        {
            if (CurrentReceipt != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["receiptID"] = CurrentReceipt.ID
                };
                await _navigationService.GoToAsync("ComplicatedElementsPage", parameters);
            }
        }

        [RelayCommand]
        private async Task GoToDiscounts()
        {
            if (CurrentReceipt != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["receiptID"] = CurrentReceipt.ID
                };
                await _navigationService.GoToAsync("DiscountsPage", parameters);
            }
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await _navigationService.GoToRootAsync();
        }
    }
}