using System;
using UnityEngine.Scripting;

// Token: 0x02001039 RID: 4153
[Preserve]
public class XUiC_SignDebugPanel : XUiController
{
	// Token: 0x060082E7 RID: 33511 RVA: 0x0031D344 File Offset: 0x0031B544
	public override void Init()
	{
		base.Init();
		this.lblGroupDepth = (XUiV_Label)base.GetChildById("lblGroupDepth").GetChildById("value").ViewComponent;
		this.lblComplexity = (XUiV_Label)base.GetChildById("lblComplexity").GetChildById("value").ViewComponent;
		this.lblDescCount = (XUiV_Label)base.GetChildById("lblDescCount").GetChildById("value").ViewComponent;
		this.lblWarpCount = (XUiV_Label)base.GetChildById("lblWarpCount").GetChildById("value").ViewComponent;
		this.lblDrawCount = (XUiV_Label)base.GetChildById("lblDrawCount").GetChildById("value").ViewComponent;
		this.lblBgnCompDepth = (XUiV_Label)base.GetChildById("lblBgnCompDepth").GetChildById("value").ViewComponent;
		this.lblMaxCompDepth = (XUiV_Label)base.GetChildById("lblMaxCompDepth").GetChildById("value").ViewComponent;
		this.lblEndCompDepth = (XUiV_Label)base.GetChildById("lblEndCompDepth").GetChildById("value").ViewComponent;
		this.lblMaxUVDepth = (XUiV_Label)base.GetChildById("lblMaxUVDepth").GetChildById("value").ViewComponent;
	}

	// Token: 0x060082E8 RID: 33512 RVA: 0x0031D4A4 File Offset: 0x0031B6A4
	public void Clear()
	{
		this.SetAll("-");
	}

	// Token: 0x060082E9 RID: 33513 RVA: 0x0031D4B4 File Offset: 0x0031B6B4
	public void Populate(in LayerComplexityInfo layerComplexityInfo)
	{
		XUiV_Label xuiV_Label = this.lblGroupDepth;
		int num = layerComplexityInfo.GroupDepth;
		xuiV_Label.Text = num.ToString();
		XUiV_Label xuiV_Label2 = this.lblComplexity;
		float complexity = layerComplexityInfo.Complexity;
		xuiV_Label2.Text = complexity.ToString();
		XUiV_Label xuiV_Label3 = this.lblDescCount;
		num = layerComplexityInfo.DescriptorCount;
		xuiV_Label3.Text = num.ToString();
		XUiV_Label xuiV_Label4 = this.lblWarpCount;
		num = layerComplexityInfo.WarpCount;
		xuiV_Label4.Text = num.ToString();
		XUiV_Label xuiV_Label5 = this.lblDrawCount;
		num = layerComplexityInfo.DrawCount;
		xuiV_Label5.Text = num.ToString();
		XUiV_Label xuiV_Label6 = this.lblBgnCompDepth;
		num = layerComplexityInfo.StartCompStackIndex;
		xuiV_Label6.Text = num.ToString();
		XUiV_Label xuiV_Label7 = this.lblMaxCompDepth;
		num = layerComplexityInfo.MaxCompStackIndex;
		xuiV_Label7.Text = num.ToString();
		XUiV_Label xuiV_Label8 = this.lblEndCompDepth;
		num = layerComplexityInfo.EndCompStackIndex;
		xuiV_Label8.Text = num.ToString();
		XUiV_Label xuiV_Label9 = this.lblMaxUVDepth;
		num = layerComplexityInfo.MaxUVStackIndex;
		xuiV_Label9.Text = num.ToString();
	}

	// Token: 0x060082EA RID: 33514 RVA: 0x0031D5A4 File Offset: 0x0031B7A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAll(string value)
	{
		this.lblGroupDepth.Text = value;
		this.lblComplexity.Text = value;
		this.lblDescCount.Text = value;
		this.lblWarpCount.Text = value;
		this.lblDrawCount.Text = value;
		this.lblBgnCompDepth.Text = value;
		this.lblMaxCompDepth.Text = value;
		this.lblEndCompDepth.Text = value;
		this.lblMaxUVDepth.Text = value;
	}

	// Token: 0x0400611E RID: 24862
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblGroupDepth;

	// Token: 0x0400611F RID: 24863
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblComplexity;

	// Token: 0x04006120 RID: 24864
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDescCount;

	// Token: 0x04006121 RID: 24865
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWarpCount;

	// Token: 0x04006122 RID: 24866
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDrawCount;

	// Token: 0x04006123 RID: 24867
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblBgnCompDepth;

	// Token: 0x04006124 RID: 24868
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxCompDepth;

	// Token: 0x04006125 RID: 24869
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblEndCompDepth;

	// Token: 0x04006126 RID: 24870
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxUVDepth;
}
