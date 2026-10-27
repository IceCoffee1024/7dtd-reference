using System;
using UnityEngine.Scripting;

// Token: 0x020010A7 RID: 4263
[Preserve]
public class XUiC_TipWindow : XUiController
{
	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x060086FC RID: 34556 RVA: 0x0033910C File Offset: 0x0033730C
	// (set) Token: 0x060086FD RID: 34557 RVA: 0x00339114 File Offset: 0x00337314
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

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x060086FE RID: 34558 RVA: 0x00339124 File Offset: 0x00337324
	// (set) Token: 0x060086FF RID: 34559 RVA: 0x0033912C File Offset: 0x0033732C
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

	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06008700 RID: 34560 RVA: 0x0033913C File Offset: 0x0033733C
	// (set) Token: 0x06008701 RID: 34561 RVA: 0x00339144 File Offset: 0x00337344
	public ToolTipEvent CloseEvent { get; set; }

	// Token: 0x06008702 RID: 34562 RVA: 0x0033914D File Offset: 0x0033734D
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

	// Token: 0x06008703 RID: 34563 RVA: 0x00339187 File Offset: 0x00337387
	public override void Init()
	{
		base.Init();
		((XUiV_Button)base.GetChildById("clickable").ViewComponent).Controller.OnPress += this.closeButton_OnPress;
	}

	// Token: 0x06008704 RID: 34564 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06008705 RID: 34565 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008706 RID: 34566 RVA: 0x003391BC File Offset: 0x003373BC
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

	// Token: 0x06008707 RID: 34567 RVA: 0x0033925F File Offset: 0x0033745F
	public override void OnClose()
	{
		base.OnClose();
		if (this.CloseEvent != null)
		{
			this.CloseEvent.HandleEvent();
			this.CloseEvent = null;
		}
	}

	// Token: 0x0400645E RID: 25694
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipText = "";

	// Token: 0x0400645F RID: 25695
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipTitle = "";

	// Token: 0x04006460 RID: 25696
	[PublicizedFrom(EAccessModifier.Private)]
	public string nextTip = "";
}
