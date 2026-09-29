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
    [QueryProperty(nameof(SelectedMinValueGarmentID), "minValueGarmentID")]
    [QueryProperty(nameof(SelectedGroup), "selectedGroup")]
    [QueryProperty(nameof(CurrentConventionalUnitValue), "currentConventionalUnitValue")]
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

        private readonly ReceiptWriteService _receiptWriteService = new();

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
        private int selectedMinValueGarmentID;

        [ObservableProperty]
        private int selectedGroup;

        [ObservableProperty]
        private MinValueGarment currentMinValueGarment;

        [ObservableProperty]
        private int currentConventionalUnitValue;

        [ObservableProperty]
        private string displayMinValueGarmentName;

        [ObservableProperty]
        private bool complicatedElementSaved;

        [ObservableProperty]
        private bool discountSaved;

        private int _isSaving = 0;

        [ObservableProperty]
        private Multiplier currentMultiplier;

        [ObservableProperty]
        private ComplicatedElement selectedComplicatedElement;

        [ObservableProperty]
        private ReceiptComplicatedElement selectedReceiptComplicatedElement;

        [ObservableProperty]
        private Discount selectedDiscount;

        [ObservableProperty]
        private ReceiptDiscount selectedReceiptDiscount;

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

        async partial void OnSelectedMinValueGarmentIDChanged(int value)
        {
            await MinValueGarmentChanged();
        }

        async partial void OnSelectedGroupChanged(int value)
        {
            await MinValueGarmentChanged();
        }

        async partial void OnCurrentConventionalUnitValueChanged(int value)
        {
            IsComplElementButtonVisable = value != 0 && DisplayMinValueGarmentName is not null ? true : false;
            if(CurrentReceipt != null)
            {
                CurrentReceipt.ConventionalUnitValue = value;
            }
            await SaveReceipt();
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
        public async Task MinValueGarmentChanged()
        {
            if (SelectedMinValueGarmentID != 0 && SelectedGroup != 0)
            {
                try
                {
                    if (CurrentReceipt == null)
                    {
                        CurrentReceipt = new Receipt();
                    }
                    CurrentReceipt.MinValueGarmentID = SelectedMinValueGarmentID;
                    CurrentReceipt.MaterialGroupID = SelectedGroup;
                    CurrentMinValueGarment = await _minValueGarmentService.GetItemAsync(SelectedMinValueGarmentID);
                    DisplayMinValueGarmentName = CurrentMinValueGarment.Name;
                    await SaveReceipt();
                    IsComplElementButtonVisable = CurrentConventionalUnitValue != 0 ? true : false;
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
            if (ReceiptDetails != null && CurrentReceipt == null)
            {
                CurrentReceipt = ReceiptDetails.Receipt;

                CurrentMinValueGarment = ReceiptDetails.MinValueGarment;

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
                        if (CurrentMinValueGarment != null)
                        {
                            CurrentReceipt.DiscountValue = 0;

                            CurrentMultiplier = await _multiplierService.GetItemAsync(CurrentMinValueGarment.MultiplierID);

                            double currentCoefficient = 0;

                            switch (CurrentReceipt.MaterialGroupID)
                            {
                                case 1:
                                    {
                                        currentCoefficient = CurrentMultiplier.FirstGroup;
                                        CurrentReceipt.MinValue = CurrentMinValueGarment.RatioFirst * CurrentReceipt.ConventionalUnitValue;
                                        CurrentReceipt.DiscountValue -= 20;
                                    }
                                    break;
                                case 2:
                                    {
                                        currentCoefficient = CurrentMultiplier.FirstGroup;
                                        CurrentReceipt.MinValue = CurrentMinValueGarment.RatioFirst * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 3:
                                    {
                                        currentCoefficient = CurrentMultiplier.SecondGroup;
                                        CurrentReceipt.MinValue = CurrentMinValueGarment.RatioSecond * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 4:
                                    {
                                        currentCoefficient = CurrentMultiplier.ThirdGroup;
                                        CurrentReceipt.MinValue = CurrentMinValueGarment.RatioThird * CurrentReceipt.ConventionalUnitValue;
                                    }
                                    break;
                                case 5:
                                    {
                                        currentCoefficient = CurrentMultiplier.FourthGroup;
                                        CurrentReceipt.MinValue = CurrentMinValueGarment.RatioFourth * CurrentReceipt.ConventionalUnitValue;
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


                            await _receiptDiscountService.GetItemsAsync(ReceiptID);
                            if (_receiptDiscountService.ReceiptDiscounts != null)
                            {
                                Discounts.Clear();
                                CurrentReceipt.DiscountValue = 0;
                                int discountsMerit = 0;
                                foreach (var element in _receiptDiscountService.ReceiptDiscounts)
                                {
                                    Discount discount = await _discountService.GetItemAsync(element.DiscountID);
                                    Discounts.Add(discount);
                                    discountsMerit += discount.Merit;
                                }
                                CurrentReceipt.DiscountValue = (discountsMerit * CurrentReceipt.TotalValue) / 100;
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
        public async Task WriteReceipt()
        {
            if (CurrentReceipt != null)
            {
                string receiptDocxFilePath = await _receiptWriteService.WriteReceipt(CurrentReceipt);
                if (receiptDocxFilePath != null)
                {
                    bool b = await _dialogService.ShowAlertAsync("Файл сохранён", "Хотите поделится чеком?", "Да", "Нет");
                    if (b)
                    {
                        await Share.Default.RequestAsync(new ShareFileRequest
                        {
                            Title = "Поделиться документом",
                            File = new ShareFile(receiptDocxFilePath)
                        });
                    }
                }
            }
        }

        [RelayCommand]
        private async Task SaveAndExit()
        {
            await SaveReceipt();
            await _navigationService.GoToRootAsync();
        }

        [RelayCommand]
        private async Task DeleteComplicatedElement(ComplicatedElement selectedComplicatedElement)
        {
            if (selectedComplicatedElement != null)
            {
                SelectedReceiptComplicatedElement = await _receiptComplicatedElementService.GetItemAsync(selectedComplicatedElement.ID);
                await _receiptComplicatedElementService.DeleteItemAsync(SelectedReceiptComplicatedElement);
                await SaveReceipt();
            }
        }

        [RelayCommand]
        private async Task DeleteDiscount(Discount selectedDiscount)
        {
            if (selectedDiscount != null)
            {
                SelectedReceiptDiscount = await _receiptDiscountService.GetItemAsync(selectedDiscount.ID);
                await _receiptDiscountService.DeleteItemAsync(SelectedReceiptDiscount);
                await SaveReceipt();
            }
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
            if (CurrentReceipt != null && CurrentMultiplier != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["receiptID"] = CurrentReceipt.ID,
                    ["multiplierID"] = CurrentMultiplier.ID
                };
                await _navigationService.GoToAsync("ComplicatedElementsPage", parameters);
            }
        }

        [RelayCommand]
        private async Task GoToDiscounts()
        {
            if (CurrentReceipt != null && CurrentMultiplier != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["receiptID"] = CurrentReceipt.ID,
                    ["multiplierID"] = CurrentMultiplier.ID
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