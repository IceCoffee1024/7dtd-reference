using System;
using UnityEngine;

// Token: 0x02001635 RID: 5685
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponShooter : vp_WeaponShooter
{
	// Token: 0x170014DE RID: 5342
	// (get) Token: 0x0600B1AD RID: 45485 RVA: 0x00425A75 File Offset: 0x00423C75
	public new vp_FPPlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)base.EventHandler;
			}
			return (vp_FPPlayerEventHandler)this.m_Player;
		}
	}

	// Token: 0x170014DF RID: 5343
	// (get) Token: 0x0600B1AE RID: 45486 RVA: 0x00425AAF File Offset: 0x00423CAF
	public new vp_FPWeapon Weapon
	{
		get
		{
			if (this.m_FPWeapon == null)
			{
				this.m_FPWeapon = base.transform.GetComponent<vp_FPWeapon>();
			}
			return this.m_FPWeapon;
		}
	}

	// Token: 0x0600B1AF RID: 45487 RVA: 0x00425AD8 File Offset: 0x00423CD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.m_FPCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		if (this.m_ProjectileSpawnPoint == null)
		{
			this.m_ProjectileSpawnPoint = this.m_FPCamera.gameObject;
		}
		this.m_ProjectileDefaultSpawnpoint = this.m_ProjectileSpawnPoint;
		this.m_NextAllowedFireTime = Time.time;
		this.ProjectileSpawnDelay = Mathf.Min(this.ProjectileSpawnDelay, this.ProjectileFiringRate - 0.1f);
	}

	// Token: 0x0600B1B0 RID: 45488 RVA: 0x00425B54 File Offset: 0x00423D54
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshFirePoint();
		base.OnEnable();
	}

	// Token: 0x0600B1B1 RID: 45489 RVA: 0x00425B62 File Offset: 0x00423D62
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600B1B2 RID: 45490 RVA: 0x00425B6C File Offset: 0x00423D6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.ProjectileFiringRate == 0f && this.AnimationFire != null)
		{
			this.ProjectileFiringRate = this.AnimationFire.length;
		}
		if (this.ProjectileFiringRate == 0f && this.AnimationFire != null)
		{
			this.ProjectileFiringRate = this.AnimationFire.length;
		}
	}

	// Token: 0x0600B1B3 RID: 45491 RVA: 0x00425BD8 File Offset: 0x00423DD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Fire()
	{
		this.m_LastFireTime = Time.time;
		if (this.AnimationFire != null)
		{
			if (this.Weapon.WeaponModel.GetComponent<Animation>()[this.AnimationFire.name] == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error (",
					(this != null) ? this.ToString() : null,
					") No animation named '",
					this.AnimationFire.name,
					"' is listed in this prefab. Make sure the prefab has an 'Animation' component which references all the clips you wish to play on the weapon."
				}));
			}
			else
			{
				this.Weapon.WeaponModel.GetComponent<Animation>()[this.AnimationFire.name].time = 0f;
				this.Weapon.WeaponModel.GetComponent<Animation>().Sample();
				this.Weapon.WeaponModel.GetComponent<Animation>().Play(this.AnimationFire.name);
			}
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

	// Token: 0x0600B1B4 RID: 45492 RVA: 0x00425D08 File Offset: 0x00423F08
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ApplyRecoil()
	{
		this.Weapon.ResetSprings(this.MotionPositionReset, this.MotionRotationReset, this.MotionPositionPause, this.MotionRotationPause);
		if (this.MotionRotationRecoil.z == 0f)
		{
			this.Weapon.AddForce2(this.MotionPositionRecoil, this.MotionRotationRecoil);
			if (this.MotionPositionRecoilCameraFactor != 0f)
			{
				this.m_FPCamera.AddForce2(this.MotionPositionRecoil * this.MotionPositionRecoilCameraFactor);
				return;
			}
		}
		else
		{
			this.Weapon.AddForce2(this.MotionPositionRecoil, Vector3.Scale(this.MotionRotationRecoil, Vector3.one + Vector3.back) + ((global::UnityEngine.Random.value < 0.5f) ? Vector3.forward : Vector3.back) * global::UnityEngine.Random.Range(this.MotionRotationRecoil.z * this.MotionRotationRecoilDeadZone, this.MotionRotationRecoil.z));
			if (this.MotionPositionRecoilCameraFactor != 0f)
			{
				this.m_FPCamera.AddForce2(this.MotionPositionRecoil * this.MotionPositionRecoilCameraFactor);
			}
			if (this.MotionRotationRecoilCameraFactor != 0f)
			{
				this.m_FPCamera.AddRollForce(global::UnityEngine.Random.Range(this.MotionRotationRecoil.z * this.MotionRotationRecoilDeadZone, this.MotionRotationRecoil.z) * this.MotionRotationRecoilCameraFactor * ((global::UnityEngine.Random.value < 0.5f) ? 1f : (-1f)));
			}
		}
	}

	// Token: 0x0600B1B5 RID: 45493 RVA: 0x00425E80 File Offset: 0x00424080
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshFirePoint();
	}

	// Token: 0x0600B1B6 RID: 45494 RVA: 0x00425E88 File Offset: 0x00424088
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshFirePoint()
	{
		if (this.Player.IsFirstPerson == null)
		{
			return;
		}
		if (this.Player.IsFirstPerson.Get())
		{
			this.m_ProjectileSpawnPoint = this.m_FPCamera.gameObject;
			if (base.MuzzleFlash != null)
			{
				base.MuzzleFlash.layer = 10;
			}
			this.m_MuzzleFlashSpawnPoint = null;
			this.m_ShellEjectSpawnPoint = null;
			this.Refresh();
		}
		else
		{
			this.m_ProjectileSpawnPoint = this.m_ProjectileDefaultSpawnpoint;
			if (base.MuzzleFlash != null)
			{
				base.MuzzleFlash.layer = 0;
			}
			this.m_MuzzleFlashSpawnPoint = null;
			this.m_ShellEjectSpawnPoint = null;
			this.Refresh();
		}
		if (this.Player.CurrentWeaponName.Get() != base.name)
		{
			this.m_ProjectileSpawnPoint = this.m_FPCamera.gameObject;
		}
	}

	// Token: 0x0400850F RID: 34063
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x04008510 RID: 34064
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008511 RID: 34065
	public float MotionPositionReset = 0.5f;

	// Token: 0x04008512 RID: 34066
	public float MotionRotationReset = 0.5f;

	// Token: 0x04008513 RID: 34067
	public float MotionPositionPause = 1f;

	// Token: 0x04008514 RID: 34068
	public float MotionRotationPause = 1f;

	// Token: 0x04008515 RID: 34069
	public float MotionRotationRecoilCameraFactor;

	// Token: 0x04008516 RID: 34070
	public float MotionPositionRecoilCameraFactor;

	// Token: 0x04008517 RID: 34071
	public AnimationClip AnimationFire;
}
