using System;
using UnityEngine;

// Token: 0x0200092B RID: 2347
public class DecoSuppressArea : IDesignatedArea
{
	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x060043A7 RID: 17319 RVA: 0x001A85DC File Offset: 0x001A67DC
	// (set) Token: 0x060043A8 RID: 17320 RVA: 0x001A85E4 File Offset: 0x001A67E4
	public BoundsInt AreaBounds
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060043A9 RID: 17321 RVA: 0x001A85ED File Offset: 0x001A67ED
	public DecoSuppressArea(Vector3i _position, Vector3i _size)
	{
		this.AreaBounds = new BoundsInt(_position, _size);
	}
}
