using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001076 RID: 4214
[Preserve]
public class XUiC_SpawnNearFriendsList : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>
{
	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x060085E3 RID: 34275 RVA: 0x00330F9E File Offset: 0x0032F19E
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

	// Token: 0x140000DC RID: 220
	// (add) Token: 0x060085E4 RID: 34276 RVA: 0x00330FD0 File Offset: 0x0032F1D0
	// (remove) Token: 0x060085E5 RID: 34277 RVA: 0x00331008 File Offset: 0x0032F208
	public event Action<PersistentPlayerData> SpawnClicked;

	// Token: 0x060085E6 RID: 34278 RVA: 0x00331040 File Offset: 0x0032F240
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

	// Token: 0x060085E7 RID: 34279 RVA: 0x00331080 File Offset: 0x0032F280
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

	// Token: 0x060085E8 RID: 34280 RVA: 0x003310FC File Offset: 0x0032F2FC
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

	// Token: 0x060085E9 RID: 34281 RVA: 0x00331171 File Offset: 0x0032F371
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_SpawnNearFriendsList.Instance = null;
	}

	// Token: 0x060085EA RID: 34282 RVA: 0x0033117F File Offset: 0x0032F37F
	[PublicizedFrom(EAccessModifier.Private)]
	public void playersDictChanged(object _sender, DictionaryChangedEventArgs<PlatformUserIdentifierAbs, PersistentPlayerData> _e)
	{
		this.UpdatePlayers();
	}

	// Token: 0x060085EB RID: 34283 RVA: 0x00331188 File Offset: 0x0032F388
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

	// Token: 0x060085EC RID: 34284 RVA: 0x003311E8 File Offset: 0x0032F3E8
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

	// Token: 0x060085ED RID: 34285 RVA: 0x00331258 File Offset: 0x0032F458
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

	// Token: 0x0400637F RID: 25471
	public static XUiC_SpawnNearFriendsList Instance;

	// Token: 0x02001077 RID: 4215
	[Preserve]
	public class ListEntry : XUiListEntry<XUiC_SpawnNearFriendsList.ListEntry>
	{
		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060085F0 RID: 34288 RVA: 0x003313BA File Offset: 0x0032F5BA
		public bool IsFriend
		{
			get
			{
				return !string.IsNullOrEmpty(this.friendPlatform);
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060085F1 RID: 34289 RVA: 0x003313CA File Offset: 0x0032F5CA
		public bool ValidSpawn
		{
			get
			{
				return this.IsFriend && this.locationAllowed;
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060085F2 RID: 34290 RVA: 0x003313DC File Offset: 0x0032F5DC
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

		// Token: 0x060085F3 RID: 34291 RVA: 0x00331404 File Offset: 0x0032F604
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

		// Token: 0x060085F4 RID: 34292 RVA: 0x003314B8 File Offset: 0x0032F6B8
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

		// Token: 0x060085F5 RID: 34293 RVA: 0x003315E0 File Offset: 0x0032F7E0
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

		// Token: 0x060085F6 RID: 34294 RVA: 0x003316D8 File Offset: 0x0032F8D8
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

		// Token: 0x060085F7 RID: 34295 RVA: 0x00331732 File Offset: 0x0032F932
		public override bool MatchesSearch(string _searchString)
		{
			return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04006380 RID: 25472
		public readonly PersistentPlayerData PersistentPlayerData;

		// Token: 0x04006381 RID: 25473
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;

		// Token: 0x04006382 RID: 25474
		public readonly string displayName;

		// Token: 0x04006383 RID: 25475
		public readonly bool showIconCrossplay;

		// Token: 0x04006384 RID: 25476
		public readonly string iconCrossplaySprite;

		// Token: 0x04006385 RID: 25477
		public Vector3i position;

		// Token: 0x04006386 RID: 25478
		public BiomeDefinition biome;

		// Token: 0x04006387 RID: 25479
		public bool locationAllowed;

		// Token: 0x04006388 RID: 25480
		[PublicizedFrom(EAccessModifier.Private)]
		public string invalidLocationReason;

		// Token: 0x04006389 RID: 25481
		public readonly string friendPlatform;
	}

	// Token: 0x02001078 RID: 4216
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class SpawnNearFriendsListEntryController : XUiC_List<XUiC_SpawnNearFriendsList.ListEntry>.XUiC_ListEntry
	{
		// Token: 0x060085F8 RID: 34296 RVA: 0x00331747 File Offset: 0x0032F947
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

		// Token: 0x060085F9 RID: 34297 RVA: 0x00331777 File Offset: 0x0032F977
		[XuiXmlBinding("name")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingName()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.displayName : null) ?? "";
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x00331794 File Offset: 0x0032F994
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

		// Token: 0x060085FB RID: 34299 RVA: 0x003317BF File Offset: 0x0032F9BF
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

		// Token: 0x060085FC RID: 34300 RVA: 0x003317E8 File Offset: 0x0032F9E8
		[XuiXmlBinding("locationAllowed")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingLocationAllowed()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.locationAllowed;
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x003317FB File Offset: 0x0032F9FB
		[XuiXmlBinding("isFriend")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsFriend()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.IsFriend;
		}

		// Token: 0x060085FE RID: 34302 RVA: 0x0033180E File Offset: 0x0032FA0E
		[XuiXmlBinding("friendPlatform")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingFriendPlatform()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.friendPlatform : null) ?? "-";
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x0033182B File Offset: 0x0032FA2B
		[XuiXmlBinding("validSpawn")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingValidSpawn()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.ValidSpawn;
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x0033183E File Offset: 0x0032FA3E
		[XuiXmlBinding("invalidSpawnReason")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingInvalidSpawnReason()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.InvalidSpawnReason : null) ?? "";
		}

		// Token: 0x06008601 RID: 34305 RVA: 0x0033185B File Offset: 0x0032FA5B
		[XuiXmlBinding("showIconCrossplay")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingShowIconCrossplay()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return entryData != null && entryData.showIconCrossplay;
		}

		// Token: 0x06008602 RID: 34306 RVA: 0x0033186E File Offset: 0x0032FA6E
		[XuiXmlBinding("iconCrossplaySprite")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingIconCrossplaySprite()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.iconCrossplaySprite : null) ?? "";
		}

		// Token: 0x06008603 RID: 34307 RVA: 0x0033188B File Offset: 0x0032FA8B
		[XuiXmlBinding("canShowProfile")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingCanShowProfile()
		{
			XUiC_SpawnNearFriendsList.ListEntry entryData = this.entryData;
			return ((entryData != null) ? entryData.PersistentPlayerData : null) != null && PlatformManager.NativePlatform.User.CanShowProfile(this.entryData.PersistentPlayerData.PlayerData.NativeId);
		}

		// Token: 0x0400638A RID: 25482
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiController btnProfile;
	}
}
