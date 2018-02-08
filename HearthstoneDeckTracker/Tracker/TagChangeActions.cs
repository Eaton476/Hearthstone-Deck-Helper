using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthDb.Enums;

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
	        }
        }

	    private static void SetHero(int entityId)
	    {
		    
	    }
    }
}
