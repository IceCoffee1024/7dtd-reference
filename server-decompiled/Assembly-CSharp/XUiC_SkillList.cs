using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001070 RID: 4208
[Preserve]
public class XUiC_SkillList : XUiController
{
	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x06008573 RID: 34163 RVA: 0x0032F0F7 File Offset: 0x0032D2F7
	// (set) Token: 0x06008574 RID: 34164 RVA: 0x0032F100 File Offset: 0x0032D300
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

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x06008575 RID: 34165 RVA: 0x0032F152 File Offset: 0x0032D352
	// (set) Token: 0x06008576 RID: 34166 RVA: 0x0032F15A File Offset: 0x0032D35A
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x06008577 RID: 34167 RVA: 0x0032F163 File Offset: 0x0032D363
	// (set) Token: 0x06008578 RID: 34168 RVA: 0x0032F16B File Offset: 0x0032D36B
	public XUiC_SkillListWindow SkillListWindow { get; set; }

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x06008579 RID: 34169 RVA: 0x0032F174 File Offset: 0x0032D374
	// (set) Token: 0x0600857A RID: 34170 RVA: 0x0032F17C File Offset: 0x0032D37C
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

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x0600857B RID: 34171 RVA: 0x0032F1BB File Offset: 0x0032D3BB
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

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x0600857C RID: 34172 RVA: 0x0032F1D2 File Offset: 0x0032D3D2
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

	// Token: 0x0600857D RID: 34173 RVA: 0x0032F1EC File Offset: 0x0032D3EC
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

	// Token: 0x0600857E RID: 34174 RVA: 0x0032F2DC File Offset: 0x0032D4DC
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

	// Token: 0x0600857F RID: 34175 RVA: 0x0032F3B5 File Offset: 0x0032D5B5
	[PublicizedFrom(EAccessModifier.Internal)]
	public int GetActiveCount()
	{
		return this.currentSkills.Count;
	}

	// Token: 0x06008580 RID: 34176 RVA: 0x0032F3C4 File Offset: 0x0032D5C4
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

	// Token: 0x06008581 RID: 34177 RVA: 0x0032F41A File Offset: 0x0032D61A
	public void SelectFirstEntry()
	{
		this.SelectedEntry = this.skillEntries[0];
		this.SelectedEntry.SelectCursorElement(true, false);
	}

	// Token: 0x06008582 RID: 34178 RVA: 0x0032F438 File Offset: 0x0032D638
	public void SetSelected(XUiC_SkillEntry _entry)
	{
		this.SelectedEntry = _entry;
		this.selectName = "";
	}

	// Token: 0x06008583 RID: 34179 RVA: 0x0032F44C File Offset: 0x0032D64C
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

	// Token: 0x06008584 RID: 34180 RVA: 0x0032F483 File Offset: 0x0032D683
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text.Trim();
		this.RefreshSkillList();
	}

	// Token: 0x06008585 RID: 34181 RVA: 0x0032F497 File Offset: 0x0032D697
	[PublicizedFrom(EAccessModifier.Private)]
	public void PagingControl_OnPageChanged()
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		this.listSkills((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0);
	}

	// Token: 0x06008586 RID: 34182 RVA: 0x0032F4B1 File Offset: 0x0032D6B1
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

	// Token: 0x06008587 RID: 34183 RVA: 0x0032F4D4 File Offset: 0x0032D6D4
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

	// Token: 0x06008588 RID: 34184 RVA: 0x0032F4E7 File Offset: 0x0032D6E7
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

	// Token: 0x06008589 RID: 34185 RVA: 0x0032F4FA File Offset: 0x0032D6FA
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

	// Token: 0x0600858A RID: 34186 RVA: 0x0032F51C File Offset: 0x0032D71C
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

	// Token: 0x0600858B RID: 34187 RVA: 0x0032F7F4 File Offset: 0x0032D9F4
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

	// Token: 0x0600858C RID: 34188 RVA: 0x0032FB80 File Offset: 0x0032DD80
	public override void OnOpen()
	{
		base.OnOpen();
		this.skills.Clear();
		this.xui.playerUI.entityPlayer.Progression.GetDict().CopyValuesTo<int, ProgressionValue>(this.skills);
		this.updateFilteredList();
		this.PagingControl_OnPageChanged();
	}

	// Token: 0x0600858D RID: 34189 RVA: 0x0032FBCF File Offset: 0x0032DDCF
	public override void OnClose()
	{
		base.OnClose();
		this.selectName = "";
	}

	// Token: 0x0600858E RID: 34190 RVA: 0x0032FBE4 File Offset: 0x0032DDE4
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

	// Token: 0x04006325 RID: 25381
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> skills = new List<ProgressionValue>();

	// Token: 0x04006326 RID: 25382
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> currentSkills = new List<ProgressionValue>();

	// Token: 0x04006327 RID: 25383
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x04006328 RID: 25384
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pagingControl;

	// Token: 0x04006329 RID: 25385
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x0400632A RID: 25386
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectName;

	// Token: 0x0400632B RID: 25387
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry selectedEntry;

	// Token: 0x0400632E RID: 25390
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400632F RID: 25391
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass attributeClass;

	// Token: 0x04006330 RID: 25392
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";
}
