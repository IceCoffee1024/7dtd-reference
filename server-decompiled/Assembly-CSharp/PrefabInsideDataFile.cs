using System;
using System.IO;

// Token: 0x0200095B RID: 2395
public class PrefabInsideDataFile
{
	// Token: 0x06004581 RID: 17793 RVA: 0x0000640C File Offset: 0x0000460C
	public PrefabInsideDataFile()
	{
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x001BA03A File Offset: 0x001B823A
	public PrefabInsideDataFile(PrefabInsideDataFile _other)
	{
		this.size = _other.size;
		this.data = _other.data;
	}

	// Token: 0x06004583 RID: 17795 RVA: 0x001BA05A File Offset: 0x001B825A
	public void Init(Vector3i _size)
	{
		this.size = _size;
		this.data = null;
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x001BA06A File Offset: 0x001B826A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Alloc()
	{
		this.data = new byte[this.size.x * this.size.y * this.size.z + 7 >> 3];
	}

	// Token: 0x06004585 RID: 17797 RVA: 0x001BA09E File Offset: 0x001B829E
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

	// Token: 0x06004586 RID: 17798 RVA: 0x001BA0CC File Offset: 0x001B82CC
	public void Add(int x, int y, int z)
	{
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		this.Add(num);
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x001BA10C File Offset: 0x001B830C
	public bool Contains(int x, int y, int z)
	{
		if (this.data == null)
		{
			return false;
		}
		int num = x + y * this.size.x + z * this.size.x * this.size.y;
		return ((int)this.data[num >> 3] & (1 << (num & 7))) > 0;
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x001BA163 File Offset: 0x001B8363
	public PrefabInsideDataFile Clone()
	{
		return new PrefabInsideDataFile(this);
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x001BA16C File Offset: 0x001B836C
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

	// Token: 0x0600458A RID: 17802 RVA: 0x001BA230 File Offset: 0x001B8430
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

	// Token: 0x0600458B RID: 17803 RVA: 0x001BA294 File Offset: 0x001B8494
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

	// Token: 0x0600458C RID: 17804 RVA: 0x001BA30C File Offset: 0x001B850C
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

	// Token: 0x04003829 RID: 14377
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 2;

	// Token: 0x0400382A RID: 14378
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x0400382B RID: 14379
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
