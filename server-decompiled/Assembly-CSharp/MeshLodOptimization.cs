using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200136D RID: 4973
public static class MeshLodOptimization
{
	// Token: 0x06009CB7 RID: 40119 RVA: 0x003B3778 File Offset: 0x003B1978
	public static void Apply(ref Transform prefab)
	{
		if (prefab.GetComponentInChildren<Tree>())
		{
			return;
		}
		int instanceID = prefab.GetInstanceID();
		if (Application.isEditor)
		{
			if (MeshLodOptimization.editorCache == null)
			{
				MeshLodOptimization.editorCache = new MeshLodOptimization.EditorCloneCache();
			}
			prefab = MeshLodOptimization.editorCache.CacheClone(instanceID, prefab);
		}
		if (MeshLodOptimization.processed.Contains(instanceID))
		{
			return;
		}
		MeshLodOptimization.RemoveLod1(prefab);
		MeshLodOptimization.processed.Add(instanceID);
	}

	// Token: 0x06009CB8 RID: 40120 RVA: 0x003B37E4 File Offset: 0x003B19E4
	public static void RemoveLod1(Transform prefab)
	{
		MeshLodOptimization.lodGroupBuffer.Clear();
		prefab.GetComponentsInChildren<LODGroup>(MeshLodOptimization.lodGroupBuffer);
		foreach (LODGroup lodgroup in MeshLodOptimization.lodGroupBuffer)
		{
			if (lodgroup.lodCount > 2)
			{
				LOD[] lods = lodgroup.GetLODs();
				Renderer[] renderers = lods[1].renderers;
				Renderer[] renderers2 = lods[2].renderers;
				lods[1].renderers = lods[2].renderers;
				lodgroup.SetLODs(lods);
				foreach (Renderer renderer in renderers)
				{
					if (!(renderer == null))
					{
						bool flag = false;
						Renderer[] array2 = renderers2;
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j] == renderer)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							MeshFilter meshFilter;
							if (renderer.TryGetComponent<MeshFilter>(out meshFilter))
							{
								global::UnityEngine.Object.Destroy(meshFilter);
							}
							global::UnityEngine.Object.Destroy(renderer);
						}
					}
				}
			}
		}
	}

	// Token: 0x04007644 RID: 30276
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshLodOptimization.EditorCloneCache editorCache;

	// Token: 0x04007645 RID: 30277
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<int> processed = new HashSet<int>();

	// Token: 0x04007646 RID: 30278
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LODGroup> lodGroupBuffer = new List<LODGroup>();

	// Token: 0x0200136E RID: 4974
	public class EditorCloneCache
	{
		// Token: 0x06009CBA RID: 40122 RVA: 0x003B391C File Offset: 0x003B1B1C
		public EditorCloneCache()
		{
			this.cloneParent = new GameObject("LodCullingCache");
			this.cloneParent.SetActive(false);
			GameManager.Instance.OnWorldChanged += delegate(World world)
			{
				if (world == null)
				{
					this.cloneParent.transform.DestroyChildren();
					this.clones.Clear();
				}
			};
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x003B396C File Offset: 0x003B1B6C
		public Transform CacheClone(int id, Transform prefab)
		{
			Transform transform;
			if (this.clones.TryGetValue(id, out transform))
			{
				return transform;
			}
			string name = prefab.name;
			transform = global::UnityEngine.Object.Instantiate<Transform>(prefab, this.cloneParent.transform);
			transform.name = name;
			this.clones.Add(id, transform);
			return transform;
		}

		// Token: 0x04007647 RID: 30279
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject cloneParent;

		// Token: 0x04007648 RID: 30280
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Transform> clones = new Dictionary<int, Transform>();
	}
}
