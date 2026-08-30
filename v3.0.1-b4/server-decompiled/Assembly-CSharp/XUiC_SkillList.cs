using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001046 RID: 4166
[Preserve]
public class XUiC_SkillList : XUiController
{
	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x0600843B RID: 33851 RVA: 0x00326CC3 File Offset: 0x00324EC3
	// (set) Token: 0x0600843C RID: 33852 RVA: 0x00326CCC File Offset: 0x00324ECC
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

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x0600843D RID: 33853 RVA: 0x00326D1E File Offset: 0x00324F1E
	// (set) Token: 0x0600843E RID: 33854 RVA: 0x00326D26 File Offset: 0x00324F26
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x0600843F RID: 33855 RVA: 0x00326D2F File Offset: 0x00324F2F
	// (set) Token: 0x06008440 RID: 33856 RVA: 0x00326D37 File Offset: 0x00324F37
	public XUiC_SkillListWindow SkillListWindow { get; set; }

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x06008441 RID: 33857 RVA: 0x00326D40 File Offset: 0x00324F40
	// (set) Token: 0x06008442 RID: 33858 RVA: 0x00326D48 File Offset: 0x00324F48
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

	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x06008443 RID: 33859 RVA: 0x00326D87 File Offset: 0x00324F87
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

	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x06008444 RID: 33860 RVA: 0x00326D9E File Offset: 0x00324F9E
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

	// Token: 0x06008445 RID: 33861 RVA: 0x00326DB8 File Offset: 0x00324FB8
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

	// Token: 0x06008446 RID: 33862 RVA: 0x00326EA8 File Offset: 0x003250A8
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

	// Token: 0x06008447 RID: 33863 RVA: 0x00326F81 File Offset: 0x00325181
	[PublicizedFrom(EAccessModifier.Internal)]
	public int GetActiveCount()
	{
		return this.currentSkills.Count;
	}

	// Token: 0x06008448 RID: 33864 RVA: 0x00326F90 File Offset: 0x00325190
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

	// Token: 0x06008449 RID: 33865 RVA: 0x00326FE6 File Offset: 0x003251E6
	public void SelectFirstEntry()
	{
		this.SelectedEntry = this.skillEntries[0];
		this.SelectedEntry.SelectCursorElement(true, false);
	}

	// Token: 0x0600844A RID: 33866 RVA: 0x00327004 File Offset: 0x00325204
	public void SetSelected(XUiC_SkillEntry _entry)
	{
		this.SelectedEntry = _entry;
		this.selectName = "";
	}

	// Token: 0x0600844B RID: 33867 RVA: 0x00327018 File Offset: 0x00325218
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

	// Token: 0x0600844C RID: 33868 RVA: 0x0032704F File Offset: 0x0032524F
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text.Trim();
		this.RefreshSkillList();
	}

	// Token: 0x0600844D RID: 33869 RVA: 0x00327063 File Offset: 0x00325263
	[PublicizedFrom(EAccessModifier.Private)]
	public void PagingControl_OnPageChanged()
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		this.listSkills((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0);
	}

	// Token: 0x0600844E RID: 33870 RVA: 0x0032707D File Offset: 0x0032527D
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

	// Token: 0x0600844F RID: 33871 RVA: 0x003270A0 File Offset: 0x003252A0
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

	// Token: 0x06008450 RID: 33872 RVA: 0x003270B3 File Offset: 0x003252B3
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

	// Token: 0x06008451 RID: 33873 RVA: 0x003270C6 File Offset: 0x003252C6
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

	// Token: 0x06008452 RID: 33874 RVA: 0x003270E8 File Offset: 0x003252E8
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

	// Token: 0x06008453 RID: 33875 RVA: 0x003273C0 File Offset: 0x003255C0
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

	// Token: 0x06008454 RID: 33876 RVA: 0x0032774C File Offset: 0x0032594C
	public override void OnOpen()
	{
		base.OnOpen();
		this.skills.Clear();
		this.xui.playerUI.entityPlayer.Progression.GetDict().CopyValuesTo<int, ProgressionValue>(this.skills);
		this.updateFilteredList();
		this.PagingControl_OnPageChanged();
	}

	// Token: 0x06008455 RID: 33877 RVA: 0x0032779B File Offset: 0x0032599B
	public override void OnClose()
	{
		base.OnClose();
		this.selectName = "";
	}

	// Token: 0x06008456 RID: 33878 RVA: 0x003277B0 File Offset: 0x003259B0
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

	// Token: 0x04006219 RID: 25113
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> skills = new List<ProgressionValue>();

	// Token: 0x0400621A RID: 25114
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> currentSkills = new List<ProgressionValue>();

	// Token: 0x0400621B RID: 25115
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x0400621C RID: 25116
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pagingControl;

	// Token: 0x0400621D RID: 25117
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x0400621E RID: 25118
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectName;

	// Token: 0x0400621F RID: 25119
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry selectedEntry;

	// Token: 0x04006222 RID: 25122
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006223 RID: 25123
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass attributeClass;

	// Token: 0x04006224 RID: 25124
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";
}
