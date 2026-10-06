using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200134C RID: 4940
public class CameraMatrixOverride : MonoBehaviour
{
	// Token: 0x06009BBE RID: 39870 RVA: 0x003AD354 File Offset: 0x003AB554
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

	// Token: 0x06009BBF RID: 39871 RVA: 0x003AD3A0 File Offset: 0x003AB5A0
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

	// Token: 0x06009BC0 RID: 39872 RVA: 0x003AD3C8 File Offset: 0x003AB5C8
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

	// Token: 0x06009BC1 RID: 39873 RVA: 0x003AD698 File Offset: 0x003AB898
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.LateUpdate)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009BC2 RID: 39874 RVA: 0x003AD6AD File Offset: 0x003AB8AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		if (this.advancedSettings.updateTiming == CameraMatrixOverride.UpdateTiming.OnPreCull)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009BC3 RID: 39875 RVA: 0x003AD6C4 File Offset: 0x003AB8C4
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

	// Token: 0x06009BC4 RID: 39876 RVA: 0x003AD8F0 File Offset: 0x003ABAF0
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

	// Token: 0x06009BC5 RID: 39877 RVA: 0x003AD988 File Offset: 0x003ABB88
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

	// Token: 0x04007552 RID: 30034
	[Tooltip("The overridden FoV to use when rendering any child Renderers in the hierarchy beneath the Camera this script is attached to.")]
	public float fov = 45f;

	// Token: 0x04007553 RID: 30035
	[Range(0.01f, 1f)]
	[Tooltip("The overridden near-clip distance to use when this script is enabled. Note this applies to the Camera as a whole, rather than specifically targeting child Renderers.")]
	public float nearClipOverride = 0.01f;

	// Token: 0x04007554 RID: 30036
	[Range(1E-45f, 8f)]
	[Tooltip("A value of 1 results in normal rendering behaviour. Higher values effectively squash the depth of child Renderers towards the camera; this reduces the likelihood of clipping into environment geometry, but can distort certain screen effects such as reflections. A value of 2 seems to provide a good balance between reducing clipping and minimising distortion of screen effects.")]
	public float nearClipFactor = 2f;

	// Token: 0x04007555 RID: 30037
	[Tooltip("An assortment of parameters left over from earlier prototyping. They remain exposed for debug purposes if ever required; otherwise it is not recommended to change them away from their default values.")]
	public CameraMatrixOverride.AdvancedSettings advancedSettings = new CameraMatrixOverride.AdvancedSettings();

	// Token: 0x04007556 RID: 30038
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x04007557 RID: 30039
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CameraMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CameraMatrixOverride.RendererSettings>();

	// Token: 0x04007558 RID: 30040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x04007559 RID: 30041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public HashSet<Renderer> renderersToRestore = new HashSet<Renderer>();

	// Token: 0x0400755A RID: 30042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float originalNearClip = 0.0751f;

	// Token: 0x0200134D RID: 4941
	public enum ProjectionMode
	{
		// Token: 0x0400755C RID: 30044
		Custom,
		// Token: 0x0400755D RID: 30045
		Reference,
		// Token: 0x0400755E RID: 30046
		ReferenceNonJittered
	}

	// Token: 0x0200134E RID: 4942
	public enum UpdateTiming
	{
		// Token: 0x04007560 RID: 30048
		LateUpdate,
		// Token: 0x04007561 RID: 30049
		OnPreCull,
		// Token: 0x04007562 RID: 30050
		OnPreRender,
		// Token: 0x04007563 RID: 30051
		None
	}

	// Token: 0x0200134F RID: 4943
	[Serializable]
	public class AdvancedSettings
	{
		// Token: 0x04007564 RID: 30052
		public bool enableNearClipOverride = true;

		// Token: 0x04007565 RID: 30053
		public bool enableChildShadows;

		// Token: 0x04007566 RID: 30054
		public bool enableBoundsPadding = true;

		// Token: 0x04007567 RID: 30055
		public CameraMatrixOverride.ProjectionMode projectionMode;

		// Token: 0x04007568 RID: 30056
		public CameraMatrixOverride.UpdateTiming updateTiming = CameraMatrixOverride.UpdateTiming.OnPreCull;

		// Token: 0x04007569 RID: 30057
		[Range(1E-45f, 2f)]
		public float depthScaleFactor = 1f;

		// Token: 0x0400756A RID: 30058
		public float farClipFactor = 1f;

		// Token: 0x0400756B RID: 30059
		public float jitterFactor = 1f;

		// Token: 0x0400756C RID: 30060
		public float boundsPadding = 1f;
	}

	// Token: 0x02001350 RID: 4944
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x0400756D RID: 30061
		public ShadowCastingMode originalShadowCastingMode;

		// Token: 0x0400756E RID: 30062
		public MaterialPropertyBlock originalProperties;

		// Token: 0x0400756F RID: 30063
		public MaterialPropertyBlock overriddenProperties;

		// Token: 0x04007570 RID: 30064
		public bool boundsDirty;

		// Token: 0x04007571 RID: 30065
		public bool shadowModeDirty;
	}
}
