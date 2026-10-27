using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001607 RID: 5639
[Serializable]
public class vp_Respawner : MonoBehaviour
{
	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x0600AF92 RID: 44946 RVA: 0x004179B6 File Offset: 0x00415BB6
	public static Dictionary<Collider, vp_Respawner> RespawnersByCollider
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (vp_Respawner.m_RespawnersByCollider == null)
			{
				vp_Respawner.m_RespawnersByCollider = new Dictionary<Collider, vp_Respawner>(100);
			}
			return vp_Respawner.m_RespawnersByCollider;
		}
	}

	// Token: 0x0600AF93 RID: 44947 RVA: 0x004179D0 File Offset: 0x00415BD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Audio = base.GetComponent<AudioSource>();
		this.Placement.Position = (this.m_InitialPosition = this.m_Transform.position);
		this.Placement.Rotation = (this.m_InitialRotation = this.m_Transform.rotation);
		if (this.m_SpawnMode == vp_Respawner.SpawnMode.SamePosition)
		{
			this.SpawnPointTag = "";
		}
		if (this.SpawnOnAwake)
		{
			this.m_IsInitialSpawnOnAwake = true;
			vp_Utility.Activate(base.gameObject, false);
			this.PickSpawnPoint();
		}
	}

	// Token: 0x0600AF94 RID: 44948 RVA: 0x00417A67 File Offset: 0x00415C67
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF95 RID: 44949 RVA: 0x00417A7A File Offset: 0x00415C7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF96 RID: 44950 RVA: 0x00417A90 File Offset: 0x00415C90
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SpawnFX()
	{
		if (!this.m_IsInitialSpawnOnAwake)
		{
			if (this.m_Audio != null)
			{
				this.m_Audio.pitch = Time.timeScale;
				this.m_Audio.PlayOneShot(this.SpawnSound);
			}
			if (this.SpawnFXPrefabs != null && this.SpawnFXPrefabs.Length != 0)
			{
				foreach (GameObject gameObject in this.SpawnFXPrefabs)
				{
					if (gameObject != null)
					{
						vp_Utility.Instantiate(gameObject, this.m_Transform.position, this.m_Transform.rotation);
					}
				}
			}
		}
		this.m_IsInitialSpawnOnAwake = false;
	}

	// Token: 0x0600AF97 RID: 44951 RVA: 0x00417B2B File Offset: 0x00415D2B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
	}

	// Token: 0x0600AF98 RID: 44952 RVA: 0x00417B58 File Offset: 0x00415D58
	public virtual void PickSpawnPoint()
	{
		if (this == null)
		{
			return;
		}
		if (this.m_SpawnMode == vp_Respawner.SpawnMode.SamePosition || vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			this.Placement.Position = this.m_InitialPosition;
			this.Placement.Rotation = this.m_InitialRotation;
			if (this.Placement.IsObstructed(this.ObstructionRadius))
			{
				vp_Respawner.ObstructionSolver obstructionSolver = this.m_ObstructionSolver;
				if (obstructionSolver == vp_Respawner.ObstructionSolver.Wait)
				{
					vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
					return;
				}
				if (obstructionSolver == vp_Respawner.ObstructionSolver.AdjustPlacement)
				{
					if (!vp_Placement.AdjustPosition(this.Placement, this.ObstructionRadius, 1000))
					{
						vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
						return;
					}
				}
			}
		}
		else
		{
			vp_Respawner.ObstructionSolver obstructionSolver = this.m_ObstructionSolver;
			if (obstructionSolver != vp_Respawner.ObstructionSolver.Wait)
			{
				if (obstructionSolver == vp_Respawner.ObstructionSolver.AdjustPlacement)
				{
					this.Placement = vp_SpawnPoint.GetRandomPlacement(this.ObstructionRadius, this.SpawnPointTag);
					if (this.Placement == null)
					{
						vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
						return;
					}
				}
			}
			else
			{
				this.Placement = vp_SpawnPoint.GetRandomPlacement(0f, this.SpawnPointTag);
				if (this.Placement == null)
				{
					this.Placement = new vp_Placement();
					this.m_SpawnMode = vp_Respawner.SpawnMode.SamePosition;
					this.PickSpawnPoint();
				}
				if (this.Placement.IsObstructed(this.ObstructionRadius))
				{
					vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
					return;
				}
			}
		}
		this.Respawn();
	}

	// Token: 0x0600AF99 RID: 44953 RVA: 0x00417D16 File Offset: 0x00415F16
	public virtual void PickSpawnPoint(Vector3 position, Quaternion rotation)
	{
		this.Placement.Position = position;
		this.Placement.Rotation = rotation;
		this.Respawn();
	}

	// Token: 0x0600AF9A RID: 44954 RVA: 0x00417D38 File Offset: 0x00415F38
	public virtual void Respawn()
	{
		this.LastRespawnTime = Time.time;
		vp_Utility.Activate(base.gameObject, true);
		this.SpawnFX();
		if (vp_Gameplay.isMaster)
		{
			vp_GlobalEvent<Transform, vp_Placement>.Send("Respawn", base.transform.root, this.Placement);
		}
		base.SendMessage("Reset");
		this.Placement.Position = this.m_InitialPosition;
		this.Placement.Rotation = this.m_InitialRotation;
	}

	// Token: 0x0600AF9B RID: 44955 RVA: 0x00417DB4 File Offset: 0x00415FB4
	public virtual void Reset()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.m_Transform.position = this.Placement.Position;
		if (base.GetComponent<Rigidbody>() != null && !base.GetComponent<Rigidbody>().isKinematic)
		{
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	// Token: 0x0600AF9C RID: 44956 RVA: 0x00417E1A File Offset: 0x0041601A
	public static vp_Respawner GetRespawnerOfCollider(Collider col)
	{
		if (!vp_Respawner.RespawnersByCollider.TryGetValue(col, out vp_Respawner.m_GetRespawnerOfColliderResult))
		{
			vp_Respawner.m_GetRespawnerOfColliderResult = col.transform.root.GetComponentInChildren<vp_Respawner>();
			vp_Respawner.RespawnersByCollider.Add(col, vp_Respawner.m_GetRespawnerOfColliderResult);
		}
		return vp_Respawner.m_GetRespawnerOfColliderResult;
	}

	// Token: 0x0600AF9D RID: 44957 RVA: 0x00417E58 File Offset: 0x00416058
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_Respawner.RespawnersByCollider.Clear();
	}

	// Token: 0x040082E0 RID: 33504
	public vp_Respawner.SpawnMode m_SpawnMode;

	// Token: 0x040082E1 RID: 33505
	public string SpawnPointTag = "";

	// Token: 0x040082E2 RID: 33506
	public vp_Respawner.ObstructionSolver m_ObstructionSolver;

	// Token: 0x040082E3 RID: 33507
	public float ObstructionRadius = 1f;

	// Token: 0x040082E4 RID: 33508
	public float MinRespawnTime = 3f;

	// Token: 0x040082E5 RID: 33509
	public float MaxRespawnTime = 3f;

	// Token: 0x040082E6 RID: 33510
	public float LastRespawnTime;

	// Token: 0x040082E7 RID: 33511
	public bool SpawnOnAwake;

	// Token: 0x040082E8 RID: 33512
	public AudioClip SpawnSound;

	// Token: 0x040082E9 RID: 33513
	public GameObject[] SpawnFXPrefabs;

	// Token: 0x040082EA RID: 33514
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition = Vector3.zero;

	// Token: 0x040082EB RID: 33515
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_InitialRotation;

	// Token: 0x040082EC RID: 33516
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Placement Placement = new vp_Placement();

	// Token: 0x040082ED RID: 33517
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040082EE RID: 33518
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082EF RID: 33519
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsInitialSpawnOnAwake;

	// Token: 0x040082F0 RID: 33520
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x040082F1 RID: 33521
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Respawner> m_RespawnersByCollider;

	// Token: 0x040082F2 RID: 33522
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Respawner m_GetRespawnerOfColliderResult;

	// Token: 0x02001608 RID: 5640
	public enum SpawnMode
	{
		// Token: 0x040082F4 RID: 33524
		SamePosition,
		// Token: 0x040082F5 RID: 33525
		SpawnPoint
	}

	// Token: 0x02001609 RID: 5641
	public enum ObstructionSolver
	{
		// Token: 0x040082F7 RID: 33527
		Wait,
		// Token: 0x040082F8 RID: 33528
		AdjustPlacement
	}
}
