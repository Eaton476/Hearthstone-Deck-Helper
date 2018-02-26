using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.Utilities.Converters;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class PowerLogFileHandler
	{
        private readonly List<Entity> _tempEntities = new List<Entity>();

		public void Handle(LogEntry entry)
		{
		    if (LogEntryRegex.GameEntityRegex.IsMatch(entry.Line))
		    {
		        Match match = LogEntryRegex.GameEntityRegex.Match(entry.Line);
			    int id = int.Parse(match.Groups["id"].Value);
			    if (!CheckIfEntityExists(id))
			    {
				    CreateEntity(id, "GameEntity");
			    }
		        Database.CurrentGame.CurrentEntityId = id;
		    }
            else if (LogEntryRegex.PlayerEntityRegex.IsMatch(entry.Line))
		    {
		        Match match = LogEntryRegex.PlayerEntityRegex.Match(entry.Line);
		        int id = int.Parse(match.Groups["id"].Value);
			    if (!CheckIfEntityExists(id))
			    {
				    CreateEntity(id);
			    }
		        Database.CurrentGame.CurrentEntityId = id;
		    }
			else if (LogEntryRegex.TagChangeRegex.IsMatch(entry.Line))
		    {
			    Match match = LogEntryRegex.TagChangeRegex.Match(entry.Line);
		        string entityValue = match.Groups["entity"].Value;
			    GameTag tag = GameTagConverter.ParseEnum<GameTag>(match.Groups["tag"].Value);
			    int value = GameTagConverter.ParseTag(tag, match.Groups["value"].Value);
				if (entityValue.StartsWith("[") && LogEntryRegex.EntityRegex.IsMatch(entityValue)) //Find the ID in the nested entity.
		        {
					//[entityName=UNKNOWN ENTITY [cardType=INVALID] id=30 zone=DECK zonePos=0 cardId= player=1]
					Match nestedEntity = LogEntryRegex.EntityRegex.Match(entityValue);
		            int entityId = int.Parse(nestedEntity.Groups["id"].Value);

			        if (!CheckIfEntityExists(entityId))
			        {
				        CreateEntity(entityId);
			        }

		            UpdateEntity(entityId, tag, value);
                }
				else if (int.TryParse(entityValue, out int entityId)) //Try and find the ID.
				{
					if (!CheckIfEntityExists(entityId))
					{
						CreateEntity(entityId);
					}

					UpdateEntity(entityId, tag, value);
                }
				else //This is if we dont have the ID of the entity.
				{
					Entity entity = Database.CurrentGame.Entities.FirstOrDefault(x => x.Value.Name == entityValue).Value;

					if (entity == null)
					{
						var unnamedPlayers = Database.CurrentGame.Entities
							.Where(x => x.Value.Tags.Any(y => y.EnumId == (int) GameTag.PLAYER_ID) &&
							            string.IsNullOrWhiteSpace(x.Value.Name)).ToList();
						Entity tempEntity;

						if (!CheckIfTempEntityExists(entityValue))
						{
							tempEntity = CreateTempEntity(entityValue);
						}
						else
						{
							tempEntity = _tempEntities.First(x => x.Name == entityValue);
						}

						if (unnamedPlayers.Count == 1)
						{
							entity = unnamedPlayers.Single().Value;
						}
						else if (unnamedPlayers.Count == 2 && tag == GameTag.CURRENT_PLAYER && value == 0)
						{
							entity = Database.CurrentGame.Entities
								.FirstOrDefault(x => x.Value.Tags.Any(y => y.EnumId == (int) GameTag.CURRENT_PLAYER)).Value;
						}
						if (entity != null)
						{
							entity.Name = tempEntity.Name;
							entity.Tags = tempEntity.Tags;
							_tempEntities.Remove(tempEntity);
							UpdateEntity(entity.EntityId, tag, value);
						}
						if (_tempEntities.Contains(tempEntity))
						{
							tempEntity.Tags.Add(new Tag {EnumId = (int) tag, Value = value});
							Player player = null;
							if (tempEntity.Name == Database.CurrentGame.User.Name)
							{
								player = Database.CurrentGame.User;
							}
							else if (tempEntity.Name == Database.CurrentGame.Opponent.Name)
							{
								player = Database.CurrentGame.Opponent;
							}
							if (player != null)
							{
								Entity playerEntity = Database.CurrentGame.Entities
									.FirstOrDefault(x => x.Value.Tags.Any(y => y.EnumId == (int) GameTag.PLAYER_ID)).Value;
								if (playerEntity != null)
								{
									playerEntity.Name = tempEntity.Name;
									playerEntity.Tags = tempEntity.Tags;
									_tempEntities.Remove(tempEntity);
								}
							}
						}
					}
					else
					{
						UpdateEntity(entity.EntityId, tag, value);
                    }
				}
		    }
			else if (LogEntryRegex.CreationRegexUpdating.IsMatch(entry.Line))
		    {
			    Match match = LogEntryRegex.CreationRegexUpdating.Match(entry.Line);
			    int id = int.Parse(match.Groups[2].Value);
			    string cardId = match.Groups[5].Value;
			    Zone zone = GameTagConverter.ParseEnum<Zone>(match.Groups["zone"].Value);

			    if (!CheckIfEntityExists(id))
			    {
				    Database.CurrentGame.Entities.Add(id, new Entity {EntityId = id, CardId = cardId});
			    }
			    else
			    {
				    Database.CurrentGame.Entities[id].CardId = cardId;
			    }

			    Database.CurrentGame.CurrentEntityId = id;
		    }
			else if (LogEntryRegex.UpdatingEntityRegex.IsMatch(entry.Line))
		    {
			    Match match = LogEntryRegex.UpdatingEntityRegex.Match(entry.Line);
			    string cardId = match.Groups["cardId"].Value;
			    string entity = match.Groups["entity"].Value;
			    string type = match.Groups["type"].Value;
			    int entityId;

			    if (entity.StartsWith("[") && LogEntryRegex.EntityRegex.IsMatch(entity))
			    {
				    Match nestedMatch = LogEntryRegex.EntityRegex.Match(entry.Line);
				    entityId = int.Parse(nestedMatch.Groups["id"].Value);
			    }
				else if (!int.TryParse(entity, out entityId))
			    {
				    entityId = -1;
			    }

			    if (entityId != -1)
			    {
				    if (!CheckIfEntityExists(entityId))
				    {
					    CreateEntity(entityId);
				    }
				    if (type != "CHANGE_ENTITY" || string.IsNullOrWhiteSpace(Database.CurrentGame.Entities[entityId].CardId)) //If the cardId has been revealed to us!
				    {
					    Database.CurrentGame.Entities[entityId].CardId = cardId;
				    }

				    Database.CurrentGame.CurrentEntityId = entityId;
			    }
		    }
			else if (LogEntryRegex.CreationTagRegex.IsMatch(entry.Line) && !entry.Line.Contains("HIDE_ENTITY"))
		    {
			    Match match = LogEntryRegex.CreationTagRegex.Match(entry.Line);
			    GameTag tag = GameTagConverter.ParseEnum<GameTag>(match.Groups["tag"].Value);
		        int value = GameTagConverter.ParseTag(tag, match.Groups["value"].Value);

                //D 12:05:41.2181993 GameState.DebugPrintPower() -         tag=CARDTYPE value=GAME

                UpdateEntity(Database.CurrentGame.CurrentEntityId, tag, value);
		        TagChangeActions.TagChange(Database.CurrentGame.CurrentEntityId, tag, value);
            }
            else if (LogEntryRegex.CreationRegexCreating.IsMatch(entry.Line))
		    {
		        Match match = LogEntryRegex.CreationRegexCreating.Match(entry.Line);
		        int id = int.Parse(match.Groups[1].Value);
		        string cardId = match.Groups[2].Value;

		        if (!CheckIfEntityExists(id))
		        {
		            CreateCardEntity(id, cardId);
		        }

		        Database.CurrentGame.CurrentEntityId = id;
		    }
		}

	    private void UpdateEntityDetails(int entityId, string name, string cardId)
	    {
	        Entity entity = GetEntity(entityId);
	        if (entity != null)
	        {
	            entity.Name = name;
	            entity.CardId = cardId;
	        }
	    }

	    private Entity GetEntity(int id)
	    {
	        if (CheckIfEntityExists(id))
	        {
	            return Database.CurrentGame.Entities.FirstOrDefault(x => x.Key == id).Value;
	        }

	        return null;
	    }

		private bool CheckIfEntityExists(int id)
		{
			return Database.CurrentGame.Entities.ContainsKey(id);
		}

		private bool CheckIfTempEntityExists(string name)
		{
			return _tempEntities.Any(x => x.Name == name);
		}

		private void CreateEntity(int entityId, string name = "")
		{
			Database.CurrentGame.Entities.Add(entityId, new Entity { Name = name, EntityId = entityId });
			Log.Info($"Created entity ID:{entityId}, NAME:{name}");
		}

		private void CreateCardEntity(int entityId, string cardId)
		{
			Database.CurrentGame.Entities.Add(entityId, new Entity { CardId = cardId, EntityId = entityId });
			Log.Info($"Created entity ID:{entityId}, CARDID:{cardId}");
		}

		private Entity CreateTempEntity(string name)
		{
			Entity tempEntity = new Entity{Name = name};
			_tempEntities.Add(tempEntity);
			Log.Info($"Created temp entity NAME:{name}");

			return tempEntity;
		}

	    private void UpdateEntity(int entityId, GameTag tag, int value)
	    {
		    Entity entityToUpdate = Database.CurrentGame.Entities.FirstOrDefault(x => x.Key == entityId).Value;
		    Tag tagToUpdate = entityToUpdate.Tags.FirstOrDefault(x => x.EnumId == (int) tag);
		    if (tagToUpdate != null)
		    {
			    int oldValue = tagToUpdate.Value;
				tagToUpdate.Value = value;
				Log.Info($"Updated tag for entity ENTITY:{entityToUpdate.EntityId} - {entityToUpdate.Name} ID:{tagToUpdate.EnumId}, OLDVALUE:{oldValue} -> NEWVALUE:{value}");
		    }
		    else
		    {
			    tagToUpdate = new Tag
			    {
				    EnumId = (int) tag,
				    Value = value
			    };
			    entityToUpdate.Tags.Add(tagToUpdate);
				Log.Info($"Added tag to ENTITY:{entityToUpdate.EntityId} - {entityToUpdate.Name} ID:{tagToUpdate.EnumId}, VALUE:{value}");
		    }
	    }
	}
}
