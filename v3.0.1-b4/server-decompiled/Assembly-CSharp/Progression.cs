using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000AB8 RID: 2744
public class Progression
{
	// Token: 0x0600521F RID: 21023 RVA: 0x001F68BC File Offset: 0x001F4ABC
	public Progression()
	{
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
	}

	// Token: 0x06005220 RID: 21024 RVA: 0x001F6928 File Offset: 0x001F4B28
	public Progression(EntityAlive _parent)
	{
		this.parent = _parent;
		this.ExpToNextLevel = this.getExpForLevel((float)(this.Level + 1));
		this.SetupData();
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x001F69A0 File Offset: 0x001F4BA0
	public Dictionary<int, ProgressionValue> GetDict()
	{
		return this.ProgressionValues.Dict;
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x001F69AD File Offset: 0x001F4BAD
	public static int CalcId(string _name)
	{
		return Progression.ProgressionNameIds.Add(_name);
	}

	// Token: 0x06005223 RID: 21027 RVA: 0x001F69BA File Offset: 0x001F4BBA
	public ProgressionValue GetProgressionValue(int _id)
	{
		return this.ProgressionValues.Get(_id);
	}

	// Token: 0x06005224 RID: 21028 RVA: 0x001F69C8 File Offset: 0x001F4BC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getLevelFloat()
	{
		return (float)this.Level + (1f - (float)this.ExpToNextLevel / (float)this.GetExpForNextLevel());
	}

	// Token: 0x06005225 RID: 21029 RVA: 0x001F69E7 File Offset: 0x001F4BE7
	[PublicizedFrom(EAccessModifier.Protected)]
	public int getExpForLevel(float _level)
	{
		return (int)Math.Min((float)Progression.BaseExpToLevel * Mathf.Pow(Progression.ExpMultiplier, _level), 2.1474836E+09f);
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x001F6A06 File Offset: 0x001F4C06
	public int GetLevel()
	{
		return this.Level;
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x001F6A0E File Offset: 0x001F4C0E
	public int GetExpForNextLevel()
	{
		return this.getExpForLevel((float)Mathf.Clamp(this.Level + 1, 0, Progression.ClampExpCostAtLevel));
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001F6A2A File Offset: 0x001F4C2A
	public float GetLevelProgressPercentage()
	{
		return this.getLevelFloat() - (float)this.Level;
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001F6A3C File Offset: 0x001F4C3C
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

	// Token: 0x0600522A RID: 21034 RVA: 0x001F6AA0 File Offset: 0x001F4CA0
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

	// Token: 0x0600522B RID: 21035 RVA: 0x001F6B30 File Offset: 0x001F4D30
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
		this.parent.Buffs.SetCustomVar("_expdeficit", (float)this.ExpDeficit, true, CVarOperation.set);
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x001F6B9C File Offset: 0x001F4D9C
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

	// Token: 0x0600522D RID: 21037 RVA: 0x001F6BEC File Offset: 0x001F4DEC
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

	// Token: 0x0600522E RID: 21038 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnDeath()
	{
	}

	// Token: 0x0600522F RID: 21039 RVA: 0x001F6D7C File Offset: 0x001F4F7C
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

	// Token: 0x06005230 RID: 21040 RVA: 0x001F6E18 File Offset: 0x001F5018
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

	// Token: 0x06005231 RID: 21041 RVA: 0x001F6EE0 File Offset: 0x001F50E0
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

	// Token: 0x06005232 RID: 21042 RVA: 0x001F70C0 File Offset: 0x001F52C0
	public void SpendSkillPoints(int _points, string _progressionName)
	{
		ProgressionValue progressionValue = this.GetProgressionValue(_progressionName);
		if (progressionValue != null && progressionValue.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
		{
			this.addProgressionCurrency(_points, progressionValue);
		}
	}

	// Token: 0x06005233 RID: 21043 RVA: 0x001F70EE File Offset: 0x001F52EE
	public ProgressionValue GetProgressionValue(string _progressionName)
	{
		return this.ProgressionValues.Get(_progressionName);
	}

	// Token: 0x06005234 RID: 21044 RVA: 0x001F70FC File Offset: 0x001F52FC
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

	// Token: 0x06005235 RID: 21045 RVA: 0x001F7170 File Offset: 0x001F5370
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

	// Token: 0x06005236 RID: 21046 RVA: 0x001F7230 File Offset: 0x001F5430
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

	// Token: 0x06005237 RID: 21047 RVA: 0x001F728C File Offset: 0x001F548C
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

	// Token: 0x06005238 RID: 21048 RVA: 0x001F7338 File Offset: 0x001F5538
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

	// Token: 0x06005239 RID: 21049 RVA: 0x001F73A0 File Offset: 0x001F55A0
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

	// Token: 0x0600523A RID: 21050 RVA: 0x001F74B0 File Offset: 0x001F56B0
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

	// Token: 0x0600523B RID: 21051 RVA: 0x001F7698 File Offset: 0x001F5898
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

	// Token: 0x0600523C RID: 21052 RVA: 0x001F7700 File Offset: 0x001F5900
	public static void Cleanup()
	{
		if (Progression.ProgressionClasses != null)
		{
			Progression.ProgressionClasses.Clear();
		}
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001F7714 File Offset: 0x001F5914
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

	// Token: 0x0600523E RID: 21054 RVA: 0x001F782C File Offset: 0x001F5A2C
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

	// Token: 0x0600523F RID: 21055 RVA: 0x001F7894 File Offset: 0x001F5A94
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

	// Token: 0x04003FBE RID: 16318
	public const byte cVersion = 3;

	// Token: 0x04003FBF RID: 16319
	public static int BaseExpToLevel;

	// Token: 0x04003FC0 RID: 16320
	public static int ClampExpCostAtLevel;

	// Token: 0x04003FC1 RID: 16321
	public static float ExpMultiplier;

	// Token: 0x04003FC2 RID: 16322
	public static int MaxLevel;

	// Token: 0x04003FC3 RID: 16323
	public static int SkillPointsPerLevelXML;

	// Token: 0x04003FC4 RID: 16324
	public static int SkillPointsGainRate;

	// Token: 0x04003FC5 RID: 16325
	public static int SkillPointsPerLevel;

	// Token: 0x04003FC6 RID: 16326
	public static float SkillPointMultiplier;

	// Token: 0x04003FC7 RID: 16327
	public static Dictionary<string, ProgressionClass> ProgressionClasses;

	// Token: 0x04003FC8 RID: 16328
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryNameIdMapping ProgressionNameIds = new DictionaryNameIdMapping();

	// Token: 0x04003FC9 RID: 16329
	public bool bProgressionStatsChanged;

	// Token: 0x04003FCA RID: 16330
	public int ExpToNextLevel;

	// Token: 0x04003FCB RID: 16331
	public int ExpDeficit;

	// Token: 0x04003FCC RID: 16332
	public int Level = 1;

	// Token: 0x04003FCD RID: 16333
	public int SkillPoints;

	// Token: 0x04003FCE RID: 16334
	public static float XPGain = 1f;

	// Token: 0x04003FCF RID: 16335
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ExpDeficitGained;

	// Token: 0x04003FD0 RID: 16336
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryNameId<ProgressionValue> ProgressionValues = new DictionaryNameId<ProgressionValue>(Progression.ProgressionNameIds);

	// Token: 0x04003FD1 RID: 16337
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> ProgressionValueQuickList = new List<ProgressionValue>();

	// Token: 0x04003FD2 RID: 16338
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> eventList = new List<ProgressionValue>();

	// Token: 0x04003FD3 RID: 16339
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, List<ProgressionValue>> passiveEffects = new Dictionary<PassiveEffects, List<ProgressionValue>>();

	// Token: 0x04003FD4 RID: 16340
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive parent;

	// Token: 0x04003FD5 RID: 16341
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global>[] xpFastTags;

	// Token: 0x04003FD6 RID: 16342
	public static Progression.ShowXPTypes ShowXPType = Progression.ShowXPTypes.All;

	// Token: 0x04003FD7 RID: 16343
	[PublicizedFrom(EAccessModifier.Private)]
	public float timer = 1f;

	// Token: 0x02000AB9 RID: 2745
	public enum XPTypes
	{
		// Token: 0x04003FD9 RID: 16345
		Kill,
		// Token: 0x04003FDA RID: 16346
		Harvesting,
		// Token: 0x04003FDB RID: 16347
		Upgrading,
		// Token: 0x04003FDC RID: 16348
		Crafting,
		// Token: 0x04003FDD RID: 16349
		Selling,
		// Token: 0x04003FDE RID: 16350
		Quest,
		// Token: 0x04003FDF RID: 16351
		Looting,
		// Token: 0x04003FE0 RID: 16352
		Party,
		// Token: 0x04003FE1 RID: 16353
		Other,
		// Token: 0x04003FE2 RID: 16354
		Repairing,
		// Token: 0x04003FE3 RID: 16355
		Debug,
		// Token: 0x04003FE4 RID: 16356
		Max
	}

	// Token: 0x02000ABA RID: 2746
	public enum ShowXPTypes
	{
		// Token: 0x04003FE6 RID: 16358
		All,
		// Token: 0x04003FE7 RID: 16359
		BarOnly,
		// Token: 0x04003FE8 RID: 16360
		NotificationsOnly,
		// Token: 0x04003FE9 RID: 16361
		None
	}
}
