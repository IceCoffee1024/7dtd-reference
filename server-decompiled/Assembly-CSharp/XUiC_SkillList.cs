using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200105F RID: 4191
[Preserve]
public class XUiC_SkillList : XUiController
{
	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x0600852D RID: 34093 RVA: 0x0032B4EB File Offset: 0x003296EB
	// (set) Token: 0x0600852E RID: 34094 RVA: 0x0032B4F4 File Offset: 0x003296F4
	public XUiC_SkillEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = true;
				this.xui.SelectedSkill = this.selectedEntry.Skill;
			}
		}
	}

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x0600852F RID: 34095 RVA: 0x0032B546 File Offset: 0x00329746
	// (set) Token: 0x06008530 RID: 34096 RVA: 0x0032B54E File Offset: 0x0032974E
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x06008531 RID: 34097 RVA: 0x0032B557 File Offset: 0x00329757
	// (set) Token: 0x06008532 RID: 34098 RVA: 0x0032B55F File Offset: 0x0032975F
	public XUiC_SkillListWindow SkillListWindow { get; set; }

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x06008533 RID: 34099 RVA: 0x0032B568 File Offset: 0x00329768
	// (set) Token: 0x06008534 RID: 34100 RVA: 0x0032B570 File Offset: 0x00329770
	public string Category
	{
		get
		{
			return this.category;
		}
		set
		{
			if (this.category != value)
			{
				this.category = value;
				if (Progression.ProgressionClasses.ContainsKey(this.category))
				{
					this.attributeClass = Progression.ProgressionClasses[this.category];
				}
			}
		}
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x06008535 RID: 34101 RVA: 0x0032B5AF File Offset: 0x003297AF
	public ProgressionClass.DisplayTypes DisplayType
	{
		get
		{
			if (this.attributeClass == null)
			{
				return ProgressionClass.DisplayTypes.Standard;
			}
			return this.attributeClass.DisplayType;
		}
	}

	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x06008536 RID: 34102 RVA: 0x0032B5C6 File Offset: 0x003297C6
	public int PageCount
	{
		get
		{
			if (this.pagingControl != null)
			{
				return this.pagingControl.LastPageNumber;
			}
			return 1;
		}
	}

	// Token: 0x06008537 RID: 34103 RVA: 0x0032B5E0 File Offset: 0x003297E0
	public override void Init()
	{
		base.Init();
		this.Category = "";
		XUiController parent = this.parent.Parent;
		this.skillEntries = base.GetChildrenByType<XUiC_SkillEntry>(null);
		this.pagingControl = parent.GetChildByType<XUiC_Paging>();
		if (this.pagingControl != null)
		{
			this.pagingControl.OnPageChanged += this.PagingControl_OnPageChanged;
		}
		this.txtInput = parent.GetChildByType<XUiC_TextInput>();
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.TxtInput_OnChangeHandler;
			this.txtInput.Text = "";
		}
		for (int i = 0; i < this.skillEntries.Length; i++)
		{
			this.skillEntries[i].skillList = this;
			this.skillEntries[i].OnPress += this.XUiC_SkillEntry_OnPress;
			this.skillEntries[i].OnScroll += this.HandleOnScroll;
		}
	}

	// Token: 0x06008538 RID: 34104 RVA: 0x0032B6D0 File Offset: 0x003298D0
	public void SetSelectedByUnlockData(RecipeUnlockData unlockData)
	{
		switch (unlockData.UnlockType)
		{
		case RecipeUnlockData.UnlockTypes.Perk:
			this.selectName = unlockData.Perk.Name;
			if (unlockData.Perk.IsPerk)
			{
				this.CategoryList.SetCategory(unlockData.Perk.Parent.ParentName);
				return;
			}
			break;
		case RecipeUnlockData.UnlockTypes.Book:
			this.selectName = unlockData.Perk.ParentName;
			if (unlockData.Perk.IsBook)
			{
				this.CategoryList.SetCategory(unlockData.Perk.Parent.ParentName);
				return;
			}
			break;
		case RecipeUnlockData.UnlockTypes.Skill:
			this.selectName = unlockData.Perk.Name;
			if (unlockData.Perk.IsCrafting)
			{
				this.CategoryList.SetCategory(unlockData.Perk.Parent.Name);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06008539 RID: 34105 RVA: 0x0032B7A9 File Offset: 0x003299A9
	[PublicizedFrom(EAccessModifier.Internal)]
	public int GetActiveCount()
	{
		return this.currentSkills.Count;
	}

	// Token: 0x0600853A RID: 34106 RVA: 0x0032B7B8 File Offset: 0x003299B8
	public void SetFilterText(string _text)
	{
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler -= this.TxtInput_OnChangeHandler;
			this.filterText = _text;
			this.txtInput.Text = _text;
			this.txtInput.OnChangeHandler += this.TxtInput_OnChangeHandler;
		}
	}

	// Token: 0x0600853B RID: 34107 RVA: 0x0032B80E File Offset: 0x00329A0E
	public void SelectFirstEntry()
	{
		this.SelectedEntry = this.skillEntries[0];
		this.SelectedEntry.SelectCursorElement(true, false);
	}

	// Token: 0x0600853C RID: 34108 RVA: 0x0032B82C File Offset: 0x00329A2C
	public void SetSelected(XUiC_SkillEntry _entry)
	{
		this.SelectedEntry = _entry;
		this.selectName = "";
	}

	// Token: 0x0600853D RID: 34109 RVA: 0x0032B840 File Offset: 0x00329A40
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SkillEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SkillEntry xuiC_SkillEntry = (XUiC_SkillEntry)_sender;
		if (xuiC_SkillEntry.Skill == null || xuiC_SkillEntry.Skill.ProgressionClass.Type == ProgressionType.Skill)
		{
			return;
		}
		this.SetSelected(xuiC_SkillEntry);
	}

	// Token: 0x0600853E RID: 34110 RVA: 0x0032B877 File Offset: 0x00329A77
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text.Trim();
		this.RefreshSkillList();
	}

	// Token: 0x0600853F RID: 34111 RVA: 0x0032B88B File Offset: 0x00329A8B
	[PublicizedFrom(EAccessModifier.Private)]
	public void PagingControl_OnPageChanged()
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		this.listSkills((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0);
	}

	// Token: 0x06008540 RID: 34112 RVA: 0x0032B8A5 File Offset: 0x00329AA5
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			this.HandlePageDown(this, new EventArgs());
			return;
		}
		this.HandlePageUp(this, new EventArgs());
	}

	// Token: 0x06008541 RID: 34113 RVA: 0x0032B8C8 File Offset: 0x00329AC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePageDown(XUiController _sender, EventArgs _e)
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		if (xuiC_Paging == null)
		{
			return;
		}
		xuiC_Paging.PageDown();
	}

	// Token: 0x06008542 RID: 34114 RVA: 0x0032B8DB File Offset: 0x00329ADB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePageUp(XUiController _sender, EventArgs _e)
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		if (xuiC_Paging == null)
		{
			return;
		}
		xuiC_Paging.PageUp();
	}

	// Token: 0x06008543 RID: 34115 RVA: 0x0032B8EE File Offset: 0x00329AEE
	public void RefreshSkillList()
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.Reset();
		}
		this.updateFilteredList();
		this.PagingControl_OnPageChanged();
	}

	// Token: 0x06008544 RID: 34116 RVA: 0x0032B910 File Offset: 0x00329B10
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFilteredList()
	{
		this.currentSkills.Clear();
		string text = this.Category.Trim();
		bool flag = this.filterText != "";
		foreach (ProgressionValue progressionValue in this.skills)
		{
			ProgressionClass progressionClass = ((progressionValue != null) ? progressionValue.ProgressionClass : null);
			if (progressionClass != null && progressionClass.ValidDisplay(this.DisplayType) && progressionClass.Name != null && !progressionClass.IsBook && !progressionClass.Hidden && progressionClass.Enabled && (!flag || progressionClass.NameKey.ContainsCaseInsensitive(this.filterText) || Localization.Get(progressionClass.NameKey, false, null).ContainsCaseInsensitive(this.filterText)))
			{
				if (!flag && (text == "" || text.EqualsCaseInsensitive(progressionClass.Name)))
				{
					this.currentSkills.Add(progressionValue);
				}
				else
				{
					ProgressionClass parent = progressionClass.Parent;
					if (parent != null && parent != progressionClass)
					{
						if (flag)
						{
							if ((!progressionClass.IsSkill && !progressionClass.IsPerk) || !parent.IsAttribute)
							{
								this.currentSkills.Add(progressionValue);
							}
						}
						else if (((progressionClass.IsSkill || progressionClass.IsCrafting) && text.EqualsCaseInsensitive(parent.Name)) || ((progressionClass.IsPerk || progressionClass.IsBookGroup) && text.EqualsCaseInsensitive(parent.Parent.Name)))
						{
							this.currentSkills.Add(progressionValue);
						}
					}
				}
			}
		}
		this.currentSkills.Sort(ProgressionClass.ListSortOrderComparer.Instance);
		if (!flag)
		{
			for (int i = 0; i < this.currentSkills.Count; i++)
			{
				if (this.currentSkills[i].ProgressionClass.IsAttribute)
				{
					while (i % this.skillEntries.Length != 0)
					{
						this.currentSkills.Insert(i, null);
						i++;
					}
				}
			}
		}
		XUiC_Paging xuiC_Paging = this.pagingControl;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength(this.currentSkills.Count, this.skillEntries.Length);
		}
		if (!string.IsNullOrEmpty(this.selectName))
		{
			int j = 0;
			while (j < this.currentSkills.Count)
			{
				if (this.currentSkills[j].Name == this.selectName)
				{
					XUiC_Paging xuiC_Paging2 = this.pagingControl;
					if (xuiC_Paging2 == null)
					{
						return;
					}
					xuiC_Paging2.SetPage(j / this.skillEntries.Length);
					return;
				}
				else
				{
					j++;
				}
			}
			return;
		}
		XUiC_Paging xuiC_Paging3 = this.pagingControl;
		if (xuiC_Paging3 == null)
		{
			return;
		}
		xuiC_Paging3.SetPage(0);
	}

	// Token: 0x06008545 RID: 34117 RVA: 0x0032BBE8 File Offset: 0x00329DE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void listSkills(int page)
	{
		int num = this.skillEntries.Length * page;
		this.SelectedEntry = null;
		for (int i = 0; i < this.skillEntries.Length; i++)
		{
			int num2 = i + num;
			XUiC_SkillEntry xuiC_SkillEntry = this.skillEntries[i];
			if (num2 < this.currentSkills.Count && this.currentSkills[num2] != null && Progression.ProgressionClasses.ContainsKey(this.currentSkills[num2].Name))
			{
				xuiC_SkillEntry.Skill = this.currentSkills[num2];
				if (this.selectName != "")
				{
					if (xuiC_SkillEntry.Skill.ProgressionClass.Name == this.selectName)
					{
						this.SelectedEntry = xuiC_SkillEntry;
						xuiC_SkillEntry.IsSelected = true;
						xuiC_SkillEntry.RefreshBindings();
						((XUiC_SkillWindowGroup)base.WindowGroup.Controller).CurrentSkill = xuiC_SkillEntry.Skill;
					}
				}
				else if (this.SelectedEntry == null && i == 0)
				{
					this.SelectedEntry = xuiC_SkillEntry;
					this.xui.SelectedSkill = xuiC_SkillEntry.Skill;
					((XUiC_SkillWindowGroup)base.WindowGroup.Controller).CurrentSkill = this.selectedEntry.Skill;
					((XUiC_SkillWindowGroup)base.WindowGroup.Controller).IsDirty = true;
				}
				if (this.xui.SelectedSkill != null)
				{
					xuiC_SkillEntry.IsSelected = xuiC_SkillEntry.Skill.Name == this.xui.SelectedSkill.Name;
					xuiC_SkillEntry.RefreshBindings();
				}
				else
				{
					xuiC_SkillEntry.IsSelected = false;
					xuiC_SkillEntry.RefreshBindings();
				}
				xuiC_SkillEntry.DisplayType = this.DisplayType;
				xuiC_SkillEntry.ViewComponent.Enabled = true;
				if (xuiC_SkillEntry.Skill.ProgressionClass.IsAttribute)
				{
					XUiView viewComponent = this.xui.GetWindow("windowSkillAttributeInfo").Controller.GetChildById("0").ViewComponent;
					xuiC_SkillEntry.ViewComponent.NavRightTarget = viewComponent;
				}
				else if (xuiC_SkillEntry.Skill.ProgressionClass.IsPerk)
				{
					XUiView viewComponent2 = this.xui.GetWindow("windowSkillPerkInfo").Controller.GetChildById("0").ViewComponent;
					xuiC_SkillEntry.ViewComponent.NavRightTarget = viewComponent2;
				}
				else if (xuiC_SkillEntry.Skill.ProgressionClass.IsBookGroup)
				{
					XUiView viewComponent3 = this.xui.GetWindow("windowSkillBookInfo").Controller.GetChildById("0").ViewComponent;
					xuiC_SkillEntry.ViewComponent.NavRightTarget = viewComponent3;
				}
				else if (xuiC_SkillEntry.Skill.ProgressionClass.IsCrafting)
				{
					XUiView viewComponent4 = this.xui.GetWindow("windowSkillCraftingInfo").Controller.GetChildById("0").ViewComponent;
					xuiC_SkillEntry.ViewComponent.NavRightTarget = viewComponent4;
				}
			}
			else
			{
				xuiC_SkillEntry.Skill = null;
				xuiC_SkillEntry.IsSelected = false;
				xuiC_SkillEntry.ViewComponent.Enabled = false;
				xuiC_SkillEntry.DisplayType = ProgressionClass.DisplayTypes.Standard;
				xuiC_SkillEntry.ViewComponent.NavRightTarget = null;
				xuiC_SkillEntry.RefreshBindings();
			}
		}
		if (this.SelectedEntry == null)
		{
			this.SelectedEntry = this.skillEntries[0];
			this.SelectedEntry.IsSelected = true;
			this.SelectedEntry.RefreshBindings();
			((XUiC_SkillWindowGroup)base.WindowGroup.Controller).CurrentSkill = this.SelectedEntry.Skill;
			((XUiC_SkillWindowGroup)base.WindowGroup.Controller).IsDirty = true;
			this.selectName = "";
		}
		base.RefreshBindings();
		this.SkillListWindow.RefreshBindings();
	}

	// Token: 0x06008546 RID: 34118 RVA: 0x0032BF74 File Offset: 0x0032A174
	public override void OnOpen()
	{
		base.OnOpen();
		this.skills.Clear();
		this.xui.playerUI.entityPlayer.Progression.GetDict().CopyValuesTo<int, ProgressionValue>(this.skills);
		this.updateFilteredList();
		this.PagingControl_OnPageChanged();
	}

	// Token: 0x06008547 RID: 34119 RVA: 0x0032BFC3 File Offset: 0x0032A1C3
	public override void OnClose()
	{
		base.OnClose();
		this.selectName = "";
	}

	// Token: 0x06008548 RID: 34120 RVA: 0x0032BFD8 File Offset: 0x0032A1D8
	public XUiC_SkillEntry GetEntryForSkill(ProgressionValue _skill)
	{
		foreach (XUiC_SkillEntry xuiC_SkillEntry in this.skillEntries)
		{
			if (xuiC_SkillEntry.Skill == _skill)
			{
				return xuiC_SkillEntry;
			}
		}
		return null;
	}

	// Token: 0x040062E0 RID: 25312
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> skills = new List<ProgressionValue>();

	// Token: 0x040062E1 RID: 25313
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> currentSkills = new List<ProgressionValue>();

	// Token: 0x040062E2 RID: 25314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x040062E3 RID: 25315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pagingControl;

	// Token: 0x040062E4 RID: 25316
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x040062E5 RID: 25317
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectName;

	// Token: 0x040062E6 RID: 25318
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry selectedEntry;

	// Token: 0x040062E9 RID: 25321
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040062EA RID: 25322
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass attributeClass;

	// Token: 0x040062EB RID: 25323
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";
}
