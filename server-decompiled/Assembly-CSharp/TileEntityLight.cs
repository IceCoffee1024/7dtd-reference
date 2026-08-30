using System;
using UnityEngine;

// Token: 0x02000C70 RID: 3184
public class TileEntityLight : TileEntity
{
	// Token: 0x0600612A RID: 24874 RVA: 0x0026644C File Offset: 0x0026464C
	public TileEntityLight(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x002664A9 File Offset: 0x002646A9
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Light;
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x002664B0 File Offset: 0x002646B0
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

	// Token: 0x0600612D RID: 24877 RVA: 0x00266540 File Offset: 0x00264740
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

	// Token: 0x0600612E RID: 24878 RVA: 0x002665CC File Offset: 0x002647CC
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

	// Token: 0x0600612F RID: 24879 RVA: 0x00266680 File Offset: 0x00264880
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

	// Token: 0x04004C12 RID: 19474
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C13 RID: 19475
	public LightType LightType = LightType.Point;

	// Token: 0x04004C14 RID: 19476
	public LightShadows LightShadows;

	// Token: 0x04004C15 RID: 19477
	public float LightIntensity = 1f;

	// Token: 0x04004C16 RID: 19478
	public float LightRange = 10f;

	// Token: 0x04004C17 RID: 19479
	public float LightAngle = 45f;

	// Token: 0x04004C18 RID: 19480
	public Color LightColor = Color.white;

	// Token: 0x04004C19 RID: 19481
	public LightStateType LightState;

	// Token: 0x04004C1A RID: 19482
	public float Rate = 1f;

	// Token: 0x04004C1B RID: 19483
	public float Delay = 1f;
}
