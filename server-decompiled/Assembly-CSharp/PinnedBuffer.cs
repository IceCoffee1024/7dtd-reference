using System;
using System.Runtime.CompilerServices;

// Token: 0x02000B7A RID: 2938
public abstract class PinnedBuffer : IDisposable
{
	// Token: 0x06005880 RID: 22656 RVA: 0x0021F8A8 File Offset: 0x0021DAA8
	public static PinnedBuffer<T> Create<[IsUnmanaged] T>(ArrayListMP<T> list, PinnedBuffer.Ownership ownership) where T : struct, ValueType
	{
		if (list == null || list.Count <= 0)
		{
			return new PinnedBuffer<T>();
		}
		switch (ownership)
		{
		case PinnedBuffer.Ownership.Pin:
			return new PinnedBuffer<T>(list.Items, list.Count);
		case PinnedBuffer.Ownership.Clone:
		{
			PinnedBuffer<T> pinnedBuffer = new PinnedBuffer<T>(list.pool, list.Count);
			list.Items.AsSpan<T>(0, list.Count).CopyTo(pinnedBuffer.AsSpan());
			return pinnedBuffer;
		}
		case PinnedBuffer.Ownership.Steal:
		{
			T[] items = list.Items;
			int count = list.Count;
			list.Items = null;
			list.Count = 0;
			return new PinnedBuffer<T>(list.pool, items, count);
		}
		default:
			throw new ArgumentOutOfRangeException("ownership", ownership, null);
		}
	}

	// Token: 0x06005881 RID: 22657
	public abstract void Dispose();

	// Token: 0x06005882 RID: 22658 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public PinnedBuffer()
	{
	}

	// Token: 0x02000B7B RID: 2939
	public enum Ownership
	{
		// Token: 0x040044B0 RID: 17584
		Pin,
		// Token: 0x040044B1 RID: 17585
		Clone,
		// Token: 0x040044B2 RID: 17586
		Steal
	}
}
