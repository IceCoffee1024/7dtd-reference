using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020014C1 RID: 5313
[PublicizedFrom(EAccessModifier.Internal)]
public class SimpleBitStream
{
	// Token: 0x0600A654 RID: 42580 RVA: 0x003DEB9E File Offset: 0x003DCD9E
	public SimpleBitStream(int _initialCapacity = 1000)
	{
		this.data = new List<byte>(_initialCapacity);
		this.Reset();
	}

	// Token: 0x0600A655 RID: 42581 RVA: 0x003DEBB8 File Offset: 0x003DCDB8
	public void Reset()
	{
		this.data.Clear();
		this.curBitIdx = 0;
		this.curByteIdx = 0;
		this.curByteData = 0;
	}

	// Token: 0x0600A656 RID: 42582 RVA: 0x003DEBDC File Offset: 0x003DCDDC
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

	// Token: 0x0600A657 RID: 42583 RVA: 0x003DEC4C File Offset: 0x003DCE4C
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

	// Token: 0x0600A658 RID: 42584 RVA: 0x003DECB4 File Offset: 0x003DCEB4
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

	// Token: 0x0600A659 RID: 42585 RVA: 0x003DED60 File Offset: 0x003DCF60
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

	// Token: 0x0600A65A RID: 42586 RVA: 0x003DEDC0 File Offset: 0x003DCFC0
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

	// Token: 0x04007BED RID: 31725
	[PublicizedFrom(EAccessModifier.Private)]
	public List<byte> data;

	// Token: 0x04007BEE RID: 31726
	[PublicizedFrom(EAccessModifier.Private)]
	public int curBitIdx;

	// Token: 0x04007BEF RID: 31727
	[PublicizedFrom(EAccessModifier.Private)]
	public int curByteIdx;

	// Token: 0x04007BF0 RID: 31728
	[PublicizedFrom(EAccessModifier.Private)]
	public byte curByteData;
}
