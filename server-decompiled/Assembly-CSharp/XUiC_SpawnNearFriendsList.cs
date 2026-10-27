using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001097 RID: 4247
[Preserve]
public class XUiC_SpawnNearFriendsList : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>
{
	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x0600867C RID: 34428 RVA: 0x00336C2E File Offset: 0x00334E2E
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
	// (add) Token: 0x0600867D RID: 34429 RVA: 0x00336C60 File Offset: 0x00334E60
	// (remove) Token: 0x0600867E RID: 34430 RVA: 0x00336C98 File Offset: 0x00334E98
	public event Action<PersistentPlayerData> SpawnClicked;

	// Token: 0x0600867F RID: 34431 RVA: 0x00336CD0 File Offset: 0x00334ED0
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

	// Token: 0x06008680 RID: 34432 RVA: 0x00336D10 File Offset: 0x00334F10
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

	// Token: 0x06008681 RID: 34433 RVA: 0x00336D8C File Offset: 0x00334F8C
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

	// Token: 0x06008682 RID: 34434 RVA: 0x00336E01 File Offset: 0x00335001
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_SpawnNearFriendsList.Instance = null;
	}

	// Token: 0x06008683 RID: 34435 RVA: 0x00336E0F File Offset: 0x0033500F
	[PublicizedFrom(EAccessModifier.Private)]
	public void playersDictChanged(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _e)
	{
		this.UpdatePlayers();
	}

	// Token: 0x06008684 RID: 34436 RVA: 0x00336E18 File Offset: 0x00335018
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

	// Token: 0x06008685 RID: 34437 RVA: 0x00336E78 File Offset: 0x00335078
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

	// Token: 0x06008686 RID: 34438 RVA: 0x00336EE8 File Offset: 0x003350E8
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

	// Token: 0x04006403 RID: 25603
	public static XUiC_SpawnNearFriendsList Instance;

	// Token: 0x02001098 RID: 4248
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SpawnNearFriendsList.ListEntry>
	{
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06008689 RID: 34441 RVA: 0x0033704A File Offset: 0x0033524A
		public bool IsFriend
		{
			get
			{
				return !string.IsNullOrEmpty(this.friendPlatform);
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x0600868A RID: 34442 RVA: 0x0033705A File Offset: 0x0033525A
		public bool ValidSpawn
		{
			get
			{
				return this.IsFriend && this.locationAllowed;
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x0600868B RID: 34443 RVA: 0x0033706C File Offset: 0x0033526C
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

		// Token: 0x0600868C RID: 34444 RVA: 0x00337094 File Offset: 0x00335294
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

		// Token: 0x0600868D RID: 34445 RVA: 0x00337148 File Offset: 0x00335348
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

		// Token: 0x0600868E RID: 34446 RVA: 0x00337270 File Offset: 0x00335470
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

		// Token: 0x0600868F RID: 34447 RVA: 0x00337368 File Offset: 0x00335568
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

		// Token: 0x06008690 RID: 34448 RVA: 0x003373C2 File Offset: 0x003355C2
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04006404 RID: 25604
		public readonly PersistentPlayerData PersistentPlayerData;

		// Token: 0x04006405 RID: 25605
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;

		// Token: 0x04006406 RID: 25606
		public readonly string displayName;

		// Token: 0x04006407 RID: 25607
		public readonly bool showIconCrossplay;

		// Token: 0x04006408 RID: 25608
		public readonly string iconCrossplaySprite;

		// Token: 0x04006409 RID: 25609
		public Vector3i position;

		// Token: 0x0400640A RID: 25610
		public BiomeDefinition biome;

		// Token: 0x0400640B RID: 25611
		public bool locationAllowed;

		// Token: 0x0400640C RID: 25612
		[PublicizedFrom(EAccessModifier.Private)]
		public string invalidLocationReason;

		// Token: 0x0400640D RID: 25613
		public readonly string friendPlatform;
	}

	// Token: 0x02001099 RID: 4249
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class SpawnNearFriendsListEntryController : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x06008691 RID: 34449 RVA: 0x003373D7 File Offset: 0x003355D7
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

		// Token: 0x06008692 RID: 34450 RVA: 0x00337407 File Offset: 0x00335607
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x00337424 File Offset: 0x00335624
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

		// Token: 0x06008694 RID: 34452 RVA: 0x0033744F File Offset: 0x0033564F
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

		// Token: 0x06008695 RID: 34453 RVA: 0x00337478 File Offset: 0x00335678
		[XuiXmlBinding("locationAllowed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingLocationAllowed()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.locationAllowed;
		}

		// Token: 0x06008696 RID: 34454 RVA: 0x0033748B File Offset: 0x0033568B
		[XuiXmlBinding("isFriend")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsFriend()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.IsFriend;
		}

		// Token: 0x06008697 RID: 34455 RVA: 0x0033749E File Offset: 0x0033569E
		[XuiXmlBinding("friendPlatform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingFriendPlatform()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.friendPlatform : null) ?? "-";
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x003374BB File Offset: 0x003356BB
		[XuiXmlBinding("validSpawn")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidSpawn()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.ValidSpawn;
		}

		// Token: 0x06008699 RID: 34457 RVA: 0x003374CE File Offset: 0x003356CE
		[XuiXmlBinding("invalidSpawnReason")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingInvalidSpawnReason()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.InvalidSpawnReason : null) ?? "";
		}

		// Token: 0x0600869A RID: 34458 RVA: 0x003374EB File Offset: 0x003356EB
		[XuiXmlBinding("showIconCrossplay")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingShowIconCrossplay()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.showIconCrossplay;
		}

		// Token: 0x0600869B RID: 34459 RVA: 0x003374FE File Offset: 0x003356FE
		[XuiXmlBinding("iconCrossplaySprite")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingIconCrossplaySprite()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.iconCrossplaySprite : null) ?? "";
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x0033751B File Offset: 0x0033571B
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.PersistentPlayerData : null) != null && PlatformManager.NativePlatform.User.CanShowProfile(this.entryData.PersistentPlayerData.PlayerData.NativeId);
		}

		// Token: 0x0400640E RID: 25614
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiController btnProfile;
	}
}
