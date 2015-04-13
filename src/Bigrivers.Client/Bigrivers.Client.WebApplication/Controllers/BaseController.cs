using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.Controllers
{
    public class BaseController : Controller
    {
        public static readonly BigriversDb AccessLayer = new BigriversDb();

        public BaseController()
        {
            //List<MenuItem> menuItems = new List<MenuItem>();

            //// create a single menuitem
            //MenuItem mi = new MenuItem();
            //mi.DisplayName = "Event 3";
            //mi.Content = "/Home/Events/31";
            //mi.Parent = 0;
            //mi.Order = 0;
            //mi.MenuItemType = MenuItemType.Page;
            //menuItems.Add(mi);

            //// create another menuitem
            //MenuItem mi2 = new MenuItem();
            //mi2.DisplayName = "Event 4";
            //mi2.Content = "/Home/Events/32";
            //mi2.Parent = 0;
            //mi2.Order = 0;
            //mi2.MenuItemType = MenuItemType.Page;
            //menuItems.Add(mi2);

            ViewBag.MenuItems = AccessLayer.MenuItems.Where(m => m.Status).ToList();
        }

    }
}