using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015EE RID: 5614
[Preserve]
public class vp_PlayerEventHandler : vp_StateEventHandler
{
	// Token: 0x0600B029 RID: 45097 RVA: 0x0041D37C File Offset: 0x0041B57C
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

	// Token: 0x04008418 RID: 33816
	public vp_Value<bool> IsFirstPerson;

	// Token: 0x04008419 RID: 33817
	public vp_Value<bool> IsLocal;

	// Token: 0x0400841A RID: 33818
	public vp_Value<bool> IsAI;

	// Token: 0x0400841B RID: 33819
	public vp_Value<float> Health;

	// Token: 0x0400841C RID: 33820
	public vp_Value<float> MaxHealth;

	// Token: 0x0400841D RID: 33821
	public vp_Value<Vector3> Position;

	// Token: 0x0400841E RID: 33822
	public vp_Value<Vector2> Rotation;

	// Token: 0x0400841F RID: 33823
	public vp_Value<float> BodyYaw;

	// Token: 0x04008420 RID: 33824
	public vp_Value<Vector3> LookPoint;

	// Token: 0x04008421 RID: 33825
	public vp_Value<Vector3> HeadLookDirection;

	// Token: 0x04008422 RID: 33826
	public vp_Value<Vector3> AimDirection;

	// Token: 0x04008423 RID: 33827
	public vp_Value<Vector3> MotorThrottle;

	// Token: 0x04008424 RID: 33828
	public vp_Value<bool> MotorJumpDone;

	// Token: 0x04008425 RID: 33829
	public vp_Value<Vector2> InputMoveVector;

	// Token: 0x04008426 RID: 33830
	public vp_Value<float> InputClimbVector;

	// Token: 0x04008427 RID: 33831
	public vp_Activity Dead;

	// Token: 0x04008428 RID: 33832
	public vp_Activity Run;

	// Token: 0x04008429 RID: 33833
	public vp_Activity Walk;

	// Token: 0x0400842A RID: 33834
	public vp_Activity Jump;

	// Token: 0x0400842B RID: 33835
	public vp_Activity Crouch;

	// Token: 0x0400842C RID: 33836
	public vp_Activity CrouchWalk;

	// Token: 0x0400842D RID: 33837
	public vp_Activity CrouchRun;

	// Token: 0x0400842E RID: 33838
	public vp_Activity Zoom;

	// Token: 0x0400842F RID: 33839
	public vp_Activity Attack;

	// Token: 0x04008430 RID: 33840
	public vp_Activity Reload;

	// Token: 0x04008431 RID: 33841
	public vp_Activity Climb;

	// Token: 0x04008432 RID: 33842
	public vp_Activity Interact;

	// Token: 0x04008433 RID: 33843
	public vp_Activity<int> SetWeapon;

	// Token: 0x04008434 RID: 33844
	public vp_Activity OutOfControl;

	// Token: 0x04008435 RID: 33845
	public vp_Activity Driving;

	// Token: 0x04008436 RID: 33846
	public vp_Value<bool> CameraRelativeMovement3P;

	// Token: 0x04008437 RID: 33847
	public vp_Value<Transform> CameraTransform;

	// Token: 0x04008438 RID: 33848
	public vp_Message<int> Wield;

	// Token: 0x04008439 RID: 33849
	public vp_Message Unwield;

	// Token: 0x0400843A RID: 33850
	public vp_Attempt Fire;

	// Token: 0x0400843B RID: 33851
	public vp_Message DryFire;

	// Token: 0x0400843C RID: 33852
	public vp_Attempt SetPrevWeapon;

	// Token: 0x0400843D RID: 33853
	public vp_Attempt SetNextWeapon;

	// Token: 0x0400843E RID: 33854
	public vp_Attempt<string> SetWeaponByName;

	// Token: 0x0400843F RID: 33855
	[Obsolete("Please use the 'CurrentWeaponIndex' vp_Value instead.")]
	public vp_Value<int> CurrentWeaponID;

	// Token: 0x04008440 RID: 33856
	public vp_Value<int> CurrentWeaponIndex;

	// Token: 0x04008441 RID: 33857
	public vp_Value<string> CurrentWeaponName;

	// Token: 0x04008442 RID: 33858
	public vp_Value<bool> CurrentWeaponWielded;

	// Token: 0x04008443 RID: 33859
	public vp_Attempt AutoReload;

	// Token: 0x04008444 RID: 33860
	public vp_Value<float> CurrentWeaponReloadDuration;

	// Token: 0x04008445 RID: 33861
	public vp_Message<string, int> GetItemCount;

	// Token: 0x04008446 RID: 33862
	public vp_Attempt RefillCurrentWeapon;

	// Token: 0x04008447 RID: 33863
	public vp_Value<int> CurrentWeaponAmmoCount;

	// Token: 0x04008448 RID: 33864
	public vp_Value<int> CurrentWeaponMaxAmmoCount;

	// Token: 0x04008449 RID: 33865
	public vp_Value<int> CurrentWeaponClipCount;

	// Token: 0x0400844A RID: 33866
	public vp_Value<int> CurrentWeaponType;

	// Token: 0x0400844B RID: 33867
	public vp_Value<int> CurrentWeaponGrip;

	// Token: 0x0400844C RID: 33868
	public vp_Attempt<object> AddItem;

	// Token: 0x0400844D RID: 33869
	public vp_Attempt<object> RemoveItem;

	// Token: 0x0400844E RID: 33870
	public vp_Attempt DepleteAmmo;

	// Token: 0x0400844F RID: 33871
	public vp_Message<Vector3> Move;

	// Token: 0x04008450 RID: 33872
	public vp_Value<Vector3> Velocity;

	// Token: 0x04008451 RID: 33873
	public vp_Value<float> SlopeLimit;

	// Token: 0x04008452 RID: 33874
	public vp_Value<float> StepOffset;

	// Token: 0x04008453 RID: 33875
	public vp_Value<float> Radius;

	// Token: 0x04008454 RID: 33876
	public vp_Value<float> Height;

	// Token: 0x04008455 RID: 33877
	public vp_Value<float> FallSpeed;

	// Token: 0x04008456 RID: 33878
	public vp_Message<float> FallImpact;

	// Token: 0x04008457 RID: 33879
	public vp_Message<float> FallImpact2;

	// Token: 0x04008458 RID: 33880
	public vp_Message<float> HeadImpact;

	// Token: 0x04008459 RID: 33881
	public vp_Message<Vector3> ForceImpact;

	// Token: 0x0400845A RID: 33882
	public vp_Message Stop;

	// Token: 0x0400845B RID: 33883
	public vp_Value<Transform> Platform;

	// Token: 0x0400845C RID: 33884
	public vp_Value<Texture> GroundTexture;

	// Token: 0x0400845D RID: 33885
	public vp_Value<vp_SurfaceIdentifier> SurfaceType;
}
