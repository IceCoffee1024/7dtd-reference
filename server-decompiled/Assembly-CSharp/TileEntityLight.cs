using System;
using UnityEngine;

// Token: 0x02000C8A RID: 3210
public class TileEntityLight : TileEntity
{
	// Token: 0x0600614C RID: 24908 RVA: 0x0026AE14 File Offset: 0x00269014
	public TileEntityLight(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x0600614D RID: 24909 RVA: 0x0026AE71 File Offset: 0x00269071
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Light;
	}

	// Token: 0x0600614E RID: 24910 RVA: 0x0026AE78 File Offset: 0x00269078
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

	// Token: 0x0600614F RID: 24911 RVA: 0x0026AF08 File Offset: 0x00269108
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

	// Token: 0x06006150 RID: 24912 RVA: 0x0026AF94 File Offset: 0x00269194
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
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

	// Token: 0x06006151 RID: 24913 RVA: 0x0026B048 File Offset: 0x00269248
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
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

	// Token: 0x04004C6E RID: 19566
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C6F RID: 19567
	public LightType LightType = LightType.Point;

	// Token: 0x04004C70 RID: 19568
	public LightShadows LightShadows;

	// Token: 0x04004C71 RID: 19569
	public float LightIntensity = 1f;

	// Token: 0x04004C72 RID: 19570
	public float LightRange = 10f;

	// Token: 0x04004C73 RID: 19571
	public float LightAngle = 45f;

	// Token: 0x04004C74 RID: 19572
	public Color LightColor = Color.white;

	// Token: 0x04004C75 RID: 19573
	public LightStateType LightState;

	// Token: 0x04004C76 RID: 19574
	public float Rate = 1f;

	// Token: 0x04004C77 RID: 19575
	public float Delay = 1f;
}
