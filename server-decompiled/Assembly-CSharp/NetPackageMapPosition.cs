using System;
using UnityEngine.Scripting;

// Token: 0x02000805 RID: 2053
[Preserve]
public class NetPackageMapPosition : NetPackage
{
	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06003C71 RID: 15473 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x0018162A File Offset: 0x0017F82A
	public NetPackageMapPosition Setup(int _entityId, Vector2i _mapMiddlePosition)
	{
		this.entityId = _entityId;
		this.mapMiddlePosition = _mapMiddlePosition;
		return this;
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x0018163B File Offset: 0x0017F83B
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.mapMiddlePosition = StreamUtils.ReadVector2i(_br);
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x00181655 File Offset: 0x0017F855
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.mapMiddlePosition);
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x00181678 File Offset: 0x0017F878
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

	// Token: 0x06003C76 RID: 15478 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x040030D5 RID: 12501
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030D6 RID: 12502
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i mapMiddlePosition;
}
