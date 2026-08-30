using System;
using UnityEngine.Scripting;

// Token: 0x02001011 RID: 4113
[Preserve]
public class XUiC_SignDebugPanel : XUiController
{
	// Token: 0x060081E6 RID: 33254 RVA: 0x00315AD4 File Offset: 0x00313CD4
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

	// Token: 0x060081E7 RID: 33255 RVA: 0x00315C34 File Offset: 0x00313E34
	public void Clear()
	{
		this.SetAll("-");
	}

	// Token: 0x060081E8 RID: 33256 RVA: 0x00315C44 File Offset: 0x00313E44
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

	// Token: 0x060081E9 RID: 33257 RVA: 0x00315D34 File Offset: 0x00313F34
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

	// Token: 0x0400602E RID: 24622
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblGroupDepth;

	// Token: 0x0400602F RID: 24623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblComplexity;

	// Token: 0x04006030 RID: 24624
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDescCount;

	// Token: 0x04006031 RID: 24625
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWarpCount;

	// Token: 0x04006032 RID: 24626
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblDrawCount;

	// Token: 0x04006033 RID: 24627
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblBgnCompDepth;

	// Token: 0x04006034 RID: 24628
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxCompDepth;

	// Token: 0x04006035 RID: 24629
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblEndCompDepth;

	// Token: 0x04006036 RID: 24630
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblMaxUVDepth;
}
