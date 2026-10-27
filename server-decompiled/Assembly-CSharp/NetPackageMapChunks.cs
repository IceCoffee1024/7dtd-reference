using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000817 RID: 2071
[Preserve]
public class NetPackageMapChunks : NetPackage
{
	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x001844DD File Offset: 0x001826DD
	public NetPackageMapChunks Setup(int _entityId, List<int> _chunks, List<ushort[]> _mapPieces)
	{
		this.entityId = _entityId;
		this.chunks = new List<int>(_chunks);
		this.mapPieces = new List<ushort[]>(_mapPieces);
		return this;
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x00184500 File Offset: 0x00182700
	public override void read(PooledBinaryReader _reader)
	{
		this.chunks = new List<int>();
		this.mapPieces = new List<ushort[]>();
		this.entityId = _reader.ReadInt32();
		int num = (int)_reader.ReadUInt16();
		for (int i = 0; i < num; i++)
		{
			this.chunks.Add(_reader.ReadInt32());
			ushort[] array = new ushort[256];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = _reader.ReadUInt16();
			}
			this.mapPieces.Add(array);
		}
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x00184584 File Offset: 0x00182784
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		bool flag = true;
		ushort num = (ushort)this.chunks.Count;
		long position = _writer.BaseStream.Position;
		_writer.Write(num);
		for (int i = 0; i < this.chunks.Count; i++)
		{
			ushort[] array = this.mapPieces[i];
			if (array.Length != 256)
			{
				Log.Warning("Player map data for entityid {0} of invalid size {1}", new object[] { this.entityId, array.Length });
				num -= 1;
				flag = false;
			}
			else
			{
				_writer.Write(this.chunks[i]);
				for (int j = 0; j < array.Length; j++)
				{
					_writer.Write(array[j]);
				}
			}
		}
		if (!flag)
		{
			long position2 = _writer.BaseStream.Position;
			_writer.BaseStream.Position = position;
			_writer.Write(num);
			_writer.BaseStream.Position = position2;
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00184690 File Offset: 0x00182890
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.entityId) as EntityPlayer;
		if (entityPlayer != null && entityPlayer.ChunkObserver.mapDatabase != null)
		{
			entityPlayer.ChunkObserver.mapDatabase.Add(this.chunks, this.mapPieces);
		}
	}

	// Token: 0x04003155 RID: 12629
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003156 RID: 12630
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> chunks;

	// Token: 0x04003157 RID: 12631
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ushort[]> mapPieces;
}
