using System;
using System.Collections.Generic;

// Token: 0x02001223 RID: 4643
public static class GameSparksCollector
{
	// Token: 0x170011D9 RID: 4569
	// (get) Token: 0x060093EE RID: 37870 RVA: 0x0037E9DA File Offset: 0x0037CBDA
	// (set) Token: 0x060093EF RID: 37871 RVA: 0x0037E9E1 File Offset: 0x0037CBE1
	public static bool CollectGamePlayData { get; set; }

	// Token: 0x060093F0 RID: 37872 RVA: 0x0037E9EC File Offset: 0x0037CBEC
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData GetObject(string _keyString, GSRequestData _collection)
	{
		object obj = GameSparksCollector.lockObject;
		GSRequestData gsrequestData2;
		lock (obj)
		{
			GSRequestData gsrequestData = _collection.GetGSData(_keyString);
			if (gsrequestData != null)
			{
				gsrequestData2 = gsrequestData;
			}
			else
			{
				gsrequestData = new GSRequestData();
				_collection.Add(_keyString, gsrequestData);
				gsrequestData2 = gsrequestData;
			}
		}
		return gsrequestData2;
	}

	// Token: 0x060093F1 RID: 37873 RVA: 0x0037EA48 File Offset: 0x0037CC48
	[PublicizedFrom(EAccessModifier.Private)]
	public static ValueTuple<GSRequestData, string> GetRequestDataAndKey(GameSparksCollector.GSDataCollection _collectionType, GameSparksCollector.GSDataKey _key, string _subKey = null)
	{
		string text = _key.ToStringCached<GameSparksCollector.GSDataKey>();
		GSRequestData gsrequestData = ((_collectionType == GameSparksCollector.GSDataCollection.SessionUpdates) ? GameSparksCollector.dataUpdates : GameSparksCollector.dataSessionTotal);
		if (_subKey == null)
		{
			return new ValueTuple<GSRequestData, string>(gsrequestData, text);
		}
		return new ValueTuple<GSRequestData, string>(GameSparksCollector.GetObject(text, gsrequestData), _subKey);
	}

	// Token: 0x060093F2 RID: 37874 RVA: 0x0037EA88 File Offset: 0x0037CC88
	public static void SetValue(GameSparksCollector.GSDataKey _key, string _subKey, int _value, bool _isGamePlay = true, GameSparksCollector.GSDataCollection _collectionType = GameSparksCollector.GSDataCollection.SessionUpdates)
	{
		if (!_isGamePlay || GameSparksCollector.CollectGamePlayData)
		{
			object obj = GameSparksCollector.lockObject;
			lock (obj)
			{
				ValueTuple<GSRequestData, string> requestDataAndKey = GameSparksCollector.GetRequestDataAndKey(_collectionType, _key, _subKey);
				GSRequestData item = requestDataAndKey.Item1;
				string item2 = requestDataAndKey.Item2;
				item.AddNumber(item2, _value);
			}
		}
	}

	// Token: 0x060093F3 RID: 37875 RVA: 0x0037EAEC File Offset: 0x0037CCEC
	public static void SetValue(GameSparksCollector.GSDataKey _key, string _subKey, string _value, bool _isGamePlay = true, GameSparksCollector.GSDataCollection _collectionType = GameSparksCollector.GSDataCollection.SessionUpdates)
	{
		if (!_isGamePlay || GameSparksCollector.CollectGamePlayData)
		{
			object obj = GameSparksCollector.lockObject;
			lock (obj)
			{
				ValueTuple<GSRequestData, string> requestDataAndKey = GameSparksCollector.GetRequestDataAndKey(_collectionType, _key, _subKey);
				GSRequestData item = requestDataAndKey.Item1;
				string item2 = requestDataAndKey.Item2;
				item.AddString(item2, _value);
			}
		}
	}

	// Token: 0x060093F4 RID: 37876 RVA: 0x0037EB50 File Offset: 0x0037CD50
	public static void IncrementCounter(GameSparksCollector.GSDataKey _key, string _subKey, int _increment, bool _isGamePlay = true, GameSparksCollector.GSDataCollection _collectionType = GameSparksCollector.GSDataCollection.SessionUpdates)
	{
		if (!_isGamePlay || GameSparksCollector.CollectGamePlayData)
		{
			object obj = GameSparksCollector.lockObject;
			lock (obj)
			{
				ValueTuple<GSRequestData, string> requestDataAndKey = GameSparksCollector.GetRequestDataAndKey(_collectionType, _key, _subKey);
				GSRequestData item = requestDataAndKey.Item1;
				string item2 = requestDataAndKey.Item2;
				int num = item.GetInt(item2).GetValueOrDefault();
				num += _increment;
				item.AddNumber(item2, num);
			}
		}
	}

