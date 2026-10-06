using System;
using UnityEngine.Scripting;

// Token: 0x02001097 RID: 4247
[Preserve]
public class XUiC_TipWindow : XUiController
{
	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x060086A9 RID: 34473 RVA: 0x00337088 File Offset: 0x00335288
	// (set) Token: 0x060086AA RID: 34474 RVA: 0x00337090 File Offset: 0x00335290
	public string TipText
	{
		get
		{
			return this.tipText;
		}
		set
		{
			this.tipText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x060086AB RID: 34475 RVA: 0x003370A0 File Offset: 0x003352A0
	// (set) Token: 0x060086AC RID: 34476 RVA: 0x003370A8 File Offset: 0x003352A8
	public string TipTitle
	{
		get
		{
			return this.tipTitle;
		}
		set
		{
			this.tipTitle = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x060086AD RID: 34477 RVA: 0x003370B8 File Offset: 0x003352B8
	// (set) Token: 0x060086AE RID: 34478 RVA: 0x003370C0 File Offset: 0x003352C0
	public ToolTipEvent CloseEvent { get; set; }

	// Token: 0x060086AF RID: 34479 RVA: 0x003370C9 File Offset: 0x003352C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "tiptext")
		{
			value = this.TipText;
			return true;
		}
		if (!(bindingName == "tiptitle"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = this.TipTitle;
		return true;
	}

	// Token: 0x060086B0 RID: 34480 RVA: 0x00337103 File Offset: 0x00335303
	public override void Init()
	{
		base.Init();
		((XUiV_Button)base.GetChildById("clickable").ViewComponent).Controller.OnPress += this.closeButton_OnPress;
	}

	// Token: 0x060086B1 RID: 34481 RVA: 0x002AAE45 File Offset: 0x002A9045
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060086B2 RID: 34482 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060086B3 RID: 34483 RVA: 0x00337138 File Offset: 0x00335338
	public static void ShowTip(string _tip, string _title, EntityPlayerLocal _localPlayer, ToolTipEvent _closeEvent)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_localPlayer);
		if (uiforPlayer != null && uiforPlayer.xui != null && uiforPlayer.windowManager.IsHUDEnabled())
		{
			XUiC_TipWindow childByType = uiforPlayer.xui.FindWindowGroupByName("tipWindow").GetChildByType<XUiC_TipWindow>();
			childByType.TipText = Localization.Get(_tip, false, null);
			childByType.TipTitle = Localization.Get(_title, false, null);
			childByType.CloseEvent = _closeEvent;
			uiforPlayer.windowManager.Open("tipWindow", true);
			uiforPlayer.windowManager.Close("windowpaging");
			uiforPlayer.windowManager.Close("toolbelt");
		}
	}

	// Token: 0x060086B4 RID: 34484 RVA: 0x003371DB File Offset: 0x003353DB
	public override void OnClose()
	{
		base.OnClose();
		if (this.CloseEvent != null)
		{
			this.CloseEvent.HandleEvent();
			this.CloseEvent = null;
		}
	}

	// Token: 0x0400641F RID: 25631
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipText = "";

	// Token: 0x04006420 RID: 25632
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipTitle = "";

	// Token: 0x04006421 RID: 25633
	[PublicizedFrom(EAccessModifier.Private)]
	public string nextTip = "";
}
