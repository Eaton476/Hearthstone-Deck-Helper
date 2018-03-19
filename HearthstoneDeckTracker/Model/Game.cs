using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Xml.Serialization;
using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthMirror.Objects;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.Utilities.Converters;
using Card = HearthDb.Card;

namespace HearthstoneDeckTracker.Model
{
	public class Game
	{
        [XmlElement]
		public Player User { get; set; } = new Player();
	    [XmlElement]
        public Player Opponent { get; set; } = new Player();
	    [XmlAttribute]
        public DateTime TimeGameStart { get; set; }
	    [XmlAttribute]
        public DateTime TimeGameFinish { get; set; }
	    [XmlAttribute]
        public TimeSpan Duration => TimeGameFinish - TimeGameStart;
        [XmlIgnore]
	    public bool GameInProgress { get; set; }
	    [XmlIgnore]
        public Dictionary<int, Entity> Entities { get; set; } = new Dictionary<int, Entity>();
	    [XmlIgnore]
        public int CurrentEntityId { get; set; }
        [XmlAttribute]
	    public int Turns { get; set; }
        [XmlIgnore]
        public MatchInfo Info { get; set; }

        public void StartGame()
		{
			TimeGameStart = DateTime.UtcNow;
		    GameInProgress = true;
		    Info = null;
		}

		public void EndGame()
		{
			TimeGameFinish = DateTime.UtcNow;
		    GameInProgress = false;
            ProcessEntities();
		}

        public void OutputEntitiesToLog()
	    {
	        foreach (var entity in Entities)
	        {
	            Log.Debug(entity.ToString());
	        }
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
	            if (string.IsNullOrEmpty(entity.Name))
	            {
	                ProcessCardEntity(entity);
	            }
	            else if (entity.Name == "GameEntity")
	            {
	                ProcessGameEntity(entity);
	            }
                else if (entity.Name != null && entity.Name != "EatonGaming#2462")
	            {
                    ProcessOpponentEntity(entity);
	            }
                else if (entity.Name == "EatonGaming#2462")
	            {
	                ProcessUserEntity(entity);
	            }
	        }

            Entities.Clear();
	        CurrentEntityId = 0;
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
                        User.Coin = true;
                        break;
	            }
	        }
        }

	    private void ProcessUserEntity(Entity entity)
	    {
	        User.EntityId = entity.EntityId;
	        User.Name = entity.Name;

	        foreach (var tag in entity.Tags)
	        {
	            GameTag gameTag = GameTagConverter.ParseEnum<GameTag>(tag.EnumId.ToString());

	            switch (gameTag)
	            {
	                case GameTag.PLAYSTATE:
	                    string value = Enum.GetName(typeof(PlayState), tag.Value);
	                    User.Result = value;
	                    break;
	                case GameTag.NUM_RESOURCES_SPENT_THIS_GAME:
	                    int mana = tag.Value;
	                    User.ManaUsedThisGame = mana;
	                    break;
	                case GameTag.NUM_FRIENDLY_MINIONS_THAT_DIED_THIS_GAME:
	                    int died = tag.Value;
	                    User.MinionsDiedThisGame = died;
	                    break;
	                case GameTag.NUM_TIMES_HERO_POWER_USED_THIS_GAME:
	                    int hero = tag.Value;
	                    User.NumberOfHeroPowerUsesThisGame = hero;
	                    break;
	                case GameTag.FIRST_PLAYER:
	                    User.Coin = false;
	                    Opponent.Coin = true;
	                    break;
	            }
	        }
        }

	    private void ProcessCardEntity(Entity entity)
	    {
	        Card card = Cards.GetCardFromId(entity.CardId);
	        if (card == null) return;
	        int controller = entity.GetTag(GameTag.CONTROLLER);

	        if (card.Collectible)
	        {
	            if (controller == Info.OpposingPlayer.Id)
	            {
	                //Hero
	                if (card.Type == CardType.HERO)
	                {
	                    Opponent.Deck.HeroDbfId = card.DbfId;
	                    Opponent.Deck.CardsInDeck.Add(card);
	                }
	                else if (card.Type != CardType.HERO_POWER)
	                {
	                    Opponent.Deck.CardsInDeck.Add(card);
	                }
	            }
	            else if (controller == Info.LocalPlayer.Id)
	            {
	                //Hero
	                if (card.Type == CardType.HERO)
	                {
	                    User.Deck.HeroDbfId = card.DbfId;
	                    User.Deck.CardsInDeck.Add(card);
	                }
	                else if (card.Type != CardType.HERO_POWER)
	                {
	                    User.Deck.CardsInDeck.Add(card);
	                }
	            }
            }
	    }
	}
}
