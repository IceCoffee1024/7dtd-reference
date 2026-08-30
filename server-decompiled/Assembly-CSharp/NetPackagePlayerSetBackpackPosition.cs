using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000823 RID: 2083
[Preserve]
public class NetPackagePlayerSetBackpackPosition : NetPackage
{
	// Token: 0x06003D22 RID: 15650 RVA: 0x0018412A File Offset: 0x0018232A
	public NetPackagePlayerSetBackpackPosition Setup(int _playerId, List<Vector3i> _positions)
	{
		this.playerId = _playerId;
		this.positions = _positions;
		return this;
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x0018413C File Offset: 0x0018233C
	public override void read(PooledBinaryReader _br)
	{
		this.playerId = _br.ReadInt32();
		int num = (int)_br.ReadByte();
		this.positions = new List<Vector3i>();
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.positions.Add(StreamUtils.ReadVector3i(_br));
			}
		}
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x00184188 File Offset: 0x00182388
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.playerId);
		if (this.positions == null)
		{
			_bw.Write(0);
			return;
		}
		_bw.Write((byte)this.positions.Count);
		for (int i = 0; i < this.positions.Count; i++)
		{
			StreamUtils.Write(_bw, this.positions[i]);
		}
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x001841F4 File Offset: 0x001823F4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = GameManager.Instance.World.GetEntity(this.playerId) as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.SetDroppedBackpackPositions(this.positions);
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06003D26 RID: 15654 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04003159 RID: 12633
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400315A RID: 12634
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> positions;
}
