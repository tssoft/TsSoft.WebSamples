namespace TsSoft.WebSamples.Models.DataTables
{
    using TsSoft.Web.Mvc.DataTablesNet;

    public class DataTableCultureSettings : DataTableSettings
    {
        public bool IsNeutralCulture { get; set; }

        public bool IsRightToLeft { get; set; }
    }
}