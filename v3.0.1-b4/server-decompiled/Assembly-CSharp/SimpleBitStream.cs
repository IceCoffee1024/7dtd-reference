using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02001486 RID: 5254
[PublicizedFrom(EAccessModifier.Internal)]
public class SimpleBitStream
{
	// Token: 0x0600A4A9 RID: 42153 RVA: 0x003D4BCE File Offset: 0x003D2DCE
	public SimpleBitStream(int _initialCapacity = 1000)
	{
		this.data = new List<byte>(_initialCapacity);
		this.Reset();
	}

	// Token: 0x0600A4AA RID: 42154 RVA: 0x003D4BE8 File Offset: 0x003D2DE8
	public void Reset()
	{
		this.data.Clear();
		this.curBitIdx = 0;
		this.curByteIdx = 0;
		this.curByteData = 0;
	}

	// Token: 0x0600A4AB RID: 42155 RVA: 0x003D4C0C File Offset: 0x003D2E0C
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

	// Token: 0x0600A4AC RID: 42156 RVA: 0x003D4C7C File Offset: 0x003D2E7C
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

	// Token: 0x0600A4AD RID: 42157 RVA: 0x003D4CE4 File Offset: 0x003D2EE4
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

	// Token: 0x0600A4AE RID: 42158 RVA: 0x003D4D90 File Offset: 0x003D2F90
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

	// Token: 0x0600A4AF RID: 42159 RVA: 0x003D4DF0 File Offset: 0x003D2FF0
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

	// Token: 0x04007A99 RID: 31385
	[PublicizedFrom(EAccessModifier.Private)]
	public List<byte> data;

	// Token: 0x04007A9A RID: 31386
	[PublicizedFrom(EAccessModifier.Private)]
	public int curBitIdx;

	// Token: 0x04007A9B RID: 31387
	[PublicizedFrom(EAccessModifier.Private)]
	public int curByteIdx;

	// Token: 0x04007A9C RID: 31388
	[PublicizedFrom(EAccessModifier.Private)]
	public byte curByteData;
}
