using demo.portal.Areas.basic.Models;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web.Mvc;

namespace demo.portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMvcContext _mvcContext;

        private readonly ISitecoreService _sitecoreService;
        private readonly ISitecoreService _masterSitecoreService;

        #region Constructor
        public HomeController(IMvcContext mvcContext, ISitecoreService sitecoreService)
        {
            _mvcContext = mvcContext;
            _sitecoreService = sitecoreService;
            _masterSitecoreService = new SitecoreService("master");
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var sourceItem = RenderingContext.Current.Rendering.Item;

            #region Mapping Fields
            var pageModel = new PageModel();

            pageModel.Header = sourceItem["Header"];
            pageModel.Body = sourceItem["Body"];
           
            DateField dateField = sourceItem.Fields["Date"];
            if (dateField != null)
            {
                pageModel.Date = dateField.DateTime;
            }

            ImageField imgField = sourceItem.Fields["Image"];
            if (imgField != null)
            {
                pageModel.Image = new Glass.Mapper.Sc.Fields.Image()
                {
                    Src = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem)
                };
            }
            #endregion

            return View("~/Areas/basic/Views/Home/Index.cshtml", pageModel);
        }

        public ActionResult About()
        {
            var dataSource = _mvcContext.GetContextItem<PageModel>();

            return View("~/Areas/basic/Views/Home/About.cshtml", dataSource);
        }

        public ActionResult Deals()
        {
            // product path
            string dealPath = "/sitecore/content/home/deals/london";
            // product Id
            string dealID = "{0CD5C7B6-D994-49BF-BB62-AC303D99A5D3}";
            // product name
            string dealName = "sydney";
            // query by product name
            string dealQuery = $"/sitecore/content/home/deals//*[contains(@@name,'{dealName.ToLower()}')]";
            // query type 1 to get multiple products
            string dealsPathType1 = "/sitecore/content/home/deals/*";
            // query type 2 to get multiple products
            string dealsPathType2 = "/sitecore/content/home/deals//*";

            BaseDeal target = _sitecoreService.GetItem<BaseDeal>(dealPath, x => x.LazyDisabled());

            #region Single Item

            #region  Get Item by Id
            //Example 1
            var options = new GetItemByIdOptions(new Guid(dealID))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var target2 = _sitecoreService.GetItem<BaseDeal>(options);
            #endregion

            #region  Get Item by Path
            // Example 2
            var options1 = new GetItemByPathOptions(dealPath)
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var target3 = _sitecoreService.GetItem<BaseDeal>(options1);
            #endregion
            
            #region  Get Item by Query
            // Example 3
            var options4 = new GetItemByQueryOptions(new Query(dealQuery))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var target4 = _sitecoreService.GetItem<BaseDeal>(options4);
            #endregion

            #endregion

            #region Multiple Items 

            #region  Get Item by Query
            //Example 1
            var options10 = new GetItemsByQueryOptions(new Query(dealsPathType1))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var target10 = _sitecoreService.GetItems<BaseDeal>(options10);

            //Example 2
            var options11 = new GetItemsByQueryOptions(new Query(dealsPathType2))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var target11 = _sitecoreService.GetItems<BaseDeal>(options11);

            #endregion

            #endregion 

            return View("~/Areas/basic/Views/Home/Deals.cshtml", target11);
        }

        public ActionResult Contact()
        {
            return View("~/Areas/basic/Views/Home/Contact.cshtml");
        }

        [HttpPost]
        public ActionResult AddDeal()
        {
            // deals root id '/sitecore/content/home/deals'
            string dealParentID = "{8069B844-1D05-400A-A40A-26412345DDF3}";

            // get deals count
            int dealCount = GetDealCount();

            #region Get parent item
            // Get Parent Item
            var options = new GetItemByIdOptions(new Guid(dealParentID))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var parentItem = _sitecoreService.GetItem<BaseDeal>(options);
            #endregion

            #region  Create a new deal
            var baseDeal = new BaseDeal()
            {
                Name = $"Newdeal-{dealCount + 1}",
                StartPrice = dealCount + 1000,
                PriceIncludes = "Price includes everything",
               HeaderImage = new Glass.Mapper.Sc.Fields.Image() { MediaId = new Guid("{CD431039-D155-42A3-89A3-B97621775075}") }
            };

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                var newdeal = _masterSitecoreService.CreateItem<BaseDeal>(parentItem, baseDeal);

                // publish newly created item.
                PublishItem(newdeal.Id);
            }
            
            #endregion

            return Redirect("/deals");
        }

        #endregion

        #region Private Methods
        // Get deals count
        private int GetDealCount()
        {
            var options = new GetItemsByQueryOptions(new Query("/sitecore/content/home/deals//*"))
            {
                InferType = true,
                Lazy = Glass.Mapper.LazyLoading.Disabled
            };
            var deals = _sitecoreService.GetItems<BaseDeal>(options);

            return deals != null ? deals.Count() : 0;
        }

        /// <summary>
        /// Publish Item
        /// </summary>
        /// <param name="Id"></param>
        private void PublishItem(Guid Id)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                Database webdb = Sitecore.Configuration.Factory.GetDatabase("web");
                Database masterdb = Sitecore.Configuration.Factory.GetDatabase("master");

                ClearSitecoreDatabaseCache(masterdb);

                Sitecore.Data.Items.Item masterItem = masterdb.GetItem(new ID(Id));

                // target databases
                Database[] databases = new Database[1] { webdb };

                Sitecore.Handle publishHandle = Sitecore.Publishing.PublishManager.PublishItem(masterItem, databases, webdb.Languages, true, false);

                ClearSitecoreDatabaseCache(webdb);
            }
        }

        /// <summary>
        /// Clear Sitecore Cache
        /// </summary>
        /// <param name="db"></param>
        private void ClearSitecoreDatabaseCache(Database db)
        {
            // clear html cache
            Sitecore.Context.Site.Caches.HtmlCache.Clear();

            db.Caches.ItemCache.Clear();
            db.Caches.DataCache.Clear();

            //Clear prefetch cache
            foreach (var cache in Sitecore.Caching.CacheManager.GetAllCaches())
            {
                if (cache.Name.Contains(string.Format("Prefetch data({0})", db.Name)))
                {
                    cache.Clear();
                }
            }
        }
        #endregion

    }
}

   
