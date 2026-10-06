using System;
using System.Text;
using UnityEngine.Scripting;

// Token: 0x0200106A RID: 4202
[Preserve]
public class XUiC_SkillBookLevel : XUiController
{
	// Token: 0x17000FAC RID: 4012
	// (set) Token: 0x06008535 RID: 34101 RVA: 0x0032D3C8 File Offset: 0x0032B5C8
	public int ListIndex
	{
		set
		{
			if (value != this.listIndex)
			{
				this.listIndex = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x06008536 RID: 34102 RVA: 0x0032D3E1 File Offset: 0x0032B5E1
	// (set) Token: 0x06008537 RID: 34103 RVA: 0x0032D3E9 File Offset: 0x0032B5E9
	public ProgressionValue Perk
	{
		get
		{
			return this.perk;
		}
		set
		{
			if (value != this.perk)
			{
				this.perk = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000FAE RID: 4014
	// (set) Token: 0x06008538 RID: 34104 RVA: 0x0032D402 File Offset: 0x0032B602
	public int Volume
	{
		set
		{
			if (value != this.volume)
			{
				this.volume = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x06008539 RID: 34105 RVA: 0x0032D41B File Offset: 0x0032B61B
	// (set) Token: 0x0600853A RID: 34106 RVA: 0x0032D423 File Offset: 0x0032B623
	public bool CompletionReward
	{
		get
		{
			return this.completionReward;
		}
		set
		{
			this.completionReward = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FB0 RID: 4016
	// (set) Token: 0x0600853B RID: 34107 RVA: 0x0032D433 File Offset: 0x0032B633
	public int MaxEntriesWithoutPaging
	{
		set
		{
			if (this.maxEntriesWithoutPaging != value)
			{
				this.maxEntriesWithoutPaging = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000FB1 RID: 4017
	// (set) Token: 0x0600853C RID: 34108 RVA: 0x0032D44C File Offset: 0x0032B64C
	public int HiddenEntriesWithPaging
	{
		set
		{
			if (this.hiddenEntriesWithPaging != value)
			{
				this.hiddenEntriesWithPaging = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x0600853D RID: 34109 RVA: 0x0032CCC4 File Offset: 0x0032AEC4
	public ProgressionValue CurrentSkill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsBookGroup)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x0600853E RID: 34110 RVA: 0x0032D468 File Offset: 0x0032B668
	public override bool ParseAttribute(string _name, string _value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
		if (num <= 1064527410U)
		{
			if (num != 537253509U)
			{
				if (num != 1050472866U)
				{
					if (num == 1064527410U)
					{
						if (_name == "color_bg_available")
						{
							this.color_bg_available = _value;
							return true;
						}
					}
				}
				else if (_name == "color_lbl_nerfed")
				{
					this.color_lbl_nerfed = _value;
					return true;
				}
			}
			else if (_name == "color_bg_locked")
			{
				this.color_bg_locked = _value;
				return true;
			}
		}
		else if (num <= 2715414458U)
		{
			if (num != 2558146821U)
			{
				if (num == 2715414458U)
				{
					if (_name == "color_lbl_buffed")
					{
						this.color_lbl_buffed = _value;
						return true;
					}
				}
			}
			else if (_name == "color_lbl_available")
			{
				this.color_lbl_available = _value;
				return true;
			}
		}
		else if (num != 2973230400U)
		{
			if (num == 3948125712U)
			{
				if (_name == "color_bg_bought")
				{
					this.color_bg_bought = _value;
					return true;
				}
			}
		}
		else if (_name == "color_lbl_locked")
		{
			this.color_lbl_locked = _value;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600853F RID: 34111 RVA: 0x0032D58C File Offset: 0x0032B78C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		bool flag = this.CurrentSkill != null && this.perk != null;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		bool flag2 = false;
		if (flag)
		{
			flag2 = this.perk != null && this.perk.Level > 0;
		}
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2155443558U)
		{
			if (num <= 1128447690U)
			{
				if (num != 12870938U)
				{
					if (num == 1128447690U)
					{
						if (_bindingName == "buycolor")
						{
							if (flag2)
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
				}
				else if (_bindingName == "color_fg")
				{
					if (flag2)
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
			else if (num != 1566407741U)
			{
				if (num != 1782036591U)
				{
					if (num == 2155443558U)
					{
						if (_bindingName == "nothiddenbypager")
						{
							_value = (this.CurrentSkill == null || this.CurrentSkill.ProgressionClass.MaxLevel <= this.maxEntriesWithoutPaging || this.listIndex < this.maxEntriesWithoutPaging - this.hiddenEntriesWithPaging).ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "buyvisible")
				{
					_value = flag.ToString();
					return true;
				}
			}
			else if (_bindingName == "hasentry")
			{
				_value = flag.ToString();
				return true;
			}
		}
		else if (num <= 2610554845U)
		{
			if (num != 2563538258U)
			{
				if (num == 2610554845U)
				{
					if (_bindingName == "level")
					{
						if (this.perk != null)
						{
							_value = (this.completionReward ? "" : this.volume.ToString());
						}
						else
						{
							_value = "";
						}
						return true;
					}
				}
			}
			else if (_bindingName == "iscomplete")
			{
				_value = this.completionReward.ToString();
				return true;
			}
		}
		else if (num != 3185987134U)
		{
			if (num != 3503204070U)
			{
				if (num == 4068915738U)
				{
					if (_bindingName == "buyicon")
					{
						if (flag2)
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
			}
			else if (_bindingName == "color_bg")
			{
				if (flag2)
				{
					_value = this.color_bg_bought;
				}
				else
				{
					_value = this.color_bg_locked;
				}
				return true;
			}
		}
		else if (_bindingName == "text")
		{
			this.effectsStringBuilder.Length = 0;
			if (this.perk == null)
			{
				_value = "";
			}
			int num2 = 1;
			if (flag && this.perk.ProgressionClass != null)
			{
				if (!string.IsNullOrEmpty(this.perk.ProgressionClass.DescKey))
				{
					_value = Localization.Get(this.perk.ProgressionClass.DescKey, false, null);
					return true;
				}
				if (this.perk.ProgressionClass.Effects != null && this.perk.ProgressionClass.Effects.EffectGroups != null)
				{
					foreach (MinEffectGroup minEffectGroup in this.perk.ProgressionClass.Effects.EffectGroups)
					{
						if (minEffectGroup.EffectDescriptions != null)
						{
							for (int i = 0; i < minEffectGroup.EffectDescriptions.Count; i++)
							{
								if (num2 >= minEffectGroup.EffectDescriptions[i].MinLevel && num2 <= minEffectGroup.EffectDescriptions[i].MaxLevel)
								{
									_value = minEffectGroup.EffectDescriptions[i].Description;
									return true;
								}
							}
						}
						foreach (PassiveEffect passiveEffect in minEffectGroup.PassiveEffects)
						{
							float num3 = 0f;
							float num4 = 1f;
							int entityClass = entityPlayer.entityClass;
							if (EntityClass.list.ContainsKey(entityClass) && EntityClass.list[entityClass].Effects != null)
							{
								EntityClass.list[entityClass].Effects.ModifyValue(entityPlayer, passiveEffect.Type, ref num3, ref num4, 0f, EntityClass.list[entityClass].Tags, 1);
							}
							float num5 = num3;
							passiveEffect.ModifyValue(entityPlayer, (float)num2, ref num3, ref num4, passiveEffect.Tags, 1);
							if (num3 != num5 || num4 != 1f)
							{
								if (this.effectsStringBuilder.Length > 0)
								{
									this.effectsStringBuilder.Append(", ");
								}
								if (num3 == num5)
								{
									this.effectsStringBuilder.Append(this.attributeSubtractionFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), 100f * num4, true));
								}
								else
								{
									this.effectsStringBuilder.Append(this.attributeSetValueFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), num4 * num3));
								}
							}
						}
					}
				}
			}
			_value = this.effectsStringBuilder.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06008540 RID: 34112 RVA: 0x0032DB28 File Offset: 0x0032BD28
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsNavigatable = true;
	}

	// Token: 0x06008541 RID: 34113 RVA: 0x0032CA3C File Offset: 0x0032AC3C
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x040062E9 RID: 25321
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x040062EA RID: 25322
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x040062EB RID: 25323
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x040062EC RID: 25324
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x040062ED RID: 25325
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x040062EE RID: 25326
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x040062EF RID: 25327
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x040062F0 RID: 25328
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x040062F1 RID: 25329
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue perk;

	// Token: 0x040062F2 RID: 25330
	[PublicizedFrom(EAccessModifier.Private)]
	public int volume;

	// Token: 0x040062F3 RID: 25331
	[PublicizedFrom(EAccessModifier.Private)]
	public bool completionReward;

	// Token: 0x040062F4 RID: 25332
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 10;

	// Token: 0x040062F5 RID: 25333
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x040062F6 RID: 25334
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x040062F7 RID: 25335
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x040062F8 RID: 25336
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040062F9 RID: 25337
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040062FA RID: 25338
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040062FB RID: 25339
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040062FC RID: 25340
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x040062FD RID: 25341
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
