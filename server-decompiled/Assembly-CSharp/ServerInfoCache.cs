using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x020008A8 RID: 2216
public class ServerInfoCache
{
	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06003FBC RID: 16316 RVA: 0x00191828 File Offset: 0x0018FA28
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

	// Token: 0x06003FBD RID: 16317 RVA: 0x00191840 File Offset: 0x0018FA40
	[PublicizedFrom(EAccessModifier.Private)]
	public int serverInfoToPersistentKey(GameServerInfo _gsi)
	{
		return (_gsi.GetValue(GameInfoString.IP) + _gsi.GetValue(GameInfoInt.Port).ToString()).GetHashCode();
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x00191870 File Offset: 0x0018FA70
	public void SavePassword(GameServerInfo _gsi, string _password)
	{
		Dictionary<int, string> passwordCacheList = this.GetPasswordCacheList();
		int num = this.serverInfoToPersistentKey(_gsi);
		passwordCacheList[num] = _password;
		GamePrefs.Set(EnumGamePrefs.ServerPasswordCache, this.DictToString<int, string>(passwordCacheList));
	}

	// Token: 0x06003FBF RID: 16319 RVA: 0x001918A4 File Offset: 0x0018FAA4
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

	// Token: 0x06003FC0 RID: 16320 RVA: 0x001918D0 File Offset: 0x0018FAD0
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

	// Token: 0x06003FC1 RID: 16321 RVA: 0x00191940 File Offset: 0x0018FB40
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

	// Token: 0x06003FC2 RID: 16322 RVA: 0x001919DC File Offset: 0x0018FBDC
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

	// Token: 0x06003FC3 RID: 16323 RVA: 0x00191A6C File Offset: 0x0018FC6C
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

	// Token: 0x06003FC4 RID: 16324 RVA: 0x00191B3C File Offset: 0x0018FD3C
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

	// Token: 0x06003FC5 RID: 16325 RVA: 0x00191BCC File Offset: 0x0018FDCC
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>.Enumerator GetFavoriteServersEnumerator()
	{
		this.getFavHistoryCacheList();
		return this.favoritesHistoryCache.GetEnumerator();
	}

	// Token: 0x06003FC6 RID: 16326 RVA: 0x00191BE0 File Offset: 0x0018FDE0
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

	// Token: 0x06003FC7 RID: 16327 RVA: 0x00191C1C File Offset: 0x0018FE1C
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

	// Token: 0x06003FC8 RID: 16328 RVA: 0x00191C90 File Offset: 0x0018FE90
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveFavHistoryCacheList()
	{
		GamePrefs.Set(EnumGamePrefs.ServerHistoryCache, this.DictToString<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>(this.favoritesHistoryCache));
	}

	// Token: 0x06003FC9 RID: 16329 RVA: 0x00191CA8 File Offset: 0x0018FEA8
	[PublicizedFrom(EAccessModifier.Private)]
	public string escapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorString, ServerInfoCache.elementSeparatorEscaped).Replace(ServerInfoCache.fieldSeparatorString, ServerInfoCache.fieldSeparatorEscaped);
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x00191CC9 File Offset: 0x0018FEC9
	[PublicizedFrom(EAccessModifier.Private)]
	public string unescapeString(string _input)
	{
		return _input.Replace(ServerInfoCache.elementSeparatorEscaped, ServerInfoCache.elementSeparatorString).Replace(ServerInfoCache.fieldSeparatorEscaped, ServerInfoCache.fieldSeparatorString);
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x00191CEC File Offset: 0x0018FEEC
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

	// Token: 0x06003FCC RID: 16332 RVA: 0x00191DA0 File Offset: 0x0018FFA0
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

	// Token: 0x06003FCD RID: 16333 RVA: 0x00191E8C File Offset: 0x0019008C
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

	// Token: 0x040033B5 RID: 13237
	[PublicizedFrom(EAccessModifier.Private)]
	public static ServerInfoCache instance;

	// Token: 0x040033B6 RID: 13238
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue> favoritesHistoryCache;

	// Token: 0x040033B7 RID: 13239
	[PublicizedFrom(EAccessModifier.Private)]
	public const char elementSeparator = ';';

	// Token: 0x040033B8 RID: 13240
	[PublicizedFrom(EAccessModifier.Private)]
	public const char fieldSeparator = ':';

	// Token: 0x040033B9 RID: 13241
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorString = ';'.ToString();

	// Token: 0x040033BA RID: 13242
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorString = ':'.ToString();

	// Token: 0x040033BB RID: 13243
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string elementSeparatorEscaped = new string(';', 2);

	// Token: 0x040033BC RID: 13244
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string fieldSeparatorEscaped = new string(':', 2);

	// Token: 0x020008A9 RID: 2217
	public class FavoritesHistoryKey : IEquatable<ServerInfoCache.FavoritesHistoryKey>
	{
		// Token: 0x06003FD0 RID: 16336 RVA: 0x00191F20 File Offset: 0x00190120
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

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00191F80 File Offset: 0x00190180
		public override string ToString()
		{
			return string.Format("{0}${1}", this.Address, this.Port);
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x00191FA0 File Offset: 0x001901A0
		public static ServerInfoCache.FavoritesHistoryKey FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			int num2 = int.Parse(_input.Substring(num + 1));
			return new ServerInfoCache.FavoritesHistoryKey(text, num2);
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00191FD3 File Offset: 0x001901D3
		public bool Equals(ServerInfoCache.FavoritesHistoryKey _other)
		{
			return _other != null && (this == _other || (string.Equals(this.Address, _other.Address, StringComparison.OrdinalIgnoreCase) && this.Port == _other.Port));
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x00192004 File Offset: 0x00190204
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (_obj.GetType() == base.GetType() && this.Equals((ServerInfoCache.FavoritesHistoryKey)_obj)));
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00192032 File Offset: 0x00190232
		public override int GetHashCode()
		{
			return (StringComparer.OrdinalIgnoreCase.GetHashCode(this.Address) * 397) ^ this.Port;
		}

		// Token: 0x040033BD RID: 13245
		public readonly string Address;

		// Token: 0x040033BE RID: 13246
		public readonly int Port;
	}

	// Token: 0x020008AA RID: 2218
	public class FavoritesHistoryValue
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x00192051 File Offset: 0x00190251
		public FavoritesHistoryValue(uint _lastPlayedTime, bool _isFavorite)
		{
			this.LastPlayedTime = _lastPlayedTime;
			this.IsFavorite = _isFavorite;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00192067 File Offset: 0x00190267
		public override string ToString()
		{
			return string.Format("{0}${1}", this.LastPlayedTime, this.IsFavorite);
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0019208C File Offset: 0x0019028C
		public static ServerInfoCache.FavoritesHistoryValue FromString(string _input)
		{
			int num = _input.IndexOf('$');
			string text = _input.Substring(0, num);
			string text2 = _input.Substring(num + 1);
			uint num2 = uint.Parse(text);
			bool flag = bool.Parse(text2);
			return new ServerInfoCache.FavoritesHistoryValue(num2, flag);
		}

		// Token: 0x040033BF RID: 13247
		public uint LastPlayedTime;

		// Token: 0x040033C0 RID: 13248
		public bool IsFavorite;
	}
}
