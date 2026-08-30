using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200103E RID: 4158
[Preserve]
public class XUiC_SkillBookInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x060083EB RID: 33771 RVA: 0x00324890 File Offset: 0x00322A90
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

	// Token: 0x060083EC RID: 33772 RVA: 0x003248C4 File Offset: 0x00322AC4
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_SkillBookLevel>(this.perkEntries);
		int num = 1;
		foreach (XUiC_SkillBookLevel xuiC_SkillBookLevel in this.perkEntries)
		{
			xuiC_SkillBookLevel.ListIndex = num - 1;
			xuiC_SkillBookLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
			xuiC_SkillBookLevel.MaxEntriesWithoutPaging = this.perkEntries.Count;
			xuiC_SkillBookLevel.OnScroll += this.Entry_OnScroll;
		}
		this.actionItemList = base.GetChildByType<XUiC_ItemActionList>();
		this.skillsPerPage = this.perkEntries.Count - this.hiddenEntriesWithPaging;
	}

	// Token: 0x060083ED RID: 33773 RVA: 0x0008A069 File Offset: 0x00088269
	public void SkillChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060083EE RID: 33774 RVA: 0x00324980 File Offset: 0x00322B80
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSkill()
	{
		if (this.CurrentSkill != null && this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		if (this.CurrentSkill != null)
		{
			this.xui.playerUI.entityPlayer.Progression.GetPerkList(this.perkList, this.CurrentSkill.Name);
		}
		XUiC_SkillEntry entryForSkill = this.windowGroup.Controller.GetChildByType<XUiC_SkillList>().GetEntryForSkill(this.CurrentSkill);
		int num = 0;
		foreach (XUiC_SkillBookLevel xuiC_SkillBookLevel in this.perkEntries)
		{
			if (num < this.perkList.Count)
			{
				xuiC_SkillBookLevel.Perk = this.perkList[num];
				xuiC_SkillBookLevel.Volume = num + 1;
				xuiC_SkillBookLevel.OnHover += this.Entry_OnHover;
				xuiC_SkillBookLevel.CompletionReward = num == this.perkList.Count - 1;
				if (entryForSkill != null)
				{
					xuiC_SkillBookLevel.ViewComponent.NavLeftTarget = entryForSkill.ViewComponent;
				}
			}
			else
			{
				xuiC_SkillBookLevel.Perk = null;
				xuiC_SkillBookLevel.Volume = -1;
				xuiC_SkillBookLevel.OnHover -= this.Entry_OnHover;
				xuiC_SkillBookLevel.CompletionReward = false;
			}
			num++;
		}
	}

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x060083EF RID: 33775 RVA: 0x00324AD4 File Offset: 0x00322CD4
	// (set) Token: 0x060083F0 RID: 33776 RVA: 0x00324ADC File Offset: 0x00322CDC
	public ProgressionValue HoveredPerk
	{
		get
		{
			return this.hoveredPerk;
		}
		set
		{
			if (this.hoveredPerk != value)
			{
				this.hoveredPerk = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x060083F1 RID: 33777 RVA: 0x00324AF4 File Offset: 0x00322CF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnHover(XUiController _sender, bool _isOver)
	{
		XUiC_SkillBookLevel xuiC_SkillBookLevel = _sender as XUiC_SkillBookLevel;
		if (_isOver && xuiC_SkillBookLevel != null)
		{
			this.HoveredPerk = xuiC_SkillBookLevel.Perk;
			return;
		}
		this.HoveredPerk = null;
	}

	// Token: 0x060083F2 RID: 33778 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060083F3 RID: 33779 RVA: 0x00324B22 File Offset: 0x00322D22
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

	// Token: 0x060083F4 RID: 33780 RVA: 0x00324B4F File Offset: 0x00322D4F
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x060083F5 RID: 33781 RVA: 0x00324B73 File Offset: 0x00322D73
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

	// Token: 0x060083F6 RID: 33782 RVA: 0x00324B98 File Offset: 0x00322D98
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "hidden_entries_with_paging")
		{
			this.hiddenEntriesWithPaging = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			foreach (XUiC_SkillBookLevel xuiC_SkillBookLevel in this.perkEntries)
			{
				if (xuiC_SkillBookLevel != null)
				{
					xuiC_SkillBookLevel.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
				}
			}
			this.skillsPerPage = this.perkEntries.Count - this.hiddenEntriesWithPaging;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060083F7 RID: 33783 RVA: 0x00324C34 File Offset: 0x00322E34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2606420134U)
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
				if (num == 2606420134U)
				{
					if (_bindingName == "groupdescription")
					{
						if (this.CurrentSkill != null)
						{
							_value = Localization.Get(this.CurrentSkill.ProgressionClass.DescKey, false, null);
						}
						else
						{
							_value = "";
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
		else if (num <= 3504806855U)
		{
			if (num != 3268933568U)
			{
				if (num == 3504806855U)
				{
					if (_bindingName == "groupname")
					{
						_value = ((this.CurrentSkill != null) ? Localization.Get(this.CurrentSkill.ProgressionClass.NameKey, false, null) : "Skill Info");
						return true;
					}
				}
			}
			else if (_bindingName == "showPaging")
			{
				_value = "false";
				return true;
			}
		}
		else if (num != 4010384093U)
		{
			if (num == 4294521801U)
			{
				if (_bindingName == "detailsdescription")
				{
					if (this.CurrentSkill != null)
					{
						if (this.hoveredPerk != null)
						{
							if (string.IsNullOrEmpty(this.hoveredPerk.ProgressionClass.LongDescKey))
							{
								_value = Localization.Get(this.hoveredPerk.ProgressionClass.DescKey, false, null);
							}
							else
							{
								_value = Localization.Get(this.hoveredPerk.ProgressionClass.LongDescKey, false, null);
							}
						}
						else
						{
							_value = Localization.Get(this.CurrentSkill.ProgressionClass.LongDescKey, false, null);
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
		else if (_bindingName == "groupicon")
		{
			_value = ((this.CurrentSkill != null) ? this.CurrentSkill.ProgressionClass.Icon : "ui_game_symbol_skills");
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x040061CF RID: 25039
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x040061D0 RID: 25040
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x040061D1 RID: 25041
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillBookLevel> perkEntries = new List<XUiC_SkillBookLevel>();

	// Token: 0x040061D2 RID: 25042
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040061D3 RID: 25043
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x040061D4 RID: 25044
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> perkList = new List<ProgressionValue>();

	// Token: 0x040061D5 RID: 25045
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue hoveredPerk;

	// Token: 0x040061D6 RID: 25046
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040061D7 RID: 25047
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040061D8 RID: 25048
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> buyCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("xuiSkillPoints", false, null));

	// Token: 0x040061D9 RID: 25049
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> expCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("RewardExp_keyword", false, null));
}
