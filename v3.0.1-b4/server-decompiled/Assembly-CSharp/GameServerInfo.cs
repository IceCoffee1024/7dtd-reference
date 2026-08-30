using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Platform;
using SandboxOptions;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class GameServerInfo
{
	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06003894 RID: 14484 RVA: 0x00173DF8 File Offset: 0x00171FF8
	// (remove) Token: 0x06003895 RID: 14485 RVA: 0x00173E30 File Offset: 0x00172030
	public event Action<GameServerInfo> OnChangedAny;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06003896 RID: 14486 RVA: 0x00173E68 File Offset: 0x00172068
	// (remove) Token: 0x06003897 RID: 14487 RVA: 0x00173EA0 File Offset: 0x001720A0
	public event Action<GameServerInfo, GameInfoString> OnChangedString;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06003898 RID: 14488 RVA: 0x00173ED8 File Offset: 0x001720D8
	// (remove) Token: 0x06003899 RID: 14489 RVA: 0x00173F10 File Offset: 0x00172110
	public event Action<GameServerInfo, GameInfoInt> OnChangedInt;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x0600389A RID: 14490 RVA: 0x00173F48 File Offset: 0x00172148
	// (remove) Token: 0x0600389B RID: 14491 RVA: 0x00173F80 File Offset: 0x00172180
	public event Action<GameServerInfo, GameInfoBool> OnChangedBool;

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x0600389C RID: 14492 RVA: 0x00173FB5 File Offset: 0x001721B5
	public bool IsValid
	{
		get
		{
			return !this.isBroken;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x0600389D RID: 14493 RVA: 0x00173FC0 File Offset: 0x001721C0
	public bool IsDedicated
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated);
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x0600389E RID: 14494 RVA: 0x00173FC9 File Offset: 0x001721C9
	public bool IsDedicatedStock
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated) && this.GetValue(GameInfoBool.StockSettings) && !this.GetValue(GameInfoBool.ModdedConfig);
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x0600389F RID: 14495 RVA: 0x00173FEA File Offset: 0x001721EA
	public bool IsDedicatedModded
	{
		get
		{
			return this.GetValue(GameInfoBool.IsDedicated) && this.GetValue(GameInfoBool.ModdedConfig);
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x060038A0 RID: 14496 RVA: 0x00173FFF File Offset: 0x001721FF
	public bool IsPeerToPeer
	{
		get
		{
			return !this.GetValue(GameInfoBool.IsDedicated) && !this.isNoResponse;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x060038A1 RID: 14497 RVA: 0x00174015 File Offset: 0x00172215
	public bool AllowsCrossplay
	{
		get
		{
			return this.GetValue(GameInfoBool.AllowCrossplay);
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x060038A2 RID: 14498 RVA: 0x0017401F File Offset: 0x0017221F
	public bool EACEnabled
	{
		get
		{
			return this.GetValue(GameInfoBool.EACEnabled);
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x060038A3 RID: 14499 RVA: 0x00174028 File Offset: 0x00172228
	public bool IgnoresSanctions
	{
		get
		{
			return this.GetValue(GameInfoBool.SanctionsIgnored);
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060038A4 RID: 14500 RVA: 0x00174034 File Offset: 0x00172234
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

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060038A5 RID: 14501 RVA: 0x00174056 File Offset: 0x00172256
	// (set) Token: 0x060038A6 RID: 14502 RVA: 0x0017405E File Offset: 0x0017225E
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

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060038A7 RID: 14503 RVA: 0x00174081 File Offset: 0x00172281
	public bool IsFavoriteHistory
	{
		get
		{
			return this.IsFavorite || this.IsHistory;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060038A8 RID: 14504 RVA: 0x00174093 File Offset: 0x00172293
	// (set) Token: 0x060038A9 RID: 14505 RVA: 0x0017409B File Offset: 0x0017229B
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

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x060038AA RID: 14506 RVA: 0x001740BE File Offset: 0x001722BE
	public bool IsHistory
	{
		get
		{
			return this.lastPlayed > 0;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x060038AB RID: 14507 RVA: 0x001740C9 File Offset: 0x001722C9
	// (set) Token: 0x060038AC RID: 14508 RVA: 0x001740D1 File Offset: 0x001722D1
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

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x060038AD RID: 14509 RVA: 0x001740F4 File Offset: 0x001722F4
	// (set) Token: 0x060038AE RID: 14510 RVA: 0x001740FC File Offset: 0x001722FC
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

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x060038AF RID: 14511 RVA: 0x0017411F File Offset: 0x0017231F
	// (set) Token: 0x060038B0 RID: 14512 RVA: 0x00174127 File Offset: 0x00172327
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

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x060038B1 RID: 14513 RVA: 0x0017414A File Offset: 0x0017234A
	// (set) Token: 0x060038B2 RID: 14514 RVA: 0x00174152 File Offset: 0x00172352
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

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060038B3 RID: 14515 RVA: 0x00174175 File Offset: 0x00172375
	public VersionInformation Version
	{
		get
		{
			return this.version;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x060038B4 RID: 14516 RVA: 0x0017417D File Offset: 0x0017237D
	public bool IsCompatibleVersion
	{
		get
		{
			return this.version.Major < 0 || this.version.EqualsMinor(Constants.cVersionInformation);
		}
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x001741A0 File Offset: 0x001723A0
	public GameServerInfo()
	{
		this.OnChangedAny += this.RefreshServerDisplayTexts;
		this.Strings = new ReadOnlyDictionary<GameInfoString, string>(this.tableStrings);
		this.Ints = new ReadOnlyDictionary<GameInfoInt, int>(this.tableInts);
		this.Bools = new ReadOnlyDictionary<GameInfoBool, bool>(this.tableBools);
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x00174270 File Offset: 0x00172470
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

	// Token: 0x060038B7 RID: 14519 RVA: 0x0017445C File Offset: 0x0017265C
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

	// Token: 0x060038B8 RID: 14520 RVA: 0x00174510 File Offset: 0x00172710
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

	// Token: 0x060038B9 RID: 14521 RVA: 0x00174670 File Offset: 0x00172870
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

	// Token: 0x060038BA RID: 14522 RVA: 0x001746F8 File Offset: 0x001728F8
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

	// Token: 0x060038BB RID: 14523 RVA: 0x001747BC File Offset: 0x001729BC
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

	// Token: 0x060038BC RID: 14524 RVA: 0x001747E4 File Offset: 0x001729E4
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

	// Token: 0x060038BD RID: 14525 RVA: 0x0017480C File Offset: 0x00172A0C
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

	// Token: 0x060038BE RID: 14526 RVA: 0x00174834 File Offset: 0x00172A34
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

	// Token: 0x060038BF RID: 14527 RVA: 0x001748EC File Offset: 0x00172AEC
	public string GetValue(GameInfoString _key)
	{
		string text;
		if (!this.tableStrings.TryGetValue(_key, out text))
		{
			return "";
		}
		return text.Replace('^', ':').Replace('*', ';');
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00174924 File Offset: 0x00172B24
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

	// Token: 0x060038C1 RID: 14529 RVA: 0x0017499C File Offset: 0x00172B9C
	public int GetValue(GameInfoInt _key)
	{
		int num;
		if (!this.tableInts.TryGetValue(_key, out num))
		{
			return -1;
		}
		return num;
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x001749BC File Offset: 0x00172BBC
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

	// Token: 0x060038C3 RID: 14531 RVA: 0x00174A08 File Offset: 0x00172C08
	public bool GetValue(GameInfoBool _key)
	{
		bool flag;
		return this.tableBools.TryGetValue(_key, out flag) && flag;
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x00174A25 File Offset: 0x00172C25
	public override string ToString()
	{
		return this.ToString(false);
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x00174A30 File Offset: 0x00172C30
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

	// Token: 0x060038C6 RID: 14534 RVA: 0x00174C30 File Offset: 0x00172E30
	public override int GetHashCode()
	{
		if (this.hashcode == -9223372036854775808L)
		{
			string text = this.GetValue(GameInfoString.IP) + this.GetValue(GameInfoInt.Port).ToString();
			this.hashcode = (long)text.GetHashCode();
		}
		return (int)this.hashcode;
	}

	// Token: 0x060038C7 RID: 14535 RVA: 0x00174C80 File Offset: 0x00172E80
	public override bool Equals(object _obj)
	{
		if (_obj == null)
		{
			return false;
		}
		GameServerInfo gameServerInfo = _obj as GameServerInfo;
		return this.Equals(gameServerInfo);
	}

	// Token: 0x060038C8 RID: 14536 RVA: 0x00174CA0 File Offset: 0x00172EA0
	public bool Equals(GameServerInfo _p)
	{
		return _p != null && this.GetValue(GameInfoString.IP) == _p.GetValue(GameInfoString.IP) && this.GetValue(GameInfoInt.Port) == _p.GetValue(GameInfoInt.Port);
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x00174CD0 File Offset: 0x00172ED0
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

	// Token: 0x060038CA RID: 14538 RVA: 0x00174D58 File Offset: 0x00172F58
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

	// Token: 0x060038CB RID: 14539 RVA: 0x00174E97 File Offset: 0x00173097
	public void ClearOnChanged()
	{
		this.OnChangedAny = null;
		this.OnChangedString = null;
		this.OnChangedInt = null;
		this.OnChangedBool = null;
	}

	// Token: 0x060038CC RID: 14540 RVA: 0x00174EB5 File Offset: 0x001730B5
	public static bool IsSearchable(GameInfoString _gameInfoKey)
	{
		return GameServerInfo.SearchableStringInfosSet.Contains(_gameInfoKey);
	}

	// Token: 0x060038CD RID: 14541 RVA: 0x00174EC2 File Offset: 0x001730C2
	public static bool IsSearchable(GameInfoInt _gameInfoKey)
	{
		return GameServerInfo.IntInfosInGameTagsSet.Contains(_gameInfoKey);
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x00174ECF File Offset: 0x001730CF
	public static bool IsSearchable(GameInfoBool _gameInfoKey)
	{
		return GameServerInfo.BoolInfosInGameTagsSet.Contains(_gameInfoKey);
	}

	// Token: 0x060038CF RID: 14543 RVA: 0x00174EDC File Offset: 0x001730DC
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

	// Token: 0x060038D0 RID: 14544 RVA: 0x0017550D File Offset: 0x0017370D
	public static void PrepareLocalServerInfo()
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo = GameServerInfo.BuildGameServerInfo();
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x00175520 File Offset: 0x00173720
	public static void SetLocalServerWorldInfo()
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoInt.CurrentServerTime, (int)GameManager.Instance.World.worldTime);
		Vector3i vector3i;
		Vector3i vector3i2;
		GameManager.Instance.World.GetWorldExtent(out vector3i, out vector3i2);
		Vector3i vector3i3 = vector3i2 - vector3i;
		SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoInt.WorldSize, vector3i3.x);
	}

	// Token: 0x04002E5D RID: 11869
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	// Token: 0x04002E5E RID: 11870
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoString, string> tableStrings = new EnumDictionary<GameInfoString, string>();

	// Token: 0x04002E5F RID: 11871
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoInt, int> tableInts = new EnumDictionary<GameInfoInt, int>();

	// Token: 0x04002E60 RID: 11872
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GameInfoBool, bool> tableBools = new EnumDictionary<GameInfoBool, bool>();

	// Token: 0x04002E61 RID: 11873
	public readonly ReadOnlyDictionary<GameInfoString, string> Strings;

	// Token: 0x04002E62 RID: 11874
	public readonly ReadOnlyDictionary<GameInfoInt, int> Ints;

	// Token: 0x04002E63 RID: 11875
	public readonly ReadOnlyDictionary<GameInfoBool, bool> Bools;

	// Token: 0x04002E64 RID: 11876
	public AuthoredText ServerDisplayName = new AuthoredText();

	// Token: 0x04002E65 RID: 11877
	public AuthoredText ServerWorldName = new AuthoredText();

	// Token: 0x04002E66 RID: 11878
	public AuthoredText ServerDescription = new AuthoredText();

	// Token: 0x04002E67 RID: 11879
	public AuthoredText ServerURL = new AuthoredText();

	// Token: 0x04002E68 RID: 11880
	public AuthoredText ServerLoginConfirmationText = new AuthoredText();

	// Token: 0x04002E69 RID: 11881
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBroken;

	// Token: 0x04002E6A RID: 11882
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isFriends;

	// Token: 0x04002E6B RID: 11883
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isFavorite;

	// Token: 0x04002E6C RID: 11884
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPlayed;

	// Token: 0x04002E6D RID: 11885
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLan;

	// Token: 0x04002E6E RID: 11886
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLobby;

	// Token: 0x04002E6F RID: 11887
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNoResponse;

	// Token: 0x04002E70 RID: 11888
	[PublicizedFrom(EAccessModifier.Private)]
	public long hashcode = long.MinValue;

	// Token: 0x04002E71 RID: 11889
	[PublicizedFrom(EAccessModifier.Private)]
	public VersionInformation version = new VersionInformation(VersionInformation.EGameReleaseType.Alpha, -1, -1, -1);

	// Token: 0x04002E76 RID: 11894
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLastWorldTimeUpdate;

	// Token: 0x04002E77 RID: 11895
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] whiteSpaceChars = new char[] { ' ', '\r', '\n', '\t' };

	// Token: 0x04002E78 RID: 11896
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedToString;

	// Token: 0x04002E79 RID: 11897
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedToStringLineBreaks;

	// Token: 0x04002E7A RID: 11898
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

	// Token: 0x04002E7B RID: 11899
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

	// Token: 0x04002E7C RID: 11900
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

	// Token: 0x04002E7D RID: 11901
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoString> SearchableStringInfosSet = new HashSet<GameInfoString>(GameServerInfo.SearchableStringInfos);

	// Token: 0x04002E7E RID: 11902
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoInt> IntInfosInGameTagsSet = new HashSet<GameInfoInt>(GameServerInfo.IntInfosInGameTags);

	// Token: 0x04002E7F RID: 11903
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<GameInfoBool> BoolInfosInGameTagsSet = new HashSet<GameInfoBool>(GameServerInfo.BoolInfosInGameTags);

	// Token: 0x0200076F RID: 1903
	public class UniqueIdEqualityComparer : IEqualityComparer<GameServerInfo>
	{
		// Token: 0x060038D3 RID: 14547 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public UniqueIdEqualityComparer()
		{
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0017562F File Offset: 0x0017382F
		public bool Equals(GameServerInfo _x, GameServerInfo _y)
		{
			return _x == _y || (_x != null && _y != null && _x.GetValue(GameInfoString.UniqueId) == _y.GetValue(GameInfoString.UniqueId));
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00175654 File Offset: 0x00173854
		public int GetHashCode(GameServerInfo _obj)
		{
			return _obj.GetValue(GameInfoString.UniqueId).GetHashCode();
		}

		// Token: 0x04002E80 RID: 11904
		public static readonly GameServerInfo.UniqueIdEqualityComparer Instance = new GameServerInfo.UniqueIdEqualityComparer();
	}
}
