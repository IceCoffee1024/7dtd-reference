using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001407 RID: 5127
public class GameObjectPool
{
	// Token: 0x170012E9 RID: 4841
	// (get) Token: 0x0600A129 RID: 41257 RVA: 0x003C7708 File Offset: 0x003C5908
	public static GameObjectPool Instance
	{
		get
		{
			if (GameObjectPool.instance == null)
			{
				GameObjectPool.Instantiate();
			}
			return GameObjectPool.instance;
		}
	}

	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x0600A12A RID: 41258 RVA: 0x003C771B File Offset: 0x003C591B
	// (set) Token: 0x0600A12B RID: 41259 RVA: 0x003C7723 File Offset: 0x003C5923
	public int MaxPooledInstancesPerItem
	{
		get
		{
			return this.maxPooledInstancesPerItem;
		}
		set
		{
			this.maxPooledInstancesPerItem = value;
			Log.Out(string.Format("[GameObjectPool] {0} set to {1}", "MaxPooledInstancesPerItem", this.maxPooledInstancesPerItem));
		}
	}

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x0600A12C RID: 41260 RVA: 0x003C774B File Offset: 0x003C594B
	// (set) Token: 0x0600A12D RID: 41261 RVA: 0x003C7753 File Offset: 0x003C5953
	public int MaxDestroysPerUpdate
	{
		get
		{
			return this.maxDestroysPerUpdate;
		}
		set
		{
			this.maxDestroysPerUpdate = value;
			Log.Out(string.Format("[GameObjectPool] {0} set to {1}", "MaxDestroysPerUpdate", this.maxDestroysPerUpdate));
		}
	}

	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x0600A12E RID: 41262 RVA: 0x003C777B File Offset: 0x003C597B
	// (set) Token: 0x0600A12F RID: 41263 RVA: 0x003C7783 File Offset: 0x003C5983
	public GameObjectPool.ShrinkThreshold ShrinkThresholdHigh
	{
		get
		{
			return this.shrinkThresholdHigh;
		}
		set
		{
			this.shrinkThresholdHigh = value;
			Log.Out(string.Format("[GameObjectPool] {0} set to {1}", "ShrinkThresholdHigh", this.shrinkThresholdHigh));
		}
	}

	// Token: 0x170012ED RID: 4845
	// (get) Token: 0x0600A130 RID: 41264 RVA: 0x003C77AB File Offset: 0x003C59AB
	// (set) Token: 0x0600A131 RID: 41265 RVA: 0x003C77B3 File Offset: 0x003C59B3
	public GameObjectPool.ShrinkThreshold ShrinkThresholdMedium
	{
		get
		{
			return this.shrinkThresholdMedium;
		}
		set
		{
			this.shrinkThresholdMedium = value;
			Log.Out(string.Format("[GameObjectPool] {0} set to {1}", "ShrinkThresholdMedium", this.shrinkThresholdMedium));
		}
	}

	// Token: 0x170012EE RID: 4846
	// (get) Token: 0x0600A132 RID: 41266 RVA: 0x003C77DB File Offset: 0x003C59DB
	// (set) Token: 0x0600A133 RID: 41267 RVA: 0x003C77E3 File Offset: 0x003C59E3
	public GameObjectPool.ShrinkThreshold ShrinkThresholdLow
	{
		get
		{
			return this.shrinkThresholdLow;
		}
		set
		{
			this.shrinkThresholdLow = value;
			Log.Out(string.Format("[GameObjectPool] {0} set to {1}", "ShrinkThresholdLow", this.shrinkThresholdLow));
		}
	}

	// Token: 0x0600A134 RID: 41268 RVA: 0x003C780B File Offset: 0x003C5A0B
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Instantiate()
	{
		GameObjectPool.instance = new GameObjectPool();
	}

