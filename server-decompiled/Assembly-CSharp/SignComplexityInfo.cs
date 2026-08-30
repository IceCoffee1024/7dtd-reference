using System;
using System.Collections.Generic;

// Token: 0x02000A81 RID: 2689
public struct SignComplexityInfo
{
	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x0600512D RID: 20781 RVA: 0x001EF92D File Offset: 0x001EDB2D
	public bool IsValid
	{
		get
		{
			return this.ComplexityByLayer != null;
		}
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x0600512E RID: 20782 RVA: 0x001EF938 File Offset: 0x001EDB38
	public static SignComplexityInfo Invalid
	{
		get
		{
			return new SignComplexityInfo(0f, null, default(LayerStackInfo));
		}
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x001EF959 File Offset: 0x001EDB59
	public SignComplexityInfo(float totalComplexity, Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer, LayerStackInfo stackInfo)
	{
		this.TotalComplexity = totalComplexity;
		this.ComplexityByLayer = complexityByLayer;
		this.StackInfo = stackInfo;
	}

	// Token: 0x06005130 RID: 20784 RVA: 0x001EF970 File Offset: 0x001EDB70
	public bool TryGetLayerComplexityInfo(SignData.SignLayer layer, out LayerComplexityInfo layerComplexityInfo)
	{
		layerComplexityInfo = default(LayerComplexityInfo);
		return layer != null && this.ComplexityByLayer != null && this.ComplexityByLayer.TryGetValue(layer, out layerComplexityInfo);
	}

	// Token: 0x04003E63 RID: 15971
	public readonly float TotalComplexity;

	// Token: 0x04003E64 RID: 15972
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> ComplexityByLayer;

	// Token: 0x04003E65 RID: 15973
	public readonly LayerStackInfo StackInfo;
}
