using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001412 RID: 5138
public class EntityMeshCache : MonoBehaviour
{
	// Token: 0x0600A103 RID: 41219 RVA: 0x003C8E92 File Offset: 0x003C7092
	public void InitData(List<CachedMeshData> collection)
	{
		this.collection = new List<CachedMeshData>(collection);
	}

	// Token: 0x0600A104 RID: 41220 RVA: 0x003C8EA0 File Offset: 0x003C70A0
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

	// Token: 0x0600A105 RID: 41221 RVA: 0x003C8F40 File Offset: 0x003C7140
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

	// Token: 0x0400798E RID: 31118
	[Header("This collection is filled on import. Do not edit manually")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<CachedMeshData> collection;
}
