using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015BB RID: 5563
[Preserve]
public class vp_PoolManager : MonoBehaviour
{
	// Token: 0x1700143C RID: 5180
	// (get) Token: 0x0600AD71 RID: 44401 RVA: 0x0040A0F0 File Offset: 0x004082F0
	[Preserve]
	public static vp_PoolManager Instance
	{
		get
		{
			return vp_PoolManager.m_Instance;
		}
	}

	// Token: 0x0600AD72 RID: 44402 RVA: 0x0040A0F7 File Offset: 0x004082F7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		vp_PoolManager.m_Instance = this;
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AD73 RID: 44403 RVA: 0x0040A10C File Offset: 0x0040830C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		foreach (vp_PoolManager.vp_CustomPooledObject vp_CustomPooledObject in this.CustomPrefabs)
		{
			this.AddObjects(vp_CustomPooledObject.Prefab, Vector3.zero, Quaternion.identity, vp_CustomPooledObject.Buffer);
		}
	}

	// Token: 0x0600AD74 RID: 44404 RVA: 0x0040A174 File Offset: 0x00408374
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Register("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Register("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AD75 RID: 44405 RVA: 0x0040A1A4 File Offset: 0x004083A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Unregister("vp_PoolManager Instantiate", new vp_GlobalCallbackReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>(this.InstantiateInternal));
		vp_GlobalEvent<global::UnityEngine.Object, float>.Unregister("vp_PoolManager Destroy", new vp_GlobalCallback<global::UnityEngine.Object, float>(this.DestroyInternal));
	}

	// Token: 0x0600AD76 RID: 44406 RVA: 0x0040A1D4 File Offset: 0x004083D4
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

	// Token: 0x0600AD77 RID: 44407 RVA: 0x0040A280 File Offset: 0x00408480
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

	// Token: 0x0600AD78 RID: 44408 RVA: 0x0040A400 File Offset: 0x00408600
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

	// Token: 0x04008057 RID: 32855
	public int MaxAmount = 25;

	// Token: 0x04008058 RID: 32856
	public bool PoolOnDestroy = true;

	// Token: 0x04008059 RID: 32857
	public List<GameObject> IgnoredPrefabs = new List<GameObject>();

	// Token: 0x0400805A RID: 32858
	public List<vp_PoolManager.vp_CustomPooledObject> CustomPrefabs = new List<vp_PoolManager.vp_CustomPooledObject>();

	// Token: 0x0400805B RID: 32859
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400805C RID: 32860
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_AvailableObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x0400805D RID: 32861
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, List<global::UnityEngine.Object>> m_UsedObjects = new Dictionary<string, List<global::UnityEngine.Object>>();

	// Token: 0x0400805E RID: 32862
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_PoolManager m_Instance;

	// Token: 0x020015BC RID: 5564
	[Serializable]
	public class vp_CustomPooledObject
	{
		// Token: 0x0400805F RID: 32863
		public GameObject Prefab;

		// Token: 0x04008060 RID: 32864
		public int Buffer = 15;

		// Token: 0x04008061 RID: 32865
		public int MaxAmount = 25;
	}
}
