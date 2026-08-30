using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200082E RID: 2094
[Preserve]
public class NetPackagePlayerSetBackpackPosition : NetPackage
{
	// Token: 0x06003D81 RID: 15745 RVA: 0x00186502 File Offset: 0x00184702
	public NetPackagePlayerSetBackpackPosition Setup(int _playerId, List<Vector3i> _positions)
	{
		this.playerId = _playerId;
		this.positions = _positions;
		return this;
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x00186514 File Offset: 0x00184714
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

	// Token: 0x06003D83 RID: 15747 RVA: 0x00186560 File Offset: 0x00184760
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

	// Token: 0x06003D84 RID: 15748 RVA: 0x001865CC File Offset: 0x001847CC
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

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06003D85 RID: 15749 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x040031D8 RID: 12760
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031D9 RID: 12761
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> positions;
}
