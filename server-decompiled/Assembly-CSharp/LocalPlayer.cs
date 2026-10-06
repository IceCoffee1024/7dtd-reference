using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000613 RID: 1555
public class LocalPlayer : MonoBehaviour
{
	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x060032CD RID: 13005 RVA: 0x001498AA File Offset: 0x00147AAA
	// (set) Token: 0x060032CE RID: 13006 RVA: 0x001498B2 File Offset: 0x00147AB2
	public EntityPlayerLocal entityPlayerLocal
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060032CF RID: 13007 RVA: 0x001498BB File Offset: 0x00147ABB
	// (set) Token: 0x060032D0 RID: 13008 RVA: 0x001498C3 File Offset: 0x00147AC3
	public LocalPlayerUI playerUI
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x001498CC File Offset: 0x00147ACC
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

	// Token: 0x060032D2 RID: 13010 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupLocalPlayerVisual(Transform _transform)
	{
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x001499F0 File Offset: 0x00147BF0
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

	// Token: 0x060032D4 RID: 13012 RVA: 0x001499FF File Offset: 0x00147BFF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x00149A12 File Offset: 0x00147C12
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x00149A25 File Offset: 0x00147C25
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchLocalPlayersChanged()
	{
		LocalPlayerManager.LocalPlayersChanged();
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00149A2C File Offset: 0x00147C2C
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

	// Token: 0x060032D8 RID: 13016 RVA: 0x00149A78 File Offset: 0x00147C78
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DispatchLocalPlayersChanged();
	}

	// Token: 0x0400281B RID: 10267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarLocalPlayerController avatarController;
}
