using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200080F RID: 2063
[Preserve]
public class NetPackageMapChunks : NetPackage
{
	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x00020021 File Offset: 0x0001E221
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x001837BF File Offset: 0x001819BF
	public NetPackageMapChunks Setup(int _entityId, List<int> _chunks, List<ushort[]> _mapPieces)
	{
		this.entityId = _entityId;
		this.chunks = new List<int>(_chunks);
		this.mapPieces = new List<ushort[]>(_mapPieces);
		return this;
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x001837E4 File Offset: 0x001819E4
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

	// Token: 0x06003CCC RID: 15564 RVA: 0x00183868 File Offset: 0x00181A68
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

	// Token: 0x06003CCD RID: 15565 RVA: 0x00183974 File Offset: 0x00181B74
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

	// Token: 0x06003CCE RID: 15566 RVA: 0x001839C9 File Offset: 0x00181BC9
	public override int GetLength()
	{
		return 4 + ((this.chunks != null) ? (this.chunks.Count * 8) : 0) + ((this.mapPieces != null) ? (this.mapPieces.Count * 512) : 0);
	}

	// Token: 0x04003151 RID: 12625
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003152 RID: 12626
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> chunks;

	// Token: 0x04003153 RID: 12627
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ushort[]> mapPieces;
}
