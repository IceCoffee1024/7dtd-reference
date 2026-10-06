using System;
using System.Collections.Generic;

// Token: 0x0200093E RID: 2366
public class PrefabCache
{
	// Token: 0x060044CF RID: 17615 RVA: 0x001B315C File Offset: 0x001B135C
	public Prefab GetPrefabRotated(string _name, int _rotation, bool _applyMapping = true, bool _fixChildblocks = true, bool _allowMissingBlocks = false, bool _skipBlockData = false)
	{
		_rotation &= 3;
		Dictionary<string, Prefab> dictionary = this.prefabCache;
		Prefab prefab2;
		lock (dictionary)
		{
			Prefab[] array;
			if (this.prefabCacheRotations.TryGetValue(_name, out array))
			{
				if (array[_rotation] != null)
				{
					return array[_rotation];
				}
			}
			else
			{
				array = new Prefab[4];
				this.prefabCacheRotations[_name] = array;
			}
			Prefab prefab = this.GetPrefab(_name, _applyMapping, _fixChildblocks && _rotation == 0, _allowMissingBlocks, _skipBlockData);
			if (prefab == null)
			{
				prefab2 = null;
			}
			else
			{
				if (_rotation > 0)
				{
					prefab = prefab.Clone(true);
					prefab.RotateY(true, _rotation);
				}
				array[_rotation] = prefab;
				prefab2 = prefab;
			}
		}
		return prefab2;
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x001B3208 File Offset: 0x001B1408
	public Prefab GetPrefab(string _name, bool _applyMapping = true, bool _fixChildblocks = true, bool _allowMissingBlocks = false, bool _skipBlockData = false)
	{
		Dictionary<string, Prefab> dictionary = this.prefabCache;
		Prefab prefab;
		lock (dictionary)
		{
			if (this.prefabCache.ContainsKey(_name))
			{
				prefab = this.prefabCache[_name];
			}
			else
			{
				Prefab prefab2 = new Prefab();
				if (prefab2.Load(_name, _applyMapping, _fixChildblocks, _allowMissingBlocks, _skipBlockData))
				{
					this.prefabCache[_name] = prefab2;
					prefab = prefab2;
				}
				else
				{
					prefab = null;
				}
			}
		}
		return prefab;
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x001B3288 File Offset: 0x001B1488
	public void Clear()
	{
		Dictionary<string, Prefab> dictionary = this.prefabCache;
		lock (dictionary)
		{
			foreach (Prefab prefab in this.prefabCache.Values)
			{
				prefab.Clear();
			}
			foreach (Prefab[] array in this.prefabCacheRotations.Values)
			{
				for (int i = 1; i < array.Length; i++)
				{
					Prefab prefab2 = array[i];
					if (prefab2 != null)
					{
						prefab2.Clear();
					}
				}
			}
			this.prefabCache.Clear();
			this.prefabCacheRotations.Clear();
		}
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x001B3380 File Offset: 0x001B1580
	public void CalculateStats(out int basePrefabCount, out int rotatedPrefabsCount, out int basePrefabBytes, out int rotatedPrefabBytes)
	{
		Dictionary<string, Prefab> dictionary = this.prefabCache;
		lock (dictionary)
		{
			basePrefabCount = this.prefabCache.Count;
			basePrefabBytes = 0;
			foreach (Prefab prefab in this.prefabCache.Values)
			{
				basePrefabBytes += prefab.EstimateOwnedBytes();
			}
			rotatedPrefabsCount = 0;
			rotatedPrefabBytes = 0;
			foreach (Prefab[] array in this.prefabCacheRotations.Values)
			{
				for (int i = 1; i < array.Length; i++)
				{
					Prefab prefab2 = array[i];
					if (prefab2 != null)
					{
						rotatedPrefabsCount++;
						rotatedPrefabBytes += prefab2.EstimateOwnedBytes();
					}
				}
			}
		}
	}

	// Token: 0x040037A4 RID: 14244
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, Prefab> prefabCache = new Dictionary<string, Prefab>();

	// Token: 0x040037A5 RID: 14245
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, Prefab[]> prefabCacheRotations = new Dictionary<string, Prefab[]>();
}
