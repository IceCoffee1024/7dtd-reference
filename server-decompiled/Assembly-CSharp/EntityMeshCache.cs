using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001424 RID: 5156
public class EntityMeshCache : MonoBehaviour
{
	// Token: 0x0600A15B RID: 41307 RVA: 0x003CB296 File Offset: 0x003C9496
	public void InitData(List<CachedMeshData> collection)
	{
		this.collection = new List<CachedMeshData>(collection);
	}

	// Token: 0x0600A15C RID: 41308 RVA: 0x003CB2A4 File Offset: 0x003C94A4
	public bool TryGetMeshData(string name, out CachedMeshData data)
	{
		name = name.Replace(" Instance", "");
		foreach (CachedMeshData cachedMeshData in this.collection)
		{
			if (cachedMeshData.name == name)
			{
				data = cachedMeshData;
				return true;
			}
		}
		Log.Warning("Could not find {0} in entity mesh cache for prefab: {1}", new object[]
		{
			name,
			base.gameObject.name
		});
		data = new CachedMeshData();
		return false;
	}

	// Token: 0x0600A15D RID: 41309 RVA: 0x003CB344 File Offset: 0x003C9544
	public bool EqualsCollection(List<CachedMeshData> otherCollection)
	{
		if (otherCollection.Count != this.collection.Count)
		{
			return false;
		}
		for (int i = 0; i < this.collection.Count; i++)
		{
			if (!this.collection[i].ApproximatelyEquals(otherCollection[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040079CF RID: 31183
	[Header("This collection is filled on import. Do not edit manually")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<CachedMeshData> collection;
}
