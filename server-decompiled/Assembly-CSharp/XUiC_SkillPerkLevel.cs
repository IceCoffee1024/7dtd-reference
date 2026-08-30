using System;
using System.Collections.Generic;
using System.Text;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02001066 RID: 4198
[Preserve]
public class XUiC_SkillPerkLevel : XUiController
{
	// Token: 0x17000F92 RID: 3986
	// (set) Token: 0x0600857C RID: 34172 RVA: 0x0032E09E File Offset: 0x0032C29E
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

	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x0600857D RID: 34173 RVA: 0x0032E0B7 File Offset: 0x0032C2B7
	// (set) Token: 0x0600857E RID: 34174 RVA: 0x0032E0BF File Offset: 0x0032C2BF
	public int Level
	{
		get
		{
			return this.level;
		}
		set
		{
			if (value != this.level)
			{
				this.level = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000F94 RID: 3988
	// (set) Token: 0x0600857F RID: 34175 RVA: 0x0032E0D8 File Offset: 0x0032C2D8
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

	// Token: 0x17000F95 RID: 3989
	// (set) Token: 0x06008580 RID: 34176 RVA: 0x0032E0F1 File Offset: 0x0032C2F1
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

	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x06008581 RID: 34177 RVA: 0x0032E10A File Offset: 0x0032C30A
	public XUiV_Button BtnBuy
	{
		get
		{
			return this.btnBuy;
		}
	}

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x06008582 RID: 34178 RVA: 0x0032D5C3 File Offset: 0x0032B7C3
	public ProgressionValue CurrentSkill
	{
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsPerk)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x06008583 RID: 34179 RVA: 0x0032E112 File Offset: 0x0032C312
	public override void Init()
	{
		base.Init();
		this.btnBuy = (XUiV_Button)base.GetChildById("btnBuy").ViewComponent;
		this.btnBuy.Controller.OnPress += this.btnBuy_OnPress;
	}

	// Token: 0x06008584 RID: 34180 RVA: 0x0032E154 File Offset: 0x0032C354
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnBuy_OnPress(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.CurrentSkill.Level + 1 != this.level || this.CurrentSkill.CalculatedMaxLevel(entityPlayer) < this.level)
		{
			return;
		}
		if (!this.CurrentSkill.CanPurchase(entityPlayer, this.level))
		{
			return;
		}
		if (entityPlayer.Progression.SkillPoints < this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.level))
		{
			return;
		}
		if (this.CurrentSkill.CostForNextLevel <= 0)
		{
			return;
		}
		ProgressionValue currentSkill = this.CurrentSkill;
		int num = currentSkill.Level;
		currentSkill.Level = num + 1;
		entityPlayer.Progression.SkillPoints -= this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.level);
		if (this.CurrentSkill.ProgressionClass.IsPerk)
		{
			entityPlayer.MinEventContext.ProgressionValue = this.CurrentSkill;
			entityPlayer.FireEvent(MinEventTypes.onPerkLevelChanged, true);
		}
		this.xui.Recipes.RefreshTrackedRecipe();
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntitySetSkillLevelServer>().Setup(entityPlayer.entityId, this.CurrentSkill.Name, this.CurrentSkill.Level), false);
		}
		QuestEventManager.Current.SpendSkillPoint(this.CurrentSkill);
		Manager.PlayInsidePlayerHead("ui_skill_purchase", -1, 0f, false, false);
		base.WindowGroup.Controller.RefreshBindingsSelfAndChildren();
		base.WindowGroup.Controller.SetAllChildrenDirty();
		entityPlayer.bPlayerStatsChanged = true;
	}

	// Token: 0x06008585 RID: 34181 RVA: 0x0032E2E4 File Offset: 0x0032C4E4
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

	// Token: 0x06008586 RID: 34182 RVA: 0x0032E408 File Offset: 0x0032C608
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		bool flag = this.CurrentSkill != null && this.CurrentSkill.ProgressionClass.MaxLevel >= this.level;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		if (flag)
		{
			flag3 = this.CurrentSkill.Level >= this.level;
			flag2 = this.CurrentSkill.Level + 1 == this.level && this.CurrentSkill.Level + 1 <= this.CurrentSkill.CalculatedMaxLevel(entityPlayer);
			flag4 = !flag3 && this.CurrentSkill.CalculatedLevel(entityPlayer) >= this.level;
			flag5 = flag3 && this.CurrentSkill.CalculatedLevel(entityPlayer) < this.level;
		}
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1782036591U)
		{
			if (num <= 142401450U)
			{
				if (num != 12870938U)
				{
					if (num == 142401450U)
					{
						if (_bindingName == "buytooltip")
						{
							if (flag3 && flag5)
							{
								_value = Localization.Get("xuiSkillNerfedEffect", false, null);
							}
							else if (flag3)
							{
								_value = "";
							}
							else if (flag2)
							{
								int num2 = this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.level);
								_value = ((num2 > 0) ? this.groupPointCostFormatter.Format(num2) : "NA");
							}
							else if (flag)
							{
								_value = "";
								LevelRequirement requirementsForLevel = this.CurrentSkill.ProgressionClass.GetRequirementsForLevel(this.level);
								if (requirementsForLevel.Requirements != null)
								{
									List<string> list = new List<string>();
									requirementsForLevel.Requirements.GetInfoStrings(ref list);
									for (int i = 0; i < list.Count; i++)
									{
										if (i > 0)
										{
											_value += "\n";
										}
										_value += list[i];
									}
								}
								if (_value == "")
								{
									int num3 = this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.level);
									_value = ((num3 > 0) ? this.groupPointCostFormatter.Format(num3) : "NA");
								}
							}
							else
							{
								_value = "";
							}
							return true;
						}
					}
				}
				else if (_bindingName == "color_fg")
				{
					if (flag3 || flag2)
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
			else if (num != 1128447690U)
			{
				if (num != 1566407741U)
				{
					if (num == 1782036591U)
					{
						if (_bindingName == "buyvisible")
						{
							_value = flag.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "hasentry")
				{
					_value = flag.ToString();
					return true;
				}
			}
			else if (_bindingName == "buycolor")
			{
				if (flag5)
				{
					_value = this.color_lbl_nerfed;
				}
				else if (flag4)
				{
					_value = this.color_lbl_buffed;
				}
				else if (flag3 || flag2)
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
		else if (num <= 2610554845U)
		{
			if (num != 2155443558U)
			{
				if (num == 2610554845U)
				{
					if (_bindingName == "level")
					{
						_value = this.level.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "nothiddenbypager")
			{
				_value = (this.CurrentSkill == null || this.CurrentSkill.ProgressionClass.MaxLevel <= this.maxEntriesWithoutPaging || this.listIndex < this.maxEntriesWithoutPaging - this.hiddenEntriesWithPaging).ToString();
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
						if (flag3)
						{
							_value = "ui_game_symbol_check";
						}
						else if (flag2)
						{
							_value = "ui_game_symbol_shopping_cart";
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
				if (flag3)
				{
					_value = this.color_bg_bought;
				}
				else if (flag2)
				{
					_value = this.color_bg_available;
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
			if (flag)
			{
				foreach (MinEffectGroup minEffectGroup in this.CurrentSkill.ProgressionClass.Effects.EffectGroups)
				{
					if (minEffectGroup.EffectDescriptions != null)
					{
						for (int j = 0; j < minEffectGroup.EffectDescriptions.Count; j++)
						{
							if (this.level >= minEffectGroup.EffectDescriptions[j].MinLevel && this.level <= minEffectGroup.EffectDescriptions[j].MaxLevel)
							{
								_value = minEffectGroup.EffectDescriptions[j].Description;
								return true;
							}
						}
					}
					foreach (PassiveEffect passiveEffect in minEffectGroup.PassiveEffects)
					{
						float num4 = 0f;
						float num5 = 1f;
						int entityClass = entityPlayer.entityClass;
						if (EntityClass.list.ContainsKey(entityClass) && EntityClass.list[entityClass].Effects != null)
						{
							EntityClass.list[entityClass].Effects.ModifyValue(entityPlayer, passiveEffect.Type, ref num4, ref num5, 0f, EntityClass.list[entityClass].Tags, 1);
						}
						float num6 = num4;
						passiveEffect.ModifyValue(entityPlayer, (float)this.level, ref num4, ref num5, passiveEffect.Tags, 1);
						if (num4 != num6 || num5 != 1f)
						{
							if (this.effectsStringBuilder.Length > 0)
							{
								this.effectsStringBuilder.Append(", ");
							}
							if (num4 == num6)
							{
								this.effectsStringBuilder.Append(this.attributeSubtractionFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), 100f * num5, true));
							}
							else
							{
								this.effectsStringBuilder.Append(this.attributeSetValueFormatter.Format(passiveEffect.Type.ToStringCached<PassiveEffects>(), num5 * num4));
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

	// Token: 0x06008587 RID: 34183 RVA: 0x00328E30 File Offset: 0x00327030
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x04006323 RID: 25379
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x04006324 RID: 25380
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x04006325 RID: 25381
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x04006326 RID: 25382
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x04006327 RID: 25383
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x04006328 RID: 25384
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x04006329 RID: 25385
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x0400632A RID: 25386
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x0400632B RID: 25387
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x0400632C RID: 25388
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 5;

	// Token: 0x0400632D RID: 25389
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x0400632E RID: 25390
	public XUiV_Button btnBuy;

	// Token: 0x0400632F RID: 25391
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x04006330 RID: 25392
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x04006331 RID: 25393
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x04006332 RID: 25394
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x04006333 RID: 25395
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x04006334 RID: 25396
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x04006335 RID: 25397
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x04006336 RID: 25398
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
