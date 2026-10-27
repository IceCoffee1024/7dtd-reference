using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Platform;
using SandboxOptions;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public class GameServerInfo
{
	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06003945 RID: 14661 RVA: 0x001777C8 File Offset: 0x001759C8
	// (remove) Token: 0x06003946 RID: 14662 RVA: 0x00177800 File Offset: 0x00175A00
	public event Action<GameServerInfo> OnChangedAny;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06003947 RID: 14663 RVA: 0x00177838 File Offset: 0x00175A38
	// (remove) Token: 0x06003948 RID: 14664 RVA: 0x00177870 File Offset: 0x00175A70
	public event Action<GameServerInfo, GameInfoString> OnChangedString;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06003949 RID: 14665 RVA: 0x001778A8 File Offset: 0x00175AA8
	// (remove) Token: 0x0600394A RID: 14666 RVA: 0x001778E0 File Offset: 0x00175AE0
	public event Action<GameServerInfo, GameInfoInt> OnChangedInt;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x0600394B RID: 14667 RVA: 0x00177918 File Offset: 0x00175B18
	// (remove) Token: 0x0600394C RID: 14668 RVA: 0x00177950 File Offset: 0x00175B50
	public event Action<GameServerInfo, GameInfoBool> OnChangedBool;

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x0600394D RID: 14669 RVA: 0x00177985 File Offset: 0x00175B85
	public bool IsValid
	{
		get
		{
			return !this.isBroken;
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x00177990 File Offset: 0x00175B90
	public bool IsDedicated
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated);
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x0600394F RID: 14671 RVA: 0x00177999 File Offset: 0x00175B99
	public bool IsDedicatedStock
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated) && this.GetValue(GameInfoBool.StockSettings) && !this.GetValue(GameInfoBool.ModdedConfig);
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x001779BA File Offset: 0x00175BBA
	public bool IsDedicatedModded
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated) && this.GetValue(GameInfoBool.ModdedConfig);
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06003951 RID: 14673 RVA: 0x001779CF File Offset: 0x00175BCF
	public bool IsPeerToPeer
	{
		get
		{
			return !this.GetValue(GameInfoBool.IsDedicated) && !this.isNoResponse;
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06003952 RID: 14674 RVA: 0x001779E5 File Offset: 0x00175BE5
	public bool AllowsCrossplay
	{
		get
		{
			return this.GetValue(GameInfoBool.AllowCrossplay);
		}
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06003953 RID: 14675 RVA: 0x001779EF File Offset: 0x00175BEF
	public bool EACEnabled
	{
		get
		{
			return this.GetValue(GameInfoBool.EACEnabled);
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06003954 RID: 14676 RVA: 0x001779F8 File Offset: 0x00175BF8
	public bool IgnoresSanctions
	{
		get
		{
			return this.GetValue(GameInfoBool.SanctionsIgnored);
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06003955 RID: 14677 RVA: 0x00177A04 File Offset: 0x00175C04
	public EPlayGroup PlayGroup
	{
		get
		{
			EPlayGroup eplayGroup;
			if (!EnumUtils.TryParse<EPlayGroup>(this.GetValue(GameInfoString.PlayGroup), out eplayGroup, false))
			{
				return EPlayGroup.Unknown;
			}
			return eplayGroup;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x00177A26 File Offset: 0x00175C26
	// (set) Token: 0x06003957 RID: 14679 RVA: 0x00177A2E File Offset: 0x00175C2E
	public bool IsFriends
	{
		get
		{
			return this.isFriends;
		}
		set
		{
			if (value != this.isFriends)
			{
				this.isFriends = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06003958 RID: 14680 RVA: 0x00177A51 File Offset: 0x00175C51
	public bool IsFavoriteHistory
	{
		get
		{
			return this.IsFavorite || this.IsHistory;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06003959 RID: 14681 RVA: 0x00177A63 File Offset: 0x00175C63
	// (set) Token: 0x0600395A RID: 14682 RVA: 0x00177A6B File Offset: 0x00175C6B
	public bool IsFavorite
	{
		get
		{
			return this.isFavorite;
		}
		set
		{
			if (value != this.isFavorite)
			{
				this.isFavorite = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600395B RID: 14683 RVA: 0x00177A8E File Offset: 0x00175C8E
	public bool IsHistory
	{
		get
		{
			return this.lastPlayed > 0;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600395C RID: 14684 RVA: 0x00177A99 File Offset: 0x00175C99
	// (set) Token: 0x0600395D RID: 14685 RVA: 0x00177AA1 File Offset: 0x00175CA1
	public int LastPlayedLinux
	{
		get
		{
			return this.lastPlayed;
		}
		set
		{
			if (value != this.lastPlayed)
			{
				this.lastPlayed = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x0600395E RID: 14686 RVA: 0x00177AC4 File Offset: 0x00175CC4
	// (set) Token: 0x0600395F RID: 14687 RVA: 0x00177ACC File Offset: 0x00175CCC
	public bool IsLAN
	{
		get
		{
			return this.isLan;
		}
		set
		{
			if (value != this.isLan)
			{
				this.isLan = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06003960 RID: 14688 RVA: 0x00177AEF File Offset: 0x00175CEF
	// (set) Token: 0x06003961 RID: 14689 RVA: 0x00177AF7 File Offset: 0x00175CF7
	public bool IsLobby
	{
		get
		{
			return this.isLobby;
		}
		set
		{
			if (value != this.isLobby)
			{
				this.isLobby = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06003962 RID: 14690 RVA: 0x00177B1A File Offset: 0x00175D1A
	// (set) Token: 0x06003963 RID: 14691 RVA: 0x00177B22 File Offset: 0x00175D22
	public bool IsNoResponse
	{
		get
		{
			return this.isNoResponse;
		}
		set
		{
			if (value != this.isNoResponse)
			{
				this.isNoResponse = value;
				Action<GameServerInfo> onChangedAny = this.OnChangedAny;
				if (onChangedAny == null)
				{
					return;
				}
				onChangedAny(this);
			}
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06003964 RID: 14692 RVA: 0x00177B45 File Offset: 0x00175D45
	public VersionInformation Version
	{
		get
		{
			return this.version;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06003965 RID: 14693 RVA: 0x00177B4D File Offset: 0x00175D4D
	public bool IsCompatibleVersion
	{
		get
		{
			return this.version.Major < 0 || this.version.EqualsMinor(Constants.cVersionInformation);
		}
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00177B70 File Offset: 0x00175D70
	public GameServerInfo()
	{
		this.OnChangedAny += this.RefreshServerDisplayTexts;
		this.Strings = new ReadOnlyDictionary<GameInfoString, string>(this.tableStrings);
		this.Ints = new ReadOnlyDictionary<GameInfoInt, int>(this.tableInts);
		this.Bools = new ReadOnlyDictionary<GameInfoBool, bool>(this.tableBools);
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x00177C40 File Offset: 0x00175E40
	public GameServerInfo(GameServerInfo _gsi)
	{
		foreach (KeyValuePair<GameInfoString, string> keyValuePair in _gsi.tableStrings)
		{
			this.SetValue(keyValuePair.Key, keyValuePair.Value);
		}
		foreach (KeyValuePair<GameInfoInt, int> keyValuePair2 in _gsi.tableInts)
		{
			this.SetValue(keyValuePair2.Key, keyValuePair2.Value);
		}
		foreach (KeyValuePair<GameInfoBool, bool> keyValuePair3 in _gsi.tableBools)
		{
			this.SetValue(keyValuePair3.Key, keyValuePair3.Value);
		}
		this.isBroken = _gsi.isBroken;
		this.isFriends = _gsi.isFriends;
		this.isFavorite = _gsi.isFavorite;
		this.lastPlayed = _gsi.lastPlayed;
		this.isLan = _gsi.isLan;
		this.isLobby = _gsi.isLobby;
		this.isNoResponse = _gsi.isNoResponse;
		this.RefreshServerDisplayTexts(this);
		this.OnChangedAny += this.RefreshServerDisplayTexts;
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00177E2C File Offset: 0x0017602C
	public GameServerInfo(string _serverInfoString)
	{
		if (_serverInfoString.Length == 0)
		{
			this.isBroken = true;
			return;
		}
		this.BuildInfoFromString(_serverInfoString);
		this.OnChangedAny += this.RefreshServerDisplayTexts;
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x00177EE0 File Offset: 0x001760E0
	public void Merge(GameServerInfo _gameServerInfo, EServerRelationType _source)
	{
		this.isFriends |= _gameServerInfo.IsFriends;
		this.isFavorite |= _gameServerInfo.IsFavorite;
		this.isLan |= _gameServerInfo.IsLAN;
		if (_source == EServerRelationType.History)
		{
			this.lastPlayed = _gameServerInfo.LastPlayedLinux;
		}
		foreach (KeyValuePair<GameInfoBool, bool> keyValuePair in _gameServerInfo.tableBools)
		{
			if (keyValuePair.Value)
			{
				this.SetValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		foreach (KeyValuePair<GameInfoString, string> keyValuePair2 in _gameServerInfo.tableStrings)
		{
			if (_source == EServerRelationType.LAN || keyValuePair2.Key != GameInfoString.IP)
			{
				this.SetValue(keyValuePair2.Key, keyValuePair2.Value);
			}
		}
		foreach (KeyValuePair<GameInfoInt, int> keyValuePair3 in _gameServerInfo.tableInts)
		{
			this.SetValue(keyValuePair3.Key, keyValuePair3.Value);
		}
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x00178040 File Offset: 0x00176240
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildInfoFromString(string _serverInfoString)
	{
		if (_serverInfoString.Length == 0)
		{
			this.isBroken = true;
			return;
		}
		foreach (string text in _serverInfoString.Substring(0, _serverInfoString.Length - 1).Split(';', StringSplitOptions.None))
		{
			if (text.Length >= 2)
			{
				string[] array2 = text.Split(':', StringSplitOptions.None);
				if (array2.Length >= 2)
				{
					string text2 = array2[0].Trim(GameServerInfo.whiteSpaceChars);
					string text3 = array2[1];
					this.ParseAny(text2, text3);
				}
			}
		}
		this.RefreshServerDisplayTexts(this);
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x001780C8 File Offset: 0x001762C8
	public bool ParseAny(string _key, string _value)
	{
		bool flag;
		try
		{
			GameInfoString gameInfoString;
			GameInfoInt gameInfoInt;
			GameInfoBool gameInfoBool;
			if (EnumUtils.TryParse<GameInfoString>(_key, out gameInfoString, true))
			{
				this.SetValue(gameInfoString, _value);
			}
			else if (EnumUtils.TryParse<GameInfoInt>(_key, out gameInfoInt, true))
			{
				this.SetValue(gameInfoInt, Convert.ToInt32(_value));
			}
			else if (EnumUtils.TryParse<GameInfoBool>(_key, out gameInfoBool, true))
			{
				this.SetValue(gameInfoBool, StringParsers.ParseBool(_value, 0, -1, true));
			}
			flag = true;
		}
		catch (Exception)
		{
			string text = this.GetValue(GameInfoString.IP);
			if (string.IsNullOrEmpty(text))
			{
				text = "<unknown>";
			}
			int value = this.GetValue(GameInfoInt.Port);
			Log.Warning("GameServer {0}:{1} replied with invalid setting: {2}={3}", new object[] { text, value, _key, _value });
			this.isBroken = true;
			flag = false;
		}
		return flag;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x0017818C File Offset: 0x0017638C
	public bool Parse(string _key, string _value)
	{
		GameInfoString gameInfoString;
		if (EnumUtils.TryParse<GameInfoString>(_key, out gameInfoString, true))
		{
			this.SetValue(gameInfoString, _value);
			return true;
		}
		return false;
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x001781B4 File Offset: 0x001763B4
	public bool Parse(string _key, int _value)
	{
		GameInfoInt gameInfoInt;
		if (EnumUtils.TryParse<GameInfoInt>(_key, out gameInfoInt, true))
		{
			this.SetValue(gameInfoInt, _value);
			return true;
		}
		return false;
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x001781DC File Offset: 0x001763DC
	public bool Parse(string _key, bool _value)
	{
		GameInfoBool gameInfoBool;
		if (EnumUtils.TryParse<GameInfoBool>(_key, out gameInfoBool, true))
		{
			this.SetValue(gameInfoBool, _value);
			return true;
		}
		return false;
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00178204 File Offset: 0x00176404
	public void SetValue(GameInfoString _key, string _value)
	{
		if (_value == null)
		{
			_value = "";
		}
		this.tableStrings[_key] = _value.Replace(':', '^').Replace(';', '*');
		if (_key == GameInfoString.IP || _key == GameInfoString.SteamID || _key == GameInfoString.UniqueId)
		{
			this.hashcode = long.MinValue;
		}
		if (_key == GameInfoString.ServerVersion)
		{
			VersionInformation versionInformation;
			if (VersionInformation.TryParseSerializedString(_value, out versionInformation))
			{
				this.version = versionInformation;
			}
			else
			{
				Log.Warning("Server browser: Could not parse version from received data (from entry: " + this.GetValue(GameInfoString.IP) + "): " + _value);
			}
		}
		this.cachedToString = null;
		this.cachedToStringLineBreaks = null;
		Action<GameServerInfo, GameInfoString> onChangedString = this.OnChangedString;
		if (onChangedString != null)
		{
			onChangedString(this, _key);
		}
		Action<GameServerInfo> onChangedAny = this.OnChangedAny;
		if (onChangedAny == null)
		{
			return;
		}
		onChangedAny(this);
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x001782BC File Offset: 0x001764BC
	public string GetValue(GameInfoString _key)
	{
		string text;
		if (!this.tableStrings.TryGetValue(_key, out text))
		{
			return "";
		}
		return text.Replace('^', ':').Replace('*', ';');
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x001782F4 File Offset: 0x001764F4
	public void SetValue(GameInfoInt _key, int _value)
	{
		if (_key != GameInfoInt.Ping || !this.tableInts.ContainsKey(GameInfoInt.Ping) || _value >= 0)
		{
			this.tableInts[_key] = _value;
		}
		if (_key == GameInfoInt.Port)
		{
			this.hashcode = long.MinValue;
		}
		this.cachedToString = null;
		this.cachedToStringLineBreaks = null;
		Action<GameServerInfo, GameInfoInt> onChangedInt = this.OnChangedInt;
		if (onChangedInt != null)
		{
			onChangedInt(this, _key);
		}
		Action<GameServerInfo> onChangedAny = this.OnChangedAny;
		if (onChangedAny == null)
		{
			return;
		}
		onChangedAny(this);
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x0017836C File Offset: 0x0017656C
	public int GetValue(GameInfoInt _key)
	{
		int num;
		if (!this.tableInts.TryGetValue(_key, out num))
		{
			return -1;
		}
		return num;
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x0017838C File Offset: 0x0017658C
	public void SetValue(GameInfoBool _key, bool _value)
	{
		this.tableBools[_key] = _value;
		this.cachedToString = null;
		this.cachedToStringLineBreaks = null;
		Action<GameServerInfo, GameInfoBool> onChangedBool = this.OnChangedBool;
		if (onChangedBool != null)
		{
			onChangedBool(this, _key);
		}
		Action<GameServerInfo> onChangedAny = this.OnChangedAny;
		if (onChangedAny == null)
		{
			return;
		}
		onChangedAny(this);
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x001783D8 File Offset: 0x001765D8
	public bool GetValue(GameInfoBool _key)
	{
		bool flag;
		return this.tableBools.TryGetValue(_key, out flag) && flag;
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x001783F5 File Offset: 0x001765F5
	public override string ToString()
	{
		return this.ToString(false);
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x00178400 File Offset: 0x00176600
	public string ToString(bool _lineBreaks)
	{
		if ((!_lineBreaks && this.cachedToString == null) || (_lineBreaks && this.cachedToStringLineBreaks == null))
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<GameInfoString, string> keyValuePair in this.tableStrings)
			{
				stringBuilder.Append(keyValuePair.Key.ToStringCached<GameInfoString>());
				stringBuilder.Append(':');
				stringBuilder.Append(keyValuePair.Value);
				stringBuilder.Append(';');
				if (_lineBreaks)
				{
					stringBuilder.Append('\r');
					stringBuilder.Append('\n');
				}
			}
			foreach (KeyValuePair<GameInfoInt, int> keyValuePair2 in this.tableInts)
			{
				stringBuilder.Append(keyValuePair2.Key.ToStringCached<GameInfoInt>());
				stringBuilder.Append(':');
				stringBuilder.Append(keyValuePair2.Value);
				stringBuilder.Append(';');
				if (_lineBreaks)
				{
					stringBuilder.Append('\r');
					stringBuilder.Append('\n');
				}
			}
			foreach (KeyValuePair<GameInfoBool, bool> keyValuePair3 in this.tableBools)
			{
				stringBuilder.Append(keyValuePair3.Key.ToStringCached<GameInfoBool>());
				stringBuilder.Append(':');
				stringBuilder.Append(keyValuePair3.Value);
				stringBuilder.Append(';');
				if (_lineBreaks)
				{
					stringBuilder.Append('\r');
					stringBuilder.Append('\n');
				}
			}
			stringBuilder.Append('\r');
			stringBuilder.Append('\n');
			if (_lineBreaks)
			{
				this.cachedToStringLineBreaks = stringBuilder.ToString();
			}
			else
			{
				this.cachedToString = stringBuilder.ToString();
			}
		}
		if (!_lineBreaks)
		{
			return this.cachedToString;
		}
		return this.cachedToStringLineBreaks;
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x00178600 File Offset: 0x00176800
	public override int GetHashCode()
	{
		if (this.hashcode == -9223372036854775808L)
		{
			string text = this.GetValue(GameInfoString.IP) + this.GetValue(GameInfoInt.Port).ToString();
			this.hashcode = (long)text.GetHashCode();
		}
		return (int)this.hashcode;
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00178650 File Offset: 0x00176850
	public override bool Equals(object _obj)
	{
		if (_obj == null)
		{
			return false;
		}
		GameServerInfo gameServerInfo = _obj as GameServerInfo;
		return this.Equals(gameServerInfo);
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x00178670 File Offset: 0x00176870
	public bool Equals(GameServerInfo _p)
	{
		return _p != null && this.GetValue(GameInfoString.IP) == _p.GetValue(GameInfoString.IP) && this.GetValue(GameInfoInt.Port) == _p.GetValue(GameInfoInt.Port);
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x001786A0 File Offset: 0x001768A0
	public void UpdateGameTimePlayers(ulong _time, int _players)
	{
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime - this.timeLastWorldTimeUpdate > 15f || this.GetValue(GameInfoInt.CurrentPlayers) != _players)
		{
			this.timeLastWorldTimeUpdate = unscaledTime;
			if (PrefabEditModeManager.Instance.IsActive())
			{
				this.SetValue(GameInfoString.LevelName, PrefabEditModeManager.Instance.LoadedPrefab.Name);
			}
			this.SetValue(GameInfoInt.CurrentServerTime, (int)_time);
			this.SetValue(GameInfoInt.CurrentPlayers, _players);
			this.SetValue(GameInfoInt.FreePlayerSlots, this.GetValue(GameInfoInt.MaxPlayers) - _players);
			Action<GameServerInfo> onChangedAny = this.OnChangedAny;
			if (onChangedAny == null)
			{
				return;
			}
			onChangedAny(this);
		}
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x00178728 File Offset: 0x00176928
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshServerDisplayTexts(GameServerInfo gsi)
	{
		string value = gsi.GetValue(GameInfoString.CombinedPrimaryId);
		PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
		if (!string.IsNullOrEmpty(value))
		{
			platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromCombinedString(value, false);
		}
		string value2 = gsi.GetValue(GameInfoString.GameHost);
		string value3 = gsi.GetValue(GameInfoString.LevelName);
		string value4 = gsi.GetValue(GameInfoString.ServerDescription);
		string value5 = gsi.GetValue(GameInfoString.ServerWebsiteURL);
		string value6 = gsi.GetValue(GameInfoString.ServerLoginConfirmationText);
		if (!string.IsNullOrEmpty(value2) && string.IsNullOrEmpty(this.ServerDisplayName.Text))
		{
			this.ServerDisplayName.Update(value2, platformUserIdentifierAbs);
		}
		if (!string.IsNullOrEmpty(value3) && (string.IsNullOrEmpty(this.ServerWorldName.Text) || this.ServerWorldName.Text != value3))
		{
			this.ServerWorldName.Update(value3, platformUserIdentifierAbs);
		}
		if (!string.IsNullOrEmpty(value4) && string.IsNullOrEmpty(this.ServerDescription.Text))
		{
			this.ServerDescription.Update(value4.Replace("\\n", "\n"), platformUserIdentifierAbs);
		}
		if (!string.IsNullOrEmpty(value5) && string.IsNullOrEmpty(this.ServerURL.Text))
		{
			this.ServerURL.Update(value5, platformUserIdentifierAbs);
		}
		if (!string.IsNullOrEmpty(value6) && string.IsNullOrEmpty(this.ServerLoginConfirmationText.Text))
		{
			this.ServerLoginConfirmationText.Update(value6, platformUserIdentifierAbs);
		}
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x00178867 File Offset: 0x00176A67
	public void ClearOnChanged()
	{
		this.OnChangedAny = null;
		this.OnChangedString = null;
		this.OnChangedInt = null;
		this.OnChangedBool = null;
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x00178885 File Offset: 0x00176A85
	public static bool IsSearchable(GameInfoString _gameInfoKey)
	{
		return GameServerInfo.SearchableStringInfosSet.Contains(_gameInfoKey);
	}

	// Token: 0x0600397E RID: 14718 RVA: 0x00178892 File Offset: 0x00176A92
	public static bool IsSearchable(GameInfoInt _gameInfoKey)
	{
		return GameServerInfo.IntInfosInGameTagsSet.Contains(_gameInfoKey);
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x0017889F File Offset: 0x00176A9F
	public static bool IsSearchable(GameInfoBool _gameInfoKey)
	{
		return GameServerInfo.BoolInfosInGameTagsSet.Contains(_gameInfoKey);
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x001788AC File Offset: 0x00176AAC
	public static GameServerInfo BuildGameServerInfo()
	{
		GameServerInfo gameServerInfo = new GameServerInfo();
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.ServerEnabled);
		SandboxOptionManager sandboxOptionManager = SandboxOptionManager.Current;
		if (GameManager.IsDedicatedServer && GamePrefs.GetBool(EnumGamePrefs.ServerAllowCrossplay))
		{
			bool flag = true;
			if (8 < GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount))
			{
				Log.Warning(string.Format("CROSSPLAY INCOMPATIBLE VALUE: PLAYER COUNT GREATER THAN MAX OF {0}", 8));
				flag = false;
			}
			if (GamePrefs.GetBool(EnumGamePrefs.IgnoreEOSSanctions))
			{
				Log.Warning("CROSSPLAY INCOMPATIBLE VALUE: EOS SANCTIONS IGNORED");
				flag = false;
			}
			if (!flag)
			{
				Log.Warning("CROSSPLAY DISABLED FOR SESSION, CORRECT VALUES TO BE CROSSPLAY COMPATIBLE");
				GamePrefs.Set(EnumGamePrefs.ServerAllowCrossplay, false);
			}
		}
		gameServerInfo.SetValue(GameInfoString.GameType, "7DTD");
		gameServerInfo.SetValue(GameInfoString.GameName, GamePrefs.GetString(EnumGamePrefs.GameName));
		gameServerInfo.SetValue(GameInfoString.GameMode, GamePrefs.GetString(EnumGamePrefs.GameMode).Replace("GameMode", ""));
		gameServerInfo.SetValue(GameInfoString.GameHost, GameManager.IsDedicatedServer ? GamePrefs.GetString(EnumGamePrefs.ServerName) : GamePrefs.GetString(EnumGamePrefs.PlayerName));
		GameInfoString gameInfoString = GameInfoString.LevelName;
		PrefabEditModeManager instance = PrefabEditModeManager.Instance;
		gameServerInfo.SetValue(gameInfoString, (instance != null && instance.IsActive()) ? PrefabEditModeManager.Instance.LoadedPrefab.Name : GamePrefs.GetString(EnumGamePrefs.GameWorld));
		gameServerInfo.SetValue(GameInfoString.ServerDescription, GamePrefs.GetString(EnumGamePrefs.ServerDescription));
		gameServerInfo.SetValue(GameInfoString.ServerWebsiteURL, GamePrefs.GetString(EnumGamePrefs.ServerWebsiteURL));
		gameServerInfo.SetValue(GameInfoString.ServerLoginConfirmationText, GamePrefs.GetString(EnumGamePrefs.ServerLoginConfirmationText));
		gameServerInfo.SetValue(GameInfoBool.IsDedicated, GameManager.IsDedicatedServer);
		gameServerInfo.SetValue(GameInfoBool.IsPasswordProtected, !string.IsNullOrEmpty(GamePrefs.GetString(EnumGamePrefs.ServerPassword)));
		bool flag2 = (GameManager.IsDedicatedServer ? GamePrefs.GetBool(EnumGamePrefs.EACEnabled) : GamePrefs.GetBool(EnumGamePrefs.ServerEACPeerToPeer));
		gameServerInfo.SetValue(GameInfoBool.EACEnabled, flag2);
		gameServerInfo.SetValue(GameInfoBool.SanctionsIgnored, GameManager.IsDedicatedServer && GamePrefs.GetBool(EnumGamePrefs.IgnoreEOSSanctions));
		gameServerInfo.SetValue(GameInfoBool.AllowCrossplay, @bool && GamePrefs.GetBool(EnumGamePrefs.ServerAllowCrossplay) && PermissionsManager.IsCrossplayAllowed());
		gameServerInfo.SetValue(GameInfoString.PlayGroup, EPlayGroupExtensions.Current.ToStringCached<EPlayGroup>());
		gameServerInfo.SetValue(GameInfoInt.MaxPlayers, GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount));
		gameServerInfo.SetValue(GameInfoInt.FreePlayerSlots, GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount) - (GameManager.IsDedicatedServer ? 0 : 1));
		gameServerInfo.SetValue(GameInfoInt.CurrentPlayers, GameManager.IsDedicatedServer ? 0 : 1);
		gameServerInfo.SetValue(GameInfoInt.Port, GamePrefs.GetInt(EnumGamePrefs.ServerPort));
		gameServerInfo.SetValue(GameInfoString.ServerVersion, Constants.cVersionInformation.SerializableString);
		gameServerInfo.SetValue(GameInfoBool.Architecture64, !Constants.Is32BitOs);
		gameServerInfo.SetValue(GameInfoString.Platform, Application.platform.ToString());
		gameServerInfo.SetValue(GameInfoBool.IsPublic, GamePrefs.GetBool(EnumGamePrefs.ServerIsPublic));
		gameServerInfo.SetValue(GameInfoInt.ServerVisibility, PermissionsManager.IsMultiplayerAllowed() ? GamePrefs.GetInt(EnumGamePrefs.ServerVisibility) : 0);
		SandboxOptionPreset preset = SandboxOptionManager.Current.GetPreset(GamePrefs.GetString(EnumGamePrefs.SandboxPreset));
		gameServerInfo.SetValue(GameInfoBool.StockSettings, preset != null && !preset.IsCustomPreset && !preset.IsModded && !preset.IsUserPreset);
		bool flag3 = StockFileHashes.HasStockXMLs();
		gameServerInfo.SetValue(GameInfoBool.StockFiles, flag3);
		gameServerInfo.SetValue(GameInfoBool.ModdedConfig, !flag3 || ModManager.AnyConfigModActive());
		gameServerInfo.SetValue(GameInfoString.Region, GamePrefs.GetString(EnumGamePrefs.Region));
		gameServerInfo.SetValue(GameInfoString.Language, GameManager.IsDedicatedServer ? GamePrefs.GetString(EnumGamePrefs.Language) : Localization.ActiveLanguage);
		gameServerInfo.SetValue(GameInfoString.SandboxPreset, GamePrefs.GetString(EnumGamePrefs.SandboxPreset));
		gameServerInfo.SetValue(GameInfoString.SandboxCode, GamePrefs.GetString(EnumGamePrefs.SandboxCode));
		gameServerInfo.SetValue(GameInfoInt.BlockDamagePlayer, GamePrefs.GetInt(EnumGamePrefs.BlockDamagePlayer));
		gameServerInfo.SetValue(GameInfoInt.BlockDamageAI, GamePrefs.GetInt(EnumGamePrefs.BlockDamageAI));
		gameServerInfo.SetValue(GameInfoInt.BlockDamageAIBM, GamePrefs.GetInt(EnumGamePrefs.BlockDamageAIBM));
		gameServerInfo.SetValue(GameInfoInt.XPMultiplier, GamePrefs.GetInt(EnumGamePrefs.XPMultiplier));
		gameServerInfo.SetValue(GameInfoBool.BuildCreate, GamePrefs.GetBool(EnumGamePrefs.BuildCreate));
		gameServerInfo.SetValue(GameInfoInt.DayNightLength, GamePrefs.GetInt(EnumGamePrefs.DayNightLength));
		gameServerInfo.SetValue(GameInfoInt.DayLightLength, GamePrefs.GetInt(EnumGamePrefs.DayLightLength));
		gameServerInfo.SetValue(GameInfoInt.DeathPenalty, GamePrefs.GetInt(EnumGamePrefs.DeathPenalty));
		gameServerInfo.SetValue(GameInfoInt.DropOnDeath, GamePrefs.GetInt(EnumGamePrefs.DropOnDeath));
		gameServerInfo.SetValue(GameInfoInt.DropOnQuit, GamePrefs.GetInt(EnumGamePrefs.DropOnQuit));
		gameServerInfo.SetValue(GameInfoInt.BedrollDeadZoneSize, GamePrefs.GetInt(EnumGamePrefs.BedrollDeadZoneSize));
		gameServerInfo.SetValue(GameInfoInt.BedrollExpiryTime, GamePrefs.GetInt(EnumGamePrefs.BedrollExpiryTime));
		gameServerInfo.SetValue(GameInfoInt.MaxSpawnedZombies, GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies));
		gameServerInfo.SetValue(GameInfoInt.MaxSpawnedAnimals, GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedAnimals));
		gameServerInfo.SetValue(GameInfoBool.EnemySpawnMode, GamePrefs.GetBool(EnumGamePrefs.EnemySpawnMode));
		gameServerInfo.SetValue(GameInfoInt.EnemyDifficulty, GamePrefs.GetInt(EnumGamePrefs.EnemyDifficulty));
		gameServerInfo.SetValue(GameInfoInt.ZombieFeralSense, GamePrefs.GetInt(EnumGamePrefs.ZombieFeralSense));
		gameServerInfo.SetValue(GameInfoInt.ZombieMove, GamePrefs.GetInt(EnumGamePrefs.ZombieMove));
		gameServerInfo.SetValue(GameInfoInt.ZombieMoveNight, GamePrefs.GetInt(EnumGamePrefs.ZombieMoveNight));
		gameServerInfo.SetValue(GameInfoInt.ZombieFeralMove, GamePrefs.GetInt(EnumGamePrefs.ZombieFeralMove));
		gameServerInfo.SetValue(GameInfoInt.ZombieBMMove, GamePrefs.GetInt(EnumGamePrefs.ZombieBMMove));
		gameServerInfo.SetValue(GameInfoInt.AISmellMode, GamePrefs.GetInt(EnumGamePrefs.AISmellMode));
		gameServerInfo.SetValue(GameInfoInt.BloodMoonFrequency, GamePrefs.GetInt(EnumGamePrefs.BloodMoonFrequency));
		gameServerInfo.SetValue(GameInfoInt.BloodMoonRange, GamePrefs.GetInt(EnumGamePrefs.BloodMoonRange));
		gameServerInfo.SetValue(GameInfoInt.BloodMoonWarning, GamePrefs.GetInt(EnumGamePrefs.BloodMoonWarning));
		gameServerInfo.SetValue(GameInfoInt.BloodMoonEnemyCount, GamePrefs.GetInt(EnumGamePrefs.BloodMoonEnemyCount));
		gameServerInfo.SetValue(GameInfoInt.LootAbundance, GamePrefs.GetInt(EnumGamePrefs.LootAbundance));
		int num = GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays);
		if (num == 0)
		{
			num = -1;
		}
		gameServerInfo.SetValue(GameInfoInt.LootRespawnDays, num);
		gameServerInfo.SetValue(GameInfoInt.AirDropFrequency, GamePrefs.GetInt(EnumGamePrefs.AirDropFrequency));
		gameServerInfo.SetValue(GameInfoBool.AirDropMarker, GamePrefs.GetBool(EnumGamePrefs.AirDropMarker));
		gameServerInfo.SetValue(GameInfoInt.PartySharedKillRange, GamePrefs.GetInt(EnumGamePrefs.PartySharedKillRange));
		gameServerInfo.SetValue(GameInfoInt.PlayerKillingMode, GamePrefs.GetInt(EnumGamePrefs.PlayerKillingMode));
		gameServerInfo.SetValue(GameInfoInt.LandClaimCount, GamePrefs.GetInt(EnumGamePrefs.LandClaimCount));
		gameServerInfo.SetValue(GameInfoInt.LandClaimSize, GamePrefs.GetInt(EnumGamePrefs.LandClaimSize));
		gameServerInfo.SetValue(GameInfoInt.LandClaimDeadZone, GamePrefs.GetInt(EnumGamePrefs.LandClaimDeadZone));
		gameServerInfo.SetValue(GameInfoInt.LandClaimExpiryTime, GamePrefs.GetInt(EnumGamePrefs.LandClaimExpiryTime));
		gameServerInfo.SetValue(GameInfoInt.LandClaimDecayMode, GamePrefs.GetInt(EnumGamePrefs.LandClaimDecayMode));
		gameServerInfo.SetValue(GameInfoInt.LandClaimOnlineDurabilityModifier, GamePrefs.GetInt(EnumGamePrefs.LandClaimOnlineDurabilityModifier));
		gameServerInfo.SetValue(GameInfoInt.LandClaimOfflineDurabilityModifier, GamePrefs.GetInt(EnumGamePrefs.LandClaimOfflineDurabilityModifier));
		gameServerInfo.SetValue(GameInfoInt.LandClaimOfflineDelay, GamePrefs.GetInt(EnumGamePrefs.LandClaimOfflineDelay));
		gameServerInfo.SetValue(GameInfoInt.MaxChunkAge, GamePrefs.GetInt(EnumGamePrefs.MaxChunkAge));
		gameServerInfo.SetValue(GameInfoBool.ShowFriendPlayerOnMap, GamePrefs.GetBool(EnumGamePrefs.ShowFriendPlayerOnMap));
		gameServerInfo.SetValue(GameInfoInt.DayCount, GamePrefs.GetInt(EnumGamePrefs.DayCount));
		gameServerInfo.SetValue(GameInfoBool.AllowSpawnNearBackpack, GamePrefs.GetBool(EnumGamePrefs.AllowSpawnNearBackpack));
		gameServerInfo.SetValue(GameInfoInt.AllowSpawnNearFriend, GamePrefs.GetInt(EnumGamePrefs.AllowSpawnNearFriend));
		gameServerInfo.SetValue(GameInfoInt.QuestProgressionDailyLimit, GamePrefs.GetInt(EnumGamePrefs.QuestProgressionDailyLimit));
		gameServerInfo.SetValue(GameInfoBool.BiomeProgression, GamePrefs.GetBool(EnumGamePrefs.BiomeProgression));
		gameServerInfo.SetValue(GameInfoInt.StormFreq, GamePrefs.GetInt(EnumGamePrefs.StormFreq));
		gameServerInfo.SetValue(GameInfoInt.JarRefund, GamePrefs.GetInt(EnumGamePrefs.JarRefund));
		return gameServerInfo;
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x00178EDD File Offset: 0x001770DD
	public static void PrepareLocalServerInfo()
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo = GameServerInfo.BuildGameServerInfo();
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x00178EF0 File Offset: 0x001770F0
	public static void SetLocalServerWorldInfo()
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoInt.CurrentServerTime, (int)GameManager.Instance.World.worldTime);
		Vector3i vector3i;
		Vector3i vector3i2;
		GameManager.Instance.World.GetWorldExtent(out vector3i, out vector3i2);
		Vector3i vector3i3 = vector3i2 - vector3i;
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoInt.WorldSize, vector3i3.x);
	}

	// Token: 0x04002EEC RID: 12012
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	// Token: 0x04002EED RID: 12013
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoString, string> tableStrings = new EnumDictionary<GameInfoString, string>();

	// Token: 0x04002EEE RID: 12014
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoInt, int> tableInts = new EnumDictionary<GameInfoInt, int>();

	// Token: 0x04002EEF RID: 12015
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoBool, bool> tableBools = new EnumDictionary<GameInfoBool, bool>();

	// Token: 0x04002EF0 RID: 12016
	public readonly ReadOnlyDictionary<GameInfoString, string> Strings;

	// Token: 0x04002EF1 RID: 12017
	public readonly ReadOnlyDictionary<GameInfoInt, int> Ints;

	// Token: 0x04002EF2 RID: 12018
	public readonly ReadOnlyDictionary<GameInfoBool, bool> Bools;

	// Token: 0x04002EF3 RID: 12019
	public AuthoredText ServerDisplayName = new AuthoredText();

	// Token: 0x04002EF4 RID: 12020
	public AuthoredText ServerWorldName = new AuthoredText();

	// Token: 0x04002EF5 RID: 12021
	public AuthoredText ServerDescription = new AuthoredText();

	// Token: 0x04002EF6 RID: 12022
	public AuthoredText ServerURL = new AuthoredText();

	// Token: 0x04002EF7 RID: 12023
	public AuthoredText ServerLoginConfirmationText = new AuthoredText();

	// Token: 0x04002EF8 RID: 12024
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBroken;

	// Token: 0x04002EF9 RID: 12025
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isFriends;

	// Token: 0x04002EFA RID: 12026
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isFavorite;

	// Token: 0x04002EFB RID: 12027
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPlayed;

	// Token: 0x04002EFC RID: 12028
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLan;

	// Token: 0x04002EFD RID: 12029
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLobby;

	// Token: 0x04002EFE RID: 12030
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNoResponse;

	// Token: 0x04002EFF RID: 12031
	[PublicizedFrom(EAccessModifier.Private)]
	public long hashcode = long.MinValue;

	// Token: 0x04002F00 RID: 12032
	[PublicizedFrom(EAccessModifier.Private)]
	public VersionInformation version = new VersionInformation(VersionInformation.EGameReleaseType.Alpha, -1, -1, -1);

	// Token: 0x04002F05 RID: 12037
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLastWorldTimeUpdate;

	// Token: 0x04002F06 RID: 12038
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] whiteSpaceChars = new char[] { ' ', '\r', '\n', '\t' };

	// Token: 0x04002F07 RID: 12039
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedToString;

	// Token: 0x04002F08 RID: 12040
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedToStringLineBreaks;

	// Token: 0x04002F09 RID: 12041
	public static readonly GameInfoString[] SearchableStringInfos = new GameInfoString[]
	{
		GameInfoString.LevelName,
		GameInfoString.GameHost,
		GameInfoString.SteamID,
		GameInfoString.Region,
		GameInfoString.Language,
		GameInfoString.UniqueId,
		GameInfoString.CombinedNativeId,
		GameInfoString.ServerVersion,
		GameInfoString.PlayGroup,
		GameInfoString.SandboxCode
	};

	// Token: 0x04002F0A RID: 12042
	public static readonly GameInfoInt[] IntInfosInGameTags = new GameInfoInt[]
	{
		GameInfoInt.DayNightLength,
		GameInfoInt.DeathPenalty,
		GameInfoInt.DropOnDeath,
		GameInfoInt.DropOnQuit,
		GameInfoInt.BloodMoonEnemyCount,
		GameInfoInt.EnemyDifficulty,
		GameInfoInt.PlayerKillingMode,
		GameInfoInt.CurrentServerTime,
		GameInfoInt.DayLightLength,
		GameInfoInt.AirDropFrequency,
		GameInfoInt.LootAbundance,
		GameInfoInt.LootRespawnDays,
		GameInfoInt.MaxSpawnedZombies,
		GameInfoInt.LandClaimCount,
		GameInfoInt.LandClaimSize,
		GameInfoInt.LandClaimExpiryTime,
		GameInfoInt.LandClaimDecayMode,
		GameInfoInt.LandClaimOnlineDurabilityModifier,
		GameInfoInt.LandClaimOfflineDurabilityModifier,
		GameInfoInt.MaxSpawnedAnimals,
		GameInfoInt.PartySharedKillRange,
		GameInfoInt.ZombieFeralSense,
		GameInfoInt.ZombieMove,
		GameInfoInt.ZombieMoveNight,
		GameInfoInt.ZombieFeralMove,
		GameInfoInt.ZombieBMMove,
		GameInfoInt.AISmellMode,
		GameInfoInt.XPMultiplier,
		GameInfoInt.BlockDamagePlayer,
		GameInfoInt.BlockDamageAI,
		GameInfoInt.BlockDamageAIBM,
		GameInfoInt.BloodMoonFrequency,
		GameInfoInt.BloodMoonRange,
		GameInfoInt.BloodMoonWarning,
		GameInfoInt.BedrollExpiryTime,
		GameInfoInt.LandClaimOfflineDelay,
		GameInfoInt.Port,
		GameInfoInt.FreePlayerSlots,
		GameInfoInt.CurrentPlayers,
		GameInfoInt.MaxPlayers,
		GameInfoInt.WorldSize,
		GameInfoInt.MaxChunkAge,
		GameInfoInt.QuestProgressionDailyLimit,
		GameInfoInt.StormFreq,
		GameInfoInt.JarRefund
	};

	// Token: 0x04002F0B RID: 12043
	public static readonly GameInfoBool[] BoolInfosInGameTags = new GameInfoBool[]
	{
		GameInfoBool.IsDedicated,
		GameInfoBool.ShowFriendPlayerOnMap,
		GameInfoBool.BuildCreate,
		GameInfoBool.StockSettings,
		GameInfoBool.ModdedConfig,
		GameInfoBool.RequiresMod,
		GameInfoBool.AirDropMarker,
		GameInfoBool.EnemySpawnMode,
		GameInfoBool.IsPasswordProtected,
		GameInfoBool.AllowCrossplay,
		GameInfoBool.EACEnabled,
		GameInfoBool.SanctionsIgnored,
		GameInfoBool.BiomeProgression
	};

	// Token: 0x04002F0C RID: 12044
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoString> SearchableStringInfosSet = new HashSet<GameInfoString>(GameServerInfo.SearchableStringInfos);

	// Token: 0x04002F0D RID: 12045
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoInt> IntInfosInGameTagsSet = new HashSet<GameInfoInt>(GameServerInfo.IntInfosInGameTags);

	// Token: 0x04002F0E RID: 12046
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoBool> BoolInfosInGameTagsSet = new HashSet<GameInfoBool>(GameServerInfo.BoolInfosInGameTags);

	// Token: 0x02000783 RID: 1923
	public class UniqueIdEqualityComparer : IEqualityComparer<GameServerInfo>
	{
		// Token: 0x06003984 RID: 14724 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public UniqueIdEqualityComparer()
		{
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00178FFF File Offset: 0x001771FF
		public bool Equals(GameServerInfo _x, GameServerInfo _y)
		{
			return _x == _y || (_x != null && _y != null && _x.GetValue(GameInfoString.UniqueId) == _y.GetValue(GameInfoString.UniqueId));
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x00179024 File Offset: 0x00177224
		public int GetHashCode(GameServerInfo _obj)
		{
			return _obj.GetValue(GameInfoString.UniqueId).GetHashCode();
		}

		// Token: 0x04002F0F RID: 12047
		public static readonly GameServerInfo.UniqueIdEqualityComparer Instance = new GameServerInfo.UniqueIdEqualityComparer();
	}
}
