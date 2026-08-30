using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015E4 RID: 5604
[Preserve]
public class vp_FPPlayerEventHandler : vp_PlayerEventHandler
{
	// Token: 0x040082FF RID: 33535
	public vp_Message<vp_DamageInfo> HUDDamageFlash;

	// Token: 0x04008300 RID: 33536
	public vp_Message<string> HUDText;

	// Token: 0x04008301 RID: 33537
	public vp_Value<Texture> Crosshair;

	// Token: 0x04008302 RID: 33538
	public vp_Value<Texture2D> CurrentAmmoIcon;

	// Token: 0x04008303 RID: 33539
	public vp_Value<Vector2> InputSmoothLook;

	// Token: 0x04008304 RID: 33540
	public vp_Value<Vector2> InputRawLook;

	// Token: 0x04008305 RID: 33541
	public vp_Message<string, bool> InputGetButton;

	// Token: 0x04008306 RID: 33542
	public vp_Message<string, bool> InputGetButtonUp;

	// Token: 0x04008307 RID: 33543
	public vp_Message<string, bool> InputGetButtonDown;

	// Token: 0x04008308 RID: 33544
	public vp_Value<bool> InputAllowGameplay;

	// Token: 0x04008309 RID: 33545
	public vp_Value<bool> Pause;

	// Token: 0x0400830A RID: 33546
	public vp_Value<Vector3> CameraLookDirection;

	// Token: 0x0400830B RID: 33547
	public vp_Message CameraToggle3rdPerson;

	// Token: 0x0400830C RID: 33548
	public vp_Message<float> CameraGroundStomp;

	// Token: 0x0400830D RID: 33549
	public vp_Message<float> CameraBombShake;

	// Token: 0x0400830E RID: 33550
	public vp_Value<Vector3> CameraEarthQuakeForce;

	// Token: 0x0400830F RID: 33551
	public vp_Activity<Vector3> CameraEarthQuake;

	// Token: 0x04008310 RID: 33552
	public vp_Value<vp_Interactable> Interactable;

	// Token: 0x04008311 RID: 33553
	public vp_Value<bool> CanInteract;

	// Token: 0x04008312 RID: 33554
	public vp_Value<string> CurrentWeaponClipType;

	// Token: 0x04008313 RID: 33555
	public vp_Attempt<object> AddAmmo;

	// Token: 0x04008314 RID: 33556
	public vp_Attempt RemoveClip;
}
