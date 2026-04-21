using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public class MauiNavigationService : INavigationService
    {
        public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            var shell = Shell.Current ?? throw new InvalidOperationException("Shell.Current is null");
            return parameters == null
                ? shell.GoToAsync(route)
                : shell.GoToAsync(route, parameters);
        }

        public Task GoBackAsync()
        {
            var shell = Shell.Current;
            return shell?.GoToAsync("..") ?? Task.CompletedTask;
        }

        public Task GoBackAsync(IDictionary<string, object>? parameters = null)
        {
            var shell = Shell.Current;
            return parameters == null
                ? shell.GoToAsync("..")
                : shell.GoToAsync("..", parameters);
        }

        public Task GoToRootAsync()
        {
            var shell = Shell.Current;
            return shell?.GoToAsync("//MainPage") ?? Task.CompletedTask;
        }

        
    }
}
