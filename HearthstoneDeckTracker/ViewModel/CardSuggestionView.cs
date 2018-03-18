using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthDb;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;

namespace HearthstoneDeckTracker.ViewModel
{
    public class CardSuggestionView
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string Cost { get; set; }
        public string Class { get; set; }
        public string Rarity { get; set; }
        public int Dbfid { get; set; }

        public CardSuggestionView(CardSuggestion card, Locale language = Locale.enUS)
        {
            Name = card.Card.GetLocName(language);
            Points = card.Points;
            Cost = card.Card.Cost.ToString();
            Class = card.Card.Class.ToString();
            Rarity = card.Card.Rarity.ToString();
            Dbfid = card.Card.DbfId;
        }
    }
}