	// Token: 0x060093F5 RID: 37877 RVA: 0x0037EBCC File Offset: 0x0037CDCC
	public static void IncrementCounter(GameSparksCollector.GSDataKey _key, string _subKey, float _increment, bool _isGamePlay = true, GameSparksCollector.GSDataCollection _collectionType = GameSparksCollector.GSDataCollection.SessionUpdates)
	{
		if (!_isGamePlay || GameSparksCollector.CollectGamePlayData)
		{
			object obj = GameSparksCollector.lockObject;
			lock (obj)
			{
				ValueTuple<GSRequestData, string> requestDataAndKey = GameSparksCollector.GetRequestDataAndKey(_collectionType, _key, _subKey);
				GSRequestData item = requestDataAndKey.Item1;
				string item2 = requestDataAndKey.Item2;
				float num = item.GetFloat(item2).GetValueOrDefault();
				num += _increment;
				item.AddNumber(item2, (double)num);
			}
		}
	}

	// Token: 0x060093F6 RID: 37878 RVA: 0x0037EC48 File Offset: 0x0037CE48
	public static void SetMax(GameSparksCollector.GSDataKey _key, string _subKey, int _currentValue, bool _isGamePlay = true, GameSparksCollector.GSDataCollection _collectionType = GameSparksCollector.GSDataCollection.SessionUpdates)
	{
		if (!_isGamePlay || GameSparksCollector.CollectGamePlayData)
		{
			object obj = GameSparksCollector.lockObject;
			lock (obj)
			{
				ValueTuple<GSRequestData, string> requestDataAndKey = GameSparksCollector.GetRequestDataAndKey(_collectionType, _key, _subKey);
				GSRequestData item = requestDataAndKey.Item1;
				string item2 = requestDataAndKey.Item2;
				int num = item.GetInt(item2) ?? int.MinValue;
				num = Math.Max(num, _currentValue);
				item.AddNumber(item2, num);
			}
		}
	}

	// Token: 0x060093F7 RID: 37879 RVA: 0x0037ECD8 File Offset: 0x0037CED8
	public static GSRequestData GetSessionUpdateDataAndReset()
	{
		object obj = GameSparksCollector.lockObject;
		GSRequestData gsrequestData2;
		lock (obj)
		{
			GSRequestData gsrequestData = GameSparksCollector.dataUpdates;
			GameSparksCollector.dataUpdates = new GSRequestData();
			gsrequestData2 = gsrequestData;
		}
		return gsrequestData2;
	}

	// Token: 0x060093F8 RID: 37880 RVA: 0x0037ED24 File Offset: 0x0037CF24
	public static GSRequestData GetSessionTotalData(bool _reset)
	{
		if (!_reset)
		{
			return GameSparksCollector.dataSessionTotal;
		}
		object obj = GameSparksCollector.lockObject;
		GSRequestData gsrequestData2;
		lock (obj)
		{
			GSRequestData gsrequestData = GameSparksCollector.dataSessionTotal;
			GameSparksCollector.dataSessionTotal = new GSRequestData();
			gsrequestData2 = gsrequestData;
		}
		return gsrequestData2;
	}

	// Token: 0x060093F9 RID: 37881 RVA: 0x0037ED78 File Offset: 0x0037CF78
	public static void PlayerLevelUp(EntityPlayerLocal _localPlayer, int _level)
	{
		if (_level == 15)
		{
			GameSparksCollector.SendSaveTimePlayed(GameSparksCollector.GSDataKey.HoursPlayedAtLevel15, _localPlayer);
			GameSparksCollector.SendSkillStats(GameSparksCollector.GSDataKey.SkillsPurchasedAtLevel15, _localPlayer);
			return;
		}
		if (_level == 30)
		{
			GameSparksCollector.SendSaveTimePlayed(GameSparksCollector.GSDataKey.HoursPlayedAtLevel30, _localPlayer);
			GameSparksCollector.SendSkillStats(GameSparksCollector.GSDataKey.SkillsPurchasedAtLevel30, _localPlayer);
			return;
		}
		if (_level != 50)
		{
			return;
		}
		GameSparksCollector.SendSaveTimePlayed(GameSparksCollector.GSDataKey.HoursPlayedAtLevel50, _localPlayer);
		GameSparksCollector.SendSkillStats(GameSparksCollector.GSDataKey.SkillsPurchasedAtLevel50, _localPlayer);
	}

