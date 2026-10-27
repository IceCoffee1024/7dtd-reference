using System;
using System.Collections.Generic;
using System.IO;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
public class Progression
{
	// Token: 0x060052E2 RID: 21218 RVA: 0x001FD690 File Offset: 0x001FB890
	public Progression()
	{
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001FD6FC File Offset: 0x001FB8FC
	public Progression(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
		this.SetupData();
	}

	// Token: 0x060052E4 RID: 21220 RVA: 0x001FD774 File Offset: 0x001FB974
	public Dictionary<int, ProgressionValue> GetDict()
	{
		return this.ProgressionValues.Dict;
	}

	// Token: 0x060052E5 RID: 21221 RVA: 0x001FD781 File Offset: 0x001FB981
	public static int CalcId(string _name)
	{
		return Progression.ProgressionNameIds.Add(_name);
	}

	// Token: 0x060052E6 RID: 21222 RVA: 0x001FD78E File Offset: 0x001FB98E
	public ProgressionValue GetProgressionValue(int _id)
	{
		return this.ProgressionValues.Get(_id);
	}

	// Token: 0x060052E7 RID: 21223 RVA: 0x001FD79C File Offset: 0x001FB99C
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getLevelFloat()
	{
		return (float)this.Level + (1f - (float)this.ExpToNextLevel / (float)this.GetExpForNextLevel());
	}

	// Token: 0x060052E8 RID: 21224 RVA: 0x001FD7BB File Offset: 0x001FB9BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public int getExpForLevel(float _level)
	{
		return (int)Math.Min((float)Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _level), 2.1474836E+09f);
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001FD7DA File Offset: 0x001FB9DA
	public int GetLevel()
	{
		return this.Level;
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001FD7E2 File Offset: 0x001FB9E2
	public int GetExpForNextLevel()
	{
		return this.getExpForLevel((float)Mathf.Clamp(this.Level + 1, 0, Progression.ClampExpCostAtLevel));
	}

	// Token: 0x060052EB RID: 21227 RVA: 0x001FD7FE File Offset: 0x001FB9FE
	public float GetLevelProgressPercentage()
	{
		return this.getLevelFloat() - (float)this.Level;
	}

	// Token: 0x060052EC RID: 21228 RVA: 0x001FD810 File Offset: 0x001FBA10
	public void ModifyValue(PassiveEffects _effect, ref float _baseVal, ref float _percVal, FastTags<TagGroup.Global> _tags)
	{
		if (_effect == PassiveEffects.AttributeLevel)
		{
			return;
		}
		if (_effect == PassiveEffects.SkillLevel)
		{
			return;
		}
		if (_effect == PassiveEffects.PerkLevel)
		{
			return;
		}
		List<ProgressionValue> list;
		if (this.passiveEffects.TryGetValue(_effect, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				ProgressionValue progressionValue = list[i];
				ProgressionClass progressionClass = progressionValue.ProgressionClass;
				if (progressionClass != null)
				{
					progressionClass.ModifyValue(this.parent, progressionValue, _effect, ref _baseVal, ref _percVal, _tags);
				}
			}
		}
	}

	// Token: 0x060052ED RID: 21229 RVA: 0x001FD874 File Offset: 0x001FBA74
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, PassiveEffects _effect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> _tags)
	{
		if (_effect == PassiveEffects.AttributeLevel)
		{
			return;
		}
		if (_effect == PassiveEffects.SkillLevel)
		{
			return;
		}
		if (_effect == PassiveEffects.PerkLevel)
		{
			return;
		}
		for (int i = 0; i < this.ProgressionValueQuickList.Count; i++)
		{
			ProgressionValue progressionValue = this.ProgressionValueQuickList[i];
			if (progressionValue != null)
			{
				ProgressionClass progressionClass = progressionValue.ProgressionClass;
				if (progressionClass != null && progressionClass.Effects != null && progressionClass.Effects.PassivesIndex != null && progressionClass.Effects.PassivesIndex.Contains(_effect))
				{
					progressionClass.GetModifiedValueData(_modValueSources, _sourceType, this.parent, progressionValue, _effect, ref _base_val, ref _perc_val, _tags);
				}
			}
		}
	}

	// Token: 0x060052EE RID: 21230 RVA: 0x001FD904 File Offset: 0x001FBB04
	public void Update()
	{
		if (this.timer <= 0f)
		{
			this.FireEvent(MinEventTypes.onSelfProgressionUpdate, this.parent.MinEventContext);
			this.timer = 1f;
		}
		else
		{
			this.timer -= Time.deltaTime;
		}
		this.parent.Buffs.SetCustomVar("_expdeficit", (float)this.ExpDeficit, true, CVarOperation.set, false);
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001FD970 File Offset: 0x001FBB70
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.eventList == null)
		{
			return;
		}
		for (int i = 0; i < this.eventList.Count; i++)
		{
			ProgressionValue progressionValue = this.eventList[i];
			ProgressionClass progressionClass = progressionValue.ProgressionClass;
			_params.ProgressionValue = progressionValue;
			progressionClass.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001FD9C0 File Offset: 0x001FBBC0
	public int AddLevelExp(int _exp, string _cvarXPName = "_xpOther", Progression.XPTypes _xpType = Progression.XPTypes.Other, bool useBonus = true, bool notifyUI = true, int _instigatorID = -1, ItemValue _itemValue = null)
	{
		if (this.parent as EntityPlayer == null)
		{
			return _exp;
		}
		this.parent.MinEventContext.Instigator = ((_instigatorID == -1) ? null : (this.parent.world.GetEntity(_instigatorID) as EntityAlive));
		float num = (float)_exp;
		if (useBonus)
		{
			if (this.xpFastTags == null)
			{
				this.xpFastTags = new FastTags<TagGroup.Global>[11];
				for (int i = 0; i < 11; i++)
				{
					this.xpFastTags[i] = FastTags<TagGroup.Global>.Parse(((Progression.XPTypes)i).ToStringCached<Progression.XPTypes>());
				}
			}
			num *= Progression.XPGain;
			PassiveEffects passiveEffects = PassiveEffects.PlayerExpGain;
			float num2 = num;
			EntityAlive entityAlive = this.parent;
			FastTags<TagGroup.Global> fastTags = this.xpFastTags[(int)_xpType];
			num = EffectManager.GetValue(passiveEffects, _itemValue, num2, entityAlive, null, fastTags, true, true, true, true, true, 1, true, false);
		}
		if (num > 214748370f)
		{
			num = 214748370f;
		}
		if (_xpType != Progression.XPTypes.Debug)
		{
			GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.XpEarnedBy, _xpType.ToStringCached<Progression.XPTypes>(), num, true, GameSparksCollector.GSDataCollection.SessionUpdates);
		}
		int level = this.Level;
		int skillPoints = this.SkillPoints;
		EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
		if (entityPlayerLocal && (Progression.ShowXPType == Progression.ShowXPTypes.All || Progression.ShowXPType == Progression.ShowXPTypes.NotificationsOnly))
		{
			entityPlayerLocal.PlayerUI.xui.CollectedItemList.AddIconNotification("ui_game_symbol_xp", (int)num, false);
		}
		this.AddLevelExpRecursive((int)num, _cvarXPName, notifyUI);
		if (this.Level != level)
		{
			Log.Out("{0} made level {1} (was {2}), exp for next level {3}", new object[]
			{
				this.parent.EntityName,
				this.Level,
				level,
				this.ExpToNextLevel
			});
			if (entityPlayerLocal && !Helper.IsEditingOrPlaytesting())
			{
				PlayerSkillPointEventData playerSkillPointEventData = new PlayerSkillPointEventData
				{
					ServerId = Helper.GetServerId(),
					SaveId = Helper.GetSaveId(),
					TransactionTimestamp = DateTime.UtcNow.ToString("O"),
					ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
					PlayerLevel = this.Level,
					InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
					TransactionType = "Earn",
					Amount = this.SkillPoints - skillPoints,
					BalanceBefore = skillPoints,
					BalanceAfter = this.SkillPoints,
					TotalSpent = this.GetAllTimeSkillPoint(),
					SPSource = "character_level_up",
					SPTarget = null,
					SPTargetDetail = null,
					SPTargerLevel = null
				};
				ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerSkillPointEventData);
			}
		}
		return (int)num;
	}

