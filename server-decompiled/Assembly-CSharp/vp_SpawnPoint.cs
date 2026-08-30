using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015D6 RID: 5590
[Serializable]
public class vp_SpawnPoint : MonoBehaviour
{
	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x0600AE94 RID: 44692 RVA: 0x00410C6C File Offset: 0x0040EE6C
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

	// Token: 0x0600AE95 RID: 44693 RVA: 0x00410C8A File Offset: 0x0040EE8A
	public static vp_Placement GetRandomPlacement()
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, null);
	}

	// Token: 0x0600AE96 RID: 44694 RVA: 0x00410C97 File Offset: 0x0040EE97
	public static vp_Placement GetRandomPlacement(float physicsCheckRadius)
	{
		return vp_SpawnPoint.GetRandomPlacement(physicsCheckRadius, null);
	}

	// Token: 0x0600AE97 RID: 44695 RVA: 0x00410CA0 File Offset: 0x0040EEA0
	public static vp_Placement GetRandomPlacement(string tag)
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, tag);
	}

	// Token: 0x0600AE98 RID: 44696 RVA: 0x00410CB0 File Offset: 0x0040EEB0
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

	// Token: 0x0600AE99 RID: 44697 RVA: 0x00410E12 File Offset: 0x0040F012
	public static vp_SpawnPoint GetRandomSpawnPoint()
	{
		if (vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			return null;
		}
		return vp_SpawnPoint.SpawnPoints[global::UnityEngine.Random.Range(0, vp_SpawnPoint.SpawnPoints.Count)];
	}

	// Token: 0x0600AE9A RID: 44698 RVA: 0x00410E40 File Offset: 0x0040F040
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

	// Token: 0x0600AE9B RID: 44699 RVA: 0x00410EDB File Offset: 0x0040F0DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE9C RID: 44700 RVA: 0x00410EEF File Offset: 0x0040F0EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AE9D RID: 44701 RVA: 0x00410F03 File Offset: 0x0040F103
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_SpawnPoint.m_SpawnPoints = null;
	}

	// Token: 0x04008235 RID: 33333
	public bool RandomDirection;

	// Token: 0x04008236 RID: 33334
	public float Radius;

	// Token: 0x04008237 RID: 33335
	public float GroundSnapThreshold = 2.5f;

	// Token: 0x04008238 RID: 33336
	public bool LockGroundSnapToRadius = true;

	// Token: 0x04008239 RID: 33337
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_MatchingSpawnPoints = new List<vp_SpawnPoint>(50);

	// Token: 0x0400823A RID: 33338
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_SpawnPoints = null;
}
