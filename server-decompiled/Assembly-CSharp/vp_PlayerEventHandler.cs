using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001628 RID: 5672
[Preserve]
public class vp_PlayerEventHandler : vp_StateEventHandler
{
	// Token: 0x0600B1B8 RID: 45496 RVA: 0x00426398 File Offset: 0x00424598
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

	// Token: 0x04008547 RID: 34119
	public vp_Value<bool> IsFirstPerson;

	// Token: 0x04008548 RID: 34120
	public vp_Value<bool> IsLocal;

	// Token: 0x04008549 RID: 34121
	public vp_Value<bool> IsAI;

	// Token: 0x0400854A RID: 34122
	public vp_Value<float> Health;

	// Token: 0x0400854B RID: 34123
	public vp_Value<float> MaxHealth;

	// Token: 0x0400854C RID: 34124
	public vp_Value<Vector3> Position;

	// Token: 0x0400854D RID: 34125
	public vp_Value<Vector2> Rotation;

	// Token: 0x0400854E RID: 34126
	public vp_Value<float> BodyYaw;

	// Token: 0x0400854F RID: 34127
	public vp_Value<Vector3> LookPoint;

	// Token: 0x04008550 RID: 34128
	public vp_Value<Vector3> HeadLookDirection;

	// Token: 0x04008551 RID: 34129
	public vp_Value<Vector3> AimDirection;

	// Token: 0x04008552 RID: 34130
	public vp_Value<Vector3> MotorThrottle;

	// Token: 0x04008553 RID: 34131
	public vp_Value<bool> MotorJumpDone;

	// Token: 0x04008554 RID: 34132
	public vp_Value<Vector2> InputMoveVector;

	// Token: 0x04008555 RID: 34133
	public vp_Value<float> InputClimbVector;

	// Token: 0x04008556 RID: 34134
	public vp_Activity Dead;

	// Token: 0x04008557 RID: 34135
	public vp_Activity Run;

	// Token: 0x04008558 RID: 34136
	public vp_Activity Walk;

	// Token: 0x04008559 RID: 34137
	public vp_Activity Jump;

	// Token: 0x0400855A RID: 34138
	public vp_Activity Crouch;

	// Token: 0x0400855B RID: 34139
	public vp_Activity CrouchWalk;

	// Token: 0x0400855C RID: 34140
	public vp_Activity CrouchRun;

	// Token: 0x0400855D RID: 34141
	public vp_Activity Zoom;

	// Token: 0x0400855E RID: 34142
	public vp_Activity Attack;

	// Token: 0x0400855F RID: 34143
	public vp_Activity Reload;

	// Token: 0x04008560 RID: 34144
	public vp_Activity Climb;

	// Token: 0x04008561 RID: 34145
	public vp_Activity Interact;

	// Token: 0x04008562 RID: 34146
	public vp_Activity<int> SetWeapon;

	// Token: 0x04008563 RID: 34147
	public vp_Activity OutOfControl;

	// Token: 0x04008564 RID: 34148
	public vp_Activity Driving;

	// Token: 0x04008565 RID: 34149
	public vp_Value<bool> CameraRelativeMovement3P;

	// Token: 0x04008566 RID: 34150
	public vp_Value<Transform> CameraTransform;

	// Token: 0x04008567 RID: 34151
	public vp_Message<int> Wield;

	// Token: 0x04008568 RID: 34152
	public vp_Message Unwield;

	// Token: 0x04008569 RID: 34153
	public vp_Attempt Fire;

	// Token: 0x0400856A RID: 34154
	public vp_Message DryFire;

	// Token: 0x0400856B RID: 34155
	public vp_Attempt SetPrevWeapon;

	// Token: 0x0400856C RID: 34156
	public vp_Attempt SetNextWeapon;

	// Token: 0x0400856D RID: 34157
	public vp_Attempt<string> SetWeaponByName;

	// Token: 0x0400856E RID: 34158
	[Obsolete("Please use the 'CurrentWeaponIndex' vp_Value instead.")]
	public vp_Value<int> CurrentWeaponID;

	// Token: 0x0400856F RID: 34159
	public vp_Value<int> CurrentWeaponIndex;

	// Token: 0x04008570 RID: 34160
	public vp_Value<string> CurrentWeaponName;

	// Token: 0x04008571 RID: 34161
	public vp_Value<bool> CurrentWeaponWielded;

	// Token: 0x04008572 RID: 34162
	public vp_Attempt AutoReload;

	// Token: 0x04008573 RID: 34163
	public vp_Value<float> CurrentWeaponReloadDuration;

	// Token: 0x04008574 RID: 34164
	public vp_Message<string, int> GetItemCount;

	// Token: 0x04008575 RID: 34165
	public vp_Attempt RefillCurrentWeapon;

	// Token: 0x04008576 RID: 34166
	public vp_Value<int> CurrentWeaponAmmoCount;

	// Token: 0x04008577 RID: 34167
	public vp_Value<int> CurrentWeaponMaxAmmoCount;

	// Token: 0x04008578 RID: 34168
	public vp_Value<int> CurrentWeaponClipCount;

	// Token: 0x04008579 RID: 34169
	public vp_Value<int> CurrentWeaponType;

	// Token: 0x0400857A RID: 34170
	public vp_Value<int> CurrentWeaponGrip;

	// Token: 0x0400857B RID: 34171
	public vp_Attempt<object> AddItem;

	// Token: 0x0400857C RID: 34172
	public vp_Attempt<object> RemoveItem;

	// Token: 0x0400857D RID: 34173
	public vp_Attempt DepleteAmmo;

	// Token: 0x0400857E RID: 34174
	public vp_Message<Vector3> Move;

	// Token: 0x0400857F RID: 34175
	public vp_Value<Vector3> Velocity;

	// Token: 0x04008580 RID: 34176
	public vp_Value<float> SlopeLimit;

	// Token: 0x04008581 RID: 34177
	public vp_Value<float> StepOffset;

	// Token: 0x04008582 RID: 34178
	public vp_Value<float> Radius;

	// Token: 0x04008583 RID: 34179
	public vp_Value<float> Height;

	// Token: 0x04008584 RID: 34180
	public vp_Value<float> FallSpeed;

	// Token: 0x04008585 RID: 34181
	public vp_Message<float> FallImpact;

	// Token: 0x04008586 RID: 34182
	public vp_Message<float> FallImpact2;

	// Token: 0x04008587 RID: 34183
	public vp_Message<float> HeadImpact;

	// Token: 0x04008588 RID: 34184
	public vp_Message<Vector3> ForceImpact;

	// Token: 0x04008589 RID: 34185
	public vp_Message Stop;

	// Token: 0x0400858A RID: 34186
	public vp_Value<Transform> Platform;

	// Token: 0x0400858B RID: 34187
	public vp_Value<Texture> GroundTexture;

	// Token: 0x0400858C RID: 34188
	public vp_Value<vp_SurfaceIdentifier> SurfaceType;
}
