using System;
using UnityEngine;

// Token: 0x02000615 RID: 1557
[RequireComponent(typeof(Camera))]
public class LocalPlayerCamera : MonoBehaviour
{
	// Token: 0x14000039 RID: 57
	// (add) Token: 0x060032E0 RID: 13024 RVA: 0x00149B00 File Offset: 0x00147D00
	// (remove) Token: 0x060032E1 RID: 13025 RVA: 0x00149B38 File Offset: 0x00147D38
	public event Action<LocalPlayerCamera> PreCull;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060032E2 RID: 13026 RVA: 0x00149B70 File Offset: 0x00147D70
	// (remove) Token: 0x060032E3 RID: 13027 RVA: 0x00149BA8 File Offset: 0x00147DA8
	public event Action<LocalPlayerCamera> PreRender;

	// Token: 0x060032E4 RID: 13028 RVA: 0x00149BE0 File Offset: 0x00147DE0
	public static LocalPlayerCamera AddToCamera(Camera camera, LocalPlayerCamera.CameraType camType)
	{
		LocalPlayerCamera localPlayerCamera = camera.gameObject.AddMissingComponent<LocalPlayerCamera>();
		if (camType != LocalPlayerCamera.CameraType.UI)
		{
			localPlayerCamera.Init(camType);
		}
		return localPlayerCamera;
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00149C05 File Offset: 0x00147E05
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

	// Token: 0x060032E6 RID: 13030 RVA: 0x00149C42 File Offset: 0x00147E42
	public void SetUI(LocalPlayerUI ui)
	{
		this.playerUI = ui;
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x00149C4B File Offset: 0x00147E4B
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

	// Token: 0x060032E8 RID: 13032 RVA: 0x00149C89 File Offset: 0x00147E89
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x00149C9C File Offset: 0x00147E9C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAttachedToLocalPlayer()
	{
		return this.localPlayer != null;
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x00149CAA File Offset: 0x00147EAA
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		this.ModifyCameraProperties();
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x00149CB4 File Offset: 0x00147EB4
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

	// Token: 0x060032EC RID: 13036 RVA: 0x00149D10 File Offset: 0x00147F10
	public void SetCameraDepth()
	{
		this.camera.depth = 1.01f + (float)this.playerUI.playerIndex * 0.01f + (float)this.uiChildIndex * 0.001f;
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x00149D44 File Offset: 0x00147F44
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

	// Token: 0x060032EE RID: 13038 RVA: 0x00149DC5 File Offset: 0x00147FC5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		if (this.PreRender != null)
		{
			this.PreRender(this);
		}
	}

	// Token: 0x04002823 RID: 10275
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float splitScreenFOVFactors = 1f;

	// Token: 0x04002824 RID: 10276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera camera;

	// Token: 0x04002825 RID: 10277
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera.CameraType cameraType;

	// Token: 0x04002826 RID: 10278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x04002827 RID: 10279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal entityPlayerLocal;

	// Token: 0x04002828 RID: 10280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayer localPlayer;

	// Token: 0x04002829 RID: 10281
	public int uiChildIndex;

	// Token: 0x02000616 RID: 1558
	public enum CameraType
	{
		// Token: 0x0400282B RID: 10283
		None,
		// Token: 0x0400282C RID: 10284
		Main,
		// Token: 0x0400282D RID: 10285
		Weapon,
		// Token: 0x0400282E RID: 10286
		UI
	}
}
