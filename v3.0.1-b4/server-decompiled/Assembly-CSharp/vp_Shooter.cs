using System;
using UnityEngine;

// Token: 0x02001596 RID: 5526
[RequireComponent(typeof(AudioSource))]
public class vp_Shooter : vp_Component
{
	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x0600ACD3 RID: 44243 RVA: 0x004073B9 File Offset: 0x004055B9
	public GameObject ProjectileSpawnPoint
	{
		get
		{
			return this.m_ProjectileSpawnPoint;
		}
	}

	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x0600ACD4 RID: 44244 RVA: 0x004073C4 File Offset: 0x004055C4
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

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x00407478 File Offset: 0x00405678
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

	// Token: 0x0600ACD6 RID: 44246 RVA: 0x00407555 File Offset: 0x00405755
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		base.Audio.playOnAwake = false;
		base.Audio.dopplerLevel = 0f;
		base.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600ACD7 RID: 44247 RVA: 0x00407585 File Offset: 0x00405785
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
	{
		this.FirePosition = this.m_ProjectileSpawnPoint.transform.position;
	}

	// Token: 0x0600ACD8 RID: 44248 RVA: 0x0040759D File Offset: 0x0040579D
	public virtual bool CanFire()
	{
		return Time.time >= this.m_NextAllowedFireTime;
	}

	// Token: 0x0600ACD9 RID: 44249 RVA: 0x004075AF File Offset: 0x004057AF
	public virtual bool TryFire()
	{
		if (Time.time < this.m_NextAllowedFireTime)
		{
			return false;
		}
		this.Fire();
		return true;
	}

	// Token: 0x0600ACDA RID: 44250 RVA: 0x004075C8 File Offset: 0x004057C8
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

	// Token: 0x0600ACDB RID: 44251 RVA: 0x004076A0 File Offset: 0x004058A0
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

	// Token: 0x0600ACDC RID: 44252 RVA: 0x00407704 File Offset: 0x00405904
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

	// Token: 0x0600ACDD RID: 44253 RVA: 0x004077F4 File Offset: 0x004059F4
	public void SetSpread(int seed, Transform target)
	{
		global::UnityEngine.Random.InitState(seed);
		target.Rotate(0f, 0f, (float)global::UnityEngine.Random.Range(0, 360));
		target.Rotate(0f, global::UnityEngine.Random.Range(-this.ProjectileSpread, this.ProjectileSpread), 0f);
	}

	// Token: 0x0600ACDE RID: 44254 RVA: 0x00407848 File Offset: 0x00405A48
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

	// Token: 0x0600ACDF RID: 44255 RVA: 0x004078D4 File Offset: 0x00405AD4
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

	// Token: 0x0600ACE0 RID: 44256 RVA: 0x00407A83 File Offset: 0x00405C83
	public virtual void DisableFiring(float seconds = 10000000f)
	{
		this.m_NextAllowedFireTime = Time.time + seconds;
	}

	// Token: 0x0600ACE1 RID: 44257 RVA: 0x00407A92 File Offset: 0x00405C92
	public virtual void EnableFiring()
	{
		this.m_NextAllowedFireTime = Time.time;
	}

	// Token: 0x0600ACE2 RID: 44258 RVA: 0x00407AA0 File Offset: 0x00405CA0
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

