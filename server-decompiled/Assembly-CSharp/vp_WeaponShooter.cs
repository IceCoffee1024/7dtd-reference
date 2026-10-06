using System;
using UnityEngine;

// Token: 0x02001635 RID: 5685
public class vp_WeaponShooter : vp_Shooter
{
	// Token: 0x17001513 RID: 5395
	// (get) Token: 0x0600B257 RID: 45655 RVA: 0x00428FF4 File Offset: 0x004271F4
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

	// Token: 0x17001514 RID: 5396
	// (get) Token: 0x0600B258 RID: 45656 RVA: 0x00429029 File Offset: 0x00427229
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

	// Token: 0x0600B259 RID: 45657 RVA: 0x00429050 File Offset: 0x00427250
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

	// Token: 0x0600B25A RID: 45658 RVA: 0x0042911E File Offset: 0x0042731E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600B25B RID: 45659 RVA: 0x00429128 File Offset: 0x00427328
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

	// Token: 0x0600B25C RID: 45660 RVA: 0x0042919C File Offset: 0x0042739C
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

	// Token: 0x0600B25D RID: 45661 RVA: 0x00429230 File Offset: 0x00427430
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

	// Token: 0x0600B25E RID: 45662 RVA: 0x00429338 File Offset: 0x00427538
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

	// Token: 0x0600B25F RID: 45663 RVA: 0x004293E0 File Offset: 0x004275E0
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

	// Token: 0x0600B260 RID: 45664 RVA: 0x0042945F File Offset: 0x0042765F
	public void OnMessage_DryFire()
	{
		this.DryFire();
	}

	// Token: 0x0600B261 RID: 45665 RVA: 0x00429467 File Offset: 0x00427667
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

	// Token: 0x0600B262 RID: 45666 RVA: 0x00429496 File Offset: 0x00427696
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

	// Token: 0x040085E5 RID: 34277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040085E6 RID: 34278
	public float ProjectileTapFiringRate = 0.1f;

	// Token: 0x040085E7 RID: 34279
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastFireTime;

	// Token: 0x040085E8 RID: 34280
	public Vector3 MotionPositionRecoil = new Vector3(0f, 0f, -0.035f);

	// Token: 0x040085E9 RID: 34281
	public Vector3 MotionRotationRecoil = new Vector3(-10f, 0f, 0f);

	// Token: 0x040085EA RID: 34282
	public float MotionRotationRecoilDeadZone = 0.5f;

	// Token: 0x040085EB RID: 34283
	public float MotionDryFireRecoil = -0.1f;

	// Token: 0x040085EC RID: 34284
	public float MotionRecoilDelay;

	// Token: 0x040085ED RID: 34285
	public float MuzzleFlashFirstShotMaxDeviation = 180f;

	// Token: 0x040085EE RID: 34286
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WeaponWasInAttackStateLastFrame;

	// Token: 0x040085EF RID: 34287
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashWeaponAngle;

	// Token: 0x040085F0 RID: 34288
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashFireAngle;

	// Token: 0x040085F1 RID: 34289
	public AudioClip SoundDryFire;

	// Token: 0x040085F2 RID: 34290
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_MuzzlePointRotation = Quaternion.identity;

	// Token: 0x040085F3 RID: 34291
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_3rdPersonFiredThisFrame;

	// Token: 0x040085F4 RID: 34292
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
