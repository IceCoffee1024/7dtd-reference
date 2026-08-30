using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001587 RID: 5511
[Preserve]
public class vp_PoolManager : MonoBehaviour
{
	// Token: 0x170013EB RID: 5099
	// (get) Token: 0x0600AC66 RID: 44134 RVA: 0x00402E98 File Offset: 0x00401098
	[Preserve]
	public static vp_PoolManager Instance
	{
		get
		{
			return vp_PoolManager.m_Instance;
		}
	}

	// Token: 0x0600AC67 RID: 44135 RVA: 0x00402E9F File Offset: 0x0040109F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		vp_PoolManager.m_Instance = this;
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AC68 RID: 44136 RVA: 0x00402EB4 File Offset: 0x004010B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		foreach (vp_PoolManager.vp_CustomPooledObject vp_CustomPooledObject in this.CustomPrefabs)
		{
			this.AddObjects(vp_CustomPooledObject.Prefab, Vector3.zero, Quaternion.identity, vp_CustomPooledObject.Buffer);
		}
	}

	// Token: 0x0600AC69 RID: 44137 RVA: 0x00402F1C File Offset: 0x0040111C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Register("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Register("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AC6A RID: 44138 RVA: 0x00402F4C File Offset: 0x0040114C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Unregister("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Unregister("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AC6B RID: 44139 RVA: 0x00402F7C File Offset: 0x0040117C
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

	// Token: 0x0600AC6C RID: 44140 RVA: 0x00403028 File Offset: 0x00401228
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

	// Token: 0x0600AC6D RID: 44141 RVA: 0x004031A8 File Offset: 0x004013A8
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

	// Token: 0x04007F93 RID: 32659
	public int MaxAmount = 25;

	// Token: 0x04007F94 RID: 32660
	public bool PoolOnDestroy = true;

	// Token: 0x04007F95 RID: 32661
	public List<GameObject> IgnoredPrefabs = new List<GameObject>();

	// Token: 0x04007F96 RID: 32662
	public List<vp_PoolManager.vp_CustomPooledObject> CustomPrefabs = new List<vp_PoolManager.vp_CustomPooledObject>();

	// Token: 0x04007F97 RID: 32663
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007F98 RID: 32664
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_AvailableObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x04007F99 RID: 32665
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_UsedObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x04007F9A RID: 32666
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_PoolManager m_Instance;

	// Token: 0x02001588 RID: 5512
	[Serializable]
	public class vp_CustomPooledObject
	{
		// Token: 0x04007F9B RID: 32667
		public GameObject Prefab;

		// Token: 0x04007F9C RID: 32668
		public int Buffer = 15;

		// Token: 0x04007F9D RID: 32669
		public int MaxAmount = 25;
	}
}
