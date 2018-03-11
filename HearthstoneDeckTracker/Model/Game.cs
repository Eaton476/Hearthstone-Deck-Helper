using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Xml.Serialization;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.Utilities.Converters;

namespace HearthstoneDeckTracker.Model
{
	public class Game
	{
		public Player User { get; set; } = new Player();
		public Player Opponent { get; set; } = new Player();
	    internal DateTime TimeGameStart { get; set; }
		internal DateTime TimeGameFinish { get; set; }
		public TimeSpan Duration => TimeGameFinish - TimeGameStart;
		public string Result { get; set; }
	    public bool GameInProgress { get; set; }
		public Dictionary<int, Entity> Entities { get; set; } = new Dictionary<int, Entity>();
        public int CurrentEntityId { get; set; }
	    public int Turns { get; set; }

	    public void StartGame()
		{
			TimeGameStart = DateTime.UtcNow;
		    GameInProgress = true;
		}

		public void EndGame()
		{
			TimeGameFinish = DateTime.UtcNow;
		    GameInProgress = false;
            ProcessEntities();
            SaveGame();
		}

	    public void OutputEntitiesToLog()
	    {
	        foreach (var entity in Entities)
	        {
	            Log.Debug(entity.ToString());
	        }
	    }

		public void SaveGame()
		{
		    //SaveTestEntitiesToXml();
            
		}

	    public void SaveTestEntitiesToXml()
        { 
            List<Entity> entites = new List<Entity>(Entities.Values);
	        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Entity>));
	        using (TextWriter writer = new StreamWriter(Config.EntityXmlFile()))
	        {
	            xmlSerializer.Serialize(writer, entites);
	        }

            Log.Info("Successfully saved the game entities to disk.");
        }

	    public void LoadTestEntitiesFromXml()
	    {
	        List<Entity> entites = new List<Entity>();
            XmlSerializer xmlDeSerializer = new XmlSerializer(typeof(List<Entity>));
	        using (TextReader reader = new StreamReader(@"D:\HearthstoneAssistant\TestEntitiesXml.xml"))
	        {
	            entites = (List<Entity>)xmlDeSerializer.Deserialize(reader);
                reader.Close();
	        }

	        Entities = entites.ToDictionary(x => x.EntityId, x => x);

	        Log.Info("Successfully loaded the test entities from disk.");
        }

	    public void CleanupEntities()
	    {
	        RemoveDuplicateTags();
	    }

	    private void RemoveDuplicateTags()
	    {
	        foreach (var entity in Entities.Values)
	        {
	            entity.Tags = entity.Tags.GroupBy(x => x.EnumId).Select(x => x.FirstOrDefault()).ToList();
	        }
	    }

        public void ProcessEntities()
	    {
	        CleanupEntities();

	        foreach (var entity in Entities.Values)
	        {
	            if (entity.Name == "GameEntity")
	            {
	                ProcessGameEntity(entity);
	            }
                else if (entity.Name != "" && entity.Name != "EatonGaming")
	            {
                    ProcessOpponentEntity(entity);
	            }
	        }
	    }

	    private void ProcessGameEntity(Entity entity)
	    {
	        foreach (var tag in entity.Tags)
	        {
	            GameTag gameTag = GameTagConverter.ParseEnum<GameTag>(tag.EnumId.ToString());
	            if (gameTag == GameTag.TURN)
	            {
	                int value = tag.Value;
	                Turns = value;
	            }
	        }
	    }

	    private void ProcessOpponentEntity(Entity entity)
	    {
	        Opponent.EntityId = entity.EntityId;
	        Opponent.Name = entity.Name;

	        foreach (var tag in entity.Tags)
	        {
	            GameTag gameTag = GameTagConverter.ParseEnum<GameTag>(tag.EnumId.ToString());

	            switch (gameTag)
	            {
	                case GameTag.PLAYSTATE:
	                    string value = Enum.GetName(typeof(PlayState), tag.Value);
	                    Opponent.Result = value;
	                    break;
	                case GameTag.NUM_RESOURCES_SPENT_THIS_GAME:
	                    int mana = tag.Value;
	                    Opponent.ManaUsedThisGame = mana;
	                    break;
	                case GameTag.NUM_FRIENDLY_MINIONS_THAT_DIED_THIS_GAME:
	                    int died = tag.Value;
	                    Opponent.MinionsDiedThisGame = died;
	                    break;
	                case GameTag.NUM_TIMES_HERO_POWER_USED_THIS_GAME:
	                    int hero = tag.Value;
	                    Opponent.NumberOfHeroPowerUsesThisGame = hero;
	                    break;
                    case GameTag.FIRST_PLAYER:
                        Opponent.Coin = false;
                        break;
	            }
	        }
        }
	}
}
