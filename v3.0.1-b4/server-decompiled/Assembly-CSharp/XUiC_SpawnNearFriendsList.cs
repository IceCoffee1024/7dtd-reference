using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200105D RID: 4189
[Preserve]
public class XUiC_SpawnNearFriendsList : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>
{
	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x060084F1 RID: 34033 RVA: 0x0032C776 File Offset: 0x0032A976
	public static AllowSpawnNearFriend SpawnNearFriendMode
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode == ProtocolManager.NetworkType.None)
			{
				return (AllowSpawnNearFriend)GamePrefs.GetInt(EnumGamePrefs.AllowSpawnNearFriend);
			}
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			if (currentGameServerInfoServerOrClient == null)
			{
				return AllowSpawnNearFriend.Always;
			}
			return (AllowSpawnNearFriend)currentGameServerInfoServerOrClient.GetValue(GameInfoInt.AllowSpawnNearFriend);
		}
	}

	// Token: 0x140000DB RID: 219
	// (add) Token: 0x060084F2 RID: 34034 RVA: 0x0032C7A8 File Offset: 0x0032A9A8
	// (remove) Token: 0x060084F3 RID: 34035 RVA: 0x0032C7E0 File Offset: 0x0032A9E0
	public event Action<PersistentPlayerData> SpawnClicked;

	// Token: 0x060084F4 RID: 34036 RVA: 0x0032C818 File Offset: 0x0032AA18
	public override void Init()
	{
		base.Init();
		XUiC_SpawnNearFriendsList.Instance = this;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnSpawnNearFriend") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += delegate(XUiController _, int _)
			{
				XUiC_SpawnNearFriendsList.ListEntry selectedEntryData = base.SelectedEntryData;
				if (selectedEntryData == null)
				{
					return;
				}
				Action<PersistentPlayerData> spawnClicked = this.SpawnClicked;
				if (spawnClicked == null)
				{
					return;
				}
				spawnClicked(selectedEntryData.PersistentPlayerData);
			};
		}
	}

	// Token: 0x060084F5 RID: 34037 RVA: 0x0032C858 File Offset: 0x0032AA58
	public override void OnOpen()
	{
		base.OnOpen();
		this.RebuildList(false);
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		ObservableDictionary<PlatformUserIdentifierAbs, PersistentPlayerData> observableDictionary = ((persistentPlayers != null) ? persistentPlayers.Players : null);
		if (observableDictionary != null)
		{
			observableDictionary.EntryAdded += this.playersDictChanged;
			observableDictionary.EntryModified += this.playersDictChanged;
			observableDictionary.EntryRemoved += this.playersDictChanged;
			observableDictionary.EntryUpdatedValue += this.playersDictChanged;
		}
	}

	// Token: 0x060084F6 RID: 34038 RVA: 0x0032C8D4 File Offset: 0x0032AAD4
	public override void OnClose()
	{
		base.OnClose();
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		ObservableDictionary<PlatformUserIdentifierAbs, PersistentPlayerData> observableDictionary = ((persistentPlayers != null) ? persistentPlayers.Players : null);
		if (observableDictionary != null)
		{
			observableDictionary.EntryAdded -= this.playersDictChanged;
			observableDictionary.EntryModified -= this.playersDictChanged;
			observableDictionary.EntryRemoved -= this.playersDictChanged;
			observableDictionary.EntryUpdatedValue -= this.playersDictChanged;
		}
	}

	// Token: 0x060084F7 RID: 34039 RVA: 0x0032C949 File Offset: 0x0032AB49
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_SpawnNearFriendsList.Instance = null;
	}

	// Token: 0x060084F8 RID: 34040 RVA: 0x0032C957 File Offset: 0x0032AB57
	[PublicizedFrom(EAccessModifier.Private)]
	public void playersDictChanged(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _e)
	{
		this.UpdatePlayers();
	}

	// Token: 0x060084F9 RID: 34041 RVA: 0x0032C960 File Offset: 0x0032AB60
	public void UpdatePlayers()
	{
		XUiC_SpawnNearFriendsList.<>c__DisplayClass13_0 CS$<>8__locals1 = new XUiC_SpawnNearFriendsList.<>c__DisplayClass13_0();
		XUiC_SpawnNearFriendsList.<>c__DisplayClass13_0 CS$<>8__locals2 = CS$<>8__locals1;
		XUiC_SpawnNearFriendsList.ListEntry currentSelectedEntry = this.currentSelectedEntry;
		CS$<>8__locals2.previouslySelectedEntityId = ((currentSelectedEntry != null) ? currentSelectedEntry.PersistentPlayerData.EntityId : (-1));
		this.RebuildList(false);
		if (CS$<>8__locals1.previouslySelectedEntityId != -1)
		{
			base.SelectedEntryIndex = this.filteredEntries.FindIndex((XUiC_SpawnNearFriendsList.ListEntry _entry) => _entry.PersistentPlayerData.EntityId == CS$<>8__locals1.previouslySelectedEntityId);
		}
	}

	// Token: 0x060084FA RID: 34042 RVA: 0x0032C9C0 File Offset: 0x0032ABC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "selectionValid")
		{
			XUiC_SpawnNearFriendsList.ListEntry currentSelectedEntry = this.currentSelectedEntry;
			_value = (currentSelectedEntry != null && currentSelectedEntry.ValidSpawn).ToString();
			return true;
		}
		if (!(_bindingName == "invalidSpawnReason"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		XUiC_SpawnNearFriendsList.ListEntry currentSelectedEntry2 = this.currentSelectedEntry;
		_value = ((currentSelectedEntry2 != null) ? currentSelectedEntry2.InvalidSpawnReason : null) ?? "";
		return true;
	}

	// Token: 0x060084FB RID: 34043 RVA: 0x0032CA30 File Offset: 0x0032AC30
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode == ProtocolManager.NetworkType.None)
		{
			for (int i = 0; i < 10; i++)
			{
				this.allEntries.Add(new XUiC_SpawnNearFriendsList.ListEntry(i + (int)(Time.unscaledTime * 1000f)));
			}
			this.allEntries.Sort();
		}
		if (GameManager.Instance.World == null)
		{
			base.RebuildList(_resetFilter);
			return;
		}
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		if (persistentPlayers == null)
		{
			return;
		}
		bool value = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient.GetValue(GameInfoBool.AllowCrossplay);
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in persistentPlayers.Players)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			PersistentPlayerData persistentPlayerData;
			keyValuePair.Deconstruct(out platformUserIdentifierAbs, out persistentPlayerData);
			PersistentPlayerData persistentPlayerData2 = persistentPlayerData;
			IPlatformUserBlockedData platformUserBlockedData;
			if (persistentPlayerData2.EntityId != -1 && (!PlatformUserManager.GetOrCreate(persistentPlayerData2.PrimaryId).Blocked.TryGetValue(EBlockType.Play, out platformUserBlockedData) || !platformUserBlockedData.IsBlocked()))
			{
				this.allEntries.Add(new XUiC_SpawnNearFriendsList.ListEntry(persistentPlayerData2, value));
			}
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x040062B8 RID: 25272
	public static XUiC_SpawnNearFriendsList Instance;

	// Token: 0x0200105E RID: 4190
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SpawnNearFriendsList.ListEntry>
	{
		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x060084FE RID: 34046 RVA: 0x0032CB92 File Offset: 0x0032AD92
		public bool IsFriend
		{
			get
			{
				return !string.IsNullOrEmpty(this.friendPlatform);
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x060084FF RID: 34047 RVA: 0x0032CBA2 File Offset: 0x0032ADA2
		public bool ValidSpawn
		{
			get
			{
				return this.IsFriend && this.locationAllowed;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06008500 RID: 34048 RVA: 0x0032CBB4 File Offset: 0x0032ADB4
		public string InvalidSpawnReason
		{
			get
			{
				if (!this.IsFriend)
				{
					return Localization.Get("xuiSpawnNearFriendNotAFriend", false, null);
				}
				if (this.locationAllowed)
				{
					return null;
				}
				return this.invalidLocationReason;
			}
		}

		// Token: 0x06008501 RID: 34049 RVA: 0x0032CBDC File Offset: 0x0032ADDC
		public ListEntry(int _randomSeed)
		{
			GameRandom tempGameRandom = GameRandomManager.Instance.GetTempGameRandom(_randomSeed);
			int randomInt = tempGameRandom.RandomInt;
			this.name = randomInt.ToString();
			this.displayName = this.name;
			this.position = new Vector3i(tempGameRandom.RandomRange(-100, 101), 0, tempGameRandom.RandomRange(-100, 101));
			this.locationAllowed = randomInt / 2 % 2 == 0;
			this.friendPlatform = ((randomInt % 2 == 0) ? "X" : null);
			if (!this.locationAllowed)
			{
				this.invalidLocationReason = Localization.Get("xuiSpawnNearFriendNotInForest", false, null);
			}
			this.showIconCrossplay = randomInt % 2 == 0;
			this.iconCrossplaySprite = "ui_platform_pc";
		}

		// Token: 0x06008502 RID: 34050 RVA: 0x0032CC90 File Offset: 0x0032AE90
		public ListEntry(PersistentPlayerData _persistentPlayerData, bool _showPlatformIcons)
		{
			this.name = _persistentPlayerData.PlayerName.AuthoredName.Text;
			this.displayName = _persistentPlayerData.PlayerName.SafeDisplayName;
			this.PersistentPlayerData = _persistentPlayerData;
			PlayerData playerData = _persistentPlayerData.PlayerData;
			this.iconCrossplaySprite = PlatformManager.NativePlatform.Utils.GetCrossplayPlayerIcon(playerData.PlayGroup, true, playerData.NativeId.PlatformIdentifier);
			this.showIconCrossplay = _showPlatformIcons;
			DiscordManager.DiscordUser discordUser;
			if (DiscordManager.Instance.TryGetUserFromEntityId(_persistentPlayerData.EntityId, out discordUser) && discordUser.IsFriend)
			{
				this.friendPlatform = "Discord";
			}
			if (!PlatformUserIdentifierAbs.Equals(playerData.NativeId, playerData.PrimaryId) && playerData.PrimaryId != null && PlatformManager.MultiPlatform.User.IsFriend(playerData.PrimaryId) && this.friendPlatform == null)
			{
				this.friendPlatform = PlatformManager.GetPlatformDisplayName(playerData.PrimaryId.PlatformIdentifier);
			}
			if (PlatformManager.MultiPlatform.User.IsFriend(playerData.NativeId) && this.friendPlatform == null)
			{
				this.friendPlatform = PlatformManager.GetPlatformDisplayName(playerData.NativeId.PlatformIdentifier);
			}
			this.UpdatePosition();
		}

		// Token: 0x06008503 RID: 34051 RVA: 0x0032CDB8 File Offset: 0x0032AFB8
		public void UpdatePosition()
		{
			if (this.PersistentPlayerData == null)
			{
				return;
			}
			this.position = this.PersistentPlayerData.Position;
			World world = GameManager.Instance.World;
			this.biome = ((world != null) ? world.GetBiomeInWorld(this.position.x, this.position.z) : null);
			AllowSpawnNearFriend spawnNearFriendMode = XUiC_SpawnNearFriendsList.SpawnNearFriendMode;
			if (spawnNearFriendMode != AllowSpawnNearFriend.Always)
			{
				if (spawnNearFriendMode != AllowSpawnNearFriend.InForest)
				{
					this.locationAllowed = false;
					this.invalidLocationReason = Localization.Get("xuiSpawnNearFriendDisabled", false, null);
				}
				else
				{
					BiomeDefinition biomeDefinition = this.biome;
					BiomeDefinition.BiomeType? biomeType = ((biomeDefinition != null) ? new BiomeDefinition.BiomeType?(biomeDefinition.m_BiomeType) : null);
					bool flag;
					if (biomeType != null)
					{
						BiomeDefinition.BiomeType valueOrDefault = biomeType.GetValueOrDefault();
						if (valueOrDefault - BiomeDefinition.BiomeType.Forest <= 1)
						{
							flag = true;
							goto IL_00AD;
						}
					}
					flag = false;
					IL_00AD:
					this.locationAllowed = flag;
					if (!this.locationAllowed)
					{
						this.invalidLocationReason = Localization.Get("xuiSpawnNearFriendNotInForest", false, null);
						return;
					}
				}
				return;
			}
			this.locationAllowed = true;
			this.invalidLocationReason = string.Empty;
		}

		// Token: 0x06008504 RID: 34052 RVA: 0x0032CEB0 File Offset: 0x0032B0B0
		public override int CompareTo(XUiC_SpawnNearFriendsList.ListEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return 1;
			}
			int num = this.IsFriend.CompareTo(_otherEntry.IsFriend);
			if (num != 0)
			{
				return -num;
			}
			num = this.ValidSpawn.CompareTo(_otherEntry.ValidSpawn);
			if (num != 0)
			{
				return -num;
			}
			return string.Compare(this.name, _otherEntry.name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008505 RID: 34053 RVA: 0x0032CF0A File Offset: 0x0032B10A
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040062B9 RID: 25273
		public readonly PersistentPlayerData PersistentPlayerData;

		// Token: 0x040062BA RID: 25274
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;

		// Token: 0x040062BB RID: 25275
		public readonly string displayName;

		// Token: 0x040062BC RID: 25276
		public readonly bool showIconCrossplay;

		// Token: 0x040062BD RID: 25277
		public readonly string iconCrossplaySprite;

		// Token: 0x040062BE RID: 25278
		public Vector3i position;

		// Token: 0x040062BF RID: 25279
		public BiomeDefinition biome;

		// Token: 0x040062C0 RID: 25280
		public bool locationAllowed;

		// Token: 0x040062C1 RID: 25281
		[PublicizedFrom(EAccessModifier.Private)]
		public string invalidLocationReason;

		// Token: 0x040062C2 RID: 25282
		public readonly string friendPlatform;
	}

	// Token: 0x0200105F RID: 4191
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class SpawnNearFriendsListEntryController : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06008506 RID: 34054 RVA: 0x0032CF1F File Offset: 0x0032B11F
		public override void Init()
		{
			base.Init();
			this.btnProfile = base.GetChildById("btnViewProfile");
			this.btnProfile.OnPress += delegate(XUiController _, int _)
			{
				XUiC_SpawnNearFriendsList.ListEntry entry = base.GetEntry();
				if (entry == null || entry.PersistentPlayerData == null)
				{
					return;
				}
				if (entry.PersistentPlayerData.NativeId != null && PlatformManager.MultiPlatform.User.CanShowProfile(entry.PersistentPlayerData.NativeId))
				{
					PlatformManager.MultiPlatform.User.ShowProfile(entry.PersistentPlayerData.NativeId);
					return;
				}
			};
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x0032CF4F File Offset: 0x0032B14F
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}

		// Token: 0x06008508 RID: 34056 RVA: 0x0032CF6C File Offset: 0x0032B16C
		[XuiXmlBinding("position")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingPosition()
		{
			if (this.entryData != null)
			{
				return ValueDisplayFormatters.WorldPos(this.entryData.position, " ", false);
			}
			return "";
		}

		// Token: 0x06008509 RID: 34057 RVA: 0x0032CF97 File Offset: 0x0032B197
		[XuiXmlBinding("biomeName")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingBiomeName()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			string text;
			if (entryData == null)
			{
				text = null;
			}
			else
			{
				BiomeDefinition biome = entryData.biome;
				text = ((biome != null) ? biome.LocalizedName : null);
			}
			return text ?? "";
		}

		// Token: 0x0600850A RID: 34058 RVA: 0x0032CFC0 File Offset: 0x0032B1C0
		[XuiXmlBinding("locationAllowed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingLocationAllowed()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.locationAllowed;
		}

		// Token: 0x0600850B RID: 34059 RVA: 0x0032CFD3 File Offset: 0x0032B1D3
		[XuiXmlBinding("isFriend")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsFriend()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.IsFriend;
		}

		// Token: 0x0600850C RID: 34060 RVA: 0x0032CFE6 File Offset: 0x0032B1E6
		[XuiXmlBinding("friendPlatform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingFriendPlatform()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.friendPlatform : null) ?? "-";
		}

		// Token: 0x0600850D RID: 34061 RVA: 0x0032D003 File Offset: 0x0032B203
		[XuiXmlBinding("validSpawn")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidSpawn()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.ValidSpawn;
		}

		// Token: 0x0600850E RID: 34062 RVA: 0x0032D016 File Offset: 0x0032B216
		[XuiXmlBinding("invalidSpawnReason")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingInvalidSpawnReason()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.InvalidSpawnReason : null) ?? "";
		}

		// Token: 0x0600850F RID: 34063 RVA: 0x0032D033 File Offset: 0x0032B233
		[XuiXmlBinding("showIconCrossplay")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingShowIconCrossplay()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.showIconCrossplay;
		}

		// Token: 0x06008510 RID: 34064 RVA: 0x0032D046 File Offset: 0x0032B246
		[XuiXmlBinding("iconCrossplaySprite")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingIconCrossplaySprite()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.iconCrossplaySprite : null) ?? "";
		}

		// Token: 0x06008511 RID: 34065 RVA: 0x0032D063 File Offset: 0x0032B263
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.PersistentPlayerData : null) != null && PlatformManager.NativePlatform.User.CanShowProfile(this.entryData.PersistentPlayerData.PlayerData.NativeId);
		}

		// Token: 0x040062C3 RID: 25283
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiController btnProfile;
	}
}
