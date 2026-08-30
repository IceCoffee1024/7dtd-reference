using System;
using System.IO;
using UnityEngine;

// Token: 0x02000A5A RID: 2650
public class ImposterCanvas
{
	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x0600504B RID: 20555 RVA: 0x001EB393 File Offset: 0x001E9593
	// (set) Token: 0x0600504C RID: 20556 RVA: 0x001EB3B5 File Offset: 0x001E95B5
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

	// Token: 0x0600504D RID: 20557 RVA: 0x001EB3DC File Offset: 0x001E95DC
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

	// Token: 0x0600504E RID: 20558 RVA: 0x001EB444 File Offset: 0x001E9644
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

	// Token: 0x0600504F RID: 20559 RVA: 0x001EB518 File Offset: 0x001E9718
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

	// Token: 0x04003D90 RID: 15760
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04003D91 RID: 15761
	public Vector3 WorldPosition;

	// Token: 0x04003D92 RID: 15762
	public Quaternion WorldRotation;

	// Token: 0x04003D93 RID: 15763
	public Vector2 Size;

	// Token: 0x04003D94 RID: 15764
	public float CanvasAspect;

	// Token: 0x04003D95 RID: 15765
	public bool IsDecal;

	// Token: 0x04003D96 RID: 15766
	public SignCanvas Canvas;

	// Token: 0x04003D97 RID: 15767
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
