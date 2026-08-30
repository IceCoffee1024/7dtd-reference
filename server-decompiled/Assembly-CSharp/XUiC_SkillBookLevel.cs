using System;
using System.Text;
using UnityEngine.Scripting;

// Token: 0x02001040 RID: 4160
[Preserve]
public class XUiC_SkillBookLevel : XUiController
{
	// Token: 0x17000F3D RID: 3901
	// (set) Token: 0x060083FD RID: 33789 RVA: 0x00324F94 File Offset: 0x00323194
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

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060083FE RID: 33790 RVA: 0x00324FAD File Offset: 0x003231AD
	// (set) Token: 0x060083FF RID: 33791 RVA: 0x00324FB5 File Offset: 0x003231B5
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

	// Token: 0x17000F3F RID: 3903
	// (set) Token: 0x06008400 RID: 33792 RVA: 0x00324FCE File Offset: 0x003231CE
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

	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x06008401 RID: 33793 RVA: 0x00324FE7 File Offset: 0x003231E7
	// (set) Token: 0x06008402 RID: 33794 RVA: 0x00324FEF File Offset: 0x003231EF
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

	// Token: 0x17000F41 RID: 3905
	// (set) Token: 0x06008403 RID: 33795 RVA: 0x00324FFF File Offset: 0x003231FF
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

	// Token: 0x17000F42 RID: 3906
	// (set) Token: 0x06008404 RID: 33796 RVA: 0x00325018 File Offset: 0x00323218
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

	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x06008405 RID: 33797 RVA: 0x00324890 File Offset: 0x00322A90
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

	// Token: 0x06008406 RID: 33798 RVA: 0x00325034 File Offset: 0x00323234
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

	// Token: 0x06008407 RID: 33799 RVA: 0x00325158 File Offset: 0x00323358
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

	// Token: 0x06008408 RID: 33800 RVA: 0x003256F4 File Offset: 0x003238F4
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsNavigatable = true;
	}

	// Token: 0x06008409 RID: 33801 RVA: 0x00324608 File Offset: 0x00322808
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x040061DD RID: 25053
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x040061DE RID: 25054
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x040061DF RID: 25055
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x040061E0 RID: 25056
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x040061E1 RID: 25057
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x040061E2 RID: 25058
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x040061E3 RID: 25059
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x040061E4 RID: 25060
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x040061E5 RID: 25061
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue perk;

	// Token: 0x040061E6 RID: 25062
	[PublicizedFrom(EAccessModifier.Private)]
	public int volume;

	// Token: 0x040061E7 RID: 25063
	[PublicizedFrom(EAccessModifier.Private)]
	public bool completionReward;

	// Token: 0x040061E8 RID: 25064
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 10;

	// Token: 0x040061E9 RID: 25065
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x040061EA RID: 25066
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x040061EB RID: 25067
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x040061EC RID: 25068
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040061ED RID: 25069
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040061EE RID: 25070
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040061EF RID: 25071
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040061F0 RID: 25072
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x040061F1 RID: 25073
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
