using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public class SignCanvas : MonoBehaviour, ISignRenderingDataUpdateListener
{
	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x060050DD RID: 20701 RVA: 0x001EE352 File Offset: 0x001EC552
	// (set) Token: 0x060050DE RID: 20702 RVA: 0x001EE35A File Offset: 0x001EC55A
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

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x060050DF RID: 20703 RVA: 0x001EE36E File Offset: 0x001EC56E
	public bool AllowedOnImposter
	{
		get
		{
			return this.allowedOnImposter;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x060050E0 RID: 20704 RVA: 0x001EE376 File Offset: 0x001EC576
	// (set) Token: 0x060050E1 RID: 20705 RVA: 0x001EE3A7 File Offset: 0x001EC5A7
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
	// (add) Token: 0x060050E2 RID: 20706 RVA: 0x001EE3BC File Offset: 0x001EC5BC
	// (remove) Token: 0x060050E3 RID: 20707 RVA: 0x001EE3F4 File Offset: 0x001EC5F4
	public event Action RenderingDataUpdated;

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x060050E4 RID: 20708 RVA: 0x001EE429 File Offset: 0x001EC629
	public Texture BakedTexture
	{
		get
		{
			return this.bakedTexture;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x060050E5 RID: 20709 RVA: 0x001EE431 File Offset: 0x001EC631
	public float SizeSquared
	{
		get
		{
			return this._sizeSquared;
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x060050E6 RID: 20710 RVA: 0x001EE439 File Offset: 0x001EC639
	// (set) Token: 0x060050E7 RID: 20711 RVA: 0x001EE448 File Offset: 0x001EC648
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

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x060050E8 RID: 20712 RVA: 0x001EE4B0 File Offset: 0x001EC6B0
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

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x060050E9 RID: 20713 RVA: 0x001EE53A File Offset: 0x001EC73A
	// (set) Token: 0x060050EA RID: 20714 RVA: 0x001EE547 File Offset: 0x001EC747
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

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x060050EB RID: 20715 RVA: 0x001EE56A File Offset: 0x001EC76A
	// (set) Token: 0x060050EC RID: 20716 RVA: 0x001EE577 File Offset: 0x001EC777
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

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x060050ED RID: 20717 RVA: 0x001EE585 File Offset: 0x001EC785
	// (set) Token: 0x060050EE RID: 20718 RVA: 0x001EE592 File Offset: 0x001EC792
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

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x060050EF RID: 20719 RVA: 0x001EE5B5 File Offset: 0x001EC7B5
	public float CanvasRotationRadians
	{
		get
		{
			return (float)this.CanvasRotation * 3.1415927f * 0.5f;
		}
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x060050F0 RID: 20720 RVA: 0x001EE5CC File Offset: 0x001EC7CC
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

	// Token: 0x060050F1 RID: 20721 RVA: 0x001EE628 File Offset: 0x001EC828
	public void Initialize(SignDataManager.RenderingDataPatcher _alternativePatcher = null)
	{
		this._patchRenderingData = _alternativePatcher ?? new SignDataManager.RenderingDataPatcher(this.PatchRenderingData);
		base.GetComponentsInChildren<SignRenderer>(true, this.SignRenderers);
		this.CacheBoundsMetrics();
		this.Register();
		this.RefreshRenderers();
	}

	// Token: 0x060050F2 RID: 20722 RVA: 0x001EE660 File Offset: 0x001EC860
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

	// Token: 0x060050F3 RID: 20723 RVA: 0x001EE730 File Offset: 0x001EC930
	public void SetTexture(Texture rt)
	{
		if (this.bakedTexture == rt)
		{
			return;
		}
		this.bakedTexture = rt;
		this.RefreshRenderers();
	}

	// Token: 0x060050F4 RID: 20724 RVA: 0x001EE74E File Offset: 0x001EC94E
	public Vector3 GetWorldPosition()
	{
		return base.transform.TransformPoint(this._localCenter) + Origin.position;
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x001EE76C File Offset: 0x001EC96C
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

	// Token: 0x060050F6 RID: 20726 RVA: 0x001EE7E8 File Offset: 0x001EC9E8
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

	// Token: 0x060050F7 RID: 20727 RVA: 0x001EE84F File Offset: 0x001ECA4F
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

	// Token: 0x060050F8 RID: 20728 RVA: 0x001EE872 File Offset: 0x001ECA72
	[PublicizedFrom(EAccessModifier.Private)]
	public void Register()
	{
		SignTextureManager.Instance.Register(this);
		SignDataManager.Instance.RegisterListener(this.DisplaySignId, this, true);
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x001EE891 File Offset: 0x001ECA91
	public void Cleanup()
	{
		SignDataManager.Instance.DeregisterListener(this);
		SignTextureManager.Instance.Deregister(this);
		this.bakedTexture = null;
	}

	// Token: 0x060050FA RID: 20730 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("DEBUG_LOG_SIGN_CANVAS")]
	public static void DebugLog(string message)
	{
		Log.Out(message);
	}

	// Token: 0x04003E2F RID: 15919
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.CanvasState _state = new SignCanvas.CanvasState();

	// Token: 0x04003E30 RID: 15920
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowedOnImposter = true;

	// Token: 0x04003E31 RID: 15921
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 canvasDimensions = Vector2.one;

	// Token: 0x04003E33 RID: 15923
	public List<SignRenderer> SignRenderers = new List<SignRenderer>();

	// Token: 0x04003E34 RID: 15924
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture bakedTexture;

	// Token: 0x04003E35 RID: 15925
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignDataManager.RenderingDataPatcher _patchRenderingData;

	// Token: 0x04003E36 RID: 15926
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailabilityDirty = true;

	// Token: 0x04003E37 RID: 15927
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailable = true;

	// Token: 0x04003E38 RID: 15928
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _localCenter = Vector3.zero;

	// Token: 0x04003E39 RID: 15929
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _sizeSquared = 1f;

	// Token: 0x02000A6F RID: 2671
	public enum SignBlendMode
	{
		// Token: 0x04003E3B RID: 15931
		Cutout,
		// Token: 0x04003E3C RID: 15932
		AlphaBlend
	}

	// Token: 0x02000A70 RID: 2672
	public class CanvasState
	{
		// Token: 0x060050FC RID: 20732 RVA: 0x001EE90F File Offset: 0x001ECB0F
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

		// Token: 0x060050FD RID: 20733 RVA: 0x001EE946 File Offset: 0x001ECB46
		public void Write(BinaryWriter _bw)
		{
			this.SignId.ToStream(_bw);
			_bw.Write((byte)this.BlendMode);
			_bw.Write((byte)this.CanvasRotation);
			_bw.Write(this.ShowOnImposter);
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x001EE979 File Offset: 0x001ECB79
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

		// Token: 0x04003E3D RID: 15933
		public GlobalSignId SignId = SignData.defaultSignDataID;

		// Token: 0x04003E3E RID: 15934
		public SignCanvas.SignBlendMode BlendMode;

		// Token: 0x04003E3F RID: 15935
		public bool ShowOnImposter;

		// Token: 0x04003E40 RID: 15936
		public CanvasRotationMode CanvasRotation;
	}
}
