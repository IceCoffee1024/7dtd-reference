using System;
using System.Collections.Generic;
using System.Xml;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008FC RID: 2300
[Preserve]
public class PersistentPlayerList
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x0600421B RID: 16923 RVA: 0x0019F780 File Offset: 0x0019D980
	// (remove) Token: 0x0600421C RID: 16924 RVA: 0x0019F7B4 File Offset: 0x0019D9B4
	public static event AllyStore.AllyChangeEvent OnPersistentAllyChangeEvent;

	// Token: 0x0600421D RID: 16925 RVA: 0x0019F7E8 File Offset: 0x0019D9E8
	public PersistentPlayerList()
	{
		this.Players = new ObservableDictionary<PlatformUserIdentifierAbs, PersistentPlayerData>();
		this.EntityToPlayerMap = new Dictionary<int, PersistentPlayerData>();
		this.PlayerToEntityMap = new Dictionary<PlatformUserIdentifierAbs, int>();
		this.m_lpBlockMap = new Dictionary<Vector3i, PersistentPlayerData>();
		this.Allies = new AllyStore();
		this.Allies.OnAllyChangeEvent += delegate(PlatformUserIdentifierAbs source, PlatformUserIdentifierAbs target, AllyStore.AllyEvent allyEvent)
		{
			AllyStore.AllyChangeEvent onPersistentAllyChangeEvent = PersistentPlayerList.OnPersistentAllyChangeEvent;
			if (onPersistentAllyChangeEvent == null)
			{
				return;
			}
			onPersistentAllyChangeEvent(source, target, allyEvent);
		};
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x0019F85C File Offset: 0x0019DA5C
	public void HandlePlayerDetailsUpdate(IPlatformUserData platformUserData, string name)
	{
		PersistentPlayerData persistentPlayerData;
		if (this.Players.TryGetValue(platformUserData.PrimaryId, out persistentPlayerData))
		{
			persistentPlayerData.PlayerName.Update(name, platformUserData.PrimaryId);
		}
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x0019F890 File Offset: 0x0019DA90
	public void PlaceLandProtectionBlock(Vector3i pos, PlatformUserIdentifierAbs owner)
	{
		PersistentPlayerData playerData = this.GetPlayerData(owner);
		if (playerData == null)
		{
			return;
		}
		PersistentPlayerData persistentPlayerData;
		if (this.m_lpBlockMap.TryGetValue(pos, out persistentPlayerData))
		{
			persistentPlayerData.RemoveLandProtectionBlock(pos);
		}
		playerData.AddLandProtectionBlock(pos);
		this.RemoveExtraLandClaims(playerData);
		this.m_lpBlockMap[pos] = playerData;
		NavObjectManager.Instance.RegisterNavObject("land_claim", pos, "", false, -1, null).OwnerEntity = GameManager.Instance.World.GetEntity(this.PlayerToEntityMap[owner]);
		this.SavePersistentPlayerData();
	}

	// Token: 0x06004220 RID: 16928 RVA: 0x0019F920 File Offset: 0x0019DB20
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveExtraLandClaims(PersistentPlayerData owner)
	{
		int @int = GameStats.GetInt(EnumGameStats.LandClaimCount);
		int num = owner.LPBlocks.Count - @int;
		for (int i = 0; i < num; i++)
		{
			Vector3i vector3i = owner.LPBlocks[0];
			owner.LPBlocks.RemoveAt(0);
			TEFeatureLandClaim tefeatureLandClaim;
			if (!GameManager.Instance.World.GetTileEntity(vector3i).TryGetSelfOrFeature<TEFeatureLandClaim>(out tefeatureLandClaim))
			{
				this.m_lpBlockMap.Remove(vector3i);
				Log.Warning(string.Format("[{0}] Could not find Land Claim at position: {1}", "PersistentPlayerList", vector3i));
			}
			else
			{
				TEFeatureLandClaim.HandleDeactivateLandClaim(vector3i);
			}
		}
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0019F9B0 File Offset: 0x0019DBB0
	public void RemoveLandProtectionBlock(Vector3i pos)
	{
		PersistentPlayerData persistentPlayerData;
		if (!this.m_lpBlockMap.TryGetValue(pos, out persistentPlayerData))
		{
			return;
		}
		this.m_lpBlockMap.Remove(pos);
		persistentPlayerData.RemoveLandProtectionBlock(pos);
		if (GameManager.Instance.World != null)
		{
			NavObjectManager.Instance.UnRegisterNavObjectByPosition(pos.ToVector3(), "land_claim");
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.LandClaim, pos.ToVector3()), false, -1, -1, -1, null, 192, false);
			}
		}
		this.SavePersistentPlayerData();
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x0019FA48 File Offset: 0x0019DC48
	public PersistentPlayerData GetLandProtectionBlockOwner(Vector3i pos)
	{
		PersistentPlayerData persistentPlayerData;
		this.m_lpBlockMap.TryGetValue(pos, out persistentPlayerData);
		return persistentPlayerData;
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x0019FA65 File Offset: 0x0019DC65
	public void AddPlayerEventHandler(PersistentPlayerData.PlayerEventHandler handler)
	{
		if (this.m_dispatch == null)
		{
			this.m_dispatch = new List<PersistentPlayerData.PlayerEventHandler>();
		}
		this.m_dispatch.Add(handler);
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x0019FA86 File Offset: 0x0019DC86
	public void RemovePlayerEventHandler(PersistentPlayerData.PlayerEventHandler handler)
	{
		this.m_dispatch.Remove(handler);
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x0019FA98 File Offset: 0x0019DC98
	public void DispatchPlayerEvent(PersistentPlayerData player, PersistentPlayerData otherPlayer, EnumPersistentPlayerDataReason reason)
	{
		if (this.m_dispatch != null)
		{
			for (int i = 0; i < this.m_dispatch.Count; i++)
			{
				this.m_dispatch[i](player, otherPlayer, reason);
			}
		}
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x0019FAD8 File Offset: 0x0019DCD8
	public PersistentPlayerData GetPlayerDataFromEntityID(int _entityId)
	{
		PersistentPlayerData persistentPlayerData;
		if (!this.EntityToPlayerMap.TryGetValue(_entityId, out persistentPlayerData))
		{
			return null;
		}
		return persistentPlayerData;
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x0019FAF8 File Offset: 0x0019DCF8
	public EntityPlayer GetEntityPlayerFromUserId(PlatformUserIdentifierAbs _userIdentifier)
	{
		int num;
		if (this.PlayerToEntityMap.TryGetValue(_userIdentifier, out num))
		{
			EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(num) as EntityPlayer;
			if (entityPlayer != null)
			{
				return entityPlayer;
			}
		}
		return null;
	}

	// Token: 0x06004228 RID: 16936 RVA: 0x0019FB34 File Offset: 0x0019DD34
	public PersistentPlayerData GetPlayerData(PlatformUserIdentifierAbs _userIdentifier)
	{
		if (_userIdentifier == null)
		{
			return null;
		}
		PersistentPlayerData persistentPlayerData;
		if (!this.Players.TryGetValue(_userIdentifier, out persistentPlayerData))
		{
			return null;
		}
		return persistentPlayerData;
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x0019FB5C File Offset: 0x0019DD5C
	public PersistentPlayerData CreatePlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, string _playerName, EPlayGroup _playGroup)
	{
		PersistentPlayerData persistentPlayerData = new PersistentPlayerData(_primaryId, _nativeId, new AuthoredText(_playerName, _primaryId), _playGroup);
		persistentPlayerData.EntityId = -1;
		persistentPlayerData.LastLogin = DateTime.Now;
		this.Players[_primaryId] = persistentPlayerData;
		return persistentPlayerData;
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x0019FB9C File Offset: 0x0019DD9C
	public void UnmapPlayer(PlatformUserIdentifierAbs _userIdentifier)
	{
		PersistentPlayerData playerData = this.GetPlayerData(_userIdentifier);
		if (playerData != null && playerData.EntityId != -1)
		{
			this.EntityToPlayerMap.Remove(playerData.EntityId);
			this.PlayerToEntityMap.Remove(_userIdentifier);
			playerData.EntityId = -1;
		}
	}

	// Token: 0x0600422B RID: 16939 RVA: 0x0019FBE3 File Offset: 0x0019DDE3
	public void MapPlayer(PersistentPlayerData ppd)
	{
		if (ppd.EntityId != -1)
		{
			this.EntityToPlayerMap[ppd.EntityId] = ppd;
			this.PlayerToEntityMap[ppd.PrimaryId] = ppd.EntityId;
		}
	}

	// Token: 0x0600422C RID: 16940 RVA: 0x0019FC18 File Offset: 0x0019DE18
	public void AutoFixNameCollisions()
	{
		if (this.EntityToPlayerMap.Count == 0)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>();
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			hashSet.Add(keyValuePair.Value.PlayerName.AuthoredName.Text);
		}
		foreach (string text in hashSet)
		{
			this.FixNameCollisions(text);
		}
		GameManager.Instance.persistentPlayers.Players.EntryModified += this.NameCollisionEvent;
	}

	// Token: 0x0600422D RID: 16941 RVA: 0x0019FCF0 File Offset: 0x0019DEF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void NameCollisionEvent(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _entry)
	{
		GameManager.Instance.persistentPlayers.FixNameCollisions(_entry.Value.PlayerName.AuthoredName.Text);
	}

	// Token: 0x0600422E RID: 16942 RVA: 0x0019FD18 File Offset: 0x0019DF18
	public void FixNameCollisions(string _name)
	{
		if (this.EntityToPlayerMap.Count == 0 || _name == null)
		{
			return;
		}
		int num = 0;
		IUserClient user = PlatformManager.MultiPlatform.User;
		string text = ((((user != null) ? user.PlatformUserId : null) != null && this.Players.ContainsKey(PlatformManager.MultiPlatform.User.PlatformUserId)) ? this.Players[PlatformManager.MultiPlatform.User.PlatformUserId].PlayerName.AuthoredName.Text : null);
		EPlatformIdentifier platformIdentifier = PlatformManager.NativePlatform.PlatformIdentifier;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
		if (_name == text)
		{
			platformUserIdentifierAbs = PlatformManager.MultiPlatform.User.PlatformUserId;
			num++;
		}
		else
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
			{
				if (platformIdentifier == keyValuePair.Value.PlatformData.NativeId.PlatformIdentifier && _name == keyValuePair.Value.PlayerName.AuthoredName.Text)
				{
					platformUserIdentifierAbs = keyValuePair.Value.PlatformData.PrimaryId;
					num++;
					break;
				}
			}
		}
		if (platformUserIdentifierAbs != null)
		{
			this.Players[platformUserIdentifierAbs].PlayerName.SetCollisionSuffix(0);
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair2 in this.Players)
		{
			if (!keyValuePair2.Key.Equals(platformUserIdentifierAbs) && this.PlayerToEntityMap.ContainsKey(keyValuePair2.Key) && keyValuePair2.Value.PlayerName.AuthoredName.Text == _name)
			{
				keyValuePair2.Value.PlayerName.SetCollisionSuffix(num++);
			}
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair3 in this.Players)
		{
			if (!keyValuePair3.Key.Equals(platformUserIdentifierAbs) && !this.PlayerToEntityMap.ContainsKey(keyValuePair3.Key) && keyValuePair3.Value.PlayerName.AuthoredName.Text == _name)
			{
				keyValuePair3.Value.PlayerName.SetCollisionSuffix(num++);
			}
		}
	}

	// Token: 0x0600422F RID: 16943 RVA: 0x0019FF94 File Offset: 0x0019E194
	public void SetPlayerData(PersistentPlayerData ppData)
	{
		if (ppData.EntityId == -1)
		{
			this.UnmapPlayer(ppData.PrimaryId);
		}
		this.Players[ppData.PrimaryId] = ppData;
		if (ppData.LPBlocks != null)
		{
			for (int i = 0; i < ppData.LPBlocks.Count; i++)
			{
				Vector3i vector3i = ppData.LPBlocks[i];
				this.m_lpBlockMap[vector3i] = ppData;
			}
		}
		this.MapPlayer(ppData);
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x001A0008 File Offset: 0x0019E208
	public PersistentPlayerList NetworkCloneRelevantForPlayer()
	{
		PersistentPlayerList persistentPlayerList = new PersistentPlayerList();
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			persistentPlayerList.Players[keyValuePair.Value.PrimaryId] = keyValuePair.Value;
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair2 in persistentPlayerList.Players)
		{
			if (keyValuePair2.Value.LPBlocks != null)
			{
				for (int i = 0; i < keyValuePair2.Value.LPBlocks.Count; i++)
				{
					Vector3i vector3i = keyValuePair2.Value.LPBlocks[i];
					persistentPlayerList.m_lpBlockMap[vector3i] = keyValuePair2.Value;
				}
			}
		}
		persistentPlayerList.Allies.CopyFrom(this.Allies);
		return persistentPlayerList;
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x001A0110 File Offset: 0x0019E310
	public bool CleanupPlayers()
	{
		List<PersistentPlayerData> list = null;
		double num = (double)GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
		DateTime now = DateTime.Now;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			if (!this.Allies.HasAllies(keyValuePair.Value.PrimaryId) && !keyValuePair.Value.HasBedrollPos && keyValuePair.Value.EntityId == -1 && (now - keyValuePair.Value.LastLogin).TotalHours > num)
			{
				if (list == null)
				{
					list = new List<PersistentPlayerData>();
				}
				list.Add(keyValuePair.Value);
			}
		}
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				PersistentPlayerData persistentPlayerData = list[i];
				if (persistentPlayerData.LPBlocks != null)
				{
					for (int j = 0; j < persistentPlayerData.LPBlocks.Count; j++)
					{
						Vector3i vector3i = persistentPlayerData.LPBlocks[j];
						this.m_lpBlockMap.Remove(vector3i);
					}
				}
				this.Players.Remove(persistentPlayerData.PrimaryId);
			}
		}
		return list != null;
	}

	// Token: 0x06004232 RID: 16946 RVA: 0x001A0258 File Offset: 0x0019E458
	public void SpawnPointRemoved(Vector3i _pos)
	{
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			if (keyValuePair.Value.BedrollPos.Equals(_pos))
			{
				keyValuePair.Value.ClearBedroll();
				break;
			}
		}
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x001A02C0 File Offset: 0x0019E4C0
	public void SavePersistentPlayerData()
	{
		if (!GameManager.Instance.IsEditMode() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.Write(GameIO.GetSaveGameDir() + "/players.xml");
		}
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x001A02F0 File Offset: 0x0019E4F0
	public void Write(PooledBinaryWriter stream)
	{
		stream.Write(this.Players.Count);
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			keyValuePair.Value.Write(stream);
		}
		stream.Write(this.m_lpBlockMap.Count);
		foreach (KeyValuePair<Vector3i, PersistentPlayerData> keyValuePair2 in this.m_lpBlockMap)
		{
			stream.Write(keyValuePair2.Key.x);
			stream.Write(keyValuePair2.Key.y);
			stream.Write(keyValuePair2.Key.z);
			keyValuePair2.Value.PrimaryId.ToStream(stream, false);
		}
		this.Allies.Write(stream);
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x001A03F8 File Offset: 0x0019E5F8
	public static PersistentPlayerList Read(PooledBinaryReader stream)
	{
		PersistentPlayerList persistentPlayerList = new PersistentPlayerList();
		int num = stream.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			PersistentPlayerData persistentPlayerData = PersistentPlayerData.Read(stream);
			persistentPlayerList.Players.Add(persistentPlayerData.PrimaryId, persistentPlayerData);
			persistentPlayerList.MapPlayer(persistentPlayerData);
		}
		int num2 = stream.ReadInt32();
		for (int j = 0; j < num2; j++)
		{
			Vector3i vector3i = new Vector3i(stream.ReadInt32(), stream.ReadInt32(), stream.ReadInt32());
			PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(stream, false, false);
			PersistentPlayerData persistentPlayerData2;
			if (persistentPlayerList.Players.TryGetValue(platformUserIdentifierAbs, out persistentPlayerData2) && persistentPlayerData2 != null)
			{
				persistentPlayerList.m_lpBlockMap[vector3i] = persistentPlayerData2;
			}
		}
		persistentPlayerList.Allies.Read(stream);
		return persistentPlayerList;
	}

	// Token: 0x06004236 RID: 16950 RVA: 0x001A04B0 File Offset: 0x0019E6B0
	public static PersistentPlayerList ReadXML(string filePath)
	{
		Log.Out("Loading players.xml");
		PersistentPlayerList persistentPlayerList = new PersistentPlayerList();
		if (!SdFile.Exists(filePath))
		{
			return persistentPlayerList;
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.SdLoad(filePath);
		}
		catch (XmlException ex)
		{
			Log.Error(string.Format("Failed loading players.xml: {0}", ex.Message));
			return persistentPlayerList;
		}
		XmlElement documentElement = xmlDocument.DocumentElement;
		if (documentElement == null)
		{
			throw new Exception("malformed persistent player data xml file!");
		}
		int num = 0;
		if (documentElement.HasAttribute("version"))
		{
			int.TryParse(documentElement.GetAttribute("version"), out num);
		}
		foreach (object obj in documentElement.ChildNodes)
		{
			XmlElement xmlElement = ((XmlNode)obj) as XmlElement;
			if (xmlElement != null)
			{
				if (xmlElement.Name == "player")
				{
					HashSet<PlatformUserIdentifierAbs> hashSet;
					PersistentPlayerData persistentPlayerData = PersistentPlayerData.ReadXML(xmlElement, num, out hashSet);
					if (persistentPlayerData == null)
					{
						return null;
					}
					persistentPlayerList.Players.Add(persistentPlayerData.PrimaryId, persistentPlayerData);
					if (persistentPlayerData.LPBlocks != null)
					{
						for (int i = 0; i < persistentPlayerData.LPBlocks.Count; i++)
						{
							persistentPlayerList.m_lpBlockMap[persistentPlayerData.LPBlocks[i]] = persistentPlayerData;
						}
					}
					if (hashSet == null)
					{
						continue;
					}
					using (HashSet<PlatformUserIdentifierAbs>.Enumerator enumerator2 = hashSet.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PlatformUserIdentifierAbs platformUserIdentifierAbs = enumerator2.Current;
							persistentPlayerList.Allies.SetStatus(persistentPlayerData.PrimaryId, platformUserIdentifierAbs, AllyStore.AllyStatus.Allies);
						}
						continue;
					}
				}
				if (xmlElement.Name == "allies" && num >= 1)
				{
					persistentPlayerList.Allies.ReadXml(xmlElement, num);
				}
			}
		}
		return persistentPlayerList;
	}

	// Token: 0x06004237 RID: 16951 RVA: 0x001A06C8 File Offset: 0x0019E8C8
	public void Write(string filePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		XmlElement xmlElement = xmlDocument.AddXmlElement("persistentplayerdata");
		xmlElement.SetAttribute("version", 1.ToString());
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in this.Players)
		{
			keyValuePair.Value.Write(xmlElement);
		}
		this.Allies.WriteXml(xmlElement);
		xmlDocument.SdSave(filePath);
	}

	// Token: 0x06004238 RID: 16952 RVA: 0x001A075C File Offset: 0x0019E95C
	public void Destroy()
	{
		PlatformUserManager.DetailsUpdated -= this.HandlePlayerDetailsUpdate;
	}

	// Token: 0x040035CC RID: 13772
	public const int cCurrentXmlVersion = 1;

	// Token: 0x040035CD RID: 13773
	public ObservableDictionary<PlatformUserIdentifierAbs, PersistentPlayerData> Players;

	// Token: 0x040035CE RID: 13774
	public Dictionary<int, PersistentPlayerData> EntityToPlayerMap;

	// Token: 0x040035CF RID: 13775
	public Dictionary<PlatformUserIdentifierAbs, int> PlayerToEntityMap;

	// Token: 0x040035D0 RID: 13776
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PersistentPlayerData.PlayerEventHandler> m_dispatch;

	// Token: 0x040035D1 RID: 13777
	public Dictionary<Vector3i, PersistentPlayerData> m_lpBlockMap;

	// Token: 0x040035D2 RID: 13778
	public readonly AllyStore Allies;
}
