using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneDeckTracker.Model
{
    public class Interaction
    {
        public bool Local { get; set; }
        public string EntityName { get; set; }
        public string Zone { get; set; }
        public string ZonePos { get; set; }
        public string CardId { get; set; }
        public int Player { get; set; }
        public string Action { get; set; }
    }
}
