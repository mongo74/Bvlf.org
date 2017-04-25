using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace bvlf_v2.Controllers
{
    /// <summary>
    ///     Controller for Ajax calls
    /// </summary>
    public class AjaxController : SurfaceController
    {
        /// <summary>
        ///     Returns additional content for Profff Article
        /// </summary>
        /// <param name="id">the node id</param>
        /// <returns></returns>
        public ActionResult GetAdditionalMaterial(int id)
        {
            var contentService = Services.ContentService;
            var contentitem = contentService.GetById(id);

            var contentstring = contentitem.GetValue("content").ToString();
            var dynamicItem = contentitem.GetValue("downloadItem");
            var downloadstring = string.Empty;
            if (dynamicItem != null && !string.IsNullOrEmpty(dynamicItem.ToString()))
            {
                var file = Umbraco.TypedMedia(Convert.ToInt32(dynamicItem));
                var url = file.GetPropertyValue("umbracoFile").ToString();
                var name = file.Name;
                downloadstring = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, name);
            }
            var returnstring = string.Format("{0}<p>{1}</p>", contentstring, downloadstring);
            return Content(returnstring);
        }

        public ActionResult ClearCache()
        {
            ClearCacheItems();
            var sb = new StringBuilder();
            sb.AppendLine("Cache cleared");
            return Content(sb.ToString());
        }

        /// <summary>
        ///     Remove all items from Cach
        /// </summary>
        public void ClearCacheItems()
        {
            var keys = new List<string>();
            var enumerator = HttpContext.Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            foreach (var t in keys)
                HttpContext.Cache.Remove(t);
        }

        public ActionResult TryCounter()
        {
            var counter = 0;
            var maxLevel = 100;
            for (var i = 0; i <= maxLevel; i++)
            {
                counter++;
            }
            var returnstring = string.Format("{0}<br />", counter);
            return Content(returnstring);
        }
    }
}