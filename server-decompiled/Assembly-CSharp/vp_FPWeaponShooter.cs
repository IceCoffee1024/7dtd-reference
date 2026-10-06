using System;
using UnityEngine;

// Token: 0x02001623 RID: 5667
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponShooter : vp_WeaponShooter
{
	// Token: 0x170014D2 RID: 5330
	// (get) Token: 0x0600B155 RID: 45397 RVA: 0x00423671 File Offset: 0x00421871
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

	// Token: 0x170014D3 RID: 5331
	// (get) Token: 0x0600B156 RID: 45398 RVA: 0x004236AB File Offset: 0x004218AB
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

	// Token: 0x0600B157 RID: 45399 RVA: 0x004236D4 File Offset: 0x004218D4
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

	// Token: 0x0600B158 RID: 45400 RVA: 0x00423750 File Offset: 0x00421950
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshFirePoint();
		base.OnEnable();
	}

	// Token: 0x0600B159 RID: 45401 RVA: 0x0042375E File Offset: 0x0042195E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600B15A RID: 45402 RVA: 0x00423768 File Offset: 0x00421968
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

	// Token: 0x0600B15B RID: 45403 RVA: 0x004237D4 File Offset: 0x004219D4
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

	// Token: 0x0600B15C RID: 45404 RVA: 0x00423904 File Offset: 0x00421B04
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

	// Token: 0x0600B15D RID: 45405 RVA: 0x00423A7C File Offset: 0x00421C7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshFirePoint();
	}

	// Token: 0x0600B15E RID: 45406 RVA: 0x00423A84 File Offset: 0x00421C84
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

	// Token: 0x040084CE RID: 33998
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x040084CF RID: 33999
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040084D0 RID: 34000
	public float MotionPositionReset = 0.5f;

	// Token: 0x040084D1 RID: 34001
	public float MotionRotationReset = 0.5f;

	// Token: 0x040084D2 RID: 34002
	public float MotionPositionPause = 1f;

	// Token: 0x040084D3 RID: 34003
	public float MotionRotationPause = 1f;

	// Token: 0x040084D4 RID: 34004
	public float MotionRotationRecoilCameraFactor;

	// Token: 0x040084D5 RID: 34005
	public float MotionPositionRecoilCameraFactor;

	// Token: 0x040084D6 RID: 34006
	public AnimationClip AnimationFire;
}
