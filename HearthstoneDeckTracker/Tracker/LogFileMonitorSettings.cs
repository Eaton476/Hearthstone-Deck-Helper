using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneDeckTracker.Tracker
{
    public class LogFileMonitorSettings
    {
        public string Name { get; set; }
        public bool Reset { get; set; }
	    public string[] StartFilters { get; set; }
		public string[] ContainingFilters { get; set; }

        public bool HasFilters => StartFilters != null || ContainingFilters != null;
    }
}
