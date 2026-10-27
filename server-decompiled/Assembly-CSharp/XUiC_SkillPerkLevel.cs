using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audio;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

// Token: 0x02001087 RID: 4231
[Preserve]
public class XUiC_SkillPerkLevel : XUiController
{
	// Token: 0x17000FD3 RID: 4051
	// (set) Token: 0x06008614 RID: 34324 RVA: 0x00333ABE File Offset: 0x00331CBE
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

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x06008615 RID: 34325 RVA: 0x00333AD7 File Offset: 0x00331CD7
	// (set) Token: 0x06008616 RID: 34326 RVA: 0x00333ADF File Offset: 0x00331CDF
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

	// Token: 0x17000FD5 RID: 4053
	// (set) Token: 0x06008617 RID: 34327 RVA: 0x00333AF8 File Offset: 0x00331CF8
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

	// Token: 0x17000FD6 RID: 4054
	// (set) Token: 0x06008618 RID: 34328 RVA: 0x00333B11 File Offset: 0x00331D11
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

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x06008619 RID: 34329 RVA: 0x00333B2A File Offset: 0x00331D2A
	public XUiV_Button BtnBuy
	{
		get
		{
			return this.btnBuy;
		}
	}

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x0600861A RID: 34330 RVA: 0x00332FE3 File Offset: 0x003311E3
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

	// Token: 0x0600861B RID: 34331 RVA: 0x00333B32 File Offset: 0x00331D32
	public override void Init()
	{
		base.Init();
		this.btnBuy = (XUiV_Button)base.GetChildById("btnBuy").ViewComponent;
		this.btnBuy.Controller.OnPress += this.btnBuy_OnPress;
	}

	// Token: 0x0600861C RID: 34332 RVA: 0x00333B74 File Offset: 0x00331D74
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
		int skillPoints = entityPlayer.Progression.SkillPoints;
		int num2 = this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.level);
		entityPlayer.Progression.SkillPoints -= num2;
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
		if (!Helper.IsEditingOrPlaytesting())
		{
			PlayerSkillPointEventData playerSkillPointEventData = new PlayerSkillPointEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				TransactionTimestamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayer.totalTimePlayed * 60f),
				PlayerLevel = entityPlayer.Progression.Level,
				InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
				TransactionType = "Spend",
				Amount = num2,
				BalanceBefore = skillPoints,
				BalanceAfter = entityPlayer.Progression.SkillPoints,
				TotalSpent = entityPlayer.Progression.GetAllTimeSkillPoint(),
				SPSource = null,
				SPTarget = "perk_purchase",
				SPTargetDetail = this.CurrentSkill.Name,
				SPTargerLevel = new int?(this.CurrentSkill.Level)
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerSkillPointEventData);
			MinEffectGroup minEffectGroup = this.CurrentSkill.ProgressionClass.Effects.EffectGroups.FirstOrDefault<MinEffectGroup>((MinEffectGroup list) => list.EffectDescriptions.Count > 1);
			string text = ((minEffectGroup != null) ? minEffectGroup.EffectDescriptions[this.CurrentSkill.Level - 1].GetDescriptionKey : null);
			PlayerPerkProgressionEventData playerPerkProgressionEventData = new PlayerPerkProgressionEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				ProgressionEventTS = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayer.totalTimePlayed * 60f),
				PlayerLevel = entityPlayer.Progression.Level,
				PerkSection = this.CurrentSkill.ProgressionClass.Parent.ParentName,
				PerkSubSection = (this.CurrentSkill.ProgressionClass.ParentName.Equals("skillgeneralperks") ? null : this.CurrentSkill.ProgressionClass.ParentName),
				PerkParent = this.CurrentSkill.ProgressionClass.Name,
				PerkName = text,
				ChangeType = "Learn",
				ChangeSource = "skill_spent"
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerPerkProgressionEventData);
		}
		base.WindowGroup.Controller.RefreshBindingsSelfAndChildren();
		base.WindowGroup.Controller.SetAllChildrenDirty();
		entityPlayer.bPlayerStatsChanged = true;
	}

	// Token: 0x0600861D RID: 34333 RVA: 0x00333F64 File Offset: 0x00332164
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

	// Token: 0x0600861E RID: 34334 RVA: 0x00334088 File Offset: 0x00332288
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

	// Token: 0x0600861F RID: 34335 RVA: 0x0032E850 File Offset: 0x0032CA50
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x040063A6 RID: 25510
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x040063A7 RID: 25511
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x040063A8 RID: 25512
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x040063A9 RID: 25513
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x040063AA RID: 25514
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x040063AB RID: 25515
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_nerfed;

	// Token: 0x040063AC RID: 25516
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_buffed;

	// Token: 0x040063AD RID: 25517
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x040063AE RID: 25518
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x040063AF RID: 25519
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 5;

	// Token: 0x040063B0 RID: 25520
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x040063B1 RID: 25521
	public XUiV_Button btnBuy;

	// Token: 0x040063B2 RID: 25522
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> groupPointCostFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiSkillBuy", false, null), _i, (_i != 1) ? Localization.Get("xuiSkillPoints", false, null) : Localization.Get("xuiSkillPoint", false, null)));

	// Token: 0x040063B3 RID: 25523
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeMultiplicationFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, (_f < 0f) ? _f.ToCultureInvariantString("0.#") : ("+" + _f.ToCultureInvariantString("0.#"))));

	// Token: 0x040063B4 RID: 25524
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeDivisionFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}%", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040063B5 RID: 25525
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeAdditionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: +{1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040063B6 RID: 25526
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x040063B7 RID: 25527
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x040063B8 RID: 25528
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> attributeLockedFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format("{0}: {1}", _s1, _s2));

	// Token: 0x040063B9 RID: 25529
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
