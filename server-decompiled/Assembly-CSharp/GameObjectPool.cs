using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001439 RID: 5177
public class GameObjectPool
{
	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x0600A22C RID: 41516 RVA: 0x003CE6A8 File Offset: 0x003CC8A8
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

	// Token: 0x1700133B RID: 4923
	// (get) Token: 0x0600A22D RID: 41517 RVA: 0x003CE6BB File Offset: 0x003CC8BB
	// (set) Token: 0x0600A22E RID: 41518 RVA: 0x003CE6C3 File Offset: 0x003CC8C3
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

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x0600A22F RID: 41519 RVA: 0x003CE6EB File Offset: 0x003CC8EB
	// (set) Token: 0x0600A230 RID: 41520 RVA: 0x003CE6F3 File Offset: 0x003CC8F3
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

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x0600A231 RID: 41521 RVA: 0x003CE71B File Offset: 0x003CC91B
	// (set) Token: 0x0600A232 RID: 41522 RVA: 0x003CE723 File Offset: 0x003CC923
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

	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x0600A233 RID: 41523 RVA: 0x003CE74B File Offset: 0x003CC94B
	// (set) Token: 0x0600A234 RID: 41524 RVA: 0x003CE753 File Offset: 0x003CC953
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

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x0600A235 RID: 41525 RVA: 0x003CE77B File Offset: 0x003CC97B
	// (set) Token: 0x0600A236 RID: 41526 RVA: 0x003CE783 File Offset: 0x003CC983
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

	// Token: 0x0600A237 RID: 41527 RVA: 0x003CE7AB File Offset: 0x003CC9AB
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Instantiate()
	{
		GameObjectPool.instance = new GameObjectPool();
	}

	// Token: 0x0600A238 RID: 41528 RVA: 0x003CE7B7 File Offset: 0x003CC9B7
	public void Init()
	{
		PlatformOptimizations.ConfigureGameObjectPoolForPlatform(this);
		this.tintMaskShader = GlobalAssets.FindShader("Game/Entity Tint Mask");
	}

	// Token: 0x0600A239 RID: 41529 RVA: 0x003CE7D0 File Offset: 0x003CC9D0
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

	// Token: 0x0600A23A RID: 41530 RVA: 0x003CE8AC File Offset: 0x003CCAAC
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

	// Token: 0x0600A23B RID: 41531 RVA: 0x003CEAAC File Offset: 0x003CCCAC
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

	// Token: 0x0600A23C RID: 41532 RVA: 0x003CEB00 File Offset: 0x003CCD00
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

	// Token: 0x0600A23D RID: 41533 RVA: 0x003CEB92 File Offset: 0x003CCD92
	[PublicizedFrom(EAccessModifier.Private)]
	public void setItemPrefab(GameObjectPool.PoolItem item, GameObject go)
	{
		item.prefab = go;
		this.getOriginalTint(item, go);
	}

	// Token: 0x0600A23E RID: 41534 RVA: 0x003CEBA4 File Offset: 0x003CCDA4
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

	// Token: 0x0600A23F RID: 41535 RVA: 0x003CEC7C File Offset: 0x003CCE7C
	public GameObject GetObjectForType(string objectType)
	{
		Color color;
		return this.GetObjectForType(objectType, out color);
	}

	// Token: 0x0600A240 RID: 41536 RVA: 0x003CEC94 File Offset: 0x003CCE94
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

	// Token: 0x0600A241 RID: 41537 RVA: 0x003CED60 File Offset: 0x003CCF60
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

	// Token: 0x0600A242 RID: 41538 RVA: 0x003CEEC8 File Offset: 0x003CD0C8
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

	// Token: 0x0600A243 RID: 41539 RVA: 0x003CEF1E File Offset: 0x003CD11E
	public void PoolObjectAsync(GameObject obj)
	{
		this.PoolObject(obj);
	}

	// Token: 0x0600A244 RID: 41540 RVA: 0x003CEF28 File Offset: 0x003CD128
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

