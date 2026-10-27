using System;
using UnityEngine;

// Token: 0x02000C60 RID: 3168
public sealed class HeightMap : IDisposable
{
	// Token: 0x06005FCE RID: 24526 RVA: 0x00260FE0 File Offset: 0x0025F1E0
	public HeightMap(int _w, int _h, float _maxHeight, IBackedArray<ushort> _data, int _targetSize = 0)
	{
		this.w = _w;
		this.h = _h;
		this.scaleShift = ((_targetSize != 0) ? ((int)Mathf.Log((float)(_targetSize / _w), 2f)) : 0);
		this.scalePixs = _targetSize / _w;
		this.maxHeight = _maxHeight;
		this.data = BackedArrays.CreateSingleView<ushort>(_data, BackedArrayHandleMode.ReadOnly, 16 * _w);
	}

	// Token: 0x06005FCF RID: 24527 RVA: 0x00261041 File Offset: 0x0025F241
	public void Dispose()
	{
		IBackedArrayView<ushort> backedArrayView = this.data;
		if (backedArrayView != null)
		{
			backedArrayView.Dispose();
		}
		this.data = null;
	}

	// Token: 0x06005FD0 RID: 24528 RVA: 0x0026105C File Offset: 0x0025F25C
	public float GetAt(int _x, int _z)
	{
		ushort num;
		if (this.scaleShift == 0 && _x + _z * this.w < this.data.Length)
		{
			num = this.data[_x + _z * this.w];
		}
		else
		{
			num = this.getInterpolatedHeight(_x, _z);
		}
		return (float)num * (this.maxHeight / 65535f);
	}

	// Token: 0x06005FD1 RID: 24529 RVA: 0x002610B8 File Offset: 0x0025F2B8
	public ushort getInterpolatedHeight(int xf, int zf)
	{
		object obj = ((xf >= 0) ? (xf >> this.scaleShift) : (xf - this.scalePixs + 1 >> this.scaleShift));
		int num = ((zf >= 0) ? (zf >> this.scaleShift) : (zf - this.scalePixs + 1 >> this.scaleShift));
		object obj2 = obj;
		int num2 = obj2 + num * this.w;
		ushort num3 = this.data[(num2 + this.w) & (this.data.Length - 1)];
		ushort num4 = this.data[num2 & (this.data.Length - 1)];
		ushort num5 = this.data[(num2 + 1) & (this.data.Length - 1)];
		ushort num6 = this.data[(num2 + 1 + this.w) & (this.data.Length - 1)];
		int num7 = obj2 << (this.scaleShift & 31);
		int num8 = num << this.scaleShift;
		float num9 = 1f - (float)(zf - num8) / (float)this.scalePixs;
		float num10 = (float)(xf - num7) / (float)this.scalePixs;
		return (ushort)((1f - num9) * ((1f - num10) * (float)num3 + num10 * (float)num6) + num9 * ((1f - num10) * (float)num4 + num10 * (float)num5));
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x0026120C File Offset: 0x0025F40C
	public float GetAt(int _offs)
	{
		ushort num;
		if (this.scaleShift == 0)
		{
			num = this.data[_offs];
		}
		else
		{
			int num2 = _offs / this.w;
			int num3 = _offs % this.w;
			num = this.getInterpolatedHeight(num3, num2);
		}
		return (float)num * this.maxHeight / 65535f;
	}

	// Token: 0x06005FD3 RID: 24531 RVA: 0x0026125A File Offset: 0x0025F45A
	public int CalcOffset(int _x, int _z)
	{
		_x >>= this.scaleShift;
		_z >>= this.scaleShift;
		return _x + _z * this.w;
	}

	// Token: 0x06005FD4 RID: 24532 RVA: 0x00261280 File Offset: 0x0025F480
	public int GetWidth()
	{
		return this.w;
	}

	// Token: 0x06005FD5 RID: 24533 RVA: 0x00261288 File Offset: 0x0025F488
	public int GetHeight()
	{
		return this.h;
	}

	// Token: 0x06005FD6 RID: 24534 RVA: 0x00261290 File Offset: 0x0025F490
	public int GetScaleSteps()
	{
		return this.scalePixs;
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x00261298 File Offset: 0x0025F498
	public int GetScaleShift()
	{
		return this.scaleShift;
	}

	// Token: 0x04004B55 RID: 19285
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int w;

	// Token: 0x04004B56 RID: 19286
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int h;

	// Token: 0x04004B57 RID: 19287
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int scaleShift;

	// Token: 0x04004B58 RID: 19288
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int scalePixs;

	// Token: 0x04004B59 RID: 19289
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly float maxHeight;

	// Token: 0x04004B5A RID: 19290
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayView<ushort> data;
}
