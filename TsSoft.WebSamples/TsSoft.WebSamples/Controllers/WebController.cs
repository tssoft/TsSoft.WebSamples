namespace TsSoft.WebSamples.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using TsSoft.Web.Mvc.DataTablesNet;
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
                Neutrality = x.IsNeutralCulture ? CultureNeutrality.Neutral : CultureNeutrality.NotNeutral,
                WritingDirection = x.TextInfo.IsRightToLeft ? CultureWritingDirection.RightToLeft : CultureWritingDirection.LeftToRight,
            });
            IEnumerable<CultureData> culturesSortable = null;
            foreach (var column in request.SortColumns)
            {
                switch (column.Name)
                {
                    case "Name":
                        if (column.Order == SortOrder.Ascending)
                        {
                            culturesSortable = culturesData.OrderBy(x => x.Name);
                        }
                        else
                        {
                            culturesSortable = culturesData.OrderByDescending(x => x.Name);
                        }
                        break;

                    case "DisplayName":
                        if (column.Order == SortOrder.Ascending)
                        {
                            culturesSortable = culturesData.OrderBy(x => x.DisplayName);
                        }
                        else
                        {
                            culturesSortable = culturesData.OrderByDescending(x => x.DisplayName);
                        }
                        break;

                    case "EnglishName":
                        if (column.Order == SortOrder.Ascending)
                        {
                            culturesSortable = culturesData.OrderBy(x => x.EnglishName);
                        }
                        else
                        {
                            culturesSortable = culturesData.OrderByDescending(x => x.EnglishName);
                        }
                        break;

                    case "NativeName":
                        if (column.Order == SortOrder.Ascending)
                        {
                            culturesSortable = culturesData.OrderBy(x => x.NativeName);
                        }
                        else
                        {
                            culturesSortable = culturesData.OrderByDescending(x => x.NativeName);
                        }
                        break;

                    default:
                        culturesSortable = culturesData.OrderBy(x => x.Name);
                        break;
                }
            }
            IEnumerable<CultureData> responseData = culturesSortable;
            if (request.Criteria.WritingDirection != CultureWritingDirection.All)
            {
                responseData = responseData.Where(x => x.WritingDirection == request.Criteria.WritingDirection);
            }
            if (request.Criteria.Neutrality != CultureNeutrality.All)
            {
                responseData = responseData.Where(x => x.Neutrality == request.Criteria.Neutrality);
            }
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