	// Token: 0x0600A135 RID: 41269 RVA: 0x003C7817 File Offset: 0x003C5A17
	public void Init()
	{
		PlatformOptimizations.ConfigureGameObjectPoolForPlatform(this);
		this.tintMaskShader = GlobalAssets.FindShader("Game/Entity Tint Mask");
	}

	// Token: 0x0600A136 RID: 41270 RVA: 0x003C7830 File Offset: 0x003C5A30
	public void Cleanup()
	{
		foreach (KeyValuePair<string, GameObjectPool.PoolItem> keyValuePair in this.pool)
		{
			List<GameObject> objs = keyValuePair.Value.objs;
			for (int i = 0; i < objs.Count; i++)
			{
				this.DestroyObject(objs[i].gameObject);
			}
			objs.Clear();
		}
		this.activePool.Clear();
		for (int j = 0; j < this.asyncItems.Count; j++)
		{
			GameObjectPool.AsyncItem asyncItem = this.asyncItems[j];
			if (!asyncItem.async.isDone)
			{
				asyncItem.async.Cancel();
			}
		}
		this.asyncItems.Clear();
	}

	// Token: 0x0600A137 RID: 41271 RVA: 0x003C790C File Offset: 0x003C5B0C
	public void FrameUpdate()
	{
		float time = Time.time;
		int num = 0;
		for (int i = this.activePool.Count - 1; i >= 0; i--)
		{
			GameObjectPool.PoolItem poolItem = this.activePool[i];
			if (poolItem.updateTime - time <= 0f)
			{
				int num2 = poolItem.objs.Count;
				if (num2 <= 0)
				{
					if (poolItem.activeCount <= 0)
					{
						this.activePool.RemoveAt(i);
					}
				}
				else
				{
					GameObjectPool.ShrinkThreshold shrinkThreshold = this.GetShrinkThreshold(num2);
					poolItem.updateTime = time + shrinkThreshold.Delay;
					int num3 = Mathf.Min(num2, shrinkThreshold.DestroyCount);
					int num4 = 0;
					while (num4 < num3 && num < this.maxDestroysPerUpdate)
					{
						num2--;
						GameObject gameObject = poolItem.objs[num2];
						poolItem.objs.RemoveAt(num2);
						poolItem.activeCount--;
						this.DestroyObject(gameObject);
						num++;
						num4++;
					}
					if (num >= this.maxDestroysPerUpdate)
					{
						break;
					}
				}
			}
		}
		for (int j = this.asyncItems.Count - 1; j >= 0; j--)
		{
			GameObjectPool.AsyncItem asyncItem = this.asyncItems[j];
			if (asyncItem.async.isDone)
			{
				global::UnityEngine.Object[] result = asyncItem.async.Result;
				int num5 = result.Length;
				GameObjectPool.PoolItem item = asyncItem.item;
				item.activeCount += num5;
				for (int k = 0; k < num5; k++)
				{
					GameObject gameObject2 = (GameObject)result[k];
					gameObject2.name = item.name;
					if (item.createOnceToAllCallback != null)
					{
						item.createOnceToAllCallback(gameObject2);
						item.createOnceToAllCallback = null;
					}
					if (item.createCallback != null)
					{
						item.createCallback(gameObject2);
					}
				}
				asyncItem.callback(asyncItem.userData, result, num5, true);
				this.asyncItems.RemoveAt(j);
			}
		}
	}

	// Token: 0x0600A138 RID: 41272 RVA: 0x003C7B0C File Offset: 0x003C5D0C
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold GetShrinkThreshold(int count)
	{
		if (count >= this.shrinkThresholdHigh.Count)
		{
			return this.shrinkThresholdHigh;
		}
		if (count >= this.shrinkThresholdMedium.Count)
		{
			return this.shrinkThresholdMedium;
		}
		if (count >= this.shrinkThresholdLow.Count)
		{
			return this.shrinkThresholdLow;
		}
		return this.shrinkThresholdMin;
	}

