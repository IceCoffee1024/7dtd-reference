using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013F2 RID: 5106
public class EntityMeshCache : MonoBehaviour
{
	// Token: 0x0600A058 RID: 41048 RVA: 0x003C42F6 File Offset: 0x003C24F6
	public void InitData(List<CachedMeshData> collection)
	{
		this.collection = new List<CachedMeshData>(collection);
	}

	// Token: 0x0600A059 RID: 41049 RVA: 0x003C4304 File Offset: 0x003C2504
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

	// Token: 0x0600A05A RID: 41050 RVA: 0x003C43A4 File Offset: 0x003C25A4
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

	// Token: 0x04007913 RID: 30995
	[Header("This collection is filled on import. Do not edit manually")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<CachedMeshData> collection;
}
