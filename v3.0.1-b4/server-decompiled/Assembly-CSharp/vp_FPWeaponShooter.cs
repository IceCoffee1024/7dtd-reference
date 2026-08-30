using System;
using UnityEngine;

// Token: 0x020015E9 RID: 5609
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponShooter : vp_WeaponShooter
{
	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x0600AFC6 RID: 44998 RVA: 0x0041A655 File Offset: 0x00418855
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

	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x0600AFC7 RID: 44999 RVA: 0x0041A68F File Offset: 0x0041888F
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

	// Token: 0x0600AFC8 RID: 45000 RVA: 0x0041A6B8 File Offset: 0x004188B8
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

	// Token: 0x0600AFC9 RID: 45001 RVA: 0x0041A734 File Offset: 0x00418934
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshFirePoint();
		base.OnEnable();
	}

	// Token: 0x0600AFCA RID: 45002 RVA: 0x0041A742 File Offset: 0x00418942
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600AFCB RID: 45003 RVA: 0x0041A74C File Offset: 0x0041894C
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

	// Token: 0x0600AFCC RID: 45004 RVA: 0x0041A7B8 File Offset: 0x004189B8
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

	// Token: 0x0600AFCD RID: 45005 RVA: 0x0041A8E8 File Offset: 0x00418AE8
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

	// Token: 0x0600AFCE RID: 45006 RVA: 0x0041AA60 File Offset: 0x00418C60
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshFirePoint();
	}

	// Token: 0x0600AFCF RID: 45007 RVA: 0x0041AA68 File Offset: 0x00418C68
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

	// Token: 0x0400839F RID: 33695
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x040083A0 RID: 33696
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040083A1 RID: 33697
	public float MotionPositionReset = 0.5f;

	// Token: 0x040083A2 RID: 33698
	public float MotionRotationReset = 0.5f;

	// Token: 0x040083A3 RID: 33699
	public float MotionPositionPause = 1f;

	// Token: 0x040083A4 RID: 33700
	public float MotionRotationPause = 1f;

	// Token: 0x040083A5 RID: 33701
	public float MotionRotationRecoilCameraFactor;

	// Token: 0x040083A6 RID: 33702
	public float MotionPositionRecoilCameraFactor;

	// Token: 0x040083A7 RID: 33703
	public AnimationClip AnimationFire;
}
