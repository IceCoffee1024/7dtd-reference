using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001317 RID: 4887
public class CharacterMatrixOverride : MonoBehaviour
{
	// Token: 0x06009A22 RID: 39458 RVA: 0x003A3CA8 File Offset: 0x003A1EA8
	public void Init(EntityPlayerLocal epl)
	{
		this.referenceCamera = epl.playerCamera;
	}

	// Token: 0x06009A23 RID: 39459 RVA: 0x003A3CB8 File Offset: 0x003A1EB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009A24 RID: 39460 RVA: 0x003A3D28 File Offset: 0x003A1F28
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009A25 RID: 39461 RVA: 0x003A3D95 File Offset: 0x003A1F95
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCullCallback(Camera camera)
	{
		if (this.Active && camera == this.referenceCamera)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009A26 RID: 39462 RVA: 0x003A3DB4 File Offset: 0x003A1FB4
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

	// Token: 0x06009A27 RID: 39463 RVA: 0x003A3E50 File Offset: 0x003A2050
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

	// Token: 0x06009A28 RID: 39464 RVA: 0x003A3F30 File Offset: 0x003A2130
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

	// Token: 0x0400741F RID: 29727
	public bool Active;

	// Token: 0x04007420 RID: 29728
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x04007421 RID: 29729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x04007422 RID: 29730
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CharacterMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CharacterMatrixOverride.RendererSettings>();

	// Token: 0x02001318 RID: 4888
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x04007423 RID: 29731
		public MaterialPropertyBlock originalProperties;

		// Token: 0x04007424 RID: 29732
		public MaterialPropertyBlock overriddenProperties;
	}
}
