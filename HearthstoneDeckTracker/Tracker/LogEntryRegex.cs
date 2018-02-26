using System.Text.RegularExpressions;

namespace HearthstoneDeckTracker.Tracker
{
	public static class LogEntryRegex
	{
		public static readonly Regex EntityRegex =
			new Regex(
				@"(?=id=(?<id>(\d+)))(?=name=(?<name>(\w+)))?(?=zone=(?<zone>(\w+)))?(?=zonePos=(?<zonePos>(\d+)))?(?=cardId=(?<cardId>(\w+)))?(?=player=(?<player>(\d+)))?(?=type=(?<type>(\w+)))?");
		public static readonly Regex LogEntryStructure = new Regex("^(D|W) (?<timestamp>([\\d:.]+)) (?<line>(.*))$");
		public static readonly Regex BeginBlurRegex = new Regex(@"BeginEffect blur \d => 1");
		public static readonly Regex TagChangeRegex =
			new Regex(@"TAG_CHANGE\ Entity=(?<entity>(.+))\ tag=(?<tag>(\w+))\ value=(?<value>(\w+))");
	    public static readonly Regex PlayerEntityRegex =
	        new Regex(@"Player\ EntityID=(?<id>(\d+))\ PlayerID=(?<playerId>(\d+))\ GameAccountId=(?<gameAccountId>(.+))");
		//public static readonly Regex CreationRegex = new Regex(@"FULL_ENTITY - Updating.*id=(?<id>(\d+)).*zone=(?<zone>(\w+)).*CardID=(?<cardId>(\w*)).player=(?<player>(\d+))");
		public static readonly Regex CreationRegexUpdating = new Regex(@"FULL_ENTITY - Updating \[entityName=(\w+) id=(\d+) zone=(\w+) zonePos=(\d) cardId=(\w+) player=(\d)\] CardID=(\w+)");
        public static readonly Regex CreationRegexCreating = new Regex(@"FULL_ENTITY - Creating ID=(\d+) CardID=(\w+)");
		public static readonly Regex CreationTagRegex = new Regex(@"tag=(?<tag>(\w+))\ value=(?<value>(\w+))");
	    public static readonly Regex GameEntityRegex = new Regex(@"GameEntity\ EntityID=(?<id>(\d+))");
	    public static readonly Regex UpdatingEntityRegex =
	        new Regex(@"(?<type>(SHOW_ENTITY|CHANGE_ENTITY))\ -\ Updating\ Entity=(?<entity>(.+))\ CardID=(?<cardId>(\w*))");
    }
}
