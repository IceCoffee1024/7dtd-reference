using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F54 RID: 3924
[Preserve]
public class XUiC_GameEventMenu : XUiController
{
	// Token: 0x06007A90 RID: 31376 RVA: 0x002E6570 File Offset: 0x002E4770
	public override void Init()
	{
		base.Init();
		XUiC_GameEventMenu.ID = base.WindowGroup.Id;
		this.categoryList = (XUiC_CategoryList)base.GetChildById("categories");
		this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		this.gameEventsList = (XUiC_GameEventsList)base.GetChildById("gameevents");
		this.gameEventsList.SelectionChanged += this.EntitiesList_SelectionChanged;
		this.cbxTarget = (XUiC_ComboBoxList<string>)base.GetChildById("cbxTarget");
	}

	// Token: 0x06007A91 RID: 31377 RVA: 0x002E6603 File Offset: 0x002E4803
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.gameEventsList.Category = _categoryEntry.CategoryName;
		this.categoryDisplay = _categoryEntry.CategoryDisplayName;
		base.RefreshBindings();
	}

	// Token: 0x06007A92 RID: 31378 RVA: 0x002E6628 File Offset: 0x002E4828
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_GameEventsList.GameEventEntry> _list, XUiC_GameEventsList.GameEventEntry _previousEntry, XUiC_GameEventsList.GameEventEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.gameEventsList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.name);
		}
	}

	// Token: 0x06007A93 RID: 31379 RVA: 0x002E6654 File Offset: 0x002E4854
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawns_OnPress(string _name)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		EntityPlayer entityPlayer2 = this.PlayerList[this.cbxTarget.Value];
		if (entityPlayer2 == entityPlayer || !entityPlayer2.IsAdmin)
		{
			GameEventManager.Current.HandleAction(_name, entityPlayer, entityPlayer2, false, "", "", false, true, "", null);
		}
	}

	// Token: 0x06007A94 RID: 31380 RVA: 0x002E66BC File Offset: 0x002E48BC
	public override void OnOpen()
	{
		base.OnOpen();
		this.categoryList.SetupCategoriesBasedOnGameEventCategories(GameEventManager.Current.CategoryList);
		this.categoryList.SetCategoryToFirst();
		this.cbxTarget.Elements.Clear();
		this.PlayerList.Clear();
		int num = 0;
		for (int i = 0; i < GameManager.Instance.World.Players.list.Count; i++)
		{
			EntityPlayer entityPlayer = GameManager.Instance.World.Players.list[i];
			this.cbxTarget.Elements.Add(entityPlayer.EntityName);
			this.PlayerList.Add(entityPlayer.EntityName, entityPlayer);
			if (entityPlayer is EntityPlayerLocal)
			{
				num = i;
			}
		}
		this.cbxTarget.SelectedIndex = num;
	}

	// Token: 0x06007A95 RID: 31381 RVA: 0x002E678C File Offset: 0x002E498C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "headertitle")
		{
			if (this.categoryDisplay == "")
			{
				value = "Game Events";
			}
			else
			{
				value = string.Format("Game Events - {0}", this.categoryDisplay);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x04005AE2 RID: 23266
	public static string ID = "";

	// Token: 0x04005AE3 RID: 23267
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005AE4 RID: 23268
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GameEventsList gameEventsList;

	// Token: 0x04005AE5 RID: 23269
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplay = "";

	// Token: 0x04005AE6 RID: 23270
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxTarget;

	// Token: 0x04005AE7 RID: 23271
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, EntityPlayer> PlayerList = new Dictionary<string, EntityPlayer>();
}
