using System;
using System.Collections.Generic;

// Token: 0x02000AA1 RID: 2721
public struct SignComplexityInfo
{
	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x060051A8 RID: 20904 RVA: 0x001F43F1 File Offset: 0x001F25F1
	public bool IsValid
	{
		get
		{
			return this.ComplexityByLayer != null;
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001F43FC File Offset: 0x001F25FC
	public static SignComplexityInfo Invalid
	{
		get
		{
			return new SignComplexityInfo(0f, null, default(LayerStackInfo));
		}
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001F441D File Offset: 0x001F261D
	public SignComplexityInfo(float totalComplexity, Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer, LayerStackInfo stackInfo)
	{
		this.TotalComplexity = totalComplexity;
		this.ComplexityByLayer = complexityByLayer;
		this.StackInfo = stackInfo;
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001F4434 File Offset: 0x001F2634
	public bool TryGetLayerComplexityInfo(SignData.SignLayer layer, out LayerComplexityInfo layerComplexityInfo)
	{
		layerComplexityInfo = default(LayerComplexityInfo);
		return layer != null && this.ComplexityByLayer != null && this.ComplexityByLayer.TryGetValue(layer, out layerComplexityInfo);
	}

	// Token: 0x04003F1F RID: 16159
	public readonly float TotalComplexity;

	// Token: 0x04003F20 RID: 16160
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> ComplexityByLayer;

	// Token: 0x04003F21 RID: 16161
	public readonly LayerStackInfo StackInfo;
}