	// Token: 0x0600ACE3 RID: 44259 RVA: 0x00407C76 File Offset: 0x00405E76
	public override void Activate()
	{
		base.Activate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, true);
		}
	}

	// Token: 0x0600ACE4 RID: 44260 RVA: 0x00407C98 File Offset: 0x00405E98
	public override void Deactivate()
	{
		base.Deactivate();
		if (this.MuzzleFlash != null)
		{
			vp_Utility.Activate(this.MuzzleFlash, false);
		}
	}

	// Token: 0x0600ACE5 RID: 44261 RVA: 0x00407CBC File Offset: 0x00405EBC
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

	// Token: 0x04008083 RID: 32899
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x04008084 RID: 32900
	public GameObject m_ProjectileSpawnPoint;

	// Token: 0x04008085 RID: 32901
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_ProjectileDefaultSpawnpoint;

	// Token: 0x04008086 RID: 32902
	public GameObject ProjectilePrefab;

	// Token: 0x04008087 RID: 32903
	public float ProjectileScale = 1f;

	// Token: 0x04008088 RID: 32904
	public float ProjectileFiringRate = 0.3f;

	// Token: 0x04008089 RID: 32905
	public float ProjectileSpawnDelay;

	// Token: 0x0400808A RID: 32906
	public int ProjectileCount = 1;

	// Token: 0x0400808B RID: 32907
	public float ProjectileSpread;

	// Token: 0x0400808C RID: 32908
	public bool ProjectileSourceIsRoot = true;

	// Token: 0x0400808D RID: 32909
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedFireTime;

	// Token: 0x0400808E RID: 32910
	public Vector3 MuzzleFlashPosition = Vector3.zero;

	// Token: 0x0400808F RID: 32911
	public Vector3 MuzzleFlashScale = Vector3.one;

	// Token: 0x04008090 RID: 32912
	public float MuzzleFlashFadeSpeed = 0.075f;

	// Token: 0x04008091 RID: 32913
	public GameObject MuzzleFlashPrefab;

	// Token: 0x04008092 RID: 32914
	public float MuzzleFlashDelay;

	// Token: 0x04008093 RID: 32915
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_MuzzleFlash;

	// Token: 0x04008094 RID: 32916
	public Transform m_MuzzleFlashSpawnPoint;

	// Token: 0x04008095 RID: 32917
	public GameObject ShellPrefab;

	// Token: 0x04008096 RID: 32918
	public float ShellScale = 1f;

	// Token: 0x04008097 RID: 32919
	public Vector3 ShellEjectDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x04008098 RID: 32920
	public Vector3 ShellEjectPosition = new Vector3(1f, 0f, 1f);

	// Token: 0x04008099 RID: 32921
	public float ShellEjectVelocity = 0.2f;

	// Token: 0x0400809A RID: 32922
	public float ShellEjectDelay;

	// Token: 0x0400809B RID: 32923
	public float ShellEjectSpin;

	// Token: 0x0400809C RID: 32924
	public Transform m_ShellEjectSpawnPoint;

	// Token: 0x0400809D RID: 32925
	public AudioClip SoundFire;

	// Token: 0x0400809E RID: 32926
	public float SoundFireDelay;

	// Token: 0x0400809F RID: 32927
	public Vector2 SoundFirePitch = new Vector2(1f, 1f);

	// Token: 0x040080A0 RID: 32928
	public vp_Shooter.NetworkFunc m_SendFireEventToNetworkFunc;

	// Token: 0x040080A1 RID: 32929
	public vp_Shooter.FirePositionFunc GetFirePosition;

	// Token: 0x040080A2 RID: 32930
	public vp_Shooter.FireRotationFunc GetFireRotation;

	// Token: 0x040080A3 RID: 32931
	public vp_Shooter.FireSeedFunc GetFireSeed;

	// Token: 0x040080A4 RID: 32932
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentFirePosition = Vector3.zero;

	// Token: 0x040080A5 RID: 32933
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_CurrentFireRotation = Quaternion.identity;

	// Token: 0x040080A6 RID: 32934
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentFireSeed;

	// Token: 0x040080A7 RID: 32935
	public Vector3 FirePosition = Vector3.zero;

	// Token: 0x02001597 RID: 5527
	// (Invoke) Token: 0x0600ACEB RID: 44267
	public delegate void NetworkFunc();

	// Token: 0x02001598 RID: 5528
	// (Invoke) Token: 0x0600ACEF RID: 44271
	public delegate Vector3 FirePositionFunc();

	// Token: 0x02001599 RID: 5529
	// (Invoke) Token: 0x0600ACF3 RID: 44275
	public delegate Quaternion FireRotationFunc();

	// Token: 0x0200159A RID: 5530
	// (Invoke) Token: 0x0600ACF7 RID: 44279
	public delegate int FireSeedFunc();
}
