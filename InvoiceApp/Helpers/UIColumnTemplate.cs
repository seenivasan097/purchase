namespace InvoiceApp.Helpers
{
    public class UIColumnTemplate
    {
        public UIColumnTemplate() { }
        /// <summary>
        /// Get Column values and properety
        /// </summary>
        /// <param name="headerText"></param>
        /// <param name="colWidth"></param>
        /// <param name="colMaxPct"></param>
        public UIColumnTemplate(string headerText, int colWidth, int colMaxPct)
        {
            HeaderText = headerText;
            ColumnWidth = colWidth;
            ColMaxPct = colMaxPct;
        }

        public int Id { get; set; }
        public bool IsVisible { get { return true; } }
        public decimal ColDynamicPct { get; set; }
        public string HeaderText { get; set; }
        public string ColName { get; set; }
        public int ColumnWidth { get; set; }
        public int ColMaxPct { get; set; }
    }
}
