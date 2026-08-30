using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001064 RID: 4196
[Preserve]
public class XUiC_SkillPerkInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06008568 RID: 34152 RVA: 0x0032D5C3 File Offset: 0x0032B7C3
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

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06008569 RID: 34153 RVA: 0x0032D5F6 File Offset: 0x0032B7F6
	// (set) Token: 0x0600856A RID: 34154 RVA: 0x0032D5FE File Offset: 0x0032B7FE
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

	// Token: 0x0600856B RID: 34155 RVA: 0x0032D618 File Offset: 0x0032B818
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

	// Token: 0x0600856C RID: 34156 RVA: 0x0032D738 File Offset: 0x0032B938
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

	// Token: 0x0600856D RID: 34157 RVA: 0x0032D778 File Offset: 0x0032B978
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

	// Token: 0x0600856E RID: 34158 RVA: 0x0032D7F0 File Offset: 0x0032B9F0
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

	// Token: 0x0600856F RID: 34159 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008570 RID: 34160 RVA: 0x0032D8B8 File Offset: 0x0032BAB8
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

	// Token: 0x06008571 RID: 34161 RVA: 0x0032D8E5 File Offset: 0x0032BAE5
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x06008572 RID: 34162 RVA: 0x0032D90C File Offset: 0x0032BB0C
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

	// Token: 0x06008573 RID: 34163 RVA: 0x0032DA48 File Offset: 0x0032BC48
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

	// Token: 0x06008574 RID: 34164 RVA: 0x0032DAE4 File Offset: 0x0032BCE4
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

	// Token: 0x04006311 RID: 25361
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04006312 RID: 25362
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x04006313 RID: 25363
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillPerkLevel> levelEntries = new List<XUiC_SkillPerkLevel>();

	// Token: 0x04006314 RID: 25364
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006315 RID: 25365
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x04006316 RID: 25366
	[PublicizedFrom(EAccessModifier.Private)]
	public int hoveredLevel = -1;

	// Token: 0x04006317 RID: 25367
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006318 RID: 25368
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006319 RID: 25369
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> buyCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("xuiSkillPoints", false, null));

	// Token: 0x0400631A RID: 25370
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> expCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("RewardExp_keyword", false, null));

	// Token: 0x0400631B RID: 25371
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => _s + ": " + _f.ToCultureInvariantString("0.#") + (_b ? "%" : ""));

	// Token: 0x0400631C RID: 25372
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => _s + ": " + _f.ToCultureInvariantString("0.#"));

	// Token: 0x0400631D RID: 25373
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
