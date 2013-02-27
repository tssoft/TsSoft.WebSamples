namespace TsSoft.WebSamples.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using TsSoft.WebSamples.Models.DataTables;

    public class WebController : Controller
    {
        private static readonly IEnumerable<CultureInfo> cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData([ModelBinder(typeof(DataTableCultureModelBuilder))]DataTableCultureSettings request)
        {
            var culturesData = cultures.Select(x => new CultureData
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                EnglishName = x.EnglishName,
                NativeName = x.NativeName,
                IsNeutralCulture = x.IsNeutralCulture,
                IsRightToLeft = x.TextInfo.IsRightToLeft,
            });
            IEnumerable<CultureData> culturesSortable = null;
            foreach (var column in request.SortColumns)
            {
                switch (column.Name)
                {
                    case "Name":
                        culturesSortable = culturesData.OrderBy(x => x.Name);
                        break;

                    case "CurrentLocaleName":
                        culturesSortable = culturesData.OrderBy(x => x.DisplayName);
                        break;

                    case "EnglishName":
                        culturesSortable = culturesData.OrderBy(x => x.EnglishName);
                        break;

                    case "NativeName":
                        culturesSortable = culturesData.OrderBy(x => x.NativeName);
                        break;

                    default:
                        culturesSortable = culturesData.OrderBy(x => x.Name);
                        break;
                }
            }
            var responseData = culturesSortable
                                .Where(x => x.IsNeutralCulture == request.IsNeutralCulture)
                                .Where(x => x.IsRightToLeft == request.IsRightToLeft);
            var response = new CultureResponse
            {
                aaData = responseData.Skip(request.Skip).Take(request.Take).ToList(),
                iTotalDisplayRecords = responseData.Count(),
                iTotalRecords = culturesData.Count(),
                sEcho = request.RequestId
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}