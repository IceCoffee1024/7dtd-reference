using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015E0 RID: 5600
public class vp_DamageHandler : MonoBehaviour
{
	// Token: 0x1700145F RID: 5215
	// (get) Token: 0x0600AE99 RID: 44697 RVA: 0x004123DD File Offset: 0x004105DD
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

	// Token: 0x17001460 RID: 5216
	// (get) Token: 0x0600AE9A RID: 44698 RVA: 0x004123F7 File Offset: 0x004105F7
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

	// Token: 0x17001461 RID: 5217
	// (get) Token: 0x0600AE9B RID: 44699 RVA: 0x00412419 File Offset: 0x00410619
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

	// Token: 0x17001462 RID: 5218
	// (get) Token: 0x0600AE9C RID: 44700 RVA: 0x0041243B File Offset: 0x0041063B
	// (set) Token: 0x0600AE9D RID: 44701 RVA: 0x0041245D File Offset: 0x0041065D
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

	// Token: 0x17001463 RID: 5219
	// (get) Token: 0x0600AE9E RID: 44702 RVA: 0x00412466 File Offset: 0x00410666
	// (set) Token: 0x0600AE9F RID: 44703 RVA: 0x00412488 File Offset: 0x00410688
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

	// Token: 0x17001464 RID: 5220
	// (get) Token: 0x0600AEA0 RID: 44704 RVA: 0x00412491 File Offset: 0x00410691
	// (set) Token: 0x0600AEA1 RID: 44705 RVA: 0x00412499 File Offset: 0x00410699
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

	// Token: 0x0600AEA2 RID: 44706 RVA: 0x004124A2 File Offset: 0x004106A2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.CurrentHealth = this.MaxHealth;
		this.CheckForObsoleteParams();
	}

	// Token: 0x0600AEA3 RID: 44707 RVA: 0x004124C2 File Offset: 0x004106C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AEA4 RID: 44708 RVA: 0x004124D5 File Offset: 0x004106D5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AEA5 RID: 44709 RVA: 0x004124E8 File Offset: 0x004106E8
	public virtual void Damage(float damage)
	{
		this.Damage(new vp_DamageInfo(damage, null));
	}

	// Token: 0x0600AEA6 RID: 44710 RVA: 0x004124F8 File Offset: 0x004106F8
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

	// Token: 0x0600AEA7 RID: 44711 RVA: 0x00412604 File Offset: 0x00410804
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

	// Token: 0x0600AEA8 RID: 44712 RVA: 0x00412654 File Offset: 0x00410854
	public virtual void DieBySource(Transform source)
	{
		this.Source = source;
		this.OriginalSource = source;
		this.Die();
	}

	// Token: 0x0600AEA9 RID: 44713 RVA: 0x00412678 File Offset: 0x00410878
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

	// Token: 0x0600AEAA RID: 44714 RVA: 0x00412770 File Offset: 0x00410970
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reset()
	{
		this.CurrentHealth = this.MaxHealth;
		this.Source = null;
		this.OriginalSource = null;
	}

	// Token: 0x0600AEAB RID: 44715 RVA: 0x0041278C File Offset: 0x0041098C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveBulletHoles()
	{
		vp_HitscanBullet[] componentsInChildren = base.GetComponentsInChildren<vp_HitscanBullet>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			vp_Utility.Destroy(componentsInChildren[i].gameObject);
		}
	}

	// Token: 0x0600AEAC RID: 44716 RVA: 0x004127BC File Offset: 0x004109BC
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

	// Token: 0x0600AEAD RID: 44717 RVA: 0x0041281D File Offset: 0x00410A1D
	public static vp_DamageHandler GetDamageHandlerOfCollider(Collider col)
	{
		if (!vp_DamageHandler.DamageHandlersByCollider.TryGetValue(col, out vp_DamageHandler.m_GetDamageHandlerOfColliderResult))
		{
			vp_DamageHandler.m_GetDamageHandlerOfColliderResult = col.transform.root.GetComponentInChildren<vp_DamageHandler>();
			vp_DamageHandler.DamageHandlersByCollider.Add(col, vp_DamageHandler.m_GetDamageHandlerOfColliderResult);
		}
		return vp_DamageHandler.m_GetDamageHandlerOfColliderResult;
	}

	// Token: 0x0600AEAE RID: 44718 RVA: 0x0041285B File Offset: 0x00410A5B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_DamageHandler.DamageHandlersByCollider.Clear();
	}

	// Token: 0x0600AEAF RID: 44719 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Respawn()
	{
	}

	// Token: 0x0600AEB0 RID: 44720 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reactivate()
	{
	}

	// Token: 0x0600AEB1 RID: 44721 RVA: 0x00412868 File Offset: 0x00410A68
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

	// Token: 0x0600AEB2 RID: 44722 RVA: 0x0041296B File Offset: 0x00410B6B
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

	// Token: 0x0600AEB3 RID: 44723 RVA: 0x0041299C File Offset: 0x00410B9C
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

	// Token: 0x0600AEB4 RID: 44724 RVA: 0x00412B76 File Offset: 0x00410D76
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableOldParams()
	{
		this.Respawns = false;
		this.MinRespawnTime = -99999f;
		this.MaxRespawnTime = -99999f;
		this.RespawnCheckRadius = -99999f;
		this.RespawnSound = null;
	}

	// Token: 0x0600AEB5 RID: 44725 RVA: 0x00412BA8 File Offset: 0x00410DA8
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

	// Token: 0x040081D4 RID: 33236
	public float MaxHealth = 1f;

	// Token: 0x040081D5 RID: 33237
	public GameObject[] DeathSpawnObjects;

	// Token: 0x040081D6 RID: 33238
	public float MinDeathDelay;

	// Token: 0x040081D7 RID: 33239
	public float MaxDeathDelay;

	// Token: 0x040081D8 RID: 33240
	public float CurrentHealth;

	// Token: 0x040081D9 RID: 33241
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InstaKill;

	// Token: 0x040081DA RID: 33242
	public AudioClip DeathSound;

	// Token: 0x040081DB RID: 33243
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081DC RID: 33244
	public float ImpactDamageThreshold = 10f;

	// Token: 0x040081DD RID: 33245
	public float ImpactDamageMultiplier;

	// Token: 0x040081DE RID: 33246
	[HideInInspector]
	public bool Respawns;

	// Token: 0x040081DF RID: 33247
	[HideInInspector]
	public float MinRespawnTime = -99999f;

	// Token: 0x040081E0 RID: 33248
	[HideInInspector]
	public float MaxRespawnTime = -99999f;

	// Token: 0x040081E1 RID: 33249
	[HideInInspector]
	public float RespawnCheckRadius = -99999f;

	// Token: 0x040081E2 RID: 33250
	[HideInInspector]
	public AudioClip RespawnSound;

	// Token: 0x040081E3 RID: 33251
	[HideInInspector]
	public GameObject DeathEffect;

	// Token: 0x040081E4 RID: 33252
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_DamageHandler> m_DamageHandlersByCollider;

	// Token: 0x040081E5 RID: 33253
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_GetDamageHandlerOfColliderResult;

	// Token: 0x040081E6 RID: 33254
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_StartPosition;

	// Token: 0x040081E7 RID: 33255
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_StartRotation;

	// Token: 0x040081E8 RID: 33256
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081E9 RID: 33257
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Respawner m_Respawner;

	// Token: 0x040081EA RID: 33258
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x040081EB RID: 33259
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
