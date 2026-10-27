using System;
using System.Collections.Generic;

// Token: 0x02000AAB RID: 2731
public struct SignComplexityInfo
{
	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x060051CC RID: 20940 RVA: 0x001F54CD File Offset: 0x001F36CD
	public bool IsValid
	{
		get
		{
			return this.ComplexityByLayer != null;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x060051CD RID: 20941 RVA: 0x001F54D8 File Offset: 0x001F36D8
	public static SignComplexityInfo Invalid
	{
		get
		{
			return new SignComplexityInfo(0f, null, default(LayerStackInfo));
		}
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001F54F9 File Offset: 0x001F36F9
	public SignComplexityInfo(float totalComplexity, Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer, LayerStackInfo stackInfo)
	{
		this.TotalComplexity = totalComplexity;
		this.ComplexityByLayer = complexityByLayer;
		this.StackInfo = stackInfo;
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001F5510 File Offset: 0x001F3710
	public bool TryGetLayerComplexityInfo(SignData.SignLayer layer, out LayerComplexityInfo layerComplexityInfo)
	{
		layerComplexityInfo = default(LayerComplexityInfo);
		return layer != null && this.ComplexityByLayer != null && this.ComplexityByLayer.TryGetValue(layer, out layerComplexityInfo);
	}

	// Token: 0x04003F40 RID: 16192
	public readonly float TotalComplexity;

	// Token: 0x04003F41 RID: 16193
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> ComplexityByLayer;

	// Token: 0x04003F42 RID: 16194
	public readonly LayerStackInfo StackInfo;
}
