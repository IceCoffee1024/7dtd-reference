using System;
using System.IO;

// Token: 0x02000B2C RID: 2860
public class ChunkBlockLayerLegacy : IMemoryPoolableObject
{
	// Token: 0x0600559A RID: 21914 RVA: 0x0020EAB5 File Offset: 0x0020CCB5
	public ChunkBlockLayerLegacy()
	{
		this.wPow = 4;
		this.hPow = 4;
		this.m_Lower16Bits = new ushort[256];
		this.m_Stability = new SmartArray(this.wPow, 0, this.hPow);
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x0020EAF4 File Offset: 0x0020CCF4
	public BlockValue GetAt(int _x, int _y, int _z)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_x + (_z << this.wPow)] << 16) | (int)this.m_Lower16Bits[_x + (_z << this.wPow)]) : ((int)this.m_Lower16Bits[_x + (_z << this.wPow)])));
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x0020EB50 File Offset: 0x0020CD50
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue getAt(int _offs)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_offs] << 16) | (int)this.m_Lower16Bits[_offs]) : ((int)this.m_Lower16Bits[_offs])));
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x0020EB80 File Offset: 0x0020CD80
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

	// Token: 0x0600559E RID: 21918 RVA: 0x0020ECF5 File Offset: 0x0020CEF5
	public byte GetStabilityAt(int _x, int _y)
	{
		return this.m_Stability.get(_x, 0, _y);
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x0020ED05 File Offset: 0x0020CF05
	public void SetStabilityAt(int _x, int _y, byte _v)
	{
		this.m_Stability.set(_x, 0, _y, _v);
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x0020ED16 File Offset: 0x0020CF16
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

	// Token: 0x060055A1 RID: 21921 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x0020ED54 File Offset: 0x0020CF54
	public static int CalcOffset(int _x, int _z)
	{
		return (_x & 15) + ((_z & 15) << 4);
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x0020ED61 File Offset: 0x0020CF61
	public static int OffsetX(int _offset)
	{
		return _offset & 15;
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x0020ED67 File Offset: 0x0020CF67
	public static int OffsetY(int _offset)
	{
		return _offset >> 4;
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x0020ED6C File Offset: 0x0020CF6C
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

	// Token: 0x060055A6 RID: 21926 RVA: 0x0020EDDA File Offset: 0x0020CFDA
	public int GetTickRefCount()
	{
		return this.tickRefCount;
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x0020EDE2 File Offset: 0x0020CFE2
	public bool IsOnlyTerrain()
	{
		return this.bOnlyTerrain;
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x0020EDEC File Offset: 0x0020CFEC
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

	// Token: 0x060055A9 RID: 21929 RVA: 0x0020EFF8 File Offset: 0x0020D1F8
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

	// Token: 0x060055AA RID: 21930 RVA: 0x0020F0B4 File Offset: 0x0020D2B4
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

	// Token: 0x060055AB RID: 21931 RVA: 0x0020F115 File Offset: 0x0020D315
	public int GetUsedMem()
	{
		return this.m_Lower16Bits.Length * 2 + ((this.m_Upper16Bits != null) ? (this.m_Upper16Bits.Length * 2) : 0) + 20 + 2;
	}

	// Token: 0x0400429D RID: 17053
	public ushort[] m_Lower16Bits;

	// Token: 0x0400429E RID: 17054
	public ushort[] m_Upper16Bits;

	// Token: 0x0400429F RID: 17055
	[PublicizedFrom(EAccessModifier.Private)]
	public SmartArray m_Stability;

	// Token: 0x040042A0 RID: 17056
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x040042A1 RID: 17057
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x040042A2 RID: 17058
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyTerrain;

	// Token: 0x040042A3 RID: 17059
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyBlocks;

	// Token: 0x040042A4 RID: 17060
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockRefCount;

	// Token: 0x040042A5 RID: 17061
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickRefCount;

	// Token: 0x040042A6 RID: 17062
	public static int InstanceCount;

	// Token: 0x040042A7 RID: 17063
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLayerHeight = 1;

	// Token: 0x040042A8 RID: 17064
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cArrSize = 256;
}
