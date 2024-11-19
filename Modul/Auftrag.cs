namespace Auftrag.Modul
{
    public class Auftrag
    {
        public int id {  get; set; }
        public string title { get; set; }
        public int contact_id { get; set; }
        public int user_id { get; set; }
        public int language_id { get; set; }
        public int bank_account_id { get; set; }
        public int currency_id { get; set; }
        public int payment_type_id { get; set; }
        public int mwst_type { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
        public string is_valid_from { get; set; }
        public int? project_id { get; set; }

    }
}
