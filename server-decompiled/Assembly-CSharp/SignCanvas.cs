using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public class SignCanvas : MonoBehaviour, ISignRenderingDataUpdateListener
{
	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x060050D2 RID: 20690 RVA: 0x001F0323 File Offset: 0x001EE523
	// (set) Token: 0x060050D3 RID: 20691 RVA: 0x001F032B File Offset: 0x001EE52B
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

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x060050D4 RID: 20692 RVA: 0x001F034B File Offset: 0x001EE54B
	public bool AllowedOnImposter
	{
		get
		{
			return this.allowedOnImposter;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060050D5 RID: 20693 RVA: 0x001F0353 File Offset: 0x001EE553
	// (set) Token: 0x060050D6 RID: 20694 RVA: 0x001F0384 File Offset: 0x001EE584
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

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060050D7 RID: 20695 RVA: 0x001F0397 File Offset: 0x001EE597
	public Texture BakedTexture
	{
		get
		{
			return this.bakedTexture;
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060050D8 RID: 20696 RVA: 0x001F039F File Offset: 0x001EE59F
	public float SizeSquared
	{
		get
		{
			return this._sizeSquared;
		}
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060050D9 RID: 20697 RVA: 0x001F03A7 File Offset: 0x001EE5A7
	// (set) Token: 0x060050DA RID: 20698 RVA: 0x001F03B4 File Offset: 0x001EE5B4
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

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060050DB RID: 20699 RVA: 0x001F03EC File Offset: 0x001EE5EC
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

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x060050DC RID: 20700 RVA: 0x001F04B5 File Offset: 0x001EE6B5
	// (set) Token: 0x060050DD RID: 20701 RVA: 0x001F04C2 File Offset: 0x001EE6C2
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

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x060050DE RID: 20702 RVA: 0x001F04E5 File Offset: 0x001EE6E5
	// (set) Token: 0x060050DF RID: 20703 RVA: 0x001F04F2 File Offset: 0x001EE6F2
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

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x060050E0 RID: 20704 RVA: 0x001F0500 File Offset: 0x001EE700
	// (set) Token: 0x060050E1 RID: 20705 RVA: 0x001F050D File Offset: 0x001EE70D
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

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x060050E2 RID: 20706 RVA: 0x001F0530 File Offset: 0x001EE730
	public float CanvasRotationRadians
	{
		get
		{
			return (float)this.CanvasRotation * 3.1415927f * 0.5f;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x060050E3 RID: 20707 RVA: 0x001F0548 File Offset: 0x001EE748
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

	// Token: 0x060050E4 RID: 20708 RVA: 0x001F05A4 File Offset: 0x001EE7A4
	public void Initialize(SignDataManager.RenderingDataPatcher _alternativePatcher = null)
	{
		this._patchRenderingData = _alternativePatcher ?? new SignDataManager.RenderingDataPatcher(this.PatchRenderingData);
		base.GetComponentsInChildren<SignRenderer>(true, this.SignRenderers);
		this.CacheBoundsMetrics();
	}

	// Token: 0x060050E5 RID: 20709 RVA: 0x001F05D0 File Offset: 0x001EE7D0
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

	// Token: 0x060050E6 RID: 20710 RVA: 0x001F06A0 File Offset: 0x001EE8A0
	public void SetTexture(Texture rt)
	{
		if (this.bakedTexture == rt)
		{
			return;
		}
		this.bakedTexture = rt;
		this.RefreshRenderers();
	}

	// Token: 0x060050E7 RID: 20711 RVA: 0x001F06BE File Offset: 0x001EE8BE
	public Vector3 GetWorldPosition()
	{
		return base.transform.TransformPoint(this._localCenter) + Origin.position;
	}

	// Token: 0x060050E8 RID: 20712 RVA: 0x001F06DC File Offset: 0x001EE8DC
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

	// Token: 0x060050E9 RID: 20713 RVA: 0x001F0760 File Offset: 0x001EE960
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

	// Token: 0x060050EA RID: 20714 RVA: 0x001F07C7 File Offset: 0x001EE9C7
	public void HandleRenderingDataUpdate()
	{
		this.RefreshRenderers();
	}

	// Token: 0x060050EB RID: 20715 RVA: 0x001F07CF File Offset: 0x001EE9CF
	public void HandleSignDataRemoved()
	{
		this._signDataAvailabilityDirty = true;
		this.SetTexture(null);
		this.UpdateRegistrations();
		this.RefreshRenderers();
	}

	// Token: 0x060050EC RID: 20716 RVA: 0x001F07EB File Offset: 0x001EE9EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRegistrations()
	{
		SignTextureManager.Instance.Register(this);
		SignDataManager.Instance.RegisterListener(this.DisplaySignId, this, true);
	}

	// Token: 0x060050ED RID: 20717 RVA: 0x001F080A File Offset: 0x001EEA0A
	public void Cleanup()
	{
		SignDataManager.Instance.DeregisterListener(this);
		SignTextureManager.Instance.Deregister(this);
		this.bakedTexture = null;
	}

	// Token: 0x060050EE RID: 20718 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("DEBUG_LOG_SIGN_CANVAS")]
	public static void DebugLog(string message)
	{
		Log.Out(message);
	}

	// Token: 0x04003E54 RID: 15956
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.CanvasState _state = new SignCanvas.CanvasState();

	// Token: 0x04003E55 RID: 15957
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowedOnImposter = true;

	// Token: 0x04003E56 RID: 15958
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 canvasDimensions = Vector2.one;

	// Token: 0x04003E57 RID: 15959
	public List<SignRenderer> SignRenderers = new List<SignRenderer>();

	// Token: 0x04003E58 RID: 15960
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture bakedTexture;

	// Token: 0x04003E59 RID: 15961
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignDataManager.RenderingDataPatcher _patchRenderingData;

	// Token: 0x04003E5A RID: 15962
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailabilityDirty = true;

	// Token: 0x04003E5B RID: 15963
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataAvailable = true;

	// Token: 0x04003E5C RID: 15964
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _signDataDeleted;

	// Token: 0x04003E5D RID: 15965
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _localCenter = Vector3.zero;

	// Token: 0x04003E5E RID: 15966
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _sizeSquared = 1f;

	// Token: 0x02000A7D RID: 2685
	public enum SignBlendMode
	{
		// Token: 0x04003E60 RID: 15968
		Cutout,
		// Token: 0x04003E61 RID: 15969
		AlphaBlend
	}

	// Token: 0x02000A7E RID: 2686
	public class CanvasState
	{
		// Token: 0x060050F0 RID: 20720 RVA: 0x001F088B File Offset: 0x001EEA8B
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

		// Token: 0x060050F1 RID: 20721 RVA: 0x001F08C2 File Offset: 0x001EEAC2
		public void Write(PooledBinaryWriter _bw)
		{
			this.SignId.ToStream(_bw);
			_bw.Write((byte)this.BlendMode);
			_bw.Write((byte)this.CanvasRotation);
			_bw.Write(this.ShowOnImposter);
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x001F08F5 File Offset: 0x001EEAF5
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

		// Token: 0x04003E62 RID: 15970
		public GlobalSignId SignId = SignData.defaultSignDataID;

		// Token: 0x04003E63 RID: 15971
		public SignCanvas.SignBlendMode BlendMode;

		// Token: 0x04003E64 RID: 15972
		public bool ShowOnImposter;

		// Token: 0x04003E65 RID: 15973
		public CanvasRotationMode CanvasRotation;
	}
}
