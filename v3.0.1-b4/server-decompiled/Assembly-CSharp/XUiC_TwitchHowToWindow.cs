using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200108A RID: 4234
[Preserve]
public class XUiC_TwitchHowToWindow : XUiController
{
	// Token: 0x060086BE RID: 34494 RVA: 0x00338B08 File Offset: 0x00336D08
	public override void Init()
	{
		base.Init();
		base.GetChildById("leftButton").OnPress += this.Left_OnPress;
		base.GetChildById("rightButton").OnPress += this.Right_OnPress;
		this.lblTipHeader = Localization.Get("TwitchInfo_TipHeader", false, null);
	}

	// Token: 0x060086BF RID: 34495 RVA: 0x00338B65 File Offset: 0x00336D65
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

	// Token: 0x060086C0 RID: 34496 RVA: 0x00338B97 File Offset: 0x00336D97
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

	// Token: 0x060086C1 RID: 34497 RVA: 0x00338BC7 File Offset: 0x00336DC7
	public override void OnOpen()
	{
		base.OnOpen();
		this.TipNames = TwitchManager.Current.tipTitleList;
		this.TipText = TwitchManager.Current.tipDescriptionList;
		base.RefreshBindings();
	}

	// Token: 0x060086C2 RID: 34498 RVA: 0x00338BF8 File Offset: 0x00336DF8
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

	// Token: 0x04006450 RID: 25680
	[PublicizedFrom(EAccessModifier.Private)]
	public int tipIndex;

	// Token: 0x04006451 RID: 25681
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipNames = new List<string>();

	// Token: 0x04006452 RID: 25682
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipText = new List<string>();

	// Token: 0x04006453 RID: 25683
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTipHeader;
}
