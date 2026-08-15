using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;
using OrderonTailoringCostCalculation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private readonly MinValueGarmentService _minValueGarmentService = new();

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
        private int currentConventionalUnitValue;

        [ObservableProperty]
        private string displayMinValueGarmentName;

        [ObservableProperty]
        private bool complicatedElementSaved;

        [ObservableProperty]
        private bool discountSaved;

        private int _isSaving = 0;

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

        async partial void OnCurrentConventionalUnitValueChanged(int value)
        {
            IsComplElementButtonVisable = value != 0 && DisplayMinValueGarmentName is not null ? true : false;
            if(CurrentReceipt != null)
            {
                CurrentReceipt.ConventionalUnitValue = value;
            }
        }

        async partial void OnSelectedMinValueGarmentIDWithSelectedGroupChanged(MinValueGarmentIDWithSelectedGroup value)
        {
            if (value == null || (value.MinValueGarmentID == 0 && value.SelectedGroup == 0 && value.ConventionalUnitValue == 0))
            {
                return;
            }
            try
            {
                if (CurrentReceipt == null)
                {
                    CurrentReceipt = new Receipt();
                }
                CurrentReceipt.MinValueGarmentID = value.MinValueGarmentID;
                CurrentReceipt.MaterialGroupID = value.SelectedGroup;
                CurrentReceipt.ConventionalUnitValue = value.ConventionalUnitValue;
                MinValueGarment currentMinValueGarment = await _minValueGarmentService.GetItemAsync(value.MinValueGarmentID);
                DisplayMinValueGarmentName = currentMinValueGarment.Name;
                CurrentConventionalUnitValue = value.ConventionalUnitValue;
                OnCurrentConventionalUnitValueChanged(value.ConventionalUnitValue);
                await SaveReceipt();

                SelectedMinValueGarmentIDWithSelectedGroup.MinValueGarmentID = 0;
                selectedMinValueGarmentIDWithSelectedGroup.SelectedGroup = 0;
                selectedMinValueGarmentIDWithSelectedGroup.ConventionalUnitValue = 0;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync(ex.Source ?? "Ошибка данных", ex.Message, "OK");
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

                CurrentConventionalUnitValue = CurrentReceipt.ConventionalUnitValue;
            }
        }

        [RelayCommand]
        public async Task SaveReceipt()
        {
            if (Interlocked.CompareExchange(ref _isSaving, 1, 0) != 0)
            {
                Debug.WriteLine("SaveReceipt уже выполняется, пропускаем.");
                return;
            }

            try
            {
                if (CurrentReceipt != null)
                {
                    if (CurrentConventionalUnitValue != 0 && CurrentReceipt.MinValueGarmentID != 0)
                    {
                        ReceiptID = await _receiptService.SaveItemAsync(CurrentReceipt);
                        CurrentReceipt.ConventionalUnitValue = CurrentConventionalUnitValue;
                        if (ReceiptDetails != null)
                        {
                            CurrentReceipt.DiscountValue = 0;

                            Multiplier multiplier = await _multiplierService.GetItemAsync(ReceiptDetails.MinValueGarment.MultiplierID);

                            double currentCoefficient = 0;

                            switch (CurrentReceipt.MaterialGroupID)
                            {
                                case 1:
                                    {
                                        currentCoefficient = multiplier.FirstGroup;
                                        CurrentReceipt.MinValue = ReceiptDetails.MinValueGarment.RatioFirst * CurrentReceipt.ConventionalUnitValue;
                                        CurrentReceipt.DiscountValue -= 20;
                                    }
                                    break;
                                case 2:
                                    {
                                        currentCoefficient = multiplier.FirstGroup;
                                        CurrentReceipt.MinValue = ReceiptDetails.MinValueGarment.RatioFirst * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 3:
                                    {
                                        currentCoefficient = multiplier.SecondGroup;
                                        CurrentReceipt.MinValue = ReceiptDetails.MinValueGarment.RatioSecond * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 4:
                                    {
                                        currentCoefficient = multiplier.ThirdGroup;
                                        CurrentReceipt.MinValue = ReceiptDetails.MinValueGarment.RatioThird * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 5:
                                    {
                                        currentCoefficient = multiplier.FourthGroup;
                                        CurrentReceipt.MinValue = ReceiptDetails.MinValueGarment.RatioFourth * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                            }


                            await _receiptComplicatedElementService.GetItemsAsync(ReceiptID);
                            if (_receiptComplicatedElementService.ReceiptComplicatedElements != null)
                            {
                                ComplicatedElements.Clear();
                                CurrentReceipt.ComplicatedElementsValue = 0;
                                double complicatedElementsCount = 0;
                                foreach (var element in _receiptComplicatedElementService.ReceiptComplicatedElements)
                                {
                                    ComplicatedElement complicatedElement = await _complicatedElementService.GetItemAsync(element.ComplicatedElementID);
                                    ComplicatedElements.Add(complicatedElement);
                                    complicatedElementsCount += complicatedElement.Merit;
                                }
                                CurrentReceipt.ComplicatedElementsValue = ((complicatedElementsCount * currentCoefficient) * CurrentReceipt.ConventionalUnitValue);
                            }

                            CurrentReceipt.TotalValue = CurrentReceipt.MinValue + CurrentReceipt.ComplicatedElementsValue;

                            List<ReceiptDiscount> receiptDiscounts = await _receiptDiscountService.GetItemsAsync(ReceiptID);
                            if (receiptDiscounts != null)
                            {
                                foreach (var discount in receiptDiscounts)
                                {
                                    var currentDiscount = await _discountService.GetItemAsync(discount.DiscountID);
                                    CurrentReceipt.DiscountValue += currentDiscount.Merit;
                                }
                            }
                            if (CurrentReceipt.DiscountValue != 0)
                            {
                                CurrentReceipt.DiscountValue = ((CurrentReceipt.DiscountValue * -1) * CurrentReceipt.TotalValue) / 100;
                            }

                            CurrentReceipt.TotalValue += CurrentReceipt.DiscountValue;

                            ReceiptID = await _receiptService.SaveItemAsync(CurrentReceipt);
                            await LoadReceiptAsync(ReceiptID);
                        }
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isSaving, 0);
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
            var parameters = new Dictionary<string, object>
            {
                ["currentConventionalUnitValue"] = CurrentConventionalUnitValue
            };
            await _navigationService.GoToAsync("MinValueGarmentsPage", parameters);
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