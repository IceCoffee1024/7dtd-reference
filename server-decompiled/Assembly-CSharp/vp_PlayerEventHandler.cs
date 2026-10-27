using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200163A RID: 5690
[Preserve]
public class vp_PlayerEventHandler : vp_StateEventHandler
{
	// Token: 0x0600B210 RID: 45584 RVA: 0x0042879C File Offset: 0x0042699C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		base.BindStateToActivity(this.Run);
		base.BindStateToActivity(this.Walk);
		base.BindStateToActivity(this.Jump);
		base.BindStateToActivity(this.Crouch);
		base.BindStateToActivity(this.CrouchWalk);
		base.BindStateToActivity(this.CrouchRun);
		base.BindStateToActivity(this.Zoom);
		base.BindStateToActivity(this.Reload);
		base.BindStateToActivity(this.Dead);
		base.BindStateToActivity(this.Climb);
		base.BindStateToActivity(this.OutOfControl);
		base.BindStateToActivity(this.Driving);
		base.BindStateToActivityOnStart(this.Attack);
		this.SetWeapon.AutoDuration = 1f;
		this.Reload.AutoDuration = 1f;
		this.Zoom.MinDuration = 0.2f;
		this.Crouch.MinDuration = 0.1f;
		this.Jump.MinPause = 0f;
		this.SetWeapon.MinPause = 0.2f;
	}

	// Token: 0x04008588 RID: 34184
	public vp_Value<bool> IsFirstPerson;

	// Token: 0x04008589 RID: 34185
	public vp_Value<bool> IsLocal;

	// Token: 0x0400858A RID: 34186
	public vp_Value<bool> IsAI;

	// Token: 0x0400858B RID: 34187
	public vp_Value<float> Health;

	// Token: 0x0400858C RID: 34188
	public vp_Value<float> MaxHealth;

	// Token: 0x0400858D RID: 34189
	public vp_Value<Vector3> Position;

	// Token: 0x0400858E RID: 34190
	public vp_Value<Vector2> Rotation;

	// Token: 0x0400858F RID: 34191
	public vp_Value<float> BodyYaw;

	// Token: 0x04008590 RID: 34192
	public vp_Value<Vector3> LookPoint;

	// Token: 0x04008591 RID: 34193
	public vp_Value<Vector3> HeadLookDirection;

	// Token: 0x04008592 RID: 34194
	public vp_Value<Vector3> AimDirection;

	// Token: 0x04008593 RID: 34195
	public vp_Value<Vector3> MotorThrottle;

	// Token: 0x04008594 RID: 34196
	public vp_Value<bool> MotorJumpDone;

	// Token: 0x04008595 RID: 34197
	public vp_Value<Vector2> InputMoveVector;

	// Token: 0x04008596 RID: 34198
	public vp_Value<float> InputClimbVector;

	// Token: 0x04008597 RID: 34199
	public vp_Activity Dead;

	// Token: 0x04008598 RID: 34200
	public vp_Activity Run;

	// Token: 0x04008599 RID: 34201
	public vp_Activity Walk;

	// Token: 0x0400859A RID: 34202
	public vp_Activity Jump;

	// Token: 0x0400859B RID: 34203
	public vp_Activity Crouch;

	// Token: 0x0400859C RID: 34204
	public vp_Activity CrouchWalk;

	// Token: 0x0400859D RID: 34205
	public vp_Activity CrouchRun;

	// Token: 0x0400859E RID: 34206
	public vp_Activity Zoom;

	// Token: 0x0400859F RID: 34207
	public vp_Activity Attack;

	// Token: 0x040085A0 RID: 34208
	public vp_Activity Reload;

	// Token: 0x040085A1 RID: 34209
	public vp_Activity Climb;

	// Token: 0x040085A2 RID: 34210
	public vp_Activity Interact;

	// Token: 0x040085A3 RID: 34211
	public vp_Activity<int> SetWeapon;

	// Token: 0x040085A4 RID: 34212
	public vp_Activity OutOfControl;

	// Token: 0x040085A5 RID: 34213
	public vp_Activity Driving;

	// Token: 0x040085A6 RID: 34214
	public vp_Value<bool> CameraRelativeMovement3P;

	// Token: 0x040085A7 RID: 34215
	public vp_Value<Transform> CameraTransform;

	// Token: 0x040085A8 RID: 34216
	public vp_Message<int> Wield;

	// Token: 0x040085A9 RID: 34217
	public vp_Message Unwield;

	// Token: 0x040085AA RID: 34218
	public vp_Attempt Fire;

	// Token: 0x040085AB RID: 34219
	public vp_Message DryFire;

	// Token: 0x040085AC RID: 34220
	public vp_Attempt SetPrevWeapon;

	// Token: 0x040085AD RID: 34221
	public vp_Attempt SetNextWeapon;

	// Token: 0x040085AE RID: 34222
	public vp_Attempt<string> SetWeaponByName;

	// Token: 0x040085AF RID: 34223
	[Obsolete("Please use the 'CurrentWeaponIndex' vp_Value instead.")]
	public vp_Value<int> CurrentWeaponID;

	// Token: 0x040085B0 RID: 34224
	public vp_Value<int> CurrentWeaponIndex;

	// Token: 0x040085B1 RID: 34225
	public vp_Value<string> CurrentWeaponName;

	// Token: 0x040085B2 RID: 34226
	public vp_Value<bool> CurrentWeaponWielded;

	// Token: 0x040085B3 RID: 34227
	public vp_Attempt AutoReload;

	// Token: 0x040085B4 RID: 34228
	public vp_Value<float> CurrentWeaponReloadDuration;

	// Token: 0x040085B5 RID: 34229
	public vp_Message<string, int> GetItemCount;

	// Token: 0x040085B6 RID: 34230
	public vp_Attempt RefillCurrentWeapon;

	// Token: 0x040085B7 RID: 34231
	public vp_Value<int> CurrentWeaponAmmoCount;

	// Token: 0x040085B8 RID: 34232
	public vp_Value<int> CurrentWeaponMaxAmmoCount;

	// Token: 0x040085B9 RID: 34233
	public vp_Value<int> CurrentWeaponClipCount;

	// Token: 0x040085BA RID: 34234
	public vp_Value<int> CurrentWeaponType;

	// Token: 0x040085BB RID: 34235
	public vp_Value<int> CurrentWeaponGrip;

	// Token: 0x040085BC RID: 34236
	public vp_Attempt<object> AddItem;

	// Token: 0x040085BD RID: 34237
	public vp_Attempt<object> RemoveItem;

	// Token: 0x040085BE RID: 34238
	public vp_Attempt DepleteAmmo;

	// Token: 0x040085BF RID: 34239
	public vp_Message<Vector3> Move;

	// Token: 0x040085C0 RID: 34240
	public vp_Value<Vector3> Velocity;

	// Token: 0x040085C1 RID: 34241
	public vp_Value<float> SlopeLimit;

	// Token: 0x040085C2 RID: 34242
	public vp_Value<float> StepOffset;

	// Token: 0x040085C3 RID: 34243
	public vp_Value<float> Radius;

	// Token: 0x040085C4 RID: 34244
	public vp_Value<float> Height;

	// Token: 0x040085C5 RID: 34245
	public vp_Value<float> FallSpeed;

	// Token: 0x040085C6 RID: 34246
	public vp_Message<float> FallImpact;

	// Token: 0x040085C7 RID: 34247
	public vp_Message<float> FallImpact2;

	// Token: 0x040085C8 RID: 34248
	public vp_Message<float> HeadImpact;

	// Token: 0x040085C9 RID: 34249
	public vp_Message<Vector3> ForceImpact;

	// Token: 0x040085CA RID: 34250
	public vp_Message Stop;

	// Token: 0x040085CB RID: 34251
	public vp_Value<Transform> Platform;

	// Token: 0x040085CC RID: 34252
	public vp_Value<Texture> GroundTexture;

	// Token: 0x040085CD RID: 34253
	public vp_Value<vp_SurfaceIdentifier> SurfaceType;
}
