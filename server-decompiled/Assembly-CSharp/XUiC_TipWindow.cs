using System;
using UnityEngine.Scripting;

// Token: 0x02001087 RID: 4231
[Preserve]
public class XUiC_TipWindow : XUiController
{
	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x06008667 RID: 34407 RVA: 0x00333750 File Offset: 0x00331950
	// (set) Token: 0x06008668 RID: 34408 RVA: 0x00333758 File Offset: 0x00331958
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

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x06008669 RID: 34409 RVA: 0x00333768 File Offset: 0x00331968
	// (set) Token: 0x0600866A RID: 34410 RVA: 0x00333770 File Offset: 0x00331970
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

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x0600866B RID: 34411 RVA: 0x00333780 File Offset: 0x00331980
	// (set) Token: 0x0600866C RID: 34412 RVA: 0x00333788 File Offset: 0x00331988
	public ToolTipEvent CloseEvent { get; set; }

	// Token: 0x0600866D RID: 34413 RVA: 0x00333791 File Offset: 0x00331991
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

	// Token: 0x0600866E RID: 34414 RVA: 0x003337CB File Offset: 0x003319CB
	public override void Init()
	{
		base.Init();
		((XUiV_Button)base.GetChildById("clickable").ViewComponent).Controller.OnPress += this.closeButton_OnPress;
	}

	// Token: 0x0600866F RID: 34415 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06008670 RID: 34416 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06008671 RID: 34417 RVA: 0x00333800 File Offset: 0x00331A00
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

	// Token: 0x06008672 RID: 34418 RVA: 0x003338A3 File Offset: 0x00331AA3
	public override void OnClose()
	{
		base.OnClose();
		if (this.CloseEvent != null)
		{
			this.CloseEvent.HandleEvent();
			this.CloseEvent = null;
		}
	}

	// Token: 0x040063DF RID: 25567
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipText = "";

	// Token: 0x040063E0 RID: 25568
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipTitle = "";

	// Token: 0x040063E1 RID: 25569
	[PublicizedFrom(EAccessModifier.Private)]
	public string nextTip = "";
}
