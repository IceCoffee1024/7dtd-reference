using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015F5 RID: 5621
[Serializable]
public class vp_Respawner : MonoBehaviour
{
	// Token: 0x1700146D RID: 5229
	// (get) Token: 0x0600AF3A RID: 44858 RVA: 0x004155B2 File Offset: 0x004137B2
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

	// Token: 0x0600AF3B RID: 44859 RVA: 0x004155CC File Offset: 0x004137CC
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

	// Token: 0x0600AF3C RID: 44860 RVA: 0x00415663 File Offset: 0x00413863
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF3D RID: 44861 RVA: 0x00415676 File Offset: 0x00413876
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF3E RID: 44862 RVA: 0x0041568C File Offset: 0x0041388C
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

	// Token: 0x0600AF3F RID: 44863 RVA: 0x00415727 File Offset: 0x00413927
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Timer.In(global::UnityEngine.Random.Range(this.MinRespawnTime, this.MaxRespawnTime), new vp_Timer.Callback(this.PickSpawnPoint), this.m_RespawnTimer);
	}

	// Token: 0x0600AF40 RID: 44864 RVA: 0x00415754 File Offset: 0x00413954
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

	// Token: 0x0600AF41 RID: 44865 RVA: 0x00415912 File Offset: 0x00413B12
	public virtual void PickSpawnPoint(Vector3 position, Quaternion rotation)
	{
		this.Placement.Position = position;
		this.Placement.Rotation = rotation;
		this.Respawn();
	}

	// Token: 0x0600AF42 RID: 44866 RVA: 0x00415934 File Offset: 0x00413B34
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

	// Token: 0x0600AF43 RID: 44867 RVA: 0x004159B0 File Offset: 0x00413BB0
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

	// Token: 0x0600AF44 RID: 44868 RVA: 0x00415A16 File Offset: 0x00413C16
	public static vp_Respawner GetRespawnerOfCollider(Collider col)
	{
		if (!vp_Respawner.RespawnersByCollider.TryGetValue(col, out vp_Respawner.m_GetRespawnerOfColliderResult))
		{
			vp_Respawner.m_GetRespawnerOfColliderResult = col.transform.root.GetComponentInChildren<vp_Respawner>();
			vp_Respawner.RespawnersByCollider.Add(col, vp_Respawner.m_GetRespawnerOfColliderResult);
		}
		return vp_Respawner.m_GetRespawnerOfColliderResult;
	}

	// Token: 0x0600AF45 RID: 44869 RVA: 0x00415A54 File Offset: 0x00413C54
	[PublicizedFrom(EAccessModifier.Protected)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_Respawner.RespawnersByCollider.Clear();
	}

	// Token: 0x0400829F RID: 33439
	public vp_Respawner.SpawnMode m_SpawnMode;

	// Token: 0x040082A0 RID: 33440
	public string SpawnPointTag = "";

	// Token: 0x040082A1 RID: 33441
	public vp_Respawner.ObstructionSolver m_ObstructionSolver;

	// Token: 0x040082A2 RID: 33442
	public float ObstructionRadius = 1f;

	// Token: 0x040082A3 RID: 33443
	public float MinRespawnTime = 3f;

	// Token: 0x040082A4 RID: 33444
	public float MaxRespawnTime = 3f;

	// Token: 0x040082A5 RID: 33445
	public float LastRespawnTime;

	// Token: 0x040082A6 RID: 33446
	public bool SpawnOnAwake;

	// Token: 0x040082A7 RID: 33447
	public AudioClip SpawnSound;

	// Token: 0x040082A8 RID: 33448
	public GameObject[] SpawnFXPrefabs;

	// Token: 0x040082A9 RID: 33449
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition = Vector3.zero;

	// Token: 0x040082AA RID: 33450
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_InitialRotation;

	// Token: 0x040082AB RID: 33451
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Placement Placement = new vp_Placement();

	// Token: 0x040082AC RID: 33452
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040082AD RID: 33453
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082AE RID: 33454
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsInitialSpawnOnAwake;

	// Token: 0x040082AF RID: 33455
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x040082B0 RID: 33456
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Respawner> m_RespawnersByCollider;

	// Token: 0x040082B1 RID: 33457
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Respawner m_GetRespawnerOfColliderResult;

	// Token: 0x020015F6 RID: 5622
	public enum SpawnMode
	{
		// Token: 0x040082B3 RID: 33459
		SamePosition,
		// Token: 0x040082B4 RID: 33460
		SpawnPoint
	}

	// Token: 0x020015F7 RID: 5623
	public enum ObstructionSolver
	{
		// Token: 0x040082B6 RID: 33462
		Wait,
		// Token: 0x040082B7 RID: 33463
		AdjustPlacement
	}
}
