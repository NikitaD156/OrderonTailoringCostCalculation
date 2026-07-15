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
    [QueryProperty(nameof(CurrentConventionalUnitValue), "currentConventionalUnitValue")]
    public partial class MinValutGarmentsViewModel : ObservableObject
    {
        private readonly MinValueGarmentService _minValueGarmentService;

        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<MinValueGarment> minValueGarments;

        [ObservableProperty]
        private MinValueGarment selectedMinValueGarment;

        [ObservableProperty]
        private int currentConventionalUnitValue;

        public MinValutGarmentsViewModel(INavigationService navigationService)
        {
            _minValueGarmentService = new MinValueGarmentService();
            minValueGarments = new ObservableCollection<MinValueGarment>();
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task SelectedGarmentChanged(MinValueGarment value)
        {
            if (value != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["currentConventionalUnitValue"] = CurrentConventionalUnitValue,
                    ["garmentID"] = value.ID
                };
                await _navigationService.GoToAsync("MaterialGroupsPage", parameters);
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            await _minValueGarmentService.GetItemsAsync();
            MinValueGarments.Clear();
            foreach (var garment in _minValueGarmentService.MinValueGarments)
            {
                MinValueGarments.Add(garment);
            }
        }
    }
}
