using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000472 RID: 1138
[Preserve]
public class OwnedEntityData
{
	// Token: 0x06002233 RID: 8755 RVA: 0x000CE51F File Offset: 0x000CC71F
	public OwnedEntityData()
	{
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000CE535 File Offset: 0x000CC735
	public OwnedEntityData(Entity _entity)
	{
		this.Id = _entity.entityId;
		this.ClassId = _entity.entityClass;
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000CE563 File Offset: 0x000CC763
	public OwnedEntityData(int _entityId, int _classId)
	{
		this.Id = _entityId;
		this.ClassId = _classId;
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06002236 RID: 8758 RVA: 0x000CE587 File Offset: 0x000CC787
	public bool hasLastKnownPosition
	{
		get
		{
			return (this.saveFlags & 1) > 0;
		}
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000CE594 File Offset: 0x000CC794
	public void SetLastKnownPosition(Vector3 pos)
	{
		this.LastKnownPosition = pos;
		this.saveFlags |= 1;
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000CE5AC File Offset: 0x000CC7AC
	public void ClearLastKnownPostition()
	{
		this.LastKnownPosition = Vector3.zero;
		this.saveFlags = (ushort)((int)this.saveFlags & -2);
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000CE5CC File Offset: 0x000CC7CC
	public void Read(PooledBinaryReader _br)
	{
		this.Id = _br.ReadInt32();
		this.ClassId = _br.ReadInt32();
		this.saveFlags = _br.ReadUInt16();
		if ((this.saveFlags & 1) > 0)
		{
			this.LastKnownPosition.x = (float)_br.ReadInt32();
			this.LastKnownPosition.y = (float)_br.ReadInt32();
			this.LastKnownPosition.z = (float)_br.ReadInt32();
		}
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000CE640 File Offset: 0x000CC840
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.Id);
		_bw.Write(this.ClassId);
		_bw.Write(this.saveFlags);
		if ((this.saveFlags & 1) > 0)
		{
			_bw.Write((int)this.LastKnownPosition.x);
			_bw.Write((int)this.LastKnownPosition.y);
			_bw.Write((int)this.LastKnownPosition.z);
		}
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000CE6B2 File Offset: 0x000CC8B2
	public override string ToString()
	{
		return string.Format("entityId: {0} classId: {1}", this.Id, this.ClassId);
	}

	// Token: 0x040017FB RID: 6139
	public int Id = -1;

	// Token: 0x040017FC RID: 6140
	public int ClassId = -1;

	// Token: 0x040017FD RID: 6141
	public Vector3 LastKnownPosition;

	// Token: 0x040017FE RID: 6142
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort saveFlags;

	// Token: 0x040017FF RID: 6143
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cHasLastKnownPosition = 1;
}
