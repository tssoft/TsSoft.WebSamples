namespace TsSoft.WebSamples.Models.DataTables
{
    using System.Collections.Generic;

    public class CultureResponse
    {
        public IEnumerable<CultureData> aaData { get; set; }

        public int iTotalRecords { get; set; }

        public int iTotalDisplayRecords { get; set; }

        public int sEcho { get; set; }
    }
}