using System;
using System.IO;

// Token: 0x02000AFD RID: 2813
public class ChunkBlockLayerLegacy : IMemoryPoolableObject
{
	// Token: 0x060054D4 RID: 21716 RVA: 0x002075AD File Offset: 0x002057AD
	public ChunkBlockLayerLegacy()
	{
		this.wPow = 4;
		this.hPow = 4;
		this.m_Lower16Bits = new ushort[256];
		this.m_Stability = new SmartArray(this.wPow, 0, this.hPow);
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x002075EC File Offset: 0x002057EC
	public BlockValue GetAt(int _x, int _y, int _z)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_x + (_z << this.wPow)] << 16) | (int)this.m_Lower16Bits[_x + (_z << this.wPow)]) : ((int)this.m_Lower16Bits[_x + (_z << this.wPow)])));
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x00207648 File Offset: 0x00205848
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue getAt(int _offs)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_offs] << 16) | (int)this.m_Lower16Bits[_offs]) : ((int)this.m_Lower16Bits[_offs])));
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x00207678 File Offset: 0x00205878
	public void SetAt(int _x, int _y, int _z, uint _fullBlock)
	{
		int num = _x + (_z << this.wPow);
		uint typeMasked = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[num]);
		this.m_Lower16Bits[num] = (ushort)(_fullBlock & 65535U);
		if ((_fullBlock & 4294901760U) != 0U)
		{
			if (this.m_Upper16Bits == null)
			{
				this.m_Upper16Bits = new ushort[256];
			}
			this.m_Upper16Bits[num] = (ushort)((_fullBlock >> 16) & 65535U);
		}
		else if (this.m_Upper16Bits != null)
		{
			this.m_Upper16Bits[num] = 0;
		}
		if (!Block.BlocksLoaded)
		{
			return;
		}
		uint typeMasked2 = BlockValue.GetTypeMasked(_fullBlock);
		Block block = Block.list[(int)typeMasked];
		Block block2 = Block.list[(int)typeMasked2];
		if (typeMasked == 0U && typeMasked2 != 0U)
		{
			this.blockRefCount++;
			if (block2 != null && block2.IsRandomlyTick)
			{
				this.tickRefCount++;
			}
		}
		else if (typeMasked != 0U && typeMasked2 == 0U)
		{
			this.blockRefCount--;
			if (block != null && block.IsRandomlyTick)
			{
				this.tickRefCount--;
			}
		}
		else if (block != null && block.IsRandomlyTick && block2 != null && !block2.IsRandomlyTick)
		{
			this.tickRefCount--;
		}
		else if (block != null && !block.IsRandomlyTick && block2 != null && block2.IsRandomlyTick)
		{
			this.tickRefCount++;
		}
		if (this.bOnlyTerrain && !block2.shape.IsTerrain())
		{
			this.bOnlyTerrain = false;
		}
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x002077ED File Offset: 0x002059ED
	public byte GetStabilityAt(int _x, int _y)
	{
		return this.m_Stability.get(_x, 0, _y);
	}

	// Token: 0x060054D9 RID: 21721 RVA: 0x002077FD File Offset: 0x002059FD
	public void SetStabilityAt(int _x, int _y, byte _v)
	{
		this.m_Stability.set(_x, 0, _y, _v);
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x0020780E File Offset: 0x00205A0E
	public void Reset()
	{
		Array.Clear(this.m_Lower16Bits, 0, this.m_Lower16Bits.Length);
		this.m_Upper16Bits = null;
		if (this.m_Stability != null)
		{
			this.m_Stability.clear();
		}
		this.blockRefCount = 0;
		this.tickRefCount = 0;
	}

	// Token: 0x060054DB RID: 21723 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x0020784C File Offset: 0x00205A4C
	public static int CalcOffset(int _x, int _z)
	{
		return (_x & 15) + ((_z & 15) << 4);
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x00207859 File Offset: 0x00205A59
	public static int OffsetX(int _offset)
	{
		return _offset & 15;
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x0020785F File Offset: 0x00205A5F
	public static int OffsetY(int _offset)
	{
		return _offset >> 4;
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x00207864 File Offset: 0x00205A64
	public void UpdateRefCounts()
	{
		this.blockRefCount = 0;
		this.tickRefCount = 0;
		for (int i = this.m_Lower16Bits.Length - 1; i >= 0; i--)
		{
			int type = this.getAt(i).type;
			if (type > 0)
			{
				this.blockRefCount++;
				if (Block.list[type].IsRandomlyTick)
				{
					this.tickRefCount++;
				}
			}
		}
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x002078D2 File Offset: 0x00205AD2
	public int GetTickRefCount()
	{
		return this.tickRefCount;
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x002078DA File Offset: 0x00205ADA
	public bool IsOnlyTerrain()
	{
		return this.bOnlyTerrain;
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x002078E4 File Offset: 0x00205AE4
	public void Read(BinaryReader stream, uint _version, bool _bNetworkRead, byte[] _tempReadBuf)
	{
		if (_version >= 19U)
		{
			stream.Read(_tempReadBuf, 0, 512);
			for (int i = 0; i < this.m_Lower16Bits.Length; i++)
			{
				ushort num = (ushort)((int)_tempReadBuf[i * 2] | ((int)_tempReadBuf[i * 2 + 1] << 8));
				this.m_Lower16Bits[i] = num;
			}
			if (stream.ReadBoolean())
			{
				if (this.m_Upper16Bits == null)
				{
					this.m_Upper16Bits = new ushort[256];
				}
				stream.Read(_tempReadBuf, 0, 512);
				for (int j = 0; j < this.m_Upper16Bits.Length; j++)
				{
					ushort num2 = (ushort)((int)_tempReadBuf[j * 2] | ((int)_tempReadBuf[j * 2 + 1] << 8));
					this.m_Upper16Bits[j] = num2;
				}
			}
			else
			{
				this.m_Upper16Bits = null;
			}
		}
		else if (_version >= 5U && _version < 19U)
		{
			stream.Read(_tempReadBuf, 0, 1024);
			for (int k = 0; k < this.m_Lower16Bits.Length; k++)
			{
				uint num3 = (uint)((int)_tempReadBuf[k * 4] | ((int)_tempReadBuf[k * 4 + 1] << 8) | ((int)_tempReadBuf[k * 4 + 2] << 16) | ((int)_tempReadBuf[k * 4 + 3] << 24));
				this.m_Lower16Bits[k] = (ushort)(num3 & 65535U);
				if ((num3 & 4294901760U) != 0U)
				{
					if (this.m_Upper16Bits == null)
					{
						this.m_Upper16Bits = new ushort[256];
					}
					this.m_Upper16Bits[k] = (ushort)((num3 >> 16) & 65535U);
				}
				else if (this.m_Upper16Bits != null)
				{
					this.m_Upper16Bits[k] = 0;
				}
			}
		}
		if (_version > 8U && _version < 18U && !_bNetworkRead)
		{
			byte[] array = new byte[256];
			stream.Read(array, 0, array.Length);
			for (int l = 0; l < 16; l++)
			{
				for (int m = 0; m < 16; m++)
				{
					this.m_Stability.set(l, 0, m, array[l + m * 16]);
				}
			}
		}
		if (_version >= 18U && _version < 28U && !_bNetworkRead)
		{
			this.m_Stability.read(stream);
		}
		this.CheckOnlyTerrain();
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x00207AF0 File Offset: 0x00205CF0
	public void Write(BinaryWriter _bw, bool _bNetworkWrite, byte[] _tempSaveBuf)
	{
		for (int i = 0; i < this.m_Lower16Bits.Length; i++)
		{
			uint num = (uint)this.m_Lower16Bits[i];
			_tempSaveBuf[i * 2] = (byte)(num & 255U);
			_tempSaveBuf[i * 2 + 1] = (byte)((num >> 8) & 255U);
		}
		_bw.Write(_tempSaveBuf, 0, this.m_Lower16Bits.Length * 2);
		_bw.Write(this.m_Upper16Bits != null);
		if (this.m_Upper16Bits != null)
		{
			for (int j = 0; j < this.m_Upper16Bits.Length; j++)
			{
				uint num2 = (uint)this.m_Upper16Bits[j];
				_tempSaveBuf[j * 2] = (byte)(num2 & 255U);
				_tempSaveBuf[j * 2 + 1] = (byte)((num2 >> 8) & 255U);
			}
			_bw.Write(_tempSaveBuf, 0, this.m_Upper16Bits.Length * 2);
		}
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x00207BAC File Offset: 0x00205DAC
	public void CheckOnlyTerrain()
	{
		this.bOnlyTerrain = true;
		uint num = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[0]);
		for (int i = 0; i < this.m_Lower16Bits.Length; i++)
		{
			num = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[i]);
			if (num != 0U && !Block.list[(int)num].shape.IsTerrain())
			{
				this.bOnlyTerrain = false;
				return;
			}
		}
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x00207C0D File Offset: 0x00205E0D
	public int GetUsedMem()
	{
		return this.m_Lower16Bits.Length * 2 + ((this.m_Upper16Bits != null) ? (this.m_Upper16Bits.Length * 2) : 0) + 20 + 2;
	}

	// Token: 0x040041A0 RID: 16800
	public ushort[] m_Lower16Bits;

	// Token: 0x040041A1 RID: 16801
	public ushort[] m_Upper16Bits;

	// Token: 0x040041A2 RID: 16802
	[PublicizedFrom(EAccessModifier.Private)]
	public SmartArray m_Stability;

	// Token: 0x040041A3 RID: 16803
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x040041A4 RID: 16804
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x040041A5 RID: 16805
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyTerrain;

	// Token: 0x040041A6 RID: 16806
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyBlocks;

	// Token: 0x040041A7 RID: 16807
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockRefCount;

	// Token: 0x040041A8 RID: 16808
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickRefCount;

	// Token: 0x040041A9 RID: 16809
	public static int InstanceCount;

	// Token: 0x040041AA RID: 16810
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLayerHeight = 1;

	// Token: 0x040041AB RID: 16811
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cArrSize = 256;
}
