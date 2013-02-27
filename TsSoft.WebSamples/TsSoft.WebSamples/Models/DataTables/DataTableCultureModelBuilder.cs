namespace TsSoft.WebSamples.Models.DataTables
{
    using System;
    using System.Web.Mvc;
    using TsSoft.Web.Mvc.DataTablesNet;

    public class DataTableCultureModelBuilder : DataTableModelBinder
    {
        public new object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = (DataTableCultureSettings)base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;
            model.IsNeutralCulture = Convert.ToBoolean(request["IsNeutralCulture"]);
            model.IsRightToLeft = Convert.ToBoolean(request["IsRightToLeft"]);
            return model;
        }
    }
}