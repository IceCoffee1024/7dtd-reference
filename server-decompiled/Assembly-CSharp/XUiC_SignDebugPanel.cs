using System;
using UnityEngine.Scripting;

// Token: 0x02001049 RID: 4169
[Preserve]
public class XUiC_SignDebugPanel : XUiController
{
	// Token: 0x06008339 RID: 33593 RVA: 0x0031F048 File Offset: 0x0031D248
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

	// Token: 0x0600833A RID: 33594 RVA: 0x0031F1A8 File Offset: 0x0031D3A8
	public void Clear()
	{
		this.SetAll("-");
	}

	// Token: 0x0600833B RID: 33595 RVA: 0x0031F1B8 File Offset: 0x0031D3B8
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

	// Token: 0x0600833C RID: 33596 RVA: 0x0031F2A8 File Offset: 0x0031D4A8
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

	// Token: 0x0400615C RID: 24924
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblGroupDepth;

	// Token: 0x0400615D RID: 24925
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblComplexity;

	// Token: 0x0400615E RID: 24926
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDescCount;

	// Token: 0x0400615F RID: 24927
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWarpCount;

	// Token: 0x04006160 RID: 24928
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDrawCount;

	// Token: 0x04006161 RID: 24929
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblBgnCompDepth;

	// Token: 0x04006162 RID: 24930
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxCompDepth;

	// Token: 0x04006163 RID: 24931
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblEndCompDepth;

	// Token: 0x04006164 RID: 24932
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxUVDepth;
}
