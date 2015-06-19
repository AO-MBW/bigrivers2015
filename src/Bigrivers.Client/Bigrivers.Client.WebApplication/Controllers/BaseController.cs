using System.Linq;
using System.Web.Mvc;
using Bigrivers.Client.WebApplication.ViewModels;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.Controllers
{
    public class BaseController : Controller
    {
        protected readonly BigriversDb AccessLayer = new BigriversDb();

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

            ViewBag.MenuItems = AccessLayer.MenuItems.Where(m => m.Status && !m.Deleted && m.Parent == null).OrderBy(m => m.Order).ToList();
            ViewBag.MenuItemsChild = AccessLayer.MenuItems.Where(m => m.Status && !m.Deleted && m.Parent != null).OrderBy(m => m.Order).ToList();
            var siteInformation = AccessLayer.SiteInformation.FirstOrDefault() ?? new SiteInformation
            {
                YoutubeChannel = null,
                Facebook = null,
                Twitter = null,
                Image = null,
                Date = null
            };
            
            ViewBag.SiteInformation = new SettingsViewModel
            {
                YoutubeChannel = siteInformation.YoutubeChannel,
                Facebook = siteInformation.Facebook,
                Twitter = siteInformation.Twitter,
                Image = siteInformation.Image,
                Date = siteInformation.Date
            };
        }

    }
}