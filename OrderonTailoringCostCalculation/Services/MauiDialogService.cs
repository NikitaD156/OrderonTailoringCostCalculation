using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class MauiDialogService : IDialogService
    {
        public Task ShowAlertAsync(string title, string message, string cancel)
        {
            // Пытаемся получить текущую страницу через Shell, если нет — через Application
            var page = Shell.Current ?? Application.Current?.MainPage;
            if (page == null)
                throw new InvalidOperationException("Cannot display alert: no current page found.");
        
            return page.DisplayAlertAsync(title, message, cancel);
        }

        public Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel)
        {
            // Пытаемся получить текущую страницу через Shell, если нет — через Application
            var page = Shell.Current ?? Application.Current?.MainPage;
            if (page == null)
                throw new InvalidOperationException("Cannot display alert: no current page found.");
            
            return page.DisplayAlertAsync(title, message, accept, cancel);
        }
    }
}
