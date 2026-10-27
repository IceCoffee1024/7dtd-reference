using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010C4 RID: 4292
[Preserve]
public class XUiC_TwitchHowToWindow : XUiController
{
	// Token: 0x06008852 RID: 34898 RVA: 0x003431CC File Offset: 0x003413CC
	public override void Init()
	{
		base.Init();
		base.GetChildById("leftButton").OnPress += this.Left_OnPress;
		base.GetChildById("rightButton").OnPress += this.Right_OnPress;
		this.lblTipHeader = Localization.Get("TwitchInfo_TipHeader", false, null);
	}

	// Token: 0x06008853 RID: 34899 RVA: 0x00343229 File Offset: 0x00341429
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

	// Token: 0x06008854 RID: 34900 RVA: 0x0034325B File Offset: 0x0034145B
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

	// Token: 0x06008855 RID: 34901 RVA: 0x0034328B File Offset: 0x0034148B
	public override void OnOpen()
	{
		base.OnOpen();
		this.TipNames = TwitchManager.Current.tipTitleList;
		this.TipText = TwitchManager.Current.tipDescriptionList;
		base.RefreshBindings();
	}

	// Token: 0x06008856 RID: 34902 RVA: 0x003432BC File Offset: 0x003414BC
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

	// Token: 0x04006598 RID: 26008
	[PublicizedFrom(EAccessModifier.Private)]
	public int tipIndex;

	// Token: 0x04006599 RID: 26009
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipNames = new List<string>();

	// Token: 0x0400659A RID: 26010
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> TipText = new List<string>();

	// Token: 0x0400659B RID: 26011
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTipHeader;
}
