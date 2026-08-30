using System;
using UnityEngine;

// Token: 0x02001616 RID: 5654
public class vp_WeaponShooter : vp_Shooter
{
	// Token: 0x170014D0 RID: 5328
	// (get) Token: 0x0600B1C5 RID: 45509 RVA: 0x00425318 File Offset: 0x00423518
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

	// Token: 0x170014D1 RID: 5329
	// (get) Token: 0x0600B1C6 RID: 45510 RVA: 0x0042534D File Offset: 0x0042354D
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

	// Token: 0x0600B1C7 RID: 45511 RVA: 0x00425374 File Offset: 0x00423574
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

	// Token: 0x0600B1C8 RID: 45512 RVA: 0x00425442 File Offset: 0x00423642
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600B1C9 RID: 45513 RVA: 0x0042544C File Offset: 0x0042364C
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

	// Token: 0x0600B1CA RID: 45514 RVA: 0x004254C0 File Offset: 0x004236C0
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

	// Token: 0x0600B1CB RID: 45515 RVA: 0x00425554 File Offset: 0x00423754
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

	// Token: 0x0600B1CC RID: 45516 RVA: 0x0042565C File Offset: 0x0042385C
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

	// Token: 0x0600B1CD RID: 45517 RVA: 0x00425704 File Offset: 0x00423904
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

	// Token: 0x0600B1CE RID: 45518 RVA: 0x00425783 File Offset: 0x00423983
	public void OnMessage_DryFire()
	{
		this.DryFire();
	}

	// Token: 0x0600B1CF RID: 45519 RVA: 0x0042578B File Offset: 0x0042398B
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

	// Token: 0x0600B1D0 RID: 45520 RVA: 0x004257BA File Offset: 0x004239BA
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

	// Token: 0x0400858F RID: 34191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x04008590 RID: 34192
	public float ProjectileTapFiringRate = 0.1f;

	// Token: 0x04008591 RID: 34193
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastFireTime;

	// Token: 0x04008592 RID: 34194
	public Vector3 MotionPositionRecoil = new Vector3(0f, 0f, -0.035f);

	// Token: 0x04008593 RID: 34195
	public Vector3 MotionRotationRecoil = new Vector3(-10f, 0f, 0f);

	// Token: 0x04008594 RID: 34196
	public float MotionRotationRecoilDeadZone = 0.5f;

	// Token: 0x04008595 RID: 34197
	public float MotionDryFireRecoil = -0.1f;

	// Token: 0x04008596 RID: 34198
	public float MotionRecoilDelay;

	// Token: 0x04008597 RID: 34199
	public float MuzzleFlashFirstShotMaxDeviation = 180f;

	// Token: 0x04008598 RID: 34200
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WeaponWasInAttackStateLastFrame;

	// Token: 0x04008599 RID: 34201
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashWeaponAngle;

	// Token: 0x0400859A RID: 34202
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashFireAngle;

	// Token: 0x0400859B RID: 34203
	public AudioClip SoundDryFire;

	// Token: 0x0400859C RID: 34204
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_MuzzlePointRotation = Quaternion.identity;

	// Token: 0x0400859D RID: 34205
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_3rdPersonFiredThisFrame;

	// Token: 0x0400859E RID: 34206
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
