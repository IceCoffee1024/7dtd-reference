using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200137F RID: 4991
public static class MeshLodOptimization
{
	// Token: 0x06009D0F RID: 40207 RVA: 0x003B5B7C File Offset: 0x003B3D7C
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

	// Token: 0x06009D10 RID: 40208 RVA: 0x003B5BE8 File Offset: 0x003B3DE8
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

	// Token: 0x04007685 RID: 30341
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshLodOptimization.EditorCloneCache editorCache;

	// Token: 0x04007686 RID: 30342
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<int> processed = new HashSet<int>();

	// Token: 0x04007687 RID: 30343
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LODGroup> lodGroupBuffer = new List<LODGroup>();

	// Token: 0x02001380 RID: 4992
	public class EditorCloneCache
	{
		// Token: 0x06009D12 RID: 40210 RVA: 0x003B5D20 File Offset: 0x003B3F20
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

		// Token: 0x06009D13 RID: 40211 RVA: 0x003B5D70 File Offset: 0x003B3F70
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

		// Token: 0x04007688 RID: 30344
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject cloneParent;

		// Token: 0x04007689 RID: 30345
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Transform> clones = new Dictionary<int, Transform>();
	}
}
