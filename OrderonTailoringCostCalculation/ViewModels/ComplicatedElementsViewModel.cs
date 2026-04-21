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
        private readonly ComplicatedElementService _complicatedElementService;

        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<ComplicatedElement> complicatedElements;

        [ObservableProperty]
        private int receiptID;

        [ObservableProperty]
        private ComplicatedElement selectedComplicatedElement;

        public ComplicatedElementsViewModel(INavigationService navigationService)
        {
            _complicatedElementService = new ComplicatedElementService();
            complicatedElements = new ObservableCollection<ComplicatedElement>();
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task SelectedElementChanged(ComplicatedElement value)
        {
            if (value != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["complicatedElementID"] = value.ID,
                    ["receiptID"] = ReceiptID
                };
                await _navigationService.GoToAsync("ComplicatedElementsCount", parameters);
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
