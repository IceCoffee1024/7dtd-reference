using System;
using System.IO;

// Token: 0x02000951 RID: 2385
public class PrefabInsideDataFile
{
	// Token: 0x0600455E RID: 17758 RVA: 0x0000640C File Offset: 0x0000460C
	public PrefabInsideDataFile()
	{
	}

	// Token: 0x0600455F RID: 17759 RVA: 0x001B8FB6 File Offset: 0x001B71B6
	public PrefabInsideDataFile(PrefabInsideDataFile _other)
	{
		this.size = _other.size;
		this.data = _other.data;
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x001B8FD6 File Offset: 0x001B71D6
	public void Init(Vector3i _size)
	{
		this.size = _size;
		this.data = null;
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x001B8FE6 File Offset: 0x001B71E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Alloc()
	{
		this.data = new byte[this.size.x * this.size.y * this.size.z + 7 >> 3];
	}

	// Token: 0x06004562 RID: 17762 RVA: 0x001B901A File Offset: 0x001B721A
	public void Add(int _offset)
	{
		if (this.data == null)
		{
			this.Alloc();
		}
		byte[] array = this.data;
		int num = _offset >> 3;
		array[num] |= (byte)(1 << (_offset & 7));
	}

	// Token: 0x06004563 RID: 17763 RVA: 0x001B9048 File Offset: 0x001B7248
	public void Add(int x, int y, int z)
	{
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		this.Add(num);
	}

	// Token: 0x06004564 RID: 17764 RVA: 0x001B9088 File Offset: 0x001B7288
	public bool Contains(int x, int y, int z)
	{
		if (this.data == null)
		{
			return false;
		}
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		return ((int)this.data[num >> 3] & (1 << (num & 7))) > 0;
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x001B90DF File Offset: 0x001B72DF
	public PrefabInsideDataFile Clone()
	{
		return new PrefabInsideDataFile(this);
	}

	// Token: 0x06004566 RID: 17766 RVA: 0x001B90E8 File Offset: 0x001B72E8
	public void Load(string _filename, Vector3i _size)
	{
		this.Init(_size);
		if (!SdFile.Exists(_filename))
		{
			return;
		}
		try
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(stream);
					try
					{
						this.Read(pooledBinaryReader);
					}
					catch (Exception ex)
					{
						Log.Error("PrefabInsideDataFile Load {0}, expected data len {1}. Probably outdated ins file, please re-save to fix. Read error:", new object[]
						{
							_filename,
							this.data.Length
						});
						Log.Exception(ex);
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Exception(ex2);
		}
	}

	// Token: 0x06004567 RID: 17767 RVA: 0x001B91AC File Offset: 0x001B73AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Read(BinaryReader _br)
	{
		int num = (int)_br.ReadByte();
		int num2 = _br.ReadInt32();
		if (num <= 1)
		{
			for (int i = 0; i < num2; i++)
			{
				int num3 = (int)_br.ReadByte();
				int num4 = (int)_br.ReadByte();
				int num5 = (int)_br.ReadByte();
				this.Add(num3, num4, num5);
			}
			return;
		}
		if (num2 > 0)
		{
			this.Alloc();
			_br.Read(this.data, 0, num2);
		}
	}

	// Token: 0x06004568 RID: 17768 RVA: 0x001B9210 File Offset: 0x001B7410
	public void Save(string _filename)
	{
		try
		{
			using (Stream stream = SdFile.Open(_filename, FileMode.Create))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					this.Write(pooledBinaryWriter);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x001B9288 File Offset: 0x001B7488
	[PublicizedFrom(EAccessModifier.Private)]
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(2);
		int num = ((this.data != null) ? this.data.Length : 0);
		_bw.Write(num);
		if (num > 0)
		{
			_bw.Write(this.data, 0, num);
		}
	}

	// Token: 0x04003809 RID: 14345
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 2;

	// Token: 0x0400380A RID: 14346
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x0400380B RID: 14347
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
