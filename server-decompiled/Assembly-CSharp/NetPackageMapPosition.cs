using System;
using UnityEngine.Scripting;

// Token: 0x02000810 RID: 2064
[Preserve]
public class NetPackageMapPosition : NetPackage
{
	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x00183A02 File Offset: 0x00181C02
	public NetPackageMapPosition Setup(int _entityId, Vector2i _mapMiddlePosition)
	{
		this.entityId = _entityId;
		this.mapMiddlePosition = _mapMiddlePosition;
		return this;
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x00183A13 File Offset: 0x00181C13
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.mapMiddlePosition = StreamUtils.ReadVector2i(_br);
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x00183A2D File Offset: 0x00181C2D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		StreamUtils.Write(_bw, this.mapMiddlePosition);
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x00183A50 File Offset: 0x00181C50
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

	// Token: 0x06003CD5 RID: 15573 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04003154 RID: 12628
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003155 RID: 12629
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i mapMiddlePosition;
}
