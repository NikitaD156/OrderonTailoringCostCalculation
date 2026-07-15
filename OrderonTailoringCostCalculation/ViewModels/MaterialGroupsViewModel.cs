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
    [QueryProperty(nameof(CurrentConventionalUnitValue), "currentConventionalUnitValue")]
    [QueryProperty(nameof(SelectedMinValueGarmentID), "garmentID")]
    public partial class MaterialGroupsViewModel : ObservableObject
    {
        private readonly MaterialGroupService _MaterialGroupService;

        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private int selectedMinValueGarmentID;

        [ObservableProperty]
        private ObservableCollection<MaterialGroup> materialGroups;

        [ObservableProperty]
        private int currentConventionalUnitValue;

        [ObservableProperty]
        private MaterialGroup selectedMaterialGroup;

        public MaterialGroupsViewModel(INavigationService navigationService)
        {
            _MaterialGroupService = new MaterialGroupService();
            MaterialGroups = new ObservableCollection<MaterialGroup>();
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task SelectedGroupChanged(MaterialGroup value)
        {
            if (value != null)
            {
                MinValueGarmentIDWithSelectedGroup minValueGarmentIDWithSelectedGroup = new();
                minValueGarmentIDWithSelectedGroup.MinValueGarmentID = SelectedMinValueGarmentID;
                minValueGarmentIDWithSelectedGroup.SelectedGroup = value.ID;
                minValueGarmentIDWithSelectedGroup.ConventionalUnitValue = CurrentConventionalUnitValue;
                var parameters = new Dictionary<string, object>
                {
                    ["garmentIDandGroup"] = minValueGarmentIDWithSelectedGroup
                };
                await _navigationService.GoToAsync("../..", parameters);
            }
        }


        [RelayCommand]
        public async Task LoadAsync()
        {
            await _MaterialGroupService.GetItemsAsync();
            MaterialGroups.Clear();
            MinValueGarmentService minValueGarmentService = new MinValueGarmentService();
            MinValueGarment minValueGarment = await minValueGarmentService.GetItemAsync(selectedMinValueGarmentID);
            
            foreach (var materialGroup in _MaterialGroupService.MaterialGroups)
            {
                switch (materialGroup.ID)
                {
                    case 1:
                        {
                            if (minValueGarment.RatioFirst != 0)
                            {
                                MaterialGroups.Add(materialGroup);
                            }

                        }
                        break;
                    case 2:
                        {
                            if (minValueGarment.RatioFirst != 0)
                            {
                                MaterialGroups.Add(materialGroup);
                            }

                        }
                        break;
                    case 3:
                        {
                            if (minValueGarment.RatioSecond != 0)
                            {
                                MaterialGroups.Add(materialGroup);
                            }

                        }
                        break;
                    case 4:
                        {
                            if (minValueGarment.RatioThird != 0)
                            {
                                MaterialGroups.Add(materialGroup);
                            }

                        }
                        break;
                    case 5:
                        {
                            if (minValueGarment.RatioFourth != 0)
                            {
                                MaterialGroups.Add(materialGroup);
                            }

                        }
                        break;
                }
                
            }
        }
    }
}
