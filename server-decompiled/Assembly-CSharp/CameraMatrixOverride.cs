using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200135E RID: 4958
public class CameraMatrixOverride : MonoBehaviour
{
	// Token: 0x06009C16 RID: 39958 RVA: 0x003AF758 File Offset: 0x003AD958
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.referenceCamera == null && !base.TryGetComponent<Camera>(out this.referenceCamera))
		{
			Debug.LogError("Failed to get Camera. The CameraMatrixOverride script must be attached to a GameObject with a Camera component.");
			base.enabled = false;
			return;
		}
		this.originalNearClip = this.referenceCamera.nearClipPlane;
	}

	// Token: 0x06009C17 RID: 39959 RVA: 0x003AF7A4 File Offset: 0x003AD9A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.referenceCamera == null)
		{
			return;
		}
		this.referenceCamera.nearClipPlane = this.originalNearClip;
		this.RestoreChildSettings();
	}

	// Token: 0x06009C18 RID: 39960 RVA: 0x003AF7CC File Offset: 0x003AD9CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRendererList()
	{
		bool flag = this.advancedSettings.enableBoundsPadding && this.referenceCamera.fieldOfView < this.fov;
		this.renderersToRestore.Clear();
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			if (renderer != null)
			{
				this.renderersToRestore.Add(renderer);
			}
		}
		this.overriddenRenderers.Clear();
		base.GetComponentsInChildren<Renderer>(this.overriddenRenderers);
		Vector3 vector = new Vector3(this.advancedSettings.boundsPadding, this.advancedSettings.boundsPadding, this.advancedSettings.boundsPadding);
		foreach (Renderer renderer2 in this.overriddenRenderers)
		{
			this.renderersToRestore.Remove(renderer2);
			CameraMatrixOverride.RendererSettings rendererSettings;
			if (!this.rendererSettingsMap.TryGetValue(renderer2, out rendererSettings))
			{
				rendererSettings = new CameraMatrixOverride.RendererSettings();
				rendererSettings.originalShadowCastingMode = renderer2.shadowCastingMode;
				rendererSettings.originalProperties = new MaterialPropertyBlock();
				rendererSettings.overriddenProperties = new MaterialPropertyBlock();
				this.rendererSettingsMap[renderer2] = rendererSettings;
			}
			if (this.advancedSettings.enableChildShadows && rendererSettings.shadowModeDirty)
			{
				renderer2.shadowCastingMode = rendererSettings.originalShadowCastingMode;
				rendererSettings.shadowModeDirty = false;
			}
			else if (!this.advancedSettings.enableChildShadows && !rendererSettings.shadowModeDirty)
			{
				renderer2.shadowCastingMode = ShadowCastingMode.Off;
				rendererSettings.shadowModeDirty = true;
			}
			if (flag && !(renderer2 is ParticleSystemRenderer))
			{
				renderer2.ResetBounds();
				Bounds bounds = renderer2.bounds;
				bounds.extents += vector;
				renderer2.bounds = bounds;
				rendererSettings.boundsDirty = true;
			}
			else if (rendererSettings.boundsDirty)
			{
				renderer2.ResetBounds();
				rendererSettings.boundsDirty = false;
			}
		}
		foreach (Renderer renderer3 in this.renderersToRestore)
		{
			CameraMatrixOverride.RendererSettings rendererSettings2;
			if (this.rendererSettingsMap.TryGetValue(renderer3, out rendererSettings2))
			{
				if (rendererSettings2.shadowModeDirty)
				{
					renderer3.shadowCastingMode = rendererSettings2.originalShadowCastingMode;
					rendererSettings2.shadowModeDirty = false;
				}
				if (rendererSettings2.boundsDirty)
				{
					renderer3.ResetBounds();
					rendererSettings2.boundsDirty = false;
				}
			}
		}
		this.renderersToRestore.Clear();
	}

	// Token: 0x06009C19 RID: 39961 RVA: 0x003AFA9C File Offset: 0x003ADC9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.LateUpdate)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009C1A RID: 39962 RVA: 0x003AFAB1 File Offset: 0x003ADCB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.OnPreCull)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009C1B RID: 39963 RVA: 0x003AFAC8 File Offset: 0x003ADCC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.OnPreRender)
		{
			this.UpdateRendererList();
		}
		this.referenceCamera.nearClipPlane = (this.advancedSettings.enableNearClipOverride ? this.nearClipOverride : this.originalNearClip);
		Matrix4x4 projectionMatrix = this.referenceCamera.projectionMatrix;
		Matrix4x4 matrix4x = Matrix4x4.Perspective(this.fov, this.referenceCamera.aspect, this.referenceCamera.nearClipPlane * this.nearClipFactor, this.referenceCamera.farClipPlane * this.advancedSettings.farClipFactor);
		ref Matrix4x4 ptr = ref matrix4x;
		ptr[0, 2] = ptr[0, 2] + this.advancedSettings.jitterFactor * (projectionMatrix[0, 2] - matrix4x[0, 2]);
		ptr = ref matrix4x;
		ptr[1, 2] = ptr[1, 2] + this.advancedSettings.jitterFactor * (projectionMatrix[1, 2] - matrix4x[1, 2]);
		Matrix4x4 matrix4x2;
		switch (this.advancedSettings.projectionMode)
		{
		case CameraMatrixOverride.ProjectionMode.Custom:
			matrix4x2 = matrix4x;
			break;
		case CameraMatrixOverride.ProjectionMode.Reference:
			matrix4x2 = projectionMatrix;
			break;
		case CameraMatrixOverride.ProjectionMode.ReferenceNonJittered:
			matrix4x2 = this.referenceCamera.nonJitteredProjectionMatrix;
			break;
		default:
			matrix4x2 = projectionMatrix;
			break;
		}
		Matrix4x4 matrix4x3 = matrix4x2;
		if (this.advancedSettings.depthScaleFactor != 1f)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity.m22 = this.advancedSettings.depthScaleFactor;
			matrix4x3 = identity * matrix4x3;
		}
		matrix4x3 = GL.GetGPUProjectionMatrix(matrix4x3, true);
		Matrix4x4 worldToCameraMatrix = this.referenceCamera.worldToCameraMatrix;
		Matrix4x4 matrix4x4 = matrix4x3 * worldToCameraMatrix;
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CameraMatrixOverride.RendererSettings rendererSettings;
			if (!this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				Debug.LogError("[CMO] Failed to retrieve RendererSettings for overridden renderer");
			}
			else
			{
				renderer.GetPropertyBlock(rendererSettings.originalProperties);
				renderer.GetPropertyBlock(rendererSettings.overriddenProperties);
				MaterialPropertyBlock overriddenProperties = rendererSettings.overriddenProperties;
				overriddenProperties.SetMatrix("unity_MatrixVP", matrix4x4);
				renderer.SetPropertyBlock(overriddenProperties);
			}
		}
	}

	// Token: 0x06009C1C RID: 39964 RVA: 0x003AFCF4 File Offset: 0x003ADEF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RestoreChildSettings()
	{
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CameraMatrixOverride.RendererSettings rendererSettings;
			if (renderer != null && this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				if (rendererSettings.shadowModeDirty)
				{
					renderer.shadowCastingMode = rendererSettings.originalShadowCastingMode;
					rendererSettings.shadowModeDirty = false;
				}
				if (rendererSettings.boundsDirty)
				{
					renderer.ResetBounds();
					rendererSettings.boundsDirty = false;
				}
			}
		}
	}

	// Token: 0x06009C1D RID: 39965 RVA: 0x003AFD8C File Offset: 0x003ADF8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPostRender()
	{
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CameraMatrixOverride.RendererSettings rendererSettings;
			if (!(renderer == null) && this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				renderer.SetPropertyBlock(rendererSettings.originalProperties);
			}
		}
	}

	// Token: 0x04007593 RID: 30099
	[Tooltip("The overridden FoV to use when rendering any child Renderers in the hierarchy beneath the Camera this script is attached to.")]
	public float fov = 45f;

	// Token: 0x04007594 RID: 30100
	[Range(0.01f, 1f)]
	[Tooltip("The overridden near-clip distance to use when this script is enabled. Note this applies to the Camera as a whole, rather than specifically targeting child Renderers.")]
	public float nearClipOverride = 0.01f;

	// Token: 0x04007595 RID: 30101
	[Range(1E-45f, 8f)]
	[Tooltip("A value of 1 results in normal rendering behaviour. Higher values effectively squash the depth of child Renderers towards the camera; this reduces the likelihood of clipping into environment geometry, but can distort certain screen effects such as reflections. A value of 2 seems to provide a good balance between reducing clipping and minimising distortion of screen effects.")]
	public float nearClipFactor = 2f;

	// Token: 0x04007596 RID: 30102
	[Tooltip("An assortment of parameters left over from earlier prototyping. They remain exposed for debug purposes if ever required; otherwise it is not recommended to change them away from their default values.")]
	public CameraMatrixOverride.AdvancedSettings advancedSettings = new CameraMatrixOverride.AdvancedSettings();

	// Token: 0x04007597 RID: 30103
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x04007598 RID: 30104
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CameraMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CameraMatrixOverride.RendererSettings>();

	// Token: 0x04007599 RID: 30105
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x0400759A RID: 30106
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public HashSet<Renderer> renderersToRestore = new HashSet<Renderer>();

	// Token: 0x0400759B RID: 30107
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float originalNearClip = 0.0751f;

	// Token: 0x0200135F RID: 4959
	public enum ProjectionMode
	{
		// Token: 0x0400759D RID: 30109
		Custom,
		// Token: 0x0400759E RID: 30110
		Reference,
		// Token: 0x0400759F RID: 30111
		ReferenceNonJittered
	}

	// Token: 0x02001360 RID: 4960
	public enum UpdateTiming
	{
		// Token: 0x040075A1 RID: 30113
		LateUpdate,
		// Token: 0x040075A2 RID: 30114
		OnPreCull,
		// Token: 0x040075A3 RID: 30115
		OnPreRender,
		// Token: 0x040075A4 RID: 30116
		None
	}

	// Token: 0x02001361 RID: 4961
	[Serializable]
	public class AdvancedSettings
	{
		// Token: 0x040075A5 RID: 30117
		public bool enableNearClipOverride = true;

		// Token: 0x040075A6 RID: 30118
		public bool enableChildShadows;

		// Token: 0x040075A7 RID: 30119
		public bool enableBoundsPadding = true;

		// Token: 0x040075A8 RID: 30120
		public CameraMatrixOverride.ProjectionMode projectionMode;

		// Token: 0x040075A9 RID: 30121
		public CameraMatrixOverride.UpdateTiming updateTiming = CameraMatrixOverride.UpdateTiming.OnPreCull;

		// Token: 0x040075AA RID: 30122
		[Range(1E-45f, 2f)]
		public float depthScaleFactor = 1f;

		// Token: 0x040075AB RID: 30123
		public float farClipFactor = 1f;

		// Token: 0x040075AC RID: 30124
		public float jitterFactor = 1f;

		// Token: 0x040075AD RID: 30125
		public float boundsPadding = 1f;
	}

	// Token: 0x02001362 RID: 4962
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x040075AE RID: 30126
		public ShadowCastingMode originalShadowCastingMode;

		// Token: 0x040075AF RID: 30127
		public MaterialPropertyBlock originalProperties;

		// Token: 0x040075B0 RID: 30128
		public MaterialPropertyBlock overriddenProperties;

		// Token: 0x040075B1 RID: 30129
		public bool boundsDirty;

		// Token: 0x040075B2 RID: 30130
		public bool shadowModeDirty;
	}
}
