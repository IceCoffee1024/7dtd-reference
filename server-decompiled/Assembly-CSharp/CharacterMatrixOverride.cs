using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001363 RID: 4963
public class CharacterMatrixOverride : MonoBehaviour
{
	// Token: 0x06009C21 RID: 39969 RVA: 0x003AFEC0 File Offset: 0x003AE0C0
	public void Init(EntityPlayerLocal epl)
	{
		this.referenceCamera = epl.playerCamera;
	}

	// Token: 0x06009C22 RID: 39970 RVA: 0x003AFED0 File Offset: 0x003AE0D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009C23 RID: 39971 RVA: 0x003AFF40 File Offset: 0x003AE140
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009C24 RID: 39972 RVA: 0x003AFFAD File Offset: 0x003AE1AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCullCallback(Camera camera)
	{
		if (this.Active && camera == this.referenceCamera)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009C25 RID: 39973 RVA: 0x003AFFCC File Offset: 0x003AE1CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRendererList()
	{
		this.overriddenRenderers.Clear();
		base.GetComponentsInChildren<Renderer>(this.overriddenRenderers);
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CharacterMatrixOverride.RendererSettings rendererSettings;
			if (!this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				rendererSettings = new CharacterMatrixOverride.RendererSettings();
				rendererSettings.originalProperties = new MaterialPropertyBlock();
				rendererSettings.overriddenProperties = new MaterialPropertyBlock();
				this.rendererSettingsMap[renderer] = rendererSettings;
			}
		}
	}

	// Token: 0x06009C26 RID: 39974 RVA: 0x003B0068 File Offset: 0x003AE268
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRenderCallback(Camera camera)
	{
		if (!this.Active || camera != this.referenceCamera)
		{
			return;
		}
		Matrix4x4 matrix4x = Matrix4x4.Perspective(90f, 1f, 10000f, 10000.1f) * this.referenceCamera.worldToCameraMatrix;
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CharacterMatrixOverride.RendererSettings rendererSettings;
			if (!this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				Log.Error("[CharacterMatrixOverride] Failed to retrieve RendererSettings for overridden renderer");
			}
			else
			{
				renderer.GetPropertyBlock(rendererSettings.originalProperties);
				renderer.GetPropertyBlock(rendererSettings.overriddenProperties);
				MaterialPropertyBlock overriddenProperties = rendererSettings.overriddenProperties;
				overriddenProperties.SetMatrix("unity_MatrixVP", matrix4x);
				renderer.SetPropertyBlock(overriddenProperties);
			}
		}
	}

	// Token: 0x06009C27 RID: 39975 RVA: 0x003B0148 File Offset: 0x003AE348
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPostRenderCallback(Camera camera)
	{
		if (camera != this.referenceCamera)
		{
			return;
		}
		foreach (Renderer renderer in this.overriddenRenderers)
		{
			CharacterMatrixOverride.RendererSettings rendererSettings;
			if (!(renderer == null) && this.rendererSettingsMap.TryGetValue(renderer, out rendererSettings))
			{
				renderer.SetPropertyBlock(rendererSettings.originalProperties);
			}
		}
	}

	// Token: 0x040075B3 RID: 30131
	public bool Active;

	// Token: 0x040075B4 RID: 30132
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x040075B5 RID: 30133
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x040075B6 RID: 30134
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CharacterMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CharacterMatrixOverride.RendererSettings>();

	// Token: 0x02001364 RID: 4964
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x040075B7 RID: 30135
		public MaterialPropertyBlock originalProperties;

		// Token: 0x040075B8 RID: 30136
		public MaterialPropertyBlock overriddenProperties;
	}
}
