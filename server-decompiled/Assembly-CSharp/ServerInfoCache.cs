using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x0200088F RID: 2191
public class ServerInfoCache
{
	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x0018E8C0 File Offset: 0x0018CAC0
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

	// Token: 0x06003FA7 RID: 16295 RVA: 0x0018E8D8 File Offset: 0x0018CAD8
	[PublicizedFrom(EAccessModifier.Private)]
	public int serverInfoToPersistentKey(GameServerInfo _gsi)
	{
		return (_gsi.GetValue(GameInfoString.IP) + _gsi.GetValue(GameInfoInt.Port).ToString()).GetHashCode();
	}

	// Token: 0x06003FA8 RID: 16296 RVA: 0x0018E908 File Offset: 0x0018CB08
	public void SavePassword(GameServerInfo _gsi, string _password)
	{
		Dictionary<int, string> passwordCacheList = this.GetPasswordCacheList();
		int num = this.serverInfoToPersistentKey(_gsi);
		passwordCacheList[num] = _password;
		GamePrefs.Set(EnumGamePrefs.ServerPasswordCache, this.DictToString<int, string>(passwordCacheList));
	}

	// Token: 0x06003FA9 RID: 16297 RVA: 0x0018E93C File Offset: 0x0018CB3C
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

	// Token: 0x06003FAA RID: 16298 RVA: 0x0018E968 File Offset: 0x0018CB68
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

	// Token: 0x06003FAB RID: 16299 RVA: 0x0018E9D8 File Offset: 0x0018CBD8
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

	// Token: 0x06003FAC RID: 16300 RVA: 0x0018EA74 File Offset: 0x0018CC74
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

	// Token: 0x06003FAD RID: 16301 RVA: 0x0018EB04 File Offset: 0x0018CD04
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

	// Token: 0x06003FAE RID: 16302 RVA: 0x0018EBD4 File Offset: 0x0018CDD4
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

	// Token: 0x06003FAF RID: 16303 RVA: 0x0018EC64 File Offset: 0x0018CE64
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>.Enumerator GetFavoriteServersEnumerator()
	{
		this.getFavHistoryCacheList();
		return this.favoritesHistoryCache.GetEnumerator();
	}

	// Token: 0x06003FB0 RID: 16304 RVA: 0x0018EC78 File Offset: 0x0018CE78
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

	// Token: 0x06003FB1 RID: 16305 RVA: 0x0018ECB4 File Offset: 0x0018CEB4
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

	// Token: 0x06003FB2 RID: 16306 RVA: 0x0018ED28 File Offset: 0x0018CF28
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveFavHistoryCacheList()
	{
		GamePrefs.Set(EnumGamePrefs.ServerHistoryCache, this.DictToString<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>(this.favoritesHistoryCache));
	}

	// Token: 0x06003FB3 RID: 16307 RVA: 0x0018ED40 File Offset: 0x0018CF40
	[PublicizedFrom(EAccessModifier.Private)]
	public string escapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorString, ServerInfoCache.elementSeparatorEscaped).Replace(ServerInfoCache.fieldSeparatorString, ServerInfoCache.fieldSeparatorEscaped);
	}

	// Token: 0x06003FB4 RID: 16308 RVA: 0x0018ED61 File Offset: 0x0018CF61
	[PublicizedFrom(EAccessModifier.Private)]
	public string unescapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorEscaped, ServerInfoCache.elementSeparatorString).Replace(ServerInfoCache.fieldSeparatorEscaped, ServerInfoCache.fieldSeparatorString);
	}

	// Token: 0x06003FB5 RID: 16309 RVA: 0x0018ED84 File Offset: 0x0018CF84
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

	// Token: 0x06003FB6 RID: 16310 RVA: 0x0018EE38 File Offset: 0x0018D038
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

	// Token: 0x06003FB7 RID: 16311 RVA: 0x0018EF24 File Offset: 0x0018D124
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

	// Token: 0x04003329 RID: 13097
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInfoCache instance;

	// Token: 0x0400332A RID: 13098
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue> favoritesHistoryCache;

	// Token: 0x0400332B RID: 13099
	[PublicizedFrom(EAccessModifier.Private)]
	public const char elementSeparator = ';';

	// Token: 0x0400332C RID: 13100
	[PublicizedFrom(EAccessModifier.Private)]
	public const char fieldSeparator = ':';

	// Token: 0x0400332D RID: 13101
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorString = ';'.ToString();

	// Token: 0x0400332E RID: 13102
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorString = ':'.ToString();

	// Token: 0x0400332F RID: 13103
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorEscaped = new string(';', 2);

	// Token: 0x04003330 RID: 13104
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorEscaped = new string(':', 2);

	// Token: 0x02000890 RID: 2192
	public class FavoritesHistoryKey : IEquatable<ServerInfoCache.FavoritesHistoryKey>
	{
		// Token: 0x06003FBA RID: 16314 RVA: 0x0018EFB8 File Offset: 0x0018D1B8
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

		// Token: 0x06003FBB RID: 16315 RVA: 0x0018F018 File Offset: 0x0018D218
		public override string ToString()
		{
			return string.Format("{0}${1}", this.Address, this.Port);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0018F038 File Offset: 0x0018D238
		public static ServerInfoCache.FavoritesHistoryKey FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			int num2 = int.Parse(_input.Substring(num + 1));
			return new ServerInfoCache.FavoritesHistoryKey(text, num2);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0018F06B File Offset: 0x0018D26B
		public bool Equals(ServerInfoCache.FavoritesHistoryKey _other)
		{
			return _other != null && (this == _other || (string.Equals(this.Address, _other.Address, StringComparison.OrdinalIgnoreCase) && this.Port == _other.Port));
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0018F09C File Offset: 0x0018D29C
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (_obj.GetType() == base.GetType() && this.Equals((ServerInfoCache.FavoritesHistoryKey)_obj)));
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0018F0CA File Offset: 0x0018D2CA
		public override int GetHashCode()
		{
			return (StringComparer.OrdinalIgnoreCase.GetHashCode(this.Address) * 397) ^ this.Port;
		}

		// Token: 0x04003331 RID: 13105
		public readonly string Address;

		// Token: 0x04003332 RID: 13106
		public readonly int Port;
	}

	// Token: 0x02000891 RID: 2193
	public class FavoritesHistoryValue
	{
		// Token: 0x06003FC0 RID: 16320 RVA: 0x0018F0E9 File Offset: 0x0018D2E9
		public FavoritesHistoryValue(uint _lastPlayedTime, bool _isFavorite)
		{
			this.LastPlayedTime = _lastPlayedTime;
			this.IsFavorite = _isFavorite;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0018F0FF File Offset: 0x0018D2FF
		public override string ToString()
		{
			return string.Format("{0}${1}", this.LastPlayedTime, this.IsFavorite);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0018F124 File Offset: 0x0018D324
		public static ServerInfoCache.FavoritesHistoryValue FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			string text2 = _input.Substring(num + 1);
			uint num2 = uint.Parse(text);
			bool flag = bool.Parse(text2);
			return new ServerInfoCache.FavoritesHistoryValue(num2, flag);
		}

		// Token: 0x04003333 RID: 13107
		public uint LastPlayedTime;

		// Token: 0x04003334 RID: 13108
		public bool IsFavorite;
	}
}