	// Token: 0x060093FA RID: 37882 RVA: 0x0037EDB8 File Offset: 0x0037CFB8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SendSkillStats(GameSparksCollector.GSDataKey _key, EntityPlayerLocal _localPlayer)
	{
		foreach (KeyValuePair<int, ProgressionValue> keyValuePair in _localPlayer.Progression.GetDict())
		{
			ProgressionClass progressionClass = keyValuePair.Value.ProgressionClass;
			for (int i = progressionClass.MinLevel + 1; i <= keyValuePair.Value.Level; i++)
			{
				string text;
				if (progressionClass.Parent == null || progressionClass.Parent == progressionClass)
				{
					text = string.Format("{0}_{1}", progressionClass.Name, i);
				}
				else
				{
					ProgressionClass progressionClass2 = progressionClass.Parent;
					while (progressionClass2.Parent != null && progressionClass2.Parent != progressionClass2)
					{
						progressionClass2 = progressionClass2.Parent;
					}
					text = string.Format("{0}_{1}_{2}", progressionClass2.Name, progressionClass.Name, i);
				}
				GameSparksCollector.SetValue(_key, text, 1, true, GameSparksCollector.GSDataCollection.SessionUpdates);
			}
		}
	}

	// Token: 0x060093FB RID: 37883 RVA: 0x0037EEBC File Offset: 0x0037D0BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SendSaveTimePlayed(GameSparksCollector.GSDataKey _key, EntityPlayerLocal _localPlayer)
	{
		int num = (int)(_localPlayer.totalTimePlayed / 60f);
		if (num > 0)
		{
			GameSparksCollector.SetValue(_key, null, num, true, GameSparksCollector.GSDataCollection.SessionUpdates);
		}
	}

	// Token: 0x04006E5F RID: 28255
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObject = new object();

	// Token: 0x04006E60 RID: 28256
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataUpdates = new GSRequestData();

	// Token: 0x04006E61 RID: 28257
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataSessionTotal = new GSRequestData();

	// Token: 0x02001224 RID: 4644
	public enum GSDataKey
	{
		// Token: 0x04006E64 RID: 28260
		HoursPlayedAtLevel15,
		// Token: 0x04006E65 RID: 28261
		HoursPlayedAtLevel30,
		// Token: 0x04006E66 RID: 28262
		HoursPlayedAtLevel50,
		// Token: 0x04006E67 RID: 28263
		SkillsPurchasedAtLevel15,
		// Token: 0x04006E68 RID: 28264
		SkillsPurchasedAtLevel30,
		// Token: 0x04006E69 RID: 28265
		SkillsPurchasedAtLevel50,
		// Token: 0x04006E6A RID: 28266
		PlayerLevelAtHour,
		// Token: 0x04006E6B RID: 28267
		XpEarnedBy,
		// Token: 0x04006E6C RID: 28268
		PlayerDeathCauses,
		// Token: 0x04006E6D RID: 28269
		ZombiesKilledBy,
		// Token: 0x04006E6E RID: 28270
		CraftedItems,
		// Token: 0x04006E6F RID: 28271
		TraderItemsBought,
		// Token: 0x04006E70 RID: 28272
		VendingItemsBought,
		// Token: 0x04006E71 RID: 28273
		TraderMoneySpentOn,
		// Token: 0x04006E72 RID: 28274
		VendingMoneySpentOn,
		// Token: 0x04006E73 RID: 28275
		TotalMoneySpentOn,
		// Token: 0x04006E74 RID: 28276
		PeakConcurrentClients,
		// Token: 0x04006E75 RID: 28277
		PeakConcurrentPlayers,
		// Token: 0x04006E76 RID: 28278
		QuestTraderToTraderDistance,
		// Token: 0x04006E77 RID: 28279
		QuestAcceptedDistance,
		// Token: 0x04006E78 RID: 28280
		QuestOfferedDistance,
		// Token: 0x04006E79 RID: 28281
		QuestStarterTraderDistance,
		// Token: 0x04006E7A RID: 28282
		PlayerProfileIsCustom,
		// Token: 0x04006E7B RID: 28283
		PlayerArchetypeName,
		// Token: 0x04006E7C RID: 28284
		UsedTwitchIntegration
	}

	// Token: 0x02001225 RID: 4645
	public enum GSDataCollection
	{
		// Token: 0x04006E7E RID: 28286
		SessionTotal,
		// Token: 0x04006E7F RID: 28287
		SessionUpdates
	}
}
