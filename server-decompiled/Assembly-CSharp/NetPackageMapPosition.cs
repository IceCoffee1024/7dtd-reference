using System;
using UnityEngine.Scripting;

// Token: 0x02000818 RID: 2072
[Preserve]
public class NetPackageMapPosition : NetPackage
{
	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06003CBF RID: 15551 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x001846E5 File Offset: 0x001828E5
	public NetPackageMapPosition Setup(int _entityId, Vector2i _mapMiddlePosition)
	{
		this.entityId = _entityId;
		this.mapMiddlePosition = _mapMiddlePosition;
		return this;
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x001846F6 File Offset: 0x001828F6
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.mapMiddlePosition = StreamUtils.ReadVector2i(_br);
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x00184710 File Offset: 0x00182910
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.mapMiddlePosition);
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x00184734 File Offset: 0x00182934
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

	// Token: 0x04003158 RID: 12632
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003159 RID: 12633
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i mapMiddlePosition;
}
