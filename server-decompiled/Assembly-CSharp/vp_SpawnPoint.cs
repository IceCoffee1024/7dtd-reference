using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015F8 RID: 5624
[Serializable]
public class vp_SpawnPoint : MonoBehaviour
{
	// Token: 0x1700146E RID: 5230
	// (get) Token: 0x0600AF47 RID: 44871 RVA: 0x00415AC0 File Offset: 0x00413CC0
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

	// Token: 0x0600AF48 RID: 44872 RVA: 0x00415ADE File Offset: 0x00413CDE
	public static vp_Placement GetRandomPlacement()
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, null);
	}

	// Token: 0x0600AF49 RID: 44873 RVA: 0x00415AEB File Offset: 0x00413CEB
	public static vp_Placement GetRandomPlacement(float physicsCheckRadius)
	{
		return vp_SpawnPoint.GetRandomPlacement(physicsCheckRadius, null);
	}

	// Token: 0x0600AF4A RID: 44874 RVA: 0x00415AF4 File Offset: 0x00413CF4
	public static vp_Placement GetRandomPlacement(string tag)
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, tag);
	}

	// Token: 0x0600AF4B RID: 44875 RVA: 0x00415B04 File Offset: 0x00413D04
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

	// Token: 0x0600AF4C RID: 44876 RVA: 0x00415C66 File Offset: 0x00413E66
	public static vp_SpawnPoint GetRandomSpawnPoint()
	{
		if (vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			return null;
		}
		return vp_SpawnPoint.SpawnPoints[global::UnityEngine.Random.Range(0, vp_SpawnPoint.SpawnPoints.Count)];
	}

	// Token: 0x0600AF4D RID: 44877 RVA: 0x00415C94 File Offset: 0x00413E94
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

	// Token: 0x0600AF4E RID: 44878 RVA: 0x00415D2F File Offset: 0x00413F2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF4F RID: 44879 RVA: 0x00415D43 File Offset: 0x00413F43
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AF50 RID: 44880 RVA: 0x00415D57 File Offset: 0x00413F57
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_SpawnPoint.m_SpawnPoints = null;
	}

	// Token: 0x040082B8 RID: 33464
	public bool RandomDirection;

	// Token: 0x040082B9 RID: 33465
	public float Radius;

	// Token: 0x040082BA RID: 33466
	public float GroundSnapThreshold = 2.5f;

	// Token: 0x040082BB RID: 33467
	public bool LockGroundSnapToRadius = true;

	// Token: 0x040082BC RID: 33468
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_MatchingSpawnPoints = new List<vp_SpawnPoint>(50);

	// Token: 0x040082BD RID: 33469
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_SpawnPoints = null;
}
