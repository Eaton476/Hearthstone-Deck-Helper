using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthDb;

namespace HearthstoneDeckTracker.Model
{
    public class CardSuggestion
    {
        public Card Card { get; set; }
        public List<string> Reasons { get; set; }
        public int Points { get; set; }

        public string ReasonsToString()
        {
            string ret = "";

            foreach (string reason in Reasons)
            {
                ret += reason + "\n";
            }

            return ret;
        }
    }
}
