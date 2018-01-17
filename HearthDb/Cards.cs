#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using HearthDb.Enums;

#endregion

namespace HearthDb
{
	public static class Cards
	{
		public static readonly Dictionary<string, Card> All = new Dictionary<string, Card>();

		public static readonly Dictionary<string, Card> Collectible = new Dictionary<string, Card>();

		static Cards()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HearthDb.CardDefs.xml");
			if(stream == null)
				return;
			using(TextReader tr = new StreamReader(stream))
			{
				var xml = new XmlSerializer(typeof(CardDefs.CardDefs));
				var cardDefs = (CardDefs.CardDefs)xml.Deserialize(tr);
				foreach(var entity in cardDefs.Entites)
				{
					var card = new Card(entity);
					All.Add(entity.CardId, card);
					if(card.Collectible && (card.Type != CardType.HERO || card.Set != CardSet.CORE && card.Set != CardSet.HERO_SKINS))
						Collectible.Add(entity.CardId, card);
				}
			}
		}

		public static Card GetFromName(string name, Locale lang, bool collectible = true)
			=> (collectible ? Collectible : All).Values.FirstOrDefault(x => x.GetLocName(lang)?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);

		public static Card GetFromDbfId(int dbfId, bool collectibe = true)
			=> (collectibe ? Collectible : All).Values.FirstOrDefault(x => x.DbfId == dbfId);

	    public static List<Card> SearchCardsByNameAsync(string query, Locale lang)
	    {
	        List<Card> cardsFound = new List<Card>();
            foreach (Card card in Collectible.Values)
            {
                string cardname = card.GetLocName(lang);
                if (cardname != null)
                {
                    if (cardname.ToLower().Contains(query))
                    {
                        cardsFound.Add(card);
                    }
                }
            }

	        return cardsFound;
        }

	    public static List<Card> GetHeroClasses()
	    {
	        return All.Values.Where(x => x.Type == CardType.HERO && x.Entity.CardId.Contains("HERO")).ToList();
	    }

	    public static Card GetCardFromId(string cardId)
	    {
	        if (string.IsNullOrEmpty(cardId))
	        {
	            return null;
	        }
	        if (All.TryGetValue(cardId, out Card dbCard))
	        {
	            return new Card(dbCard.Entity);
	        }
	        return null;
	    }
	}
}