using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F1F RID: 3871
[Preserve]
public class XUiC_GameEventMenu : XUiController
{
	// Token: 0x06007997 RID: 31127 RVA: 0x002DD95C File Offset: 0x002DBB5C
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

	// Token: 0x06007998 RID: 31128 RVA: 0x002DD9EF File Offset: 0x002DBBEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.gameEventsList.Category = _categoryEntry.CategoryName;
		this.categoryDisplay = _categoryEntry.CategoryDisplayName;
		base.RefreshBindings();
	}

	// Token: 0x06007999 RID: 31129 RVA: 0x002DDA14 File Offset: 0x002DBC14
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_GameEventsList.GameEventEntry> _list, XUiC_GameEventsList.GameEventEntry _previousEntry, XUiC_GameEventsList.GameEventEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.gameEventsList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.name);
		}
	}

	// Token: 0x0600799A RID: 31130 RVA: 0x002DDA40 File Offset: 0x002DBC40
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

	// Token: 0x0600799B RID: 31131 RVA: 0x002DDAA8 File Offset: 0x002DBCA8
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

	// Token: 0x0600799C RID: 31132 RVA: 0x002DDB78 File Offset: 0x002DBD78
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

	// Token: 0x040059C4 RID: 22980
	public static string ID = "";

	// Token: 0x040059C5 RID: 22981
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040059C6 RID: 22982
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GameEventsList gameEventsList;

	// Token: 0x040059C7 RID: 22983
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplay = "";

	// Token: 0x040059C8 RID: 22984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxTarget;

	// Token: 0x040059C9 RID: 22985
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, EntityPlayer> PlayerList = new Dictionary<string, EntityPlayer>();
}
