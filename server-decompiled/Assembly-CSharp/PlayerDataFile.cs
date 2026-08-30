using System;
using System.Collections.Generic;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x020012B1 RID: 4785
public class PlayerDataFile
{
	// Token: 0x060097BC RID: 38844 RVA: 0x003927BC File Offset: 0x003909BC
	public void ToPlayer(EntityPlayer _player)
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
		_player.inventory.SetSlots(this.inventory, true);
		_player.inventory.SetFocusedItemIdx(this.selectedInventorySlot);
		_player.inventory.SetHoldingItemIdx(this.selectedInventorySlot);
		_player.bag = this.bag;
		if (!_player.isEntityRemote)
		{
			ItemStack item = _player.inventory.GetItem(_player.inventory.DUMMY_SLOT_IDX);
			if (!item.IsEmpty())
			{
				if (!_player.bag.AddItem(item) && !_player.inventory.AddItem(item))
				{
					GameManager.Instance.ItemDropServer(item, _player.GetDropPosition(), Vector3.zero, _player.entityId, 60f, false);
					Manager.BroadcastPlay("itemdropped");
				}
				_player.inventory.SetItem(_player.inventory.DUMMY_SLOT_IDX, ItemStack.Empty);
			}
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
		_player.equipment.Apply(this.equipment, true);
		if (_player == GameManager.Instance.World.GetPrimaryPlayer())
		{
			_player.TurnOffLightFlares();
		}
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
		if (this.progressionData.Length > 0L)
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(this.progressionData);
				_player.Progression = Progression.Read(pooledBinaryReader, _player);
			}
		}
		if (this.buffData.Length > 0L)
		{
			if (_player.Buffs == null)
			{
				_player.Buffs = new EntityBuffs(_player);
			}
			using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader2.SetBaseStream(this.buffData);
				_player.Buffs.Read(pooledBinaryReader2);
			}
		}
		if (this.stealthData.Length > 0L)
		{
			using (PooledBinaryReader pooledBinaryReader3 = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader3.SetBaseStream(this.stealthData);
				_player.Stealth = PlayerStealth.Read(_player, pooledBinaryReader3);
			}
		}
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

	// Token: 0x060097BD RID: 38845 RVA: 0x00392D64 File Offset: 0x00390F64
	public void FromPlayer(EntityPlayer _player)
	{
		this.ecd = new EntityCreationData(_player, true);
		this.inventory = ((_player.AttachedToEntity != null && _player.saveInventory != null) ? ItemStack.Clone(_player.saveInventory.GetSlots()) : ItemStack.Clone(_player.inventory.GetSlots()));
		this.bag = _player.bag.Clone();
		this.equipment = _player.equipment.Clone();
		this.selectedInventorySlot = _player.inventory.holdingItemIdx;
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
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.progressionData);
			_player.Progression.Write(pooledBinaryWriter, false);
		}
		using (PooledBinaryWriter pooledBinaryWriter2 = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter2.SetBaseStream(this.buffData);
			_player.Buffs.Write(pooledBinaryWriter2, false);
		}
		using (PooledBinaryWriter pooledBinaryWriter3 = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter3.SetBaseStream(this.stealthData);
			_player.Stealth.Write(pooledBinaryWriter3);
		}
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

	// Token: 0x060097BE RID: 38846 RVA: 0x00393128 File Offset: 0x00391328
	public void ToggleWaypointHiddenStatus(NavObject nav)
	{
		Waypoint waypointForNavObject = this.waypoints.GetWaypointForNavObject(nav);
		if (waypointForNavObject != null)
		{
			waypointForNavObject.hiddenOnCompass = nav.hiddenOnCompass;
		}
	}

	// Token: 0x060097BF RID: 38847 RVA: 0x00393154 File Offset: 0x00391354
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
							this.Read(pooledBinaryReader, num);
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
								this.Read(pooledBinaryReader2, num2);
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

	// Token: 0x060097C0 RID: 38848 RVA: 0x003933F4 File Offset: 0x003915F4
	public void ReadNetwork(PooledBinaryReader _br)
	{
		this.Read(_br, uint.MaxValue);
		this.metadata = PlayerMetaInfo.FromStream(_br);
	}

	// Token: 0x060097C1 RID: 38849 RVA: 0x0039340C File Offset: 0x0039160C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Read(PooledBinaryReader _br, uint _version)
	{
		if (_version > 37U)
		{
			this.ecd = new EntityCreationData();
			this.ecd.read(_br, false);
			if (_version < 10U)
			{
				this.inventory = GameUtils.ReadItemStackOld(_br);
			}
			else
			{
				this.inventory = GameUtils.ReadItemStack(_br);
			}
			this.selectedInventorySlot = (int)_br.ReadByte();
			if (_version >= 58U)
			{
				this.bag = Bag.Read(_br);
			}
			else
			{
				ItemStack[] array = GameUtils.ReadItemStack(_br);
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
				this.bag = new Bag(array.Length);
				this.bag.SetSlots(array);
				this.bag.LockedSlots = packedBoolArray;
			}
			if (_version >= 52U)
			{
				ItemStack[] array2 = GameUtils.ReadItemStack(_br);
				if (array2 != null && array2.Length != 0)
				{
					this.dragAndDropItem = array2[0];
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
			this.equipment = Equipment.Read(_br);
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
				Equipment.Read(_br);
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
			num4 = _br.ReadInt32();
			this.progressionData = ((num4 > 0) ? new MemoryStream(_br.ReadBytes(num4)) : new MemoryStream());
			num4 = _br.ReadInt32();
			this.buffData = ((num4 > 0) ? new MemoryStream(_br.ReadBytes(num4)) : new MemoryStream());
			num4 = _br.ReadInt32();
			this.stealthData = ((num4 > 0) ? new MemoryStream(_br.ReadBytes(num4)) : new MemoryStream());
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

	// Token: 0x060097C2 RID: 38850 RVA: 0x003939AC File Offset: 0x00391BAC
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
					pooledBinaryWriter.Write(59);
					this.Write(pooledBinaryWriter);
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

	// Token: 0x060097C3 RID: 38851 RVA: 0x00393B60 File Offset: 0x00391D60
	public void WriteNetwork(PooledBinaryWriter _bw)
	{
		this.Write(_bw);
		this.metadata.Write(_bw);
	}

	// Token: 0x060097C4 RID: 38852 RVA: 0x00393B78 File Offset: 0x00391D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void Write(PooledBinaryWriter _bw)
	{
		this.ecd.write(_bw, false);
		GameUtils.WriteItemStack(_bw, this.inventory);
		_bw.Write((byte)this.selectedInventorySlot);
		this.bag.Write(_bw);
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
		this.equipment.Write(_bw);
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
		this.progressionData.Position = 0L;
		_bw.Write((int)this.progressionData.Length);
		StreamUtils.StreamCopy(this.progressionData, _bw.BaseStream, null, true);
		this.buffData.Position = 0L;
		_bw.Write((int)this.buffData.Length);
		StreamUtils.StreamCopy(this.buffData, _bw.BaseStream, null, true);
		this.stealthData.Position = 0L;
		_bw.Write((int)this.stealthData.Length);
		StreamUtils.StreamCopy(this.stealthData, _bw.BaseStream, null, true);
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

	// Token: 0x060097C5 RID: 38853 RVA: 0x00393FD8 File Offset: 0x003921D8
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

	// Token: 0x040071C1 RID: 29121
	public static string EXT = "ttp";

	// Token: 0x040071C2 RID: 29122
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFileVersion = 59;

	// Token: 0x040071C3 RID: 29123
	public bool bLoaded;

	// Token: 0x040071C4 RID: 29124
	public bool bModifiedSinceLastSave;

	// Token: 0x040071C5 RID: 29125
	public EntityCreationData ecd = new EntityCreationData();

	// Token: 0x040071C6 RID: 29126
	public ItemStack[] inventory = new ItemStack[0];

	// Token: 0x040071C7 RID: 29127
	public Bag bag = new Bag(0);

	// Token: 0x040071C8 RID: 29128
	public ItemStack dragAndDropItem = new ItemStack();

	// Token: 0x040071C9 RID: 29129
	public Equipment equipment = new Equipment();

	// Token: 0x040071CA RID: 29130
	public int selectedInventorySlot;

	// Token: 0x040071CB RID: 29131
	public List<Vector3i> spawnPoints = new List<Vector3i>();

	// Token: 0x040071CC RID: 29132
	public long selectedSpawnPointKey;

	// Token: 0x040071CD RID: 29133
	public HashSet<string> alreadyCraftedList = new HashSet<string>();

	// Token: 0x040071CE RID: 29134
	public List<string> unlockedRecipeList = new List<string>();

	// Token: 0x040071CF RID: 29135
	public List<string> favoriteRecipeList = new List<string>();

	// Token: 0x040071D0 RID: 29136
	public SpawnPosition lastSpawnPosition = SpawnPosition.Undef;

	// Token: 0x040071D1 RID: 29137
	public List<OwnedEntityData> ownedEntities = new List<OwnedEntityData>();

	// Token: 0x040071D2 RID: 29138
	public int playerKills;

	// Token: 0x040071D3 RID: 29139
	public int zombieKills;

	// Token: 0x040071D4 RID: 29140
	public int deaths;

	// Token: 0x040071D5 RID: 29141
	public int score;

	// Token: 0x040071D6 RID: 29142
	public int id = -1;

	// Token: 0x040071D7 RID: 29143
	public Vector3i markerPosition;

	// Token: 0x040071D8 RID: 29144
	public bool markerHidden;

	// Token: 0x040071D9 RID: 29145
	public bool bCrouchedLocked;

	// Token: 0x040071DA RID: 29146
	public CraftingData craftingData = new CraftingData();

	// Token: 0x040071DB RID: 29147
	public int deathUpdateTime;

	// Token: 0x040071DC RID: 29148
	public bool bDead;

	// Token: 0x040071DD RID: 29149
	public float distanceWalked;

	// Token: 0x040071DE RID: 29150
	public uint totalItemsCrafted;

	// Token: 0x040071DF RID: 29151
	public float longestLife;

	// Token: 0x040071E0 RID: 29152
	public float currentLife;

	// Token: 0x040071E1 RID: 29153
	public float totalTimePlayed;

	// Token: 0x040071E2 RID: 29154
	public ulong gameStageBornAtWorldTime = ulong.MaxValue;

	// Token: 0x040071E3 RID: 29155
	public MemoryStream progressionData = new MemoryStream(0);

	// Token: 0x040071E4 RID: 29156
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream buffData = new MemoryStream(0);

	// Token: 0x040071E5 RID: 29157
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream stealthData = new MemoryStream(0);

	// Token: 0x040071E6 RID: 29158
	public WaypointCollection waypoints = new WaypointCollection();

	// Token: 0x040071E7 RID: 29159
	public QuestJournal questJournal = new QuestJournal();

	// Token: 0x040071E8 RID: 29160
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bModdedSaveGame;

	// Token: 0x040071E9 RID: 29161
	public ChallengeJournal challengeJournal = new ChallengeJournal();

	// Token: 0x040071EA RID: 29162
	public Vector3i rentedVMPosition = Vector3i.zero;

	// Token: 0x040071EB RID: 29163
	public ulong rentalEndTime;

	// Token: 0x040071EC RID: 29164
	public int rentalEndDay;

	// Token: 0x040071ED RID: 29165
	public List<ushort> favoriteCreativeStacks = new List<ushort>();

	// Token: 0x040071EE RID: 29166
	public List<string> favoriteShapes = new List<string>();

	// Token: 0x040071EF RID: 29167
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerMetaInfo metadata;
}
