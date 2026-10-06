using System;

// Token: 0x02000C8A RID: 3210
public class TileEntitySleeper : TileEntity
{
	// Token: 0x060061D2 RID: 25042 RVA: 0x0026C883 File Offset: 0x0026AA83
	public TileEntitySleeper(Chunk _chunk)
		: base(_chunk)
	{
		this.priorityMultiplier = 1f;
		this.sightAngle = -1;
		this.sightRange = -1;
		this.hearingPercent = 1f;
	}

	// Token: 0x060061D3 RID: 25043 RVA: 0x0026C8B0 File Offset: 0x0026AAB0
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

	// Token: 0x060061D4 RID: 25044 RVA: 0x0026C904 File Offset: 0x0026AB04
	public override void CopyFrom(TileEntity _other)
	{
		TileEntitySleeper tileEntitySleeper = (TileEntitySleeper)_other;
		this.priorityMultiplier = tileEntitySleeper.priorityMultiplier;
		this.sightAngle = tileEntitySleeper.sightAngle;
		this.sightRange = tileEntitySleeper.sightRange;
		this.hearingPercent = tileEntitySleeper.hearingPercent;
	}

	// Token: 0x060061D5 RID: 25045 RVA: 0x000F659D File Offset: 0x000F479D
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Sleeper;
	}

	// Token: 0x060061D6 RID: 25046 RVA: 0x0026C948 File Offset: 0x0026AB48
	public void SetPriorityMultiplier(float _priorityMultiplier)
	{
		this.priorityMultiplier = _priorityMultiplier;
		this.setModified();
	}

	// Token: 0x060061D7 RID: 25047 RVA: 0x0026C957 File Offset: 0x0026AB57
	public float GetPriorityMultiplier()
	{
		return this.priorityMultiplier;
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x0026C95F File Offset: 0x0026AB5F
	public void SetSightAngle(int _sightAngle)
	{
		this.sightAngle = _sightAngle;
		this.setModified();
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x0026C96E File Offset: 0x0026AB6E
	public int GetSightAngle()
	{
		return this.sightAngle;
	}

	// Token: 0x060061DA RID: 25050 RVA: 0x0026C976 File Offset: 0x0026AB76
	public void SetSightRange(int _sightRange)
	{
		this.sightRange = _sightRange;
		this.setModified();
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x0026C985 File Offset: 0x0026AB85
	public int GetSightRange()
	{
		return this.sightRange;
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x0026C98D File Offset: 0x0026AB8D
	public void SetHearingPercent(float _hearingPercent)
	{
		this.hearingPercent = _hearingPercent;
		this.setModified();
	}

	// Token: 0x060061DD RID: 25053 RVA: 0x0026C99C File Offset: 0x0026AB9C
	public float GetHearingPercent()
	{
		return this.hearingPercent;
	}

	// Token: 0x060061DE RID: 25054 RVA: 0x0026C9A4 File Offset: 0x0026ABA4
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
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

	// Token: 0x060061DF RID: 25055 RVA: 0x0026CA04 File Offset: 0x0026AC04
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.priorityMultiplier);
		_bw.Write((short)this.sightRange);
		_bw.Write(this.hearingPercent);
		_bw.Write((short)this.sightAngle);
	}

	// Token: 0x04004C8D RID: 19597
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C8E RID: 19598
	[PublicizedFrom(EAccessModifier.Private)]
	public float priorityMultiplier;

	// Token: 0x04004C8F RID: 19599
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightAngle;

	// Token: 0x04004C90 RID: 19600
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightRange;

	// Token: 0x04004C91 RID: 19601
	[PublicizedFrom(EAccessModifier.Private)]
	public float hearingPercent;
}
