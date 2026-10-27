using System;
using UnityEngine;

// Token: 0x0200061C RID: 1564
[RequireComponent(typeof(Camera))]
public class LocalPlayerCamera : MonoBehaviour
{
	// Token: 0x14000039 RID: 57
	// (add) Token: 0x060032EE RID: 13038 RVA: 0x0014A6F4 File Offset: 0x001488F4
	// (remove) Token: 0x060032EF RID: 13039 RVA: 0x0014A72C File Offset: 0x0014892C
	public event Action<LocalPlayerCamera> PreCull;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060032F0 RID: 13040 RVA: 0x0014A764 File Offset: 0x00148964
	// (remove) Token: 0x060032F1 RID: 13041 RVA: 0x0014A79C File Offset: 0x0014899C
	public event Action<LocalPlayerCamera> PreRender;

	// Token: 0x060032F2 RID: 13042 RVA: 0x0014A7D4 File Offset: 0x001489D4
	public static LocalPlayerCamera AddToCamera(Camera camera, LocalPlayerCamera.CameraType camType)
	{
		LocalPlayerCamera localPlayerCamera = camera.gameObject.AddMissingComponent<LocalPlayerCamera>();
		if (camType != LocalPlayerCamera.CameraType.UI)
		{
			localPlayerCamera.Init(camType);
		}
		return localPlayerCamera;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x0014A7F9 File Offset: 0x001489F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(LocalPlayerCamera.CameraType camType)
	{
		this.camera = base.GetComponent<Camera>();
		this.cameraType = camType;
		if (camType != LocalPlayerCamera.CameraType.UI)
		{
			this.camera.allowDynamicResolution = true;
		}
		this.entityPlayerLocal = base.GetComponentInParent<EntityPlayerLocal>();
		this.localPlayer = base.GetComponentInParent<LocalPlayer>();
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0014A836 File Offset: 0x00148A36
	public void SetUI(LocalPlayerUI ui)
	{
		this.playerUI = ui;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x0014A83F File Offset: 0x00148A3F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.playerUI = base.GetComponentInChildren<LocalPlayerUI>();
		if (this.playerUI)
		{
			this.Init(LocalPlayerCamera.CameraType.UI);
			this.playerUI.UpdateChildCameraIndices();
		}
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x0014A87D File Offset: 0x00148A7D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x0014A890 File Offset: 0x00148A90
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAttachedToLocalPlayer()
	{
		return this.localPlayer != null;
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x0014A89E File Offset: 0x00148A9E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		this.ModifyCameraProperties();
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x0014A8A8 File Offset: 0x00148AA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ModifyCameraProperties()
	{
		this.camera.enabled = true;
		if (this.IsAttachedToLocalPlayer())
		{
			this.camera.fieldOfView = (float)Constants.cDefaultCameraFieldOfView * LocalPlayerCamera.splitScreenFOVFactors;
			return;
		}
		UIRect[] componentsInChildren = base.GetComponentsInChildren<UIRect>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].UpdateAnchors();
		}
		this.SetCameraDepth();
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x0014A904 File Offset: 0x00148B04
	public void SetCameraDepth()
	{
		this.camera.depth = 1.01f + (float)this.playerUI.playerIndex * 0.01f + (float)this.uiChildIndex * 0.001f;
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x0014A938 File Offset: 0x00148B38
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		if (this.cameraType == LocalPlayerCamera.CameraType.Main)
		{
			OcclusionManager.Instance.LocalPlayerOnPreCull();
		}
		if (this.PreCull != null)
		{
			this.PreCull(this);
		}
		if (GameRenderManager.dynamicIsEnabled && (this.cameraType == LocalPlayerCamera.CameraType.Main || this.cameraType == LocalPlayerCamera.CameraType.Weapon))
		{
			this.camera.targetTexture = this.entityPlayerLocal.renderManager.GetDynamicRenderTexture();
		}
		if (this.cameraType == LocalPlayerCamera.CameraType.Main)
		{
			this.entityPlayerLocal.renderManager.UpscalingPreCull();
		}
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x0014A9B9 File Offset: 0x00148BB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		if (this.PreRender != null)
		{
			this.PreRender(this);
		}
	}

	// Token: 0x0400283E RID: 10302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float splitScreenFOVFactors = 1f;

	// Token: 0x0400283F RID: 10303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera camera;

	// Token: 0x04002840 RID: 10304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera.CameraType cameraType;

	// Token: 0x04002841 RID: 10305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x04002842 RID: 10306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal entityPlayerLocal;

	// Token: 0x04002843 RID: 10307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayer localPlayer;

	// Token: 0x04002844 RID: 10308
	public int uiChildIndex;

	// Token: 0x0200061D RID: 1565
	public enum CameraType
	{
		// Token: 0x04002846 RID: 10310
		None,
		// Token: 0x04002847 RID: 10311
		Main,
		// Token: 0x04002848 RID: 10312
		Weapon,
		// Token: 0x04002849 RID: 10313
		UI
	}
}
