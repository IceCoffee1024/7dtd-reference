using System;
using UnityEngine;

// Token: 0x02000C55 RID: 3157
public sealed class HeightMap : IDisposable
{
	// Token: 0x06005FA9 RID: 24489 RVA: 0x0025FAB8 File Offset: 0x0025DCB8
	public HeightMap(int _w, int _h, float _maxHeight, IBackedArray<ushort> _data, int _targetSize = 0)
	{
		this.w = _w;
		this.h = _h;
		this.scaleShift = ((_targetSize != 0) ? ((int)Mathf.Log((float)(_targetSize / _w), 2f)) : 0);
		this.scalePixs = _targetSize / _w;
		this.maxHeight = _maxHeight;
		this.data = BackedArrays.CreateSingleView<ushort>(_data, BackedArrayHandleMode.ReadOnly, 16 * _w);
	}

	// Token: 0x06005FAA RID: 24490 RVA: 0x0025FB19 File Offset: 0x0025DD19
	public void Dispose()
	{
		IBackedArrayView<ushort> backedArrayView = this.data;
		if (backedArrayView != null)
		{
			backedArrayView.Dispose();
		}
		this.data = null;
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x0025FB34 File Offset: 0x0025DD34
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

	// Token: 0x06005FAC RID: 24492 RVA: 0x0025FB90 File Offset: 0x0025DD90
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

	// Token: 0x06005FAD RID: 24493 RVA: 0x0025FCE4 File Offset: 0x0025DEE4
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

	// Token: 0x06005FAE RID: 24494 RVA: 0x0025FD32 File Offset: 0x0025DF32
	public int CalcOffset(int _x, int _z)
	{
		_x >>= this.scaleShift;
		_z >>= this.scaleShift;
		return _x + _z * this.w;
	}

	// Token: 0x06005FAF RID: 24495 RVA: 0x0025FD58 File Offset: 0x0025DF58
	public int GetWidth()
	{
		return this.w;
	}

	// Token: 0x06005FB0 RID: 24496 RVA: 0x0025FD60 File Offset: 0x0025DF60
	public int GetHeight()
	{
		return this.h;
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x0025FD68 File Offset: 0x0025DF68
	public int GetScaleSteps()
	{
		return this.scalePixs;
	}

	// Token: 0x06005FB2 RID: 24498 RVA: 0x0025FD70 File Offset: 0x0025DF70
	public int GetScaleShift()
	{
		return this.scaleShift;
	}

	// Token: 0x04004B30 RID: 19248
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int w;

	// Token: 0x04004B31 RID: 19249
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int h;

	// Token: 0x04004B32 RID: 19250
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int scaleShift;

	// Token: 0x04004B33 RID: 19251
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int scalePixs;

	// Token: 0x04004B34 RID: 19252
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly float maxHeight;

	// Token: 0x04004B35 RID: 19253
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayView<ushort> data;
}
