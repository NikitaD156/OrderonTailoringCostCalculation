using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderonTailoringCostCalculation.Models;
using OrderonTailoringCostCalculation.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OrderonTailoringCostCalculation.ViewModels
{
    [QueryProperty(nameof(ReceiptID), "receiptID")]
    public partial class DiscountsViewModel : ObservableObject
    {
        private readonly DiscountService _discountService;

        private readonly ReceiptDiscountService _receiptDiscountService;

        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Discount> discounts;

        [ObservableProperty]
        private int receiptID;

        [ObservableProperty]
        private Discount selectedDiscount;

        public DiscountsViewModel(INavigationService navigationService)
        {
            _discountService = new DiscountService();
            _receiptDiscountService = new ReceiptDiscountService();
            discounts = new ObservableCollection<Discount>();
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task DiscountSelected(Discount value)
        {
            ReceiptDiscount currentReceiptDiscount = new ReceiptDiscount();
            currentReceiptDiscount.ReceiptID = ReceiptID;
            currentReceiptDiscount.DiscountID = value.ID;

            await _receiptDiscountService.SaveItemAsync(currentReceiptDiscount);

            bool saved = true;

            var parameters = new Dictionary<string, object>
            {
                ["discountSaved"] = saved
            };
            await _navigationService.GoToAsync("..", parameters);

        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            await _discountService.GetItemsAsync();
            Discounts.Clear();
            foreach (var discount in _discountService.Discounts)
            {
                Discounts.Add(discount);
            }
        }
    }
}
