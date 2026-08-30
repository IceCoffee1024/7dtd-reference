using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200046E RID: 1134
[Preserve]
public class OwnedEntityData
{
	// Token: 0x06002216 RID: 8726 RVA: 0x000CDD83 File Offset: 0x000CBF83
	public OwnedEntityData()
	{
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000CDD99 File Offset: 0x000CBF99
	public OwnedEntityData(Entity _entity)
	{
		this.Id = _entity.entityId;
		this.ClassId = _entity.entityClass;
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000CDDC7 File Offset: 0x000CBFC7
	public OwnedEntityData(int _entityId, int _classId)
	{
		this.Id = _entityId;
		this.ClassId = _classId;
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06002219 RID: 8729 RVA: 0x000CDDEB File Offset: 0x000CBFEB
	public bool hasLastKnownPosition
	{
		get
		{
			return (this.saveFlags & 1) > 0;
		}
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000CDDF8 File Offset: 0x000CBFF8
	public void SetLastKnownPosition(Vector3 pos)
	{
		this.LastKnownPosition = pos;
		this.saveFlags |= 1;
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000CDE10 File Offset: 0x000CC010
	public void ClearLastKnownPostition()
	{
		this.LastKnownPosition = Vector3.zero;
		this.saveFlags = (ushort)((int)this.saveFlags & -2);
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000CDE30 File Offset: 0x000CC030
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

	// Token: 0x0600221D RID: 8733 RVA: 0x000CDEA4 File Offset: 0x000CC0A4
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

	// Token: 0x0600221E RID: 8734 RVA: 0x000CDF16 File Offset: 0x000CC116
	public override string ToString()
	{
		return string.Format("entityId: {0} classId: {1}", this.Id, this.ClassId);
	}

	// Token: 0x040017E5 RID: 6117
	public int Id = -1;

	// Token: 0x040017E6 RID: 6118
	public int ClassId = -1;

	// Token: 0x040017E7 RID: 6119
	public Vector3 LastKnownPosition;

	// Token: 0x040017E8 RID: 6120
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort saveFlags;

	// Token: 0x040017E9 RID: 6121
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cHasLastKnownPosition = 1;
}
