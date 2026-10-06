using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200082F RID: 2095
[Preserve]
public class NetPackagePlayerSetBackpackPosition : NetPackage
{
	// Token: 0x06003D3F RID: 15679 RVA: 0x0018603A File Offset: 0x0018423A
	public NetPackagePlayerSetBackpackPosition Setup(int _playerId, List<Vector3i> _positions)
	{
		this.playerId = _playerId;
		this.positions = _positions;
		return this;
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x0018604C File Offset: 0x0018424C
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

	// Token: 0x06003D41 RID: 15681 RVA: 0x00186098 File Offset: 0x00184298
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

	// Token: 0x06003D42 RID: 15682 RVA: 0x00186104 File Offset: 0x00184304
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

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06003D43 RID: 15683 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031BF RID: 12735
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031C0 RID: 12736
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> positions;
}
