namespace InvoiceApp.Helpers
{
    public class DropDownCrossoverValue
    {
        public int RelatedDropDownId { get; set; }
        public string DropDownName { get; set; }
        /// <summary>
        /// Its used to bind the values in Dropdown from list 
        /// </summary>
        /// <param name="relateddropdownid"></param>
        /// <param name="dropdownname"></param>
        public DropDownCrossoverValue(int relateddropdownid, string dropdownname)
        {
            RelatedDropDownId = relateddropdownid;
            DropDownName = dropdownname;
        }
    }
}