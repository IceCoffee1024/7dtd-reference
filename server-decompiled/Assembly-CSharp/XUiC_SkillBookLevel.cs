using System;
using System.Text;
using UnityEngine.Scripting;

// Token: 0x0200107A RID: 4218
[Preserve]
public class XUiC_SkillBookLevel : XUiController
{
	// Token: 0x17000FB7 RID: 4023
	// (set) Token: 0x06008587 RID: 34183 RVA: 0x0032F1DC File Offset: 0x0032D3DC
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

	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x06008588 RID: 34184 RVA: 0x0032F1F5 File Offset: 0x0032D3F5
	// (set) Token: 0x06008589 RID: 34185 RVA: 0x0032F1FD File Offset: 0x0032D3FD
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

	// Token: 0x17000FB9 RID: 4025
	// (set) Token: 0x0600858A RID: 34186 RVA: 0x0032F216 File Offset: 0x0032D416
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

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x0600858B RID: 34187 RVA: 0x0032F22F File Offset: 0x0032D42F
	// (set) Token: 0x0600858C RID: 34188 RVA: 0x0032F237 File Offset: 0x0032D437
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

	// Token: 0x17000FBB RID: 4027
	// (set) Token: 0x0600858D RID: 34189 RVA: 0x0032F247 File Offset: 0x0032D447
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

	// Token: 0x17000FBC RID: 4028
	// (set) Token: 0x0600858E RID: 34190 RVA: 0x0032F260 File Offset: 0x0032D460
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

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x0600858F RID: 34191 RVA: 0x0032EAD8 File Offset: 0x0032CCD8
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

	// Token: 0x06008590 RID: 34192 RVA: 0x0032F27C File Offset: 0x0032D47C
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

	// Token: 0x06008591 RID: 34193 RVA: 0x0032F3A0 File Offset: 0x0032D5A0
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

	// Token: 0x06008592 RID: 34194 RVA: 0x0032F93C File Offset: 0x0032DB3C
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsNavigatable = true;
	}

	// Token: 0x06008593 RID: 34195 RVA: 0x0032E850 File Offset: 0x0032CA50
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x04006327 RID: 25383
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x04006328 RID: 25384
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x04006329 RID: 25385
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x0400632A RID: 25386
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x0400632B RID: 25387
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x0400632C RID: 25388
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x0400632D RID: 25389
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x0400632E RID: 25390
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x0400632F RID: 25391
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue perk;

	// Token: 0x04006330 RID: 25392
	[PublicizedFrom(EAccessModifier.Private)]
	public int volume;

	// Token: 0x04006331 RID: 25393
	[PublicizedFrom(EAccessModifier.Private)]
	public bool completionReward;

	// Token: 0x04006332 RID: 25394
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 10;

	// Token: 0x04006333 RID: 25395
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x04006334 RID: 25396
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x04006335 RID: 25397
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x04006336 RID: 25398
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x04006337 RID: 25399
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x04006338 RID: 25400
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x04006339 RID: 25401
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x0400633A RID: 25402
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x0400633B RID: 25403
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
