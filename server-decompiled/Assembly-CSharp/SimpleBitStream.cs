using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020014D3 RID: 5331
[PublicizedFrom(EAccessModifier.Internal)]
public class SimpleBitStream
{
	// Token: 0x0600A6AC RID: 42668 RVA: 0x003E0FA2 File Offset: 0x003DF1A2
	public SimpleBitStream(int _initialCapacity = 1000)
	{
		this.data = new List<byte>(_initialCapacity);
		this.Reset();
	}

	// Token: 0x0600A6AD RID: 42669 RVA: 0x003E0FBC File Offset: 0x003DF1BC
	public void Reset()
	{
		this.data.Clear();
		this.curBitIdx = 0;
		this.curByteIdx = 0;
		this.curByteData = 0;
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003E0FE0 File Offset: 0x003DF1E0
	public void Add(bool _b)
	{
		if (_b)
		{
			this.curByteData = (byte)((int)this.curByteData | (1 << this.curBitIdx));
		}
		this.curBitIdx++;
		if (this.curBitIdx > 7)
		{
			this.data.Add(this.curByteData);
			this.curBitIdx = 0;
			this.curByteIdx++;
			this.curByteData = 0;
		}
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003E1050 File Offset: 0x003DF250
	public bool GetNext()
	{
		if (this.curBitIdx > 7)
		{
			List<byte> list = this.data;
			int num = this.curByteIdx + 1;
			this.curByteIdx = num;
			this.curByteData = list[num];
			this.curBitIdx = 0;
		}
		bool flag = (this.curByteData & 1) > 0;
		this.curByteData = (byte)(this.curByteData >> 1);
		this.curBitIdx++;
		return flag;
	}

	// Token: 0x0600A6B0 RID: 42672 RVA: 0x003E10B8 File Offset: 0x003DF2B8
	public int GetNextOffset()
	{
		bool flag = false;
		for (;;)
		{
			if (this.curBitIdx > 7)
			{
				this.curByteIdx++;
				if (this.curByteIdx >= this.data.Count)
				{
					break;
				}
				this.curByteData = this.data[this.curByteIdx];
				this.curBitIdx = 0;
			}
			if (this.curByteData == 0)
			{
				this.curBitIdx = 8;
			}
			else
			{
				flag = (this.curByteData & 1) == 1;
				this.curByteData = (byte)(this.curByteData >> 1);
				this.curBitIdx++;
			}
			if (flag)
			{
				goto Block_4;
			}
		}
		return -1;
		Block_4:
		return this.curByteIdx * 8 + this.curBitIdx - 1;
	}

	// Token: 0x0600A6B1 RID: 42673 RVA: 0x003E1164 File Offset: 0x003DF364
	public void Write(BinaryWriter _bw)
	{
		if (this.curBitIdx > 0)
		{
			this.data.Add(this.curByteData);
		}
		_bw.Write(this.data.Count);
		for (int i = 0; i < this.data.Count; i++)
		{
			_bw.Write(this.data[i]);
		}
	}

	// Token: 0x0600A6B2 RID: 42674 RVA: 0x003E11C4 File Offset: 0x003DF3C4
	public void Read(BinaryReader _br)
	{
		int num = _br.ReadInt32();
		byte[] array = _br.ReadBytes(num);
		this.data.Clear();
		this.data.AddRange(array);
		if (num > 0)
		{
			this.curByteData = this.data[0];
		}
	}

	// Token: 0x04007C2E RID: 31790
	[PublicizedFrom(EAccessModifier.Private)]
	public List<byte> data;

	// Token: 0x04007C2F RID: 31791
	[PublicizedFrom(EAccessModifier.Private)]
	public int curBitIdx;

	// Token: 0x04007C30 RID: 31792
	[PublicizedFrom(EAccessModifier.Private)]
	public int curByteIdx;

	// Token: 0x04007C31 RID: 31793
	[PublicizedFrom(EAccessModifier.Private)]
	public byte curByteData;
}
