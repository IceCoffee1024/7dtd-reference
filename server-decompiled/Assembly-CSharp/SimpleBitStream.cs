using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020014A1 RID: 5281
[PublicizedFrom(EAccessModifier.Internal)]
public class SimpleBitStream
{
	// Token: 0x0600A5A6 RID: 42406 RVA: 0x003D9F12 File Offset: 0x003D8112
	public SimpleBitStream(int _initialCapacity = 1000)
	{
		this.data = new List<byte>(_initialCapacity);
		this.Reset();
	}

	// Token: 0x0600A5A7 RID: 42407 RVA: 0x003D9F2C File Offset: 0x003D812C
	public void Reset()
	{
		this.data.Clear();
		this.curBitIdx = 0;
		this.curByteIdx = 0;
		this.curByteData = 0;
	}

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x003D9F50 File Offset: 0x003D8150
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

	// Token: 0x0600A5A9 RID: 42409 RVA: 0x003D9FC0 File Offset: 0x003D81C0
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

	// Token: 0x0600A5AA RID: 42410 RVA: 0x003DA028 File Offset: 0x003D8228
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

	// Token: 0x0600A5AB RID: 42411 RVA: 0x003DA0D4 File Offset: 0x003D82D4
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

	// Token: 0x0600A5AC RID: 42412 RVA: 0x003DA134 File Offset: 0x003D8334
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

	// Token: 0x04007B72 RID: 31602
	[PublicizedFrom(EAccessModifier.Private)]
	public List<byte> data;

	// Token: 0x04007B73 RID: 31603
	[PublicizedFrom(EAccessModifier.Private)]
	public int curBitIdx;

	// Token: 0x04007B74 RID: 31604
	[PublicizedFrom(EAccessModifier.Private)]
	public int curByteIdx;

	// Token: 0x04007B75 RID: 31605
	[PublicizedFrom(EAccessModifier.Private)]
	public byte curByteData;
}
