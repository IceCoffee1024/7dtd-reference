using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000B28 RID: 2856
public class ChunkBlockChannel
{
	// Token: 0x06005554 RID: 21844 RVA: 0x0020CF98 File Offset: 0x0020B198
	public ChunkBlockChannel(long _defaultValue, int _bytesPerVal = 1)
	{
		this.defaultValue = _defaultValue;
		this.bytesPerVal = _bytesPerVal;
		this.sameValue = new byte[64 * this.bytesPerVal];
		this.fillSameValue(-1L);
		this.layers = new CBCLayer[64 * this.bytesPerVal];
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x0020CFE9 File Offset: 0x0020B1E9
	[PublicizedFrom(EAccessModifier.Private)]
	public CBCLayer allocLayer()
	{
		return MemoryPools.poolCBC.AllocSync(true);
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x0020CFF6 File Offset: 0x0020B1F6
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

	// Token: 0x06005557 RID: 21847 RVA: 0x0020D01E File Offset: 0x0020B21E
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int calcOffset(int _x, int _y, int _z)
	{
		return _x + _z * 16 + (_y & 3) * 256;
	}

	// Token: 0x06005558 RID: 21848 RVA: 0x0020D030 File Offset: 0x0020B230
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

	// Token: 0x06005559 RID: 21849 RVA: 0x0020D088 File Offset: 0x0020B288
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

	// Token: 0x0600555A RID: 21850 RVA: 0x0020D0C0 File Offset: 0x0020B2C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSameValue(int _idx, long _value)
	{
		for (int i = 0; i < this.bytesPerVal; i++)
		{
			this.sameValue[_idx + i] = (byte)_value;
			_value >>= 8;
		}
	}

	// Token: 0x0600555B RID: 21851 RVA: 0x0020D0F0 File Offset: 0x0020B2F0
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

	// Token: 0x0600555C RID: 21852 RVA: 0x0020D134 File Offset: 0x0020B334
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

	// Token: 0x0600555D RID: 21853 RVA: 0x0020D188 File Offset: 0x0020B388
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

	// Token: 0x0600555E RID: 21854 RVA: 0x0020D224 File Offset: 0x0020B424
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

	// Token: 0x0600555F RID: 21855 RVA: 0x0020D2E8 File Offset: 0x0020B4E8
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

	// Token: 0x06005560 RID: 21856 RVA: 0x0020D340 File Offset: 0x0020B540
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

	// Token: 0x06005561 RID: 21857 RVA: 0x0020D380 File Offset: 0x0020B580
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

	// Token: 0x06005562 RID: 21858 RVA: 0x0020D4D4 File Offset: 0x0020B6D4
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

	// Token: 0x06005563 RID: 21859 RVA: 0x0020D5A2 File Offset: 0x0020B7A2
	[PublicizedFrom(EAccessModifier.Private)]
	public void onLayerRead(int _idx)
	{
		if (this.layers[_idx] == null)
		{
			return;
		}
		this.checkSameValue(_idx);
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x0020D5B8 File Offset: 0x0020B7B8
	public void CheckSameValue()
	{
		for (int i = 63; i >= 0; i--)
		{
			this.checkSameValue(i * this.bytesPerVal);
		}
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x0020D5E0 File Offset: 0x0020B7E0
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

	// Token: 0x06005566 RID: 21862 RVA: 0x0020D640 File Offset: 0x0020B840
	public bool HasSameValue(int _y)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		return this.layers[num] == null;
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x0020D664 File Offset: 0x0020B864
	public long GetSameValue(int _y)
	{
		int num = (_y >> 2) * this.bytesPerVal;
		return this.getSameValue(num);
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x0020D684 File Offset: 0x0020B884
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

	// Token: 0x06005569 RID: 21865 RVA: 0x0020D6AC File Offset: 0x0020B8AC
	public bool IsDefault(int _y)
	{
		int num = _y >> 2;
		return this.IsDefaultLayer(num);
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x0020D6C4 File Offset: 0x0020B8C4
	public bool IsDefaultLayer(int _blockLayer)
	{
		return this.isDefault(_blockLayer * this.bytesPerVal);
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x0020D6D4 File Offset: 0x0020B8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDefault(int _idx)
	{
		this.checkSameValue(_idx);
		return this.layers[_idx] == null && this.getSameValue(_idx) == this.defaultValue;
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x0020D6F8 File Offset: 0x0020B8F8
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

	// Token: 0x0600556D RID: 21869 RVA: 0x0020D747 File Offset: 0x0020B947
	public void FreeLayers()
	{
		MemoryPools.poolCBC.FreeSync(this.layers);
		this.fillSameValue(-1L);
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x0020D764 File Offset: 0x0020B964
	public void Clear(long _defaultValue = -1L)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			this.freeLayer(i);
		}
		this.fillSameValue(_defaultValue);
	}

	// Token: 0x0600556F RID: 21871 RVA: 0x0020D794 File Offset: 0x0020B994
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

	// Token: 0x06005570 RID: 21872 RVA: 0x0020D800 File Offset: 0x0020BA00
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

	// Token: 0x06005571 RID: 21873 RVA: 0x0020D890 File Offset: 0x0020BA90
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

	// Token: 0x06005572 RID: 21874 RVA: 0x0020D914 File Offset: 0x0020BB14
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

	// Token: 0x06005573 RID: 21875 RVA: 0x0020D980 File Offset: 0x0020BB80
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

	// Token: 0x04004289 RID: 17033
	public const int cElementsPerLayer = 1024;

	// Token: 0x0400428A RID: 17034
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] sameValue;

	// Token: 0x0400428B RID: 17035
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CBCLayer[] layers;

	// Token: 0x0400428C RID: 17036
	[PublicizedFrom(EAccessModifier.Private)]
	public long defaultValue;

	// Token: 0x0400428D RID: 17037
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerVal;
}
