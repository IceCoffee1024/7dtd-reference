using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013D7 RID: 5079
public class EntityMeshCache : MonoBehaviour
{
	// Token: 0x06009F5B RID: 40795 RVA: 0x003BEFB2 File Offset: 0x003BD1B2
	public void InitData(List<CachedMeshData> collection)
	{
		this.collection = new List<CachedMeshData>(collection);
	}

	// Token: 0x06009F5C RID: 40796 RVA: 0x003BEFC0 File Offset: 0x003BD1C0
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

	// Token: 0x06009F5D RID: 40797 RVA: 0x003BF060 File Offset: 0x003BD260
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

	// Token: 0x0400783A RID: 30778
	[Header("This collection is filled on import. Do not edit manually")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<CachedMeshData> collection;
}
