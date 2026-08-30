using System;
using System.Text;
using UnityEngine.Scripting;

// Token: 0x02001059 RID: 4185
[Preserve]
public class XUiC_SkillBookLevel : XUiController
{
	// Token: 0x17000F76 RID: 3958
	// (set) Token: 0x060084EF RID: 34031 RVA: 0x003297BC File Offset: 0x003279BC
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

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x060084F0 RID: 34032 RVA: 0x003297D5 File Offset: 0x003279D5
	// (set) Token: 0x060084F1 RID: 34033 RVA: 0x003297DD File Offset: 0x003279DD
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

	// Token: 0x17000F78 RID: 3960
	// (set) Token: 0x060084F2 RID: 34034 RVA: 0x003297F6 File Offset: 0x003279F6
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

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x060084F3 RID: 34035 RVA: 0x0032980F File Offset: 0x00327A0F
	// (set) Token: 0x060084F4 RID: 34036 RVA: 0x00329817 File Offset: 0x00327A17
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

	// Token: 0x17000F7A RID: 3962
	// (set) Token: 0x060084F5 RID: 34037 RVA: 0x00329827 File Offset: 0x00327A27
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

	// Token: 0x17000F7B RID: 3963
	// (set) Token: 0x060084F6 RID: 34038 RVA: 0x00329840 File Offset: 0x00327A40
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

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060084F7 RID: 34039 RVA: 0x003290B8 File Offset: 0x003272B8
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

	// Token: 0x060084F8 RID: 34040 RVA: 0x0032985C File Offset: 0x00327A5C
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

	// Token: 0x060084F9 RID: 34041 RVA: 0x00329980 File Offset: 0x00327B80
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

	// Token: 0x060084FA RID: 34042 RVA: 0x00329F1C File Offset: 0x0032811C
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsNavigatable = true;
	}

	// Token: 0x060084FB RID: 34043 RVA: 0x00328E30 File Offset: 0x00327030
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x040062A4 RID: 25252
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x040062A5 RID: 25253
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x040062A6 RID: 25254
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x040062A7 RID: 25255
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x040062A8 RID: 25256
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x040062A9 RID: 25257
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x040062AA RID: 25258
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x040062AB RID: 25259
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x040062AC RID: 25260
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue perk;

	// Token: 0x040062AD RID: 25261
	[PublicizedFrom(EAccessModifier.Private)]
	public int volume;

	// Token: 0x040062AE RID: 25262
	[PublicizedFrom(EAccessModifier.Private)]
	public bool completionReward;

	// Token: 0x040062AF RID: 25263
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 10;

	// Token: 0x040062B0 RID: 25264
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x040062B1 RID: 25265
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x040062B2 RID: 25266
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x040062B3 RID: 25267
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040062B4 RID: 25268
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040062B5 RID: 25269
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040062B6 RID: 25270
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040062B7 RID: 25271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x040062B8 RID: 25272
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
