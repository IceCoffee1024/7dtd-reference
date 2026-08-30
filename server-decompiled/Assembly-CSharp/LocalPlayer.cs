using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class LocalPlayer : MonoBehaviour
{
	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x0600328A RID: 12938 RVA: 0x00148F06 File Offset: 0x00147106
	// (set) Token: 0x0600328B RID: 12939 RVA: 0x00148F0E File Offset: 0x0014710E
	public EntityPlayerLocal entityPlayerLocal
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x0600328C RID: 12940 RVA: 0x00148F17 File Offset: 0x00147117
	// (set) Token: 0x0600328D RID: 12941 RVA: 0x00148F1F File Offset: 0x0014711F
	public LocalPlayerUI playerUI
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x00148F28 File Offset: 0x00147128
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.avatarController = base.GetComponentInChildren<AvatarLocalPlayerController>();
		this.entityPlayerLocal = base.GetComponent<EntityPlayerLocal>();
		this.playerUI = LocalPlayerUI.GetUIForPlayer(this.entityPlayerLocal);
		LocalPlayerCamera.CameraType cameraType = LocalPlayerCamera.CameraType.Main;
		Camera[] componentsInChildren = base.GetComponentsInChildren<Camera>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Camera camera = componentsInChildren[i];
			if (i > 0)
			{
				cameraType = LocalPlayerCamera.CameraType.Weapon;
			}
			if (camera.name != "FinalCamera")
			{
				LocalPlayerCamera.AddToCamera(camera, cameraType).SetUI(this.playerUI);
			}
		}
		Transform transform = this.entityPlayerLocal.playerCamera.transform;
		Transform transform2 = transform.Find("ScreenEffectsWithDepth");
		if (transform2 != null)
		{
			this.SetupLocalPlayerVisual(transform2.Find("UnderwaterHaze"));
		}
		Transform transform3 = transform.Find("effect_refract_plane");
		if (transform3 != null)
		{
			transform3.GetComponent<MeshRenderer>().material.SetInt("_ZTest", 8);
			this.SetupLocalPlayerVisual(transform3);
		}
		this.SetupLocalPlayerVisual(transform.Find("effect_underwater_debris"));
		this.SetupLocalPlayerVisual(transform.Find("effect_dropletsParticle"));
		this.SetupLocalPlayerVisual(transform.Find("effect_water_fade"));
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupLocalPlayerVisual(Transform _transform)
	{
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x0014904C File Offset: 0x0014724C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator Start()
	{
		this.DispatchLocalPlayersChanged();
		while (this.avatarController == null)
		{
			yield return null;
			this.avatarController = base.GetComponentInChildren<AvatarLocalPlayerController>();
		}
		yield break;
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x0014905B File Offset: 0x0014725B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x0014906E File Offset: 0x0014726E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x00149081 File Offset: 0x00147281
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchLocalPlayersChanged()
	{
		LocalPlayerManager.LocalPlayersChanged();
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x00149088 File Offset: 0x00147288
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		int num = 0;
		Camera[] componentsInChildren = base.GetComponentsInChildren<Camera>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].depth = -1f + (float)(this.playerUI.playerIndex * 2 + num++) * 0.01f;
		}
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x001490D4 File Offset: 0x001472D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DispatchLocalPlayersChanged();
	}

	// Token: 0x04002822 RID: 10274
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarLocalPlayerController avatarController;
}
