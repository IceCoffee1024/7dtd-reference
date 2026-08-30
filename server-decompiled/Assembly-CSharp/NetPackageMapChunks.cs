using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000804 RID: 2052
[Preserve]
public class NetPackageMapChunks : NetPackage
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06003C68 RID: 15464 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06003C69 RID: 15465 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06003C6A RID: 15466 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x001813E7 File Offset: 0x0017F5E7
	public NetPackageMapChunks Setup(int _entityId, List<int> _chunks, List<ushort[]> _mapPieces)
	{
		this.entityId = _entityId;
		this.chunks = new List<int>(_chunks);
		this.mapPieces = new List<ushort[]>(_mapPieces);
		return this;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x0018140C File Offset: 0x0017F60C
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

	// Token: 0x06003C6D RID: 15469 RVA: 0x00181490 File Offset: 0x0017F690
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

	// Token: 0x06003C6E RID: 15470 RVA: 0x0018159C File Offset: 0x0017F79C
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

	// Token: 0x06003C6F RID: 15471 RVA: 0x001815F1 File Offset: 0x0017F7F1
	public override int GetLength()
	{
		return 4 + ((this.chunks != null) ? (this.chunks.Count * 8) : 0) + ((this.mapPieces != null) ? (this.mapPieces.Count * 512) : 0);
	}

	// Token: 0x040030D2 RID: 12498
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030D3 RID: 12499
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> chunks;

	// Token: 0x040030D4 RID: 12500
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ushort[]> mapPieces;
}
