namespace TsSoft.WebSamples.Models.DataTables
{
    using System;
    using System.Web.Mvc;
    using TsSoft.Web.Mvc.DataTablesNet;

    public class DataTableCultureModelBuilder : DataTableModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = (DataTableCultureSettings)base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;
            model.Criteria = new SearchCriteria();
            model.Criteria.Neutrality = (CultureNeutrality)Enum.Parse(typeof(CultureNeutrality), request["Criteria.Neutrality"]);
            model.Criteria.WritingDirection = (CultureWritingDirection)Enum.Parse(typeof(CultureWritingDirection), request["Criteria.WritingDirection"]);
            return model;
        }
    }
}