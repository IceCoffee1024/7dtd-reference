using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008F8 RID: 2296
[Preserve]
public class PersistentPlayerData
{
	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x060041F0 RID: 16880 RVA: 0x0019DE80 File Offset: 0x0019C080
	public PlatformUserIdentifierAbs PrimaryId
	{
		get
		{
			return this.PlayerData.PrimaryId;
		}
	}

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x060041F1 RID: 16881 RVA: 0x0019DE8D File Offset: 0x0019C08D
	public IPlatformUserData PlatformData
	{
		get
		{
			return this.PlayerData.PlatformData;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x060041F2 RID: 16882 RVA: 0x0019DE9A File Offset: 0x0019C09A
	public PlatformUserIdentifierAbs NativeId
	{
		get
		{
			return this.PlayerData.NativeId;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x060041F3 RID: 16883 RVA: 0x0019DEA7 File Offset: 0x0019C0A7
	public EPlayGroup PlayGroup
	{
		get
		{
			return this.PlayerData.PlayGroup;
		}
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x0019DEB4 File Offset: 0x0019C0B4
	public PersistentPlayerData(PlatformUserIdentifierAbs _primaryId, PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PlayerData = new PlayerData(_primaryId, _nativeId, _playerName, _playGroup);
		this.PlayerName = new PersistentPlayerName(_playerName);
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x0019DF2F File Offset: 0x0019C12F
	public void Update(PlatformUserIdentifierAbs _nativeId, AuthoredText _playerName, EPlayGroup _playGroup)
	{
		this.PlayerData = new PlayerData(this.PrimaryId, _nativeId, _playerName, _playGroup);
		this.PlayerName.Update(_playerName);
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x060041F6 RID: 16886 RVA: 0x0019DF51 File Offset: 0x0019C151
	public Vector3i MostRecentBackpackPosition
	{
		get
		{
			if (this.backpacksByID.Count == 0)
			{
				return Vector3i.zero;
			}
			this.RefreshSortedBackpacksList();
			return this.backpacksSortedByTimestamp[this.backpacksSortedByTimestamp.Count - 1].Position;
		}
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0019DF8C File Offset: 0x0019C18C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshSortedBackpacksList()
	{
		if (!this.sortedBackpacksDirty)
		{
			return;
		}
		this.backpacksSortedByTimestamp.Clear();
		foreach (KeyValuePair<int, PersistentPlayerData.ProtectedBackpack> keyValuePair in this.backpacksByID)
		{
			this.backpacksSortedByTimestamp.Add(keyValuePair.Value);
		}
		this.backpacksSortedByTimestamp.Sort((PersistentPlayerData.ProtectedBackpack a, PersistentPlayerData.ProtectedBackpack b) => a.Timestamp.CompareTo(b.Timestamp));
		this.sortedBackpacksDirty = false;
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x0019E030 File Offset: 0x0019C230
	public void AddDroppedBackpack(int backpackEntityId, Vector3i pos, uint timestamp)
	{
		this.backpacksByID[backpackEntityId] = new PersistentPlayerData.ProtectedBackpack(backpackEntityId, pos, timestamp);
		this.sortedBackpacksDirty = true;
		this.RefreshSortedBackpacksList();
		if (this.backpacksByID.Count > 3)
		{
			for (int i = 0; i < this.backpacksSortedByTimestamp.Count - 3; i++)
			{
				int entityID = this.backpacksSortedByTimestamp[i].EntityID;
				if (entityID == backpackEntityId)
				{
					Debug.LogError("AddDroppedBackpack failed: dropped backpack timestamp is older than other tracked backpacks and the tracking limit has been reached.");
				}
				this.TryRemoveDroppedBackpack(entityID);
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerSetBackpackPosition>().Setup(this.EntityId, this.GetDroppedBackpackPositions()), false, this.EntityId, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x0019E0F0 File Offset: 0x0019C2F0
	public bool TryUpdateBackpackPosition(int entityID, Vector3i pos)
	{
		PersistentPlayerData.ProtectedBackpack protectedBackpack;
		if (!this.backpacksByID.TryGetValue(entityID, out protectedBackpack))
		{
			return false;
		}
		this.backpacksByID[entityID] = new PersistentPlayerData.ProtectedBackpack(entityID, pos, protectedBackpack.Timestamp);
		return true;
	}

	// Token: 0x060041FA RID: 16890 RVA: 0x0019E129 File Offset: 0x0019C329
	public bool TryRemoveDroppedBackpack(int entityID)
	{
		if (this.backpacksByID.Remove(entityID))
		{
			this.sortedBackpacksDirty = true;
			this.RefreshSortedBackpacksList();
			return true;
		}
		return false;
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x0019E14C File Offset: 0x0019C34C
	public void ProcessBackpacks(Action<PersistentPlayerData.ProtectedBackpack> action)
	{
		foreach (PersistentPlayerData.ProtectedBackpack protectedBackpack in this.backpacksByID.Values)
		{
			action(protectedBackpack);
		}
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x0019E1A4 File Offset: 0x0019C3A4
	public void RemoveBackpacks(Predicate<PersistentPlayerData.ProtectedBackpack> shouldRemove)
	{
		this.RefreshSortedBackpacksList();
		for (int i = 0; i < this.backpacksSortedByTimestamp.Count; i++)
		{
			if (shouldRemove(this.backpacksSortedByTimestamp[i]))
			{
				this.TryRemoveDroppedBackpack(this.backpacksSortedByTimestamp[i].EntityID);
			}
		}
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x0019E1F9 File Offset: 0x0019C3F9
	public void ClearDroppedBackpacks()
	{
		this.backpacksByID.Clear();
		this.backpacksSortedByTimestamp.Clear();
		this.sortedBackpacksDirty = true;
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x0019E218 File Offset: 0x0019C418
	public List<Vector3i> GetDroppedBackpackPositions()
	{
		List<Vector3i> list = new List<Vector3i>();
		foreach (PersistentPlayerData.ProtectedBackpack protectedBackpack in this.backpacksSortedByTimestamp)
		{
			list.Add(protectedBackpack.Position);
		}
		return list;
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x0019E278 File Offset: 0x0019C478
	public void AddVendingMachinePosition(Vector3i pos)
	{
		if (!this.OwnedVendingMachinePositions.Contains(pos))
		{
			this.OwnedVendingMachinePositions.Add(pos);
		}
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x0019E294 File Offset: 0x0019C494
	public bool TryRemoveVendingMachinePosition(Vector3i pos)
	{
		return this.OwnedVendingMachinePositions.Remove(pos);
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06004201 RID: 16897 RVA: 0x0019E2A2 File Offset: 0x0019C4A2
	public bool HasBedrollPos
	{
		get
		{
			return this.BedrollPos.y != int.MaxValue;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06004202 RID: 16898 RVA: 0x0019E2BC File Offset: 0x0019C4BC
	public double OfflineHours
	{
		get
		{
			if (this.EntityId != -1)
			{
				return -1.0;
			}
			return (DateTime.Now - this.LastLogin).TotalHours;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06004203 RID: 16899 RVA: 0x0019E2F4 File Offset: 0x0019C4F4
	public double OfflineMinutes
	{
		get
		{
			if (this.EntityId != -1)
			{
				return -1.0;
			}
			return (DateTime.Now - this.LastLogin).TotalMinutes;
		}
	}

	// Token: 0x06004204 RID: 16900 RVA: 0x0019E32C File Offset: 0x0019C52C
	public bool IsAlly(PlatformUserIdentifierAbs other)
	{
		return GameManager.Instance.persistentPlayers.Allies.IsAlly(this.PrimaryId, other);
	}

	// Token: 0x06004205 RID: 16901 RVA: 0x0019E349 File Offset: 0x0019C549
	public bool IsAlly(PersistentPlayerData other)
	{
		return GameManager.Instance.persistentPlayers.Allies.IsAlly(this.PrimaryId, (other != null) ? other.PrimaryId : null);
	}

	// Token: 0x06004206 RID: 16902 RVA: 0x0019E371 File Offset: 0x0019C571
	public void AddLandProtectionBlock(Vector3i pos)
	{
		if (this.LPBlocks == null)
		{
			this.LPBlocks = new List<Vector3i>();
		}
		this.LPBlocks.Add(pos);
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x0019E392 File Offset: 0x0019C592
	public List<Vector3i> GetLandProtectionBlocks()
	{
		if (this.LPBlocks == null)
		{
			this.LPBlocks = new List<Vector3i>();
		}
		return this.LPBlocks;
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x0019E3AD File Offset: 0x0019C5AD
	public bool GetLandProtectionBlock(out Vector3i _blockPos)
	{
		_blockPos = Vector3i.zero;
		if (this.LPBlocks != null && this.LPBlocks.Count > 0)
		{
			_blockPos = this.LPBlocks[0];
			return true;
		}
		return false;
	}

	// Token: 0x06004209 RID: 16905 RVA: 0x0019E3E5 File Offset: 0x0019C5E5
	public void RemoveLandProtectionBlock(Vector3i pos)
	{
		this.LPBlocks.Remove(pos);
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x0019E3F4 File Offset: 0x0019C5F4
	public void ClearBedroll()
	{
		Entity entity = GameManager.Instance.World.GetEntity(this.EntityId);
		if (entity)
		{
			NavObjectManager.Instance.UnRegisterNavObjectByOwnerEntity(entity, "sleeping_bag");
			this.BedrollPos.y = int.MaxValue;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.SleepingBag, this.EntityId), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x0019E47C File Offset: 0x0019C67C
	public void ShowBedrollOnMap()
	{
		if (GameManager.Instance.IsEditMode())
		{
			return;
		}
		if (this.BedrollPos.y != 2147483647)
		{
			Entity entity = GameManager.Instance.World.GetEntity(this.EntityId);
			if (entity)
			{
				NavObject navObject = NavObjectManager.Instance.RegisterNavObject("sleeping_bag", this.BedrollPos.ToVector3(), "", false, -1, null);
				if (navObject != null)
				{
					navObject.OwnerEntity = entity;
				}
			}
		}
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x0019E4F4 File Offset: 0x0019C6F4
	public void AddQuestPosition(int questCode, Quest.PositionDataTypes positionDataType, Vector3 position)
	{
		Vector3i vector3i = World.worldToBlockPos(position);
		foreach (QuestPositionData questPositionData in this.QuestPositions)
		{
			if (questPositionData.questCode == questCode && questPositionData.positionDataType == positionDataType)
			{
				questPositionData.blockPosition = vector3i;
				return;
			}
		}
		if (positionDataType == Quest.PositionDataTypes.TreasureOffset || positionDataType == Quest.PositionDataTypes.POISize || positionDataType == Quest.PositionDataTypes.TraderPosition)
		{
			return;
		}
		this.QuestPositions.Add(new QuestPositionData(questCode, positionDataType, vector3i));
		this.questPositionsChanged = true;
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x0019E58C File Offset: 0x0019C78C
	public void RemovePositionsForQuest(int questCode)
	{
		List<QuestPositionData> list = new List<QuestPositionData>();
		foreach (QuestPositionData questPositionData in this.QuestPositions)
		{
			if (questPositionData.questCode == questCode)
			{
				list.Add(questPositionData);
			}
		}
		foreach (QuestPositionData questPositionData2 in list)
		{
			this.QuestPositions.Remove(questPositionData2);
		}
		this.questPositionsChanged = true;
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x0019E638 File Offset: 0x0019C838
	public void UpdatePositionFromEntity()
	{
		if (this.EntityId == -1)
		{
			return;
		}
		Entity entity = GameManager.Instance.World.GetEntity(this.EntityId);
		if (!entity)
		{
			return;
		}
		this.Position = new Vector3i(entity.position);
	}

	// Token: 0x0600420F RID: 16911 RVA: 0x0019E680 File Offset: 0x0019C880
	public void Write(PooledBinaryWriter stream)
	{
		this.UpdatePositionFromEntity();
		this.PrimaryId.ToStream(stream, true);
		this.NativeId.ToStream(stream, true);
		stream.Write((byte)this.PlayGroup);
		AuthoredText.ToStream(this.PlayerName.AuthoredName, stream);
		stream.Write(this.LastLogin.Ticks);
		stream.Write(this.Position.x);
		stream.Write(this.Position.y);
		stream.Write(this.Position.z);
		stream.Write(this.EntityId);
		List<Vector3i> lpblocks = this.LPBlocks;
		stream.Write((lpblocks != null) ? lpblocks.Count : 0);
		stream.Write(this.backpacksByID.Count);
		if (this.LPBlocks != null)
		{
			for (int i = 0; i < this.LPBlocks.Count; i++)
			{
				Vector3i vector3i = this.LPBlocks[i];
				stream.Write(vector3i.x);
				stream.Write(vector3i.y);
				stream.Write(vector3i.z);
			}
		}
		foreach (KeyValuePair<int, PersistentPlayerData.ProtectedBackpack> keyValuePair in this.backpacksByID)
		{
			stream.Write(keyValuePair.Key);
			Vector3i position = keyValuePair.Value.Position;
			stream.Write(position.x);
			stream.Write(position.y);
			stream.Write(position.z);
			stream.Write(keyValuePair.Value.Timestamp);
		}
		stream.Write(this.BedrollPos.x);
		stream.Write(this.BedrollPos.y);
		stream.Write(this.BedrollPos.z);
		stream.Write(this.QuestPositions.Count);
		foreach (QuestPositionData questPositionData in this.QuestPositions)
		{
			questPositionData.Write(stream);
		}
		stream.Write(this.OwnedVendingMachinePositions.Count);
		foreach (Vector3i vector3i2 in this.OwnedVendingMachinePositions)
		{
			stream.Write(vector3i2.x);
			stream.Write(vector3i2.y);
			stream.Write(vector3i2.z);
		}
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x0019E920 File Offset: 0x0019CB20
	public static PersistentPlayerData Read(PooledBinaryReader stream)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(stream, false, true);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(stream, false, true);
		EPlayGroup eplayGroup = (EPlayGroup)stream.ReadByte();
		PersistentPlayerData persistentPlayerData = new PersistentPlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, AuthoredText.FromStream(stream), eplayGroup);
		persistentPlayerData.LastLogin = new DateTime(stream.ReadInt64());
		persistentPlayerData.Position.x = stream.ReadInt32();
		persistentPlayerData.Position.y = stream.ReadInt32();
		persistentPlayerData.Position.z = stream.ReadInt32();
		persistentPlayerData.EntityId = stream.ReadInt32();
		int num = stream.ReadInt32();
		int num2 = stream.ReadInt32();
		if (num > 0)
		{
			persistentPlayerData.LPBlocks = new List<Vector3i>();
			for (int i = 0; i < num; i++)
			{
				persistentPlayerData.LPBlocks.Add(new Vector3i(stream.ReadInt32(), stream.ReadInt32(), stream.ReadInt32()));
			}
		}
		for (int j = 0; j < num2; j++)
		{
			int num3 = stream.ReadInt32();
			Vector3i vector3i = new Vector3i(stream.ReadInt32(), stream.ReadInt32(), stream.ReadInt32());
			uint num4 = stream.ReadUInt32();
			persistentPlayerData.AddDroppedBackpack(num3, vector3i, num4);
		}
		persistentPlayerData.BedrollPos.x = stream.ReadInt32();
		persistentPlayerData.BedrollPos.y = stream.ReadInt32();
		persistentPlayerData.BedrollPos.z = stream.ReadInt32();
		int num5 = stream.ReadInt32();
		persistentPlayerData.QuestPositions = new List<QuestPositionData>();
		for (int k = 0; k < num5; k++)
		{
			persistentPlayerData.QuestPositions.Add(QuestPositionData.Read(stream));
		}
		int num6 = stream.ReadInt32();
		persistentPlayerData.OwnedVendingMachinePositions = new List<Vector3i>();
		for (int l = 0; l < num6; l++)
		{
			persistentPlayerData.OwnedVendingMachinePositions.Add(new Vector3i(stream.ReadInt32(), stream.ReadInt32(), stream.ReadInt32()));
		}
		return persistentPlayerData;
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x0019EAE8 File Offset: 0x0019CCE8
	public void Write(XmlElement _root)
	{
		XmlElement xmlElement = _root.AddXmlElement("player");
		this.PrimaryId.ToXml(xmlElement, "");
		PlatformUserIdentifierAbs nativeId = this.NativeId;
		if (nativeId != null)
		{
			nativeId.ToXml(xmlElement, "native");
		}
		xmlElement.SetAttrib("playername", this.PlayerName.AuthoredName.Text);
		xmlElement.SetAttrib("playgroup", this.PlayGroup.ToStringCached<EPlayGroup>());
		xmlElement.SetAttrib("lastlogin", this.LastLogin.ToCultureInvariantString());
		xmlElement.SetAttrib("position", string.Format("{0},{1},{2}", this.Position.x, this.Position.y, this.Position.z));
		if ((this.LPBlocks == null || this.LPBlocks.Count == 0) && this.backpacksByID.Count == 0 && this.BedrollPos.y == 2147483647 && this.QuestPositions.Count == 0 && this.OwnedVendingMachinePositions.Count == 0)
		{
			return;
		}
		if (this.LPBlocks != null)
		{
			for (int i = 0; i < this.LPBlocks.Count; i++)
			{
				Vector3i vector3i = this.LPBlocks[i];
				xmlElement.AddXmlElement("lpblock").SetAttrib("pos", string.Format("{0},{1},{2}", vector3i.x, vector3i.y, vector3i.z));
			}
		}
		foreach (KeyValuePair<int, PersistentPlayerData.ProtectedBackpack> keyValuePair in this.backpacksByID)
		{
			int key = keyValuePair.Key;
			Vector3i position = keyValuePair.Value.Position;
			uint timestamp = keyValuePair.Value.Timestamp;
			XmlElement xmlElement2 = xmlElement.AddXmlElement("backpack");
			xmlElement2.SetAttrib("id", string.Format("{0}", key));
			xmlElement2.SetAttrib("pos", string.Format("{0},{1},{2}", position.x, position.y, position.z));
			xmlElement2.SetAttrib("timestamp", string.Format("{0}", timestamp));
		}
		if (this.BedrollPos.y != 2147483647)
		{
			xmlElement.AddXmlElement("bedroll").SetAttrib("pos", string.Format("{0},{1},{2}", this.BedrollPos.x, this.BedrollPos.y, this.BedrollPos.z));
		}
		if (this.QuestPositions != null && this.QuestPositions.Count > 0)
		{
			XmlElement xmlElement3 = xmlElement.AddXmlElement("questpositions");
			foreach (QuestPositionData questPositionData in this.QuestPositions)
			{
				XmlElement xmlElement4 = xmlElement3.AddXmlElement("position");
				xmlElement4.SetAttribute("id", questPositionData.questCode.ToString());
				string text = "positiondatatype";
				int positionDataType = (int)questPositionData.positionDataType;
				xmlElement4.SetAttribute(text, positionDataType.ToString());
				xmlElement4.SetAttrib("pos", string.Format("{0},{1},{2}", questPositionData.blockPosition.x, questPositionData.blockPosition.y, questPositionData.blockPosition.z));
			}
		}
		if (this.OwnedVendingMachinePositions != null && this.OwnedVendingMachinePositions.Count > 0)
		{
			XmlElement xmlElement5 = xmlElement.AddXmlElement("vendingmachinepositions");
			foreach (Vector3i vector3i2 in this.OwnedVendingMachinePositions)
			{
				xmlElement5.AddXmlElement("position").SetAttrib("pos", string.Format("{0},{1},{2}", vector3i2.x, vector3i2.y, vector3i2.z));
			}
		}
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x0019EF5C File Offset: 0x0019D15C
	public static PersistentPlayerData ReadXML(XmlElement root, int readVersion, out HashSet<PlatformUserIdentifierAbs> legacyACL)
	{
		legacyACL = null;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromXml(root, true, null);
		if (platformUserIdentifierAbs == null)
		{
			Log.Error("player-entry has missing or invalid user-identifier attributes: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		if (!root.HasAttribute("playername"))
		{
			Log.Error("player-entry is missing 'playername' attribute: " + root.OuterXml);
			return null;
		}
		AuthoredText authoredText = new AuthoredText(root.GetAttribute("playername"), platformUserIdentifierAbs);
		string text;
		EPlayGroup eplayGroup;
		if (!root.TryGetAttribute("playgroup", out text))
		{
			eplayGroup = EPlayGroup.Unknown;
		}
		else if (!Enum.TryParse<EPlayGroup>(text, out eplayGroup))
		{
			Log.Error("player-entry has missing or malformed 'playgroup' attribute: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromXml(root, true, "native");
		PersistentPlayerData persistentPlayerData = new PersistentPlayerData(platformUserIdentifierAbs, platformUserIdentifierAbs2, authoredText, eplayGroup);
		if (!root.HasAttribute("lastlogin"))
		{
			Log.Error("player-entry is missing 'lastlogin' attribute: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		if (!StringParsers.TryParseDateTime(root.GetAttribute("lastlogin"), out persistentPlayerData.LastLogin) && !DateTime.TryParse(root.GetAttribute("lastlogin"), out persistentPlayerData.LastLogin))
		{
			Log.Error("player-entry has malfored 'lastlogin' attribute: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		if (!root.HasAttribute("position"))
		{
			Log.Error("player-entry is missing 'position' attribute: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		string[] array = root.GetAttribute("position").Split(',', StringSplitOptions.None);
		if (array.Length < 3)
		{
			Log.Error("player-entry has invalid 'position' attribute: " + root.OuterXml);
			Application.Quit();
			return null;
		}
		persistentPlayerData.Position.x = int.Parse(array[0].Trim());
		persistentPlayerData.Position.y = int.Parse(array[1].Trim());
		persistentPlayerData.Position.z = int.Parse(array[2].Trim());
		foreach (object obj in root.ChildNodes)
		{
			XmlNode xmlNode = (XmlNode)obj;
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (readVersion == 0 && xmlNode.Name == "acl")
				{
					if (legacyACL == null)
					{
						legacyACL = new HashSet<PlatformUserIdentifierAbs>();
					}
					PlatformUserIdentifierAbs platformUserIdentifierAbs3 = PlatformUserIdentifierAbs.FromXml(xmlElement, true, null);
					if (platformUserIdentifierAbs3 == null)
					{
						Log.Warning("Ignoring malformed acl-entry: " + xmlNode.OuterXml);
					}
					else
					{
						legacyACL.Add(platformUserIdentifierAbs3);
					}
				}
				else if (xmlNode.Name == "lpblock")
				{
					if (!xmlElement.HasAttribute("pos"))
					{
						Log.Warning("Ignoring lpblock-entry because of missing 'pos' attribute: " + xmlNode.OuterXml);
					}
					else
					{
						string[] array2 = xmlElement.GetAttribute("pos").Split(',', StringSplitOptions.None);
						if (array2.Length < 3)
						{
							Log.Warning("Ignoring lpblock-entry because of malformed 'pos' attribute: " + xmlNode.OuterXml);
						}
						else
						{
							Vector3i vector3i = default(Vector3i);
							vector3i.x = int.Parse(array2[0].Trim());
							vector3i.y = int.Parse(array2[1].Trim());
							vector3i.z = int.Parse(array2[2].Trim());
							if (persistentPlayerData.LPBlocks == null)
							{
								persistentPlayerData.LPBlocks = new List<Vector3i>();
							}
							persistentPlayerData.LPBlocks.Add(vector3i);
						}
					}
				}
				else if (xmlNode.Name == "backpack")
				{
					if (!xmlElement.HasAttribute("pos"))
					{
						Log.Warning("Ignoring backpack-entry because of missing 'pos' attribute: " + xmlNode.OuterXml);
					}
					else if (!xmlElement.HasAttribute("id"))
					{
						Log.Warning("Ignoring backpack-entry because of missing 'id' attribute: " + xmlNode.OuterXml);
					}
					else if (!xmlElement.HasAttribute("timestamp"))
					{
						Log.Warning("Ignoring backpack-entry because of missing 'timestamp' attribute: " + xmlNode.OuterXml);
					}
					else
					{
						string[] array3 = xmlElement.GetAttribute("pos").Split(',', StringSplitOptions.None);
						if (array3.Length < 3)
						{
							Log.Warning("Ignoring backpack-entry because of malformed 'pos' attribute: " + xmlNode.OuterXml);
						}
						else
						{
							int num = int.Parse(xmlElement.GetAttribute("id"));
							Vector3i vector3i2 = default(Vector3i);
							vector3i2.x = int.Parse(array3[0].Trim());
							vector3i2.y = int.Parse(array3[1].Trim());
							vector3i2.z = int.Parse(array3[2].Trim());
							uint num2 = uint.Parse(xmlElement.GetAttribute("timestamp"));
							persistentPlayerData.AddDroppedBackpack(num, vector3i2, num2);
						}
					}
				}
				else if (xmlNode.Name == "bedroll")
				{
					string[] array4 = xmlElement.GetAttribute("pos").Split(',', StringSplitOptions.None);
					if (array4.Length < 3)
					{
						Log.Warning("Ignoring bedroll-entry. Invalid 'pos' attribute: " + xmlNode.OuterXml);
					}
					else
					{
						persistentPlayerData.BedrollPos.x = int.Parse(array4[0].Trim());
						persistentPlayerData.BedrollPos.y = int.Parse(array4[1].Trim());
						persistentPlayerData.BedrollPos.z = int.Parse(array4[2].Trim());
					}
				}
				else
				{
					if (xmlNode.Name == "questpositions")
					{
						persistentPlayerData.QuestPositions = new List<QuestPositionData>();
						using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								XmlElement xmlElement2 = (XmlElement)obj2;
								if (xmlElement2.Name == "position")
								{
									int num3 = int.Parse(xmlElement2.GetAttribute("id"));
									Vector3i vector3i3 = default(Vector3i);
									int num4 = int.Parse(xmlElement2.GetAttribute("positiondatatype"));
									string[] array5 = xmlElement2.GetAttribute("pos").Split(',', StringSplitOptions.None);
									if (array5.Length < 3)
									{
										Log.Warning("Ignoring bedroll-entry. Invalid 'pos' attribute: " + xmlNode.OuterXml);
									}
									else
									{
										vector3i3.x = int.Parse(array5[0].Trim());
										vector3i3.y = int.Parse(array5[1].Trim());
										vector3i3.z = int.Parse(array5[2].Trim());
										persistentPlayerData.QuestPositions.Add(new QuestPositionData(num3, (Quest.PositionDataTypes)num4, vector3i3));
									}
								}
							}
							continue;
						}
					}
					if (xmlNode.Name == "vendingmachinepositions")
					{
						persistentPlayerData.OwnedVendingMachinePositions = new List<Vector3i>();
						foreach (object obj3 in xmlNode.ChildNodes)
						{
							XmlElement xmlElement3 = (XmlElement)obj3;
							if (xmlElement3.Name == "position")
							{
								Vector3i vector3i4 = default(Vector3i);
								string[] array6 = xmlElement3.GetAttribute("pos").Split(',', StringSplitOptions.None);
								vector3i4.x = int.Parse(array6[0].Trim());
								vector3i4.y = int.Parse(array6[1].Trim());
								vector3i4.z = int.Parse(array6[2].Trim());
								persistentPlayerData.OwnedVendingMachinePositions.Add(vector3i4);
							}
						}
					}
				}
			}
		}
		return persistentPlayerData;
	}

	// Token: 0x040035B8 RID: 13752
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxTrackedBackpacks = 3;

	// Token: 0x040035B9 RID: 13753
	public PlayerData PlayerData;

	// Token: 0x040035BA RID: 13754
	public readonly PersistentPlayerName PlayerName;

	// Token: 0x040035BB RID: 13755
	public DateTime LastLogin;

	// Token: 0x040035BC RID: 13756
	public int EntityId = -1;

	// Token: 0x040035BD RID: 13757
	public List<Vector3i> LPBlocks;

	// Token: 0x040035BE RID: 13758
	public const int cBedrollUnsetY = 2147483647;

	// Token: 0x040035BF RID: 13759
	public Vector3i BedrollPos = new Vector3i(0, int.MaxValue, 0);

	// Token: 0x040035C0 RID: 13760
	public Vector3i Position;

	// Token: 0x040035C1 RID: 13761
	public List<QuestPositionData> QuestPositions = new List<QuestPositionData>();

	// Token: 0x040035C2 RID: 13762
	public bool questPositionsChanged;

	// Token: 0x040035C3 RID: 13763
	public List<Vector3i> OwnedVendingMachinePositions = new List<Vector3i>();

	// Token: 0x040035C4 RID: 13764
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, PersistentPlayerData.ProtectedBackpack> backpacksByID = new Dictionary<int, PersistentPlayerData.ProtectedBackpack>();

	// Token: 0x040035C5 RID: 13765
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortedBackpacksDirty = true;

	// Token: 0x040035C6 RID: 13766
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PersistentPlayerData.ProtectedBackpack> backpacksSortedByTimestamp = new List<PersistentPlayerData.ProtectedBackpack>();

	// Token: 0x020008F9 RID: 2297
	// (Invoke) Token: 0x06004214 RID: 16916
	public delegate void PlayerEventHandler(PersistentPlayerData ppData, PersistentPlayerData otherPlayer, EnumPersistentPlayerDataReason reason);

	// Token: 0x020008FA RID: 2298
	public struct ProtectedBackpack
	{
		// Token: 0x06004217 RID: 16919 RVA: 0x0019F738 File Offset: 0x0019D938
		public ProtectedBackpack(int entityID, Vector3i position, uint timestamp)
		{
			this.EntityID = entityID;
			this.Position = position;
			this.Timestamp = timestamp;
		}

		// Token: 0x040035C7 RID: 13767
		public readonly int EntityID;

		// Token: 0x040035C8 RID: 13768
		public readonly Vector3i Position;

		// Token: 0x040035C9 RID: 13769
		public readonly uint Timestamp;
	}
}
