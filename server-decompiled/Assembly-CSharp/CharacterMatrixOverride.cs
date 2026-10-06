using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001351 RID: 4945
public class CharacterMatrixOverride : MonoBehaviour
{
	// Token: 0x06009BC9 RID: 39881 RVA: 0x003ADABC File Offset: 0x003ABCBC
	public void Init(EntityPlayerLocal epl)
	{
		this.referenceCamera = epl.playerCamera;
	}

	// Token: 0x06009BCA RID: 39882 RVA: 0x003ADACC File Offset: 0x003ABCCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009BCB RID: 39883 RVA: 0x003ADB3C File Offset: 0x003ABD3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCullCallback));
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnPostRenderCallback));
	}

	// Token: 0x06009BCC RID: 39884 RVA: 0x003ADBA9 File Offset: 0x003ABDA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCullCallback(Camera camera)
	{
		if (this.Active && camera == this.referenceCamera)
		{
			this.UpdateRendererList();
		}
	}

	// Token: 0x06009BCD RID: 39885 RVA: 0x003ADBC8 File Offset: 0x003ABDC8
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

	// Token: 0x06009BCE RID: 39886 RVA: 0x003ADC64 File Offset: 0x003ABE64
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

	// Token: 0x06009BCF RID: 39887 RVA: 0x003ADD44 File Offset: 0x003ABF44
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

	// Token: 0x04007572 RID: 30066
	public bool Active;

	// Token: 0x04007573 RID: 30067
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera referenceCamera;

	// Token: 0x04007574 RID: 30068
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> overriddenRenderers = new List<Renderer>();

	// Token: 0x04007575 RID: 30069
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Renderer, CharacterMatrixOverride.RendererSettings> rendererSettingsMap = new Dictionary<Renderer, CharacterMatrixOverride.RendererSettings>();

	// Token: 0x02001352 RID: 4946
	[PublicizedFrom(EAccessModifier.Private)]
	public class RendererSettings
	{
		// Token: 0x04007576 RID: 30070
		public MaterialPropertyBlock originalProperties;

		// Token: 0x04007577 RID: 30071
		public MaterialPropertyBlock overriddenProperties;
	}
}
