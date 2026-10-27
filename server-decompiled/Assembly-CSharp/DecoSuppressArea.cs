using System;
using UnityEngine;

// Token: 0x02000935 RID: 2357
public class DecoSuppressArea : IDesignatedArea
{
	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x060043CA RID: 17354 RVA: 0x001A9660 File Offset: 0x001A7860
	// (set) Token: 0x060043CB RID: 17355 RVA: 0x001A9668 File Offset: 0x001A7868
	public BoundsInt AreaBounds
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060043CC RID: 17356 RVA: 0x001A9671 File Offset: 0x001A7871
	public DecoSuppressArea(Vector3i _position, Vector3i _size)
	{
		this.AreaBounds = new BoundsInt(_position, _size);
	}
}
