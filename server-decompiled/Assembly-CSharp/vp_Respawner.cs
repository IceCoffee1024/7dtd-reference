using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015D3 RID: 5587
[Serializable]
public class vp_Respawner : MonoBehaviour
{
	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x0600AE87 RID: 44679 RVA: 0x0041075E File Offset: 0x0040E95E
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

	// Token: 0x0600AE88 RID: 44680 RVA: 0x00410778 File Offset: 0x0040E978
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

	// Token: 0x0600AE89 RID: 44681 RVA: 0x0041080F File Offset: 0x0040EA0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE8A RID: 44682 RVA: 0x00410822 File Offset: 0x0040EA22
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE8B RID: 44683 RVA: 0x00410838 File Offset: 0x0040EA38
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

	// Token: 0x0600AE8C RID: 44684 RVA: 0x004108D3 File Offset: 0x0040EAD3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
	}

	// Token: 0x0600AE8D RID: 44685 RVA: 0x00410900 File Offset: 0x0040EB00
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

	// Token: 0x0600AE8E RID: 44686 RVA: 0x00410ABE File Offset: 0x0040ECBE
	public virtual void PickSpawnPoint(Vector3 position, Quaternion rotation)
	{
		this.Placement.Position = position;
		this.Placement.Rotation = rotation;
		this.Respawn();
	}

	// Token: 0x0600AE8F RID: 44687 RVA: 0x00410AE0 File Offset: 0x0040ECE0
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

	// Token: 0x0600AE90 RID: 44688 RVA: 0x00410B5C File Offset: 0x0040ED5C
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

	// Token: 0x0600AE91 RID: 44689 RVA: 0x00410BC2 File Offset: 0x0040EDC2
	public static vp_Respawner GetRespawnerOfCollider(Collider col)
	{
		if (!vp_Respawner.RespawnersByCollider.TryGetValue(col, out vp_Respawner.m_GetRespawnerOfColliderResult))
		{
			vp_Respawner.m_GetRespawnerOfColliderResult = col.transform.root.GetComponentInChildren<vp_Respawner>();
			vp_Respawner.RespawnersByCollider.Add(col, vp_Respawner.m_GetRespawnerOfColliderResult);
		}
		return vp_Respawner.m_GetRespawnerOfColliderResult;
	}

	// Token: 0x0600AE92 RID: 44690 RVA: 0x00410C00 File Offset: 0x0040EE00
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_Respawner.RespawnersByCollider.Clear();
	}

	// Token: 0x0400821C RID: 33308
	public vp_Respawner.SpawnMode m_SpawnMode;

	// Token: 0x0400821D RID: 33309
	public string SpawnPointTag = "";

	// Token: 0x0400821E RID: 33310
	public vp_Respawner.ObstructionSolver m_ObstructionSolver;

	// Token: 0x0400821F RID: 33311
	public float ObstructionRadius = 1f;

	// Token: 0x04008220 RID: 33312
	public float MinRespawnTime = 3f;

	// Token: 0x04008221 RID: 33313
	public float MaxRespawnTime = 3f;

	// Token: 0x04008222 RID: 33314
	public float LastRespawnTime;

	// Token: 0x04008223 RID: 33315
	public bool SpawnOnAwake;

	// Token: 0x04008224 RID: 33316
	public AudioClip SpawnSound;

	// Token: 0x04008225 RID: 33317
	public GameObject[] SpawnFXPrefabs;

	// Token: 0x04008226 RID: 33318
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition = Vector3.zero;

	// Token: 0x04008227 RID: 33319
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_InitialRotation;

	// Token: 0x04008228 RID: 33320
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Placement Placement = new vp_Placement();

	// Token: 0x04008229 RID: 33321
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400822A RID: 33322
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400822B RID: 33323
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsInitialSpawnOnAwake;

	// Token: 0x0400822C RID: 33324
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x0400822D RID: 33325
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Respawner> m_RespawnersByCollider;

	// Token: 0x0400822E RID: 33326
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Respawner m_GetRespawnerOfColliderResult;

	// Token: 0x020015D4 RID: 5588
	public enum SpawnMode
	{
		// Token: 0x04008230 RID: 33328
		SamePosition,
		// Token: 0x04008231 RID: 33329
		SpawnPoint
	}

	// Token: 0x020015D5 RID: 5589
	public enum ObstructionSolver
	{
		// Token: 0x04008233 RID: 33331
		Wait,
		// Token: 0x04008234 RID: 33332
		AdjustPlacement
	}
}
