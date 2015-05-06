using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bigrivers.Server.Data;

namespace Bigrivers.Client.Backend.Controllers
{
    public class BaseController : Controller
    {
        public readonly BigriversDb Db = new BigriversDb();
    }
}