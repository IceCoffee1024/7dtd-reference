using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000476 RID: 1142
[Preserve]
public class OwnedEntityData
{
	// Token: 0x06002227 RID: 8743 RVA: 0x000CE97F File Offset: 0x000CCB7F
	public OwnedEntityData()
	{
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000CE995 File Offset: 0x000CCB95
	public OwnedEntityData(Entity _entity)
	{
		this.Id = _entity.entityId;
		this.ClassId = _entity.entityClass;
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000CE9C3 File Offset: 0x000CCBC3
	public OwnedEntityData(int _entityId, int _classId)
	{
		this.Id = _entityId;
		this.ClassId = _classId;
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600222A RID: 8746 RVA: 0x000CE9E7 File Offset: 0x000CCBE7
	public bool hasLastKnownPosition
	{
		get
		{
			return (this.saveFlags & 1) > 0;
		}
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x000CE9F4 File Offset: 0x000CCBF4
	public void SetLastKnownPosition(Vector3 pos)
	{
		this.LastKnownPosition = pos;
		this.saveFlags |= 1;
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000CEA0C File Offset: 0x000CCC0C
	public void ClearLastKnownPostition()
	{
		this.LastKnownPosition = Vector3.zero;
		this.saveFlags = (ushort)((int)this.saveFlags & -2);
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000CEA2C File Offset: 0x000CCC2C
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

	// Token: 0x0600222E RID: 8750 RVA: 0x000CEAA0 File Offset: 0x000CCCA0
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

	// Token: 0x0600222F RID: 8751 RVA: 0x000CEB12 File Offset: 0x000CCD12
	public override string ToString()
	{
		return string.Format("entityId: {0} classId: {1}", this.Id, this.ClassId);
	}

	// Token: 0x04001802 RID: 6146
	public int Id = -1;

	// Token: 0x04001803 RID: 6147
	public int ClassId = -1;

	// Token: 0x04001804 RID: 6148
	public Vector3 LastKnownPosition;

	// Token: 0x04001805 RID: 6149
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort saveFlags;

	// Token: 0x04001806 RID: 6150
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cHasLastKnownPosition = 1;
}
