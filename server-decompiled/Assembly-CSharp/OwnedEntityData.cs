using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000474 RID: 1140
[Preserve]
public class OwnedEntityData
{
	// Token: 0x06002222 RID: 8738 RVA: 0x000CE5D3 File Offset: 0x000CC7D3
	public OwnedEntityData()
	{
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x000CE5E9 File Offset: 0x000CC7E9
	public OwnedEntityData(Entity _entity)
	{
		this.Id = _entity.entityId;
		this.ClassId = _entity.entityClass;
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000CE617 File Offset: 0x000CC817
	public OwnedEntityData(int _entityId, int _classId)
	{
		this.Id = _entityId;
		this.ClassId = _classId;
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06002225 RID: 8741 RVA: 0x000CE63B File Offset: 0x000CC83B
	public bool hasLastKnownPosition
	{
		get
		{
			return (this.saveFlags & 1) > 0;
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000CE648 File Offset: 0x000CC848
	public void SetLastKnownPosition(Vector3 pos)
	{
		this.LastKnownPosition = pos;
		this.saveFlags |= 1;
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000CE660 File Offset: 0x000CC860
	public void ClearLastKnownPostition()
	{
		this.LastKnownPosition = Vector3.zero;
		this.saveFlags = (ushort)((int)this.saveFlags & -2);
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000CE680 File Offset: 0x000CC880
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

	// Token: 0x06002229 RID: 8745 RVA: 0x000CE6F4 File Offset: 0x000CC8F4
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

	// Token: 0x0600222A RID: 8746 RVA: 0x000CE766 File Offset: 0x000CC966
	public override string ToString()
	{
		return string.Format("entityId: {0} classId: {1}", this.Id, this.ClassId);
	}

	// Token: 0x040017F2 RID: 6130
	public int Id = -1;

	// Token: 0x040017F3 RID: 6131
	public int ClassId = -1;

	// Token: 0x040017F4 RID: 6132
	public Vector3 LastKnownPosition;

	// Token: 0x040017F5 RID: 6133
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort saveFlags;

	// Token: 0x040017F6 RID: 6134
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cHasLastKnownPosition = 1;
}
