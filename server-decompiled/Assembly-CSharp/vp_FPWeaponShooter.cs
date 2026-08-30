using System;
using UnityEngine;

// Token: 0x02001604 RID: 5636
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponShooter : vp_WeaponShooter
{
	// Token: 0x1700148F RID: 5263
	// (get) Token: 0x0600B0C3 RID: 45251 RVA: 0x0041F995 File Offset: 0x0041DB95
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

	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x0600B0C4 RID: 45252 RVA: 0x0041F9CF File Offset: 0x0041DBCF
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

	// Token: 0x0600B0C5 RID: 45253 RVA: 0x0041F9F8 File Offset: 0x0041DBF8
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

	// Token: 0x0600B0C6 RID: 45254 RVA: 0x0041FA74 File Offset: 0x0041DC74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshFirePoint();
		base.OnEnable();
	}

	// Token: 0x0600B0C7 RID: 45255 RVA: 0x0041FA82 File Offset: 0x0041DC82
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600B0C8 RID: 45256 RVA: 0x0041FA8C File Offset: 0x0041DC8C
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

	// Token: 0x0600B0C9 RID: 45257 RVA: 0x0041FAF8 File Offset: 0x0041DCF8
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

	// Token: 0x0600B0CA RID: 45258 RVA: 0x0041FC28 File Offset: 0x0041DE28
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

	// Token: 0x0600B0CB RID: 45259 RVA: 0x0041FDA0 File Offset: 0x0041DFA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraToggle3rdPerson()
	{
		this.RefreshFirePoint();
	}

	// Token: 0x0600B0CC RID: 45260 RVA: 0x0041FDA8 File Offset: 0x0041DFA8
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

	// Token: 0x04008478 RID: 33912
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x04008479 RID: 33913
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x0400847A RID: 33914
	public float MotionPositionReset = 0.5f;

	// Token: 0x0400847B RID: 33915
	public float MotionRotationReset = 0.5f;

	// Token: 0x0400847C RID: 33916
	public float MotionPositionPause = 1f;

	// Token: 0x0400847D RID: 33917
	public float MotionRotationPause = 1f;

	// Token: 0x0400847E RID: 33918
	public float MotionRotationRecoilCameraFactor;

	// Token: 0x0400847F RID: 33919
	public float MotionPositionRecoilCameraFactor;

	// Token: 0x04008480 RID: 33920
	public AnimationClip AnimationFire;
}
