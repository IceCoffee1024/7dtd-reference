using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class LocalPlayer : MonoBehaviour
{
	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x060032DB RID: 13019 RVA: 0x0014A49E File Offset: 0x0014869E
	// (set) Token: 0x060032DC RID: 13020 RVA: 0x0014A4A6 File Offset: 0x001486A6
	public EntityPlayerLocal entityPlayerLocal
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060032DD RID: 13021 RVA: 0x0014A4AF File Offset: 0x001486AF
	// (set) Token: 0x060032DE RID: 13022 RVA: 0x0014A4B7 File Offset: 0x001486B7
	public LocalPlayerUI playerUI
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x0014A4C0 File Offset: 0x001486C0
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

	// Token: 0x060032E0 RID: 13024 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupLocalPlayerVisual(Transform _transform)
	{
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x0014A5E4 File Offset: 0x001487E4
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

	// Token: 0x060032E2 RID: 13026 RVA: 0x0014A5F3 File Offset: 0x001487F3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x0014A606 File Offset: 0x00148806
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x0014A619 File Offset: 0x00148819
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchLocalPlayersChanged()
	{
		LocalPlayerManager.LocalPlayersChanged();
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x0014A620 File Offset: 0x00148820
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

	// Token: 0x060032E6 RID: 13030 RVA: 0x0014A66C File Offset: 0x0014886C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DispatchLocalPlayersChanged();
	}

	// Token: 0x04002836 RID: 10294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarLocalPlayerController avatarController;
}