	// Token: 0x0600A139 RID: 41273 RVA: 0x003C7B60 File Offset: 0x003C5D60
	public void AddPooledObject(string name, GameObjectPool.LoadCallback _loadCallback, GameObjectPool.CreateCallback _createOnceToAllCallback, GameObjectPool.CreateCallback _createCallback)
	{
		GameObjectPool.PoolItem poolItem;
		if (!this.pool.TryGetValue(name, out poolItem))
		{
			poolItem = new GameObjectPool.PoolItem();
			poolItem.name = name;
			poolItem.loadCallback = _loadCallback;
			poolItem.createOnceToAllCallback = _createOnceToAllCallback;
			poolItem.createCallback = _createCallback;
			poolItem.objs = new List<GameObject>();
			this.pool.Add(name, poolItem);
		}
		else
		{
			GameObjectPool.PoolItem poolItem2 = poolItem;
			poolItem2.createOnceToAllCallback = (GameObjectPool.CreateCallback)Delegate.Combine(poolItem2.createOnceToAllCallback, _createOnceToAllCallback);
		}
		Transform transform = poolItem.loadCallback();
		if (transform)
		{
			this.setItemPrefab(poolItem, transform.gameObject);
		}
	}

	// Token: 0x0600A13A RID: 41274 RVA: 0x003C7BF2 File Offset: 0x003C5DF2
	[PublicizedFrom(EAccessModifier.Private)]
	public void setItemPrefab(GameObjectPool.PoolItem item, GameObject go)
	{
		item.prefab = go;
		this.getOriginalTint(item, go);
	}

