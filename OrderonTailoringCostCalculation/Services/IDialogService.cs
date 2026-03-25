using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string cancel);
    }
}
