using System;
using UnityEngine.Scripting;

// Token: 0x0200106E RID: 4206
[Preserve]
public class XUiC_TipWindow : XUiController
{
	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06008575 RID: 34165 RVA: 0x0032EF28 File Offset: 0x0032D128
	// (set) Token: 0x06008576 RID: 34166 RVA: 0x0032EF30 File Offset: 0x0032D130
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

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06008577 RID: 34167 RVA: 0x0032EF40 File Offset: 0x0032D140
	// (set) Token: 0x06008578 RID: 34168 RVA: 0x0032EF48 File Offset: 0x0032D148
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

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06008579 RID: 34169 RVA: 0x0032EF58 File Offset: 0x0032D158
	// (set) Token: 0x0600857A RID: 34170 RVA: 0x0032EF60 File Offset: 0x0032D160
	public ToolTipEvent CloseEvent { get; set; }

	// Token: 0x0600857B RID: 34171 RVA: 0x0032EF69 File Offset: 0x0032D169
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

	// Token: 0x0600857C RID: 34172 RVA: 0x0032EFA3 File Offset: 0x0032D1A3
	public override void Init()
	{
		base.Init();
		((XUiV_Button)base.GetChildById("clickable").ViewComponent).Controller.OnPress += this.closeButton_OnPress;
	}

	// Token: 0x0600857D RID: 34173 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600857E RID: 34174 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600857F RID: 34175 RVA: 0x0032EFD8 File Offset: 0x0032D1D8
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

	// Token: 0x06008580 RID: 34176 RVA: 0x0032F07B File Offset: 0x0032D27B
	public override void OnClose()
	{
		base.OnClose();
		if (this.CloseEvent != null)
		{
			this.CloseEvent.HandleEvent();
			this.CloseEvent = null;
		}
	}

	// Token: 0x04006318 RID: 25368
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipText = "";

	// Token: 0x04006319 RID: 25369
	[PublicizedFrom(EAccessModifier.Private)]
	public string tipTitle = "";

	// Token: 0x0400631A RID: 25370
	[PublicizedFrom(EAccessModifier.Private)]
	public string nextTip = "";
}
