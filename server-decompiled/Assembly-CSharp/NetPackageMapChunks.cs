using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200080D RID: 2061
[Preserve]
public class NetPackageMapChunks : NetPackage
{
	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06003C94 RID: 15508 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06003C95 RID: 15509 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06003C96 RID: 15510 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x001833D9 File Offset: 0x001815D9
	public NetPackageMapChunks Setup(int _entityId, List<int> _chunks, List<ushort[]> _mapPieces)
	{
		this.entityId = _entityId;
		this.chunks = new List<int>(_chunks);
		this.mapPieces = new List<ushort[]>(_mapPieces);
		return this;
	}

	// Token: 0x06003C98 RID: 15512 RVA: 0x001833FC File Offset: 0x001815FC
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

	// Token: 0x06003C99 RID: 15513 RVA: 0x00183480 File Offset: 0x00181680
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

	// Token: 0x06003C9A RID: 15514 RVA: 0x0018358C File Offset: 0x0018178C
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

	// Token: 0x04003135 RID: 12597
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003136 RID: 12598
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> chunks;

	// Token: 0x04003137 RID: 12599
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ushort[]> mapPieces;
}
