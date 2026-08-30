using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200134D RID: 4941
public static class MeshLodOptimization
{
	// Token: 0x06009C0B RID: 39947 RVA: 0x003AEBFC File Offset: 0x003ACDFC
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

	// Token: 0x06009C0C RID: 39948 RVA: 0x003AEC68 File Offset: 0x003ACE68
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

	// Token: 0x040075C9 RID: 30153
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshLodOptimization.EditorCloneCache editorCache;

	// Token: 0x040075CA RID: 30154
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<int> processed = new HashSet<int>();

	// Token: 0x040075CB RID: 30155
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<LODGroup> lodGroupBuffer = new List<LODGroup>();

	// Token: 0x0200134E RID: 4942
	public class EditorCloneCache
	{
		// Token: 0x06009C0E RID: 39950 RVA: 0x003AEDA0 File Offset: 0x003ACFA0
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

		// Token: 0x06009C0F RID: 39951 RVA: 0x003AEDF0 File Offset: 0x003ACFF0
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

		// Token: 0x040075CC RID: 30156
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject cloneParent;

		// Token: 0x040075CD RID: 30157
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, Transform> clones = new Dictionary<int, Transform>();
	}
}
