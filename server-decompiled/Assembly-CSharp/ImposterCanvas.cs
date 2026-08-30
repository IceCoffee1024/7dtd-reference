using System;
using System.IO;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
public class ImposterCanvas
{
	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x060050D1 RID: 20689 RVA: 0x001EDE2B File Offset: 0x001EC02B
	// (set) Token: 0x060050D2 RID: 20690 RVA: 0x001EDE4D File Offset: 0x001EC04D
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

	// Token: 0x060050D3 RID: 20691 RVA: 0x001EDE74 File Offset: 0x001EC074
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

	// Token: 0x060050D4 RID: 20692 RVA: 0x001EDEDC File Offset: 0x001EC0DC
	public void Write(BinaryWriter _bw)
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

	// Token: 0x060050D5 RID: 20693 RVA: 0x001EDFB0 File Offset: 0x001EC1B0
	public static ImposterCanvas Read(BinaryReader _br)
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

	// Token: 0x04003E21 RID: 15905
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04003E22 RID: 15906
	public Vector3 WorldPosition;

	// Token: 0x04003E23 RID: 15907
	public Quaternion WorldRotation;

	// Token: 0x04003E24 RID: 15908
	public Vector2 Size;

	// Token: 0x04003E25 RID: 15909
	public float CanvasAspect;

	// Token: 0x04003E26 RID: 15910
	public bool IsDecal;

	// Token: 0x04003E27 RID: 15911
	public SignCanvas Canvas;

	// Token: 0x04003E28 RID: 15912
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
