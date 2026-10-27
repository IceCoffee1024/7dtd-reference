using System;

// Token: 0x02000937 RID: 2359
public class EventPrefabsClient
{
	// Token: 0x060043D4 RID: 17364 RVA: 0x001A9BEC File Offset: 0x001A7DEC
	public EventPrefabsClient(PrefabCache prefabCache, DynamicPrefabDecorator dynamicPrefabDecorator)
	{
		this.prefabCache = prefabCache;
		this.dpd = dynamicPrefabDecorator;
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x001A9C04 File Offset: 0x001A7E04
	public void TryAdd(int id, string prefabName, byte rotation, Vector3i position)
	{
		if (this.dpd.GetPrefab(id) != null)
		{
			Log.Error(string.Format("[{0}] Cannot add prefab, prefab already exists with id {1}", "EventPrefabsClient", id));
			return;
		}
		Prefab prefabRotated = this.prefabCache.GetPrefabRotated(prefabName, (int)rotation, true, true, false, false);
		if (prefabRotated == null)
		{
			Log.Error("[EventPrefabsClient] Could not load prefab '" + prefabName + "'");
			return;
		}
		Log.Out(string.Format("EventPrefabsClient Add {0} {1}", id, prefabName));
		PrefabInstance prefabInstance = new PrefabInstance(id, prefabRotated.location, position, rotation, prefabRotated, 0);
		this.dpd.AddEventPrefab(prefabInstance);
		DecoManager.Instance.ClearDecoObjectsInArea(prefabInstance.boundingBoxPosition, prefabInstance.boundingBoxSize);
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x001A9CB0 File Offset: 0x001A7EB0
	public void Remove(int id, string prefabName, byte rotation, Vector3i position)
	{
		PrefabInstance prefab = this.dpd.GetPrefab(id);
		if (prefab == null)
		{
			Log.Error(string.Format("[{0}] Could not find prefab with id {1} to remove", "EventPrefabsClient", id));
			return;
		}
		if (prefab.prefab.PrefabName != prefabName)
		{
			Log.Error(string.Format("[{0}] trying to remove prefab with id {1} but it has a different name. Looking for: {2}, found: {3}", new object[]
			{
				"EventPrefabsClient",
				id,
				prefabName,
				prefab.prefab.PrefabName
			}));
			return;
		}
		if (prefab.boundingBoxPosition != position)
		{
			Log.Error(string.Format("[{0}] trying to remove prefab with id {1} but it has a different position. Looking for: {2}, found: {3}", new object[] { "EventPrefabsClient", id, position, prefab.boundingBoxPosition }));
			return;
		}
		if (prefab.rotation != rotation)
		{
			Log.Error(string.Format("[{0}] trying to remove prefab with id {1} but it has a different rotation. Looking for: {2}, found: {3}", new object[] { "EventPrefabsClient", id, rotation, prefab.rotation }));
			return;
		}
		Log.Out(string.Format("EventPrefabsClient Remove {0} {1}", id, prefabName));
		this.dpd.RemoveEventPrefab(prefab);
	}

	// Token: 0x040036FE RID: 14078
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabCache prefabCache;

	// Token: 0x040036FF RID: 14079
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicPrefabDecorator dpd;
}
