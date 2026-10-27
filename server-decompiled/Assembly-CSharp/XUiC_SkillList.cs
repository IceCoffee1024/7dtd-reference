using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001080 RID: 4224
[Preserve]
public class XUiC_SkillList : XUiController
{
	// Token: 0x17000FC8 RID: 4040
	// (get) Token: 0x060085C5 RID: 34245 RVA: 0x00330F0B File Offset: 0x0032F10B
	// (set) Token: 0x060085C6 RID: 34246 RVA: 0x00330F14 File Offset: 0x0032F114
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

	// Token: 0x17000FC9 RID: 4041
	// (get) Token: 0x060085C7 RID: 34247 RVA: 0x00330F66 File Offset: 0x0032F166
	// (set) Token: 0x060085C8 RID: 34248 RVA: 0x00330F6E File Offset: 0x0032F16E
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000FCA RID: 4042
	// (get) Token: 0x060085C9 RID: 34249 RVA: 0x00330F77 File Offset: 0x0032F177
	// (set) Token: 0x060085CA RID: 34250 RVA: 0x00330F7F File Offset: 0x0032F17F
	public XUiC_SkillListWindow SkillListWindow { get; set; }

	// Token: 0x17000FCB RID: 4043
	// (get) Token: 0x060085CB RID: 34251 RVA: 0x00330F88 File Offset: 0x0032F188
	// (set) Token: 0x060085CC RID: 34252 RVA: 0x00330F90 File Offset: 0x0032F190
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

	// Token: 0x17000FCC RID: 4044
	// (get) Token: 0x060085CD RID: 34253 RVA: 0x00330FCF File Offset: 0x0032F1CF
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

	// Token: 0x17000FCD RID: 4045
	// (get) Token: 0x060085CE RID: 34254 RVA: 0x00330FE6 File Offset: 0x0032F1E6
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

	// Token: 0x060085CF RID: 34255 RVA: 0x00331000 File Offset: 0x0032F200
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

	// Token: 0x060085D0 RID: 34256 RVA: 0x003310F0 File Offset: 0x0032F2F0
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

	// Token: 0x060085D1 RID: 34257 RVA: 0x003311C9 File Offset: 0x0032F3C9
	[PublicizedFrom(EAccessModifier.Internal)]
	public int GetActiveCount()
	{
		return this.currentSkills.Count;
	}

	// Token: 0x060085D2 RID: 34258 RVA: 0x003311D8 File Offset: 0x0032F3D8
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

	// Token: 0x060085D3 RID: 34259 RVA: 0x0033122E File Offset: 0x0032F42E
	public void SelectFirstEntry()
	{
		this.SelectedEntry = this.skillEntries[0];
		this.SelectedEntry.SelectCursorElement(true, false);
	}

	// Token: 0x060085D4 RID: 34260 RVA: 0x0033124C File Offset: 0x0032F44C
	public void SetSelected(XUiC_SkillEntry _entry)
	{
		this.SelectedEntry = _entry;
		this.selectName = "";
	}

	// Token: 0x060085D5 RID: 34261 RVA: 0x00331260 File Offset: 0x0032F460
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

	// Token: 0x060085D6 RID: 34262 RVA: 0x00331297 File Offset: 0x0032F497
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text.Trim();
		this.RefreshSkillList();
	}

	// Token: 0x060085D7 RID: 34263 RVA: 0x003312AB File Offset: 0x0032F4AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void PagingControl_OnPageChanged()
	{
		XUiC_Paging xuiC_Paging = this.pagingControl;
		this.listSkills((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0);
	}

	// Token: 0x060085D8 RID: 34264 RVA: 0x003312C5 File Offset: 0x0032F4C5
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

	// Token: 0x060085D9 RID: 34265 RVA: 0x003312E8 File Offset: 0x0032F4E8
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

	// Token: 0x060085DA RID: 34266 RVA: 0x003312FB File Offset: 0x0032F4FB
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

	// Token: 0x060085DB RID: 34267 RVA: 0x0033130E File Offset: 0x0032F50E
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

	// Token: 0x060085DC RID: 34268 RVA: 0x00331330 File Offset: 0x0032F530
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

	// Token: 0x060085DD RID: 34269 RVA: 0x00331608 File Offset: 0x0032F808
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

	// Token: 0x060085DE RID: 34270 RVA: 0x00331994 File Offset: 0x0032FB94
	public override void OnOpen()
	{
		base.OnOpen();
		this.skills.Clear();
		this.xui.playerUI.entityPlayer.Progression.GetDict().CopyValuesTo<int, ProgressionValue>(this.skills);
		this.updateFilteredList();
		this.PagingControl_OnPageChanged();
	}

	// Token: 0x060085DF RID: 34271 RVA: 0x003319E3 File Offset: 0x0032FBE3
	public override void OnClose()
	{
		base.OnClose();
		this.selectName = "";
	}

	// Token: 0x060085E0 RID: 34272 RVA: 0x003319F8 File Offset: 0x0032FBF8
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

	// Token: 0x04006363 RID: 25443
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> skills = new List<ProgressionValue>();

	// Token: 0x04006364 RID: 25444
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ProgressionValue> currentSkills = new List<ProgressionValue>();

	// Token: 0x04006365 RID: 25445
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x04006366 RID: 25446
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pagingControl;

	// Token: 0x04006367 RID: 25447
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04006368 RID: 25448
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectName;

	// Token: 0x04006369 RID: 25449
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry selectedEntry;

	// Token: 0x0400636C RID: 25452
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400636D RID: 25453
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass attributeClass;

	// Token: 0x0400636E RID: 25454
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";
}
