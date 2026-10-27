using System;
using System.IO;

// Token: 0x0200149E RID: 5278
public class PackedBoolArray
{
	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x0600A50B RID: 42251 RVA: 0x003D9AE0 File Offset: 0x003D7CE0
	// (set) Token: 0x0600A50C RID: 42252 RVA: 0x003D9AE8 File Offset: 0x003D7CE8
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

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x0600A50D RID: 42253 RVA: 0x003D9BB6 File Offset: 0x003D7DB6
	public int ByteSize
	{
		get
		{
			return PackedBoolArray.calcArraySize(this.Length);
		}
	}

	// Token: 0x170013A2 RID: 5026
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

	// Token: 0x0600A510 RID: 42256 RVA: 0x003D9C35 File Offset: 0x003D7E35
	public PackedBoolArray(int _length = 0)
	{
		this.Length = _length;
	}

	// Token: 0x0600A511 RID: 42257 RVA: 0x003D9C44 File Offset: 0x003D7E44
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

	// Token: 0x0600A512 RID: 42258 RVA: 0x003D9C98 File Offset: 0x003D7E98
	public PackedBoolArray Clone()
	{
		PackedBoolArray packedBoolArray = new PackedBoolArray(this.Length);
		for (int i = 0; i < PackedBoolArray.calcArraySize(this.Length); i++)
		{
			packedBoolArray.data[i] = this.data[i];
		}
		return packedBoolArray;
	}

	// Token: 0x0600A513 RID: 42259 RVA: 0x003D9CD8 File Offset: 0x003D7ED8
	public void CopyFrom(PackedBoolArray _other)
	{
		this.Length = _other.Length;
		if (this.Length > 0)
		{
			Array.Copy(_other.data, this.data, this.ByteSize);
		}
	}

	// Token: 0x0600A514 RID: 42260 RVA: 0x003D9D06 File Offset: 0x003D7F06
	public void Write(Stream _targetStream)
	{
		StreamUtils.Write7BitEncodedInt(_targetStream, this.Length);
		if (this.Length > 0)
		{
			_targetStream.Write(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A515 RID: 42261 RVA: 0x003D9D30 File Offset: 0x003D7F30
	public void Write(BinaryWriter _targetWriter)
	{
		StreamUtils.Write7BitEncodedInt(_targetWriter.BaseStream, this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A516 RID: 42262 RVA: 0x003D9D58 File Offset: 0x003D7F58
	public void Write(PooledBinaryWriter _targetWriter)
	{
		_targetWriter.Write7BitEncodedInt(this.Length);
		if (this.Length > 0)
		{
			_targetWriter.Write(this.data);
		}
	}

	// Token: 0x0600A517 RID: 42263 RVA: 0x003D9D7B File Offset: 0x003D7F7B
	public void Read(Stream _sourceStream)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceStream);
		if (this.Length > 0)
		{
			_sourceStream.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A518 RID: 42264 RVA: 0x003D9DA6 File Offset: 0x003D7FA6
	public void Read(BinaryReader _sourceReader)
	{
		this.Length = StreamUtils.Read7BitEncodedInt(_sourceReader.BaseStream);
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A519 RID: 42265 RVA: 0x003D9DD6 File Offset: 0x003D7FD6
	public void Read(PooledBinaryReader _sourceReader)
	{
		this.Length = _sourceReader.Read7BitEncodedInt();
		if (this.Length > 0)
		{
			_sourceReader.Read(this.data, 0, this.ByteSize);
		}
	}

	// Token: 0x0600A51A RID: 42266 RVA: 0x003D9E01 File Offset: 0x003D8001
	[PublicizedFrom(EAccessModifier.Private)]
	public static int calcArraySize(int _length)
	{
		return (_length + 7) / 8;
	}

	// Token: 0x0600A51B RID: 42267 RVA: 0x003D9E08 File Offset: 0x003D8008
	public void Clear()
	{
		for (int i = 0; i < this.ByteSize; i++)
		{
			this.data[i] = 0;
		}
	}

	// Token: 0x04007B6E RID: 31598
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04007B6F RID: 31599
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
