using HearthDb;
using HearthDb.Enums;

namespace HearthstoneDeckTracker.ViewModel
{
    class CardSearch
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Cost { get; set; }
        public string Class { get; set; }
        public string Rarity { get; set; }
        public string Set { get; set; }
        public int Dbfid { get; set; }

        public CardSearch(Card card, Locale language = Locale.enUS)
        {
            Name = card.GetLocName(language);
            Type = card.Type.ToString();
            Cost = card.Cost.ToString();
            Class = card.Class.ToString();
            Rarity = card.Rarity.ToString();
            Set = card.Set.ToString();
            Dbfid = card.DbfId;

        }
    }
}
