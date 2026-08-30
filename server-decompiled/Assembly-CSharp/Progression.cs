using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000AC9 RID: 2761
public class Progression
{
	// Token: 0x060052A5 RID: 21157 RVA: 0x001F9354 File Offset: 0x001F7554
	public Progression()
	{
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
	}

	// Token: 0x060052A6 RID: 21158 RVA: 0x001F93C0 File Offset: 0x001F75C0
	public Progression(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
		this.SetupData();
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x001F9438 File Offset: 0x001F7638
	public Dictionary<int, ProgressionValue> GetDict()
	{
		return this.ProgressionValues.Dict;
	}

	// Token: 0x060052A8 RID: 21160 RVA: 0x001F9445 File Offset: 0x001F7645
	public static int CalcId(string _name)
	{
		return Progression.ProgressionNameIds.Add(_name);
	}

	// Token: 0x060052A9 RID: 21161 RVA: 0x001F9452 File Offset: 0x001F7652
	public ProgressionValue GetProgressionValue(int _id)
	{
		return this.ProgressionValues.Get(_id);
	}

	// Token: 0x060052AA RID: 21162 RVA: 0x001F9460 File Offset: 0x001F7660
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getLevelFloat()
	{
		return (float)this.Level + (1f - (float)this.ExpToNextLevel / (float)this.GetExpForNextLevel());
	}

	// Token: 0x060052AB RID: 21163 RVA: 0x001F947F File Offset: 0x001F767F
	[PublicizedFrom(EAccessModifier.Protected)]
	public int getExpForLevel(float _level)
	{
		return (int)Math.Min((float)Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _level), 2.1474836E+09f);
	}

	// Token: 0x060052AC RID: 21164 RVA: 0x001F949E File Offset: 0x001F769E
	public int GetLevel()
	{
		return this.Level;
	}

	// Token: 0x060052AD RID: 21165 RVA: 0x001F94A6 File Offset: 0x001F76A6
	public int GetExpForNextLevel()
	{
		return this.getExpForLevel((float)Mathf.Clamp(this.Level + 1, 0, Progression.ClampExpCostAtLevel));
	}