	// Token: 0x0600A245 RID: 41541 RVA: 0x003CEFC6 File Offset: 0x003CD1C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyObject(GameObject obj)
	{
		obj.GetComponentsInChildren<Renderer>(this.tempRenderers);
		Utils.CleanupMaterialsOfRenderers<List<Renderer>>(this.tempRenderers);
		this.tempRenderers.Clear();
		global::UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x0600A246 RID: 41542 RVA: 0x003CEFF0 File Offset: 0x003CD1F0
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

	// Token: 0x0600A247 RID: 41543 RVA: 0x003CF228 File Offset: 0x003CD428
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

	// Token: 0x0600A248 RID: 41544 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerBegin(string _name)
	{
	}

	// Token: 0x0600A249 RID: 41545 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_GOPOOL_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerEnd()
	{
	}

	// Token: 0x04007A0B RID: 31243
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolAddAtCount = 1;

	// Token: 0x04007A0C RID: 31244
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActivePoolMinCount = 0;

	// Token: 0x04007A0D RID: 31245
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivePoolRemoveDelay = 10f;

	// Token: 0x04007A0E RID: 31246
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObjectPool instance;

	// Token: 0x04007A0F RID: 31247
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader tintMaskShader;

	// Token: 0x04007A10 RID: 31248
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, GameObjectPool.PoolItem> pool = new Dictionary<string, GameObjectPool.PoolItem>();

	// Token: 0x04007A11 RID: 31249
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.PoolItem> activePool = new List<GameObjectPool.PoolItem>();

	// Token: 0x04007A12 RID: 31250
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObjectPool.AsyncItem> asyncItems = new List<GameObjectPool.AsyncItem>();

	// Token: 0x04007A13 RID: 31251
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAsyncPoolObjsCount = 128;

	// Token: 0x04007A14 RID: 31252
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] asyncPoolObjs = new GameObject[128];

	// Token: 0x04007A15 RID: 31253
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Renderer> tempRenderers = new List<Renderer>();

	// Token: 0x04007A16 RID: 31254
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxPooledInstancesPerItem = 200;

	// Token: 0x04007A17 RID: 31255
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxDestroysPerUpdate = 1;

	// Token: 0x04007A18 RID: 31256
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdHigh = new GameObjectPool.ShrinkThreshold(100, 1, 0.1f);

	// Token: 0x04007A19 RID: 31257
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMedium = new GameObjectPool.ShrinkThreshold(40, 1, 0.5f);

	// Token: 0x04007A1A RID: 31258
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdLow = new GameObjectPool.ShrinkThreshold(12, 1, 3f);

	// Token: 0x04007A1B RID: 31259
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObjectPool.ShrinkThreshold shrinkThresholdMin = new GameObjectPool.ShrinkThreshold(0, 1, 10f);

	// Token: 0x0200143A RID: 5178
	// (Invoke) Token: 0x0600A24C RID: 41548
	public delegate Transform LoadCallback();

	// Token: 0x0200143B RID: 5179
	// (Invoke) Token: 0x0600A250 RID: 41552
	public delegate void CreateCallback(GameObject obj);

	// Token: 0x0200143C RID: 5180
	// (Invoke) Token: 0x0600A254 RID: 41556
	public delegate void CreateAsyncCallback(object _userData, global::UnityEngine.Object[] _objs, int _objsCount, bool _isAsync);

	// Token: 0x0200143D RID: 5181
	public class PoolItem
	{
		// Token: 0x0600A257 RID: 41559 RVA: 0x003CF330 File Offset: 0x003CD530
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

		// Token: 0x04007A1C RID: 31260
		public string name;

		// Token: 0x04007A1D RID: 31261
		public GameObject prefab;

		// Token: 0x04007A1E RID: 31262
		public GameObjectPool.LoadCallback loadCallback;

		// Token: 0x04007A1F RID: 31263
		public GameObjectPool.CreateCallback createOnceToAllCallback;

		// Token: 0x04007A20 RID: 31264
		public GameObjectPool.CreateCallback createCallback;

		// Token: 0x04007A21 RID: 31265
		public List<GameObject> objs;

		// Token: 0x04007A22 RID: 31266
		public float updateTime;

		// Token: 0x04007A23 RID: 31267
		public int activeCount;

		// Token: 0x04007A24 RID: 31268
		public Color originalTint;
	}

	// Token: 0x0200143E RID: 5182
	public class AsyncItem
	{
		// Token: 0x04007A25 RID: 31269
		public GameObjectPool.PoolItem item;

		// Token: 0x04007A26 RID: 31270
		public GameObjectPool.CreateAsyncCallback callback;

		// Token: 0x04007A27 RID: 31271
		public AsyncInstantiateOperation async;

		// Token: 0x04007A28 RID: 31272
		public object userData;
	}

	// Token: 0x0200143F RID: 5183
	public struct ShrinkThreshold
	{
		// Token: 0x0600A25A RID: 41562 RVA: 0x003CF393 File Offset: 0x003CD593
		public ShrinkThreshold(int count, int destroyCount, float delay)
		{
			this.Count = count;
			this.DestroyCount = destroyCount;
			this.Delay = delay;
		}

		// Token: 0x0600A25B RID: 41563 RVA: 0x003CF3AC File Offset: 0x003CD5AC
		public override string ToString()
		{
			return string.Format("({0} = {1}, {2} = {3}, {4} = {5:F2}s)", new object[] { "Count", this.Count, "DestroyCount", this.DestroyCount, "Delay", this.Delay });
		}

		// Token: 0x04007A29 RID: 31273
		public int Count;

		// Token: 0x04007A2A RID: 31274
		public int DestroyCount;

		// Token: 0x04007A2B RID: 31275
		public float Delay;
	}
}
