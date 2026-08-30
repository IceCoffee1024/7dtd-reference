using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008EB RID: 2283
[Preserve]
public class PersistentPlayerList
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06004224 RID: 16932 RVA: 0x0019D5A4 File Offset: 0x0019B7A4
	// (remove) Token: 0x06004225 RID: 16933 RVA: 0x0019D5D8 File Offset: 0x0019B7D8
	public static event AllyStore.AllyChangeEvent OnPersistentAllyChangeEvent;

	// Token: 0x06004226 RID: 16934 RVA: 0x0019D60C File Offset: 0x0019B80C
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

	// Token: 0x06004227 RID: 16935 RVA: 0x0019D680 File Offset: 0x0019B880
	public void HandlePlayerDetailsUpdate(IPlatformUserData platformUserData, string name)
	{
		PersistentPlayerData persistentPlayerData;
		if (this.Players.TryGetValue(platformUserData.PrimaryId, out persistentPlayerData))
		{
			persistentPlayerData.PlayerName.Update(name, platformUserData.PrimaryId);
		}
	}

	// Token: 0x06004228 RID: 16936 RVA: 0x0019D6B4 File Offset: 0x0019B8B4
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

	// Token: 0x06004229 RID: 16937 RVA: 0x0019D744 File Offset: 0x0019B944
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

	// Token: 0x0600422A RID: 16938 RVA: 0x0019D7D4 File Offset: 0x0019B9D4
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

	// Token: 0x0600422B RID: 16939 RVA: 0x0019D86C File Offset: 0x0019BA6C
	public PersistentPlayerData GetLandProtectionBlockOwner(Vector3i pos)
	{
		PersistentPlayerData persistentPlayerData;
		this.m_lpBlockMap.TryGetValue(pos, out persistentPlayerData);
		return persistentPlayerData;
	}

	// Token: 0x0600422C RID: 16940 RVA: 0x0019D889 File Offset: 0x0019BA89
	public void AddPlayerEventHandler(PersistentPlayerData.PlayerEventHandler handler)
	{
		if (this.m_dispatch == null)
		{
			this.m_dispatch = new List<PersistentPlayerData.PlayerEventHandler>();
		}
		this.m_dispatch.Add(handler);
	}

	// Token: 0x0600422D RID: 16941 RVA: 0x0019D8AA File Offset: 0x0019BAAA
	public void RemovePlayerEventHandler(PersistentPlayerData.PlayerEventHandler handler)
	{
		this.m_dispatch.Remove(handler);
	}

	// Token: 0x0600422E RID: 16942 RVA: 0x0019D8BC File Offset: 0x0019BABC
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

	// Token: 0x0600422F RID: 16943 RVA: 0x0019D8FC File Offset: 0x0019BAFC
	public PersistentPlayerData GetPlayerDataFromEntityID(int _entityId)
	{
		PersistentPlayerData persistentPlayerData;
		if (!this.EntityToPlayerMap.TryGetValue(_entityId, out persistentPlayerData))
		{
			return null;
		}
		return persistentPlayerData;
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x0019D91C File Offset: 0x0019BB1C
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

	// Token: 0x06004231 RID: 16945 RVA: 0x0019D958 File Offset: 0x0019BB58
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

	// Token: 0x06004232 RID: 16946 RVA: 0x0019D980 File Offset: 0x0019BB80
	public PersistentPlayerData CreatePlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, string _playerName, EPlayGroup _playGroup)
	{
		PersistentPlayerData persistentPlayerData = new PersistentPlayerData(_primaryId, _nativeId, new AuthoredText(_playerName, _primaryId), _playGroup);
		persistentPlayerData.EntityId = -1;
		persistentPlayerData.LastLogin = DateTime.Now;
		this.Players[_primaryId] = persistentPlayerData;
		return persistentPlayerData;
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x0019D9C0 File Offset: 0x0019BBC0
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

	// Token: 0x06004234 RID: 16948 RVA: 0x0019DA07 File Offset: 0x0019BC07
	public void MapPlayer(PersistentPlayerData ppd)
	{
		if (ppd.EntityId != -1)
		{
			this.EntityToPlayerMap[ppd.EntityId] = ppd;
			this.PlayerToEntityMap[ppd.PrimaryId] = ppd.EntityId;
		}
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x0019DA3C File Offset: 0x0019BC3C
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

	// Token: 0x06004236 RID: 16950 RVA: 0x0019DB14 File Offset: 0x0019BD14
	[PublicizedFrom(EAccessModifier.Private)]
	public void NameCollisionEvent(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _entry)
	{
		GameManager.Instance.persistentPlayers.FixNameCollisions(_entry.Value.PlayerName.AuthoredName.Text);
	}

	// Token: 0x06004237 RID: 16951 RVA: 0x0019DB3C File Offset: 0x0019BD3C
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

	// Token: 0x06004238 RID: 16952 RVA: 0x0019DDB8 File Offset: 0x0019BFB8
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

	// Token: 0x06004239 RID: 16953 RVA: 0x0019DE2C File Offset: 0x0019C02C
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

	// Token: 0x0600423A RID: 16954 RVA: 0x0019DF34 File Offset: 0x0019C134
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

	// Token: 0x0600423B RID: 16955 RVA: 0x0019E07C File Offset: 0x0019C27C
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

	// Token: 0x0600423C RID: 16956 RVA: 0x0019E0E4 File Offset: 0x0019C2E4
	public void SavePersistentPlayerData()
	{
		if (!GameManager.Instance.IsEditMode() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.Write(GameIO.GetSaveGameDir() + "/players.xml");
		}
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x0019E114 File Offset: 0x0019C314
	public void Write(BinaryWriter stream)
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

	// Token: 0x0600423E RID: 16958 RVA: 0x0019E21C File Offset: 0x0019C41C
	public static PersistentPlayerList Read(BinaryReader stream)
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

	// Token: 0x0600423F RID: 16959 RVA: 0x0019E2D4 File Offset: 0x0019C4D4
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

	// Token: 0x06004240 RID: 16960 RVA: 0x0019E4EC File Offset: 0x0019C6EC
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

	// Token: 0x06004241 RID: 16961 RVA: 0x0019E580 File Offset: 0x0019C780
	public void Destroy()
	{
		PlatformUserManager.DetailsUpdated -= this.HandlePlayerDetailsUpdate;
	}

	// Token: 0x04003557 RID: 13655
	public const int cCurrentXmlVersion = 1;

	// Token: 0x04003558 RID: 13656
	public ObservableDictionary<PlatformUserIdentifierAbs, PersistentPlayerData> Players;

	// Token: 0x04003559 RID: 13657
	public Dictionary<int, PersistentPlayerData> EntityToPlayerMap;

	// Token: 0x0400355A RID: 13658
	public Dictionary<PlatformUserIdentifierAbs, int> PlayerToEntityMap;

	// Token: 0x0400355B RID: 13659
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PersistentPlayerData.PlayerEventHandler> m_dispatch;

	// Token: 0x0400355C RID: 13660
	public Dictionary<Vector3i, PersistentPlayerData> m_lpBlockMap;

	// Token: 0x0400355D RID: 13661
	public readonly AllyStore Allies;
}
