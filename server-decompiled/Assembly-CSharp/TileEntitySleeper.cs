using System;

// Token: 0x02000C7B RID: 3195
public class TileEntitySleeper : TileEntity
{
	// Token: 0x060061D9 RID: 25049 RVA: 0x002693DB File Offset: 0x002675DB
	public TileEntitySleeper(Chunk _chunk)
		: base(_chunk)
	{
		this.priorityMultiplier = 1f;
		this.sightAngle = -1;
		this.sightRange = -1;
		this.hearingPercent = 1f;
	}

	// Token: 0x060061DA RID: 25050 RVA: 0x00269408 File Offset: 0x00267608
	public override TileEntity Clone()
	{
		return new TileEntitySleeper(this.chunk)
		{
			localChunkPos = base.localChunkPos,
			priorityMultiplier = this.priorityMultiplier,
			sightAngle = this.sightAngle,
			sightRange = this.sightRange,
			hearingPercent = this.hearingPercent
		};
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x0026945C File Offset: 0x0026765C
	public override void CopyFrom(TileEntity _other)
	{
		TileEntitySleeper tileEntitySleeper = (TileEntitySleeper)_other;
		this.priorityMultiplier = tileEntitySleeper.priorityMultiplier;
		this.sightAngle = tileEntitySleeper.sightAngle;
		this.sightRange = tileEntitySleeper.sightRange;
		this.hearingPercent = tileEntitySleeper.hearingPercent;
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Sleeper;
	}

	// Token: 0x060061DD RID: 25053 RVA: 0x002694A0 File Offset: 0x002676A0
	public void SetPriorityMultiplier(float _priorityMultiplier)
	{
		this.priorityMultiplier = _priorityMultiplier;
		this.setModified();
	}

	// Token: 0x060061DE RID: 25054 RVA: 0x002694AF File Offset: 0x002676AF
	public float GetPriorityMultiplier()
	{
		return this.priorityMultiplier;
	}

	// Token: 0x060061DF RID: 25055 RVA: 0x002694B7 File Offset: 0x002676B7
	public void SetSightAngle(int _sightAngle)
	{
		this.sightAngle = _sightAngle;
		this.setModified();
	}

	// Token: 0x060061E0 RID: 25056 RVA: 0x002694C6 File Offset: 0x002676C6
	public int GetSightAngle()
	{
		return this.sightAngle;
	}

	// Token: 0x060061E1 RID: 25057 RVA: 0x002694CE File Offset: 0x002676CE
	public void SetSightRange(int _sightRange)
	{
		this.sightRange = _sightRange;
		this.setModified();
	}

	// Token: 0x060061E2 RID: 25058 RVA: 0x002694DD File Offset: 0x002676DD
	public int GetSightRange()
	{
		return this.sightRange;
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x002694E5 File Offset: 0x002676E5
	public void SetHearingPercent(float _hearingPercent)
	{
		this.hearingPercent = _hearingPercent;
		this.setModified();
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x002694F4 File Offset: 0x002676F4
	public float GetHearingPercent()
	{
		return this.hearingPercent;
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x002694FC File Offset: 0x002676FC
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		this.priorityMultiplier = _br.ReadSingle();
		this.sightRange = (int)_br.ReadInt16();
		this.hearingPercent = _br.ReadSingle();
		this.sightAngle = (int)_br.ReadInt16();
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x0026955C File Offset: 0x0026775C
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.priorityMultiplier);
		_bw.Write((short)this.sightRange);
		_bw.Write(this.hearingPercent);
		_bw.Write((short)this.sightAngle);
	}

	// Token: 0x04004C56 RID: 19542
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C57 RID: 19543
	[PublicizedFrom(EAccessModifier.Private)]
	public float priorityMultiplier;

	// Token: 0x04004C58 RID: 19544
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightAngle;

	// Token: 0x04004C59 RID: 19545
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightRange;

	// Token: 0x04004C5A RID: 19546
	[PublicizedFrom(EAccessModifier.Private)]
	public float hearingPercent;
}
