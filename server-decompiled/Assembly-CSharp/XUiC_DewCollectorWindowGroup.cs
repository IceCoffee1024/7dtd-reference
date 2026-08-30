using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F21 RID: 3873
[Preserve]
public class XUiC_DewCollectorWindowGroup : XUiController
{
	// Token: 0x0600798B RID: 31115 RVA: 0x002DCF14 File Offset: 0x002DB114
	public override void Init()
	{
		base.Init();
		this.dewCatcherWindow = base.GetChildByType<XUiC_DewCollectorWindow>();
		this.dewCollectorModGrid = base.GetChildByType<XUiC_DewCollectorModGrid>();
		this.collectorStacks = base.GetChildrenByType<XUiC_DewCollectorStack>(null);
		this.fuelWindow = base.GetChildById("windowCollectorFuel");
		this.fuelWindowTitle = this.fuelWindow.GetChildById("windowCollectorFuelTitle").ViewComponent as XUiV_Label;
		this.fuelWindowSprite = this.fuelWindow.GetChildById("windowCollectorFuelSprite").ViewComponent as XUiV_Sprite;
		this.collectorFuelGrid = this.fuelWindow.GetChildByType<XUiC_CollectorFuelGrid>();
		this.catalystWindow = base.GetChildById("windowCollectorCatalyst");
		this.catalystWindowTitle = this.catalystWindow.GetChildById("windowCollectorFuelTitle").ViewComponent as XUiV_Label;
		this.catalystWindowSprite = this.catalystWindow.GetChildById("windowCollectorFuelSprite").ViewComponent as XUiV_Sprite;
		this.collectorCatalyst = this.catalystWindow.GetChildByType<XUiC_CollectorFuelGrid>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x0600798C RID: 31116 RVA: 0x002DD01C File Offset: 0x002DB21C
	public void SetTileEntity(TileEntityCollector _te)
	{
		this.te = _te;
		BlockCollector blockCollector = this.te.blockValue.Block as BlockCollector;
		this.fuelWindowTitle.Text = Localization.Get(blockCollector.FuelTitleText, false, null);
		this.fuelWindowSprite.UIAtlas = blockCollector.FuelTitleSpriteAtlas;
		this.fuelWindowSprite.SpriteName = blockCollector.FuelTitleSprite;
		this.catalystWindowTitle.Text = Localization.Get(blockCollector.CatalystTitleText, false, null);
		this.catalystWindowSprite.UIAtlas = blockCollector.CatalystTitleSpriteAtlas;
		this.catalystWindowSprite.SpriteName = blockCollector.CatalystTitleSprite;
		this.dewCatcherWindow.SetTileEntity(_te);
		this.dewCollectorModGrid.SetTileEntity(_te);
		this.collectorFuelGrid.SetTileEntity(_te);
		this.collectorCatalyst.SetTileEntity(_te);
		int num = this.collectorStacks.Length;
		for (int i = 0; i < num; i++)
		{
			XUiC_DewCollectorStack xuiC_DewCollectorStack = this.collectorStacks[i];
			xuiC_DewCollectorStack.SetStandardFillColor(blockCollector.ItemBackgroundColor);
			xuiC_DewCollectorStack.SetSprite(blockCollector.ItemIconBackdrop[i % blockCollector.ItemIconBackdrop.Length]);
		}
		this.lootingHeader = Localization.Get(blockCollector.LootLabelKey, false, null);
	}

	// Token: 0x0600798D RID: 31117 RVA: 0x002DD140 File Offset: 0x002DB340
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OpenContainer()
	{
		base.OnOpen();
		this.xui.playerUI.windowManager.Open("backpack", false);
		this.dewCatcherWindow.ViewComponent.IsVisible = true;
		this.dewCatcherWindow.OpenContainer();
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(this.lootingHeader);
		}
		this.dewCatcherWindow.ViewComponent.IsVisible = true;
	}

