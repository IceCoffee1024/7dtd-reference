using System;
using UnityEngine;

// Token: 0x02000A81 RID: 2689
public class ImposterCanvas
{
	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060050DD RID: 20701 RVA: 0x001F0A8D File Offset: 0x001EEC8D
	// (set) Token: 0x060050DE RID: 20702 RVA: 0x001F0AAF File Offset: 0x001EECAF
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

	// Token: 0x060050DF RID: 20703 RVA: 0x001F0AD4 File Offset: 0x001EECD4
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

	// Token: 0x060050E0 RID: 20704 RVA: 0x001F0B3C File Offset: 0x001EED3C
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

	// Token: 0x060050E1 RID: 20705 RVA: 0x001F0C10 File Offset: 0x001EEE10
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

	// Token: 0x04003E5E RID: 15966
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04003E5F RID: 15967
	public Vector3 WorldPosition;

	// Token: 0x04003E60 RID: 15968
	public Quaternion WorldRotation;

	// Token: 0x04003E61 RID: 15969
	public Vector2 Size;

	// Token: 0x04003E62 RID: 15970
	public float CanvasAspect;

	// Token: 0x04003E63 RID: 15971
	public bool IsDecal;

	// Token: 0x04003E64 RID: 15972
	public SignCanvas Canvas;

	// Token: 0x04003E65 RID: 15973
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
