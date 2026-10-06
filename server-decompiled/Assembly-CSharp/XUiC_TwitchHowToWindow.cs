using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B4 RID: 4276
[Preserve]
public class XUiC_TwitchHowToWindow : XUiController
{
	// Token: 0x060087FF RID: 34815 RVA: 0x00341148 File Offset: 0x0033F348
	public override void Init()
	{
		base.Init();
		base.GetChildById("leftButton").OnPress += this.Left_OnPress;
		base.GetChildById("rightButton").OnPress += this.Right_OnPress;
		this.lblTipHeader = Localization.Get("TwitchInfo_TipHeader", false, null);
	}

	// Token: 0x06008800 RID: 34816 RVA: 0x003411A5 File Offset: 0x0033F3A5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Left_OnPress(XUiController _sender, int _mouseButton)
	{
		this.tipIndex--;
		if (this.tipIndex == -1)
		{
			this.tipIndex = this.TipNames.Count - 1;
		}
		base.RefreshBindings();
	}

	// Token: 0x06008801 RID: 34817 RVA: 0x003411D7 File Offset: 0x0033F3D7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Right_OnPress(XUiController _sender, int _mouseButton)
	{
		this.tipIndex++;
		if (this.tipIndex == this.TipNames.Count)
		{
			this.tipIndex = 0;
		}
		base.RefreshBindings();
	}

	// Token: 0x06008802 RID: 34818 RVA: 0x00341207 File Offset: 0x0033F407
	public override void OnOpen()
	{
		base.OnOpen();
		this.TipNames = TwitchManager.Current.tipTitleList;
		this.TipText = TwitchManager.Current.tipDescriptionList;
		base.RefreshBindings();
	}

	// Token: 0x06008803 RID: 34819 RVA: 0x00341238 File Offset: 0x0033F438
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "tipheader")
		{
			value = this.lblTipHeader;
			return true;
		}
		if (bindingName == "tiptitle")
		{
			value = ((this.TipNames.Count > 0) ? this.TipNames[this.tipIndex] : "");
			return true;
		}
		if (!(bindingName == "tiptext"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = ((this.TipText.Count > 0) ? this.TipText[this.tipIndex] : "");
		return true;
	}

	// Token: 0x04006559 RID: 25945
	[PublicizedFrom(EAccessModifier.Private)]
	public int tipIndex;

	// Token: 0x0400655A RID: 25946
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipNames = new List<string>();

	// Token: 0x0400655B RID: 25947
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipText = new List<string>();

	// Token: 0x0400655C RID: 25948
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTipHeader;
}
