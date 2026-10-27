using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A86 RID: 2694
public class SignCanvas : MonoBehaviour, ISignRenderingDataUpdateListener
{
	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060050F6 RID: 20726 RVA: 0x001F13FF File Offset: 0x001EF5FF
	// (set) Token: 0x060050F7 RID: 20727 RVA: 0x001F1407 File Offset: 0x001EF607
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
			this.UpdateRegistrations();
			this.RefreshRenderers();
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x060050F8 RID: 20728 RVA: 0x001F1427 File Offset: 0x001EF627
	public bool AllowedOnImposter
	{
		get
		{
			return this.allowedOnImposter;
		}
	}

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x060050F9 RID: 20729 RVA: 0x001F142F File Offset: 0x001EF62F
	// (set) Token: 0x060050FA RID: 20730 RVA: 0x001F1460 File Offset: 0x001EF660
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

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x060050FB RID: 20731 RVA: 0x001F1473 File Offset: 0x001EF673
	public Texture BakedTexture
	{
		get
		{
			return this.bakedTexture;
		}
	}

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x060050FC RID: 20732 RVA: 0x001F147B File Offset: 0x001EF67B
	public float SizeSquared
	{
		get
		{
			return this._sizeSquared;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x060050FD RID: 20733 RVA: 0x001F1483 File Offset: 0x001EF683
	// (set) Token: 0x060050FE RID: 20734 RVA: 0x001F1490 File Offset: 0x001EF690
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
			this.UpdateRegistrations();
			this.RefreshRenderers();
		}
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x060050FF RID: 20735 RVA: 0x001F14C8 File Offset: 0x001EF6C8
	public GlobalSignId DisplaySignId
	{
		get
		{
			if (this._signDataAvailabilityDirty)
			{
				SignData signData;
				this._signDataAvailable = SignDataManager.Instance.TryGetSignData(this.State.SignId, out signData);
				this._signDataDeleted = !this._signDataAvailable && SignDataManager.Instance.IsKnownDeleted(this.State.SignId);
				if (!this._signDataAvailable && !this._signDataDeleted)
				{
					Log.Error(string.Format("Failed to retrieve sign data for global id: '{0}'. World pos: {1}.", this.State.SignId.ToString(), this.GetWorldPosition()));
				}
				this._signDataAvailabilityDirty = false;
			}
			if (this._signDataAvailable)
			{
				return this.State.SignId;
			}
			if (!this._signDataDeleted)
			{
				return SignDataManager.ErrorSignId;
			}
			return SignData.defaultSignDataID;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06005100 RID: 20736 RVA: 0x001F1591 File Offset: 0x001EF791
	// (set) Token: 0x06005101 RID: 20737 RVA: 0x001F159E File Offset: 0x001EF79E
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

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06005102 RID: 20738 RVA: 0x001F15C1 File Offset: 0x001EF7C1
	// (set) Token: 0x06005103 RID: 20739 RVA: 0x001F15CE File Offset: 0x001EF7CE
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

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06005104 RID: 20740 RVA: 0x001F15DC File Offset: 0x001EF7DC
	// (set) Token: 0x06005105 RID: 20741 RVA: 0x001F15E9 File Offset: 0x001EF7E9
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

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06005106 RID: 20742 RVA: 0x001F160C File Offset: 0x001EF80C
	public float CanvasRotationRadians
	{
		get
		{
			return (float)this.CanvasRotation * 3.1415927f * 0.5f;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06005107 RID: 20743 RVA: 0x001F1624 File Offset: 0x001EF824
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

	// Token: 0x06005108 RID: 20744 RVA: 0x001F1680 File Offset: 0x001EF880
	public void Initialize(SignDataManager.RenderingDataPatcher _alternativePatcher = null)
	{
		this._patchRenderingData = _alternativePatcher ?? new SignDataManager.RenderingDataPatcher(this.PatchRenderingData);
		base.GetComponentsInChildren<SignRenderer>(true, this.SignRenderers);
		this.CacheBoundsMetrics();
	}

	// Token: 0x06005109 RID: 20745 RVA: 0x001F16AC File Offset: 0x001EF8AC
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

	// Token: 0x0600510A RID: 20746 RVA: 0x001F177C File Offset: 0x001EF97C
	public void SetTexture(Texture rt)
	{
		if (this.bakedTexture == rt)
		{
			return;
		}
		this.bakedTexture = rt;
		this.RefreshRenderers();
	}

	// Token: 0x0600510B RID: 20747 RVA: 0x001F179A File Offset: 0x001EF99A
	public Vector3 GetWorldPosition()
	{
		return base.transform.TransformPoint(this._localCenter) + Origin.position;
	}

	// Token: 0x0600510C RID: 20748 RVA: 0x001F17B8 File Offset: 0x001EF9B8
	public void RefreshRenderers()
	{
		if (this == null || this.SignRenderers == null || this.SignRenderers.Count == 0)
		{
			return;
		}
		if (!SignDataManager.Instance.TryApplyRenderingData(this.DisplaySignId, this._patchRenderingData, this.SignRenderers, this.State.BlendMode, null))
		{
			Log.Error(string.Format("Failed to apply rendering data for sign with display id: '{0}'. World pos: {1}.", this.DisplaySignId.ToString(), this.GetWorldPosition()));
		}
	}

	// Token: 0x0600510D RID: 20749 RVA: 0x001F183C File Offset: 0x001EFA3C
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

	// Token: 0x0600510E RID: 20750 RVA: 0x001F18A3 File Offset: 0x001EFAA3
	public void HandleRenderingDataUpdate()
	{
		this.RefreshRenderers();
	}

	// Token: 0x0600510F RID: 20751 RVA: 0x001F18AB File Offset: 0x001EFAAB
	public void HandleSignDataRemoved()
	{
		this._signDataAvailabilityDirty = true;
		this.SetTexture(null);
		this.UpdateRegistrations();
		this.RefreshRenderers();
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x001F18C7 File Offset: 0x001EFAC7
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRegistrations()
	{
		SignTextureManager.Instance.Register(this);
		SignDataManager.Instance.RegisterListener(this.DisplaySignId, this, true);
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001F18E6 File Offset: 0x001EFAE6
	public void Cleanup()
	{
		SignDataManager.Instance.DeregisterListener(this);
		SignTextureManager.Instance.Deregister(this);
		this.bakedTexture = null;
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("DEBUG_LOG_SIGN_CANVAS")]
	public static void DebugLog(string message)
	{
		Log.Out(message);
	}

	// Token: 0x04003E75 RID: 15989
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.CanvasState _state = new SignCanvas.CanvasState();

	// Token: 0x04003E76 RID: 15990
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowedOnImposter = true;

	// Token: 0x04003E77 RID: 15991
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 canvasDimensions = Vector2.one;

	// Token: 0x04003E78 RID: 15992
	public List<SignRenderer> SignRenderers = new List<SignRenderer>();

	// Token: 0x04003E79 RID: 15993
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture bakedTexture;

	// Token: 0x04003E7A RID: 15994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignDataManager.RenderingDataPatcher _patchRenderingData;

	// Token: 0x04003E7B RID: 15995
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailabilityDirty = true;

	// Token: 0x04003E7C RID: 15996
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailable = true;

	// Token: 0x04003E7D RID: 15997
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataDeleted;

	// Token: 0x04003E7E RID: 15998
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _localCenter = Vector3.zero;

	// Token: 0x04003E7F RID: 15999
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _sizeSquared = 1f;

	// Token: 0x02000A87 RID: 2695
	public enum SignBlendMode
	{
		// Token: 0x04003E81 RID: 16001
		Cutout,
		// Token: 0x04003E82 RID: 16002
		AlphaBlend
	}

	// Token: 0x02000A88 RID: 2696
	public class CanvasState
	{
		// Token: 0x06005114 RID: 20756 RVA: 0x001F1967 File Offset: 0x001EFB67
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

		// Token: 0x06005115 RID: 20757 RVA: 0x001F199E File Offset: 0x001EFB9E
		public void Write(PooledBinaryWriter _bw)
		{
			this.SignId.ToStream(_bw);
			_bw.Write((byte)this.BlendMode);
			_bw.Write((byte)this.CanvasRotation);
			_bw.Write(this.ShowOnImposter);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001F19D1 File Offset: 0x001EFBD1
		public static SignCanvas.CanvasState Read(PooledBinaryReader _br)
		{
			return new SignCanvas.CanvasState
			{
				SignId = GlobalSignId.FromStream(_br),
				BlendMode = (SignCanvas.SignBlendMode)_br.ReadByte(),
				CanvasRotation = (CanvasRotationMode)_br.ReadByte(),
				ShowOnImposter = _br.ReadBoolean()
			};
		}

		// Token: 0x04003E83 RID: 16003
		public GlobalSignId SignId = SignData.defaultSignDataID;

		// Token: 0x04003E84 RID: 16004
		public SignCanvas.SignBlendMode BlendMode;

		// Token: 0x04003E85 RID: 16005
		public bool ShowOnImposter;

		// Token: 0x04003E86 RID: 16006
		public CanvasRotationMode CanvasRotation;
	}
}
