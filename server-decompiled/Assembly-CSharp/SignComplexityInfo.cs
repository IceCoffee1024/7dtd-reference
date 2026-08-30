using System;
using System.Collections.Generic;

// Token: 0x02000A92 RID: 2706
public struct SignComplexityInfo
{
	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060051B3 RID: 20915 RVA: 0x001F23C5 File Offset: 0x001F05C5
	public bool IsValid
	{
		get
		{
			return this.ComplexityByLayer != null;
		}
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060051B4 RID: 20916 RVA: 0x001F23D0 File Offset: 0x001F05D0
	public static SignComplexityInfo Invalid
	{
		get
		{
			return new SignComplexityInfo(0f, null, default(LayerStackInfo));
		}
	}

	// Token: 0x060051B5 RID: 20917 RVA: 0x001F23F1 File Offset: 0x001F05F1
	public SignComplexityInfo(float totalComplexity, Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer, LayerStackInfo stackInfo)
	{
		this.TotalComplexity = totalComplexity;
		this.ComplexityByLayer = complexityByLayer;
		this.StackInfo = stackInfo;
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x001F2408 File Offset: 0x001F0608
	public bool TryGetLayerComplexityInfo(SignData.SignLayer layer, out LayerComplexityInfo layerComplexityInfo)
	{
		layerComplexityInfo = default(LayerComplexityInfo);
		return layer != null && this.ComplexityByLayer != null && this.ComplexityByLayer.TryGetValue(layer, out layerComplexityInfo);
	}

	// Token: 0x04003EF4 RID: 16116
	public readonly float TotalComplexity;

	// Token: 0x04003EF5 RID: 16117
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> ComplexityByLayer;

	// Token: 0x04003EF6 RID: 16118
	public readonly LayerStackInfo StackInfo;
}
