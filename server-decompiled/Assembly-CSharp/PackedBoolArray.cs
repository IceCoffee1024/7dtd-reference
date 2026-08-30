using System;
using System.IO;

// Token: 0x02001451 RID: 5201
public class PackedBoolArray
{
	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x0600A30A RID: 41738 RVA: 0x003CD788 File Offset: 0x003CB988
	// (set) Token: 0x0600A30B RID: 41739 RVA: 0x003CD790 File Offset: 0x003CB990
	public int Length
	{
		get
		{
			return this.length;
		}
		set
		{
			if (value == this.length)
			{
				return;
			}
			if (value == 0)
			{
				this.data = null;
				this.length = value;
				return;
			}
			if (this.data == null)
			{
				this.data = new byte[PackedBoolArray.calcArraySize(value)];
				this.length = value;
				return;
			}
			int num = PackedBoolArray.calcArraySize(this.length);
			int num2 = PackedBoolArray.calcArraySize(value);
			if (num2 > num)
			{
				byte[] array = new byte[num2];
				Array.Copy(this.data, array, num);
				this.data = array;
				this.length = value;
				return;
			}
			if (num2 < num)
			{
				byte[] array2 = new byte[num2];
				Array.Copy(this.data, array2, num2);
				this.data = array2;
			}
			this.length = num2 * 8;
			for (int i = value; i < num2 * 8; i++)
			{
				this[i] = false;
			}
			this.length = value;
		}
	}

	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x0600A30C RID: 41740 RVA: 0x003CD85E File Offset: 0x003CBA5E
	public int ByteSize
	{
		get
		{
			return PackedBoolArray.calcArraySize(this.Length);
		}
	}

	// Token: 0x17001318 RID: 4888
	public bool this[int _i]
	{
		get
		{
			this.validateIndex(_i);
			return ((int)this.data[_i / 8] & (1 << _i % 8)) != 0;
		}
		set
		{
			this.validateIndex(_i);
			if (value)
			{
				byte[] array = this.data;
				int num = _i / 8;
				array[num] |= (byte)(1 << _i % 8);
				return;
			}
			byte[] array2 = this.data;
			int num2 = _i / 8;
			array2[num2] &= (byte)(~(byte)(1 << _i % 8));
		}
	}

	// Token: 0x0600A30F RID: 41743 RVA: 0x003CD8DD File Offset: 0x003CBADD
	public PackedBoolArray(int _length = 0)
	{
		this.Length = _length;
	}

	// Token: 0x0600A310 RID: 41744 RVA: 0x003CD8EC File Offset: 0x003CBAEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateIndex(int _i)
	{
		if (_i < 0)
		{
			throw new IndexOutOfRangeException(string.Format("Index ({0}) needs to be non-negative", _i));
		}
		if (_i >= this.length)
		{
			throw new IndexOutOfRangeException(string.Format("Index ({0}) needs to be lower than length ({1})", _i, this.Length));
		}
	}

	// Token: 0x0600A311 RID: 41745 RVA: 0x003CD940 File Offset: 0x003CBB40
	public PackedBoolArray Clone()
	{
		PackedBoolArray packedBoolArray = new PackedBoolArray(this.Length);
		for (int i = 0; i < PackedBoolArray.calcArraySize(this.Length); i++)
		{
			packedBoolArray.data[i] = this.data[i];
		}
		return packedBoolArray;
	}

	// Token: 0x0600A312 RID: 41746 RVA: 0x003CD980 File Offset: 0x003CBB80
	public void Write(Stream _targetStream)
	{
		StreamUtils.Write7BitEncodedInt(_targetStream, this.Length);
		if (this.Length > 0)
		{
			_targetStream.Write(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A313 RID: 41747 RVA: 0x003CD9AA File Offset: 0x003CBBAA
	public void Write(BinaryWriter _targetWriter)
	{
		StreamUtils.Write7BitEncodedInt(_targetWriter.BaseStream, this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A314 RID: 41748 RVA: 0x003CD9D2 File Offset: 0x003CBBD2
	public void Write(PooledBinaryWriter _targetWriter)
	{
		_targetWriter.Write7BitEncodedInt(this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A315 RID: 41749 RVA: 0x003CD9F5 File Offset: 0x003CBBF5
	public void Read(Stream _sourceStream)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceStream);
		if (this.Length > 0)
		{
			_sourceStream.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A316 RID: 41750 RVA: 0x003CDA20 File Offset: 0x003CBC20
	public void Read(BinaryReader _sourceReader)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceReader.BaseStream);
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A317 RID: 41751 RVA: 0x003CDA50 File Offset: 0x003CBC50
	public void Read(PooledBinaryReader _sourceReader)
	{
		this.Length = _sourceReader.Read7BitEncodedInt();
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A318 RID: 41752 RVA: 0x003CDA7B File Offset: 0x003CBC7B
	[PublicizedFrom(EAccessModifier.Private)]
	public static int calcArraySize(int _length)
	{
		return (_length + 7) / 8;
	}

	// Token: 0x0600A319 RID: 41753 RVA: 0x003CDA84 File Offset: 0x003CBC84
	public void Clear()
	{
		for (int i = 0; i < this.ByteSize; i++)
		{
			this.data[i] = 0;
		}
	}

	// Token: 0x040079D9 RID: 31193
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040079DA RID: 31194
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
