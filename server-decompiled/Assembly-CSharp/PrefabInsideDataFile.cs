using System;
using System.IO;

// Token: 0x0200094B RID: 2379
public class PrefabInsideDataFile
{
	// Token: 0x060045BD RID: 17853 RVA: 0x0000640C File Offset: 0x0000460C
	public PrefabInsideDataFile()
	{
	}

	// Token: 0x060045BE RID: 17854 RVA: 0x001B8AAE File Offset: 0x001B6CAE
	public PrefabInsideDataFile(PrefabInsideDataFile _other)
	{
		this.size = _other.size;
		this.data = _other.data;
	}

	// Token: 0x060045BF RID: 17855 RVA: 0x001B8ACE File Offset: 0x001B6CCE
	public void Init(Vector3i _size)
	{
		this.size = _size;
		this.data = null;
	}

	// Token: 0x060045C0 RID: 17856 RVA: 0x001B8ADE File Offset: 0x001B6CDE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Alloc()
	{
		this.data = new byte[this.size.x * this.size.y * this.size.z + 7 >> 3];
	}

	// Token: 0x060045C1 RID: 17857 RVA: 0x001B8B12 File Offset: 0x001B6D12
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

	// Token: 0x060045C2 RID: 17858 RVA: 0x001B8B40 File Offset: 0x001B6D40
	public void Add(int x, int y, int z)
	{
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		this.Add(num);
	}

	// Token: 0x060045C3 RID: 17859 RVA: 0x001B8B80 File Offset: 0x001B6D80
	public bool Contains(int x, int y, int z)
	{
		if (this.data == null)
		{
			return false;
		}
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		return ((int)this.data[num >> 3] & (1 << (num & 7))) > 0;
	}

	// Token: 0x060045C4 RID: 17860 RVA: 0x001B8BD7 File Offset: 0x001B6DD7
	public PrefabInsideDataFile Clone()
	{
		return new PrefabInsideDataFile(this);
	}

	// Token: 0x060045C5 RID: 17861 RVA: 0x001B8BE0 File Offset: 0x001B6DE0
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

	// Token: 0x060045C6 RID: 17862 RVA: 0x001B8CA4 File Offset: 0x001B6EA4
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

	// Token: 0x060045C7 RID: 17863 RVA: 0x001B8D08 File Offset: 0x001B6F08
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

	// Token: 0x060045C8 RID: 17864 RVA: 0x001B8D80 File Offset: 0x001B6F80
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

	// Token: 0x0400380D RID: 14349
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 2;

	// Token: 0x0400380E RID: 14350
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x0400380F RID: 14351
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
