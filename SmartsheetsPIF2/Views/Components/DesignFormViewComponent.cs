using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Pages.Components.DesignForm
{
    public class DesignFormViewComponent : ViewComponent
    {
        public DesignFormViewComponent()
        {
        }

        public IViewComponentResult
        Invoke(string designFormType)
        {
            return View("Default", designFormType);
        }
    }
}