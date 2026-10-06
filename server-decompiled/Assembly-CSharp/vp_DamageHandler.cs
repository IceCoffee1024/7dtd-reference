using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015CE RID: 5582
public class vp_DamageHandler : MonoBehaviour
{
	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x0600AE41 RID: 44609 RVA: 0x0040FFD9 File Offset: 0x0040E1D9
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

	// Token: 0x17001454 RID: 5204
	// (get) Token: 0x0600AE42 RID: 44610 RVA: 0x0040FFF3 File Offset: 0x0040E1F3
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

	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x0600AE43 RID: 44611 RVA: 0x00410015 File Offset: 0x0040E215
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

	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x0600AE44 RID: 44612 RVA: 0x00410037 File Offset: 0x0040E237
	// (set) Token: 0x0600AE45 RID: 44613 RVA: 0x00410059 File Offset: 0x0040E259
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

	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x0600AE46 RID: 44614 RVA: 0x00410062 File Offset: 0x0040E262
	// (set) Token: 0x0600AE47 RID: 44615 RVA: 0x00410084 File Offset: 0x0040E284
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

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x0600AE48 RID: 44616 RVA: 0x0041008D File Offset: 0x0040E28D
	// (set) Token: 0x0600AE49 RID: 44617 RVA: 0x00410095 File Offset: 0x0040E295
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

	// Token: 0x0600AE4A RID: 44618 RVA: 0x0041009E File Offset: 0x0040E29E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.CurrentHealth = this.MaxHealth;
		this.CheckForObsoleteParams();
	}

	// Token: 0x0600AE4B RID: 44619 RVA: 0x004100BE File Offset: 0x0040E2BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE4C RID: 44620 RVA: 0x004100D1 File Offset: 0x0040E2D1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE4D RID: 44621 RVA: 0x004100E4 File Offset: 0x0040E2E4
	public virtual void Damage(float damage)
	{
		this.Damage(new vp_DamageInfo(damage, null));
	}

	// Token: 0x0600AE4E RID: 44622 RVA: 0x004100F4 File Offset: 0x0040E2F4
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

	// Token: 0x0600AE4F RID: 44623 RVA: 0x00410200 File Offset: 0x0040E400
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

	// Token: 0x0600AE50 RID: 44624 RVA: 0x00410250 File Offset: 0x0040E450
	public virtual void DieBySource(Transform source)
	{
		this.Source = source;
		this.OriginalSource = source;
		this.Die();
	}

	// Token: 0x0600AE51 RID: 44625 RVA: 0x00410274 File Offset: 0x0040E474
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

	// Token: 0x0600AE52 RID: 44626 RVA: 0x0041036C File Offset: 0x0040E56C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reset()
	{
		this.CurrentHealth = this.MaxHealth;
		this.Source = null;
		this.OriginalSource = null;
	}

	// Token: 0x0600AE53 RID: 44627 RVA: 0x00410388 File Offset: 0x0040E588
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveBulletHoles()
	{
		vp_HitscanBullet[] componentsInChildren = base.GetComponentsInChildren<vp_HitscanBullet>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			vp_Utility.Destroy(componentsInChildren[i].gameObject);
		}
	}

	// Token: 0x0600AE54 RID: 44628 RVA: 0x004103B8 File Offset: 0x0040E5B8
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

	// Token: 0x0600AE55 RID: 44629 RVA: 0x00410419 File Offset: 0x0040E619
	public static vp_DamageHandler GetDamageHandlerOfCollider(Collider col)
	{
		if (!vp_DamageHandler.DamageHandlersByCollider.TryGetValue(col, out vp_DamageHandler.m_GetDamageHandlerOfColliderResult))
		{
			vp_DamageHandler.m_GetDamageHandlerOfColliderResult = col.transform.root.GetComponentInChildren<vp_DamageHandler>();
			vp_DamageHandler.DamageHandlersByCollider.Add(col, vp_DamageHandler.m_GetDamageHandlerOfColliderResult);
		}
		return vp_DamageHandler.m_GetDamageHandlerOfColliderResult;
	}

	// Token: 0x0600AE56 RID: 44630 RVA: 0x00410457 File Offset: 0x0040E657
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_DamageHandler.DamageHandlersByCollider.Clear();
	}

	// Token: 0x0600AE57 RID: 44631 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Respawn()
	{
	}

	// Token: 0x0600AE58 RID: 44632 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reactivate()
	{
	}

	// Token: 0x0600AE59 RID: 44633 RVA: 0x00410464 File Offset: 0x0040E664
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

	// Token: 0x0600AE5A RID: 44634 RVA: 0x00410567 File Offset: 0x0040E767
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

	// Token: 0x0600AE5B RID: 44635 RVA: 0x00410598 File Offset: 0x0040E798
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

	// Token: 0x0600AE5C RID: 44636 RVA: 0x00410772 File Offset: 0x0040E972
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableOldParams()
	{
		this.Respawns = false;
		this.MinRespawnTime = -99999f;
		this.MaxRespawnTime = -99999f;
		this.RespawnCheckRadius = -99999f;
		this.RespawnSound = null;
	}

	// Token: 0x0600AE5D RID: 44637 RVA: 0x004107A4 File Offset: 0x0040E9A4
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

	// Token: 0x04008193 RID: 33171
	public float MaxHealth = 1f;

	// Token: 0x04008194 RID: 33172
	public GameObject[] DeathSpawnObjects;

	// Token: 0x04008195 RID: 33173
	public float MinDeathDelay;

	// Token: 0x04008196 RID: 33174
	public float MaxDeathDelay;

	// Token: 0x04008197 RID: 33175
	public float CurrentHealth;

	// Token: 0x04008198 RID: 33176
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InstaKill;

	// Token: 0x04008199 RID: 33177
	public AudioClip DeathSound;

	// Token: 0x0400819A RID: 33178
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400819B RID: 33179
	public float ImpactDamageThreshold = 10f;

	// Token: 0x0400819C RID: 33180
	public float ImpactDamageMultiplier;

	// Token: 0x0400819D RID: 33181
	[HideInInspector]
	public bool Respawns;

	// Token: 0x0400819E RID: 33182
	[HideInInspector]
	public float MinRespawnTime = -99999f;

	// Token: 0x0400819F RID: 33183
	[HideInInspector]
	public float MaxRespawnTime = -99999f;

	// Token: 0x040081A0 RID: 33184
	[HideInInspector]
	public float RespawnCheckRadius = -99999f;

	// Token: 0x040081A1 RID: 33185
	[HideInInspector]
	public AudioClip RespawnSound;

	// Token: 0x040081A2 RID: 33186
	[HideInInspector]
	public GameObject DeathEffect;

	// Token: 0x040081A3 RID: 33187
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_DamageHandler> m_DamageHandlersByCollider;

	// Token: 0x040081A4 RID: 33188
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_GetDamageHandlerOfColliderResult;

	// Token: 0x040081A5 RID: 33189
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_StartPosition;

	// Token: 0x040081A6 RID: 33190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_StartRotation;

	// Token: 0x040081A7 RID: 33191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081A8 RID: 33192
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Respawner m_Respawner;

	// Token: 0x040081A9 RID: 33193
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x040081AA RID: 33194
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
