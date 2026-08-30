using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public class SignCanvas : MonoBehaviour, ISignRenderingDataUpdateListener
{
	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x06005057 RID: 20567 RVA: 0x001EB8BA File Offset: 0x001E9ABA
	// (set) Token: 0x06005058 RID: 20568 RVA: 0x001EB8C2 File Offset: 0x001E9AC2
	public SignCanvas.CanvasState State
	{
		get
		{
			return this._state;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			this._state = value;
			this._signDataAvailabilityDirty = true;
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x06005059 RID: 20569 RVA: 0x001EB8D6 File Offset: 0x001E9AD6
	public bool AllowedOnImposter
	{
		get
		{
			return this.allowedOnImposter;
		}
	}

	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x0600505A RID: 20570 RVA: 0x001EB8DE File Offset: 0x001E9ADE
	// (set) Token: 0x0600505B RID: 20571 RVA: 0x001EB90F File Offset: 0x001E9B0F
	public float CanvasAspect
	{
		get
		{
			if (this.canvasDimensions.y == 0f)
			{
				return 1f;
			}
			return this.canvasDimensions.x / this.canvasDimensions.y;
		}
		set
		{
			this.canvasDimensions = new Vector2(value, 1f);
		}
	}

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x0600505C RID: 20572 RVA: 0x001EB924 File Offset: 0x001E9B24
	// (remove) Token: 0x0600505D RID: 20573 RVA: 0x001EB95C File Offset: 0x001E9B5C
	public event Action RenderingDataUpdated;

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x0600505E RID: 20574 RVA: 0x001EB991 File Offset: 0x001E9B91
	public Texture BakedTexture
	{
		get
		{
			return this.bakedTexture;
		}
	}

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x0600505F RID: 20575 RVA: 0x001EB999 File Offset: 0x001E9B99
	public float SizeSquared
	{
		get
		{
			return this._sizeSquared;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06005060 RID: 20576 RVA: 0x001EB9A1 File Offset: 0x001E9BA1
	// (set) Token: 0x06005061 RID: 20577 RVA: 0x001EB9B0 File Offset: 0x001E9BB0
	public GlobalSignId SignId
	{
		get
		{
			return this.State.SignId;
		}
		set
		{
			if (this.State.SignId.Equals(value))
			{
				return;
			}
			this.State.SignId = value;
			this._signDataAvailabilityDirty = true;
			SignDataManager.Instance.RegisterListener(this.DisplaySignId, this, true);
			SignTextureManager.Instance.Invalidate(this);
			this.RefreshRenderers();
			Action renderingDataUpdated = this.RenderingDataUpdated;
			if (renderingDataUpdated == null)
			{
				return;
			}
			renderingDataUpdated();
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06005062 RID: 20578 RVA: 0x001EBA18 File Offset: 0x001E9C18
	public GlobalSignId DisplaySignId
	{
		get
		{
			if (this._signDataAvailabilityDirty)
			{
				SignData signData;
				this._signDataAvailable = SignDataManager.Instance.TryGetSignData(this.State.SignId, out signData);
				if (!this._signDataAvailable)
				{
					Log.Error(string.Format("Failed to retrieve sign data for global id: '{0}'. World pos: {1}.", this.State.SignId.ToString(), this.GetWorldPosition()));
				}
				this._signDataAvailabilityDirty = false;
			}
			if (!this._signDataAvailable)
			{
				return SignDataManager.ErrorSignId;
			}
			return this.State.SignId;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06005063 RID: 20579 RVA: 0x001EBAA2 File Offset: 0x001E9CA2
	// (set) Token: 0x06005064 RID: 20580 RVA: 0x001EBAAF File Offset: 0x001E9CAF
	public SignCanvas.SignBlendMode BlendMode
	{
		get
		{
			return this.State.BlendMode;
		}
		set
		{
			if (this.State.BlendMode == value)
			{
				return;
			}
			this.State.BlendMode = value;
			this.RefreshRenderers();
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06005065 RID: 20581 RVA: 0x001EBAD2 File Offset: 0x001E9CD2
	// (set) Token: 0x06005066 RID: 20582 RVA: 0x001EBADF File Offset: 0x001E9CDF
	public bool ShowOnImposter
	{
		get
		{
			return this.State.ShowOnImposter;
		}
		set
		{
			this.State.ShowOnImposter = value;
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06005067 RID: 20583 RVA: 0x001EBAED File Offset: 0x001E9CED
	// (set) Token: 0x06005068 RID: 20584 RVA: 0x001EBAFA File Offset: 0x001E9CFA
	public CanvasRotationMode CanvasRotation
	{
		get
		{
			return this.State.CanvasRotation;
		}
		set
		{
			if (this.State.CanvasRotation == value)
			{
				return;
			}
			this.State.CanvasRotation = value;
			this.RefreshRenderers();
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06005069 RID: 20585 RVA: 0x001EBB1D File Offset: 0x001E9D1D
	public float CanvasRotationRadians
	{
		get
		{
			return (float)this.CanvasRotation * 3.1415927f * 0.5f;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x0600506A RID: 20586 RVA: 0x001EBB34 File Offset: 0x001E9D34
	public bool IsDecal
	{
		get
		{
			using (List<SignRenderer>.Enumerator enumerator = this.SignRenderers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsDecal)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x0600506B RID: 20587 RVA: 0x001EBB90 File Offset: 0x001E9D90
	public void Initialize(SignDataManager.RenderingDataPatcher _alternativePatcher = null)
	{
		this._patchRenderingData = _alternativePatcher ?? new SignDataManager.RenderingDataPatcher(this.PatchRenderingData);
		base.GetComponentsInChildren<SignRenderer>(true, this.SignRenderers);
		this.CacheBoundsMetrics();
		this.Register();
		this.RefreshRenderers();
	}

	// Token: 0x0600506C RID: 20588 RVA: 0x001EBBC8 File Offset: 0x001E9DC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CacheBoundsMetrics()
	{
		if (this.SignRenderers.Count == 0)
		{
			this._localCenter = Vector3.zero;
			this._sizeSquared = 1f;
			return;
		}
		Bounds bounds = this.SignRenderers[0].Renderer.bounds;
		for (int i = 1; i < this.SignRenderers.Count; i++)
		{
			bounds.Encapsulate(this.SignRenderers[i].Renderer.bounds);
		}
		this._localCenter = base.transform.InverseTransformPoint(bounds.center);
		Vector3 size = bounds.size;
		float num = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
		this._sizeSquared = ((num > 0f) ? (num * num) : 1f);
	}

	// Token: 0x0600506D RID: 20589 RVA: 0x001EBC98 File Offset: 0x001E9E98
	public void SetTexture(Texture rt)
	{
		if (this.bakedTexture == rt)
		{
			return;
		}
		this.bakedTexture = rt;
		this.RefreshRenderers();
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x001EBCB6 File Offset: 0x001E9EB6
	public Vector3 GetWorldPosition()
	{
		return base.transform.TransformPoint(this._localCenter) + Origin.position;
	}

	// Token: 0x0600506F RID: 20591 RVA: 0x001EBCD4 File Offset: 0x001E9ED4
	public void RefreshRenderers()
	{
		if (this.SignRenderers == null || this.SignRenderers.Count == 0)
		{
			return;
		}
		if (!SignDataManager.Instance.TryApplyRenderingData(this.DisplaySignId, this._patchRenderingData, this.SignRenderers, this.State.BlendMode, null))
		{
			Log.Error(string.Format("Failed to apply rendering data for sign with display id: '{0}'. World pos: {1}.", this.DisplaySignId.ToString(), this.GetWorldPosition()));
		}
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001EBD50 File Offset: 0x001E9F50
	public void PatchRenderingData(MaterialPropertyBlock mpb)
	{
		if (this.bakedTexture != null)
		{
			mpb.SetInteger(SignShaderIDs._UseTexture, 1);
			mpb.SetTexture(SignShaderIDs._BakedTexture, this.bakedTexture);
		}
		else
		{
			mpb.SetInteger(SignShaderIDs._UseTexture, 0);
			mpb.SetTexture(SignShaderIDs._BakedTexture, Texture2D.whiteTexture);
		}
		mpb.SetFloat(SignShaderIDs._CanvasRotation, this.CanvasRotationRadians);
	}

	// Token: 0x06005071 RID: 20593 RVA: 0x001EBDB7 File Offset: 0x001E9FB7
	public void HandleRenderingDataUpdate()
	{
		this.RefreshRenderers();
		SignTextureManager.Instance.Invalidate(this);
		Action renderingDataUpdated = this.RenderingDataUpdated;
		if (renderingDataUpdated == null)
		{
			return;
		}
		renderingDataUpdated();
	}

	// Token: 0x06005072 RID: 20594 RVA: 0x001EBDDA File Offset: 0x001E9FDA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Register()
	{
		SignTextureManager.Instance.Register(this);
		SignDataManager.Instance.RegisterListener(this.DisplaySignId, this, true);
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001EBDF9 File Offset: 0x001E9FF9
	public void Cleanup()
	{
		SignDataManager.Instance.DeregisterListener(this);
		SignTextureManager.Instance.Deregister(this);
		this.bakedTexture = null;
	}

	// Token: 0x06005074 RID: 20596 RVA: 0x00067848 File Offset: 0x00065A48
	[Conditional("DEBUG_LOG_SIGN_CANVAS")]
	public static void DebugLog(string message)
	{
		Log.Out(message);
	}

	// Token: 0x04003D9E RID: 15774
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.CanvasState _state = new SignCanvas.CanvasState();

	// Token: 0x04003D9F RID: 15775
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowedOnImposter = true;

	// Token: 0x04003DA0 RID: 15776
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 canvasDimensions = Vector2.one;

	// Token: 0x04003DA2 RID: 15778
	public List<SignRenderer> SignRenderers = new List<SignRenderer>();

	// Token: 0x04003DA3 RID: 15779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture bakedTexture;

	// Token: 0x04003DA4 RID: 15780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignDataManager.RenderingDataPatcher _patchRenderingData;

	// Token: 0x04003DA5 RID: 15781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailabilityDirty = true;

	// Token: 0x04003DA6 RID: 15782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailable = true;

	// Token: 0x04003DA7 RID: 15783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _localCenter = Vector3.zero;

	// Token: 0x04003DA8 RID: 15784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _sizeSquared = 1f;

	// Token: 0x02000A5E RID: 2654
	public enum SignBlendMode
	{
		// Token: 0x04003DAA RID: 15786
		Cutout,
		// Token: 0x04003DAB RID: 15787
		AlphaBlend
	}

	// Token: 0x02000A5F RID: 2655
	public class CanvasState
	{
		// Token: 0x06005076 RID: 20598 RVA: 0x001EBE77 File Offset: 0x001EA077
		public SignCanvas.CanvasState Clone()
		{
			return new SignCanvas.CanvasState
			{
				SignId = this.SignId,
				BlendMode = this.BlendMode,
				ShowOnImposter = this.ShowOnImposter,
				CanvasRotation = this.CanvasRotation
			};
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x001EBEAE File Offset: 0x001EA0AE
		public void Write(BinaryWriter _bw)
		{
			this.SignId.ToStream(_bw);
			_bw.Write((byte)this.BlendMode);
			_bw.Write((byte)this.CanvasRotation);
			_bw.Write(this.ShowOnImposter);
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x001EBEE1 File Offset: 0x001EA0E1
		public static SignCanvas.CanvasState Read(BinaryReader _br)
		{
			return new SignCanvas.CanvasState
			{
				SignId = GlobalSignId.FromStream(_br),
				BlendMode = (SignCanvas.SignBlendMode)_br.ReadByte(),
				CanvasRotation = (CanvasRotationMode)_br.ReadByte(),
				ShowOnImposter = _br.ReadBoolean()
			};
		}

		// Token: 0x04003DAC RID: 15788
		public GlobalSignId SignId = SignData.defaultSignDataID;

		// Token: 0x04003DAD RID: 15789
		public SignCanvas.SignBlendMode BlendMode;

		// Token: 0x04003DAE RID: 15790
		public bool ShowOnImposter;

		// Token: 0x04003DAF RID: 15791
		public CanvasRotationMode CanvasRotation;
	}
}
