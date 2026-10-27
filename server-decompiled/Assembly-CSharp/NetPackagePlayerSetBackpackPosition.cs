using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000839 RID: 2105
[Preserve]
public class NetPackagePlayerSetBackpackPosition : NetPackage
{
	// Token: 0x06003D62 RID: 15714 RVA: 0x0018713E File Offset: 0x0018533E
	public NetPackagePlayerSetBackpackPosition Setup(int _playerId, List<Vector3i> _positions)
	{
		this.playerId = _playerId;
		this.positions = _positions;
		return this;
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x00187150 File Offset: 0x00185350
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

	// Token: 0x06003D64 RID: 15716 RVA: 0x0018719C File Offset: 0x0018539C
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

	// Token: 0x06003D65 RID: 15717 RVA: 0x00187208 File Offset: 0x00185408
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

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06003D66 RID: 15718 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031DF RID: 12767
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031E0 RID: 12768
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> positions;
}
