using System;
using UnityEngine;

// Token: 0x02000926 RID: 2342
public class DecoSuppressArea : IDesignatedArea
{
	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06004415 RID: 17429 RVA: 0x001A84D8 File Offset: 0x001A66D8
	// (set) Token: 0x06004416 RID: 17430 RVA: 0x001A84E0 File Offset: 0x001A66E0
	public BoundsInt AreaBounds
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x001A84E9 File Offset: 0x001A66E9
	public DecoSuppressArea(Vector3i _position, Vector3i _size)
	{
		this.AreaBounds = new BoundsInt(_position, _size);
	}
}
