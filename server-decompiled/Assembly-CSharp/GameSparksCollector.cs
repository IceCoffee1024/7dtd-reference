using System;
using System.Collections.Generic;

// Token: 0x02001213 RID: 4627
public static class GameSparksCollector
{
	// Token: 0x170011CE RID: 4558
	// (get) Token: 0x06009398 RID: 37784 RVA: 0x0037C866 File Offset: 0x0037AA66
	// (set) Token: 0x06009399 RID: 37785 RVA: 0x0037C86D File Offset: 0x0037AA6D
	public static bool CollectGamePlayData { get; set; }

	// Token: 0x0600939A RID: 37786 RVA: 0x0037C878 File Offset: 0x0037AA78
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

	// Token: 0x0600939B RID: 37787 RVA: 0x0037C8D4 File Offset: 0x0037AAD4
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

	// Token: 0x0600939C RID: 37788 RVA: 0x0037C914 File Offset: 0x0037AB14
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

	// Token: 0x0600939D RID: 37789 RVA: 0x0037C978 File Offset: 0x0037AB78
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

	// Token: 0x0600939E RID: 37790 RVA: 0x0037C9DC File Offset: 0x0037ABDC
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

	// Token: 0x0600939F RID: 37791 RVA: 0x0037CA58 File Offset: 0x0037AC58
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

	// Token: 0x060093A0 RID: 37792 RVA: 0x0037CAD4 File Offset: 0x0037ACD4
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

	// Token: 0x060093A1 RID: 37793 RVA: 0x0037CB64 File Offset: 0x0037AD64
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

	// Token: 0x060093A2 RID: 37794 RVA: 0x0037CBB0 File Offset: 0x0037ADB0
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

	// Token: 0x060093A3 RID: 37795 RVA: 0x0037CC04 File Offset: 0x0037AE04
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

	// Token: 0x060093A4 RID: 37796 RVA: 0x0037CC44 File Offset: 0x0037AE44
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

	// Token: 0x060093A5 RID: 37797 RVA: 0x0037CD48 File Offset: 0x0037AF48
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SendSaveTimePlayed(GameSparksCollector.GSDataKey _key, EntityPlayerLocal _localPlayer)
	{
		int num = (int)(_localPlayer.totalTimePlayed / 60f);
		if (num > 0)
		{
			GameSparksCollector.SetValue(_key, null, num, true, GameSparksCollector.GSDataCollection.SessionUpdates);
		}
	}

	// Token: 0x04006E47 RID: 28231
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObject = new object();

	// Token: 0x04006E48 RID: 28232
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataUpdates = new GSRequestData();

	// Token: 0x04006E49 RID: 28233
	[PublicizedFrom(EAccessModifier.Private)]
	public static GSRequestData dataSessionTotal = new GSRequestData();

	// Token: 0x02001214 RID: 4628
	public enum GSDataKey
	{
		// Token: 0x04006E4C RID: 28236
		HoursPlayedAtLevel15,
		// Token: 0x04006E4D RID: 28237
		HoursPlayedAtLevel30,
		// Token: 0x04006E4E RID: 28238
		HoursPlayedAtLevel50,
		// Token: 0x04006E4F RID: 28239
		SkillsPurchasedAtLevel15,
		// Token: 0x04006E50 RID: 28240
		SkillsPurchasedAtLevel30,
		// Token: 0x04006E51 RID: 28241
		SkillsPurchasedAtLevel50,
		// Token: 0x04006E52 RID: 28242
		PlayerLevelAtHour,
		// Token: 0x04006E53 RID: 28243
		XpEarnedBy,
		// Token: 0x04006E54 RID: 28244
		PlayerDeathCauses,
		// Token: 0x04006E55 RID: 28245
		ZombiesKilledBy,
		// Token: 0x04006E56 RID: 28246
		CraftedItems,
		// Token: 0x04006E57 RID: 28247
		TraderItemsBought,
		// Token: 0x04006E58 RID: 28248
		VendingItemsBought,
		// Token: 0x04006E59 RID: 28249
		TraderMoneySpentOn,
		// Token: 0x04006E5A RID: 28250
		VendingMoneySpentOn,
		// Token: 0x04006E5B RID: 28251
		TotalMoneySpentOn,
		// Token: 0x04006E5C RID: 28252
		PeakConcurrentClients,
		// Token: 0x04006E5D RID: 28253
		PeakConcurrentPlayers,
		// Token: 0x04006E5E RID: 28254
		QuestTraderToTraderDistance,
		// Token: 0x04006E5F RID: 28255
		QuestAcceptedDistance,
		// Token: 0x04006E60 RID: 28256
		QuestOfferedDistance,
		// Token: 0x04006E61 RID: 28257
		QuestStarterTraderDistance,
		// Token: 0x04006E62 RID: 28258
		PlayerProfileIsCustom,
		// Token: 0x04006E63 RID: 28259
		PlayerArchetypeName,
		// Token: 0x04006E64 RID: 28260
		UsedTwitchIntegration
	}

	// Token: 0x02001215 RID: 4629
	public enum GSDataCollection
	{
		// Token: 0x04006E66 RID: 28262
		SessionTotal,
		// Token: 0x04006E67 RID: 28263
		SessionUpdates
	}
}
