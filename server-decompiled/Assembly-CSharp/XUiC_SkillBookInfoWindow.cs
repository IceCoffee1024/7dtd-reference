using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02001078 RID: 4216
[Preserve]
public class XUiC_SkillBookInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x06008575 RID: 34165 RVA: 0x0032EAD8 File Offset: 0x0032CCD8
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

	// Token: 0x06008576 RID: 34166 RVA: 0x0032EB0C File Offset: 0x0032CD0C
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

	// Token: 0x06008577 RID: 34167 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void SkillChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008578 RID: 34168 RVA: 0x0032EBC8 File Offset: 0x0032CDC8
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

	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x06008579 RID: 34169 RVA: 0x0032ED1C File Offset: 0x0032CF1C
	// (set) Token: 0x0600857A RID: 34170 RVA: 0x0032ED24 File Offset: 0x0032CF24
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

	// Token: 0x0600857B RID: 34171 RVA: 0x0032ED3C File Offset: 0x0032CF3C
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

	// Token: 0x0600857C RID: 34172 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600857D RID: 34173 RVA: 0x0032ED6A File Offset: 0x0032CF6A
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

	// Token: 0x0600857E RID: 34174 RVA: 0x0032ED97 File Offset: 0x0032CF97
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x0600857F RID: 34175 RVA: 0x0032EDBB File Offset: 0x0032CFBB
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

	// Token: 0x06008580 RID: 34176 RVA: 0x0032EDE0 File Offset: 0x0032CFE0
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

	// Token: 0x06008581 RID: 34177 RVA: 0x0032EE7C File Offset: 0x0032D07C
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

	// Token: 0x04006319 RID: 25369
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x0400631A RID: 25370
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x0400631B RID: 25371
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillBookLevel> perkEntries = new List<XUiC_SkillBookLevel>();

	// Token: 0x0400631C RID: 25372
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400631D RID: 25373
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x0400631E RID: 25374
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> perkList = new List<ProgressionValue>();

	// Token: 0x0400631F RID: 25375
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue hoveredPerk;

	// Token: 0x04006320 RID: 25376
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006321 RID: 25377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006322 RID: 25378
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> buyCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("xuiSkillPoints", false, null));

	// Token: 0x04006323 RID: 25379
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> expCostFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + " " + Localization.Get("RewardExp_keyword", false, null));
}