	// Token: 0x0600798E RID: 31118 RVA: 0x002DD1B4 File Offset: 0x002DB3B4
	public override void OnOpen()
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader("LOOTING");
		}
		this.OpenContainer();
		BlockCollector blockCollector = this.te.blockValue.Block as BlockCollector;
		if (blockCollector != null)
		{
			this.fuelWindow.ViewComponent.IsVisible = blockCollector.UsesFuel();
			this.catalystWindow.ViewComponent.IsVisible = blockCollector.UsesCatalyst();
			string openSound = blockCollector.OpenSound;
			Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, openSound);
			this.doEvent(blockCollector.ActivationEvent);
		}
	}

	// Token: 0x0600798F RID: 31119 RVA: 0x002DD278 File Offset: 0x002DB478
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("backpack");
		this.te.ToWorldPos();
		this.te.SetUserAccessing(false);
		this.te.SetModified();
		LockManager.Instance.UnlockRequestLocal();
		if (this.te.blockValue.Block != null)
		{
			BlockCollector blockCollector = this.te.blockValue.Block as BlockCollector;
			if (blockCollector != null)
			{
				string closeSound = blockCollector.CloseSound;
				Manager.BroadcastPlayByLocalPlayer(this.te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, closeSound);
				string runningSound = blockCollector.RunningSound;
				this.doEvent(blockCollector.CloseEvent);
			}
		}
	}

	// Token: 0x06007990 RID: 31120 RVA: 0x002DD34C File Offset: 0x002DB54C
	[PublicizedFrom(EAccessModifier.Private)]
	public void doEvent(string activationEvent)
	{
		if (!string.IsNullOrEmpty(activationEvent))
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			int num = 1;
			foreach (ItemStack itemStack in this.te.ModSlots)
			{
				string text = "";
				if (!itemStack.IsEmpty())
				{
					text = itemStack.itemValue.ItemClass.Name;
				}
				list.Add(new Tuple<string, string>(string.Format("_Mod{0}Name", num++), text));
			}
			GameEventManager.Current.HandleAction(activationEvent, null, null, false, this.te.ToWorldCenterPos(), "", "", false, true, "", null, list);
		}
	}

	// Token: 0x06007991 RID: 31121 RVA: 0x002DD401 File Offset: 0x002DB601
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_DewCollectorWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_DewCollectorWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_DewCollectorWindowGroup.ID);
	}

	// Token: 0x06007992 RID: 31122 RVA: 0x002DD422 File Offset: 0x002DB622
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		TileEntityCollector tileEntityCollector = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).te;
		if (tileEntityCollector == null)
		{
			return Vector3i.zero;
		}
		return tileEntityCollector.ToWorldPos();
	}

	// Token: 0x06007993 RID: 31123 RVA: 0x002DD440 File Offset: 0x002DB640
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_DewCollectorWindowGroup.ID) && XUiC_DewCollectorWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_DewCollectorWindowGroup.ID);
		}
	}

	// Token: 0x040059D8 RID: 23000
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorWindow dewCatcherWindow;

	// Token: 0x040059D9 RID: 23001
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorModGrid dewCollectorModGrid;

	// Token: 0x040059DA RID: 23002
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label headerName;

	// Token: 0x040059DB RID: 23003
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040059DC RID: 23004
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorStack[] collectorStacks;

	// Token: 0x040059DD RID: 23005
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x040059DE RID: 23006
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController fuelWindow;

	// Token: 0x040059DF RID: 23007
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label fuelWindowTitle;

	// Token: 0x040059E0 RID: 23008
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite fuelWindowSprite;

	// Token: 0x040059E1 RID: 23009
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorFuelGrid;

	// Token: 0x040059E2 RID: 23010
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController catalystWindow;

	// Token: 0x040059E3 RID: 23011
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label catalystWindowTitle;

	// Token: 0x040059E4 RID: 23012
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite catalystWindowSprite;

	// Token: 0x040059E5 RID: 23013
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorCatalyst;

	// Token: 0x040059E6 RID: 23014
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x040059E7 RID: 23015
	public static string ID = "dewcollector";

	// Token: 0x040059E8 RID: 23016
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalOpenTime;
}
