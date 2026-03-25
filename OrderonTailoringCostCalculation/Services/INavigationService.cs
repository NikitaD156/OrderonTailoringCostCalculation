using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public interface INavigationService
    {
        /// <summary>Переход на указанный маршрут.</summary>
        /// <param name="route">Имя маршрута (например, "DetailsPage").</param>
        /// <param name="parameters">Параметры для передачи (например, ID объекта).</param>
        Task GoToAsync(string route, IDictionary<string, object>? parameters = null);

        /// <summary>Возврат на предыдущую страницу.</summary>
        Task GoBackAsync();

        /// <summary>Возврат на предыдущую страницу с передачей данных обратно.</summary>
        Task GoBackAsync(IDictionary<string, object>? parameters = null);

        /// <summary>Переход на корневую страницу (очистка стека).</summary>
        Task GoToRootAsync();

        INavigation GetCurrentNavigation();
    }
}
