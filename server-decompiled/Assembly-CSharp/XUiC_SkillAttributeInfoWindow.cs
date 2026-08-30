using System;
using System.Collections.Generic;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001053 RID: 4179
[Preserve]
public class XUiC_SkillAttributeInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x060084B6 RID: 33974 RVA: 0x00327980 File Offset: 0x00325B80
	public ProgressionValue CurrentSkill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsAttribute)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x060084B7 RID: 33975 RVA: 0x003279B3 File Offset: 0x00325BB3
	// (set) Token: 0x060084B8 RID: 33976 RVA: 0x003279BB File Offset: 0x00325BBB
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

	// Token: 0x060084B9 RID: 33977 RVA: 0x003279D4 File Offset: 0x00325BD4
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_SkillAttributeLevel>(this.levelEntries);
		int num = 1;
		foreach (XUiC_SkillAttributeLevel xuiC_SkillAttributeLevel in this.levelEntries)
		{
			xuiC_SkillAttributeLevel.ListIndex = num - 1;
			xuiC_SkillAttributeLevel.Level = num++;
			xuiC_SkillAttributeLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
			xuiC_SkillAttributeLevel.MaxEntriesWithoutPaging = this.levelEntries.Count;
			xuiC_SkillAttributeLevel.OnScroll += this.Entry_OnScroll;
			xuiC_SkillAttributeLevel.OnHover += this.Entry_OnHover;
			xuiC_SkillAttributeLevel.btnBuy.Controller.OnHover += this.Entry_OnHover;
		}
		this.actionItemList = base.GetChildByType<XUiC_ItemActionList>();
		this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += this.Pager_OnPageChanged;
		}
	}

	// Token: 0x060084BA RID: 33978 RVA: 0x00327AF4 File Offset: 0x00325CF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnHover(XUiController _sender, bool _isOver)
	{
		XUiC_SkillAttributeLevel xuiC_SkillAttributeLevel = _sender as XUiC_SkillAttributeLevel;
		if (xuiC_SkillAttributeLevel == null)
		{
			xuiC_SkillAttributeLevel = _sender.Parent as XUiC_SkillAttributeLevel;
		}
		if (_isOver && xuiC_SkillAttributeLevel != null)
		{
			this.HoveredLevel = xuiC_SkillAttributeLevel.Level;
			return;
		}
		this.HoveredLevel = -1;
	}

	// Token: 0x060084BB RID: 33979 RVA: 0x00327B34 File Offset: 0x00325D34
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

	// Token: 0x060084BC RID: 33980 RVA: 0x00327BAC File Offset: 0x00325DAC
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
		foreach (XUiC_SkillAttributeLevel xuiC_SkillAttributeLevel in this.levelEntries)
		{
			xuiC_SkillAttributeLevel.Level = num++;
			xuiC_SkillAttributeLevel.IsDirty = true;
			if (entryForSkill != null)
			{
				xuiC_SkillAttributeLevel.btnBuy.NavLeftTarget = entryForSkill.ViewComponent;
			}
		}
	}

	// Token: 0x060084BD RID: 33981 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060084BE RID: 33982 RVA: 0x00327C74 File Offset: 0x00325E74
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

	// Token: 0x060084BF RID: 33983 RVA: 0x00327CA1 File Offset: 0x00325EA1
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x060084C0 RID: 33984 RVA: 0x00327CC8 File Offset: 0x00325EC8
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
			foreach (XUiC_SkillAttributeLevel xuiC_SkillAttributeLevel in this.levelEntries)
			{
				if (xuiC_SkillAttributeLevel.CurrentSkill != null && xuiC_SkillAttributeLevel.Level == this.CurrentSkill.Level + 1)
				{
					xuiC_SkillAttributeLevel.btnBuy.Controller.Pressed(-1);
					break;
				}
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060084C1 RID: 33985 RVA: 0x00327E04 File Offset: 0x00326004
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "hidden_entries_with_paging")
		{
			this.hiddenEntriesWithPaging = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			foreach (XUiC_SkillAttributeLevel xuiC_SkillAttributeLevel in this.levelEntries)
			{
				if (xuiC_SkillAttributeLevel != null)
				{
					xuiC_SkillAttributeLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
				}
			}
			this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060084C2 RID: 33986 RVA: 0x00327EA0 File Offset: 0x003260A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1912580562U)
		{
			if (num <= 1275709072U)
			{
				if (num != 443815844U)
				{
					if (num == 1275709072U)
					{
						if (_bindingName == "maxSkillLevel")
						{
							_value = ((this.CurrentSkill != null) ? this.maxSkillLevelFormatter.Format((float)ProgressionClass.GetCalculatedMaxLevel(entityPlayer, this.CurrentSkill)) : "0");
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
			else if (num != 1283949528U)
			{
				if (num == 1912580562U)
				{
					if (_bindingName == "buycost")
					{
						_value = "-- PTS";
						if (this.CurrentSkill != null && this.CurrentSkill.Level < this.CurrentSkill.ProgressionClass.MaxLevel)
						{
							if (this.CurrentSkill.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
							{
								_value = this.buyCostFormatter.Format(this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.CurrentSkill.Level + 1));
							}
							else
							{
								_value = this.expCostFormatter.Format((int)((1f - this.CurrentSkill.PercToNextLevel) * (float)this.CurrentSkill.ProgressionClass.CalculatedCostForLevel(this.CurrentSkill.Level + 1)));
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
						if (this.CurrentSkill != null && this.hoveredLevel != -1)
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

	// Token: 0x0400626D RID: 25197
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x0400626E RID: 25198
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x0400626F RID: 25199
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillAttributeLevel> levelEntries = new List<XUiC_SkillAttributeLevel>();

	// Token: 0x04006270 RID: 25200
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006271 RID: 25201
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x04006272 RID: 25202
	[PublicizedFrom(EAccessModifier.Private)]
	public int hoveredLevel = -1;

	// Token: 0x04006273 RID: 25203
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006274 RID: 25204
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006275 RID: 25205
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> buyCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("xuiSkillPoints", false, null));

	// Token: 0x04006276 RID: 25206
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> expCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("RewardExp_keyword", false, null));
}
