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
    public partial class ReceiptListItemViewModel : ObservableObject
    {
        private readonly Receipt _receipt;

        public int id => _receipt.ID;

        public double minValue => _receipt.MinValue;

        public double complicatedElementsValue => _receipt.ComplicatedElementsValue;

        public double discountValue => _receipt.DiscountValue;

        public double totalValue => _receipt.TotalValue;

        [ObservableProperty]
        private string displayMinValueGarmentName;

        public ReceiptListItemViewModel(Receipt receipt, string minValueGarmentName)
        {
            _receipt = receipt;

            DisplayMinValueGarmentName = minValueGarmentName;
        }
    }
}