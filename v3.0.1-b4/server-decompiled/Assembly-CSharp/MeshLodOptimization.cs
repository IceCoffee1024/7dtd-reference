using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001333 RID: 4915
public static class MeshLodOptimization
{
	// Token: 0x06009B10 RID: 39696 RVA: 0x003A9954 File Offset: 0x003A7B54
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

	// Token: 0x06009B11 RID: 39697 RVA: 0x003A99C0 File Offset: 0x003A7BC0
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

	// Token: 0x040074F1 RID: 29937
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshLodOptimization.EditorCloneCache editorCache;

	// Token: 0x040074F2 RID: 29938
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<int> processed = new HashSet<int>();

	// Token: 0x040074F3 RID: 29939
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LODGroup> lodGroupBuffer = new List<LODGroup>();

	// Token: 0x02001334 RID: 4916
	public class EditorCloneCache
	{
		// Token: 0x06009B13 RID: 39699 RVA: 0x003A9AF8 File Offset: 0x003A7CF8
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

		// Token: 0x06009B14 RID: 39700 RVA: 0x003A9B48 File Offset: 0x003A7D48
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

		// Token: 0x040074F4 RID: 29940
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject cloneParent;

		// Token: 0x040074F5 RID: 29941
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Transform> clones = new Dictionary<int, Transform>();
	}
}
