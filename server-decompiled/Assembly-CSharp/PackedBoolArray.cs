using System;
using System.IO;

// Token: 0x0200148C RID: 5260
public class PackedBoolArray
{
	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x0600A4B3 RID: 42163 RVA: 0x003D76DC File Offset: 0x003D58DC
	// (set) Token: 0x0600A4B4 RID: 42164 RVA: 0x003D76E4 File Offset: 0x003D58E4
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

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x0600A4B5 RID: 42165 RVA: 0x003D77B2 File Offset: 0x003D59B2
	public int ByteSize
	{
		get
		{
			return PackedBoolArray.calcArraySize(this.Length);
		}
	}

	// Token: 0x17001396 RID: 5014
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

	// Token: 0x0600A4B8 RID: 42168 RVA: 0x003D7831 File Offset: 0x003D5A31
	public PackedBoolArray(int _length = 0)
	{
		this.Length = _length;
	}

	// Token: 0x0600A4B9 RID: 42169 RVA: 0x003D7840 File Offset: 0x003D5A40
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

	// Token: 0x0600A4BA RID: 42170 RVA: 0x003D7894 File Offset: 0x003D5A94
	public PackedBoolArray Clone()
	{
		PackedBoolArray packedBoolArray = new PackedBoolArray(this.Length);
		for (int i = 0; i < PackedBoolArray.calcArraySize(this.Length); i++)
		{
			packedBoolArray.data[i] = this.data[i];
		}
		return packedBoolArray;
	}

	// Token: 0x0600A4BB RID: 42171 RVA: 0x003D78D4 File Offset: 0x003D5AD4
	public void CopyFrom(PackedBoolArray _other)
	{
		this.Length = _other.Length;
		if (this.Length > 0)
		{
			Array.Copy(_other.data, this.data, this.ByteSize);
		}
	}

	// Token: 0x0600A4BC RID: 42172 RVA: 0x003D7902 File Offset: 0x003D5B02
	public void Write(Stream _targetStream)
	{
		StreamUtils.Write7BitEncodedInt(_targetStream, this.Length);
		if (this.Length > 0)
		{
			_targetStream.Write(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A4BD RID: 42173 RVA: 0x003D792C File Offset: 0x003D5B2C
	public void Write(BinaryWriter _targetWriter)
	{
		StreamUtils.Write7BitEncodedInt(_targetWriter.BaseStream, this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A4BE RID: 42174 RVA: 0x003D7954 File Offset: 0x003D5B54
	public void Write(PooledBinaryWriter _targetWriter)
	{
		_targetWriter.Write7BitEncodedInt(this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A4BF RID: 42175 RVA: 0x003D7977 File Offset: 0x003D5B77
	public void Read(Stream _sourceStream)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceStream);
		if (this.Length > 0)
		{
			_sourceStream.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A4C0 RID: 42176 RVA: 0x003D79A2 File Offset: 0x003D5BA2
	public void Read(BinaryReader _sourceReader)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceReader.BaseStream);
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A4C1 RID: 42177 RVA: 0x003D79D2 File Offset: 0x003D5BD2
	public void Read(PooledBinaryReader _sourceReader)
	{
		this.Length = _sourceReader.Read7BitEncodedInt();
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A4C2 RID: 42178 RVA: 0x003D79FD File Offset: 0x003D5BFD
	[PublicizedFrom(EAccessModifier.Private)]
	public static int calcArraySize(int _length)
	{
		return (_length + 7) / 8;
	}

	// Token: 0x0600A4C3 RID: 42179 RVA: 0x003D7A04 File Offset: 0x003D5C04
	public void Clear()
	{
		for (int i = 0; i < this.ByteSize; i++)
		{
			this.data[i] = 0;
		}
	}

	// Token: 0x04007B2D RID: 31533
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04007B2E RID: 31534
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
