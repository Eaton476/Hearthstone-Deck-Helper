using System.Linq;
using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities.Converters;

namespace HearthstoneDeckTracker.Tracker
{
    public static class TagChangeActions
    {
        public static void TagChange(int entityId, GameTag tag, int value)
        {
            switch (tag)
            {
                    case GameTag.CARDTYPE:
                        ChangeCardType(entityId, value);
                        break;
            }
        }

        private static void ChangeCardType(int entityId, int value)
        {
	        switch (value)
	        {
				case (int)CardType.HERO:
					SetHero(entityId);
				    break;
                case (int)CardType.PLAYER:
                    SetPlayer(entityId);
					break;
	        }
        }

	    private static void SetHero(int entityId)
	    {
		    Entity entity = Database.CurrentGame.Entities[entityId];
		    if (entity.CardId != null)
		    {
			    int controller = Database.CurrentGame.Entities[entityId].Tags
				    .First(x => x.EnumId == (int)GameTag.CONTROLLER).Value;

			    int dbfId = Cards.GetCardFromId(entity.CardId).DbfId;

			    if (controller == 1)
			    {
				    Database.CurrentGame.Opponent.Deck.HeroDbfId = dbfId;
			    }
			    else if (controller == 2)
			    {
				    Database.CurrentGame.User.Deck.HeroDbfId = dbfId;
			    }
			}
        }

        private static void SetPlayer(int entityId)
        {
            int controller = Database.CurrentGame.Entities[entityId].Tags
                .First(x => x.EnumId == (int) GameTag.CONTROLLER).Value;

            if (controller == 1)
            {
                Database.CurrentGame.Opponent.EntityId = entityId;
            }
            else if (controller == 2)
            {
                Database.CurrentGame.Opponent.EntityId = entityId;
            }
        }
    }
}