	// Token: 0x060052AE RID: 21166 RVA: 0x001F94C2 File Offset: 0x001F76C2
	public float GetLevelProgressPercentage()
	{
		return this.getLevelFloat() - (float)this.Level;
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x001F94D4 File Offset: 0x001F76D4
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

	// Token: 0x060052B0 RID: 21168 RVA: 0x001F9538 File Offset: 0x001F7738
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

	// Token: 0x060052B1 RID: 21169 RVA: 0x001F95C8 File Offset: 0x001F77C8
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

	// Token: 0x060052B2 RID: 21170 RVA: 0x001F9634 File Offset: 0x001F7834
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

	// Token: 0x060052B3 RID: 21171 RVA: 0x001F9684 File Offset: 0x001F7884
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

	// Token: 0x060052B4 RID: 21172 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDeath()
	{
	}

	// Token: 0x060052B5 RID: 21173 RVA: 0x001F9814 File Offset: 0x001F7A14
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

	// Token: 0x060052B6 RID: 21174 RVA: 0x001F98B0 File Offset: 0x001F7AB0
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

	// Token: 0x060052B7 RID: 21175 RVA: 0x001F9978 File Offset: 0x001F7B78
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

	// Token: 0x060052B8 RID: 21176 RVA: 0x001F9B58 File Offset: 0x001F7D58
	public void SpendSkillPoints(int _points, string _progressionName)
	{
		ProgressionValue progressionValue = this.GetProgressionValue(_progressionName);
		if (progressionValue != null && progressionValue.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
		{
			this.addProgressionCurrency(_points, progressionValue);
		}
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001F9B86 File Offset: 0x001F7D86
	public ProgressionValue GetProgressionValue(string _progressionName)
	{
		return this.ProgressionValues.Get(_progressionName);
	}

	// Token: 0x060052BA RID: 21178 RVA: 0x001F9B94 File Offset: 0x001F7D94
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

	// Token: 0x060052BB RID: 21179 RVA: 0x001F9C08 File Offset: 0x001F7E08
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

	// Token: 0x060052BC RID: 21180 RVA: 0x001F9CC8 File Offset: 0x001F7EC8
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

	// Token: 0x060052BD RID: 21181 RVA: 0x001F9D24 File Offset: 0x001F7F24
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

	// Token: 0x060052BE RID: 21182 RVA: 0x001F9DD0 File Offset: 0x001F7FD0
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

	// Token: 0x060052BF RID: 21183 RVA: 0x001F9E38 File Offset: 0x001F8038
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

	// Token: 0x060052C0 RID: 21184 RVA: 0x001F9F48 File Offset: 0x001F8148
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

	// Token: 0x060052C1 RID: 21185 RVA: 0x001FA130 File Offset: 0x001F8330
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

	// Token: 0x060052C2 RID: 21186 RVA: 0x001FA198 File Offset: 0x001F8398
	public static void Cleanup()
	{
		if (Progression.ProgressionClasses != null)
		{
			Progression.ProgressionClasses.Clear();
		}
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x001FA1AC File Offset: 0x001F83AC
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

	// Token: 0x060052C4 RID: 21188 RVA: 0x001FA2C4 File Offset: 0x001F84C4
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

	// Token: 0x060052C5 RID: 21189 RVA: 0x001FA32C File Offset: 0x001F852C
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

	// Token: 0x0400404F RID: 16463
	public const byte cVersion = 3;

	// Token: 0x04004050 RID: 16464
	public static int BaseExpToLevel;

	// Token: 0x04004051 RID: 16465
	public static int ClampExpCostAtLevel;

	// Token: 0x04004052 RID: 16466
	public static float ExpMultiplier;

	// Token: 0x04004053 RID: 16467
	public static int MaxLevel;

	// Token: 0x04004054 RID: 16468
	public static int SkillPointsPerLevelXML;

	// Token: 0x04004055 RID: 16469
	public static int SkillPointsGainRate;

	// Token: 0x04004056 RID: 16470
	public static int SkillPointsPerLevel;

	// Token: 0x04004057 RID: 16471
	public static float SkillPointMultiplier;

	// Token: 0x04004058 RID: 16472
	public static Dictionary<string, ProgressionClass> ProgressionClasses;

	// Token: 0x04004059 RID: 16473
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryNameIdMapping ProgressionNameIds = new DictionaryNameIdMapping();

	// Token: 0x0400405A RID: 16474
	public bool bProgressionStatsChanged;

	// Token: 0x0400405B RID: 16475
	public int ExpToNextLevel;

	// Token: 0x0400405C RID: 16476
	public int ExpDeficit;

	// Token: 0x0400405D RID: 16477
	public int Level = 1;

	// Token: 0x0400405E RID: 16478
	public int SkillPoints;

	// Token: 0x0400405F RID: 16479
	public static float XPGain = 1f;

	// Token: 0x04004060 RID: 16480
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ExpDeficitGained;

	// Token: 0x04004061 RID: 16481
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryNameId<ProgressionValue> ProgressionValues = new DictionaryNameId<ProgressionValue>(Progression.ProgressionNameIds);

	// Token: 0x04004062 RID: 16482
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> ProgressionValueQuickList = new List<ProgressionValue>();

	// Token: 0x04004063 RID: 16483
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> eventList = new List<ProgressionValue>();

	// Token: 0x04004064 RID: 16484
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, List<ProgressionValue>> passiveEffects = new Dictionary<PassiveEffects, List<ProgressionValue>>();

	// Token: 0x04004065 RID: 16485
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive parent;

	// Token: 0x04004066 RID: 16486
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global>[] xpFastTags;

	// Token: 0x04004067 RID: 16487
	public static Progression.ShowXPTypes ShowXPType = Progression.ShowXPTypes.All;

	// Token: 0x04004068 RID: 16488
	[PublicizedFrom(EAccessModifier.Private)]
	public float timer = 1f;

	// Token: 0x02000ACA RID: 2762
	public enum XPTypes
	{
		// Token: 0x0400406A RID: 16490
		Kill,
		// Token: 0x0400406B RID: 16491
		Harvesting,
		// Token: 0x0400406C RID: 16492
		Upgrading,
		// Token: 0x0400406D RID: 16493
		Crafting,
		// Token: 0x0400406E RID: 16494
		Selling,
		// Token: 0x0400406F RID: 16495
		Quest,
		// Token: 0x04004070 RID: 16496
		Looting,
		// Token: 0x04004071 RID: 16497
		Party,
		// Token: 0x04004072 RID: 16498
		Other,
		// Token: 0x04004073 RID: 16499
		Repairing,
		// Token: 0x04004074 RID: 16500
		Debug,
		// Token: 0x04004075 RID: 16501
		Max
	}

	// Token: 0x02000ACB RID: 2763
	public enum ShowXPTypes
	{
		// Token: 0x04004077 RID: 16503
		All,
		// Token: 0x04004078 RID: 16504
		BarOnly,
		// Token: 0x04004079 RID: 16505
		NotificationsOnly,
		// Token: 0x0400407A RID: 16506
		None
	}
}
