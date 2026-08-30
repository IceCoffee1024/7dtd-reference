using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001609 RID: 5641
[Preserve]
public class vp_PlayerEventHandler : vp_StateEventHandler
{
	// Token: 0x0600B126 RID: 45350 RVA: 0x004226BC File Offset: 0x004208BC
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

	// Token: 0x040084F1 RID: 34033
	public vp_Value<bool> IsFirstPerson;

	// Token: 0x040084F2 RID: 34034
	public vp_Value<bool> IsLocal;

	// Token: 0x040084F3 RID: 34035
	public vp_Value<bool> IsAI;

	// Token: 0x040084F4 RID: 34036
	public vp_Value<float> Health;

	// Token: 0x040084F5 RID: 34037
	public vp_Value<float> MaxHealth;

	// Token: 0x040084F6 RID: 34038
	public vp_Value<Vector3> Position;

	// Token: 0x040084F7 RID: 34039
	public vp_Value<Vector2> Rotation;

	// Token: 0x040084F8 RID: 34040
	public vp_Value<float> BodyYaw;

	// Token: 0x040084F9 RID: 34041
	public vp_Value<Vector3> LookPoint;

	// Token: 0x040084FA RID: 34042
	public vp_Value<Vector3> HeadLookDirection;

	// Token: 0x040084FB RID: 34043
	public vp_Value<Vector3> AimDirection;

	// Token: 0x040084FC RID: 34044
	public vp_Value<Vector3> MotorThrottle;

	// Token: 0x040084FD RID: 34045
	public vp_Value<bool> MotorJumpDone;

	// Token: 0x040084FE RID: 34046
	public vp_Value<Vector2> InputMoveVector;

	// Token: 0x040084FF RID: 34047
	public vp_Value<float> InputClimbVector;

	// Token: 0x04008500 RID: 34048
	public vp_Activity Dead;

	// Token: 0x04008501 RID: 34049
	public vp_Activity Run;

	// Token: 0x04008502 RID: 34050
	public vp_Activity Walk;

	// Token: 0x04008503 RID: 34051
	public vp_Activity Jump;

	// Token: 0x04008504 RID: 34052
	public vp_Activity Crouch;

	// Token: 0x04008505 RID: 34053
	public vp_Activity CrouchWalk;

	// Token: 0x04008506 RID: 34054
	public vp_Activity CrouchRun;

	// Token: 0x04008507 RID: 34055
	public vp_Activity Zoom;

	// Token: 0x04008508 RID: 34056
	public vp_Activity Attack;

	// Token: 0x04008509 RID: 34057
	public vp_Activity Reload;

	// Token: 0x0400850A RID: 34058
	public vp_Activity Climb;

	// Token: 0x0400850B RID: 34059
	public vp_Activity Interact;

	// Token: 0x0400850C RID: 34060
	public vp_Activity<int> SetWeapon;

	// Token: 0x0400850D RID: 34061
	public vp_Activity OutOfControl;

	// Token: 0x0400850E RID: 34062
	public vp_Activity Driving;

	// Token: 0x0400850F RID: 34063
	public vp_Value<bool> CameraRelativeMovement3P;

	// Token: 0x04008510 RID: 34064
	public vp_Value<Transform> CameraTransform;

	// Token: 0x04008511 RID: 34065
	public vp_Message<int> Wield;

	// Token: 0x04008512 RID: 34066
	public vp_Message Unwield;

	// Token: 0x04008513 RID: 34067
	public vp_Attempt Fire;

	// Token: 0x04008514 RID: 34068
	public vp_Message DryFire;

	// Token: 0x04008515 RID: 34069
	public vp_Attempt SetPrevWeapon;

	// Token: 0x04008516 RID: 34070
	public vp_Attempt SetNextWeapon;

	// Token: 0x04008517 RID: 34071
	public vp_Attempt<string> SetWeaponByName;

	// Token: 0x04008518 RID: 34072
	[Obsolete("Please use the 'CurrentWeaponIndex' vp_Value instead.")]
	public vp_Value<int> CurrentWeaponID;

	// Token: 0x04008519 RID: 34073
	public vp_Value<int> CurrentWeaponIndex;

	// Token: 0x0400851A RID: 34074
	public vp_Value<string> CurrentWeaponName;

	// Token: 0x0400851B RID: 34075
	public vp_Value<bool> CurrentWeaponWielded;

	// Token: 0x0400851C RID: 34076
	public vp_Attempt AutoReload;

	// Token: 0x0400851D RID: 34077
	public vp_Value<float> CurrentWeaponReloadDuration;

	// Token: 0x0400851E RID: 34078
	public vp_Message<string, int> GetItemCount;

	// Token: 0x0400851F RID: 34079
	public vp_Attempt RefillCurrentWeapon;

	// Token: 0x04008520 RID: 34080
	public vp_Value<int> CurrentWeaponAmmoCount;

	// Token: 0x04008521 RID: 34081
	public vp_Value<int> CurrentWeaponMaxAmmoCount;

	// Token: 0x04008522 RID: 34082
	public vp_Value<int> CurrentWeaponClipCount;

	// Token: 0x04008523 RID: 34083
	public vp_Value<int> CurrentWeaponType;

	// Token: 0x04008524 RID: 34084
	public vp_Value<int> CurrentWeaponGrip;

	// Token: 0x04008525 RID: 34085
	public vp_Attempt<object> AddItem;

	// Token: 0x04008526 RID: 34086
	public vp_Attempt<object> RemoveItem;

	// Token: 0x04008527 RID: 34087
	public vp_Attempt DepleteAmmo;

	// Token: 0x04008528 RID: 34088
	public vp_Message<Vector3> Move;

	// Token: 0x04008529 RID: 34089
	public vp_Value<Vector3> Velocity;

	// Token: 0x0400852A RID: 34090
	public vp_Value<float> SlopeLimit;

	// Token: 0x0400852B RID: 34091
	public vp_Value<float> StepOffset;

	// Token: 0x0400852C RID: 34092
	public vp_Value<float> Radius;

	// Token: 0x0400852D RID: 34093
	public vp_Value<float> Height;

	// Token: 0x0400852E RID: 34094
	public vp_Value<float> FallSpeed;

	// Token: 0x0400852F RID: 34095
	public vp_Message<float> FallImpact;

	// Token: 0x04008530 RID: 34096
	public vp_Message<float> FallImpact2;

	// Token: 0x04008531 RID: 34097
	public vp_Message<float> HeadImpact;

	// Token: 0x04008532 RID: 34098
	public vp_Message<Vector3> ForceImpact;

	// Token: 0x04008533 RID: 34099
	public vp_Message Stop;

	// Token: 0x04008534 RID: 34100
	public vp_Value<Transform> Platform;

	// Token: 0x04008535 RID: 34101
	public vp_Value<Texture> GroundTexture;

	// Token: 0x04008536 RID: 34102
	public vp_Value<vp_SurfaceIdentifier> SurfaceType;
}
