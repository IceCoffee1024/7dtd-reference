using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000AF9 RID: 2809
public class ChunkBlockChannel
{
	// Token: 0x0600548E RID: 21646 RVA: 0x00205A90 File Offset: 0x00203C90
	public ChunkBlockChannel(long _defaultValue, int _bytesPerVal = 1)
	{
		this.defaultValue = _defaultValue;
		this.bytesPerVal = _bytesPerVal;
		this.sameValue = new byte[64 * this.bytesPerVal];
		this.fillSameValue(-1L);
		this.layers = new CBCLayer[64 * this.bytesPerVal];
	}

	// Token: 0x0600548F RID: 21647 RVA: 0x00205AE1 File Offset: 0x00203CE1
	[PublicizedFrom(EAccessModifier.Private)]
	public CBCLayer allocLayer()
	{
		return MemoryPools.poolCBC.AllocSync(true);
	}

	// Token: 0x06005490 RID: 21648 RVA: 0x00205AEE File Offset: 0x00203CEE
	[PublicizedFrom(EAccessModifier.Private)]
	public void freeLayer(int _idx)
	{
		if (this.layers[_idx] == null)
		{
			return;
		}
		MemoryPools.poolCBC.FreeSync(this.layers[_idx]);
		this.layers[_idx] = null;
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x00205B16 File Offset: 0x00203D16
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int calcOffset(int _x, int _y, int _z)
	{
		return _x + _z * 16 + (_y & 3) * 256;
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x00205B28 File Offset: 0x00203D28
	[PublicizedFrom(EAccessModifier.Private)]
	public void fillSameValue(long _value = -1L)
	{
		long num = ((_value == -1L) ? this.defaultValue : _value);
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			byte b = (byte)(num >> i * 8);
			for (int j = 63; j >= 0; j--)
			{
				this.sameValue[j * this.bytesPerVal + i] = b;
			}
		}
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x00205B80 File Offset: 0x00203D80
	[PublicizedFrom(EAccessModifier.Private)]
	public long getSameValue(int _idx)
	{
		long num = 0L;
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			num |= (long)((long)((ulong)this.sameValue[_idx + i]) << i * 8);
		}
		return num;
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x00205BB8 File Offset: 0x00203DB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSameValue(int _idx, long _value)
	{
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			this.sameValue[_idx + i] = (byte)_value;
			_value >>= 8;
		}
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x00205BE8 File Offset: 0x00203DE8
	[PublicizedFrom(EAccessModifier.Private)]
	public long getData(int _idx, int _offs)
	{
		long num = 0L;
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			CBCLayer cbclayer = this.layers[_idx + i];
			if (cbclayer == null)
			{
				break;
			}
			num |= (long)((long)((ulong)cbclayer.data[_offs]) << i * 8);
		}
		return num;
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x00205C2C File Offset: 0x00203E2C
	[PublicizedFrom(EAccessModifier.Private)]
	public long getSetData(int _idx, int _offs, long _value)
	{
		long num = 0L;
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			CBCLayer cbclayer = this.layers[_idx + i];
			if (cbclayer == null)
			{
				break;
			}
			num |= (long)((long)((ulong)cbclayer.data[_offs]) << i * 8);
			cbclayer.data[_offs] = (byte)_value;
			_value >>= 8;
		}
		return num;
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x00205C80 File Offset: 0x00203E80
	public long GetSet(int _x, int _y, int _z, long _value)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		if (this.layers[num] == null)
		{
			long num2 = this.getSameValue(num);
			if (num2 == _value)
			{
				return _value;
			}
			for (int i = 0; i < this.bytesPerVal; i++)
			{
				CBCLayer cbclayer = this.allocLayer();
				this.layers[num + i] = cbclayer;
				byte b = (byte)(num2 >> i * 8);
				for (int j = 1023; j >= 0; j--)
				{
					cbclayer.data[j] = b;
				}
			}
		}
		int num3 = ChunkBlockChannel.calcOffset(_x, _y, _z);
		return this.getSetData(num, num3, _value);
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x00205D1C File Offset: 0x00203F1C
	public void Set(int _x, int _y, int _z, long _value)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		if (this.layers[num] == null)
		{
			long num2 = this.getSameValue(num);
			if (num2 == _value)
			{
				return;
			}
			for (int i = 0; i < this.bytesPerVal; i++)
			{
				CBCLayer cbclayer = this.allocLayer();
				this.layers[num + i] = cbclayer;
				byte b = (byte)(num2 >> i * 8);
				for (int j = 1023; j >= 0; j--)
				{
					cbclayer.data[j] = b;
				}
			}
		}
		int num3 = ChunkBlockChannel.calcOffset(_x, _y, _z);
		for (int k = 0; k < this.bytesPerVal; k++)
		{
			CBCLayer cbclayer = this.layers[num + k];
			if (cbclayer == null)
			{
				break;
			}
			cbclayer.data[num3] = (byte)_value;
			_value >>= 8;
		}
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x00205DE0 File Offset: 0x00203FE0
	public long Get(int _x, int _y, int _z)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		if (num < 0)
		{
			return 0L;
		}
		CBCLayer cbclayer = this.layers[num];
		if (cbclayer == null)
		{
			return this.getSameValue(num);
		}
		int num2 = ChunkBlockChannel.calcOffset(_x, _y, _z);
		if (this.bytesPerVal == 1)
		{
			return (long)((ulong)cbclayer.data[num2]);
		}
		return this.getData(num, num2);
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x00205E38 File Offset: 0x00204038
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetByte(int _x, int _y, int _z)
	{
		int num = _y >> 2;
		if (num >= 64)
		{
			return 0;
		}
		CBCLayer cbclayer = this.layers[num];
		if (cbclayer == null)
		{
			return this.sameValue[num];
		}
		int num2 = ChunkBlockChannel.calcOffset(_x, _y, _z);
		return cbclayer.data[num2];
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x00205E78 File Offset: 0x00204078
	public void Read(BinaryReader _br, uint _version, bool _bNetworkRead)
	{
		if (_version > 34U)
		{
			for (int i = 0; i < 64; i++)
			{
				int num = i * this.bytesPerVal;
				bool flag = _br.ReadByte() == 1;
				for (int j = 0; j < this.bytesPerVal; j++)
				{
					int num2 = num + j;
					if (!flag)
					{
						if (this.layers[num2] == null)
						{
							this.layers[num2] = this.allocLayer();
						}
						_br.Read(this.layers[num2].data, 0, 1024);
					}
					else
					{
						this.sameValue[num2] = _br.ReadByte();
						this.freeLayer(num2);
					}
				}
				this.onLayerRead(num);
			}
			return;
		}
		for (int k = 0; k < 64; k++)
		{
			int num3 = k * this.bytesPerVal;
			bool flag2 = _br.ReadBoolean();
			for (int l = 0; l < this.bytesPerVal; l++)
			{
				if (!flag2)
				{
					if (this.layers[num3 + l] == null)
					{
						this.layers[num3 + l] = this.allocLayer();
					}
					_br.Read(this.layers[num3 + l].data, 0, 1024);
				}
				else
				{
					this.sameValue[num3 + l] = _br.ReadByte();
					this.freeLayer(num3 + l);
				}
			}
			this.onLayerRead(num3);
		}
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x00205FCC File Offset: 0x002041CC
	public void Write(BinaryWriter _bw, bool _bNetworkWrite, byte[] temp)
	{
		int num = 0;
		for (int i = 0; i < 64; i++)
		{
			int num2 = i * this.bytesPerVal;
			bool flag = this.layers[num2] == null;
			temp[num++] = (flag ? 1 : 0);
			if (num == temp.Length)
			{
				_bw.Write(temp, 0, num);
				num = 0;
			}
			for (int j = 0; j < this.bytesPerVal; j++)
			{
				if (!flag)
				{
					if (num > 0)
					{
						_bw.Write(temp, 0, num);
						num = 0;
					}
					_bw.Write(this.layers[num2 + j].data, 0, 1024);
				}
				else
				{
					temp[num++] = this.sameValue[num2 + j];
					if (num == temp.Length)
					{
						_bw.Write(temp, 0, num);
						num = 0;
					}
				}
			}
		}
		if (num > 0)
		{
			_bw.Write(temp, 0, num);
		}
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x0020609A File Offset: 0x0020429A
	[PublicizedFrom(EAccessModifier.Private)]
	public void onLayerRead(int _idx)
	{
		if (this.layers[_idx] == null)
		{
			return;
		}
		this.checkSameValue(_idx);
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x002060B0 File Offset: 0x002042B0
	public void CheckSameValue()
	{
		for (int i = 63; i >= 0; i--)
		{
			this.checkSameValue(i * this.bytesPerVal);
		}
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x002060D8 File Offset: 0x002042D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkSameValue(int _idx)
	{
		if (this.layers[_idx] == null)
		{
			return;
		}
		long data = this.getData(_idx, 0);
		for (int i = 1; i < 1024; i++)
		{
			if (data != this.getData(_idx, i))
			{
				return;
			}
		}
		this.setSameValue(_idx, data);
		for (int j = 0; j < this.bytesPerVal; j++)
		{
			this.freeLayer(_idx + j);
		}
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x00206138 File Offset: 0x00204338
	public bool HasSameValue(int _y)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		return this.layers[num] == null;
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x0020615C File Offset: 0x0020435C
	public long GetSameValue(int _y)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		return this.getSameValue(num);
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x0020617C File Offset: 0x0020437C
	public bool IsDefault()
	{
		for (int i = 63; i >= 0; i--)
		{
			if (!this.IsDefaultLayer(i))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x002061A4 File Offset: 0x002043A4
	public bool IsDefault(int _y)
	{
		int num = _y >> 2;
		return this.IsDefaultLayer(num);
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x002061BC File Offset: 0x002043BC
	public bool IsDefaultLayer(int _blockLayer)
	{
		return this.isDefault(_blockLayer * this.bytesPerVal);
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x002061CC File Offset: 0x002043CC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDefault(int _idx)
	{
		this.checkSameValue(_idx);
		return this.layers[_idx] == null && this.getSameValue(_idx) == this.defaultValue;
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x002061F0 File Offset: 0x002043F0
	public int GetUsedMem()
	{
		int num = 0;
		for (int i = this.layers.Length - 1; i >= 0; i--)
		{
			if (this.layers[i] != null)
			{
				num += 1024;
			}
		}
		num += this.sameValue.Length;
		return num + this.layers.Length * 4;
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x0020623F File Offset: 0x0020443F
	public void FreeLayers()
	{
		MemoryPools.poolCBC.FreeSync(this.layers);
		this.fillSameValue(-1L);
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x0020625C File Offset: 0x0020445C
	public void Clear(long _defaultValue = -1L)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			this.freeLayer(i);
		}
		this.fillSameValue(_defaultValue);
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x0020628C File Offset: 0x0020448C
	public void ClearHalf(bool _bClearUpperHalf)
	{
		byte b = (_bClearUpperHalf ? 15 : 240);
		for (int i = 0; i < 64; i++)
		{
			CBCLayer cbclayer = this.layers[i];
			if (cbclayer != null)
			{
				for (int j = 0; j < 1024; j++)
				{
					byte[] data = cbclayer.data;
					int num = j;
					data[num] &= b;
				}
			}
			else
			{
				byte[] array = this.sameValue;
				int num2 = i;
				array[num2] &= b;
			}
		}
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x002062F8 File Offset: 0x002044F8
	public void SetHalf(bool _bSetUpperHalf, byte _v)
	{
		byte b = (_bSetUpperHalf ? 15 : 240);
		for (int i = 0; i < 64; i++)
		{
			CBCLayer cbclayer = this.layers[i];
			if (cbclayer != null)
			{
				for (int j = 0; j < 1024; j++)
				{
					byte[] data = cbclayer.data;
					int num = j;
					data[num] &= b;
					byte[] data2 = cbclayer.data;
					int num2 = j;
					data2[num2] |= _v;
				}
			}
			else
			{
				byte[] array = this.sameValue;
				int num3 = i;
				array[num3] &= b;
				byte[] array2 = this.sameValue;
				int num4 = i;
				array2[num4] |= _v;
			}
		}
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x00206388 File Offset: 0x00204588
	public void CopyFrom(ChunkBlockChannel _other)
	{
		for (int i = 0; i < _other.layers.Length; i++)
		{
			if (_other.layers[i] != null)
			{
				if (this.layers[i] == null)
				{
					this.layers[i] = this.allocLayer();
				}
				this.layers[i].CopyFrom(_other.layers[i]);
			}
			else
			{
				this.freeLayer(i);
			}
		}
		for (int j = 0; j < _other.sameValue.Length; j++)
		{
			this.sameValue[j] = _other.sameValue[j];
		}
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x0020640C File Offset: 0x0020460C
	public void Convert(SmartArray _sa, int _shiftBits)
	{
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					byte b = _sa.get(j, i, k);
					byte b2 = (byte)this.Get(j, i, k);
					b2 |= (byte)(b << _shiftBits);
					this.Set(j, i, k, (long)((ulong)b2));
				}
			}
		}
		this.CheckSameValue();
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x00206478 File Offset: 0x00204678
	public void Convert(ChunkBlockLayerLegacy[] m_BlockLayers)
	{
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					byte b;
					if (m_BlockLayers[i] != null)
					{
						b = m_BlockLayers[i].GetStabilityAt(j, k);
					}
					else
					{
						b = 0;
					}
					this.Set(j, i, k, (long)((ulong)b));
				}
			}
		}
		this.CheckSameValue();
	}

	// Token: 0x0400418C RID: 16780
	public const int cElementsPerLayer = 1024;

	// Token: 0x0400418D RID: 16781
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] sameValue;

	// Token: 0x0400418E RID: 16782
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CBCLayer[] layers;

	// Token: 0x0400418F RID: 16783
	[PublicizedFrom(EAccessModifier.Private)]
	public long defaultValue;

	// Token: 0x04004190 RID: 16784
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerVal;
}
