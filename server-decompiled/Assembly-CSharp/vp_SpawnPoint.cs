using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200160A RID: 5642
[Serializable]
public class vp_SpawnPoint : MonoBehaviour
{
	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x0600AF9F RID: 44959 RVA: 0x00417EC4 File Offset: 0x004160C4
	public static List<vp_SpawnPoint> SpawnPoints
	{
		get
		{
			if (vp_SpawnPoint.m_SpawnPoints == null)
			{
				vp_SpawnPoint.m_SpawnPoints = new List<vp_SpawnPoint>(global::UnityEngine.Object.FindObjectsByType<vp_SpawnPoint>(FindObjectsSortMode.None));
			}
			return vp_SpawnPoint.m_SpawnPoints;
		}
	}

	// Token: 0x0600AFA0 RID: 44960 RVA: 0x00417EE2 File Offset: 0x004160E2
	public static vp_Placement GetRandomPlacement()
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, null);
	}

	// Token: 0x0600AFA1 RID: 44961 RVA: 0x00417EEF File Offset: 0x004160EF
	public static vp_Placement GetRandomPlacement(float physicsCheckRadius)
	{
		return vp_SpawnPoint.GetRandomPlacement(physicsCheckRadius, null);
	}

	// Token: 0x0600AFA2 RID: 44962 RVA: 0x00417EF8 File Offset: 0x004160F8
	public static vp_Placement GetRandomPlacement(string tag)
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, tag);
	}

	// Token: 0x0600AFA3 RID: 44963 RVA: 0x00417F08 File Offset: 0x00416108
	public static vp_Placement GetRandomPlacement(float physicsCheckRadius, string tag)
	{
		if (vp_SpawnPoint.SpawnPoints == null || vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			return null;
		}
		vp_SpawnPoint vp_SpawnPoint;
		if (string.IsNullOrEmpty(tag))
		{
			vp_SpawnPoint = vp_SpawnPoint.GetRandomSpawnPoint();
		}
		else
		{
			vp_SpawnPoint = vp_SpawnPoint.GetRandomSpawnPoint(tag);
			if (vp_SpawnPoint == null)
			{
				vp_SpawnPoint = vp_SpawnPoint.GetRandomSpawnPoint();
				Debug.LogWarning("Warning (vp_SpawnPoint --> GetRandomPlacement) Could not find a spawnpoint tagged '" + tag + "'. Falling back to 'any random spawnpoint'.");
			}
		}
		if (vp_SpawnPoint == null)
		{
			Debug.LogError("Error (vp_SpawnPoint --> GetRandomPlacement) Could not find a spawnpoint" + ((!string.IsNullOrEmpty(tag)) ? (" tagged '" + tag + "'") : ".") + " Reverting to world origin.");
			return null;
		}
		vp_Placement vp_Placement = new vp_Placement();
		vp_Placement.Position = vp_SpawnPoint.transform.position;
		if (vp_SpawnPoint.Radius > 0f)
		{
			Vector3 vector = global::UnityEngine.Random.insideUnitSphere * vp_SpawnPoint.Radius;
			vp_Placement vp_Placement2 = vp_Placement;
			vp_Placement2.Position.x = vp_Placement2.Position.x + vector.x;
			vp_Placement vp_Placement3 = vp_Placement;
			vp_Placement3.Position.z = vp_Placement3.Position.z + vector.z;
		}
		if (physicsCheckRadius != 0f)
		{
			if (!vp_Placement.AdjustPosition(vp_Placement, physicsCheckRadius, 1000))
			{
				return null;
			}
			vp_Placement.SnapToGround(vp_Placement, physicsCheckRadius, vp_SpawnPoint.GroundSnapThreshold);
		}
		if (vp_SpawnPoint.RandomDirection)
		{
			vp_Placement.Rotation = Quaternion.Euler(Vector3.up * global::UnityEngine.Random.Range(0f, 360f));
		}
		else
		{
			vp_Placement.Rotation = vp_SpawnPoint.transform.rotation;
		}
		return vp_Placement;
	}

	// Token: 0x0600AFA4 RID: 44964 RVA: 0x0041806A File Offset: 0x0041626A
	public static vp_SpawnPoint GetRandomSpawnPoint()
	{
		if (vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			return null;
		}
		return vp_SpawnPoint.SpawnPoints[global::UnityEngine.Random.Range(0, vp_SpawnPoint.SpawnPoints.Count)];
	}

	// Token: 0x0600AFA5 RID: 44965 RVA: 0x00418098 File Offset: 0x00416298
	public static vp_SpawnPoint GetRandomSpawnPoint(string tag)
	{
		vp_SpawnPoint.m_MatchingSpawnPoints.Clear();
		for (int i = 0; i < vp_SpawnPoint.SpawnPoints.Count; i++)
		{
			if (vp_SpawnPoint.m_SpawnPoints[i].tag == tag)
			{
				vp_SpawnPoint.m_MatchingSpawnPoints.Add(vp_SpawnPoint.m_SpawnPoints[i]);
			}
		}
		if (vp_SpawnPoint.m_MatchingSpawnPoints.Count < 1)
		{
			return null;
		}
		if (vp_SpawnPoint.m_MatchingSpawnPoints.Count == 1)
		{
			return vp_SpawnPoint.m_MatchingSpawnPoints[0];
		}
		return vp_SpawnPoint.m_MatchingSpawnPoints[global::UnityEngine.Random.Range(0, vp_SpawnPoint.m_MatchingSpawnPoints.Count)];
	}

	// Token: 0x0600AFA6 RID: 44966 RVA: 0x00418133 File Offset: 0x00416333
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AFA7 RID: 44967 RVA: 0x00418147 File Offset: 0x00416347
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AFA8 RID: 44968 RVA: 0x0041815B File Offset: 0x0041635B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_SpawnPoint.m_SpawnPoints = null;
	}

	// Token: 0x040082F9 RID: 33529
	public bool RandomDirection;

	// Token: 0x040082FA RID: 33530
	public float Radius;

	// Token: 0x040082FB RID: 33531
	public float GroundSnapThreshold = 2.5f;

	// Token: 0x040082FC RID: 33532
	public bool LockGroundSnapToRadius = true;

	// Token: 0x040082FD RID: 33533
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_MatchingSpawnPoints = new List<vp_SpawnPoint>(50);

	// Token: 0x040082FE RID: 33534
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_SpawnPoints = null;
}
