using System;
using UnityEngine;

// Token: 0x020015B1 RID: 5553
[RequireComponent(typeof(AudioSource))]
public class vp_Shooter : vp_Component
{
	// Token: 0x17001419 RID: 5145
	// (get) Token: 0x0600ADD0 RID: 44496 RVA: 0x0040C6F9 File Offset: 0x0040A8F9
	public GameObject ProjectileSpawnPoint
	{
		get
		{
			return this.m_ProjectileSpawnPoint;
		}
	}

	// Token: 0x1700141A RID: 5146
	// (get) Token: 0x0600ADD1 RID: 44497 RVA: 0x0040C704 File Offset: 0x0040A904
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

	// Token: 0x0600ADD2 RID: 44498 RVA: 0x0040C7B8 File Offset: 0x0040A9B8
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

	// Token: 0x0600ADD3 RID: 44499 RVA: 0x0040C895 File Offset: 0x0040AA95
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		base.Audio.playOnAwake = false;
		base.Audio.dopplerLevel = 0f;
		base.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600ADD4 RID: 44500 RVA: 0x0040C8C5 File Offset: 0x0040AAC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		this.FirePosition = this.m_ProjectileSpawnPoint.transform.position;
	}

	// Token: 0x0600ADD5 RID: 44501 RVA: 0x0040C8DD File Offset: 0x0040AADD
	public virtual bool CanFire()
	{
		return Time.time >= this.m_NextAllowedFireTime;
	}

	// Token: 0x0600ADD6 RID: 44502 RVA: 0x0040C8EF File Offset: 0x0040AAEF
	public virtual bool TryFire()
	{
		if (Time.time < this.m_NextAllowedFireTime)
		{
			return false;
		}
		this.Fire();
		return true;
	}

	// Token: 0x0600ADD7 RID: 44503 RVA: 0x0040C908 File Offset: 0x0040AB08
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

	// Token: 0x0600ADD8 RID: 44504 RVA: 0x0040C9E0 File Offset: 0x0040ABE0
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

	// Token: 0x0600ADD9 RID: 44505 RVA: 0x0040CA44 File Offset: 0x0040AC44
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

	// Token: 0x0600ADDA RID: 44506 RVA: 0x0040CB34 File Offset: 0x0040AD34
	public void SetSpread(int seed, Transform target)
	{
		global::UnityEngine.Random.InitState(seed);
		target.Rotate(0f, 0f, (float)global::UnityEngine.Random.Range(0, 360));
		target.Rotate(0f, global::UnityEngine.Random.Range(-this.ProjectileSpread, this.ProjectileSpread), 0f);
	}

	// Token: 0x0600ADDB RID: 44507 RVA: 0x0040CB88 File Offset: 0x0040AD88
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

	// Token: 0x0600ADDC RID: 44508 RVA: 0x0040CC14 File Offset: 0x0040AE14
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

	// Token: 0x0600ADDD RID: 44509 RVA: 0x0040CDC3 File Offset: 0x0040AFC3
	public virtual void DisableFiring(float seconds = 10000000f)
	{
		this.m_NextAllowedFireTime = Time.time + seconds;
	}

	// Token: 0x0600ADDE RID: 44510 RVA: 0x0040CDD2 File Offset: 0x0040AFD2
	public virtual void EnableFiring()
	{
		this.m_NextAllowedFireTime = Time.time;
	}

	// Token: 0x0600ADDF RID: 44511 RVA: 0x0040CDE0 File Offset: 0x0040AFE0
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

	// Token: 0x0600ADE0 RID: 44512 RVA: 0x0040CFB6 File Offset: 0x0040B1B6
	public override void Activate()
	{
		base.Activate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, true);
		}
	}

	// Token: 0x0600ADE1 RID: 44513 RVA: 0x0040CFD8 File Offset: 0x0040B1D8
	public override void Deactivate()
	{
		base.Deactivate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, false);
		}
	}

	// Token: 0x0600ADE2 RID: 44514 RVA: 0x0040CFFC File Offset: 0x0040B1FC
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

	// Token: 0x0400815C RID: 33116
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x0400815D RID: 33117
	public GameObject m_ProjectileSpawnPoint;

	// Token: 0x0400815E RID: 33118
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_ProjectileDefaultSpawnpoint;

	// Token: 0x0400815F RID: 33119
	public GameObject ProjectilePrefab;

	// Token: 0x04008160 RID: 33120
	public float ProjectileScale = 1f;

	// Token: 0x04008161 RID: 33121
	public float ProjectileFiringRate = 0.3f;

	// Token: 0x04008162 RID: 33122
	public float ProjectileSpawnDelay;

	// Token: 0x04008163 RID: 33123
	public int ProjectileCount = 1;

	// Token: 0x04008164 RID: 33124
	public float ProjectileSpread;

	// Token: 0x04008165 RID: 33125
	public bool ProjectileSourceIsRoot = true;

	// Token: 0x04008166 RID: 33126
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedFireTime;

	// Token: 0x04008167 RID: 33127
	public Vector3 MuzzleFlashPosition = Vector3.zero;

	// Token: 0x04008168 RID: 33128
	public Vector3 MuzzleFlashScale = Vector3.one;

	// Token: 0x04008169 RID: 33129
	public float MuzzleFlashFadeSpeed = 0.075f;

	// Token: 0x0400816A RID: 33130
	public GameObject MuzzleFlashPrefab;

	// Token: 0x0400816B RID: 33131
	public float MuzzleFlashDelay;

	// Token: 0x0400816C RID: 33132
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_MuzzleFlash;

	// Token: 0x0400816D RID: 33133
	public Transform m_MuzzleFlashSpawnPoint;

	// Token: 0x0400816E RID: 33134
	public GameObject ShellPrefab;

	// Token: 0x0400816F RID: 33135
	public float ShellScale = 1f;

	// Token: 0x04008170 RID: 33136
	public Vector3 ShellEjectDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x04008171 RID: 33137
	public Vector3 ShellEjectPosition = new Vector3(1f, 0f, 1f);

	// Token: 0x04008172 RID: 33138
	public float ShellEjectVelocity = 0.2f;

	// Token: 0x04008173 RID: 33139
	public float ShellEjectDelay;

	// Token: 0x04008174 RID: 33140
	public float ShellEjectSpin;

	// Token: 0x04008175 RID: 33141
	public Transform m_ShellEjectSpawnPoint;

	// Token: 0x04008176 RID: 33142
	public AudioClip SoundFire;

	// Token: 0x04008177 RID: 33143
	public float SoundFireDelay;

	// Token: 0x04008178 RID: 33144
	public Vector2 SoundFirePitch = new Vector2(1f, 1f);

	// Token: 0x04008179 RID: 33145
	public vp_Shooter.NetworkFunc m_SendFireEventToNetworkFunc;

	// Token: 0x0400817A RID: 33146
	public vp_Shooter.FirePositionFunc GetFirePosition;

	// Token: 0x0400817B RID: 33147
	public vp_Shooter.FireRotationFunc GetFireRotation;

	// Token: 0x0400817C RID: 33148
	public vp_Shooter.FireSeedFunc GetFireSeed;

	// Token: 0x0400817D RID: 33149
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentFirePosition = Vector3.zero;

	// Token: 0x0400817E RID: 33150
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_CurrentFireRotation = Quaternion.identity;

	// Token: 0x0400817F RID: 33151
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentFireSeed;

	// Token: 0x04008180 RID: 33152
	public Vector3 FirePosition = Vector3.zero;

	// Token: 0x020015B2 RID: 5554
	// (Invoke) Token: 0x0600ADE8 RID: 44520
	public delegate void NetworkFunc();

	// Token: 0x020015B3 RID: 5555
	// (Invoke) Token: 0x0600ADEC RID: 44524
	public delegate Vector3 FirePositionFunc();

	// Token: 0x020015B4 RID: 5556
	// (Invoke) Token: 0x0600ADF0 RID: 44528
	public delegate Quaternion FireRotationFunc();

	// Token: 0x020015B5 RID: 5557
	// (Invoke) Token: 0x0600ADF4 RID: 44532
	public delegate int FireSeedFunc();
}
