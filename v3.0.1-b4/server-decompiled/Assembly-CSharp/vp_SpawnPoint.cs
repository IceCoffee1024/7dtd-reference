using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015BB RID: 5563
[Serializable]
public class vp_SpawnPoint : MonoBehaviour
{
	// Token: 0x170013F0 RID: 5104
	// (get) Token: 0x0600AD97 RID: 44439 RVA: 0x0040B92C File Offset: 0x00409B2C
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

	// Token: 0x0600AD98 RID: 44440 RVA: 0x0040B94A File Offset: 0x00409B4A
	public static vp_Placement GetRandomPlacement()
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, null);
	}

	// Token: 0x0600AD99 RID: 44441 RVA: 0x0040B957 File Offset: 0x00409B57
	public static vp_Placement GetRandomPlacement(float physicsCheckRadius)
	{
		return vp_SpawnPoint.GetRandomPlacement(physicsCheckRadius, null);
	}

	// Token: 0x0600AD9A RID: 44442 RVA: 0x0040B960 File Offset: 0x00409B60
	public static vp_Placement GetRandomPlacement(string tag)
	{
		return vp_SpawnPoint.GetRandomPlacement(0f, tag);
	}

	// Token: 0x0600AD9B RID: 44443 RVA: 0x0040B970 File Offset: 0x00409B70
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

	// Token: 0x0600AD9C RID: 44444 RVA: 0x0040BAD2 File Offset: 0x00409CD2
	public static vp_SpawnPoint GetRandomSpawnPoint()
	{
		if (vp_SpawnPoint.SpawnPoints.Count < 1)
		{
			return null;
		}
		return vp_SpawnPoint.SpawnPoints[global::UnityEngine.Random.Range(0, vp_SpawnPoint.SpawnPoints.Count)];
	}

	// Token: 0x0600AD9D RID: 44445 RVA: 0x0040BB00 File Offset: 0x00409D00
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

	// Token: 0x0600AD9E RID: 44446 RVA: 0x0040BB9B File Offset: 0x00409D9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD9F RID: 44447 RVA: 0x0040BBAF File Offset: 0x00409DAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600ADA0 RID: 44448 RVA: 0x0040BBC3 File Offset: 0x00409DC3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		vp_SpawnPoint.m_SpawnPoints = null;
	}

	// Token: 0x0400815C RID: 33116
	public bool RandomDirection;

	// Token: 0x0400815D RID: 33117
	public float Radius;

	// Token: 0x0400815E RID: 33118
	public float GroundSnapThreshold = 2.5f;

	// Token: 0x0400815F RID: 33119
	public bool LockGroundSnapToRadius = true;

	// Token: 0x04008160 RID: 33120
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_MatchingSpawnPoints = new List<vp_SpawnPoint>(50);

	// Token: 0x04008161 RID: 33121
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<vp_SpawnPoint> m_SpawnPoints = null;
}
