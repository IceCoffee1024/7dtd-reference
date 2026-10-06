using System;
using UnityEngine.Scripting;

// Token: 0x0200080E RID: 2062
[Preserve]
public class NetPackageMapPosition : NetPackage
{
	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06003C9C RID: 15516 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C9D RID: 15517 RVA: 0x001835E1 File Offset: 0x001817E1
	public NetPackageMapPosition Setup(int _entityId, Vector2i _mapMiddlePosition)
	{
		this.entityId = _entityId;
		this.mapMiddlePosition = _mapMiddlePosition;
		return this;
	}

	// Token: 0x06003C9E RID: 15518 RVA: 0x001835F2 File Offset: 0x001817F2
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.mapMiddlePosition = StreamUtils.ReadVector2i(_br);
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x0018360C File Offset: 0x0018180C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.mapMiddlePosition);
	}

	// Token: 0x06003CA0 RID: 15520 RVA: 0x00183630 File Offset: 0x00181830
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.entityId) as EntityPlayer;
		if (entityPlayer != null && entityPlayer.ChunkObserver.mapDatabase != null)
		{
			entityPlayer.ChunkObserver.mapDatabase.SetClientMapMiddlePosition(this.mapMiddlePosition);
		}
	}

	// Token: 0x04003138 RID: 12600
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003139 RID: 12601
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i mapMiddlePosition;
}
