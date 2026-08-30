using System;
using UnityEngine.Scripting;

// Token: 0x0200102A RID: 4138
[Preserve]
public class XUiC_SignDebugPanel : XUiController
{
	// Token: 0x060082D8 RID: 33496 RVA: 0x0031A2FC File Offset: 0x003184FC
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

	// Token: 0x060082D9 RID: 33497 RVA: 0x0031A45C File Offset: 0x0031865C
	public void Clear()
	{
		this.SetAll("-");
	}

	// Token: 0x060082DA RID: 33498 RVA: 0x0031A46C File Offset: 0x0031866C
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

	// Token: 0x060082DB RID: 33499 RVA: 0x0031A55C File Offset: 0x0031875C
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

	// Token: 0x040060F5 RID: 24821
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblGroupDepth;

	// Token: 0x040060F6 RID: 24822
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblComplexity;

	// Token: 0x040060F7 RID: 24823
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDescCount;

	// Token: 0x040060F8 RID: 24824
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWarpCount;

	// Token: 0x040060F9 RID: 24825
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDrawCount;

	// Token: 0x040060FA RID: 24826
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblBgnCompDepth;

	// Token: 0x040060FB RID: 24827
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxCompDepth;

	// Token: 0x040060FC RID: 24828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblEndCompDepth;

	// Token: 0x040060FD RID: 24829
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxUVDepth;
}
