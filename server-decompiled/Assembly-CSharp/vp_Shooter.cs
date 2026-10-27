using System;
using UnityEngine;

// Token: 0x020015E5 RID: 5605
[RequireComponent(typeof(AudioSource))]
public class vp_Shooter : vp_Component
{
	// Token: 0x1700146A RID: 5226
	// (get) Token: 0x0600AEDB RID: 44763 RVA: 0x00413951 File Offset: 0x00411B51
	public GameObject ProjectileSpawnPoint
	{
		get
		{
			return this.m_ProjectileSpawnPoint;
		}
	}

	// Token: 0x1700146B RID: 5227
	// (get) Token: 0x0600AEDC RID: 44764 RVA: 0x0041395C File Offset: 0x00411B5C
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

	// Token: 0x0600AEDD RID: 44765 RVA: 0x00413A10 File Offset: 0x00411C10
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

	// Token: 0x0600AEDE RID: 44766 RVA: 0x00413AED File Offset: 0x00411CED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		base.Audio.playOnAwake = false;
		base.Audio.dopplerLevel = 0f;
		base.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600AEDF RID: 44767 RVA: 0x00413B1D File Offset: 0x00411D1D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		this.FirePosition = this.m_ProjectileSpawnPoint.transform.position;
	}

	// Token: 0x0600AEE0 RID: 44768 RVA: 0x00413B35 File Offset: 0x00411D35
	public virtual bool CanFire()
	{
		return Time.time >= this.m_NextAllowedFireTime;
	}

	// Token: 0x0600AEE1 RID: 44769 RVA: 0x00413B47 File Offset: 0x00411D47
	public virtual bool TryFire()
	{
		if (Time.time < this.m_NextAllowedFireTime)
		{
			return false;
		}
		this.Fire();
		return true;
	}

	// Token: 0x0600AEE2 RID: 44770 RVA: 0x00413B60 File Offset: 0x00411D60
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

	// Token: 0x0600AEE3 RID: 44771 RVA: 0x00413C38 File Offset: 0x00411E38
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

	// Token: 0x0600AEE4 RID: 44772 RVA: 0x00413C9C File Offset: 0x00411E9C
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

	// Token: 0x0600AEE5 RID: 44773 RVA: 0x00413D8C File Offset: 0x00411F8C
	public void SetSpread(int seed, Transform target)
	{
		global::UnityEngine.Random.InitState(seed);
		target.Rotate(0f, 0f, (float)global::UnityEngine.Random.Range(0, 360));
		target.Rotate(0f, global::UnityEngine.Random.Range(-this.ProjectileSpread, this.ProjectileSpread), 0f);
	}

	// Token: 0x0600AEE6 RID: 44774 RVA: 0x00413DE0 File Offset: 0x00411FE0
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

	// Token: 0x0600AEE7 RID: 44775 RVA: 0x00413E6C File Offset: 0x0041206C
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

	// Token: 0x0600AEE8 RID: 44776 RVA: 0x0041401B File Offset: 0x0041221B
	public virtual void DisableFiring(float seconds = 10000000f)
	{
		this.m_NextAllowedFireTime = Time.time + seconds;
	}

	// Token: 0x0600AEE9 RID: 44777 RVA: 0x0041402A File Offset: 0x0041222A
	public virtual void EnableFiring()
	{
		this.m_NextAllowedFireTime = Time.time;
	}

	// Token: 0x0600AEEA RID: 44778 RVA: 0x00414038 File Offset: 0x00412238
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

	// Token: 0x0600AEEB RID: 44779 RVA: 0x0041420E File Offset: 0x0041240E
	public override void Activate()
	{
		base.Activate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, true);
		}
	}

	// Token: 0x0600AEEC RID: 44780 RVA: 0x00414230 File Offset: 0x00412430
	public override void Deactivate()
	{
		base.Deactivate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, false);
		}
	}

	// Token: 0x0600AEED RID: 44781 RVA: 0x00414254 File Offset: 0x00412454
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

	// Token: 0x04008220 RID: 33312
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x04008221 RID: 33313
	public GameObject m_ProjectileSpawnPoint;

	// Token: 0x04008222 RID: 33314
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_ProjectileDefaultSpawnpoint;

	// Token: 0x04008223 RID: 33315
	public GameObject ProjectilePrefab;

	// Token: 0x04008224 RID: 33316
	public float ProjectileScale = 1f;

	// Token: 0x04008225 RID: 33317
	public float ProjectileFiringRate = 0.3f;

	// Token: 0x04008226 RID: 33318
	public float ProjectileSpawnDelay;

	// Token: 0x04008227 RID: 33319
	public int ProjectileCount = 1;

	// Token: 0x04008228 RID: 33320
	public float ProjectileSpread;

	// Token: 0x04008229 RID: 33321
	public bool ProjectileSourceIsRoot = true;

	// Token: 0x0400822A RID: 33322
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedFireTime;

	// Token: 0x0400822B RID: 33323
	public Vector3 MuzzleFlashPosition = Vector3.zero;

	// Token: 0x0400822C RID: 33324
	public Vector3 MuzzleFlashScale = Vector3.one;

	// Token: 0x0400822D RID: 33325
	public float MuzzleFlashFadeSpeed = 0.075f;

	// Token: 0x0400822E RID: 33326
	public GameObject MuzzleFlashPrefab;

	// Token: 0x0400822F RID: 33327
	public float MuzzleFlashDelay;

	// Token: 0x04008230 RID: 33328
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_MuzzleFlash;

	// Token: 0x04008231 RID: 33329
	public Transform m_MuzzleFlashSpawnPoint;

	// Token: 0x04008232 RID: 33330
	public GameObject ShellPrefab;

	// Token: 0x04008233 RID: 33331
	public float ShellScale = 1f;

	// Token: 0x04008234 RID: 33332
	public Vector3 ShellEjectDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x04008235 RID: 33333
	public Vector3 ShellEjectPosition = new Vector3(1f, 0f, 1f);

	// Token: 0x04008236 RID: 33334
	public float ShellEjectVelocity = 0.2f;

	// Token: 0x04008237 RID: 33335
	public float ShellEjectDelay;

	// Token: 0x04008238 RID: 33336
	public float ShellEjectSpin;

	// Token: 0x04008239 RID: 33337
	public Transform m_ShellEjectSpawnPoint;

	// Token: 0x0400823A RID: 33338
	public AudioClip SoundFire;

	// Token: 0x0400823B RID: 33339
	public float SoundFireDelay;

	// Token: 0x0400823C RID: 33340
	public Vector2 SoundFirePitch = new Vector2(1f, 1f);

	// Token: 0x0400823D RID: 33341
	public vp_Shooter.NetworkFunc m_SendFireEventToNetworkFunc;

	// Token: 0x0400823E RID: 33342
	public vp_Shooter.FirePositionFunc GetFirePosition;

	// Token: 0x0400823F RID: 33343
	public vp_Shooter.FireRotationFunc GetFireRotation;

	// Token: 0x04008240 RID: 33344
	public vp_Shooter.FireSeedFunc GetFireSeed;

	// Token: 0x04008241 RID: 33345
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentFirePosition = Vector3.zero;

	// Token: 0x04008242 RID: 33346
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_CurrentFireRotation = Quaternion.identity;

	// Token: 0x04008243 RID: 33347
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentFireSeed;

	// Token: 0x04008244 RID: 33348
	public Vector3 FirePosition = Vector3.zero;

	// Token: 0x020015E6 RID: 5606
	// (Invoke) Token: 0x0600AEF3 RID: 44787
	public delegate void NetworkFunc();

	// Token: 0x020015E7 RID: 5607
	// (Invoke) Token: 0x0600AEF7 RID: 44791
	public delegate Vector3 FirePositionFunc();

	// Token: 0x020015E8 RID: 5608
	// (Invoke) Token: 0x0600AEFB RID: 44795
	public delegate Quaternion FireRotationFunc();

	// Token: 0x020015E9 RID: 5609
	// (Invoke) Token: 0x0600AEFF RID: 44799
	public delegate int FireSeedFunc();
}
