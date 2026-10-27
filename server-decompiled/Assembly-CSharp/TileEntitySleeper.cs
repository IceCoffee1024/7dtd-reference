using System;

// Token: 0x02000C95 RID: 3221
public class TileEntitySleeper : TileEntity
{
	// Token: 0x060061F7 RID: 25079 RVA: 0x0026DDAB File Offset: 0x0026BFAB
	public TileEntitySleeper(Chunk _chunk)
		: base(_chunk)
	{
		this.priorityMultiplier = 1f;
		this.sightAngle = -1;
		this.sightRange = -1;
		this.hearingPercent = 1f;
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x0026DDD8 File Offset: 0x0026BFD8
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

	// Token: 0x060061F9 RID: 25081 RVA: 0x0026DE2C File Offset: 0x0026C02C
	public override void CopyFrom(TileEntity _other)
	{
		TileEntitySleeper tileEntitySleeper = (TileEntitySleeper)_other;
		this.priorityMultiplier = tileEntitySleeper.priorityMultiplier;
		this.sightAngle = tileEntitySleeper.sightAngle;
		this.sightRange = tileEntitySleeper.sightRange;
		this.hearingPercent = tileEntitySleeper.hearingPercent;
	}

	// Token: 0x060061FA RID: 25082 RVA: 0x000F6AD5 File Offset: 0x000F4CD5
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Sleeper;
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x0026DE70 File Offset: 0x0026C070
	public void SetPriorityMultiplier(float _priorityMultiplier)
	{
		this.priorityMultiplier = _priorityMultiplier;
		this.setModified();
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x0026DE7F File Offset: 0x0026C07F
	public float GetPriorityMultiplier()
	{
		return this.priorityMultiplier;
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x0026DE87 File Offset: 0x0026C087
	public void SetSightAngle(int _sightAngle)
	{
		this.sightAngle = _sightAngle;
		this.setModified();
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x0026DE96 File Offset: 0x0026C096
	public int GetSightAngle()
	{
		return this.sightAngle;
	}

	// Token: 0x060061FF RID: 25087 RVA: 0x0026DE9E File Offset: 0x0026C09E
	public void SetSightRange(int _sightRange)
	{
		this.sightRange = _sightRange;
		this.setModified();
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x0026DEAD File Offset: 0x0026C0AD
	public int GetSightRange()
	{
		return this.sightRange;
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x0026DEB5 File Offset: 0x0026C0B5
	public void SetHearingPercent(float _hearingPercent)
	{
		this.hearingPercent = _hearingPercent;
		this.setModified();
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x0026DEC4 File Offset: 0x0026C0C4
	public float GetHearingPercent()
	{
		return this.hearingPercent;
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x0026DECC File Offset: 0x0026C0CC
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

	// Token: 0x06006204 RID: 25092 RVA: 0x0026DF2C File Offset: 0x0026C12C
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

	// Token: 0x04004CB2 RID: 19634
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004CB3 RID: 19635
	[PublicizedFrom(EAccessModifier.Private)]
	public float priorityMultiplier;

	// Token: 0x04004CB4 RID: 19636
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightAngle;

	// Token: 0x04004CB5 RID: 19637
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightRange;

	// Token: 0x04004CB6 RID: 19638
	[PublicizedFrom(EAccessModifier.Private)]
	public float hearingPercent;
}
