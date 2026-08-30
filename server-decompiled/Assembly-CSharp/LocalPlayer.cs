using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public class LocalPlayer : MonoBehaviour
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06003237 RID: 12855 RVA: 0x00146DE2 File Offset: 0x00144FE2
	// (set) Token: 0x06003238 RID: 12856 RVA: 0x00146DEA File Offset: 0x00144FEA
	public EntityPlayerLocal entityPlayerLocal
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06003239 RID: 12857 RVA: 0x00146DF3 File Offset: 0x00144FF3
	// (set) Token: 0x0600323A RID: 12858 RVA: 0x00146DFB File Offset: 0x00144FFB
	public LocalPlayerUI playerUI
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x00146E04 File Offset: 0x00145004
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

	// Token: 0x0600323C RID: 12860 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupLocalPlayerVisual(Transform _transform)
	{
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x00146F28 File Offset: 0x00145128
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

	// Token: 0x0600323E RID: 12862 RVA: 0x00146F37 File Offset: 0x00145137
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x00146F4A File Offset: 0x0014514A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x00146F5D File Offset: 0x0014515D
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchLocalPlayersChanged()
	{
		LocalPlayerManager.LocalPlayersChanged();
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x00146F64 File Offset: 0x00145164
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

	// Token: 0x06003242 RID: 12866 RVA: 0x00146FB0 File Offset: 0x001451B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DispatchLocalPlayersChanged();
	}

	// Token: 0x040027AF RID: 10159
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarLocalPlayerController avatarController;
}
