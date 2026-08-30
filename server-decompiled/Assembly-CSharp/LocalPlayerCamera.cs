using System;
using UnityEngine;

// Token: 0x02000613 RID: 1555
[RequireComponent(typeof(Camera))]
public class LocalPlayerCamera : MonoBehaviour
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600329D RID: 12957 RVA: 0x0014915C File Offset: 0x0014735C
	// (remove) Token: 0x0600329E RID: 12958 RVA: 0x00149194 File Offset: 0x00147394
	public event Action<LocalPlayerCamera> PreCull;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600329F RID: 12959 RVA: 0x001491CC File Offset: 0x001473CC
	// (remove) Token: 0x060032A0 RID: 12960 RVA: 0x00149204 File Offset: 0x00147404
	public event Action<LocalPlayerCamera> PreRender;

	// Token: 0x060032A1 RID: 12961 RVA: 0x0014923C File Offset: 0x0014743C
	public static LocalPlayerCamera AddToCamera(Camera camera, LocalPlayerCamera.CameraType camType)
	{
		LocalPlayerCamera localPlayerCamera = camera.gameObject.AddMissingComponent<LocalPlayerCamera>();
		if (camType != LocalPlayerCamera.CameraType.UI)
		{
			localPlayerCamera.Init(camType);
		}
		return localPlayerCamera;
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x00149261 File Offset: 0x00147461
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

	// Token: 0x060032A3 RID: 12963 RVA: 0x0014929E File Offset: 0x0014749E
	public void SetUI(LocalPlayerUI ui)
	{
		this.playerUI = ui;
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x001492A7 File Offset: 0x001474A7
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

	// Token: 0x060032A5 RID: 12965 RVA: 0x001492E5 File Offset: 0x001474E5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x001492F8 File Offset: 0x001474F8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAttachedToLocalPlayer()
	{
		return this.localPlayer != null;
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x00149306 File Offset: 0x00147506
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		this.ModifyCameraProperties();
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x00149310 File Offset: 0x00147510
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

	// Token: 0x060032A9 RID: 12969 RVA: 0x0014936C File Offset: 0x0014756C
	public void SetCameraDepth()
	{
		this.camera.depth = 1.01f + (float)this.playerUI.playerIndex * 0.01f + (float)this.uiChildIndex * 0.001f;
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x001493A0 File Offset: 0x001475A0
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

	// Token: 0x060032AB RID: 12971 RVA: 0x00149421 File Offset: 0x00147621
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		if (this.PreRender != null)
		{
			this.PreRender(this);
		}
	}

	// Token: 0x0400282A RID: 10282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float splitScreenFOVFactors = 1f;

	// Token: 0x0400282B RID: 10283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera camera;

	// Token: 0x0400282C RID: 10284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera.CameraType cameraType;

	// Token: 0x0400282D RID: 10285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x0400282E RID: 10286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal entityPlayerLocal;

	// Token: 0x0400282F RID: 10287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayer localPlayer;

	// Token: 0x04002830 RID: 10288
	public int uiChildIndex;

	// Token: 0x02000614 RID: 1556
	public enum CameraType
	{
		// Token: 0x04002832 RID: 10290
		None,
		// Token: 0x04002833 RID: 10291
		Main,
		// Token: 0x04002834 RID: 10292
		Weapon,
		// Token: 0x04002835 RID: 10293
		UI
	}
}
