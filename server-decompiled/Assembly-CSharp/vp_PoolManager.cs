using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015A9 RID: 5545
[Preserve]
public class vp_PoolManager : MonoBehaviour
{
	// Token: 0x17001430 RID: 5168
	// (get) Token: 0x0600AD19 RID: 44313 RVA: 0x00407CEC File Offset: 0x00405EEC
	[Preserve]
	public static vp_PoolManager Instance
	{
		get
		{
			return vp_PoolManager.m_Instance;
		}
	}

	// Token: 0x0600AD1A RID: 44314 RVA: 0x00407CF3 File Offset: 0x00405EF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		vp_PoolManager.m_Instance = this;
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AD1B RID: 44315 RVA: 0x00407D08 File Offset: 0x00405F08
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		foreach (vp_PoolManager.vp_CustomPooledObject vp_CustomPooledObject in this.CustomPrefabs)
		{
			this.AddObjects(vp_CustomPooledObject.Prefab, Vector3.zero, Quaternion.identity, vp_CustomPooledObject.Buffer);
		}
	}

	// Token: 0x0600AD1C RID: 44316 RVA: 0x00407D70 File Offset: 0x00405F70
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Register("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Register("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AD1D RID: 44317 RVA: 0x00407DA0 File Offset: 0x00405FA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Unregister("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Unregister("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AD1E RID: 44318 RVA: 0x00407DD0 File Offset: 0x00405FD0
	public virtual void AddObjects(global::UnityEngine.Object obj, Vector3 position, Quaternion rotation, int amount = 1)
	{
		if (obj == null)
		{
			return;
		}
		if (!this.m_AvailableObjects.ContainsKey(obj.name))
		{
			this.m_AvailableObjects.Add(obj.name, new List<global::UnityEngine.Object>());
			this.m_UsedObjects.Add(obj.name, new List<global::UnityEngine.Object>());
		}
		for (int i = 0; i < amount; i++)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(obj, position, rotation) as GameObject;
			gameObject.name = obj.name;
			gameObject.transform.parent = this.m_Transform;
			vp_Utility.Activate(gameObject, false);
			this.m_AvailableObjects[obj.name].Add(gameObject);
		}
	}

	// Token: 0x0600AD1F RID: 44319 RVA: 0x00407E7C File Offset: 0x0040607C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual global::UnityEngine.Object InstantiateInternal(global::UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		if (this.IgnoredPrefabs.FirstOrDefault<GameObject>((GameObject obj) => obj.name == original.name || obj.name == original.name + "(Clone)") != null)
		{
			return global::UnityEngine.Object.Instantiate(original, position, rotation);
		}
		List<global::UnityEngine.Object> list = null;
		List<global::UnityEngine.Object> list2 = null;
		if (this.m_AvailableObjects.TryGetValue(original.name, out list))
		{
			GameObject gameObject;
			for (;;)
			{
				this.m_UsedObjects.TryGetValue(original.name, out list2);
				int num = list.Count + list2.Count;
				if (this.CustomPrefabs.FirstOrDefault<vp_PoolManager.vp_CustomPooledObject>((vp_PoolManager.vp_CustomPooledObject obj) => obj.Prefab.name == original.name) == null && num < this.MaxAmount && list.Count == 0)
				{
					this.AddObjects(original, position, rotation, 1);
				}
				if (list.Count == 0)
				{
					gameObject = list2.FirstOrDefault<global::UnityEngine.Object>() as GameObject;
					if (gameObject == null)
					{
						list2.Remove(gameObject);
					}
					else
					{
						vp_Utility.Activate(gameObject, false);
						list2.Remove(gameObject);
						list.Add(gameObject);
					}
				}
				else
				{
					gameObject = list.FirstOrDefault<global::UnityEngine.Object>() as GameObject;
					if (!(gameObject == null))
					{
						break;
					}
					list.Remove(gameObject);
				}
			}
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			list.Remove(gameObject);
			list2.Add(gameObject);
			vp_Utility.Activate(gameObject, true);
			return gameObject;
		}
		this.AddObjects(original, position, rotation, 1);
		return this.InstantiateInternal(original, position, rotation);
	}

	// Token: 0x0600AD20 RID: 44320 RVA: 0x00407FFC File Offset: 0x004061FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DestroyInternal(global::UnityEngine.Object obj, float t)
	{
		if (obj == null)
		{
			return;
		}
		if (this.IgnoredPrefabs.FirstOrDefault<GameObject>((GameObject o) => o.name == obj.name || o.name == obj.name + "(Clone)") != null || (!this.m_AvailableObjects.ContainsKey(obj.name) && !this.PoolOnDestroy))
		{
			global::UnityEngine.Object.Destroy(obj, t);
			return;
		}
		if (t != 0f)
		{
			vp_Timer.In(t, delegate
			{
				this.DestroyInternal(obj, 0f);
			}, null);
			return;
		}
		if (!this.m_AvailableObjects.ContainsKey(obj.name))
		{
			this.AddObjects(obj, Vector3.zero, Quaternion.identity, 1);
			return;
		}
		List<global::UnityEngine.Object> list = null;
		List<global::UnityEngine.Object> list2 = null;
		this.m_AvailableObjects.TryGetValue(obj.name, out list);
		this.m_UsedObjects.TryGetValue(obj.name, out list2);
		GameObject gameObject = list2.FirstOrDefault<global::UnityEngine.Object>((global::UnityEngine.Object o) => o.GetInstanceID() == obj.GetInstanceID()) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		gameObject.transform.parent = this.m_Transform;
		vp_Utility.Activate(gameObject, false);
		list2.Remove(gameObject);
		list.Add(gameObject);
	}

	// Token: 0x04008016 RID: 32790
	public int MaxAmount = 25;

	// Token: 0x04008017 RID: 32791
	public bool PoolOnDestroy = true;

	// Token: 0x04008018 RID: 32792
	public List<GameObject> IgnoredPrefabs = new List<GameObject>();

	// Token: 0x04008019 RID: 32793
	public List<vp_PoolManager.vp_CustomPooledObject> CustomPrefabs = new List<vp_PoolManager.vp_CustomPooledObject>();

	// Token: 0x0400801A RID: 32794
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400801B RID: 32795
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_AvailableObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x0400801C RID: 32796
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_UsedObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x0400801D RID: 32797
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_PoolManager m_Instance;

	// Token: 0x020015AA RID: 5546
	[Serializable]
	public class vp_CustomPooledObject
	{
		// Token: 0x0400801E RID: 32798
		public GameObject Prefab;

		// Token: 0x0400801F RID: 32799
		public int Buffer = 15;

		// Token: 0x04008020 RID: 32800
		public int MaxAmount = 25;
	}
}
