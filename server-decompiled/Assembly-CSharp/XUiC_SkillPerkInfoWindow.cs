using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001075 RID: 4213
[Preserve]
public class XUiC_SkillPerkInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x060085AE RID: 34222 RVA: 0x003311CF File Offset: 0x0032F3CF
	public ProgressionValue CurrentSkill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsPerk)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x060085AF RID: 34223 RVA: 0x00331202 File Offset: 0x0032F402
	// (set) Token: 0x060085B0 RID: 34224 RVA: 0x0033120A File Offset: 0x0032F40A
	public int HoveredLevel
	{
		get
		{
			return this.hoveredLevel;
		}
		set
		{
			if (this.hoveredLevel != value)
			{
				this.hoveredLevel = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x060085B1 RID: 34225 RVA: 0x00331224 File Offset: 0x0032F424
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_SkillPerkLevel>(this.levelEntries);
		int num = 1;
		foreach (XUiC_SkillPerkLevel xuiC_SkillPerkLevel in this.levelEntries)
		{
			xuiC_SkillPerkLevel.ListIndex = num - 1;
			xuiC_SkillPerkLevel.Level = num++;
			xuiC_SkillPerkLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
			xuiC_SkillPerkLevel.MaxEntriesWithoutPaging = this.levelEntries.Count;
			xuiC_SkillPerkLevel.OnScroll += this.Entry_OnScroll;
			xuiC_SkillPerkLevel.OnHover += this.Entry_OnHover;
			xuiC_SkillPerkLevel.btnBuy.Controller.OnHover += this.Entry_OnHover;
		}
		this.actionItemList = base.GetChildByType<XUiC_ItemActionList>();
		this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += this.Pager_OnPageChanged;
		}
	}

	// Token: 0x060085B2 RID: 34226 RVA: 0x00331344 File Offset: 0x0032F544
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnHover(XUiController _sender, bool _isOver)
	{
		XUiC_SkillPerkLevel xuiC_SkillPerkLevel = _sender as XUiC_SkillPerkLevel;
		if (xuiC_SkillPerkLevel == null)
		{
			xuiC_SkillPerkLevel = _sender.Parent as XUiC_SkillPerkLevel;
		}
		if (_isOver && xuiC_SkillPerkLevel != null)
		{
			this.HoveredLevel = xuiC_SkillPerkLevel.Level;
			return;
		}
		this.HoveredLevel = -1;
	}

	// Token: 0x060085B3 RID: 34227 RVA: 0x00331384 File Offset: 0x0032F584
	public void SkillChanged()
	{
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength((this.CurrentSkill != null && this.CurrentSkill.ProgressionClass.MaxLevel > this.levelEntries.Count) ? (this.CurrentSkill.ProgressionClass.MaxLevel - 1) : 0, this.skillsPerPage);
		}
		XUiC_Paging xuiC_Paging2 = this.pager;
		if (xuiC_Paging2 != null)
		{
			xuiC_Paging2.Reset();
		}
		this.IsDirty = true;
	}

	// Token: 0x060085B4 RID: 34228 RVA: 0x003313FC File Offset: 0x0032F5FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSkill()
	{
		if (this.CurrentSkill != null && this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		XUiC_SkillEntry entryForSkill = this.windowGroup.Controller.GetChildByType<XUiC_SkillList>().GetEntryForSkill(this.CurrentSkill);
		XUiC_Paging xuiC_Paging = this.pager;
		int num = ((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0) * this.skillsPerPage + 1;
		foreach (XUiC_SkillPerkLevel xuiC_SkillPerkLevel in this.levelEntries)
		{
			xuiC_SkillPerkLevel.Level = num++;
			xuiC_SkillPerkLevel.IsDirty = true;
			if (entryForSkill != null)
			{
				xuiC_SkillPerkLevel.btnBuy.NavLeftTarget = entryForSkill.ViewComponent;
			}
		}
	}

	// Token: 0x060085B5 RID: 34229 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060085B6 RID: 34230 RVA: 0x003314C4 File Offset: 0x0032F6C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x060085B7 RID: 34231 RVA: 0x003314F1 File Offset: 0x0032F6F1
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x060085B8 RID: 34232 RVA: 0x00331518 File Offset: 0x0032F718
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			this.UpdateSkill();
			base.RefreshBindings();
		}
		if (base.ViewComponent.IsActiveInHierarchy && this.CurrentSkill != null && !this.xui.playerUI.windowManager.IsInputActive() && ((PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.Inspect.WasPressed) || (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.DPad_Up.WasPressed)))
		{
			foreach (XUiC_SkillPerkLevel xuiC_SkillPerkLevel in this.levelEntries)
			{
				if (xuiC_SkillPerkLevel.CurrentSkill != null && xuiC_SkillPerkLevel.Level == this.CurrentSkill.Level + 1)
				{
					xuiC_SkillPerkLevel.btnBuy.Controller.Pressed(-1);
					break;
				}
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060085B9 RID: 34233 RVA: 0x00331654 File Offset: 0x0032F854
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "hidden_entries_with_paging")
		{
			this.hiddenEntriesWithPaging = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			foreach (XUiC_SkillPerkLevel xuiC_SkillPerkLevel in this.levelEntries)
			{
				if (xuiC_SkillPerkLevel != null)
				{
					xuiC_SkillPerkLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
				}
			}
			this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060085BA RID: 34234 RVA: 0x003316F0 File Offset: 0x0032F8F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1912580562U)
		{
			if (num <= 464048759U)
			{
				if (num != 443815844U)
				{
					if (num == 464048759U)
					{
						if (_bindingName == "alwaysfalse")
						{
							_value = "false";
							return true;
						}
					}
				}
				else if (_bindingName == "skillLevel")
				{
					_value = ((this.CurrentSkill != null) ? this.skillLevelFormatter.Format(this.CurrentSkill.GetCalculatedLevel(entityPlayer)) : "0");
					return true;
				}
			}
			else if (num != 1275709072U)
			{
				if (num != 1283949528U)
				{
					if (num == 1912580562U)
					{
						if (_bindingName == "buycost")
						{
							_value = "-- PTS";
							if (this.CurrentSkill != null && this.CurrentSkill.CalculatedLevel(entityPlayer) < this.CurrentSkill.ProgressionClass.MaxLevel)
							{
								if (this.CurrentSkill.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
								{
									_value = this.buyCostFormatter.Format(this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.CurrentSkill.CalculatedLevel(entityPlayer) + 1));
								}
								else
								{
									_value = this.expCostFormatter.Format((int)((1f - this.CurrentSkill.PercToNextLevel) * (float)this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.CurrentSkill.CalculatedLevel(entityPlayer) + 1)));
								}
							}
							return true;
						}
					}
				}
				else if (_bindingName == "currentlevel")
				{
					_value = Localization.Get("xuiSkillLevel", false, null);
					return true;
				}
			}
			else if (_bindingName == "maxSkillLevel")
			{
				_value = ((this.CurrentSkill != null) ? this.maxSkillLevelFormatter.Format((float)ProgressionClass.GetCalculatedMaxLevel(entityPlayer, this.CurrentSkill)) : "0");
				return true;
			}
		}
		else if (num <= 3268933568U)
		{
			if (num != 2606420134U)
			{
				if (num == 3268933568U)
				{
					if (_bindingName == "showPaging")
					{
						_value = (this.CurrentSkill != null && this.CurrentSkill.ProgressionClass.MaxLevel > this.levelEntries.Count).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "groupdescription")
			{
				_value = ((this.CurrentSkill != null) ? Localization.Get(this.CurrentSkill.ProgressionClass.DescKey, false, null) : "");
				return true;
			}
		}
		else if (num != 3504806855U)
		{
			if (num != 4010384093U)
			{
				if (num == 4294521801U)
				{
					if (_bindingName == "detailsdescription")
					{
						if (this.CurrentSkill != null && this.hoveredLevel != -1 && this.CurrentSkill.ProgressionClass.MaxLevel >= this.hoveredLevel)
						{
							using (List<MinEffectGroup>.Enumerator enumerator = this.CurrentSkill.ProgressionClass.Effects.EffectGroups.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									MinEffectGroup minEffectGroup = enumerator.Current;
									if (minEffectGroup.EffectDescriptions != null)
									{
										for (int i = 0; i < minEffectGroup.EffectDescriptions.Count; i++)
										{
											if (this.hoveredLevel >= minEffectGroup.EffectDescriptions[i].MinLevel && this.hoveredLevel <= minEffectGroup.EffectDescriptions[i].MaxLevel)
											{
												_value = ((!string.IsNullOrEmpty(minEffectGroup.EffectDescriptions[i].LongDescription)) ? minEffectGroup.EffectDescriptions[i].LongDescription : minEffectGroup.EffectDescriptions[i].Description);
												return true;
											}
										}
									}
								}
								return true;
							}
						}
						_value = "";
						return true;
					}
				}
			}
			else if (_bindingName == "groupicon")
			{
				_value = ((this.CurrentSkill != null) ? this.CurrentSkill.ProgressionClass.Icon : "ui_game_symbol_skills");
				return true;
			}
		}
		else if (_bindingName == "groupname")
		{
			_value = ((this.CurrentSkill != null) ? Localization.Get(this.CurrentSkill.ProgressionClass.NameKey, false, null) : "Skill Info");
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04006356 RID: 25430
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04006357 RID: 25431
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x04006358 RID: 25432
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillPerkLevel> levelEntries = new List<XUiC_SkillPerkLevel>();

	// Token: 0x04006359 RID: 25433
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400635A RID: 25434
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x0400635B RID: 25435
	[PublicizedFrom(EAccessModifier.Private)]
	public int hoveredLevel = -1;

	// Token: 0x0400635C RID: 25436
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400635D RID: 25437
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400635E RID: 25438
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> buyCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("xuiSkillPoints", false, null));

	// Token: 0x0400635F RID: 25439
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> expCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("RewardExp_keyword", false, null));

	// Token: 0x04006360 RID: 25440
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => _s + ": " + _f.ToCultureInvariantString("0.#") + (_b ? "%" : ""));

	// Token: 0x04006361 RID: 25441
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => _s + ": " + _f.ToCultureInvariantString("0.#"));

	// Token: 0x04006362 RID: 25442
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
