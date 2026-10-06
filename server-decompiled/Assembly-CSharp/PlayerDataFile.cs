using System;
using System.Collections.Generic;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x020012C9 RID: 4809
public class PlayerDataFile
{
	// Token: 0x06009849 RID: 38985 RVA: 0x00397024 File Offset: 0x00395224
	public void ToPlayer(EntityPlayer _player, StreamModeRead _eStreamMode)
	{
		if (this.id != -1)
		{
			_player.entityId = this.id;
		}
		if (this.ecd.stats != null)
		{
			_player.SetStats(this.ecd.stats);
		}
		_player.position = this.ecd.pos;
		_player.rotation = this.ecd.rot;
		StreamUtils.FromBlob(this.bagData, delegate(PooledBinaryReader pbr)
		{
			_player.bag.ReadInto(pbr, _eStreamMode);
		});
		StreamUtils.FromBlob(this.equipmentData, delegate(PooledBinaryReader pbr)
		{
			_player.equipment.ReadInto(pbr, _eStreamMode);
		});
		List<ItemStack> overflow = null;
		StreamUtils.FromBlob(this.inventoryData, delegate(PooledBinaryReader pbr)
		{
			overflow = _player.inventory.ReadInto(pbr, _eStreamMode);
		});
		int num = 0;
		while (overflow != null && num < overflow.Count)
		{
			if (!_player.bag.AddItem(overflow[num]) && _player.inventory.ItemGrid.AddItem(overflow[num], -1) < 0)
			{
				GameManager.Instance.ItemDropServer(overflow[num], _player.GetDropPosition(), Vector3.zero, _player.entityId, 60f, false);
				Manager.BroadcastPlay("itemdropped");
			}
			num++;
		}
		if (this.spawnPoints.Count > 0)
		{
			_player.SpawnPoints.Set(this.spawnPoints[0]);
		}
		_player.onGround = this.ecd.onGround;
		_player.selectedSpawnPointKey = this.selectedSpawnPointKey;
		_player.lastSpawnPosition = this.lastSpawnPosition;
		_player.belongsPlayerId = this.id;
		_player.KilledPlayers = this.playerKills;
		_player.KilledZombies = this.zombieKills;
		_player.Died = this.deaths;
		_player.Score = this.score;
		_player.navMarkerHidden = this.markerHidden;
		_player.markerPosition = this.markerPosition;
		_player.CrouchingLocked = this.bCrouchedLocked;
		_player.deathUpdateTime = this.deathUpdateTime;
		if (this.bDead)
		{
			_player.SetDead();
		}
		EntityPlayerLocal entityPlayerLocal = _player as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			CraftingManager.AlreadyCraftedList = this.alreadyCraftedList;
			for (int i = 0; i < this.unlockedRecipeList.Count; i++)
			{
				CraftingManager.UnlockedRecipeList.Add(this.unlockedRecipeList[i]);
			}
			for (int j = 0; j < this.favoriteRecipeList.Count; j++)
			{
				CraftingManager.FavoriteRecipeList.Add(this.favoriteRecipeList[j]);
			}
			entityPlayerLocal.DragAndDropItem = this.dragAndDropItem;
		}
		StreamUtils.FromBlob(this.progressionData, delegate(PooledBinaryReader pbr)
		{
			_player.Progression = Progression.Read(pbr, _player);
		});
		if (this.buffData.Length > 0L && _player.Buffs == null)
		{
			_player.Buffs = new EntityBuffs(_player);
		}
		StreamUtils.FromBlob(this.buffData, delegate(PooledBinaryReader pbr)
		{
			_player.Buffs.Read(pbr);
		});
		StreamUtils.FromBlob(this.stealthData, delegate(PooledBinaryReader pbr)
		{
			_player.Stealth = PlayerStealth.Read(_player, pbr);
		});
		if (this.ownedEntities.Count > 0)
		{
			for (int k = 0; k < this.ownedEntities.Count; k++)
			{
				_player.AddOwnedEntity(this.ownedEntities[k]);
			}
		}
		_player.totalItemsCrafted = this.totalItemsCrafted;
		_player.distanceWalked = this.distanceWalked;
		_player.longestLife = this.longestLife;
		_player.currentLife = this.currentLife;
		_player.totalTimePlayed = this.totalTimePlayed;
		ulong worldTime = _player.world.worldTime;
		if (worldTime != 0UL && this.gameStageBornAtWorldTime > worldTime)
		{
			this.gameStageBornAtWorldTime = worldTime;
		}
		_player.gameStageBornAtWorldTime = this.gameStageBornAtWorldTime;
		_player.Waypoints = this.waypoints;
		_player.QuestJournal = this.questJournal;
		_player.QuestJournal.OwnerPlayer = _player as EntityPlayerLocal;
		_player.challengeJournal = this.challengeJournal;
		_player.challengeJournal.Player = _player as EntityPlayerLocal;
		_player.RentedVMPosition = this.rentedVMPosition;
		_player.RentalEndTime = this.rentalEndTime;
		_player.RentalEndDay = this.rentalEndDay;
		if (_player is EntityPlayerLocal)
		{
			for (int l = 0; l < _player.Waypoints.Collection.list.Count; l++)
			{
				Waypoint waypoint = _player.Waypoints.Collection.list[l];
				waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", waypoint.pos.ToVector3(), waypoint.icon, waypoint.hiddenOnCompass, -1, null);
				waypoint.navObject.IsActive = waypoint.bTracked;
				waypoint.navObject.name = waypoint.name.Text;
				waypoint.navObject.usingLocalizationId = waypoint.bUsingLocalizationId;
				waypoint.navObject.hiddenOnMap = waypoint.HiddenOnMap;
			}
		}
		_player.favoriteCreativeStacks = this.favoriteCreativeStacks;
		_player.favoriteShapes = this.favoriteShapes;
	}

	// Token: 0x0600984A RID: 38986 RVA: 0x0039761C File Offset: 0x0039581C
	public void SetInventoryFrom(EntityPlayer _player, StreamModeWrite _eStreamMode)
	{
		this.inventoryData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.inventory.Write(pbw, _eStreamMode);
		});
		this.bagData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.bag.Write(pbw, _eStreamMode);
		});
		this.equipmentData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.equipment.Write(pbw, _eStreamMode);
		});
	}

	// Token: 0x0600984B RID: 38987 RVA: 0x00397684 File Offset: 0x00395884
	public void FromPlayer(EntityPlayer _player, StreamModeWrite _eStreamMode = StreamModeWrite.Persistency)
	{
		this.ecd = new EntityCreationData(_player, StreamModeWrite.ToClient);
		this.SetInventoryFrom(_player, _eStreamMode);
		this.spawnPoints = new List<Vector3i>(new Vector3i[0]);
		this.selectedSpawnPointKey = _player.selectedSpawnPointKey;
		this.lastSpawnPosition = _player.lastSpawnPosition;
		this.playerKills = _player.KilledPlayers;
		this.zombieKills = _player.KilledZombies;
		this.deaths = _player.Died;
		this.score = _player.Score;
		this.deathUpdateTime = _player.deathUpdateTime;
		this.bDead = _player.IsDead();
		this.id = _player.entityId;
		this.markerPosition = _player.markerPosition;
		this.markerHidden = _player.navMarkerHidden;
		this.bCrouchedLocked = _player.CrouchingLocked;
		EntityPlayerLocal entityPlayerLocal = _player as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			this.alreadyCraftedList = CraftingManager.AlreadyCraftedList;
			this.unlockedRecipeList.AddRange(CraftingManager.UnlockedRecipeList);
			this.favoriteRecipeList.AddRange(CraftingManager.FavoriteRecipeList);
			this.dragAndDropItem = entityPlayerLocal.DragAndDropItem;
		}
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_player as EntityPlayerLocal);
		if (_player is EntityPlayerLocal && uiforPlayer.xui != null && uiforPlayer.xui.IsReady)
		{
			this.craftingData = uiforPlayer.xui.GetCraftingData();
		}
		this.progressionData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.Progression.Write(pbw, false);
		});
		this.buffData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.Buffs.Write(pbw, false);
		});
		this.stealthData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_player.Stealth.Write(pbw);
		});
		this.ownedEntities = new List<OwnedEntityData>(_player.ownedEntities);
		this.totalItemsCrafted = _player.totalItemsCrafted;
		this.distanceWalked = _player.distanceWalked;
		this.longestLife = _player.longestLife;
		this.currentLife = _player.currentLife;
		this.totalTimePlayed = _player.totalTimePlayed;
		if (_player.gameStageBornAtWorldTime > _player.world.worldTime)
		{
			_player.gameStageBornAtWorldTime = _player.world.worldTime;
		}
		this.gameStageBornAtWorldTime = _player.gameStageBornAtWorldTime;
		this.waypoints = _player.Waypoints.Clone();
		this.questJournal.ClearSharedQuestMarkers();
		this.questJournal = _player.QuestJournal.Clone();
		this.challengeJournal = _player.challengeJournal.Clone();
		this.rentedVMPosition = _player.RentedVMPosition;
		this.rentalEndTime = _player.RentalEndTime;
		this.rentalEndDay = _player.RentalEndDay;
		this.favoriteCreativeStacks = new List<ushort>(_player.favoriteCreativeStacks);
		this.favoriteShapes = new List<string>(_player.favoriteShapes);
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		PersistentPlayerData persistentPlayerData = ((persistentPlayers != null) ? persistentPlayers.GetPlayerDataFromEntityID(_player.entityId) : null);
		this.metadata = new PlayerMetaInfo((persistentPlayerData != null) ? persistentPlayerData.NativeId : null, _player.EntityName, _player.Progression.Level, _player.distanceWalked);
		this.bLoaded = true;
	}

	// Token: 0x0600984C RID: 38988 RVA: 0x00397A38 File Offset: 0x00395C38
	public void ToggleWaypointHiddenStatus(NavObject nav)
	{
		Waypoint waypointForNavObject = this.waypoints.GetWaypointForNavObject(nav);
		if (waypointForNavObject != null)
		{
			waypointForNavObject.hiddenOnCompass = nav.hiddenOnCompass;
		}
	}

	// Token: 0x0600984D RID: 38989 RVA: 0x00397A64 File Offset: 0x00395C64
	public void Load(string _dir, string _playerName)
	{
		try
		{
			string text = string.Concat(new string[]
			{
				_dir,
				"/",
				_playerName,
				".",
				PlayerDataFile.EXT
			});
			if (SdFile.Exists(text))
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						if (pooledBinaryReader.ReadChar() == 't' && pooledBinaryReader.ReadChar() == 't' && pooledBinaryReader.ReadChar() == 'p' && pooledBinaryReader.ReadChar() == '\0')
						{
							uint num = (uint)pooledBinaryReader.ReadByte();
							this.Read(pooledBinaryReader, num, StreamModeRead.Persistency);
							this.bLoaded = true;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			try
			{
				Log.Error(string.Concat(new string[] { "Loading player data failed for player '", _playerName, "', rolling back: ", ex.Message, "\n", ex.StackTrace }));
				string text2 = string.Concat(new string[]
				{
					_dir,
					"/",
					_playerName,
					".",
					PlayerDataFile.EXT,
					".bak"
				});
				if (SdFile.Exists(text2))
				{
					using (Stream stream2 = SdFile.OpenRead(text2))
					{
						using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader2.SetBaseStream(stream2);
							if (pooledBinaryReader2.ReadChar() == 't' && pooledBinaryReader2.ReadChar() == 't' && pooledBinaryReader2.ReadChar() == 'p' && pooledBinaryReader2.ReadChar() == '\0')
							{
								uint num2 = (uint)pooledBinaryReader2.ReadByte();
								this.Read(pooledBinaryReader2, num2, StreamModeRead.Persistency);
								this.bLoaded = true;
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Error(string.Concat(new string[] { "Loading backup player data failed for player '", _playerName, "', rolling back: ", ex2.Message, "\n", ex2.StackTrace }));
			}
		}
	}

	// Token: 0x0600984E RID: 38990 RVA: 0x00397D08 File Offset: 0x00395F08
	public void ReadNetwork(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		this.Read(_br, uint.MaxValue, _eStreamMode);
		this.metadata = PlayerMetaInfo.FromStream(_br);
	}

	// Token: 0x0600984F RID: 38991 RVA: 0x00397D20 File Offset: 0x00395F20
	[PublicizedFrom(EAccessModifier.Private)]
	public void Read(PooledBinaryReader _br, uint _version, StreamModeRead _eStreamMode)
	{
		if (_version > 37U)
		{
			this.ecd = new EntityCreationData();
			this.ecd.read(_br, _eStreamMode);
			if (_version >= 62U)
			{
				this.inventoryData = StreamUtils.ReadBlob(_br);
			}
			else
			{
				ItemStackGrid itemGrid = ItemStackGrid.Create(new Vector2i(0, 0), XUiC_ItemStack.StackLocationTypes.ToolBelt, false, false, null);
				if (_version >= 61U)
				{
					StreamUtils.FromBlob(StreamUtils.ReadBlob(_br), delegate(PooledBinaryReader pbr)
					{
						itemGrid.ReadInto(pbr, StreamModeRead.Persistency);
					});
				}
				else if (_version >= 60U)
				{
					itemGrid.ReadInto(_br, _eStreamMode);
				}
				else
				{
					ItemStack[] array = ((_version >= 10U) ? GameUtils.ReadItemStack(_br) : GameUtils.ReadItemStackOld(_br));
					itemGrid.Resize(new Vector2i(array.Length, 1));
					itemGrid.SetItems(array, false);
				}
				Inventory inventory = new Inventory(itemGrid, (int)_br.ReadByte());
				this.inventoryData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
				{
					inventory.Write(pbw, StreamModeWrite.Persistency);
				});
			}
			if (_version >= 61U)
			{
				this.bagData = StreamUtils.ReadBlob(_br);
			}
			else if (_version >= 58U)
			{
				Bag legacyBag2 = Bag.Read(_br, XUiC_ItemStack.StackLocationTypes.Backpack, _eStreamMode, null);
				this.bagData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
				{
					legacyBag2.Write(pbw, StreamModeWrite.Persistency);
				});
			}
			else
			{
				ItemStack[] array2 = GameUtils.ReadItemStack(_br);
				PackedBoolArray packedBoolArray = null;
				if (_version >= 57U)
				{
					if (_br.ReadBoolean())
					{
						packedBoolArray = new PackedBoolArray(0);
						packedBoolArray.Read(_br);
					}
					else
					{
						packedBoolArray = null;
					}
				}
				else if (_version >= 55U)
				{
					ushort num = _br.ReadUInt16();
					if (num == 0)
					{
						packedBoolArray = null;
					}
					else
					{
						packedBoolArray = new PackedBoolArray((int)num);
						for (int i = 0; i < (int)num; i++)
						{
							packedBoolArray[i] = _br.ReadBoolean();
						}
					}
				}
				else if (_version >= 53U)
				{
					int num2 = _br.ReadInt32();
					packedBoolArray = new PackedBoolArray(num2);
					for (int j = 0; j < num2; j++)
					{
						packedBoolArray[j] = true;
					}
				}
				if (packedBoolArray == null)
				{
					packedBoolArray = new PackedBoolArray(array2.Length);
				}
				packedBoolArray.Length = array2.Length;
				Bag legacyBag = new Bag(new Vector2i(array2.Length, 1), XUiC_ItemStack.StackLocationTypes.Backpack, null);
				legacyBag.ItemGrid.SetItems(array2, false);
				legacyBag.ItemGrid.SetSlotLocks(packedBoolArray);
				this.bagData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
				{
					legacyBag.Write(pbw, StreamModeWrite.Persistency);
				});
			}
			if (_version >= 52U)
			{
				ItemStack[] array3 = GameUtils.ReadItemStack(_br);
				if (array3 != null && array3.Length != 0)
				{
					this.dragAndDropItem = array3[0];
				}
			}
			this.alreadyCraftedList = new HashSet<string>();
			int num3 = (int)_br.ReadUInt16();
			for (int k = 0; k < num3; k++)
			{
				this.alreadyCraftedList.Add(_br.ReadString());
			}
			byte b = _br.ReadByte();
			for (int l = 0; l < (int)b; l++)
			{
				this.spawnPoints.Add(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
			}
			this.selectedSpawnPointKey = _br.ReadInt64();
			_br.ReadBoolean();
			_br.ReadInt16();
			this.bLoaded = _br.ReadBoolean();
			this.lastSpawnPosition = new SpawnPosition(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), _br.ReadSingle());
			this.id = _br.ReadInt32();
			if (_version < 49U)
			{
				_br.ReadInt32();
				_br.ReadInt32();
				_br.ReadInt32();
			}
			this.playerKills = _br.ReadInt32();
			this.zombieKills = _br.ReadInt32();
			this.deaths = _br.ReadInt32();
			this.score = _br.ReadInt32();
			if (_version >= 61U)
			{
				this.equipmentData = StreamUtils.ReadBlob(_br);
			}
			else
			{
				Equipment legacyEquipment = Equipment.Read(_br, _eStreamMode);
				this.equipmentData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
				{
					legacyEquipment.Write(pbw, StreamModeWrite.Persistency);
				});
			}
			this.unlockedRecipeList = new List<string>();
			num3 = (int)_br.ReadUInt16();
			for (int m = 0; m < num3; m++)
			{
				this.unlockedRecipeList.Add(_br.ReadString());
			}
			_br.ReadUInt16();
			this.markerPosition = StreamUtils.ReadVector3i(_br);
			if (_version > 49U)
			{
				this.markerHidden = _br.ReadBoolean();
			}
			if (_version < 54U)
			{
				Equipment.Read(_br, _eStreamMode);
			}
			this.bCrouchedLocked = _br.ReadBoolean();
			if (_version >= 59U)
			{
				this.craftingData.Read(_br);
			}
			else
			{
				this.craftingData.ReadLegacy(_br);
			}
			this.favoriteRecipeList = new List<string>();
			num3 = (int)_br.ReadUInt16();
			for (int n = 0; n < num3; n++)
			{
				this.favoriteRecipeList.Add(_br.ReadString());
			}
			this.totalItemsCrafted = _br.ReadUInt32();
			this.distanceWalked = _br.ReadSingle();
			this.longestLife = _br.ReadSingle();
			this.gameStageBornAtWorldTime = _br.ReadUInt64();
			this.waypoints = new WaypointCollection();
			this.waypoints.Read(_br);
			this.questJournal = new QuestJournal();
			this.questJournal.Read(_br);
			this.deathUpdateTime = _br.ReadInt32();
			this.currentLife = _br.ReadSingle();
			this.bDead = _br.ReadBoolean();
			_br.ReadByte();
			this.bModdedSaveGame = _br.ReadBoolean();
			if (this.bModdedSaveGame)
			{
				Log.Out("Modded save game");
			}
			this.challengeJournal = new ChallengeJournal();
			this.challengeJournal.Read(_br);
			this.rentedVMPosition = StreamUtils.ReadVector3i(_br);
			if (_version <= 38U)
			{
				this.rentalEndTime = _br.ReadUInt64();
			}
			else
			{
				this.rentalEndDay = _br.ReadInt32();
			}
			int num4;
			if (_version <= 55U)
			{
				num4 = (int)_br.ReadUInt16();
				for (int num5 = 0; num5 < num4; num5++)
				{
					_br.ReadInt32();
				}
			}
			this.progressionData = StreamUtils.ReadBlob(_br);
			this.buffData = StreamUtils.ReadBlob(_br);
			this.stealthData = StreamUtils.ReadBlob(_br);
			this.favoriteCreativeStacks.Clear();
			num4 = (int)_br.ReadUInt16();
			for (int num6 = 0; num6 < num4; num6++)
			{
				this.favoriteCreativeStacks.Add(_br.ReadUInt16());
			}
			if (_version > 50U)
			{
				this.favoriteShapes.Clear();
				num4 = (int)_br.ReadUInt16();
				for (int num7 = 0; num7 < num4; num7++)
				{
					this.favoriteShapes.Add(_br.ReadString());
				}
			}
			if (_version > 44U)
			{
				num4 = (int)_br.ReadUInt16();
				this.ownedEntities.Clear();
				for (int num8 = 0; num8 < num4; num8++)
				{
					if (_version > 47U)
					{
						OwnedEntityData ownedEntityData = new OwnedEntityData();
						ownedEntityData.Read(_br);
						this.ownedEntities.Add(ownedEntityData);
					}
					else
					{
						int num9 = _br.ReadInt32();
						int num10 = -1;
						if (_version > 46U)
						{
							num10 = _br.ReadInt32();
						}
						this.ownedEntities.Add(new OwnedEntityData(num9, num10));
					}
				}
			}
			if (_version > 45U)
			{
				this.totalTimePlayed = _br.ReadSingle();
			}
		}
	}

	// Token: 0x06009850 RID: 38992 RVA: 0x003983D8 File Offset: 0x003965D8
	public void Save(string _dir, string _playerId)
	{
		try
		{
			if (!SdDirectory.Exists(_dir))
			{
				SdDirectory.CreateDirectory(_dir);
			}
			string text = string.Concat(new string[]
			{
				_dir,
				"/",
				_playerId,
				".",
				PlayerDataFile.EXT
			});
			if (SdFile.Exists(text))
			{
				SdFile.Copy(text, text + ".bak", true);
			}
			if (SdFile.Exists(text + ".tmp"))
			{
				SdFile.Delete(text + ".tmp");
			}
			using (Stream stream = SdFile.Open(text + ".tmp", FileMode.CreateNew, FileAccess.Write, FileShare.Read))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					pooledBinaryWriter.Write('t');
					pooledBinaryWriter.Write('t');
					pooledBinaryWriter.Write('p');
					pooledBinaryWriter.Write(0);
					pooledBinaryWriter.Write(62);
					this.Write(pooledBinaryWriter, StreamModeWrite.Persistency);
					this.bModifiedSinceLastSave = false;
				}
			}
			if (SdFile.Exists(text + ".tmp"))
			{
				SdFile.Copy(text + ".tmp", text, true);
				SdFile.Delete(text + ".tmp");
			}
			this.metadata.Write(text + ".meta");
		}
		catch (Exception ex)
		{
			Log.Error("Save PlayerData file: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	// Token: 0x06009851 RID: 38993 RVA: 0x0039858C File Offset: 0x0039678C
	public void WriteNetwork(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		this.Write(_bw, _eStreamMode);
		this.metadata.Write(_bw);
	}

	// Token: 0x06009852 RID: 38994 RVA: 0x003985A4 File Offset: 0x003967A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		this.ecd.write(_bw, _eStreamMode);
		StreamUtils.WriteBlob(_bw, this.inventoryData);
		StreamUtils.WriteBlob(_bw, this.bagData);
		GameUtils.WriteItemStack(_bw, new List<ItemStack> { this.dragAndDropItem });
		_bw.Write((ushort)this.alreadyCraftedList.Count);
		foreach (string text in this.alreadyCraftedList)
		{
			_bw.Write(text);
		}
		_bw.Write(0);
		_bw.Write(this.selectedSpawnPointKey);
		_bw.Write(true);
		_bw.Write(0);
		_bw.Write(this.bLoaded);
		_bw.Write((int)this.lastSpawnPosition.position.x);
		_bw.Write((int)this.lastSpawnPosition.position.y);
		_bw.Write((int)this.lastSpawnPosition.position.z);
		_bw.Write(this.lastSpawnPosition.heading);
		_bw.Write(this.id);
		_bw.Write(this.playerKills);
		_bw.Write(this.zombieKills);
		_bw.Write(this.deaths);
		_bw.Write(this.score);
		StreamUtils.WriteBlob(_bw, this.equipmentData);
		_bw.Write((ushort)this.unlockedRecipeList.Count);
		foreach (string text2 in this.unlockedRecipeList)
		{
			_bw.Write(text2);
		}
		_bw.Write(1);
		StreamUtils.Write(_bw, this.markerPosition);
		_bw.Write(this.markerHidden);
		_bw.Write(this.bCrouchedLocked);
		this.craftingData.Write(_bw);
		_bw.Write((ushort)this.favoriteRecipeList.Count);
		foreach (string text3 in this.favoriteRecipeList)
		{
			_bw.Write(text3);
		}
		_bw.Write(this.totalItemsCrafted);
		_bw.Write(this.distanceWalked);
		_bw.Write(this.longestLife);
		_bw.Write(this.gameStageBornAtWorldTime);
		this.waypoints.Write(_bw);
		this.questJournal.Write(_bw);
		_bw.Write(this.deathUpdateTime);
		_bw.Write(this.currentLife);
		_bw.Write(this.bDead);
		_bw.Write(88);
		_bw.Write(this.bModdedSaveGame);
		this.challengeJournal.Write(_bw);
		StreamUtils.Write(_bw, this.rentedVMPosition);
		_bw.Write(this.rentalEndDay);
		StreamUtils.WriteBlob(_bw, this.progressionData);
		StreamUtils.WriteBlob(_bw, this.buffData);
		StreamUtils.WriteBlob(_bw, this.stealthData);
		_bw.Write((ushort)this.favoriteCreativeStacks.Count);
		for (int i = 0; i < this.favoriteCreativeStacks.Count; i++)
		{
			_bw.Write(this.favoriteCreativeStacks[i]);
		}
		_bw.Write((ushort)this.favoriteShapes.Count);
		for (int j = 0; j < this.favoriteShapes.Count; j++)
		{
			_bw.Write(this.favoriteShapes[j]);
		}
		_bw.Write((ushort)this.ownedEntities.Count);
		for (int k = 0; k < this.ownedEntities.Count; k++)
		{
			this.ownedEntities[k].Write(_bw);
		}
		_bw.Write(this.totalTimePlayed);
	}

	// Token: 0x06009853 RID: 38995 RVA: 0x00398984 File Offset: 0x00396B84
	public static bool Exists(string _dir, string _playerName)
	{
		return SdFile.Exists(string.Concat(new string[]
		{
			_dir,
			"/",
			_playerName,
			".",
			PlayerDataFile.EXT
		}));
	}

	// Token: 0x0400722B RID: 29227
	public static string EXT = "ttp";

	// Token: 0x0400722C RID: 29228
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFileVersion = 62;

	// Token: 0x0400722D RID: 29229
	public bool bLoaded;

	// Token: 0x0400722E RID: 29230
	public bool bModifiedSinceLastSave;

	// Token: 0x0400722F RID: 29231
	public EntityCreationData ecd = new EntityCreationData();

	// Token: 0x04007230 RID: 29232
	public MemoryStream inventoryData = new MemoryStream(0);

	// Token: 0x04007231 RID: 29233
	public MemoryStream bagData = new MemoryStream(0);

	// Token: 0x04007232 RID: 29234
	public MemoryStream equipmentData = new MemoryStream(0);

	// Token: 0x04007233 RID: 29235
	public ItemStack dragAndDropItem = new ItemStack();

	// Token: 0x04007234 RID: 29236
	public List<Vector3i> spawnPoints = new List<Vector3i>();

	// Token: 0x04007235 RID: 29237
	public long selectedSpawnPointKey;

	// Token: 0x04007236 RID: 29238
	public HashSet<string> alreadyCraftedList = new HashSet<string>();

	// Token: 0x04007237 RID: 29239
	public List<string> unlockedRecipeList = new List<string>();

	// Token: 0x04007238 RID: 29240
	public List<string> favoriteRecipeList = new List<string>();

	// Token: 0x04007239 RID: 29241
	public SpawnPosition lastSpawnPosition = SpawnPosition.Undef;

	// Token: 0x0400723A RID: 29242
	public List<OwnedEntityData> ownedEntities = new List<OwnedEntityData>();

	// Token: 0x0400723B RID: 29243
	public int playerKills;

	// Token: 0x0400723C RID: 29244
	public int zombieKills;

	// Token: 0x0400723D RID: 29245
	public int deaths;

	// Token: 0x0400723E RID: 29246
	public int score;

	// Token: 0x0400723F RID: 29247
	public int id = -1;

	// Token: 0x04007240 RID: 29248
	public Vector3i markerPosition;

	// Token: 0x04007241 RID: 29249
	public bool markerHidden;

	// Token: 0x04007242 RID: 29250
	public bool bCrouchedLocked;

	// Token: 0x04007243 RID: 29251
	public CraftingData craftingData = new CraftingData();

	// Token: 0x04007244 RID: 29252
	public int deathUpdateTime;

	// Token: 0x04007245 RID: 29253
	public bool bDead;

	// Token: 0x04007246 RID: 29254
	public float distanceWalked;

	// Token: 0x04007247 RID: 29255
	public uint totalItemsCrafted;

	// Token: 0x04007248 RID: 29256
	public float longestLife;

	// Token: 0x04007249 RID: 29257
	public float currentLife;

	// Token: 0x0400724A RID: 29258
	public float totalTimePlayed;

	// Token: 0x0400724B RID: 29259
	public ulong gameStageBornAtWorldTime = ulong.MaxValue;

	// Token: 0x0400724C RID: 29260
	public MemoryStream progressionData = new MemoryStream(0);

	// Token: 0x0400724D RID: 29261
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream buffData = new MemoryStream(0);

	// Token: 0x0400724E RID: 29262
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream stealthData = new MemoryStream(0);

	// Token: 0x0400724F RID: 29263
	public WaypointCollection waypoints = new WaypointCollection();

	// Token: 0x04007250 RID: 29264
	public QuestJournal questJournal = new QuestJournal();

	// Token: 0x04007251 RID: 29265
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bModdedSaveGame;

	// Token: 0x04007252 RID: 29266
	public ChallengeJournal challengeJournal = new ChallengeJournal();

	// Token: 0x04007253 RID: 29267
	public Vector3i rentedVMPosition = Vector3i.zero;

	// Token: 0x04007254 RID: 29268
	public ulong rentalEndTime;

	// Token: 0x04007255 RID: 29269
	public int rentalEndDay;

	// Token: 0x04007256 RID: 29270
	public List<ushort> favoriteCreativeStacks = new List<ushort>();

	// Token: 0x04007257 RID: 29271
	public List<string> favoriteShapes = new List<string>();

	// Token: 0x04007258 RID: 29272
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerMetaInfo metadata;
}
