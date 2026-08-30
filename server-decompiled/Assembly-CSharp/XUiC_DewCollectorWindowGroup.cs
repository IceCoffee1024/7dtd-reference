using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F0A RID: 3850
[Preserve]
public class XUiC_DewCollectorWindowGroup : XUiController
{
	// Token: 0x060078D3 RID: 30931 RVA: 0x002D8F88 File Offset: 0x002D7188
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

	// Token: 0x060078D4 RID: 30932 RVA: 0x002D9090 File Offset: 0x002D7290
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

	// Token: 0x060078D5 RID: 30933 RVA: 0x002D91B4 File Offset: 0x002D73B4
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

	// Token: 0x060078D6 RID: 30934 RVA: 0x002D9228 File Offset: 0x002D7428
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

	// Token: 0x060078D7 RID: 30935 RVA: 0x002D92EC File Offset: 0x002D74EC
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

	// Token: 0x060078D8 RID: 30936 RVA: 0x002D93C0 File Offset: 0x002D75C0
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

	// Token: 0x060078D9 RID: 30937 RVA: 0x002D9475 File Offset: 0x002D7675
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_DewCollectorWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_DewCollectorWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_DewCollectorWindowGroup.ID);
	}

	// Token: 0x060078DA RID: 30938 RVA: 0x002D9496 File Offset: 0x002D7696
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		TileEntityCollector tileEntityCollector = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).te;
		if (tileEntityCollector == null)
		{
			return Vector3i.zero;
		}
		return tileEntityCollector.ToWorldPos();
	}

	// Token: 0x060078DB RID: 30939 RVA: 0x002D94B4 File Offset: 0x002D76B4
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_DewCollectorWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_DewCollectorWindowGroup.ID) && XUiC_DewCollectorWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_DewCollectorWindowGroup.ID);
		}
	}

	// Token: 0x04005926 RID: 22822
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorWindow dewCatcherWindow;

	// Token: 0x04005927 RID: 22823
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorModGrid dewCollectorModGrid;

	// Token: 0x04005928 RID: 22824
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label headerName;

	// Token: 0x04005929 RID: 22825
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400592A RID: 22826
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DewCollectorStack[] collectorStacks;

	// Token: 0x0400592B RID: 22827
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector te;

	// Token: 0x0400592C RID: 22828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController fuelWindow;

	// Token: 0x0400592D RID: 22829
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label fuelWindowTitle;

	// Token: 0x0400592E RID: 22830
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite fuelWindowSprite;

	// Token: 0x0400592F RID: 22831
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorFuelGrid;

	// Token: 0x04005930 RID: 22832
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController catalystWindow;

	// Token: 0x04005931 RID: 22833
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label catalystWindowTitle;

	// Token: 0x04005932 RID: 22834
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite catalystWindowSprite;

	// Token: 0x04005933 RID: 22835
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid collectorCatalyst;

	// Token: 0x04005934 RID: 22836
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x04005935 RID: 22837
	public static string ID = "dewcollector";

	// Token: 0x04005936 RID: 22838
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalOpenTime;
}
