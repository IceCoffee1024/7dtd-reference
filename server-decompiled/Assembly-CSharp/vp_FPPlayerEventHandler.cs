using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015FF RID: 5631
[Preserve]
public class vp_FPPlayerEventHandler : vp_PlayerEventHandler
{
	// Token: 0x040083D8 RID: 33752
	public vp_Message<vp_DamageInfo> HUDDamageFlash;

	// Token: 0x040083D9 RID: 33753
	public vp_Message<string> HUDText;

	// Token: 0x040083DA RID: 33754
	public vp_Value<Texture> Crosshair;

	// Token: 0x040083DB RID: 33755
	public vp_Value<Texture2D> CurrentAmmoIcon;

	// Token: 0x040083DC RID: 33756
	public vp_Value<Vector2> InputSmoothLook;

	// Token: 0x040083DD RID: 33757
	public vp_Value<Vector2> InputRawLook;

	// Token: 0x040083DE RID: 33758
	public vp_Message<string, bool> InputGetButton;

	// Token: 0x040083DF RID: 33759
	public vp_Message<string, bool> InputGetButtonUp;

	// Token: 0x040083E0 RID: 33760
	public vp_Message<string, bool> InputGetButtonDown;

	// Token: 0x040083E1 RID: 33761
	public vp_Value<bool> InputAllowGameplay;

	// Token: 0x040083E2 RID: 33762
	public vp_Value<bool> Pause;

	// Token: 0x040083E3 RID: 33763
	public vp_Value<Vector3> CameraLookDirection;

	// Token: 0x040083E4 RID: 33764
	public vp_Message CameraToggle3rdPerson;

	// Token: 0x040083E5 RID: 33765
	public vp_Message<float> CameraGroundStomp;

	// Token: 0x040083E6 RID: 33766
	public vp_Message<float> CameraBombShake;

	// Token: 0x040083E7 RID: 33767
	public vp_Value<Vector3> CameraEarthQuakeForce;

	// Token: 0x040083E8 RID: 33768
	public vp_Activity<Vector3> CameraEarthQuake;

	// Token: 0x040083E9 RID: 33769
	public vp_Value<vp_Interactable> Interactable;

	// Token: 0x040083EA RID: 33770
	public vp_Value<bool> CanInteract;

	// Token: 0x040083EB RID: 33771
	public vp_Value<string> CurrentWeaponClipType;

	// Token: 0x040083EC RID: 33772
	public vp_Attempt<object> AddAmmo;

	// Token: 0x040083ED RID: 33773
	public vp_Attempt RemoveClip;
}
