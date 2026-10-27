using System;
using System.Runtime.CompilerServices;

// Token: 0x02000B85 RID: 2949
public abstract class PinnedBuffer : IDisposable
{
	// Token: 0x060058A5 RID: 22693 RVA: 0x00220E04 File Offset: 0x0021F004
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

	// Token: 0x060058A6 RID: 22694
	public abstract void Dispose();

	// Token: 0x060058A7 RID: 22695 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public PinnedBuffer()
	{
	}

	// Token: 0x02000B86 RID: 2950
	public enum Ownership
	{
		// Token: 0x040044D5 RID: 17621
		Pin,
		// Token: 0x040044D6 RID: 17622
		Clone,
		// Token: 0x040044D7 RID: 17623
		Steal
	}
}
