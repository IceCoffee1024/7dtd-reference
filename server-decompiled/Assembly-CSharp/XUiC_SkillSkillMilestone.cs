using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200107B RID: 4219
[Preserve]
public class XUiC_SkillSkillMilestone : XUiController
{
	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x060085E7 RID: 34279 RVA: 0x00332862 File Offset: 0x00330A62
	public ProgressionValue CurrentSkill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsSkill)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x060085E8 RID: 34280 RVA: 0x00333348 File Offset: 0x00331548
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSkill()
	{
		this.effectLines.Clear();
		if (this.CurrentSkill != null)
		{
			int levelGoal = this.LevelGoal;
			foreach (MinEffectGroup minEffectGroup in this.CurrentSkill.ProgressionClass.Effects.EffectGroups)
			{
				if (minEffectGroup.EffectDescriptions != null)
				{
					for (int i = 0; i < minEffectGroup.EffectDescriptions.Count; i++)
					{
						if (levelGoal >= minEffectGroup.EffectDescriptions[i].MinLevel && levelGoal <= minEffectGroup.EffectDescriptions[i].MaxLevel)
						{
							this.effectLines.Add(minEffectGroup.EffectDescriptions[i].Description);
							return;
						}
					}
				}
				foreach (PassiveEffect passiveEffect in minEffectGroup.PassiveEffects)
				{
					float num = 0f;
					float num2 = 1f;
					int entityClass = this.xui.playerUI.entityPlayer.entityClass;
					if (EntityClass.list.ContainsKey(entityClass) && EntityClass.list[entityClass].Effects != null)
					{
						EntityClass.list[entityClass].Effects.ModifyValue(this.xui.playerUI.entityPlayer, passiveEffect.Type, ref num, ref num2, 0f, EntityClass.list[entityClass].Tags, 1);
					}
					float num3 = num;
					passiveEffect.ModifyValue(this.xui.playerUI.entityPlayer, (float)levelGoal, ref num, ref num2, passiveEffect.Tags, 1);
					if (num != num3 || num2 != 1f)
					{
						if (num == num3)
						{
							this.effectLines.Add(this.attributeSubtractionFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), 100f * num2, true));
						}
						else
						{
							this.effectLines.Add(this.attributeSetValueFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), num2 * num));
						}
					}
				}
			}
		}
	}

	// Token: 0x060085E9 RID: 34281 RVA: 0x003335A8 File Offset: 0x003317A8
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "color_lbl_available")
		{
			this.color_lbl_available = _value;
			return true;
		}
		if (!(_name == "color_lbl_locked"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.color_lbl_locked = _value;
		return true;
	}

	// Token: 0x060085EA RID: 34282 RVA: 0x003335E0 File Offset: 0x003317E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		bool flag = this.CurrentSkill != null && this.CurrentSkill.ProgressionClass.MaxLevel >= this.LevelGoal;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1566407741U)
		{
			if (num <= 1240702784U)
			{
				if (num != 12870938U)
				{
					if (num == 1240702784U)
					{
						if (_bindingName == "effectsCol1")
						{
							int num2 = this.effectLines.Count;
							if (this.effectLines.Count > 3)
							{
								num2 = this.effectLines.Count / 2 + this.effectLines.Count % 2;
							}
							_value = "";
							for (int i = 0; i < num2; i++)
							{
								if (_value.Length > 0)
								{
									_value += "\n";
								}
								_value += this.effectLines[i];
							}
							return true;
						}
					}
				}
				else if (_bindingName == "color_fg")
				{
					if (flag && this.CurrentSkill.CalculatedLevel(entityPlayer) >= this.LevelGoal && !this.CurrentSkill.IsLocked(entityPlayer))
					{
						_value = this.color_lbl_available;
					}
					else
					{
						_value = this.color_lbl_locked;
					}
					return true;
				}
			}
			else if (num != 1291035641U)
			{
				if (num == 1566407741U)
				{
					if (_bindingName == "hasentry")
					{
						_value = flag.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "effectsCol2")
			{
				int num2 = this.effectLines.Count;
				if (this.effectLines.Count > 3)
				{
					num2 = this.effectLines.Count / 2 + this.effectLines.Count % 2;
				}
				_value = "";
				for (int j = num2; j < this.effectLines.Count; j++)
				{
					if (_value.Length > 0)
					{
						_value += "\n";
					}
					_value += this.effectLines[j];
				}
				return true;
			}
		}
		else if (num <= 3406319174U)
		{
			if (num != 2610554845U)
			{
				if (num == 3406319174U)
				{
					if (_bindingName == "progress")
					{
						if (flag)
						{
							if (this.CurrentSkill.CalculatedLevel(entityPlayer) < this.LevelStart)
							{
								_value = "0.0";
							}
							else if (this.CurrentSkill.CalculatedLevel(entityPlayer) >= this.LevelGoal)
							{
								_value = "1.0";
							}
							else
							{
								float num3 = (float)this.CurrentSkill.CalculatedLevel(entityPlayer) + this.CurrentSkill.PercToNextLevel;
								_value = ((num3 - (float)this.LevelStart) / (float)(this.LevelGoal - this.LevelStart)).ToCultureInvariantString();
							}
						}
						else
						{
							_value = "0.0";
						}
						return true;
					}
				}
			}
			else if (_bindingName == "level")
			{
				_value = this.LevelGoal.ToString();
				return true;
			}
		}
		else if (num != 3499124306U)
		{
			if (num != 3862959600U)
			{
				if (num == 4105239451U)
				{
					if (_bindingName == "icontooltip")
					{
						if (flag && this.CurrentSkill.CalculatedMaxLevel(entityPlayer) < this.LevelGoal)
						{
							ProgressionValue progressionValue = entityPlayer.Progression.GetProgressionValue(this.CurrentSkill.ProgressionClass.ParentName);
							_value = string.Format(Localization.Get("xuiSkillRequirement", false, null), Localization.Get(progressionValue.ProgressionClass.NameKey, false, null), Mathf.CeilToInt((float)this.LevelGoal / this.CurrentSkill.ProgressionClass.ParentMaxLevelRatio));
						}
						else
						{
							_value = "";
						}
						return true;
					}
				}
			}
			else if (_bindingName == "icon")
			{
				if (flag && this.CurrentSkill.CalculatedLevel(entityPlayer) >= this.LevelGoal)
				{
					_value = "ui_game_symbol_check";
				}
				else
				{
					_value = "ui_game_symbol_lock";
				}
				return true;
			}
		}
		else if (_bindingName == "iconvisible")
		{
			_value = (flag && (this.CurrentSkill.CalculatedLevel(entityPlayer) >= this.LevelGoal || this.CurrentSkill.CalculatedMaxLevel(entityPlayer) < this.LevelGoal)).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060085EB RID: 34283 RVA: 0x00333A3F File Offset: 0x00331C3F
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			this.UpdateSkill();
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x04006396 RID: 25494
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x04006397 RID: 25495
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x04006398 RID: 25496
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> effectLines = new List<string>();

	// Token: 0x04006399 RID: 25497
	public int LevelStart;

	// Token: 0x0400639A RID: 25498
	public int LevelGoal;

	// Token: 0x0400639B RID: 25499
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x0400639C RID: 25500
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x0400639D RID: 25501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x0400639E RID: 25502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x0400639F RID: 25503
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040063A0 RID: 25504
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));
}
