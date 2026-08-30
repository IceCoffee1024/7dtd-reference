using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001312 RID: 4882
public class CameraMatrixOverride : MonoBehaviour
{
	// Token: 0x06009A17 RID: 39447 RVA: 0x003A3540 File Offset: 0x003A1740
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

	// Token: 0x06009A18 RID: 39448 RVA: 0x003A358C File Offset: 0x003A178C
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

	// Token: 0x06009A19 RID: 39449 RVA: 0x003A35B4 File Offset: 0x003A17B4
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

	// Token: 0x06009A1A RID: 39450 RVA: 0x003A3884 File Offset: 0x003A1A84
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.LateUpdate)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009A1B RID: 39451 RVA: 0x003A3899 File Offset: 0x003A1A99
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.OnPreCull)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009A1C RID: 39452 RVA: 0x003A38B0 File Offset: 0x003A1AB0
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

	// Token: 0x06009A1D RID: 39453 RVA: 0x003A3ADC File Offset: 0x003A1CDC
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

	// Token: 0x06009A1E RID: 39454 RVA: 0x003A3B74 File Offset: 0x003A1D74
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

	// Token: 0x040073FF RID: 29695
	[Tooltip("The overridden FoV to use when rendering any child Renderers in the hierarchy beneath the Camera this script is attached to.")]
	public float fov = 45f;

	// Token: 0x04007400 RID: 29696
	[Range(0.01f, 1f)]
	[Tooltip("The overridden near-clip distance to use when this script is enabled. Note this applies to the Camera as a whole, rather than specifically targeting child Renderers.")]
	public float nearClipOverride = 0.01f;

	// Token: 0x04007401 RID: 29697
	[Range(1E-45f, 8f)]
	[Tooltip("A value of 1 results in normal rendering behaviour. Higher values effectively squash the depth of child Renderers towards the camera; this reduces the likelihood of clipping into environment geometry, but can distort certain screen effects such as reflections. A value of 2 seems to provide a good balance between reducing clipping and minimising distortion of screen effects.")]
	public float nearClipFactor = 2f;

	// Token: 0x04007402 RID: 29698
	[Tooltip("An assortment of parameters left over from earlier prototyping. They remain exposed for debug purposes if ever required; otherwise it is not recommended to change them away from their default values.")]
	public CameraMatrixOverride.AdvancedSettings advancedSettings = new CameraMatrixOverride.AdvancedSettings();

	// Token: 0x04007403 RID: 29699
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x04007404 RID: 29700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CameraMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CameraMatrixOverride.RendererSettings>();

	// Token: 0x04007405 RID: 29701
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x04007406 RID: 29702
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public HashSet<Renderer> renderersToRestore = new HashSet<Renderer>();

	// Token: 0x04007407 RID: 29703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float originalNearClip = 0.0751f;

	// Token: 0x02001313 RID: 4883
	public enum ProjectionMode
	{
		// Token: 0x04007409 RID: 29705
		Custom,
		// Token: 0x0400740A RID: 29706
		Reference,
		// Token: 0x0400740B RID: 29707
		ReferenceNonJittered
	}

	// Token: 0x02001314 RID: 4884
	public enum UpdateTiming
	{
		// Token: 0x0400740D RID: 29709
		LateUpdate,
		// Token: 0x0400740E RID: 29710
		OnPreCull,
		// Token: 0x0400740F RID: 29711
		OnPreRender,
		// Token: 0x04007410 RID: 29712
		None
	}

	// Token: 0x02001315 RID: 4885
	[Serializable]
	public class AdvancedSettings
	{
		// Token: 0x04007411 RID: 29713
		public bool enableNearClipOverride = true;

		// Token: 0x04007412 RID: 29714
		public bool enableChildShadows;

		// Token: 0x04007413 RID: 29715
		public bool enableBoundsPadding = true;

		// Token: 0x04007414 RID: 29716
		public CameraMatrixOverride.ProjectionMode projectionMode;

		// Token: 0x04007415 RID: 29717
		public CameraMatrixOverride.UpdateTiming updateTiming = CameraMatrixOverride.UpdateTiming.OnPreCull;

		// Token: 0x04007416 RID: 29718
		[Range(1E-45f, 2f)]
		public float depthScaleFactor = 1f;

		// Token: 0x04007417 RID: 29719
		public float farClipFactor = 1f;

		// Token: 0x04007418 RID: 29720
		public float jitterFactor = 1f;

		// Token: 0x04007419 RID: 29721
		public float boundsPadding = 1f;
	}

	// Token: 0x02001316 RID: 4886
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x0400741A RID: 29722
		public ShadowCastingMode originalShadowCastingMode;

		// Token: 0x0400741B RID: 29723
		public MaterialPropertyBlock originalProperties;

		// Token: 0x0400741C RID: 29724
		public MaterialPropertyBlock overriddenProperties;

		// Token: 0x0400741D RID: 29725
		public bool boundsDirty;

		// Token: 0x0400741E RID: 29726
		public bool shadowModeDirty;
	}
}
