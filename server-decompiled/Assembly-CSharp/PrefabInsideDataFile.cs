using System;
using System.IO;

// Token: 0x0200093B RID: 2363
public class PrefabInsideDataFile
{
	// Token: 0x0600453C RID: 17724 RVA: 0x0000640C File Offset: 0x0000460C
	public PrefabInsideDataFile()
	{
	}

	// Token: 0x0600453D RID: 17725 RVA: 0x001B64D6 File Offset: 0x001B46D6
	public PrefabInsideDataFile(PrefabInsideDataFile _other)
	{
		this.size = _other.size;
		this.data = _other.data;
	}

	// Token: 0x0600453E RID: 17726 RVA: 0x001B64F6 File Offset: 0x001B46F6
	public void Init(Vector3i _size)
	{
		this.size = _size;
		this.data = null;
	}

	// Token: 0x0600453F RID: 17727 RVA: 0x001B6506 File Offset: 0x001B4706
	[PublicizedFrom(EAccessModifier.Private)]
	public void Alloc()
	{
		this.data = new byte[this.size.x * this.size.y * this.size.z + 7 >> 3];
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x001B653A File Offset: 0x001B473A
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

	// Token: 0x06004541 RID: 17729 RVA: 0x001B6568 File Offset: 0x001B4768
	public void Add(int x, int y, int z)
	{
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		this.Add(num);
	}

	// Token: 0x06004542 RID: 17730 RVA: 0x001B65A8 File Offset: 0x001B47A8
	public bool Contains(int x, int y, int z)
	{
		if (this.data == null)
		{
			return false;
		}
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		return ((int)this.data[num >> 3] & (1 << (num & 7))) > 0;
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x001B65FF File Offset: 0x001B47FF
	public PrefabInsideDataFile Clone()
	{
		return new PrefabInsideDataFile(this);
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x001B6608 File Offset: 0x001B4808
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

	// Token: 0x06004545 RID: 17733 RVA: 0x001B66CC File Offset: 0x001B48CC
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

	// Token: 0x06004546 RID: 17734 RVA: 0x001B6730 File Offset: 0x001B4930
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

	// Token: 0x06004547 RID: 17735 RVA: 0x001B67A8 File Offset: 0x001B49A8
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

	// Token: 0x04003784 RID: 14212
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 2;

	// Token: 0x04003785 RID: 14213
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003786 RID: 14214
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
