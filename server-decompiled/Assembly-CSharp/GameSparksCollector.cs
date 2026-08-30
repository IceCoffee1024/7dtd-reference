using System;
using System.Collections.Generic;

// Token: 0x020011DF RID: 4575
public static class GameSparksCollector
{
	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06009206 RID: 37382 RVA: 0x00372D52 File Offset: 0x00370F52
	// (set) Token: 0x06009207 RID: 37383 RVA: 0x00372D59 File Offset: 0x00370F59
	public static bool CollectGamePlayData { get; set; }

	// Token: 0x06009208 RID: 37384 RVA: 0x00372D64 File Offset: 0x00370F64
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

	// Token: 0x06009209 RID: 37385 RVA: 0x00372DC0 File Offset: 0x00370FC0
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

	// Token: 0x0600920A RID: 37386 RVA: 0x00372E00 File Offset: 0x00371000
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

	// Token: 0x0600920B RID: 37387 RVA: 0x00372E64 File Offset: 0x00371064
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

	// Token: 0x0600920C RID: 37388 RVA: 0x00372EC8 File Offset: 0x003710C8
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

	// Token: 0x0600920D RID: 37389 RVA: 0x00372F44 File Offset: 0x00371144
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

	// Token: 0x0600920E RID: 37390 RVA: 0x00372FC0 File Offset: 0x003711C0
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

	// Token: 0x0600920F RID: 37391 RVA: 0x00373050 File Offset: 0x00371250
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

	// Token: 0x06009210 RID: 37392 RVA: 0x0037309C File Offset: 0x0037129C
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

	// Token: 0x06009211 RID: 37393 RVA: 0x003730F0 File Offset: 0x003712F0
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

	// Token: 0x06009212 RID: 37394 RVA: 0x00373130 File Offset: 0x00371330
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

	// Token: 0x06009213 RID: 37395 RVA: 0x00373234 File Offset: 0x00371434
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SendSaveTimePlayed(GameSparksCollector.GSDataKey _key, EntityPlayerLocal _localPlayer)
	{
		int num = (int)(_localPlayer.totalTimePlayed / 60f);
		if (num > 0)
		{
			GameSparksCollector.SetValue(_key, null, num, true, GameSparksCollector.GSDataCollection.SessionUpdates);
		}
	}

	// Token: 0x04006D04 RID: 27908
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObject = new object();

	// Token: 0x04006D05 RID: 27909
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataUpdates = new GSRequestData();

	// Token: 0x04006D06 RID: 27910
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataSessionTotal = new GSRequestData();

	// Token: 0x020011E0 RID: 4576
	public enum GSDataKey
	{
		// Token: 0x04006D09 RID: 27913
		HoursPlayedAtLevel15,
		// Token: 0x04006D0A RID: 27914
		HoursPlayedAtLevel30,
		// Token: 0x04006D0B RID: 27915
		HoursPlayedAtLevel50,
		// Token: 0x04006D0C RID: 27916
		SkillsPurchasedAtLevel15,
		// Token: 0x04006D0D RID: 27917
		SkillsPurchasedAtLevel30,
		// Token: 0x04006D0E RID: 27918
		SkillsPurchasedAtLevel50,
		// Token: 0x04006D0F RID: 27919
		PlayerLevelAtHour,
		// Token: 0x04006D10 RID: 27920
		XpEarnedBy,
		// Token: 0x04006D11 RID: 27921
		PlayerDeathCauses,
		// Token: 0x04006D12 RID: 27922
		ZombiesKilledBy,
		// Token: 0x04006D13 RID: 27923
		CraftedItems,
		// Token: 0x04006D14 RID: 27924
		TraderItemsBought,
		// Token: 0x04006D15 RID: 27925
		VendingItemsBought,
		// Token: 0x04006D16 RID: 27926
		TraderMoneySpentOn,
		// Token: 0x04006D17 RID: 27927
		VendingMoneySpentOn,
		// Token: 0x04006D18 RID: 27928
		TotalMoneySpentOn,
		// Token: 0x04006D19 RID: 27929
		PeakConcurrentClients,
		// Token: 0x04006D1A RID: 27930
		PeakConcurrentPlayers,
		// Token: 0x04006D1B RID: 27931
		QuestTraderToTraderDistance,
		// Token: 0x04006D1C RID: 27932
		QuestAcceptedDistance,
		// Token: 0x04006D1D RID: 27933
		QuestOfferedDistance,
		// Token: 0x04006D1E RID: 27934
		QuestStarterTraderDistance,
		// Token: 0x04006D1F RID: 27935
		PlayerProfileIsCustom,
		// Token: 0x04006D20 RID: 27936
		PlayerArchetypeName,
		// Token: 0x04006D21 RID: 27937
		UsedTwitchIntegration
	}

	// Token: 0x020011E1 RID: 4577
	public enum GSDataCollection
	{
		// Token: 0x04006D23 RID: 27939
		SessionTotal,
		// Token: 0x04006D24 RID: 27940
		SessionUpdates
	}
}
