using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200161E RID: 5662
[Preserve]
public class vp_FPPlayerEventHandler : vp_PlayerEventHandler
{
	// Token: 0x0400842E RID: 33838
	public vp_Message<vp_DamageInfo> HUDDamageFlash;

	// Token: 0x0400842F RID: 33839
	public vp_Message<string> HUDText;

	// Token: 0x04008430 RID: 33840
	public vp_Value<Texture> Crosshair;

	// Token: 0x04008431 RID: 33841
	public vp_Value<Texture2D> CurrentAmmoIcon;

	// Token: 0x04008432 RID: 33842
	public vp_Value<Vector2> InputSmoothLook;

	// Token: 0x04008433 RID: 33843
	public vp_Value<Vector2> InputRawLook;

	// Token: 0x04008434 RID: 33844
	public vp_Message<string, bool> InputGetButton;

	// Token: 0x04008435 RID: 33845
	public vp_Message<string, bool> InputGetButtonUp;

	// Token: 0x04008436 RID: 33846
	public vp_Message<string, bool> InputGetButtonDown;

	// Token: 0x04008437 RID: 33847
	public vp_Value<bool> InputAllowGameplay;

	// Token: 0x04008438 RID: 33848
	public vp_Value<bool> Pause;

	// Token: 0x04008439 RID: 33849
	public vp_Value<Vector3> CameraLookDirection;

	// Token: 0x0400843A RID: 33850
	public vp_Message CameraToggle3rdPerson;

	// Token: 0x0400843B RID: 33851
	public vp_Message<float> CameraGroundStomp;

	// Token: 0x0400843C RID: 33852
	public vp_Message<float> CameraBombShake;

	// Token: 0x0400843D RID: 33853
	public vp_Value<Vector3> CameraEarthQuakeForce;

	// Token: 0x0400843E RID: 33854
	public vp_Activity<Vector3> CameraEarthQuake;

	// Token: 0x0400843F RID: 33855
	public vp_Value<vp_Interactable> Interactable;

	// Token: 0x04008440 RID: 33856
	public vp_Value<bool> CanInteract;

	// Token: 0x04008441 RID: 33857
	public vp_Value<string> CurrentWeaponClipType;

	// Token: 0x04008442 RID: 33858
	public vp_Attempt<object> AddAmmo;

	// Token: 0x04008443 RID: 33859
	public vp_Attempt RemoveClip;
}
