using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015AC RID: 5548
public class vp_DamageHandler : MonoBehaviour
{
	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x0600AD8E RID: 44430 RVA: 0x0040B185 File Offset: 0x00409385
	public static Dictionary<Collider, vp_DamageHandler> DamageHandlersByCollider
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (vp_DamageHandler.m_DamageHandlersByCollider == null)
			{
				vp_DamageHandler.m_DamageHandlersByCollider = new Dictionary<Collider, vp_DamageHandler>(100);
			}
			return vp_DamageHandler.m_DamageHandlersByCollider;
		}
	}

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x0600AD8F RID: 44431 RVA: 0x0040B19F File Offset: 0x0040939F
	public Transform Transform
	{
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x0600AD90 RID: 44432 RVA: 0x0040B1C1 File Offset: 0x004093C1
	public vp_Respawner Respawner
	{
		get
		{
			if (this.m_Respawner == null)
			{
				this.m_Respawner = base.GetComponent<vp_Respawner>();
			}
			return this.m_Respawner;
		}
	}

	// Token: 0x17001411 RID: 5137
	// (get) Token: 0x0600AD91 RID: 44433 RVA: 0x0040B1E3 File Offset: 0x004093E3
	// (set) Token: 0x0600AD92 RID: 44434 RVA: 0x0040B205 File Offset: 0x00409405
	public Transform Source
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Source == null)
			{
				this.m_Source = this.Transform;
			}
			return this.m_Source;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_Source = value;
		}
	}

	// Token: 0x17001412 RID: 5138
	// (get) Token: 0x0600AD93 RID: 44435 RVA: 0x0040B20E File Offset: 0x0040940E
	// (set) Token: 0x0600AD94 RID: 44436 RVA: 0x0040B230 File Offset: 0x00409430
	public Transform OriginalSource
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_OriginalSource == null)
			{
				this.m_OriginalSource = this.Transform;
			}
			return this.m_OriginalSource;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_OriginalSource = value;
		}
	}

	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x0600AD95 RID: 44437 RVA: 0x0040B239 File Offset: 0x00409439
	// (set) Token: 0x0600AD96 RID: 44438 RVA: 0x0040B241 File Offset: 0x00409441
	[Obsolete("This property will be removed in an upcoming release.")]
	public Transform Sender
	{
		get
		{
			return this.Source;
		}
		set
		{
			this.Source = value;
		}
	}

	// Token: 0x0600AD97 RID: 44439 RVA: 0x0040B24A File Offset: 0x0040944A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.CurrentHealth = this.MaxHealth;
		this.CheckForObsoleteParams();
	}

	// Token: 0x0600AD98 RID: 44440 RVA: 0x0040B26A File Offset: 0x0040946A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD99 RID: 44441 RVA: 0x0040B27D File Offset: 0x0040947D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD9A RID: 44442 RVA: 0x0040B290 File Offset: 0x00409490
	public virtual void Damage(float damage)
	{
		this.Damage(new vp_DamageInfo(damage, null));
	}

	// Token: 0x0600AD9B RID: 44443 RVA: 0x0040B2A0 File Offset: 0x004094A0
	public virtual void Damage(vp_DamageInfo damageInfo)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		if (this.CurrentHealth <= 0f)
		{
			return;
		}
		if (damageInfo != null)
		{
			if (damageInfo.Source != null)
			{
				this.Source = damageInfo.Source;
			}
			if (damageInfo.OriginalSource != null)
			{
				this.OriginalSource = damageInfo.OriginalSource;
			}
		}
		this.CurrentHealth = Mathf.Min(this.CurrentHealth - damageInfo.Damage, this.MaxHealth);
		if (vp_Gameplay.isMaster)
		{
			if (vp_Gameplay.isMultiplayer && damageInfo.Source != null)
			{
				vp_GlobalEvent<Transform, Transform, float>.Send("Damage", this.Transform.root, damageInfo.OriginalSource, damageInfo.Damage, vp_GlobalEventMode.REQUIRE_LISTENER);
			}
			if (this.CurrentHealth <= 0f)
			{
				if (this.m_InstaKill)
				{
					base.SendMessage("Die");
					return;
				}
				vp_Timer.In(global::UnityEngine.Random.Range(this.MinDeathDelay, this.MaxDeathDelay), delegate
				{
					base.SendMessage("Die");
				}, null);
			}
		}
	}

	// Token: 0x0600AD9C RID: 44444 RVA: 0x0040B3AC File Offset: 0x004095AC
	public virtual void DieBySources(Transform[] sourceAndOriginalSource)
	{
		if (sourceAndOriginalSource.Length != 2)
		{
			Debug.LogWarning("Warning (" + ((this != null) ? this.ToString() : null) + ") 'DieBySources' argument must contain 2 transforms.");
			return;
		}
		this.Source = sourceAndOriginalSource[0];
		this.OriginalSource = sourceAndOriginalSource[1];
		this.Die();
	}

	// Token: 0x0600AD9D RID: 44445 RVA: 0x0040B3FC File Offset: 0x004095FC
	public virtual void DieBySource(Transform source)
	{
		this.Source = source;
		this.OriginalSource = source;
		this.Die();
	}

	// Token: 0x0600AD9E RID: 44446 RVA: 0x0040B420 File Offset: 0x00409620
	public virtual void Die()
	{
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.DeathSound);
		}
		foreach (GameObject gameObject in this.DeathSpawnObjects)
		{
			if (gameObject != null)
			{
				GameObject gameObject2 = (GameObject)vp_Utility.Instantiate(gameObject, this.Transform.position, this.Transform.rotation);
				if (this.Source != null && gameObject2 != null)
				{
					vp_TargetEvent<Transform>.Send(gameObject2.transform, "SetSource", this.OriginalSource, vp_TargetEventOptions.DontRequireReceiver);
				}
			}
		}
		if (this.Respawner == null)
		{
			vp_Utility.Destroy(base.gameObject);
		}
		else
		{
			this.RemoveBulletHoles();
			vp_Utility.Activate(base.gameObject, false);
		}
		this.m_InstaKill = false;
	}

	// Token: 0x0600AD9F RID: 44447 RVA: 0x0040B518 File Offset: 0x00409718
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reset()
	{
		this.CurrentHealth = this.MaxHealth;
		this.Source = null;
		this.OriginalSource = null;
	}

	// Token: 0x0600ADA0 RID: 44448 RVA: 0x0040B534 File Offset: 0x00409734
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveBulletHoles()
	{
		vp_HitscanBullet[] componentsInChildren = base.GetComponentsInChildren<vp_HitscanBullet>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			vp_Utility.Destroy(componentsInChildren[i].gameObject);
		}
	}

	// Token: 0x0600ADA1 RID: 44449 RVA: 0x0040B564 File Offset: 0x00409764
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCollisionEnter(Collision collision)
	{
		float num = collision.relativeVelocity.sqrMagnitude * 0.1f;
		float num2 = ((num > this.ImpactDamageThreshold) ? (num * this.ImpactDamageMultiplier) : 0f);
		if (num2 <= 0f)
		{
			return;
		}
		if (this.CurrentHealth - num2 <= 0f)
		{
			this.m_InstaKill = true;
		}
		this.Damage(num2);
	}

	// Token: 0x0600ADA2 RID: 44450 RVA: 0x0040B5C5 File Offset: 0x004097C5
	public static vp_DamageHandler GetDamageHandlerOfCollider(Collider col)
	{
		if (!vp_DamageHandler.DamageHandlersByCollider.TryGetValue(col, out vp_DamageHandler.m_GetDamageHandlerOfColliderResult))
		{
			vp_DamageHandler.m_GetDamageHandlerOfColliderResult = col.transform.root.GetComponentInChildren<vp_DamageHandler>();
			vp_DamageHandler.DamageHandlersByCollider.Add(col, vp_DamageHandler.m_GetDamageHandlerOfColliderResult);
		}
		return vp_DamageHandler.m_GetDamageHandlerOfColliderResult;
	}

	// Token: 0x0600ADA3 RID: 44451 RVA: 0x0040B603 File Offset: 0x00409803
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_DamageHandler.DamageHandlersByCollider.Clear();
	}

	// Token: 0x0600ADA4 RID: 44452 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Respawn()
	{
	}

	// Token: 0x0600ADA5 RID: 44453 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reactivate()
	{
	}

	// Token: 0x0600ADA6 RID: 44454 RVA: 0x0040B610 File Offset: 0x00409810
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckForObsoleteParams()
	{
		if (this.DeathEffect != null)
		{
			Debug.LogWarning(((this != null) ? this.ToString() : null) + "'DeathEffect' is obsolete! Please use the 'DeathSpawnObjects' array instead.");
		}
		string text = "";
		if (this.Respawns)
		{
			text += "Respawns, ";
		}
		if (this.MinRespawnTime != -99999f)
		{
			text += "MinRespawnTime, ";
		}
		if (this.MaxRespawnTime != -99999f)
		{
			text += "MaxRespawnTime, ";
		}
		if (this.RespawnCheckRadius != -99999f)
		{
			text += "RespawnCheckRadius, ";
		}
		if (this.RespawnSound != null)
		{
			text += "RespawnSound, ";
		}
		if (text != "")
		{
			text = text.Remove(text.LastIndexOf(", "));
			Debug.LogWarning(string.Format("Warning + (" + ((this != null) ? this.ToString() : null) + ") The following parameters are obsolete: \"{0}\". Creating a temp vp_Respawner component. To remove this warning, see the UFPS menu -> Wizards -> Convert Old DamageHandlers.", text));
			this.CreateTempRespawner();
		}
	}

	// Token: 0x0600ADA7 RID: 44455 RVA: 0x0040B713 File Offset: 0x00409913
	public bool CreateTempRespawner()
	{
		if (base.GetComponent<vp_Respawner>() || base.GetComponent<vp_PlayerRespawner>())
		{
			this.DisableOldParams();
			return false;
		}
		vp_DamageHandler.CreateRespawnerForDamageHandler(this);
		this.DisableOldParams();
		return true;
	}

	// Token: 0x0600ADA8 RID: 44456 RVA: 0x0040B744 File Offset: 0x00409944
	public static int GenerateRespawnersForAllDamageHandlers()
	{
		vp_PlayerDamageHandler[] array = global::UnityEngine.Object.FindObjectsByType<vp_PlayerDamageHandler>(FindObjectsSortMode.None);
		if (array != null && array.Length != 0)
		{
			foreach (vp_PlayerDamageHandler vp_PlayerDamageHandler in array)
			{
				if (!(vp_PlayerDamageHandler.transform.GetComponent<vp_FPPlayerEventHandler>() == null))
				{
					vp_FPPlayerDamageHandler vp_FPPlayerDamageHandler = vp_PlayerDamageHandler.gameObject.AddComponent<vp_FPPlayerDamageHandler>();
					vp_FPPlayerDamageHandler.AllowFallDamage = vp_PlayerDamageHandler.AllowFallDamage;
					vp_FPPlayerDamageHandler.DeathEffect = vp_PlayerDamageHandler.DeathEffect;
					vp_FPPlayerDamageHandler.DeathSound = vp_PlayerDamageHandler.DeathSound;
					vp_FPPlayerDamageHandler.DeathSpawnObjects = vp_PlayerDamageHandler.DeathSpawnObjects;
					vp_FPPlayerDamageHandler.FallImpactPitch = vp_PlayerDamageHandler.FallImpactPitch;
					vp_FPPlayerDamageHandler.FallImpactSounds = vp_PlayerDamageHandler.FallImpactSounds;
					vp_FPPlayerDamageHandler.FallImpactThreshold = vp_PlayerDamageHandler.FallImpactThreshold;
					vp_FPPlayerDamageHandler.ImpactDamageMultiplier = vp_PlayerDamageHandler.ImpactDamageMultiplier;
					vp_FPPlayerDamageHandler.ImpactDamageThreshold = vp_PlayerDamageHandler.ImpactDamageThreshold;
					vp_FPPlayerDamageHandler.m_Audio = vp_PlayerDamageHandler.m_Audio;
					vp_FPPlayerDamageHandler.CurrentHealth = vp_PlayerDamageHandler.CurrentHealth;
					vp_FPPlayerDamageHandler.m_StartPosition = vp_PlayerDamageHandler.m_StartPosition;
					vp_FPPlayerDamageHandler.m_StartRotation = vp_PlayerDamageHandler.m_StartRotation;
					vp_FPPlayerDamageHandler.MaxDeathDelay = vp_PlayerDamageHandler.MaxDeathDelay;
					vp_FPPlayerDamageHandler.MaxHealth = vp_PlayerDamageHandler.MaxHealth;
					vp_FPPlayerDamageHandler.MaxRespawnTime = vp_PlayerDamageHandler.MaxRespawnTime;
					vp_FPPlayerDamageHandler.MinDeathDelay = vp_PlayerDamageHandler.MinDeathDelay;
					vp_FPPlayerDamageHandler.MinRespawnTime = vp_PlayerDamageHandler.MinRespawnTime;
					vp_FPPlayerDamageHandler.RespawnCheckRadius = vp_PlayerDamageHandler.RespawnCheckRadius;
					vp_FPPlayerDamageHandler.Respawns = vp_PlayerDamageHandler.Respawns;
					vp_FPPlayerDamageHandler.RespawnSound = vp_PlayerDamageHandler.RespawnSound;
					global::UnityEngine.Object.DestroyImmediate(vp_PlayerDamageHandler);
				}
			}
		}
		vp_DamageHandler[] array3 = global::UnityEngine.Object.FindObjectsByType<vp_DamageHandler>(FindObjectsSortMode.None);
		vp_DamageHandler[] array4 = global::UnityEngine.Object.FindObjectsByType<vp_FPPlayerDamageHandler>(FindObjectsSortMode.None);
		vp_DamageHandler[] array5 = array4;
		int num = 0;
		array4 = array3;
		for (int i = 0; i < array4.Length; i++)
		{
			if (array4[i].CreateTempRespawner())
			{
				num++;
			}
		}
		array4 = array5;
		for (int i = 0; i < array4.Length; i++)
		{
			if (array4[i].CreateTempRespawner())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600ADA9 RID: 44457 RVA: 0x0040B91E File Offset: 0x00409B1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableOldParams()
	{
		this.Respawns = false;
		this.MinRespawnTime = -99999f;
		this.MaxRespawnTime = -99999f;
		this.RespawnCheckRadius = -99999f;
		this.RespawnSound = null;
	}

	// Token: 0x0600ADAA RID: 44458 RVA: 0x0040B950 File Offset: 0x00409B50
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateRespawnerForDamageHandler(vp_DamageHandler damageHandler)
	{
		if (damageHandler.gameObject.GetComponent<vp_Respawner>() || damageHandler.gameObject.GetComponent<vp_PlayerRespawner>())
		{
			return;
		}
		vp_Respawner vp_Respawner;
		if (damageHandler is vp_FPPlayerDamageHandler)
		{
			vp_Respawner = damageHandler.gameObject.AddComponent<vp_PlayerRespawner>();
		}
		else
		{
			vp_Respawner = damageHandler.gameObject.AddComponent<vp_Respawner>();
		}
		if (vp_Respawner == null)
		{
			return;
		}
		if (damageHandler.MinRespawnTime != -99999f)
		{
			vp_Respawner.MinRespawnTime = damageHandler.MinRespawnTime;
		}
		if (damageHandler.MaxRespawnTime != -99999f)
		{
			vp_Respawner.MaxRespawnTime = damageHandler.MaxRespawnTime;
		}
		if (damageHandler.RespawnCheckRadius != -99999f)
		{
			vp_Respawner.ObstructionRadius = damageHandler.RespawnCheckRadius;
		}
		if (damageHandler.RespawnSound != null)
		{
			vp_Respawner.SpawnSound = damageHandler.RespawnSound;
		}
	}

	// Token: 0x04008110 RID: 33040
	public float MaxHealth = 1f;

	// Token: 0x04008111 RID: 33041
	public GameObject[] DeathSpawnObjects;

	// Token: 0x04008112 RID: 33042
	public float MinDeathDelay;

	// Token: 0x04008113 RID: 33043
	public float MaxDeathDelay;

	// Token: 0x04008114 RID: 33044
	public float CurrentHealth;

	// Token: 0x04008115 RID: 33045
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InstaKill;

	// Token: 0x04008116 RID: 33046
	public AudioClip DeathSound;

	// Token: 0x04008117 RID: 33047
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008118 RID: 33048
	public float ImpactDamageThreshold = 10f;

	// Token: 0x04008119 RID: 33049
	public float ImpactDamageMultiplier;

	// Token: 0x0400811A RID: 33050
	[HideInInspector]
	public bool Respawns;

	// Token: 0x0400811B RID: 33051
	[HideInInspector]
	public float MinRespawnTime = -99999f;

	// Token: 0x0400811C RID: 33052
	[HideInInspector]
	public float MaxRespawnTime = -99999f;

	// Token: 0x0400811D RID: 33053
	[HideInInspector]
	public float RespawnCheckRadius = -99999f;

	// Token: 0x0400811E RID: 33054
	[HideInInspector]
	public AudioClip RespawnSound;

	// Token: 0x0400811F RID: 33055
	[HideInInspector]
	public GameObject DeathEffect;

	// Token: 0x04008120 RID: 33056
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_DamageHandler> m_DamageHandlersByCollider;

	// Token: 0x04008121 RID: 33057
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_GetDamageHandlerOfColliderResult;

	// Token: 0x04008122 RID: 33058
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_StartPosition;

	// Token: 0x04008123 RID: 33059
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_StartRotation;

	// Token: 0x04008124 RID: 33060
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008125 RID: 33061
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Respawner m_Respawner;

	// Token: 0x04008126 RID: 33062
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008127 RID: 33063
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
