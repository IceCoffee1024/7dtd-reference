using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001630 RID: 5680
[Preserve]
public class vp_FPPlayerEventHandler : vp_PlayerEventHandler
{
	// Token: 0x0400846F RID: 33903
	public vp_Message<vp_DamageInfo> HUDDamageFlash;

	// Token: 0x04008470 RID: 33904
	public vp_Message<string> HUDText;

	// Token: 0x04008471 RID: 33905
	public vp_Value<Texture> Crosshair;

	// Token: 0x04008472 RID: 33906
	public vp_Value<Texture2D> CurrentAmmoIcon;

	// Token: 0x04008473 RID: 33907
	public vp_Value<Vector2> InputSmoothLook;

	// Token: 0x04008474 RID: 33908
	public vp_Value<Vector2> InputRawLook;

	// Token: 0x04008475 RID: 33909
	public vp_Message<string, bool> InputGetButton;

	// Token: 0x04008476 RID: 33910
	public vp_Message<string, bool> InputGetButtonUp;

	// Token: 0x04008477 RID: 33911
	public vp_Message<string, bool> InputGetButtonDown;

	// Token: 0x04008478 RID: 33912
	public vp_Value<bool> InputAllowGameplay;

	// Token: 0x04008479 RID: 33913
	public vp_Value<bool> Pause;

	// Token: 0x0400847A RID: 33914
	public vp_Value<Vector3> CameraLookDirection;

	// Token: 0x0400847B RID: 33915
	public vp_Message CameraToggle3rdPerson;

	// Token: 0x0400847C RID: 33916
	public vp_Message<float> CameraGroundStomp;

	// Token: 0x0400847D RID: 33917
	public vp_Message<float> CameraBombShake;

	// Token: 0x0400847E RID: 33918
	public vp_Value<Vector3> CameraEarthQuakeForce;

	// Token: 0x0400847F RID: 33919
	public vp_Activity<Vector3> CameraEarthQuake;

	// Token: 0x04008480 RID: 33920
	public vp_Value<vp_Interactable> Interactable;

	// Token: 0x04008481 RID: 33921
	public vp_Value<bool> CanInteract;

	// Token: 0x04008482 RID: 33922
	public vp_Value<string> CurrentWeaponClipType;

	// Token: 0x04008483 RID: 33923
	public vp_Attempt<object> AddAmmo;

	// Token: 0x04008484 RID: 33924
	public vp_Attempt RemoveClip;
}
