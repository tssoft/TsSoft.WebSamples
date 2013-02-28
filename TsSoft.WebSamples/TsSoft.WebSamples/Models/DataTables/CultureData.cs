namespace TsSoft.WebSamples.Models.DataTables
{
    public class CultureData
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public CultureNeutrality Neutrality { get; set; }

        public CultureWritingDirection WritingDirection { get; set; }
    }
}