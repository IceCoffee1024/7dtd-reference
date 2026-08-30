using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F36 RID: 3894
[Preserve]
public class XUiC_GameEventMenu : XUiController
{
	// Token: 0x06007A50 RID: 31312 RVA: 0x002E1910 File Offset: 0x002DFB10
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

	// Token: 0x06007A51 RID: 31313 RVA: 0x002E19A3 File Offset: 0x002DFBA3
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.gameEventsList.Category = _categoryEntry.CategoryName;
		this.categoryDisplay = _categoryEntry.CategoryDisplayName;
		base.RefreshBindings();
	}

	// Token: 0x06007A52 RID: 31314 RVA: 0x002E19C8 File Offset: 0x002DFBC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_GameEventsList.GameEventEntry> _list, XUiC_GameEventsList.GameEventEntry _previousEntry, XUiC_GameEventsList.GameEventEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.gameEventsList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.name);
		}
	}

	// Token: 0x06007A53 RID: 31315 RVA: 0x002E19F4 File Offset: 0x002DFBF4
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

	// Token: 0x06007A54 RID: 31316 RVA: 0x002E1A5C File Offset: 0x002DFC5C
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

	// Token: 0x06007A55 RID: 31317 RVA: 0x002E1B2C File Offset: 0x002DFD2C
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

	// Token: 0x04005A77 RID: 23159
	public static string ID = "";

	// Token: 0x04005A78 RID: 23160
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005A79 RID: 23161
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GameEventsList gameEventsList;

	// Token: 0x04005A7A RID: 23162
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplay = "";

	// Token: 0x04005A7B RID: 23163
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxTarget;

	// Token: 0x04005A7C RID: 23164
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, EntityPlayer> PlayerList = new Dictionary<string, EntityPlayer>();
}
