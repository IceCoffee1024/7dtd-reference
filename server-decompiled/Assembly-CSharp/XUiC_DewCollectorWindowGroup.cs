using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F3F RID: 3903
[Preserve]
public class XUiC_DewCollectorWindowGroup : XUiController
{
	// Token: 0x060079C9 RID: 31177 RVA: 0x002E1BA0 File Offset: 0x002DFDA0
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

	// Token: 0x060079CA RID: 31178 RVA: 0x002E1CA8 File Offset: 0x002DFEA8
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

	// Token: 0x060079CB RID: 31179 RVA: 0x002E1DCC File Offset: 0x002DFFCC
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

	// Token: 0x060079CC RID: 31180 RVA: 0x002E1E40 File Offset: 0x002E0040
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

	// Token: 0x060079CD RID: 31181 RVA: 0x002E1F04 File Offset: 0x002E0104
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

	// Token: 0x060079CE RID: 31182 RVA: 0x002E1FD8 File Offset: 0x002E01D8
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

	// Token: 0x060079CF RID: 31183 RVA: 0x002E208D File Offset: 0x002E028D
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_DewCollectorWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_DewCollectorWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_DewCollectorWindowGroup.ID);
	}

	// Token: 0x060079D0 RID: 31184 RVA: 0x002E20AE File Offset: 0x002E02AE
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		TileEntityCollector tileEntityCollector = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).te;
		if (tileEntityCollector == null)
		{
			return Vector3i.zero;
		}
		return tileEntityCollector.ToWorldPos();
	}

	// Token: 0x060079D1 RID: 31185 RVA: 0x002E20CC File Offset: 0x002E02CC
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_DewCollectorWindowGroup.ID) && XUiC_DewCollectorWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_DewCollectorWindowGroup.ID);
		}
	}

	// Token: 0x04005A43 RID: 23107
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorWindow dewCatcherWindow;

	// Token: 0x04005A44 RID: 23108
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorModGrid dewCollectorModGrid;

	// Token: 0x04005A45 RID: 23109
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label headerName;

	// Token: 0x04005A46 RID: 23110
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005A47 RID: 23111
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorStack[] collectorStacks;

	// Token: 0x04005A48 RID: 23112
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x04005A49 RID: 23113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController fuelWindow;

	// Token: 0x04005A4A RID: 23114
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label fuelWindowTitle;

	// Token: 0x04005A4B RID: 23115
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite fuelWindowSprite;

	// Token: 0x04005A4C RID: 23116
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorFuelGrid;

	// Token: 0x04005A4D RID: 23117
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController catalystWindow;

	// Token: 0x04005A4E RID: 23118
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label catalystWindowTitle;

	// Token: 0x04005A4F RID: 23119
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite catalystWindowSprite;

	// Token: 0x04005A50 RID: 23120
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorCatalyst;

	// Token: 0x04005A51 RID: 23121
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x04005A52 RID: 23122
	public static string ID = "dewcollector";

	// Token: 0x04005A53 RID: 23123
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalOpenTime;
}
