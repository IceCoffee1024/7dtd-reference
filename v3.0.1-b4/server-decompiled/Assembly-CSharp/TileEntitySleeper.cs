using System;

// Token: 0x02000C69 RID: 3177
public class TileEntitySleeper : TileEntity
{
	// Token: 0x0600614E RID: 24910 RVA: 0x002664F7 File Offset: 0x002646F7
	public TileEntitySleeper(Chunk _chunk)
		: base(_chunk)
	{
		this.priorityMultiplier = 1f;
		this.sightAngle = -1;
		this.sightRange = -1;
		this.hearingPercent = 1f;
	}

	// Token: 0x0600614F RID: 24911 RVA: 0x00266524 File Offset: 0x00264724
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

	// Token: 0x06006150 RID: 24912 RVA: 0x00266578 File Offset: 0x00264778
	public override void CopyFrom(TileEntity _other)
	{
		TileEntitySleeper tileEntitySleeper = (TileEntitySleeper)_other;
		this.priorityMultiplier = tileEntitySleeper.priorityMultiplier;
		this.sightAngle = tileEntitySleeper.sightAngle;
		this.sightRange = tileEntitySleeper.sightRange;
		this.hearingPercent = tileEntitySleeper.hearingPercent;
	}

	// Token: 0x06006151 RID: 24913 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Sleeper;
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x002665BC File Offset: 0x002647BC
	public void SetPriorityMultiplier(float _priorityMultiplier)
	{
		this.priorityMultiplier = _priorityMultiplier;
		this.setModified();
	}

	// Token: 0x06006153 RID: 24915 RVA: 0x002665CB File Offset: 0x002647CB
	public float GetPriorityMultiplier()
	{
		return this.priorityMultiplier;
	}

	// Token: 0x06006154 RID: 24916 RVA: 0x002665D3 File Offset: 0x002647D3
	public void SetSightAngle(int _sightAngle)
	{
		this.sightAngle = _sightAngle;
		this.setModified();
	}

	// Token: 0x06006155 RID: 24917 RVA: 0x002665E2 File Offset: 0x002647E2
	public int GetSightAngle()
	{
		return this.sightAngle;
	}

	// Token: 0x06006156 RID: 24918 RVA: 0x002665EA File Offset: 0x002647EA
	public void SetSightRange(int _sightRange)
	{
		this.sightRange = _sightRange;
		this.setModified();
	}

	// Token: 0x06006157 RID: 24919 RVA: 0x002665F9 File Offset: 0x002647F9
	public int GetSightRange()
	{
		return this.sightRange;
	}

	// Token: 0x06006158 RID: 24920 RVA: 0x00266601 File Offset: 0x00264801
	public void SetHearingPercent(float _hearingPercent)
	{
		this.hearingPercent = _hearingPercent;
		this.setModified();
	}

	// Token: 0x06006159 RID: 24921 RVA: 0x00266610 File Offset: 0x00264810
	public float GetHearingPercent()
	{
		return this.hearingPercent;
	}

	// Token: 0x0600615A RID: 24922 RVA: 0x00266618 File Offset: 0x00264818
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

	// Token: 0x0600615B RID: 24923 RVA: 0x00266678 File Offset: 0x00264878
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

	// Token: 0x04004BB9 RID: 19385
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004BBA RID: 19386
	[PublicizedFrom(EAccessModifier.Private)]
	public float priorityMultiplier;

	// Token: 0x04004BBB RID: 19387
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightAngle;

	// Token: 0x04004BBC RID: 19388
	[PublicizedFrom(EAccessModifier.Private)]
	public int sightRange;

	// Token: 0x04004BBD RID: 19389
	[PublicizedFrom(EAccessModifier.Private)]
	public float hearingPercent;
}
