using System;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
public class ImposterCanvas
{
	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x060050B9 RID: 20665 RVA: 0x001EF9B1 File Offset: 0x001EDBB1
	// (set) Token: 0x060050BA RID: 20666 RVA: 0x001EF9D3 File Offset: 0x001EDBD3
	public SignCanvas.CanvasState State
	{
		get
		{
			if (!(this.Canvas != null))
			{
				return this.pendingCanvasState;
			}
			return this.Canvas.State;
		}
		set
		{
			if (this.Canvas != null)
			{
				this.Canvas.State = value;
				return;
			}
			this.pendingCanvasState = value;
		}
	}

	// Token: 0x060050BB RID: 20667 RVA: 0x001EF9F8 File Offset: 0x001EDBF8
	public ImposterCanvas Clone()
	{
		ImposterCanvas imposterCanvas = new ImposterCanvas();
		imposterCanvas.WorldPosition = this.WorldPosition;
		imposterCanvas.WorldRotation = this.WorldRotation;
		imposterCanvas.Size = this.Size;
		imposterCanvas.CanvasAspect = this.CanvasAspect;
		imposterCanvas.IsDecal = this.IsDecal;
		SignCanvas.CanvasState state = this.State;
		imposterCanvas.pendingCanvasState = ((state != null) ? state.Clone() : null);
		return imposterCanvas;
	}

	// Token: 0x060050BC RID: 20668 RVA: 0x001EFA60 File Offset: 0x001EDC60
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(2);
		_bw.Write(this.WorldPosition.x);
		_bw.Write(this.WorldPosition.y);
		_bw.Write(this.WorldPosition.z);
		_bw.Write(this.WorldRotation.x);
		_bw.Write(this.WorldRotation.y);
		_bw.Write(this.WorldRotation.z);
		_bw.Write(this.WorldRotation.w);
		_bw.Write(this.Size.x);
		_bw.Write(this.Size.y);
		_bw.Write(this.CanvasAspect);
		_bw.Write(this.IsDecal);
		this.State.Write(_bw);
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001EFB34 File Offset: 0x001EDD34
	public static ImposterCanvas Read(PooledBinaryReader _br)
	{
		ImposterCanvas imposterCanvas = new ImposterCanvas();
		ushort num = _br.ReadUInt16();
		imposterCanvas.WorldPosition.x = _br.ReadSingle();
		imposterCanvas.WorldPosition.y = _br.ReadSingle();
		imposterCanvas.WorldPosition.z = _br.ReadSingle();
		imposterCanvas.WorldRotation.x = _br.ReadSingle();
		imposterCanvas.WorldRotation.y = _br.ReadSingle();
		imposterCanvas.WorldRotation.z = _br.ReadSingle();
		imposterCanvas.WorldRotation.w = _br.ReadSingle();
		imposterCanvas.Size.x = _br.ReadSingle();
		imposterCanvas.Size.y = _br.ReadSingle();
		imposterCanvas.CanvasAspect = _br.ReadSingle();
		if (num >= 2)
		{
			imposterCanvas.IsDecal = _br.ReadBoolean();
		}
		imposterCanvas.pendingCanvasState = SignCanvas.CanvasState.Read(_br);
		return imposterCanvas;
	}

	// Token: 0x04003E3D RID: 15933
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04003E3E RID: 15934
	public Vector3 WorldPosition;

	// Token: 0x04003E3F RID: 15935
	public Quaternion WorldRotation;

	// Token: 0x04003E40 RID: 15936
	public Vector2 Size;

	// Token: 0x04003E41 RID: 15937
	public float CanvasAspect;

	// Token: 0x04003E42 RID: 15938
	public bool IsDecal;

	// Token: 0x04003E43 RID: 15939
	public SignCanvas Canvas;

	// Token: 0x04003E44 RID: 15940
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
