using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A3 RID: 4259
[Preserve]
public class XUiC_TwitchHowToWindow : XUiController
{
	// Token: 0x060087B0 RID: 34736 RVA: 0x0033D330 File Offset: 0x0033B530
	public override void Init()
	{
		base.Init();
		base.GetChildById("leftButton").OnPress += this.Left_OnPress;
		base.GetChildById("rightButton").OnPress += this.Right_OnPress;
		this.lblTipHeader = Localization.Get("TwitchInfo_TipHeader", false, null);
	}

	// Token: 0x060087B1 RID: 34737 RVA: 0x0033D38D File Offset: 0x0033B58D
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

	// Token: 0x060087B2 RID: 34738 RVA: 0x0033D3BF File Offset: 0x0033B5BF
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

	// Token: 0x060087B3 RID: 34739 RVA: 0x0033D3EF File Offset: 0x0033B5EF
	public override void OnOpen()
	{
		base.OnOpen();
		this.TipNames = TwitchManager.Current.tipTitleList;
		this.TipText = TwitchManager.Current.tipDescriptionList;
		base.RefreshBindings();
	}

	// Token: 0x060087B4 RID: 34740 RVA: 0x0033D420 File Offset: 0x0033B620
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

	// Token: 0x04006517 RID: 25879
	[PublicizedFrom(EAccessModifier.Private)]
	public int tipIndex;

	// Token: 0x04006518 RID: 25880
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipNames = new List<string>();

	// Token: 0x04006519 RID: 25881
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipText = new List<string>();

	// Token: 0x0400651A RID: 25882
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTipHeader;
}
