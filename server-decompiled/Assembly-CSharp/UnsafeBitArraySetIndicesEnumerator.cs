using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CC2 RID: 3266
public struct UnsafeBitArraySetIndicesEnumerator : IEnumerator<int>, IEnumerator, IDisposable
{
	// Token: 0x06006457 RID: 25687 RVA: 0x00274324 File Offset: 0x00272524
	public unsafe UnsafeBitArraySetIndicesEnumerator(UnsafeBitArray bitArray)
	{
		this.bitArray = bitArray;
		this.currentSlice = *bitArray.Ptr;
		this.sliceIndex = 0;
		this.numSlices = bitArray.Length / 64;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.numSetBits = bitArray.CountBits(0, bitArray.Length);
		this.foundBits = 0;
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x06006458 RID: 25688 RVA: 0x00274384 File Offset: 0x00272584
	public int Current
	{
		get
		{
			return this.currentIndex;
		}
	}

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06006459 RID: 25689 RVA: 0x0027438C File Offset: 0x0027258C
	public object System.Collections.IEnumerator.Current
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Current;
		}
	}

	// Token: 0x0600645A RID: 25690 RVA: 0x0027439C File Offset: 0x0027259C
	public unsafe bool MoveNext()
	{
		while (this.foundBits < this.numSetBits && this.sliceIndex < this.numSlices)
		{
			if (this.currentSlice == 0UL)
			{
				this.sliceIndex++;
				if (this.sliceIndex < this.numSlices)
				{
					this.currentSlice = this.bitArray.Ptr[this.sliceIndex];
					this.leadingZeroCount = 0;
				}
			}
			else
			{
				if ((this.currentSlice & 1UL) != 0UL)
				{
					this.currentIndex = this.leadingZeroCount + this.sliceIndex * 64;
					this.leadingZeroCount++;
					this.currentSlice >>= 1;
					this.foundBits++;
					return true;
				}
				if ((this.currentSlice & (ulong)(-1)) == 0UL)
				{
					this.leadingZeroCount += 32;
					this.currentSlice >>= 32;
				}
				if ((this.currentSlice & 65535UL) == 0UL)
				{
					this.leadingZeroCount += 16;
					this.currentSlice >>= 16;
				}
				if ((this.currentSlice & 255UL) == 0UL)
				{
					this.leadingZeroCount += 8;
					this.currentSlice >>= 8;
				}
				if ((this.currentSlice & 15UL) == 0UL)
				{
					this.leadingZeroCount += 4;
					this.currentSlice >>= 4;
				}
				if ((this.currentSlice & 3UL) == 0UL)
				{
					this.leadingZeroCount += 2;
					this.currentSlice >>= 2;
				}
				if ((this.currentSlice & 1UL) == 0UL)
				{
					this.leadingZeroCount++;
					this.currentSlice >>= 1;
				}
			}
		}
		return false;
	}

	// Token: 0x0600645B RID: 25691 RVA: 0x00274561 File Offset: 0x00272761
	public unsafe void Reset()
	{
		this.sliceIndex = 0;
		this.currentSlice = *this.bitArray.Ptr;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.foundBits = 0;
	}

	// Token: 0x0600645C RID: 25692 RVA: 0x000027FC File Offset: 0x000009FC
	public void Dispose()
	{
	}

	// Token: 0x04004E15 RID: 19989
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeBitArray bitArray;

	// Token: 0x04004E16 RID: 19990
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong currentSlice;

	// Token: 0x04004E17 RID: 19991
	[PublicizedFrom(EAccessModifier.Private)]
	public int sliceIndex;

	// Token: 0x04004E18 RID: 19992
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSlices;

	// Token: 0x04004E19 RID: 19993
	[PublicizedFrom(EAccessModifier.Private)]
	public int leadingZeroCount;

	// Token: 0x04004E1A RID: 19994
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x04004E1B RID: 19995
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSetBits;

	// Token: 0x04004E1C RID: 19996
	[PublicizedFrom(EAccessModifier.Private)]
	public int foundBits;
}
