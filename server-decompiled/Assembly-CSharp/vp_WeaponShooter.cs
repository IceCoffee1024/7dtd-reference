using System;
using UnityEngine;

// Token: 0x02001647 RID: 5703
public class vp_WeaponShooter : vp_Shooter
{
	// Token: 0x1700151F RID: 5407
	// (get) Token: 0x0600B2AF RID: 45743 RVA: 0x0042B3F8 File Offset: 0x004295F8
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_PlayerEventHandler)base.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x17001520 RID: 5408
	// (get) Token: 0x0600B2B0 RID: 45744 RVA: 0x0042B42D File Offset: 0x0042962D
	public vp_Weapon Weapon
	{
		get
		{
			if (this.m_Weapon == null)
			{
				this.m_Weapon = base.transform.GetComponent<vp_Weapon>();
			}
			return this.m_Weapon;
		}
	}

	// Token: 0x0600B2B1 RID: 45745 RVA: 0x0042B454 File Offset: 0x00429654
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		if (this.m_ProjectileSpawnPoint == null && this.Weapon.Weapon3rdPersonModel != null)
		{
			this.m_ProjectileSpawnPoint = this.Weapon.Weapon3rdPersonModel;
		}
		if (this.GetFireSeed == null)
		{
			this.GetFireSeed = () => global::UnityEngine.Random.Range(0, 100);
		}
		if (this.GetFirePosition == null)
		{
			this.GetFirePosition = () => this.FirePosition;
		}
		if (this.GetFireRotation == null)
		{
			this.GetFireRotation = delegate
			{
				Quaternion quaternion = Quaternion.identity;
				if (this.Player.LookPoint.Get() - this.FirePosition != Vector3.zero)
				{
					quaternion = vp_MathUtility.NaNSafeQuaternion(Quaternion.LookRotation(this.Player.LookPoint.Get() - this.FirePosition), default(Quaternion));
				}
				return quaternion;
			};
		}
		base.Awake();
		this.m_NextAllowedFireTime = Time.time;
		this.ProjectileSpawnDelay = Mathf.Min(this.ProjectileSpawnDelay, this.ProjectileFiringRate - 0.1f);
	}

	// Token: 0x0600B2B2 RID: 45746 RVA: 0x0042B522 File Offset: 0x00429722
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600B2B3 RID: 45747 RVA: 0x0042B52C File Offset: 0x0042972C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		if (this.Player == null)
		{
			return;
		}
		if (this.Player.IsFirstPerson == null)
		{
			return;
		}
		if (!this.Player.IsFirstPerson.Get() && this.m_3rdPersonFiredThisFrame)
		{
			this.m_3rdPersonFiredThisFrame = false;
		}
		this.m_WeaponWasInAttackStateLastFrame = this.Weapon.StateManager.IsEnabled("Attack");
		base.LateUpdate();
	}

	// Token: 0x0600B2B4 RID: 45748 RVA: 0x0042B5A0 File Offset: 0x004297A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Fire()
	{
		if (vp_Gameplay.isMultiplayer && !this.Player.IsLocal.Get())
		{
			this.ProjectileSpawnDelay = 0f;
		}
		this.m_LastFireTime = Time.time;
		if (!this.Player.IsFirstPerson.Get())
		{
			this.m_3rdPersonFiredThisFrame = true;
		}
		if (this.MotionRecoilDelay == 0f)
		{
			this.ApplyRecoil();
		}
		else
		{
			vp_Timer.In(this.MotionRecoilDelay, new vp_Timer.Callback(this.ApplyRecoil), null);
		}
		base.Fire();
	}

	// Token: 0x0600B2B5 RID: 45749 RVA: 0x0042B634 File Offset: 0x00429834
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ShowMuzzleFlash()
	{
		if (this.m_MuzzleFlash == null)
		{
			return;
		}
		if (this.MuzzleFlashFirstShotMaxDeviation == 180f || this.Player.IsFirstPerson.Get() || this.m_WeaponWasInAttackStateLastFrame)
		{
			base.ShowMuzzleFlash();
			return;
		}
		this.m_MuzzleFlashWeaponAngle = base.Transform.eulerAngles.x + 90f;
		this.m_MuzzleFlashFireAngle = this.m_CurrentFireRotation.eulerAngles.x + 90f;
		this.m_MuzzleFlashWeaponAngle = ((this.m_MuzzleFlashWeaponAngle >= 360f) ? (this.m_MuzzleFlashWeaponAngle - 360f) : this.m_MuzzleFlashWeaponAngle);
		this.m_MuzzleFlashFireAngle = ((this.m_MuzzleFlashFireAngle >= 360f) ? (this.m_MuzzleFlashFireAngle - 360f) : this.m_MuzzleFlashFireAngle);
		if (Mathf.Abs(this.m_MuzzleFlashWeaponAngle - this.m_MuzzleFlashFireAngle) > this.MuzzleFlashFirstShotMaxDeviation)
		{
			this.m_MuzzleFlash.SendMessage("ShootLightOnly", SendMessageOptions.DontRequireReceiver);
			return;
		}
		base.ShowMuzzleFlash();
	}

	// Token: 0x0600B2B6 RID: 45750 RVA: 0x0042B73C File Offset: 0x0042993C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void ApplyRecoil()
	{
		if (this.MotionRotationRecoil.z == 0f)
		{
			this.Weapon.AddForce2(this.MotionPositionRecoil, this.MotionRotationRecoil);
			return;
		}
		this.Weapon.AddForce2(this.MotionPositionRecoil, Vector3.Scale(this.MotionRotationRecoil, Vector3.one + Vector3.back) + ((global::UnityEngine.Random.value < 0.5f) ? Vector3.forward : Vector3.back) * global::UnityEngine.Random.Range(this.MotionRotationRecoil.z * this.MotionRotationRecoilDeadZone, this.MotionRotationRecoil.z));
	}

	// Token: 0x0600B2B7 RID: 45751 RVA: 0x0042B7E4 File Offset: 0x004299E4
	public virtual void DryFire()
	{
		if (base.Audio != null)
		{
			base.Audio.pitch = Time.timeScale;
			base.Audio.PlayOneShot(this.SoundDryFire);
		}
		this.DisableFiring(10000000f);
		this.m_LastFireTime = Time.time;
		this.Weapon.AddForce2(this.MotionPositionRecoil * this.MotionDryFireRecoil, this.MotionRotationRecoil * this.MotionDryFireRecoil);
	}

	// Token: 0x0600B2B8 RID: 45752 RVA: 0x0042B863 File Offset: 0x00429A63
	public void OnMessage_DryFire()
	{
		this.DryFire();
	}

	// Token: 0x0600B2B9 RID: 45753 RVA: 0x0042B86B File Offset: 0x00429A6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		if (this.ProjectileFiringRate == 0f)
		{
			this.EnableFiring();
			return;
		}
		this.DisableFiring(this.ProjectileTapFiringRate - (Time.time - this.m_LastFireTime));
	}

	// Token: 0x0600B2BA RID: 45754 RVA: 0x0042B89A File Offset: 0x00429A9A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_Fire()
	{
		if (Time.time < this.m_NextAllowedFireTime)
		{
			return false;
		}
		if (!this.Player.DepleteAmmo.Try())
		{
			this.DryFire();
			return false;
		}
		this.Fire();
		return true;
	}

	// Token: 0x04008626 RID: 34342
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x04008627 RID: 34343
	public float ProjectileTapFiringRate = 0.1f;

	// Token: 0x04008628 RID: 34344
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastFireTime;

	// Token: 0x04008629 RID: 34345
	public Vector3 MotionPositionRecoil = new Vector3(0f, 0f, -0.035f);

	// Token: 0x0400862A RID: 34346
	public Vector3 MotionRotationRecoil = new Vector3(-10f, 0f, 0f);

	// Token: 0x0400862B RID: 34347
	public float MotionRotationRecoilDeadZone = 0.5f;

	// Token: 0x0400862C RID: 34348
	public float MotionDryFireRecoil = -0.1f;

	// Token: 0x0400862D RID: 34349
	public float MotionRecoilDelay;

	// Token: 0x0400862E RID: 34350
	public float MuzzleFlashFirstShotMaxDeviation = 180f;

	// Token: 0x0400862F RID: 34351
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WeaponWasInAttackStateLastFrame;

	// Token: 0x04008630 RID: 34352
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashWeaponAngle;

	// Token: 0x04008631 RID: 34353
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashFireAngle;

	// Token: 0x04008632 RID: 34354
	public AudioClip SoundDryFire;

	// Token: 0x04008633 RID: 34355
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_MuzzlePointRotation = Quaternion.identity;

	// Token: 0x04008634 RID: 34356
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_3rdPersonFiredThisFrame;

	// Token: 0x04008635 RID: 34357
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
