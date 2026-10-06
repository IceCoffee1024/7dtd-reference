using System;
using UnityEngine;

// Token: 0x020015D3 RID: 5587
[RequireComponent(typeof(AudioSource))]
public class vp_Shooter : vp_Component
{
	// Token: 0x1700145E RID: 5214
	// (get) Token: 0x0600AE83 RID: 44675 RVA: 0x0041154D File Offset: 0x0040F74D
	public GameObject ProjectileSpawnPoint
	{
		get
		{
			return this.m_ProjectileSpawnPoint;
		}
	}

	// Token: 0x1700145F RID: 5215
	// (get) Token: 0x0600AE84 RID: 44676 RVA: 0x00411558 File Offset: 0x0040F758
	public GameObject MuzzleFlash
	{
		get
		{
			if (this.m_MuzzleFlash == null && this.MuzzleFlashPrefab != null && this.ProjectileSpawnPoint != null)
			{
				this.m_MuzzleFlash = (GameObject)vp_Utility.Instantiate(this.MuzzleFlashPrefab, this.ProjectileSpawnPoint.transform.position, this.ProjectileSpawnPoint.transform.rotation);
				this.m_MuzzleFlash.name = base.transform.name + "MuzzleFlash";
				this.m_MuzzleFlash.transform.parent = this.ProjectileSpawnPoint.transform;
			}
			return this.m_MuzzleFlash;
		}
	}

