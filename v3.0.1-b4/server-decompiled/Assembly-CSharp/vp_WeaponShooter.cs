using System;
using UnityEngine;

// Token: 0x020015FB RID: 5627
public class vp_WeaponShooter : vp_Shooter
{
	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x0600B0C8 RID: 45256 RVA: 0x0041FFD8 File Offset: 0x0041E1D8
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

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x0600B0C9 RID: 45257 RVA: 0x0042000D File Offset: 0x0041E20D
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

	// Token: 0x0600B0CA RID: 45258 RVA: 0x00420034 File Offset: 0x0041E234
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

	// Token: 0x0600B0CB RID: 45259 RVA: 0x00420102 File Offset: 0x0041E302
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600B0CC RID: 45260 RVA: 0x0042010C File Offset: 0x0041E30C
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

	// Token: 0x0600B0CD RID: 45261 RVA: 0x00420180 File Offset: 0x0041E380
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

	// Token: 0x0600B0CE RID: 45262 RVA: 0x00420214 File Offset: 0x0041E414
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

	// Token: 0x0600B0CF RID: 45263 RVA: 0x0042031C File Offset: 0x0041E51C
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

	// Token: 0x0600B0D0 RID: 45264 RVA: 0x004203C4 File Offset: 0x0041E5C4
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

	// Token: 0x0600B0D1 RID: 45265 RVA: 0x00420443 File Offset: 0x0041E643
	public void OnMessage_DryFire()
	{
		this.DryFire();
	}

	// Token: 0x0600B0D2 RID: 45266 RVA: 0x0042044B File Offset: 0x0041E64B
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

	// Token: 0x0600B0D3 RID: 45267 RVA: 0x0042047A File Offset: 0x0041E67A
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

	// Token: 0x040084B6 RID: 33974
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040084B7 RID: 33975
	public float ProjectileTapFiringRate = 0.1f;

	// Token: 0x040084B8 RID: 33976
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastFireTime;

	// Token: 0x040084B9 RID: 33977
	public Vector3 MotionPositionRecoil = new Vector3(0f, 0f, -0.035f);

	// Token: 0x040084BA RID: 33978
	public Vector3 MotionRotationRecoil = new Vector3(-10f, 0f, 0f);

	// Token: 0x040084BB RID: 33979
	public float MotionRotationRecoilDeadZone = 0.5f;

	// Token: 0x040084BC RID: 33980
	public float MotionDryFireRecoil = -0.1f;

	// Token: 0x040084BD RID: 33981
	public float MotionRecoilDelay;

	// Token: 0x040084BE RID: 33982
	public float MuzzleFlashFirstShotMaxDeviation = 180f;

	// Token: 0x040084BF RID: 33983
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_WeaponWasInAttackStateLastFrame;

	// Token: 0x040084C0 RID: 33984
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashWeaponAngle;

	// Token: 0x040084C1 RID: 33985
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MuzzleFlashFireAngle;

	// Token: 0x040084C2 RID: 33986
	public AudioClip SoundDryFire;

	// Token: 0x040084C3 RID: 33987
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_MuzzlePointRotation = Quaternion.identity;

	// Token: 0x040084C4 RID: 33988
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_3rdPersonFiredThisFrame;

	// Token: 0x040084C5 RID: 33989
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
