using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x0200089B RID: 2203
public class ServerInfoCache
{
	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x0600400B RID: 16395 RVA: 0x00190990 File Offset: 0x0018EB90
	public static ServerInfoCache Instance
	{
		get
		{
			ServerInfoCache serverInfoCache;
			if ((serverInfoCache = ServerInfoCache.instance) == null)
			{
				serverInfoCache = (ServerInfoCache.instance = new ServerInfoCache());
			}
			return serverInfoCache;
		}
	}

	// Token: 0x0600400C RID: 16396 RVA: 0x001909A8 File Offset: 0x0018EBA8
	[PublicizedFrom(EAccessModifier.Private)]
	public int serverInfoToPersistentKey(GameServerInfo _gsi)
	{
		return (_gsi.GetValue(GameInfoString.IP) + _gsi.GetValue(GameInfoInt.Port).ToString()).GetHashCode();
	}

	// Token: 0x0600400D RID: 16397 RVA: 0x001909D8 File Offset: 0x0018EBD8
	public void SavePassword(GameServerInfo _gsi, string _password)
	{
		Dictionary<int, string> passwordCacheList = this.GetPasswordCacheList();
		int num = this.serverInfoToPersistentKey(_gsi);
		passwordCacheList[num] = _password;
		GamePrefs.Set(EnumGamePrefs.ServerPasswordCache, this.DictToString<int, string>(passwordCacheList));
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x00190A0C File Offset: 0x0018EC0C
	public string GetPassword(GameServerInfo _gsi)
	{
		Dictionary<int, string> passwordCacheList = this.GetPasswordCacheList();
		int num = this.serverInfoToPersistentKey(_gsi);
		string text;
		if (!passwordCacheList.TryGetValue(num, out text))
		{
			return "";
		}
		return text;
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x00190A38 File Offset: 0x0018EC38
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, string> GetPasswordCacheList()
	{
		Dictionary<int, string> dictionary;
		try
		{
			dictionary = this.StringToDict<int, string>(GamePrefs.GetString(EnumGamePrefs.ServerPasswordCache), new Func<string, int>(Convert.ToInt32), (string _valueString) => _valueString);
		}
		catch (Exception)
		{
			GamePrefs.Set(EnumGamePrefs.ServerPasswordCache, "");
			dictionary = new Dictionary<int, string>();
		}
		return dictionary;
	}

	// Token: 0x06004010 RID: 16400 RVA: 0x00190AA8 File Offset: 0x0018ECA8
	public void AddHistory(GameServerInfo _info)
	{
		try
		{
			this.getFavHistoryCacheList();
			ServerInfoCache.FavoritesHistoryKey keyFromInfo = this.getKeyFromInfo(_info);
			ServerInfoCache.FavoritesHistoryValue favoritesHistoryValue;
			if (!this.favoritesHistoryCache.TryGetValue(keyFromInfo, out favoritesHistoryValue))
			{
				favoritesHistoryValue = new ServerInfoCache.FavoritesHistoryValue(0U, false);
				this.favoritesHistoryCache[keyFromInfo] = favoritesHistoryValue;
			}
			favoritesHistoryValue.LastPlayedTime = Utils.CurrentUnixTime;
			this.saveFavHistoryCacheList();
			Log.Out("[NET] Added server to history: " + keyFromInfo.Address);
		}
		catch (Exception ex)
		{
			Log.Error("Could not add server " + _info.GetValue(GameInfoString.IP) + " to history:");
			Log.Exception(ex);
		}
	}

	// Token: 0x06004011 RID: 16401 RVA: 0x00190B44 File Offset: 0x0018ED44
	public uint IsHistory(GameServerInfo _info)
	{
		uint num;
		try
		{
			this.getFavHistoryCacheList();
			ServerInfoCache.FavoritesHistoryKey keyFromInfo = this.getKeyFromInfo(_info);
			if (keyFromInfo == null)
			{
				Log.Warning("Could not check if server " + _info.GetValue(GameInfoString.IP) + " is in history: Invalid IP/port");
				num = 0U;
			}
			else
			{
				ServerInfoCache.FavoritesHistoryValue favoritesHistoryValue;
				num = ((!this.favoritesHistoryCache.TryGetValue(keyFromInfo, out favoritesHistoryValue)) ? 0U : favoritesHistoryValue.LastPlayedTime);
			}
		}
		catch (Exception ex)
		{
			Log.Error("Could not check if server " + _info.GetValue(GameInfoString.IP) + " is in history:");
			Log.Exception(ex);
			num = 0U;
		}
		return num;
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x00190BD4 File Offset: 0x0018EDD4
	public void ToggleFavorite(GameServerInfo _info)
	{
		try
		{
			this.getFavHistoryCacheList();
			ServerInfoCache.FavoritesHistoryKey keyFromInfo = this.getKeyFromInfo(_info);
			ServerInfoCache.FavoritesHistoryValue favoritesHistoryValue;
			if (!this.favoritesHistoryCache.TryGetValue(keyFromInfo, out favoritesHistoryValue))
			{
				favoritesHistoryValue = new ServerInfoCache.FavoritesHistoryValue(0U, false);
				this.favoritesHistoryCache[keyFromInfo] = favoritesHistoryValue;
			}
			_info.IsFavorite = (favoritesHistoryValue.IsFavorite = !favoritesHistoryValue.IsFavorite);
			if (!favoritesHistoryValue.IsFavorite && favoritesHistoryValue.LastPlayedTime == 0U)
			{
				this.favoritesHistoryCache.Remove(keyFromInfo);
			}
			this.saveFavHistoryCacheList();
			Log.Out(string.Format("[NET] Toggled server favorite: {0} - {1}", keyFromInfo.Address, favoritesHistoryValue.IsFavorite));
		}
		catch (Exception ex)
		{
			Log.Error("Could not toggle server " + _info.GetValue(GameInfoString.IP) + " favorite:");
			Log.Exception(ex);
		}
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x00190CA4 File Offset: 0x0018EEA4
	public bool IsFavorite(GameServerInfo _info)
	{
		bool flag;
		try
		{
			this.getFavHistoryCacheList();
			ServerInfoCache.FavoritesHistoryKey keyFromInfo = this.getKeyFromInfo(_info);
			if (keyFromInfo == null)
			{
				Log.Warning("Could not check if server " + _info.GetValue(GameInfoString.IP) + " is favorite: Invalid IP/port");
				flag = false;
			}
			else
			{
				ServerInfoCache.FavoritesHistoryValue favoritesHistoryValue;
				flag = this.favoritesHistoryCache.TryGetValue(keyFromInfo, out favoritesHistoryValue) && favoritesHistoryValue.IsFavorite;
			}
		}
		catch (Exception ex)
		{
			Log.Error("Could not check if server " + _info.GetValue(GameInfoString.IP) + " is favorite:");
			Log.Exception(ex);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06004014 RID: 16404 RVA: 0x00190D34 File Offset: 0x0018EF34
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>.Enumerator GetFavoriteServersEnumerator()
	{
		this.getFavHistoryCacheList();
		return this.favoritesHistoryCache.GetEnumerator();
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x00190D48 File Offset: 0x0018EF48
	[PublicizedFrom(EAccessModifier.Private)]
	public ServerInfoCache.FavoritesHistoryKey getKeyFromInfo(GameServerInfo _info)
	{
		string value = _info.GetValue(GameInfoString.IP);
		int value2 = _info.GetValue(GameInfoInt.Port);
		if (string.IsNullOrEmpty(value) || value2 < 1 || value2 > 65535)
		{
			return null;
		}
		return new ServerInfoCache.FavoritesHistoryKey(value, value2);
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x00190D84 File Offset: 0x0018EF84
	[PublicizedFrom(EAccessModifier.Private)]
	public void getFavHistoryCacheList()
	{
		if (this.favoritesHistoryCache != null)
		{
			return;
		}
		try
		{
			this.favoritesHistoryCache = this.StringToDict<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>(GamePrefs.GetString(EnumGamePrefs.ServerHistoryCache), new Func<string, ServerInfoCache.FavoritesHistoryKey>(ServerInfoCache.FavoritesHistoryKey.FromString), new Func<string, ServerInfoCache.FavoritesHistoryValue>(ServerInfoCache.FavoritesHistoryValue.FromString));
		}
		catch (Exception)
		{
			GamePrefs.Set(EnumGamePrefs.ServerHistoryCache, "");
			this.favoritesHistoryCache = new Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>();
		}
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x00190DF8 File Offset: 0x0018EFF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveFavHistoryCacheList()
	{
		GamePrefs.Set(EnumGamePrefs.ServerHistoryCache, this.DictToString<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>(this.favoritesHistoryCache));
	}

	// Token: 0x06004018 RID: 16408 RVA: 0x00190E10 File Offset: 0x0018F010
	[PublicizedFrom(EAccessModifier.Private)]
	public string escapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorString, ServerInfoCache.elementSeparatorEscaped).Replace(ServerInfoCache.fieldSeparatorString, ServerInfoCache.fieldSeparatorEscaped);
	}

	// Token: 0x06004019 RID: 16409 RVA: 0x00190E31 File Offset: 0x0018F031
	[PublicizedFrom(EAccessModifier.Private)]
	public string unescapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorEscaped, ServerInfoCache.elementSeparatorString).Replace(ServerInfoCache.fieldSeparatorEscaped, ServerInfoCache.fieldSeparatorString);
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x00190E54 File Offset: 0x0018F054
	[PublicizedFrom(EAccessModifier.Private)]
	public string DictToString<TKey, TValue>(Dictionary<TKey, TValue> _dictionary)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _dictionary)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(';');
			}
			StringBuilder stringBuilder2 = stringBuilder;
			TKey key = keyValuePair.Key;
			stringBuilder2.Append(this.escapeString(key.ToString()));
			stringBuilder.Append(':');
			StringBuilder stringBuilder3 = stringBuilder;
			TValue value = keyValuePair.Value;
			stringBuilder3.Append(this.escapeString(value.ToString()));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x00190F08 File Offset: 0x0018F108
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<TKey, TValue> StringToDict<TKey, TValue>(string _input, Func<string, TKey> _keyParser, Func<string, TValue> _valueParser)
	{
		if (string.IsNullOrEmpty(_input))
		{
			return new Dictionary<TKey, TValue>();
		}
		Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(Math.Max(_input.Length >> 5, 16));
		int i = 0;
		while (i < _input.Length)
		{
			int num = this.findNextSeparator(_input, ';', i);
			int num2 = this.findNextSeparator(_input, ':', i);
			if (num2 >= num)
			{
				Log.Warning("Invalid cache elementy: No field separator found in '" + _input.Substring(i, num - i) + "'");
				i = num + 1;
			}
			else
			{
				string text = this.unescapeString(_input.Substring(i, num2 - i));
				string text2 = this.unescapeString(_input.Substring(num2 + 1, num - num2 - 1));
				i = num + 1;
				TKey tkey = _keyParser(text);
				TValue tvalue = _valueParser(text2);
				if (dictionary.ContainsKey(tkey))
				{
					Log.Warning(string.Format("Cache contains multiple elements for '{0}'", tkey));
				}
				dictionary[tkey] = tvalue;
			}
		}
		return dictionary;
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x00190FF4 File Offset: 0x0018F1F4
	[PublicizedFrom(EAccessModifier.Private)]
	public int findNextSeparator(string _input, char _separator, int _startInclusive)
	{
		int num = _input.IndexOf(_separator, _startInclusive);
		while (num >= 0 && num < _input.Length - 1 && _input[num + 1] == _separator)
		{
			num = _input.IndexOf(_separator, num + 2);
		}
		if (num < 0)
		{
			return _input.Length;
		}
		return num;
	}

	// Token: 0x040033AA RID: 13226
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInfoCache instance;

	// Token: 0x040033AB RID: 13227
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue> favoritesHistoryCache;

	// Token: 0x040033AC RID: 13228
	[PublicizedFrom(EAccessModifier.Private)]
	public const char elementSeparator = ';';

	// Token: 0x040033AD RID: 13229
	[PublicizedFrom(EAccessModifier.Private)]
	public const char fieldSeparator = ':';

	// Token: 0x040033AE RID: 13230
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorString = ';'.ToString();

	// Token: 0x040033AF RID: 13231
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorString = ':'.ToString();

	// Token: 0x040033B0 RID: 13232
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorEscaped = new string(';', 2);

	// Token: 0x040033B1 RID: 13233
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorEscaped = new string(':', 2);

	// Token: 0x0200089C RID: 2204
	public class FavoritesHistoryKey : IEquatable<ServerInfoCache.FavoritesHistoryKey>
	{
		// Token: 0x0600401F RID: 16415 RVA: 0x00191088 File Offset: 0x0018F288
		public FavoritesHistoryKey(string _address, int _port)
		{
			if (string.IsNullOrEmpty(_address))
			{
				throw new ArgumentException("Parameter must contain a valid IP", "_address");
			}
			if (_port < 1 || _port > 65535)
			{
				throw new ArgumentException(string.Format("Parameter needs to be a valid port (is: {0})", _port), "_port");
			}
			this.Address = _address;
			this.Port = _port;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x001910E8 File Offset: 0x0018F2E8
		public override string ToString()
		{
			return string.Format("{0}${1}", this.Address, this.Port);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00191108 File Offset: 0x0018F308
		public static ServerInfoCache.FavoritesHistoryKey FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			int num2 = int.Parse(_input.Substring(num + 1));
			return new ServerInfoCache.FavoritesHistoryKey(text, num2);
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x0019113B File Offset: 0x0018F33B
		public bool Equals(ServerInfoCache.FavoritesHistoryKey _other)
		{
			return _other != null && (this == _other || (string.Equals(this.Address, _other.Address, StringComparison.OrdinalIgnoreCase) && this.Port == _other.Port));
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x0019116C File Offset: 0x0018F36C
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (_obj.GetType() == base.GetType() && this.Equals((ServerInfoCache.FavoritesHistoryKey)_obj)));
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x0019119A File Offset: 0x0018F39A
		public override int GetHashCode()
		{
			return (StringComparer.OrdinalIgnoreCase.GetHashCode(this.Address) * 397) ^ this.Port;
		}

		// Token: 0x040033B2 RID: 13234
		public readonly string Address;

		// Token: 0x040033B3 RID: 13235
		public readonly int Port;
	}

	// Token: 0x0200089D RID: 2205
	public class FavoritesHistoryValue
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x001911B9 File Offset: 0x0018F3B9
		public FavoritesHistoryValue(uint _lastPlayedTime, bool _isFavorite)
		{
			this.LastPlayedTime = _lastPlayedTime;
			this.IsFavorite = _isFavorite;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x001911CF File Offset: 0x0018F3CF
		public override string ToString()
		{
			return string.Format("{0}${1}", this.LastPlayedTime, this.IsFavorite);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x001911F4 File Offset: 0x0018F3F4
		public static ServerInfoCache.FavoritesHistoryValue FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			string text2 = _input.Substring(num + 1);
			uint num2 = uint.Parse(text);
			bool flag = bool.Parse(text2);
			return new ServerInfoCache.FavoritesHistoryValue(num2, flag);
		}

		// Token: 0x040033B4 RID: 13236
		public uint LastPlayedTime;

		// Token: 0x040033B5 RID: 13237
		public bool IsFavorite;
	}
}
