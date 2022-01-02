using DotNetBanky.Core.DTOModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBanky.Admin.Pages.Shared.Components.CountryCard
{
    public class CountryCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DashboardCountryCardDTO countryData)
        {
            return View(countryData);
        }
    }
}
