using System;
using UnityEngine;

// Token: 0x02000C7F RID: 3199
public class TileEntityLight : TileEntity
{
	// Token: 0x06006127 RID: 24871 RVA: 0x002698EC File Offset: 0x00267AEC
	public TileEntityLight(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x00269949 File Offset: 0x00267B49
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Light;
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x00269950 File Offset: 0x00267B50
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

	// Token: 0x0600612A RID: 24874 RVA: 0x002699E0 File Offset: 0x00267BE0
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

	// Token: 0x0600612B RID: 24875 RVA: 0x00269A6C File Offset: 0x00267C6C
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

	// Token: 0x0600612C RID: 24876 RVA: 0x00269B20 File Offset: 0x00267D20
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

	// Token: 0x04004C49 RID: 19529
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C4A RID: 19530
	public LightType LightType = LightType.Point;

	// Token: 0x04004C4B RID: 19531
	public LightShadows LightShadows;

	// Token: 0x04004C4C RID: 19532
	public float LightIntensity = 1f;

	// Token: 0x04004C4D RID: 19533
	public float LightRange = 10f;

	// Token: 0x04004C4E RID: 19534
	public float LightAngle = 45f;

	// Token: 0x04004C4F RID: 19535
	public Color LightColor = Color.white;

	// Token: 0x04004C50 RID: 19536
	public LightStateType LightState;

	// Token: 0x04004C51 RID: 19537
	public float Rate = 1f;

	// Token: 0x04004C52 RID: 19538
	public float Delay = 1f;
}
