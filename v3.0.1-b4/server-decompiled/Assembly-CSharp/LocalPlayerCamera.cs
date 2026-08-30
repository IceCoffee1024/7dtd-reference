using System;
using UnityEngine;

// Token: 0x0200060A RID: 1546
[RequireComponent(typeof(Camera))]
public class LocalPlayerCamera : MonoBehaviour
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600324A RID: 12874 RVA: 0x00147038 File Offset: 0x00145238
	// (remove) Token: 0x0600324B RID: 12875 RVA: 0x00147070 File Offset: 0x00145270
	public event Action<LocalPlayerCamera> PreCull;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600324C RID: 12876 RVA: 0x001470A8 File Offset: 0x001452A8
	// (remove) Token: 0x0600324D RID: 12877 RVA: 0x001470E0 File Offset: 0x001452E0
	public event Action<LocalPlayerCamera> PreRender;

	// Token: 0x0600324E RID: 12878 RVA: 0x00147118 File Offset: 0x00145318
	public static LocalPlayerCamera AddToCamera(Camera camera, LocalPlayerCamera.CameraType camType)
	{
		LocalPlayerCamera localPlayerCamera = camera.gameObject.AddMissingComponent<LocalPlayerCamera>();
		if (camType != LocalPlayerCamera.CameraType.UI)
		{
			localPlayerCamera.Init(camType);
		}
		return localPlayerCamera;
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x0014713D File Offset: 0x0014533D
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

	// Token: 0x06003250 RID: 12880 RVA: 0x0014717A File Offset: 0x0014537A
	public void SetUI(LocalPlayerUI ui)
	{
		this.playerUI = ui;
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x00147183 File Offset: 0x00145383
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

	// Token: 0x06003252 RID: 12882 RVA: 0x001471C1 File Offset: 0x001453C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x001471D4 File Offset: 0x001453D4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAttachedToLocalPlayer()
	{
		return this.localPlayer != null;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x001471E2 File Offset: 0x001453E2
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		this.ModifyCameraProperties();
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x001471EC File Offset: 0x001453EC
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

	// Token: 0x06003256 RID: 12886 RVA: 0x00147248 File Offset: 0x00145448
	public void SetCameraDepth()
	{
		this.camera.depth = 1.01f + (float)this.playerUI.playerIndex * 0.01f + (float)this.uiChildIndex * 0.001f;
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x0014727C File Offset: 0x0014547C
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

	// Token: 0x06003258 RID: 12888 RVA: 0x001472FD File Offset: 0x001454FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		if (this.PreRender != null)
		{
			this.PreRender(this);
		}
	}

	// Token: 0x040027B7 RID: 10167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float splitScreenFOVFactors = 1f;

	// Token: 0x040027B8 RID: 10168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera camera;

	// Token: 0x040027B9 RID: 10169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera.CameraType cameraType;

	// Token: 0x040027BA RID: 10170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x040027BB RID: 10171
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal entityPlayerLocal;

	// Token: 0x040027BC RID: 10172
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayer localPlayer;

	// Token: 0x040027BD RID: 10173
	public int uiChildIndex;

	// Token: 0x0200060B RID: 1547
	public enum CameraType
	{
		// Token: 0x040027BF RID: 10175
		None,
		// Token: 0x040027C0 RID: 10176
		Main,
		// Token: 0x040027C1 RID: 10177
		Weapon,
		// Token: 0x040027C2 RID: 10178
		UI
	}
}