	// Token: 0x060052F1 RID: 21233 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDeath()
	{
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001FDC54 File Offset: 0x001FBE54
	public void AddXPDeficit()
	{
		if (Progression.XPGain == 0f)
		{
			return;
		}
		this.ExpDeficit += (int)((float)this.GetExpForNextLevel() * EffectManager.GetValue(PassiveEffects.ExpDeficitPerDeathPercentage, null, 0.1f, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		this.ExpDeficit = Mathf.Clamp(this.ExpDeficit, 0, (int)((float)this.GetExpForNextLevel() * EffectManager.GetValue(PassiveEffects.ExpDeficitMaxPercentage, null, 0.5f, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)));
		this.ExpDeficitGained = true;
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001FDCF0 File Offset: 0x001FBEF0
	public void OnRespawnFromDeath()
	{
		if (!this.ExpDeficitGained)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
		if (this.ExpDeficit == (int)((float)this.GetExpForNextLevel() * EffectManager.GetValue(PassiveEffects.ExpDeficitMaxPercentage, null, 0.5f, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)))
		{
			GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("ttResurrectMaxXPLost", false, null), false, false, 0f);
		}
		else
		{
			GameManager.ShowTooltip(entityPlayerLocal, string.Format(Localization.Get("ttResurrectXPLost", false, null), EffectManager.GetValue(PassiveEffects.ExpDeficitPerDeathPercentage, null, 0.1f, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * 100f), false, false, 0f);
		}
		this.ExpDeficitGained = false;
	}

	// Token: 0x060052F4 RID: 21236 RVA: 0x001FDDB8 File Offset: 0x001FBFB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddLevelExpRecursive(int exp, string _cvarXPName, bool notifyUI = true)
	{
		if (this.Level >= Progression.MaxLevel)
		{
			this.Level = Progression.MaxLevel;
			return;
		}
		this.parent.Buffs.IncrementCustomVar(_cvarXPName, (float)exp);
		int num;
		if (this.ExpDeficit > 0)
		{
			num = exp - this.ExpDeficit;
			this.ExpDeficit -= exp;
			this.ExpDeficit = Mathf.Clamp(this.ExpDeficit, 0, (int)((float)this.GetExpForNextLevel() * EffectManager.GetValue(PassiveEffects.ExpDeficitMaxPercentage, null, 0.5f, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)));
		}
		else
		{
			num = exp - this.ExpToNextLevel;
			this.ExpToNextLevel -= exp;
		}
		EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
		if (this.ExpDeficit <= 0)
		{
			int level = this.Level;
			if (this.ExpToNextLevel <= 0)
			{
				this.Level++;
				if (this.Level % Progression.SkillPointsGainRate == 0)
				{
					if (Progression.SkillPointMultiplier == 0f)
					{
						this.SkillPoints += Progression.SkillPointsPerLevel;
					}
					else
					{
						this.SkillPoints += (int)Math.Min((float)Progression.SkillPointsPerLevel * Mathf.Pow(Progression.SkillPointMultiplier, (float)this.Level), 2.1474836E+09f);
					}
				}
				if (entityPlayerLocal)
				{
					GameSparksCollector.PlayerLevelUp(entityPlayerLocal, this.Level);
				}
				this.ExpToNextLevel = this.GetExpForNextLevel();
			}
			if ((this.ExpToNextLevel > num || this.Level == Progression.MaxLevel) && level != this.Level && entityPlayerLocal && notifyUI)
			{
				GameManager.ShowTooltip(entityPlayerLocal, string.Format(Localization.Get("ttLevelUp", false, null), this.Level.ToString(), this.SkillPoints), string.Empty, "levelupplayer", null, false, false, 0f);
			}
		}
		if (num > 0)
		{
			this.AddLevelExpRecursive(num, _cvarXPName, true);
		}
	}

	// Token: 0x060052F5 RID: 21237 RVA: 0x001FDF98 File Offset: 0x001FC198
	public void SpendSkillPoints(int _points, string _progressionName)
	{
		ProgressionValue progressionValue = this.GetProgressionValue(_progressionName);
		int skillPoints = this.SkillPoints;
		if (progressionValue != null && progressionValue.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
		{
			this.addProgressionCurrency(_points, progressionValue);
		}
		EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
		if (entityPlayerLocal && !Helper.IsEditingOrPlaytesting())
		{
			PlayerSkillPointEventData playerSkillPointEventData = new PlayerSkillPointEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				TransactionTimestamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
				PlayerLevel = this.Level,
				InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
				TransactionType = "Earn",
				Amount = _points,
				BalanceBefore = skillPoints,
				BalanceAfter = this.SkillPoints,
				TotalSpent = this.GetAllTimeSkillPoint(),
				SPSource = null,
				SPTarget = "perk_purchase",
				SPTargetDetail = _progressionName,
				SPTargerLevel = new int?(progressionValue.Level)
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerSkillPointEventData);
		}
	}

	// Token: 0x060052F6 RID: 21238 RVA: 0x001FE0C7 File Offset: 0x001FC2C7
	public ProgressionValue GetProgressionValue(string _progressionName)
	{
		return this.ProgressionValues.Get(_progressionName);
	}

	// Token: 0x060052F7 RID: 21239 RVA: 0x001FE0D8 File Offset: 0x001FC2D8
	public void GetPerkList(List<ProgressionValue> perkList, string _skillName)
	{
		perkList.Clear();
		for (int i = 0; i < this.ProgressionValueQuickList.Count; i++)
		{
			ProgressionValue progressionValue = this.ProgressionValueQuickList[i];
			if ((progressionValue.ProgressionClass.Type == ProgressionType.Perk || progressionValue.ProgressionClass.Type == ProgressionType.Book) && progressionValue.ProgressionClass.Parent.Name == _skillName)
			{
				perkList.Add(progressionValue);
			}
		}
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x001FE14C File Offset: 0x001FC34C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addProgressionCurrency(int _currencyAmount, ProgressionValue _pv)
	{
		if (_pv == null)
		{
			return;
		}
		ProgressionClass progressionClass = _pv.ProgressionClass;
		if (_pv.Level >= progressionClass.MaxLevel)
		{
			if (_pv.Level > progressionClass.MaxLevel)
			{
				_pv.Level = progressionClass.MaxLevel;
			}
			return;
		}
		if (progressionClass.Type == ProgressionType.Skill)
		{
			_currencyAmount = (int)EffectManager.GetValue(PassiveEffects.SkillExpGain, null, (float)_currencyAmount, this.parent, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		int num = _currencyAmount - _pv.CostForNextLevel;
		_pv.CostForNextLevel -= _currencyAmount;
		if (_pv.CostForNextLevel <= 0)
		{
			int level = _pv.Level;
			_pv.Level = level + 1;
			_pv.CostForNextLevel = progressionClass.CalculatedCostForLevel(_pv.Level + 1);
		}
		if (num > 0)
		{
			this.addProgressionCurrency(num, _pv);
		}
	}

	// Token: 0x060052F9 RID: 21241 RVA: 0x001FE20C File Offset: 0x001FC40C
	public byte[] ToBytes(bool _IsNetwork = false)
	{
		byte[] array = null;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
		{
			MemoryStream memoryStream = new MemoryStream();
			pooledBinaryWriter.SetBaseStream(memoryStream);
			this.Write(pooledBinaryWriter, _IsNetwork);
			array = memoryStream.ToArray();
			memoryStream.Dispose();
		}
		return array;
	}

	// Token: 0x060052FA RID: 21242 RVA: 0x001FE268 File Offset: 0x001FC468
	public void Write(BinaryWriter _bw, bool _IsNetwork = false)
	{
		_bw.Write(3);
		_bw.Write((ushort)this.Level);
		_bw.Write(this.ExpToNextLevel);
		_bw.Write((ushort)this.SkillPoints);
		int count = this.ProgressionValues.Count;
		_bw.Write(count);
		foreach (KeyValuePair<int, ProgressionValue> keyValuePair in this.ProgressionValues.Dict)
		{
			keyValuePair.Value.Write(_bw, _IsNetwork);
		}
		_bw.Write(this.ExpDeficit);
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x001FE314 File Offset: 0x001FC514
	public static Progression FromBytes(byte[] data, EntityAlive _parent)
	{
		Progression progression = null;
		try
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				MemoryStream memoryStream = new MemoryStream(data);
				pooledBinaryReader.SetBaseStream(memoryStream);
				progression = Progression.Read(pooledBinaryReader, _parent);
				memoryStream.Dispose();
			}
		}
		catch
		{
			progression = null;
		}
		return progression;
	}

	// Token: 0x060052FC RID: 21244 RVA: 0x001FE37C File Offset: 0x001FC57C
	public static Progression Read(BinaryReader _br, EntityAlive _parent)
	{
		Progression progression = _parent.Progression;
		if (progression == null)
		{
			Log.Warning("Progression Read {0}, new", new object[] { _parent });
			progression = new Progression(_parent);
			_parent.Progression = progression;
		}
		byte b = _br.ReadByte();
		progression.Level = (int)_br.ReadUInt16();
		progression.ExpToNextLevel = _br.ReadInt32();
		progression.SkillPoints = (int)_br.ReadUInt16();
		int num = _br.ReadInt32();
		ProgressionValue progressionValue = new ProgressionValue();
		for (int i = 0; i < num; i++)
		{
			progressionValue.Read(_br);
			if (Progression.ProgressionClasses.ContainsKey(progressionValue.Name))
			{
				ProgressionValue progressionValue2 = progression.ProgressionValues.Get(progressionValue.Name);
				if (progressionValue2 != null)
				{
					progressionValue2.CopyFrom(progressionValue);
				}
				else
				{
					Log.Error("ProgressionValues missing {0}", new object[] { progressionValue.Name });
					progressionValue2 = new ProgressionValue();
					progressionValue2.CopyFrom(progressionValue);
					progression.ProgressionValues.Add(progressionValue.Name, progressionValue2);
				}
			}
		}
		if (b > 2)
		{
			progression.ExpDeficit = _br.ReadInt32();
		}
		progression.SetupData();
		return progression;
	}

	// Token: 0x060052FD RID: 21245 RVA: 0x001FE48C File Offset: 0x001FC68C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupData()
	{
		foreach (KeyValuePair<string, ProgressionClass> keyValuePair in Progression.ProgressionClasses)
		{
			string name = keyValuePair.Value.Name;
			if (!this.ProgressionValues.Contains(name))
			{
				ProgressionValue progressionValue = new ProgressionValue(name)
				{
					Level = keyValuePair.Value.MinLevel,
					CostForNextLevel = keyValuePair.Value.CalculatedCostForLevel(this.Level + 1)
				};
				this.ProgressionValues.Add(name, progressionValue);
			}
		}
		this.ProgressionValueQuickList.Clear();
		foreach (KeyValuePair<int, ProgressionValue> keyValuePair2 in this.ProgressionValues.Dict)
		{
			this.ProgressionValueQuickList.Add(keyValuePair2.Value);
		}
		this.eventList.Clear();
		this.passiveEffects.Clear();
		for (int i = 0; i < this.ProgressionValueQuickList.Count; i++)
		{
			ProgressionValue progressionValue2 = this.ProgressionValueQuickList[i];
			ProgressionClass progressionClass = progressionValue2.ProgressionClass;
			if (progressionClass.HasEvents())
			{
				this.eventList.Add(progressionValue2);
			}
			MinEffectController effects = progressionClass.Effects;
			if (effects != null)
			{
				HashSet<PassiveEffects> passivesIndex = effects.PassivesIndex;
				if (passivesIndex != null)
				{
					foreach (PassiveEffects passiveEffects in passivesIndex)
					{
						List<ProgressionValue> list;
						if (!this.passiveEffects.TryGetValue(passiveEffects, out list))
						{
							list = new List<ProgressionValue>();
							this.passiveEffects.Add(passiveEffects, list);
						}
						list.Add(progressionValue2);
					}
				}
			}
		}
	}

	// Token: 0x060052FE RID: 21246 RVA: 0x001FE674 File Offset: 0x001FC874
	public void ClearProgressionClassLinks()
	{
		if (this.ProgressionValueQuickList == null)
		{
			return;
		}
		foreach (ProgressionValue progressionValue in this.ProgressionValueQuickList)
		{
			if (progressionValue != null)
			{
				progressionValue.ClearProgressionClassLink();
			}
		}
		this.SetupData();
	}

	// Token: 0x060052FF RID: 21247 RVA: 0x001FE6DC File Offset: 0x001FC8DC
	public static void Cleanup()
	{
		if (Progression.ProgressionClasses != null)
		{
			Progression.ProgressionClasses.Clear();
		}
	}

	// Token: 0x06005300 RID: 21248 RVA: 0x001FE6F0 File Offset: 0x001FC8F0
	public void ResetProgression(bool _resetSkills = true, bool _resetBooks = false, bool _resetCrafting = false)
	{
		int num = 0;
		int i = 0;
		while (i < this.ProgressionValueQuickList.Count)
		{
			ProgressionValue progressionValue = this.ProgressionValueQuickList[i];
			ProgressionClass progressionClass = progressionValue.ProgressionClass;
			if (!progressionClass.IsBook)
			{
				goto IL_0039;
			}
			if (_resetBooks)
			{
				progressionValue.Level = 0;
				goto IL_0039;
			}
			IL_01B1:
			i++;
			continue;
			IL_0039:
			if (progressionClass.IsCrafting)
			{
				if (!_resetCrafting)
				{
					goto IL_01B1;
				}
				progressionValue.Level = 1;
			}
			if (!_resetSkills)
			{
				goto IL_01B1;
			}
			if (progressionClass.IsAttribute)
			{
				if (progressionValue.Level > 1)
				{
					for (int j = 2; j <= progressionValue.Level; j++)
					{
						num += progressionClass.CalculatedCostForLevel(j);
					}
					progressionValue.Level = 1;
					goto IL_01B1;
				}
				goto IL_01B1;
			}
			else
			{
				if (progressionClass.IsPerk && progressionValue.Level > 0)
				{
					for (int k = 1; k <= progressionValue.Level; k++)
					{
						num += progressionClass.CalculatedCostForLevel(k);
					}
					progressionValue.Level = 0;
					EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
					PlayerPerkProgressionEventData playerPerkProgressionEventData = new PlayerPerkProgressionEventData
					{
						ServerId = Helper.GetServerId(),
						SaveId = Helper.GetSaveId(),
						ProgressionEventTS = DateTime.UtcNow.ToString("O"),
						ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
						PlayerLevel = entityPlayerLocal.Progression.Level,
						PerkSection = progressionClass.Parent.ParentName,
						PerkSubSection = null,
						PerkParent = progressionClass.ParentName,
						PerkName = progressionClass.Name,
						ChangeType = "Unlearn",
						ChangeSource = (entityPlayerLocal.IsDead() ? "PlayerPermaDeath" : "DrinkJarGrandpasForgettingElixir")
					};
					ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerPerkProgressionEventData);
					goto IL_01B1;
				}
				goto IL_01B1;
			}
		}
		EntityPlayerLocal entityPlayerLocal2 = this.parent as EntityPlayerLocal;
		if (entityPlayerLocal2 != null)
		{
			entityPlayerLocal2.PlayerUI.xui.Recipes.RefreshTrackedRecipe();
		}
		EntityPlayerLocal entityPlayerLocal3 = this.parent as EntityPlayerLocal;
		int skillPoints = this.SkillPoints;
		this.SkillPoints += num;
		if (entityPlayerLocal3 && !Helper.IsEditingOrPlaytesting() && num != 0)
		{
			PlayerSkillPointEventData playerSkillPointEventData = new PlayerSkillPointEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				TransactionTimestamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayerLocal3.totalTimePlayed * 60f),
				PlayerLevel = entityPlayerLocal3.Progression.Level,
				InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
				TransactionType = "Refund",
				Amount = num,
				BalanceBefore = skillPoints,
				BalanceAfter = this.SkillPoints,
				TotalSpent = this.SkillPoints,
				SPSource = (entityPlayerLocal3.IsDead() ? "PlayerPermaDeath" : "DrinkJarGrandpasForgettingElixir"),
				SPTarget = null,
				SPTargetDetail = null,
				SPTargerLevel = null
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerSkillPointEventData);
		}
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x001FEA14 File Offset: 0x001FCC14
	public void RefreshPerks(string attribute)
	{
		for (int i = 0; i < this.ProgressionValueQuickList.Count; i++)
		{
			ProgressionValue progressionValue = this.ProgressionValueQuickList[i];
			ProgressionClass progressionClass = progressionValue.ProgressionClass;
			if (progressionClass.IsPerk && (attribute == "" || attribute.EqualsCaseInsensitive(progressionClass.ParentName)))
			{
				progressionValue.CalculatedLevel(this.parent);
			}
		}
	}

	// Token: 0x06005302 RID: 21250 RVA: 0x001FEA7C File Offset: 0x001FCC7C
	public void UpdateForSandbox()
	{
		foreach (ProgressionClass progressionClass in Progression.ProgressionClasses.Values)
		{
			if (progressionClass.DisplayDataList != null)
			{
				progressionClass.UpdateForSandbox();
			}
		}
	}

	// Token: 0x06005303 RID: 21251 RVA: 0x001FEADC File Offset: 0x001FCCDC
	public int GetAllTimeSkillPoint()
	{
		int num = 0;
		for (int i = 0; i < this.ProgressionValueQuickList.Count; i++)
		{
			ProgressionValue progressionValue = this.ProgressionValueQuickList[i];
			ProgressionClass progressionClass = progressionValue.ProgressionClass;
			if (progressionClass.IsAttribute)
			{
				for (int j = 2; j <= progressionValue.Level; j++)
				{
					num += progressionClass.CalculatedCostForLevel(j);
				}
			}
			else if (progressionClass.IsPerk)
			{
				for (int k = 1; k <= progressionValue.Level; k++)
				{
					num += progressionClass.CalculatedCostForLevel(k);
				}
			}
		}
		return this.SkillPoints + num;
	}

	// Token: 0x040040AD RID: 16557
	public const byte cVersion = 3;

	// Token: 0x040040AE RID: 16558
	public static int BaseExpToLevel;

	// Token: 0x040040AF RID: 16559
	public static int ClampExpCostAtLevel;

	// Token: 0x040040B0 RID: 16560
	public static float ExpMultiplier;

	// Token: 0x040040B1 RID: 16561
	public static int MaxLevel;

	// Token: 0x040040B2 RID: 16562
	public static int SkillPointsPerLevelXML;

	// Token: 0x040040B3 RID: 16563
	public static int SkillPointsGainRate;

	// Token: 0x040040B4 RID: 16564
	public static int SkillPointsPerLevel;

	// Token: 0x040040B5 RID: 16565
	public static float SkillPointMultiplier;

	// Token: 0x040040B6 RID: 16566
	public static Dictionary<string, ProgressionClass> ProgressionClasses;

	// Token: 0x040040B7 RID: 16567
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryNameIdMapping ProgressionNameIds = new DictionaryNameIdMapping();

	// Token: 0x040040B8 RID: 16568
	public bool bProgressionStatsChanged;

	// Token: 0x040040B9 RID: 16569
	public int ExpToNextLevel;

	// Token: 0x040040BA RID: 16570
	public int ExpDeficit;

	// Token: 0x040040BB RID: 16571
	public int Level = 1;

	// Token: 0x040040BC RID: 16572
	public int SkillPoints;

	// Token: 0x040040BD RID: 16573
	public static float XPGain = 1f;

	// Token: 0x040040BE RID: 16574
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ExpDeficitGained;

	// Token: 0x040040BF RID: 16575
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryNameId<ProgressionValue> ProgressionValues = new DictionaryNameId<ProgressionValue>(Progression.ProgressionNameIds);

	// Token: 0x040040C0 RID: 16576
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> ProgressionValueQuickList = new List<ProgressionValue>();

	// Token: 0x040040C1 RID: 16577
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> eventList = new List<ProgressionValue>();

	// Token: 0x040040C2 RID: 16578
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, List<ProgressionValue>> passiveEffects = new Dictionary<PassiveEffects, List<ProgressionValue>>();

	// Token: 0x040040C3 RID: 16579
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive parent;

	// Token: 0x040040C4 RID: 16580
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global>[] xpFastTags;

	// Token: 0x040040C5 RID: 16581
	public static Progression.ShowXPTypes ShowXPType = Progression.ShowXPTypes.All;

	// Token: 0x040040C6 RID: 16582
	[PublicizedFrom(EAccessModifier.Private)]
	public float timer = 1f;

	// Token: 0x02000AE6 RID: 2790
	public enum XPTypes
	{
		// Token: 0x040040C8 RID: 16584
		Kill,
		// Token: 0x040040C9 RID: 16585
		Harvesting,
		// Token: 0x040040CA RID: 16586
		Upgrading,
		// Token: 0x040040CB RID: 16587
		Crafting,
		// Token: 0x040040CC RID: 16588
		Selling,
		// Token: 0x040040CD RID: 16589
		Quest,
		// Token: 0x040040CE RID: 16590
		Looting,
		// Token: 0x040040CF RID: 16591
		Party,
		// Token: 0x040040D0 RID: 16592
		Other,
		// Token: 0x040040D1 RID: 16593
		Repairing,
		// Token: 0x040040D2 RID: 16594
		Debug,
		// Token: 0x040040D3 RID: 16595
		Max
	}

	// Token: 0x02000AE7 RID: 2791
	public enum SkillTransactionType
	{
		// Token: 0x040040D5 RID: 16597
		Earn,
		// Token: 0x040040D6 RID: 16598
		Spend,
		// Token: 0x040040D7 RID: 16599
		Refund
	}

	// Token: 0x02000AE8 RID: 2792
	public enum ShowXPTypes
	{
		// Token: 0x040040D9 RID: 16601
		All,
		// Token: 0x040040DA RID: 16602
		BarOnly,
		// Token: 0x040040DB RID: 16603
		NotificationsOnly,
		// Token: 0x040040DC RID: 16604
		None
	}
}
