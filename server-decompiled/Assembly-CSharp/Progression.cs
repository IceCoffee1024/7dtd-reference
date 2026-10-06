using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
public class Progression
{
	// Token: 0x060052BE RID: 21182 RVA: 0x001FC5B4 File Offset: 0x001FA7B4
	public Progression()
	{
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001FC620 File Offset: 0x001FA820
	public Progression(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
		this.SetupData();
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001FC698 File Offset: 0x001FA898
	public Dictionary<int, ProgressionValue> GetDict()
	{
		return this.ProgressionValues.Dict;
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001FC6A5 File Offset: 0x001FA8A5
	public static int CalcId(string _name)
	{
		return Progression.ProgressionNameIds.Add(_name);
	}

	// Token: 0x060052C2 RID: 21186 RVA: 0x001FC6B2 File Offset: 0x001FA8B2
	public ProgressionValue GetProgressionValue(int _id)
	{
		return this.ProgressionValues.Get(_id);
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x001FC6C0 File Offset: 0x001FA8C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getLevelFloat()
	{
		return (float)this.Level + (1f - (float)this.ExpToNextLevel / (float)this.GetExpForNextLevel());
	}

	// Token: 0x060052C4 RID: 21188 RVA: 0x001FC6DF File Offset: 0x001FA8DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public int getExpForLevel(float _level)
	{
		return (int)Math.Min((float)Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _level), 2.1474836E+09f);
	}

	// Token: 0x060052C5 RID: 21189 RVA: 0x001FC6FE File Offset: 0x001FA8FE
	public int GetLevel()
	{
		return this.Level;
	}

	// Token: 0x060052C6 RID: 21190 RVA: 0x001FC706 File Offset: 0x001FA906
	public int GetExpForNextLevel()
	{
		return this.getExpForLevel((float)Mathf.Clamp(this.Level + 1, 0, Progression.ClampExpCostAtLevel));
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x001FC722 File Offset: 0x001FA922
	public float GetLevelProgressPercentage()
	{
		return this.getLevelFloat() - (float)this.Level;
	}

	// Token: 0x060052C8 RID: 21192 RVA: 0x001FC734 File Offset: 0x001FA934
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

	// Token: 0x060052C9 RID: 21193 RVA: 0x001FC798 File Offset: 0x001FA998
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

	// Token: 0x060052CA RID: 21194 RVA: 0x001FC828 File Offset: 0x001FAA28
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

	// Token: 0x060052CB RID: 21195 RVA: 0x001FC894 File Offset: 0x001FAA94
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

	// Token: 0x060052CC RID: 21196 RVA: 0x001FC8E4 File Offset: 0x001FAAE4
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
		}
		return (int)num;
	}

	// Token: 0x060052CD RID: 21197 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDeath()
	{
	}

	// Token: 0x060052CE RID: 21198 RVA: 0x001FCA74 File Offset: 0x001FAC74
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

	// Token: 0x060052CF RID: 21199 RVA: 0x001FCB10 File Offset: 0x001FAD10
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

	// Token: 0x060052D0 RID: 21200 RVA: 0x001FCBD8 File Offset: 0x001FADD8
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

	// Token: 0x060052D1 RID: 21201 RVA: 0x001FCDB8 File Offset: 0x001FAFB8
	public void SpendSkillPoints(int _points, string _progressionName)
	{
		ProgressionValue progressionValue = this.GetProgressionValue(_progressionName);
		if (progressionValue != null && progressionValue.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
		{
			this.addProgressionCurrency(_points, progressionValue);
		}
	}

	// Token: 0x060052D2 RID: 21202 RVA: 0x001FCDE6 File Offset: 0x001FAFE6
	public ProgressionValue GetProgressionValue(string _progressionName)
	{
		return this.ProgressionValues.Get(_progressionName);
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001FCDF4 File Offset: 0x001FAFF4
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

	// Token: 0x060052D4 RID: 21204 RVA: 0x001FCE68 File Offset: 0x001FB068
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

	// Token: 0x060052D5 RID: 21205 RVA: 0x001FCF28 File Offset: 0x001FB128
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

	// Token: 0x060052D6 RID: 21206 RVA: 0x001FCF84 File Offset: 0x001FB184
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

	// Token: 0x060052D7 RID: 21207 RVA: 0x001FD030 File Offset: 0x001FB230
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

	// Token: 0x060052D8 RID: 21208 RVA: 0x001FD098 File Offset: 0x001FB298
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

	// Token: 0x060052D9 RID: 21209 RVA: 0x001FD1A8 File Offset: 0x001FB3A8
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

	// Token: 0x060052DA RID: 21210 RVA: 0x001FD390 File Offset: 0x001FB590
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

	// Token: 0x060052DB RID: 21211 RVA: 0x001FD3F8 File Offset: 0x001FB5F8
	public static void Cleanup()
	{
		if (Progression.ProgressionClasses != null)
		{
			Progression.ProgressionClasses.Clear();
		}
	}

	// Token: 0x060052DC RID: 21212 RVA: 0x001FD40C File Offset: 0x001FB60C
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
				goto IL_0034;
			}
			if (_resetBooks)
			{
				progressionValue.Level = 0;
				goto IL_0034;
			}
			IL_00C3:
			i++;
			continue;
			IL_0034:
			if (progressionClass.IsCrafting)
			{
				if (!_resetCrafting)
				{
					goto IL_00C3;
				}
				progressionValue.Level = 1;
			}
			if (!_resetSkills)
			{
				goto IL_00C3;
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
					goto IL_00C3;
				}
				goto IL_00C3;
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
					goto IL_00C3;
				}
				goto IL_00C3;
			}
		}
		EntityPlayerLocal entityPlayerLocal = this.parent as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.PlayerUI.xui.Recipes.RefreshTrackedRecipe();
		}
		this.SkillPoints += num;
	}

	// Token: 0x060052DD RID: 21213 RVA: 0x001FD524 File Offset: 0x001FB724
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

	// Token: 0x060052DE RID: 21214 RVA: 0x001FD58C File Offset: 0x001FB78C
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

	// Token: 0x0400408C RID: 16524
	public const byte cVersion = 3;

	// Token: 0x0400408D RID: 16525
	public static int BaseExpToLevel;

	// Token: 0x0400408E RID: 16526
	public static int ClampExpCostAtLevel;

	// Token: 0x0400408F RID: 16527
	public static float ExpMultiplier;

	// Token: 0x04004090 RID: 16528
	public static int MaxLevel;

	// Token: 0x04004091 RID: 16529
	public static int SkillPointsPerLevelXML;

	// Token: 0x04004092 RID: 16530
	public static int SkillPointsGainRate;

	// Token: 0x04004093 RID: 16531
	public static int SkillPointsPerLevel;

	// Token: 0x04004094 RID: 16532
	public static float SkillPointMultiplier;

	// Token: 0x04004095 RID: 16533
	public static Dictionary<string, ProgressionClass> ProgressionClasses;

	// Token: 0x04004096 RID: 16534
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryNameIdMapping ProgressionNameIds = new DictionaryNameIdMapping();

	// Token: 0x04004097 RID: 16535
	public bool bProgressionStatsChanged;

	// Token: 0x04004098 RID: 16536
	public int ExpToNextLevel;

	// Token: 0x04004099 RID: 16537
	public int ExpDeficit;

	// Token: 0x0400409A RID: 16538
	public int Level = 1;

	// Token: 0x0400409B RID: 16539
	public int SkillPoints;

	// Token: 0x0400409C RID: 16540
	public static float XPGain = 1f;

	// Token: 0x0400409D RID: 16541
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ExpDeficitGained;

	// Token: 0x0400409E RID: 16542
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryNameId<ProgressionValue> ProgressionValues = new DictionaryNameId<ProgressionValue>(Progression.ProgressionNameIds);

	// Token: 0x0400409F RID: 16543
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> ProgressionValueQuickList = new List<ProgressionValue>();

	// Token: 0x040040A0 RID: 16544
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> eventList = new List<ProgressionValue>();

	// Token: 0x040040A1 RID: 16545
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, List<ProgressionValue>> passiveEffects = new Dictionary<PassiveEffects, List<ProgressionValue>>();

	// Token: 0x040040A2 RID: 16546
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive parent;

	// Token: 0x040040A3 RID: 16547
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global>[] xpFastTags;

	// Token: 0x040040A4 RID: 16548
	public static Progression.ShowXPTypes ShowXPType = Progression.ShowXPTypes.All;

	// Token: 0x040040A5 RID: 16549
	[PublicizedFrom(EAccessModifier.Private)]
	public float timer = 1f;

	// Token: 0x02000ADC RID: 2780
	public enum XPTypes
	{
		// Token: 0x040040A7 RID: 16551
		Kill,
		// Token: 0x040040A8 RID: 16552
		Harvesting,
		// Token: 0x040040A9 RID: 16553
		Upgrading,
		// Token: 0x040040AA RID: 16554
		Crafting,
		// Token: 0x040040AB RID: 16555
		Selling,
		// Token: 0x040040AC RID: 16556
		Quest,
		// Token: 0x040040AD RID: 16557
		Looting,
		// Token: 0x040040AE RID: 16558
		Party,
		// Token: 0x040040AF RID: 16559
		Other,
		// Token: 0x040040B0 RID: 16560
		Repairing,
		// Token: 0x040040B1 RID: 16561
		Debug,
		// Token: 0x040040B2 RID: 16562
		Max
	}

	// Token: 0x02000ADD RID: 2781
	public enum ShowXPTypes
	{
		// Token: 0x040040B4 RID: 16564
		All,
		// Token: 0x040040B5 RID: 16565
		BarOnly,
		// Token: 0x040040B6 RID: 16566
		NotificationsOnly,
		// Token: 0x040040B7 RID: 16567
		None
	}
}