	// Token: 0x0600A13B RID: 41275 RVA: 0x003C7C04 File Offset: 0x003C5E04
	[PublicizedFrom(EAccessModifier.Private)]
	public void getOriginalTint(GameObjectPool.PoolItem item, GameObject go)
	{
		bool flag = false;
		List<Color> list = new List<Color>();
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			foreach (Material material in componentsInChildren[i].sharedMaterials)
			{
				if (!(material != null) || !(material.shader == this.tintMaskShader))
				{
					flag = true;
					break;
				}
				list.Add(material.color);
			}
		}
		item.originalTint = Color.clear;
		if (!flag && list.Count > 0)
		{
			int num = 1;
			while (num < list.Count && !(list[0] != list[num]))
			{
				num++;
			}
			if (num == list.Count)
			{
				item.originalTint = list[0];
			}
		}
	}

	// Token: 0x0600A13C RID: 41276 RVA: 0x003C7CDC File Offset: 0x003C5EDC
	public GameObject GetObjectForType(string objectType)
	{
		Color color;
		return this.GetObjectForType(objectType, out color);
	}

	// Token: 0x0600A13D RID: 41277 RVA: 0x003C7CF4 File Offset: 0x003C5EF4
	public GameObject GetObjectForType(string objectType, out Color originalTint)
	{
		GameObjectPool.PoolItem poolItem;
		if (!this.pool.TryGetValue(objectType, out poolItem))
		{
			Log.Error("GameObjectPool GetObjectForType {0} unknown", new object[] { objectType });
			originalTint = Color.white;
			return null;
		}
		GameObject gameObject = poolItem.prefab;
		if (!gameObject)
		{
			Transform transform = poolItem.loadCallback();
			if (transform)
			{
				gameObject = transform.gameObject;
				this.setItemPrefab(poolItem, gameObject);
			}
		}
		originalTint = poolItem.originalTint;
		if (!gameObject)
		{
			return null;
		}
		List<GameObject> objs = poolItem.objs;
		int count = objs.Count;
		if (count > 0)
		{
			poolItem.updateTime = Time.time + 5f;
			GameObject gameObject2 = objs[count - 1];
			objs.RemoveAt(count - 1);
			return gameObject2;
		}
		return poolItem.Instantiate();
	}

	// Token: 0x0600A13E RID: 41278 RVA: 0x003C7DC0 File Offset: 0x003C5FC0
	public GameObjectPool.AsyncItem GetObjectsForTypeAsync(string objectType, int _count, GameObjectPool.CreateAsyncCallback _callback, object _userData)
	{
		GameObjectPool.PoolItem poolItem;
		if (!this.pool.TryGetValue(objectType, out poolItem))
		{
			Log.Error("GameObjectPool GetObjectForType {0} unknown", new object[] { objectType });
			return null;
		}
		GameObject gameObject = poolItem.prefab;
		if (!gameObject)
		{
			Transform transform = poolItem.loadCallback();
			if (transform)
			{
				gameObject = transform.gameObject;
				this.setItemPrefab(poolItem, gameObject);
			}
		}
		if (!gameObject)
		{
			return null;
		}
		List<GameObject> objs = poolItem.objs;
		int count = objs.Count;
		if (count >= _count && count <= 128)
		{
			poolItem.updateTime = Time.time + 5f;
			for (int i = 0; i < _count; i++)
			{
				int num = count - 1 - i;
				GameObject gameObject2 = objs[num];
				objs.RemoveAt(num);
				this.asyncPoolObjs[i] = gameObject2;
			}
			global::UnityEngine.Object[] array = this.asyncPoolObjs;
			_callback(_userData, array, _count, false);
			return null;
		}
		if (_count <= 3)
		{
			for (int j = 0; j < _count; j++)
			{
				GameObject gameObject3 = poolItem.Instantiate();
				this.asyncPoolObjs[j] = gameObject3;
			}
			global::UnityEngine.Object[] array = this.asyncPoolObjs;
			_callback(_userData, array, _count, false);
			return null;
		}
		GameObjectPool.AsyncItem asyncItem = new GameObjectPool.AsyncItem();
		asyncItem.item = poolItem;
		asyncItem.callback = _callback;
		asyncItem.userData = _userData;
		asyncItem.async = global::UnityEngine.Object.InstantiateAsync<GameObject>(gameObject, _count);
		this.asyncItems.Add(asyncItem);
		return asyncItem;
	}

	// Token: 0x0600A13F RID: 41279 RVA: 0x003C7F28 File Offset: 0x003C6128
	public void CancelAsync(GameObjectPool.AsyncItem _ai)
	{
		if (this.asyncItems.Remove(_ai))
		{
			if (_ai.async.isDone)
			{
				global::UnityEngine.Object[] result = _ai.async.Result;
				for (int i = 0; i < result.Length; i++)
				{
					global::UnityEngine.Object.Destroy(result[i]);
				}
				return;
			}
			_ai.async.Cancel();
		}
	}

	// Token: 0x0600A140 RID: 41280 RVA: 0x003C7F7E File Offset: 0x003C617E
	public void PoolObjectAsync(GameObject obj)
	{
		this.PoolObject(obj);
	}

	// Token: 0x0600A141 RID: 41281 RVA: 0x003C7F88 File Offset: 0x003C6188
	public void PoolObject(GameObject obj)
	{
		if (!obj)
		{
			return;
		}
		string name = obj.name;
		GameObjectPool.PoolItem poolItem;
		if (!this.pool.TryGetValue(name, out poolItem))
		{
			return;
		}
		List<GameObject> objs = poolItem.objs;
		if (objs.Count < this.maxPooledInstancesPerItem)
		{
			obj.SetActive(false);
			obj.transform.SetParent(null, false);
			objs.Add(obj);
			if (objs.Count >= 1 && !this.activePool.Contains(poolItem))
			{
				this.activePool.Add(poolItem);
				return;
			}
		}
		else
		{
			poolItem.activeCount--;
			obj.SetActive(false);
			this.DestroyObject(obj);
		}
	}

	// Token: 0x0600A142 RID: 41282 RVA: 0x003C8026 File Offset: 0x003C6226
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyObject(GameObject obj)
	{
		obj.GetComponentsInChildren<Renderer>(this.tempRenderers);
		Utils.CleanupMaterialsOfRenderers<List<Renderer>>(this.tempRenderers);
		this.tempRenderers.Clear();
		global::UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x0600A143 RID: 41283 RVA: 0x003C8050 File Offset: 0x003C6250
	public void CmdList(string _mode)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Pool objects:");
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		foreach (KeyValuePair<string, GameObjectPool.PoolItem> keyValuePair in this.pool)
		{
			GameObjectPool.PoolItem value = keyValuePair.Value;
			if (value.prefab != null)
			{
				list.Add(value.prefab);
			}
			num += value.activeCount;
			num2 += value.objs.Count;
			if (value.activeCount > 0)
			{
				num3++;
				list2.Add(value.prefab);
			}
			if (_mode == "all" || (_mode == "active" && value.activeCount > 0))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" {0}, prefab {1}, active {2}, count {3}", new object[]
				{
					value.name,
					value.prefab ? "1" : "0",
					value.activeCount,
					value.objs.Count
				});
			}
		}
		string text = string.Format(" types {0}, used {1}, pooled {2}, active {3}", new object[]
		{
			this.pool.Count,
			num3,
			num2,
			num
		});
		if (Application.isEditor)
		{
			long num4;
			long num5;
			ProfilerUtils.CalculateDependentBytes(list.ToArray(), out num4, out num5);
			long num6;
			long num7;
			ProfilerUtils.CalculateDependentBytes(list2.ToArray(), out num6, out num7);
			text += string.Format(", used mesh {0:F2} MB, used texture {1:F2} MB, required mesh {2:F2} MB, required texture {3:F2} MB", new object[]
			{
				(double)num4 * 9.5367431640625E-07,
				(double)num5 * 9.5367431640625E-07,
				(double)num6 * 9.5367431640625E-07,
				(double)num7 * 9.5367431640625E-07
			});
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(text);
	}

	// Token: 0x0600A144 RID: 41284 RVA: 0x003C8288 File Offset: 0x003C6488
	public void CmdShrink()
	{
		bool flag;
		do
		{
			flag = false;
			for (int i = this.activePool.Count - 1; i >= 0; i--)
			{
				GameObjectPool.PoolItem poolItem = this.activePool[i];
				if (poolItem.objs.Count > 0)
				{
					poolItem.updateTime = 0f;
					this.FrameUpdate();
					flag = true;
					break;
				}
			}
		}
		while (flag);
	}

	// Token: 0x0600A145 RID: 41285 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerBegin(string _name)
	{
	}

	// Token: 0x0600A146 RID: 41286 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerEnd()
	{
	}

	// Token: 0x0400794F RID: 31055
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolAddAtCount = 1;

	// Token: 0x04007950 RID: 31056
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolMinCount = 0;

	// Token: 0x04007951 RID: 31057
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivePoolRemoveDelay = 10f;

	// Token: 0x04007952 RID: 31058
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObjectPool instance;

	// Token: 0x04007953 RID: 31059
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader tintMaskShader;

	// Token: 0x04007954 RID: 31060
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, GameObjectPool.PoolItem> pool = new Dictionary<string, GameObjectPool.PoolItem>();

	// Token: 0x04007955 RID: 31061
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.PoolItem> activePool = new List<GameObjectPool.PoolItem>();

	// Token: 0x04007956 RID: 31062
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.AsyncItem> asyncItems = new List<GameObjectPool.AsyncItem>();

	// Token: 0x04007957 RID: 31063
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAsyncPoolObjsCount = 128;

	// Token: 0x04007958 RID: 31064
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] asyncPoolObjs = new GameObject[128];

	// Token: 0x04007959 RID: 31065
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> tempRenderers = new List<Renderer>();

	// Token: 0x0400795A RID: 31066
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxPooledInstancesPerItem = 200;

	// Token: 0x0400795B RID: 31067
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxDestroysPerUpdate = 1;

	// Token: 0x0400795C RID: 31068
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdHigh = new GameObjectPool.ShrinkThreshold(100, 1, 0.1f);

	// Token: 0x0400795D RID: 31069
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMedium = new GameObjectPool.ShrinkThreshold(40, 1, 0.5f);

	// Token: 0x0400795E RID: 31070
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdLow = new GameObjectPool.ShrinkThreshold(12, 1, 3f);

	// Token: 0x0400795F RID: 31071
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMin = new GameObjectPool.ShrinkThreshold(0, 1, 10f);

	// Token: 0x02001408 RID: 5128
	// (Invoke) Token: 0x0600A149 RID: 41289
	public delegate Transform LoadCallback();

	// Token: 0x02001409 RID: 5129
	// (Invoke) Token: 0x0600A14D RID: 41293
	public delegate void CreateCallback(GameObject obj);

	// Token: 0x0200140A RID: 5130
	// (Invoke) Token: 0x0600A151 RID: 41297
	public delegate void CreateAsyncCallback(object _userData, global::UnityEngine.Object[] _objs, int _objsCount, bool _isAsync);

	// Token: 0x0200140B RID: 5131
	public class PoolItem
	{
		// Token: 0x0600A154 RID: 41300 RVA: 0x003C8390 File Offset: 0x003C6590
		public GameObject Instantiate()
		{
			this.activeCount++;
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			gameObject.name = this.name;
			if (this.createOnceToAllCallback != null)
			{
				this.createOnceToAllCallback(gameObject);
				this.createOnceToAllCallback = null;
			}
			if (this.createCallback != null)
			{
				this.createCallback(gameObject);
			}
			return gameObject;
		}

		// Token: 0x04007960 RID: 31072
		public string name;

		// Token: 0x04007961 RID: 31073
		public GameObject prefab;

		// Token: 0x04007962 RID: 31074
		public GameObjectPool.LoadCallback loadCallback;

		// Token: 0x04007963 RID: 31075
		public GameObjectPool.CreateCallback createOnceToAllCallback;

		// Token: 0x04007964 RID: 31076
		public GameObjectPool.CreateCallback createCallback;

		// Token: 0x04007965 RID: 31077
		public List<GameObject> objs;

		// Token: 0x04007966 RID: 31078
		public float updateTime;

		// Token: 0x04007967 RID: 31079
		public int activeCount;

		// Token: 0x04007968 RID: 31080
		public Color originalTint;
	}

	// Token: 0x0200140C RID: 5132
	public class AsyncItem
	{
		// Token: 0x04007969 RID: 31081
		public GameObjectPool.PoolItem item;

		// Token: 0x0400796A RID: 31082
		public GameObjectPool.CreateAsyncCallback callback;

		// Token: 0x0400796B RID: 31083
		public AsyncInstantiateOperation async;

		// Token: 0x0400796C RID: 31084
		public object userData;
	}

	// Token: 0x0200140D RID: 5133
	public struct ShrinkThreshold
	{
		// Token: 0x0600A157 RID: 41303 RVA: 0x003C83F3 File Offset: 0x003C65F3
		public ShrinkThreshold(int count, int destroyCount, float delay)
		{
			this.Count = count;
			this.DestroyCount = destroyCount;
			this.Delay = delay;
		}

		// Token: 0x0600A158 RID: 41304 RVA: 0x003C840C File Offset: 0x003C660C
		public override string ToString()
		{
			return string.Format("({0} = {1}, {2} = {3}, {4} = {5:F2}s)", new object[] { "Count", this.Count, "DestroyCount", this.DestroyCount, "Delay", this.Delay });
		}

		// Token: 0x0400796D RID: 31085
		public int Count;

		// Token: 0x0400796E RID: 31086
		public int DestroyCount;

		// Token: 0x0400796F RID: 31087
		public float Delay;
	}
}
