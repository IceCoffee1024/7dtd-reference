using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015B8 RID: 5560
[Serializable]
public class vp_Respawner : MonoBehaviour
{
	// Token: 0x170013EF RID: 5103
	// (get) Token: 0x0600AD8A RID: 44426 RVA: 0x0040B41E File Offset: 0x0040961E
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

	// Token: 0x0600AD8B RID: 44427 RVA: 0x0040B438 File Offset: 0x00409638
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

	// Token: 0x0600AD8C RID: 44428 RVA: 0x0040B4CF File Offset: 0x004096CF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD8D RID: 44429 RVA: 0x0040B4E2 File Offset: 0x004096E2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD8E RID: 44430 RVA: 0x0040B4F8 File Offset: 0x004096F8
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

	// Token: 0x0600AD8F RID: 44431 RVA: 0x0040B593 File Offset: 0x00409793
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
	}

	// Token: 0x0600AD90 RID: 44432 RVA: 0x0040B5C0 File Offset: 0x004097C0
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

	// Token: 0x0600AD91 RID: 44433 RVA: 0x0040B77E File Offset: 0x0040997E
	public virtual void PickSpawnPoint(Vector3 position, Quaternion rotation)
	{
		this.Placement.Position = position;
		this.Placement.Rotation = rotation;
		this.Respawn();
	}

	// Token: 0x0600AD92 RID: 44434 RVA: 0x0040B7A0 File Offset: 0x004099A0
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

	// Token: 0x0600AD93 RID: 44435 RVA: 0x0040B81C File Offset: 0x00409A1C
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

	// Token: 0x0600AD94 RID: 44436 RVA: 0x0040B882 File Offset: 0x00409A82
	public static vp_Respawner GetRespawnerOfCollider(Collider col)
	{
		if (!vp_Respawner.RespawnersByCollider.TryGetValue(col, out vp_Respawner.m_GetRespawnerOfColliderResult))
		{
			vp_Respawner.m_GetRespawnerOfColliderResult = col.transform.root.GetComponentInChildren<vp_Respawner>();
			vp_Respawner.RespawnersByCollider.Add(col, vp_Respawner.m_GetRespawnerOfColliderResult);
		}
		return vp_Respawner.m_GetRespawnerOfColliderResult;
	}

	// Token: 0x0600AD95 RID: 44437 RVA: 0x0040B8C0 File Offset: 0x00409AC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_Respawner.RespawnersByCollider.Clear();
	}

	// Token: 0x04008143 RID: 33091
	public vp_Respawner.SpawnMode m_SpawnMode;

	// Token: 0x04008144 RID: 33092
	public string SpawnPointTag = "";

	// Token: 0x04008145 RID: 33093
	public vp_Respawner.ObstructionSolver m_ObstructionSolver;

	// Token: 0x04008146 RID: 33094
	public float ObstructionRadius = 1f;

	// Token: 0x04008147 RID: 33095
	public float MinRespawnTime = 3f;

	// Token: 0x04008148 RID: 33096
	public float MaxRespawnTime = 3f;

	// Token: 0x04008149 RID: 33097
	public float LastRespawnTime;

	// Token: 0x0400814A RID: 33098
	public bool SpawnOnAwake;

	// Token: 0x0400814B RID: 33099
	public AudioClip SpawnSound;

	// Token: 0x0400814C RID: 33100
	public GameObject[] SpawnFXPrefabs;

	// Token: 0x0400814D RID: 33101
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition = Vector3.zero;

	// Token: 0x0400814E RID: 33102
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_InitialRotation;

	// Token: 0x0400814F RID: 33103
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Placement Placement = new vp_Placement();

	// Token: 0x04008150 RID: 33104
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008151 RID: 33105
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008152 RID: 33106
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsInitialSpawnOnAwake;

	// Token: 0x04008153 RID: 33107
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x04008154 RID: 33108
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Respawner> m_RespawnersByCollider;

	// Token: 0x04008155 RID: 33109
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Respawner m_GetRespawnerOfColliderResult;

	// Token: 0x020015B9 RID: 5561
	public enum SpawnMode
	{
		// Token: 0x04008157 RID: 33111
		SamePosition,
		// Token: 0x04008158 RID: 33112
		SpawnPoint
	}

	// Token: 0x020015BA RID: 5562
	public enum ObstructionSolver
	{
		// Token: 0x0400815A RID: 33114
		Wait,
		// Token: 0x0400815B RID: 33115
		AdjustPlacement
	}
}
