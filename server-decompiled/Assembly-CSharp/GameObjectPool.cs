using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001427 RID: 5159
public class GameObjectPool
{
	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x0600A1D4 RID: 41428 RVA: 0x003CC2A4 File Offset: 0x003CA4A4
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

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x0600A1D5 RID: 41429 RVA: 0x003CC2B7 File Offset: 0x003CA4B7
	// (set) Token: 0x0600A1D6 RID: 41430 RVA: 0x003CC2BF File Offset: 0x003CA4BF
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

	// Token: 0x17001330 RID: 4912
	// (get) Token: 0x0600A1D7 RID: 41431 RVA: 0x003CC2E7 File Offset: 0x003CA4E7
	// (set) Token: 0x0600A1D8 RID: 41432 RVA: 0x003CC2EF File Offset: 0x003CA4EF
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

	// Token: 0x17001331 RID: 4913
	// (get) Token: 0x0600A1D9 RID: 41433 RVA: 0x003CC317 File Offset: 0x003CA517
	// (set) Token: 0x0600A1DA RID: 41434 RVA: 0x003CC31F File Offset: 0x003CA51F
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

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x0600A1DB RID: 41435 RVA: 0x003CC347 File Offset: 0x003CA547
	// (set) Token: 0x0600A1DC RID: 41436 RVA: 0x003CC34F File Offset: 0x003CA54F
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

	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x0600A1DD RID: 41437 RVA: 0x003CC377 File Offset: 0x003CA577
	// (set) Token: 0x0600A1DE RID: 41438 RVA: 0x003CC37F File Offset: 0x003CA57F
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

	// Token: 0x0600A1DF RID: 41439 RVA: 0x003CC3A7 File Offset: 0x003CA5A7
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Instantiate()
	{
		GameObjectPool.instance = new GameObjectPool();
	}

	// Token: 0x0600A1E0 RID: 41440 RVA: 0x003CC3B3 File Offset: 0x003CA5B3
	public void Init()
	{
		PlatformOptimizations.ConfigureGameObjectPoolForPlatform(this);
		this.tintMaskShader = GlobalAssets.FindShader("Game/Entity Tint Mask");
	}

	// Token: 0x0600A1E1 RID: 41441 RVA: 0x003CC3CC File Offset: 0x003CA5CC
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

	// Token: 0x0600A1E2 RID: 41442 RVA: 0x003CC4A8 File Offset: 0x003CA6A8
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

	// Token: 0x0600A1E3 RID: 41443 RVA: 0x003CC6A8 File Offset: 0x003CA8A8
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

	// Token: 0x0600A1E4 RID: 41444 RVA: 0x003CC6FC File Offset: 0x003CA8FC
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

	// Token: 0x0600A1E5 RID: 41445 RVA: 0x003CC78E File Offset: 0x003CA98E
	[PublicizedFrom(EAccessModifier.Private)]
	public void setItemPrefab(GameObjectPool.PoolItem item, GameObject go)
	{
		item.prefab = go;
		this.getOriginalTint(item, go);
	}

	// Token: 0x0600A1E6 RID: 41446 RVA: 0x003CC7A0 File Offset: 0x003CA9A0
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

	// Token: 0x0600A1E7 RID: 41447 RVA: 0x003CC878 File Offset: 0x003CAA78
	public GameObject GetObjectForType(string objectType)
	{
		Color color;
		return this.GetObjectForType(objectType, out color);
	}

	// Token: 0x0600A1E8 RID: 41448 RVA: 0x003CC890 File Offset: 0x003CAA90
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

	// Token: 0x0600A1E9 RID: 41449 RVA: 0x003CC95C File Offset: 0x003CAB5C
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

	// Token: 0x0600A1EA RID: 41450 RVA: 0x003CCAC4 File Offset: 0x003CACC4
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

	// Token: 0x0600A1EB RID: 41451 RVA: 0x003CCB1A File Offset: 0x003CAD1A
	public void PoolObjectAsync(GameObject obj)
	{
		this.PoolObject(obj);
	}

	// Token: 0x0600A1EC RID: 41452 RVA: 0x003CCB24 File Offset: 0x003CAD24
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

