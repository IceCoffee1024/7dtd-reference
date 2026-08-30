using System;
using System.IO;

// Token: 0x0200146C RID: 5228
public class PackedBoolArray
{
	// Token: 0x1700134F RID: 4943
	// (get) Token: 0x0600A407 RID: 41991 RVA: 0x003D2ACC File Offset: 0x003D0CCC
	// (set) Token: 0x0600A408 RID: 41992 RVA: 0x003D2AD4 File Offset: 0x003D0CD4
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

	// Token: 0x17001350 RID: 4944
	// (get) Token: 0x0600A409 RID: 41993 RVA: 0x003D2BA2 File Offset: 0x003D0DA2
	public int ByteSize
	{
		get
		{
			return PackedBoolArray.calcArraySize(this.Length);
		}
	}

	// Token: 0x17001351 RID: 4945
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

	// Token: 0x0600A40C RID: 41996 RVA: 0x003D2C21 File Offset: 0x003D0E21
	public PackedBoolArray(int _length = 0)
	{
		this.Length = _length;
	}

	// Token: 0x0600A40D RID: 41997 RVA: 0x003D2C30 File Offset: 0x003D0E30
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

	// Token: 0x0600A40E RID: 41998 RVA: 0x003D2C84 File Offset: 0x003D0E84
	public PackedBoolArray Clone()
	{
		PackedBoolArray packedBoolArray = new PackedBoolArray(this.Length);
		for (int i = 0; i < PackedBoolArray.calcArraySize(this.Length); i++)
		{
			packedBoolArray.data[i] = this.data[i];
		}
		return packedBoolArray;
	}

	// Token: 0x0600A40F RID: 41999 RVA: 0x003D2CC4 File Offset: 0x003D0EC4
	public void Write(Stream _targetStream)
	{
		StreamUtils.Write7BitEncodedInt(_targetStream, this.Length);
		if (this.Length > 0)
		{
			_targetStream.Write(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A410 RID: 42000 RVA: 0x003D2CEE File Offset: 0x003D0EEE
	public void Write(BinaryWriter _targetWriter)
	{
		StreamUtils.Write7BitEncodedInt(_targetWriter.BaseStream, this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A411 RID: 42001 RVA: 0x003D2D16 File Offset: 0x003D0F16
	public void Write(PooledBinaryWriter _targetWriter)
	{
		_targetWriter.Write7BitEncodedInt(this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A412 RID: 42002 RVA: 0x003D2D39 File Offset: 0x003D0F39
	public void Read(Stream _sourceStream)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceStream);
		if (this.Length > 0)
		{
			_sourceStream.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A413 RID: 42003 RVA: 0x003D2D64 File Offset: 0x003D0F64
	public void Read(BinaryReader _sourceReader)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceReader.BaseStream);
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A414 RID: 42004 RVA: 0x003D2D94 File Offset: 0x003D0F94
	public void Read(PooledBinaryReader _sourceReader)
	{
		this.Length = _sourceReader.Read7BitEncodedInt();
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A415 RID: 42005 RVA: 0x003D2DBF File Offset: 0x003D0FBF
	[PublicizedFrom(EAccessModifier.Private)]
	public static int calcArraySize(int _length)
	{
		return (_length + 7) / 8;
	}

	// Token: 0x0600A416 RID: 42006 RVA: 0x003D2DC8 File Offset: 0x003D0FC8
	public void Clear()
	{
		for (int i = 0; i < this.ByteSize; i++)
		{
			this.data[i] = 0;
		}
	}

	// Token: 0x04007AB2 RID: 31410
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04007AB3 RID: 31411
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
