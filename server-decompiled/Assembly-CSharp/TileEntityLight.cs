using System;
using UnityEngine;

// Token: 0x02000C5E RID: 3166
public class TileEntityLight : TileEntity
{
	// Token: 0x0600609F RID: 24735 RVA: 0x00263564 File Offset: 0x00261764
	public TileEntityLight(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x002635C1 File Offset: 0x002617C1
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Light;
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x002635C8 File Offset: 0x002617C8
	public override TileEntity Clone()
	{
		return new TileEntityLight(this.chunk)
		{
			localChunkPos = base.localChunkPos,
			LightType = this.LightType,
			LightIntensity = this.LightIntensity,
			LightRange = this.LightRange,
			LightColor = this.LightColor,
			LightAngle = this.LightAngle,
			LightShadows = this.LightShadows,
			LightState = this.LightState,
			Rate = this.Rate,
			Delay = this.Delay
		};
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x00263658 File Offset: 0x00261858
	public override void CopyFrom(TileEntity _other)
	{
		TileEntityLight tileEntityLight = (TileEntityLight)_other;
		base.localChunkPos = tileEntityLight.localChunkPos;
		this.LightType = tileEntityLight.LightType;
		this.LightIntensity = tileEntityLight.LightIntensity;
		this.LightRange = tileEntityLight.LightRange;
		this.LightColor = tileEntityLight.LightColor;
		this.LightAngle = tileEntityLight.LightAngle;
		this.LightShadows = tileEntityLight.LightShadows;
		this.LightState = tileEntityLight.LightState;
		this.Rate = tileEntityLight.Rate;
		this.Delay = tileEntityLight.Delay;
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x002636E4 File Offset: 0x002618E4
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			num = 18;
		}
		else if (base.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.GetLegacyForkVersion();
		}
		this.LightIntensity = _br.ReadSingle();
		this.LightRange = _br.ReadSingle();
		this.LightColor = StreamUtils.ReadColor32(_br);
		if (num > 4)
		{
			this.LightType = (LightType)_br.ReadByte();
			this.LightAngle = _br.ReadSingle();
			this.LightShadows = (LightShadows)_br.ReadByte();
		}
		if (num > 5)
		{
			this.LightState = (LightStateType)_br.ReadByte();
		}
		if (num > 6)
		{
			this.Rate = _br.ReadSingle();
		}
		if (num > 7)
		{
			this.Delay = _br.ReadSingle();
		}
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x00263798 File Offset: 0x00261998
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.LightIntensity);
		_bw.Write(this.LightRange);
		StreamUtils.WriteColor32(_bw, this.LightColor);
		_bw.Write((byte)this.LightType);
		_bw.Write(this.LightAngle);
		_bw.Write((byte)this.LightShadows);
		_bw.Write((byte)this.LightState);
		_bw.Write(this.Rate);
		_bw.Write(this.Delay);
	}

	// Token: 0x04004B75 RID: 19317
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004B76 RID: 19318
	public LightType LightType = LightType.Point;

	// Token: 0x04004B77 RID: 19319
	public LightShadows LightShadows;

	// Token: 0x04004B78 RID: 19320
	public float LightIntensity = 1f;

	// Token: 0x04004B79 RID: 19321
	public float LightRange = 10f;

	// Token: 0x04004B7A RID: 19322
	public float LightAngle = 45f;

	// Token: 0x04004B7B RID: 19323
	public Color LightColor = Color.white;

	// Token: 0x04004B7C RID: 19324
	public LightStateType LightState;

	// Token: 0x04004B7D RID: 19325
	public float Rate = 1f;

	// Token: 0x04004B7E RID: 19326
	public float Delay = 1f;
}
