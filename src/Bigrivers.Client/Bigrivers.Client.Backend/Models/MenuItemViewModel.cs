using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class MenuItemViewModel
    {
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }

        public int? Order { get; set; }
        public int Parent { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Naar...")]
        public string LinkType { get; set; }

        [Display(Name = "URL")]
        [DataType(DataType.Url)]
        public string ExternalUrl { get; set; }

        public string InternalType { get; set; }
        public string InternalId { get; set; }

        public IEnumerable<SelectListItem> LinkTypes
        {
            get
            {
                return new[]
                {
                    new SelectListItem()
                    {
                        Text = "Sitepagina",
                        Value = "internal"
                    },
                    new SelectListItem()
                    {
                        Text = "Andere website",
                        Value = "external"
                    },
                    new SelectListItem()
                    {
                        Text = "Een bestand",
                        Value = "file"
                    }
                };
            }
        } 

        public IEnumerable<SelectListItem> Types
        {
            get
            {
                return new[]
                {
                    new SelectListItem()
                    {
                        Text = "Evenementen",
                        Value = "Events"
                    },
                    new SelectListItem()
                    {
                        Text = "Artiesten",
                        Value = "Artists"

                    },
                    new SelectListItem()
                    {
                        Text = "Optredens",
                        Value = "Performances"
                    },
                    new SelectListItem()
                    {
                        Text = "Nieuwsberichten",
                        Value = "News"
                    },
                    new SelectListItem()
                    {
                        Text = "Sponsoren",
                        Value = "Sponsors"

                    },
                    new SelectListItem()
                    {
                        Text = "Genres",
                        Value = "Genres"

                    }
                };
            }
        }

        public List<SelectListItem> Events { get; set; }
        public List<SelectListItem> Artists { get; set; }
        public List<SelectListItem> Performances { get; set; }
        public List<SelectListItem> NewsItems { get; set; }
        public List<SelectListItem> Sponsors { get; set; }
        public List<SelectListItem> Genres { get; set; }

    }
}