using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CB0 RID: 3248
public struct UnsafeBitArraySetIndicesEnumerator : IEnumerator<int>, IEnumerator, IDisposable
{
	// Token: 0x060063CB RID: 25547 RVA: 0x00271428 File Offset: 0x0026F628
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

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x060063CC RID: 25548 RVA: 0x00271488 File Offset: 0x0026F688
	public int Current
	{
		get
		{
			return this.currentIndex;
		}
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x060063CD RID: 25549 RVA: 0x00271490 File Offset: 0x0026F690
	public object System.Collections.IEnumerator.Current
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Current;
		}
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x002714A0 File Offset: 0x0026F6A0
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

	// Token: 0x060063CF RID: 25551 RVA: 0x00271665 File Offset: 0x0026F865
	public unsafe void Reset()
	{
		this.sliceIndex = 0;
		this.currentSlice = *this.bitArray.Ptr;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.foundBits = 0;
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x000027FC File Offset: 0x000009FC
	public void Dispose()
	{
	}

	// Token: 0x04004D77 RID: 19831
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeBitArray bitArray;

	// Token: 0x04004D78 RID: 19832
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong currentSlice;

	// Token: 0x04004D79 RID: 19833
	[PublicizedFrom(EAccessModifier.Private)]
	public int sliceIndex;

	// Token: 0x04004D7A RID: 19834
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSlices;

	// Token: 0x04004D7B RID: 19835
	[PublicizedFrom(EAccessModifier.Private)]
	public int leadingZeroCount;

	// Token: 0x04004D7C RID: 19836
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x04004D7D RID: 19837
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSetBits;

	// Token: 0x04004D7E RID: 19838
	[PublicizedFrom(EAccessModifier.Private)]
	public int foundBits;
}