	// Token: 0x0600A1ED RID: 41453 RVA: 0x003CCBC2 File Offset: 0x003CADC2
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyObject(GameObject obj)
	{
		obj.GetComponentsInChildren<Renderer>(this.tempRenderers);
		Utils.CleanupMaterialsOfRenderers<List<Renderer>>(this.tempRenderers);
		this.tempRenderers.Clear();
		global::UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x0600A1EE RID: 41454 RVA: 0x003CCBEC File Offset: 0x003CADEC
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

	// Token: 0x0600A1EF RID: 41455 RVA: 0x003CCE24 File Offset: 0x003CB024
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

	// Token: 0x0600A1F0 RID: 41456 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerBegin(string _name)
	{
	}

	// Token: 0x0600A1F1 RID: 41457 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerEnd()
	{
	}

	// Token: 0x040079CA RID: 31178
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolAddAtCount = 1;

	// Token: 0x040079CB RID: 31179
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolMinCount = 0;

	// Token: 0x040079CC RID: 31180
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivePoolRemoveDelay = 10f;

	// Token: 0x040079CD RID: 31181
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObjectPool instance;

	// Token: 0x040079CE RID: 31182
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader tintMaskShader;

	// Token: 0x040079CF RID: 31183
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, GameObjectPool.PoolItem> pool = new Dictionary<string, GameObjectPool.PoolItem>();

	// Token: 0x040079D0 RID: 31184
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.PoolItem> activePool = new List<GameObjectPool.PoolItem>();

	// Token: 0x040079D1 RID: 31185
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.AsyncItem> asyncItems = new List<GameObjectPool.AsyncItem>();

	// Token: 0x040079D2 RID: 31186
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAsyncPoolObjsCount = 128;

	// Token: 0x040079D3 RID: 31187
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] asyncPoolObjs = new GameObject[128];

	// Token: 0x040079D4 RID: 31188
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> tempRenderers = new List<Renderer>();

	// Token: 0x040079D5 RID: 31189
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxPooledInstancesPerItem = 200;

	// Token: 0x040079D6 RID: 31190
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxDestroysPerUpdate = 1;

	// Token: 0x040079D7 RID: 31191
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdHigh = new GameObjectPool.ShrinkThreshold(100, 1, 0.1f);

	// Token: 0x040079D8 RID: 31192
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMedium = new GameObjectPool.ShrinkThreshold(40, 1, 0.5f);

	// Token: 0x040079D9 RID: 31193
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdLow = new GameObjectPool.ShrinkThreshold(12, 1, 3f);

	// Token: 0x040079DA RID: 31194
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMin = new GameObjectPool.ShrinkThreshold(0, 1, 10f);

	// Token: 0x02001428 RID: 5160
	// (Invoke) Token: 0x0600A1F4 RID: 41460
	public delegate Transform LoadCallback();

	// Token: 0x02001429 RID: 5161
	// (Invoke) Token: 0x0600A1F8 RID: 41464
	public delegate void CreateCallback(GameObject obj);

	// Token: 0x0200142A RID: 5162
	// (Invoke) Token: 0x0600A1FC RID: 41468
	public delegate void CreateAsyncCallback(object _userData, global::UnityEngine.Object[] _objs, int _objsCount, bool _isAsync);

	// Token: 0x0200142B RID: 5163
	public class PoolItem
	{
		// Token: 0x0600A1FF RID: 41471 RVA: 0x003CCF2C File Offset: 0x003CB12C
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

		// Token: 0x040079DB RID: 31195
		public string name;

		// Token: 0x040079DC RID: 31196
		public GameObject prefab;

		// Token: 0x040079DD RID: 31197
		public GameObjectPool.LoadCallback loadCallback;

		// Token: 0x040079DE RID: 31198
		public GameObjectPool.CreateCallback createOnceToAllCallback;

		// Token: 0x040079DF RID: 31199
		public GameObjectPool.CreateCallback createCallback;

		// Token: 0x040079E0 RID: 31200
		public List<GameObject> objs;

		// Token: 0x040079E1 RID: 31201
		public float updateTime;

		// Token: 0x040079E2 RID: 31202
		public int activeCount;

		// Token: 0x040079E3 RID: 31203
		public Color originalTint;
	}

	// Token: 0x0200142C RID: 5164
	public class AsyncItem
	{
		// Token: 0x040079E4 RID: 31204
		public GameObjectPool.PoolItem item;

		// Token: 0x040079E5 RID: 31205
		public GameObjectPool.CreateAsyncCallback callback;

		// Token: 0x040079E6 RID: 31206
		public AsyncInstantiateOperation async;

		// Token: 0x040079E7 RID: 31207
		public object userData;
	}

	// Token: 0x0200142D RID: 5165
	public struct ShrinkThreshold
	{
		// Token: 0x0600A202 RID: 41474 RVA: 0x003CCF8F File Offset: 0x003CB18F
		public ShrinkThreshold(int count, int destroyCount, float delay)
		{
			this.Count = count;
			this.DestroyCount = destroyCount;
			this.Delay = delay;
		}

		// Token: 0x0600A203 RID: 41475 RVA: 0x003CCFA8 File Offset: 0x003CB1A8
		public override string ToString()
		{
			return string.Format("({0} = {1}, {2} = {3}, {4} = {5:F2}s)", new object[] { "Count", this.Count, "DestroyCount", this.DestroyCount, "Delay", this.Delay });
		}

		// Token: 0x040079E8 RID: 31208
		public int Count;

		// Token: 0x040079E9 RID: 31209
		public int DestroyCount;

		// Token: 0x040079EA RID: 31210
		public float Delay;
	}
}