	// Token: 0x0600AE85 RID: 44677 RVA: 0x0041160C File Offset: 0x0040F80C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.m_ProjectileSpawnPoint == null)
		{
			this.m_ProjectileSpawnPoint = base.gameObject;
		}
		this.m_ProjectileDefaultSpawnpoint = this.m_ProjectileSpawnPoint;
		if (this.GetFirePosition == null)
		{
			this.GetFirePosition = () => this.FirePosition;
		}
		if (this.GetFireRotation == null)
		{
			this.GetFireRotation = () => this.m_ProjectileSpawnPoint.transform.rotation;
		}
		if (this.GetFireSeed == null)
		{
			this.GetFireSeed = () => global::UnityEngine.Random.Range(0, 100);
		}
		this.m_CharacterController = this.m_ProjectileSpawnPoint.transform.root.GetComponentInChildren<CharacterController>();
		this.m_NextAllowedFireTime = Time.time;
		this.ProjectileSpawnDelay = Mathf.Min(this.ProjectileSpawnDelay, this.ProjectileFiringRate - 0.1f);
	}

	// Token: 0x0600AE86 RID: 44678 RVA: 0x004116E9 File Offset: 0x0040F8E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		base.Audio.playOnAwake = false;
		base.Audio.dopplerLevel = 0f;
		base.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600AE87 RID: 44679 RVA: 0x00411719 File Offset: 0x0040F919
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		this.FirePosition = this.m_ProjectileSpawnPoint.transform.position;
	}

	// Token: 0x0600AE88 RID: 44680 RVA: 0x00411731 File Offset: 0x0040F931
	public virtual bool CanFire()
	{
		return Time.time >= this.m_NextAllowedFireTime;
	}

	// Token: 0x0600AE89 RID: 44681 RVA: 0x00411743 File Offset: 0x0040F943
	public virtual bool TryFire()
	{
		if (Time.time < this.m_NextAllowedFireTime)
		{
			return false;
		}
		this.Fire();
		return true;
	}

	// Token: 0x0600AE8A RID: 44682 RVA: 0x0041175C File Offset: 0x0040F95C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Fire()
	{
		this.m_NextAllowedFireTime = Time.time + this.ProjectileFiringRate;
		if (this.SoundFireDelay == 0f)
		{
			this.PlayFireSound();
		}
		else
		{
			vp_Timer.In(this.SoundFireDelay, new vp_Timer.Callback(this.PlayFireSound), null);
		}
		if (this.ProjectileSpawnDelay == 0f)
		{
			this.SpawnProjectiles();
		}
		else
		{
			vp_Timer.In(this.ProjectileSpawnDelay, delegate
			{
				this.SpawnProjectiles();
			}, null);
		}
		if (this.ShellEjectDelay == 0f)
		{
			this.EjectShell();
		}
		else
		{
			vp_Timer.In(this.ShellEjectDelay, new vp_Timer.Callback(this.EjectShell), null);
		}
		if (this.MuzzleFlashDelay == 0f)
		{
			this.ShowMuzzleFlash();
			return;
		}
		vp_Timer.In(this.MuzzleFlashDelay, new vp_Timer.Callback(this.ShowMuzzleFlash), null);
	}

	// Token: 0x0600AE8B RID: 44683 RVA: 0x00411834 File Offset: 0x0040FA34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void PlayFireSound()
	{
		if (base.Audio == null)
		{
			return;
		}
		base.Audio.pitch = global::UnityEngine.Random.Range(this.SoundFirePitch.x, this.SoundFirePitch.y) * Time.timeScale;
		base.Audio.clip = this.SoundFire;
		base.Audio.Play();
	}

	// Token: 0x0600AE8C RID: 44684 RVA: 0x00411898 File Offset: 0x0040FA98
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SpawnProjectiles()
	{
		if (this.ProjectilePrefab == null)
		{
			return;
		}
		if (this.m_SendFireEventToNetworkFunc != null)
		{
			this.m_SendFireEventToNetworkFunc();
		}
		this.m_CurrentFirePosition = this.GetFirePosition();
		this.m_CurrentFireRotation = this.GetFireRotation();
		this.m_CurrentFireSeed = this.GetFireSeed();
		for (int i = 0; i < this.ProjectileCount; i++)
		{
			GameObject gameObject = (GameObject)vp_Utility.Instantiate(this.ProjectilePrefab, this.m_CurrentFirePosition, this.m_CurrentFireRotation);
			gameObject.SendMessage("SetSource", this.ProjectileSourceIsRoot ? base.Root : base.Transform, SendMessageOptions.DontRequireReceiver);
			gameObject.transform.localScale = new Vector3(this.ProjectileScale, this.ProjectileScale, this.ProjectileScale);
			this.SetSpread(this.m_CurrentFireSeed * (i + 1), gameObject.transform);
		}
	}

	// Token: 0x0600AE8D RID: 44685 RVA: 0x00411988 File Offset: 0x0040FB88
	public void SetSpread(int seed, Transform target)
	{
		global::UnityEngine.Random.InitState(seed);
		target.Rotate(0f, 0f, (float)global::UnityEngine.Random.Range(0, 360));
		target.Rotate(0f, global::UnityEngine.Random.Range(-this.ProjectileSpread, this.ProjectileSpread), 0f);
	}

	// Token: 0x0600AE8E RID: 44686 RVA: 0x004119DC File Offset: 0x0040FBDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void ShowMuzzleFlash()
	{
		if (this.MuzzleFlash == null)
		{
			return;
		}
		if (this.m_MuzzleFlashSpawnPoint != null && this.ProjectileSpawnPoint != null)
		{
			this.MuzzleFlash.transform.position = this.m_MuzzleFlashSpawnPoint.transform.position;
			this.MuzzleFlash.transform.rotation = this.ProjectileSpawnPoint.transform.rotation;
		}
		this.MuzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600AE8F RID: 44687 RVA: 0x00411A68 File Offset: 0x0040FC68
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void EjectShell()
	{
		if (this.ShellPrefab == null)
		{
			return;
		}
		GameObject gameObject = (GameObject)vp_Utility.Instantiate(this.ShellPrefab, (this.m_ShellEjectSpawnPoint == null) ? (this.FirePosition + this.m_ProjectileSpawnPoint.transform.TransformDirection(this.ShellEjectPosition)) : this.m_ShellEjectSpawnPoint.transform.position, this.m_ProjectileSpawnPoint.transform.rotation);
		gameObject.transform.localScale = new Vector3(this.ShellScale, this.ShellScale, this.ShellScale);
		vp_Layer.Set(gameObject.gameObject, 29, false);
		if (gameObject.GetComponent<Rigidbody>())
		{
			Vector3 vector = ((this.m_ShellEjectSpawnPoint == null) ? (base.transform.TransformDirection(this.ShellEjectDirection).normalized * this.ShellEjectVelocity) : (this.m_ShellEjectSpawnPoint.transform.forward.normalized * this.ShellEjectVelocity));
			gameObject.GetComponent<Rigidbody>().AddForce(vector, ForceMode.Impulse);
		}
		if (this.m_CharacterController)
		{
			Vector3 velocity = this.m_CharacterController.velocity;
			gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
		}
		if (this.ShellEjectSpin > 0f)
		{
			if (global::UnityEngine.Random.value > 0.5f)
			{
				gameObject.GetComponent<Rigidbody>().AddRelativeTorque(-global::UnityEngine.Random.rotation.eulerAngles * this.ShellEjectSpin);
				return;
			}
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(global::UnityEngine.Random.rotation.eulerAngles * this.ShellEjectSpin);
		}
	}

	// Token: 0x0600AE90 RID: 44688 RVA: 0x00411C17 File Offset: 0x0040FE17
	public virtual void DisableFiring(float seconds = 10000000f)
	{
		this.m_NextAllowedFireTime = Time.time + seconds;
	}

	// Token: 0x0600AE91 RID: 44689 RVA: 0x00411C26 File Offset: 0x0040FE26
	public virtual void EnableFiring()
	{
		this.m_NextAllowedFireTime = Time.time;
	}

	// Token: 0x0600AE92 RID: 44690 RVA: 0x00411C34 File Offset: 0x0040FE34
	public override void Refresh()
	{
		if (this.MuzzleFlash != null)
		{
			if (this.m_MuzzleFlashSpawnPoint == null)
			{
				if (this.ProjectileSpawnPoint == this.m_ProjectileDefaultSpawnpoint)
				{
					this.m_MuzzleFlashSpawnPoint = vp_Utility.GetTransformByNameInChildren(this.ProjectileSpawnPoint.transform, "muzzle", false, false);
				}
				else
				{
					this.m_MuzzleFlashSpawnPoint = vp_Utility.GetTransformByNameInChildren(base.Transform, "muzzle", false, false);
				}
			}
			if (this.m_MuzzleFlashSpawnPoint != null)
			{
				if (this.ProjectileSpawnPoint == this.m_ProjectileDefaultSpawnpoint)
				{
					this.m_MuzzleFlash.transform.parent = this.ProjectileSpawnPoint.transform.parent.parent.parent;
				}
				else
				{
					this.m_MuzzleFlash.transform.parent = this.ProjectileSpawnPoint.transform;
				}
			}
			else
			{
				this.m_MuzzleFlash.transform.parent = this.ProjectileSpawnPoint.transform;
				this.MuzzleFlash.transform.localPosition = this.MuzzleFlashPosition;
				this.MuzzleFlash.transform.rotation = this.ProjectileSpawnPoint.transform.rotation;
			}
			this.MuzzleFlash.transform.localScale = this.MuzzleFlashScale;
			this.MuzzleFlash.SendMessage("SetFadeSpeed", this.MuzzleFlashFadeSpeed, SendMessageOptions.DontRequireReceiver);
		}
		if (this.ShellPrefab != null && this.m_ShellEjectSpawnPoint == null && this.ProjectileSpawnPoint != null)
		{
			if (this.ProjectileSpawnPoint == this.m_ProjectileDefaultSpawnpoint)
			{
				this.m_ShellEjectSpawnPoint = vp_Utility.GetTransformByNameInChildren(this.ProjectileSpawnPoint.transform, "shell", false, false);
				return;
			}
			this.m_ShellEjectSpawnPoint = vp_Utility.GetTransformByNameInChildren(base.Transform, "shell", false, false);
		}
	}

	// Token: 0x0600AE93 RID: 44691 RVA: 0x00411E0A File Offset: 0x0041000A
	public override void Activate()
	{
		base.Activate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, true);
		}
	}

	// Token: 0x0600AE94 RID: 44692 RVA: 0x00411E2C File Offset: 0x0041002C
	public override void Deactivate()
	{
		base.Deactivate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, false);
		}
	}

	// Token: 0x0600AE95 RID: 44693 RVA: 0x00411E50 File Offset: 0x00410050
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DrawProjectileDebugInfo(int projectileIndex)
	{
		GameObject gameObject = vp_3DUtility.DebugPointer(null);
		gameObject.transform.rotation = this.GetFireRotation();
		gameObject.transform.position = this.GetFirePosition();
		GameObject gameObject2 = vp_3DUtility.DebugBall(null);
		RaycastHit raycastHit;
		if (Physics.Linecast(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 1000f, out raycastHit, 1084850184) && !raycastHit.collider.isTrigger && base.Root.InverseTransformPoint(raycastHit.point).z > 0f)
		{
			gameObject2.transform.position = raycastHit.point;
		}
	}

	// Token: 0x040081DF RID: 33247
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x040081E0 RID: 33248
	public GameObject m_ProjectileSpawnPoint;

	// Token: 0x040081E1 RID: 33249
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_ProjectileDefaultSpawnpoint;

	// Token: 0x040081E2 RID: 33250
	public GameObject ProjectilePrefab;

	// Token: 0x040081E3 RID: 33251
	public float ProjectileScale = 1f;

	// Token: 0x040081E4 RID: 33252
	public float ProjectileFiringRate = 0.3f;

	// Token: 0x040081E5 RID: 33253
	public float ProjectileSpawnDelay;

	// Token: 0x040081E6 RID: 33254
	public int ProjectileCount = 1;

	// Token: 0x040081E7 RID: 33255
	public float ProjectileSpread;

	// Token: 0x040081E8 RID: 33256
	public bool ProjectileSourceIsRoot = true;

	// Token: 0x040081E9 RID: 33257
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedFireTime;

	// Token: 0x040081EA RID: 33258
	public Vector3 MuzzleFlashPosition = Vector3.zero;

	// Token: 0x040081EB RID: 33259
	public Vector3 MuzzleFlashScale = Vector3.one;

	// Token: 0x040081EC RID: 33260
	public float MuzzleFlashFadeSpeed = 0.075f;

	// Token: 0x040081ED RID: 33261
	public GameObject MuzzleFlashPrefab;

	// Token: 0x040081EE RID: 33262
	public float MuzzleFlashDelay;

	// Token: 0x040081EF RID: 33263
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_MuzzleFlash;

	// Token: 0x040081F0 RID: 33264
	public Transform m_MuzzleFlashSpawnPoint;

	// Token: 0x040081F1 RID: 33265
	public GameObject ShellPrefab;

	// Token: 0x040081F2 RID: 33266
	public float ShellScale = 1f;

	// Token: 0x040081F3 RID: 33267
	public Vector3 ShellEjectDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x040081F4 RID: 33268
	public Vector3 ShellEjectPosition = new Vector3(1f, 0f, 1f);

	// Token: 0x040081F5 RID: 33269
	public float ShellEjectVelocity = 0.2f;

	// Token: 0x040081F6 RID: 33270
	public float ShellEjectDelay;

	// Token: 0x040081F7 RID: 33271
	public float ShellEjectSpin;

	// Token: 0x040081F8 RID: 33272
	public Transform m_ShellEjectSpawnPoint;

	// Token: 0x040081F9 RID: 33273
	public AudioClip SoundFire;

	// Token: 0x040081FA RID: 33274
	public float SoundFireDelay;

	// Token: 0x040081FB RID: 33275
	public Vector2 SoundFirePitch = new Vector2(1f, 1f);

	// Token: 0x040081FC RID: 33276
	public vp_Shooter.NetworkFunc m_SendFireEventToNetworkFunc;

	// Token: 0x040081FD RID: 33277
	public vp_Shooter.FirePositionFunc GetFirePosition;

	// Token: 0x040081FE RID: 33278
	public vp_Shooter.FireRotationFunc GetFireRotation;

	// Token: 0x040081FF RID: 33279
	public vp_Shooter.FireSeedFunc GetFireSeed;

	// Token: 0x04008200 RID: 33280
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentFirePosition = Vector3.zero;

	// Token: 0x04008201 RID: 33281
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_CurrentFireRotation = Quaternion.identity;

	// Token: 0x04008202 RID: 33282
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentFireSeed;

	// Token: 0x04008203 RID: 33283
	public Vector3 FirePosition = Vector3.zero;

	// Token: 0x020015D4 RID: 5588
	// (Invoke) Token: 0x0600AE9B RID: 44699
	public delegate void NetworkFunc();

	// Token: 0x020015D5 RID: 5589
	// (Invoke) Token: 0x0600AE9F RID: 44703
	public delegate Vector3 FirePositionFunc();

	// Token: 0x020015D6 RID: 5590
	// (Invoke) Token: 0x0600AEA3 RID: 44707
	public delegate Quaternion FireRotationFunc();

	// Token: 0x020015D7 RID: 5591
	// (Invoke) Token: 0x0600AEA7 RID: 44711
	public delegate int FireSeedFunc();
}
