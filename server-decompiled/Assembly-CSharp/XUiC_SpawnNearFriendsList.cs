using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001087 RID: 4231
[Preserve]
public class XUiC_SpawnNearFriendsList : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>
{
	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x06008629 RID: 34345 RVA: 0x00334BAA File Offset: 0x00332DAA
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
	// (add) Token: 0x0600862A RID: 34346 RVA: 0x00334BDC File Offset: 0x00332DDC
	// (remove) Token: 0x0600862B RID: 34347 RVA: 0x00334C14 File Offset: 0x00332E14
	public event Action<PersistentPlayerData> SpawnClicked;

	// Token: 0x0600862C RID: 34348 RVA: 0x00334C4C File Offset: 0x00332E4C
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

	// Token: 0x0600862D RID: 34349 RVA: 0x00334C8C File Offset: 0x00332E8C
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

	// Token: 0x0600862E RID: 34350 RVA: 0x00334D08 File Offset: 0x00332F08
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

	// Token: 0x0600862F RID: 34351 RVA: 0x00334D7D File Offset: 0x00332F7D
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_SpawnNearFriendsList.Instance = null;
	}

	// Token: 0x06008630 RID: 34352 RVA: 0x00334D8B File Offset: 0x00332F8B
	[PublicizedFrom(EAccessModifier.Private)]
	public void playersDictChanged(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _e)
	{
		this.UpdatePlayers();
	}

	// Token: 0x06008631 RID: 34353 RVA: 0x00334D94 File Offset: 0x00332F94
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

	// Token: 0x06008632 RID: 34354 RVA: 0x00334DF4 File Offset: 0x00332FF4
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

	// Token: 0x06008633 RID: 34355 RVA: 0x00334E64 File Offset: 0x00333064
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

	// Token: 0x040063C4 RID: 25540
	public static XUiC_SpawnNearFriendsList Instance;

	// Token: 0x02001088 RID: 4232
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SpawnNearFriendsList.ListEntry>
	{
		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06008636 RID: 34358 RVA: 0x00334FC6 File Offset: 0x003331C6
		public bool IsFriend
		{
			get
			{
				return !string.IsNullOrEmpty(this.friendPlatform);
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06008637 RID: 34359 RVA: 0x00334FD6 File Offset: 0x003331D6
		public bool ValidSpawn
		{
			get
			{
				return this.IsFriend && this.locationAllowed;
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06008638 RID: 34360 RVA: 0x00334FE8 File Offset: 0x003331E8
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

		// Token: 0x06008639 RID: 34361 RVA: 0x00335010 File Offset: 0x00333210
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

		// Token: 0x0600863A RID: 34362 RVA: 0x003350C4 File Offset: 0x003332C4
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

		// Token: 0x0600863B RID: 34363 RVA: 0x003351EC File Offset: 0x003333EC
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

		// Token: 0x0600863C RID: 34364 RVA: 0x003352E4 File Offset: 0x003334E4
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

		// Token: 0x0600863D RID: 34365 RVA: 0x0033533E File Offset: 0x0033353E
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040063C5 RID: 25541
		public readonly PersistentPlayerData PersistentPlayerData;

		// Token: 0x040063C6 RID: 25542
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;

		// Token: 0x040063C7 RID: 25543
		public readonly string displayName;

		// Token: 0x040063C8 RID: 25544
		public readonly bool showIconCrossplay;

		// Token: 0x040063C9 RID: 25545
		public readonly string iconCrossplaySprite;

		// Token: 0x040063CA RID: 25546
		public Vector3i position;

		// Token: 0x040063CB RID: 25547
		public BiomeDefinition biome;

		// Token: 0x040063CC RID: 25548
		public bool locationAllowed;

		// Token: 0x040063CD RID: 25549
		[PublicizedFrom(EAccessModifier.Private)]
		public string invalidLocationReason;

		// Token: 0x040063CE RID: 25550
		public readonly string friendPlatform;
	}

	// Token: 0x02001089 RID: 4233
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class SpawnNearFriendsListEntryController : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x0600863E RID: 34366 RVA: 0x00335353 File Offset: 0x00333553
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

		// Token: 0x0600863F RID: 34367 RVA: 0x00335383 File Offset: 0x00333583
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}

		// Token: 0x06008640 RID: 34368 RVA: 0x003353A0 File Offset: 0x003335A0
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

		// Token: 0x06008641 RID: 34369 RVA: 0x003353CB File Offset: 0x003335CB
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

		// Token: 0x06008642 RID: 34370 RVA: 0x003353F4 File Offset: 0x003335F4
		[XuiXmlBinding("locationAllowed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingLocationAllowed()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.locationAllowed;
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x00335407 File Offset: 0x00333607
		[XuiXmlBinding("isFriend")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsFriend()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.IsFriend;
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x0033541A File Offset: 0x0033361A
		[XuiXmlBinding("friendPlatform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingFriendPlatform()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.friendPlatform : null) ?? "-";
		}

		// Token: 0x06008645 RID: 34373 RVA: 0x00335437 File Offset: 0x00333637
		[XuiXmlBinding("validSpawn")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidSpawn()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.ValidSpawn;
		}

		// Token: 0x06008646 RID: 34374 RVA: 0x0033544A File Offset: 0x0033364A
		[XuiXmlBinding("invalidSpawnReason")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingInvalidSpawnReason()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.InvalidSpawnReason : null) ?? "";
		}

		// Token: 0x06008647 RID: 34375 RVA: 0x00335467 File Offset: 0x00333667
		[XuiXmlBinding("showIconCrossplay")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingShowIconCrossplay()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.showIconCrossplay;
		}

		// Token: 0x06008648 RID: 34376 RVA: 0x0033547A File Offset: 0x0033367A
		[XuiXmlBinding("iconCrossplaySprite")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingIconCrossplaySprite()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.iconCrossplaySprite : null) ?? "";
		}

		// Token: 0x06008649 RID: 34377 RVA: 0x00335497 File Offset: 0x00333697
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.PersistentPlayerData : null) != null && PlatformManager.NativePlatform.User.CanShowProfile(this.entryData.PersistentPlayerData.PlayerData.NativeId);
		}

		// Token: 0x040063CF RID: 25551
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiController btnProfile;
	}
}
