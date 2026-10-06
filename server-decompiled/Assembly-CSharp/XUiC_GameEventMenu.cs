using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F45 RID: 3909
[Preserve]
public class XUiC_GameEventMenu : XUiController
{
	// Token: 0x06007A4D RID: 31309 RVA: 0x002E4C80 File Offset: 0x002E2E80
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

	// Token: 0x06007A4E RID: 31310 RVA: 0x002E4D13 File Offset: 0x002E2F13
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.gameEventsList.Category = _categoryEntry.CategoryName;
		this.categoryDisplay = _categoryEntry.CategoryDisplayName;
		base.RefreshBindings();
	}

	// Token: 0x06007A4F RID: 31311 RVA: 0x002E4D38 File Offset: 0x002E2F38
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_GameEventsList.GameEventEntry> _list, XUiC_GameEventsList.GameEventEntry _previousEntry, XUiC_GameEventsList.GameEventEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.gameEventsList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.name);
		}
	}

	// Token: 0x06007A50 RID: 31312 RVA: 0x002E4D64 File Offset: 0x002E2F64
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

	// Token: 0x06007A51 RID: 31313 RVA: 0x002E4DCC File Offset: 0x002E2FCC
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

	// Token: 0x06007A52 RID: 31314 RVA: 0x002E4E9C File Offset: 0x002E309C
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

	// Token: 0x04005AAF RID: 23215
	public static string ID = "";

	// Token: 0x04005AB0 RID: 23216
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005AB1 RID: 23217
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GameEventsList gameEventsList;

	// Token: 0x04005AB2 RID: 23218
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplay = "";

	// Token: 0x04005AB3 RID: 23219
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxTarget;

	// Token: 0x04005AB4 RID: 23220
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, EntityPlayer> PlayerList = new Dictionary<string, EntityPlayer>();
}
