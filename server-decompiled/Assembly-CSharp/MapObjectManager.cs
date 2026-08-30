using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class MapObjectManager
{
	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06003378 RID: 13176 RVA: 0x0014CED4 File Offset: 0x0014B0D4
	// (remove) Token: 0x06003379 RID: 13177 RVA: 0x0014CF0C File Offset: 0x0014B10C
	public event MapObjectManager.MapObjectListChangedDelegate ChangedDelegates;

	// Token: 0x0600337A RID: 13178 RVA: 0x0014CF41 File Offset: 0x0014B141
	public static void Reset()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0014CF50 File Offset: 0x0014B150
	public MapObjectManager()
	{
		for (int i = 0; i < 17; i++)
		{
			this.mapObjects.Add(new DictionaryList<int, MapObject>());
		}
		foreach (MapObject mapObject in MapObjectManager.entityList)
		{
			if (mapObject is MapObjectVehicle)
			{
				this.Add(new MapObjectVehicle(mapObject as MapObjectVehicle));
			}
			else
			{
				this.Add(new MapObject(mapObject));
			}
		}
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x0014CF41 File Offset: 0x0014B141
	public static void ClearEntityList()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0014CFF0 File Offset: 0x0014B1F0
	public void Add(MapObject _mapObject)
	{
		if (this.mapObjects[(int)_mapObject.type].dict.ContainsKey((int)_mapObject.key))
		{
			this.Remove(_mapObject.type, (int)_mapObject.key);
		}
		this.mapObjects[(int)_mapObject.type].Add((int)_mapObject.key, _mapObject);
		if (this.ChangedDelegates != null)
		{
			this.ChangedDelegates(_mapObject.type, _mapObject, true);
		}
		if (_mapObject.type == EnumMapObjectType.Entity && !MapObjectManager.entityList.Contains(_mapObject))
		{
			MapObjectManager.entityList.Add(_mapObject);
		}
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0014D090 File Offset: 0x0014B290
	public void Remove(EnumMapObjectType _type, int _key)
	{
		if (this.mapObjects[(int)_type].dict.ContainsKey(_key))
		{
			MapObject mapObject = this.mapObjects[(int)_type].dict[_key];
			if (mapObject.type == EnumMapObjectType.Entity && MapObjectManager.entityList.Contains(mapObject))
			{
				MapObjectManager.entityList.Remove(mapObject);
			}
			this.mapObjects[(int)_type].Remove(_key);
			if (this.ChangedDelegates != null)
			{
				this.ChangedDelegates(_type, mapObject, false);
			}
		}
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0014D118 File Offset: 0x0014B318
	public void RemoveByPosition(EnumMapObjectType _type, Vector3 _position)
	{
		for (int i = this.mapObjects[(int)_type].list.Count - 1; i >= 0; i--)
		{
			Vector3 position = this.mapObjects[(int)_type].list[i].GetPosition();
			if (position.x == _position.x && position.z == _position.z)
			{
				this.mapObjects[(int)_type].list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0014D198 File Offset: 0x0014B398
	public void RemoveByType(EnumMapObjectType _type)
	{
		for (int i = this.mapObjects[(int)_type].list.Count - 1; i >= 0; i--)
		{
			if (this.mapObjects[(int)_type].list[i].type == _type)
			{
				this.mapObjects[(int)_type].list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x0014D1FE File Offset: 0x0014B3FE
	public void Clear()
	{
		this.mapObjects.Clear();
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x0014D20B File Offset: 0x0014B40B
	public List<MapObject> GetList(EnumMapObjectType _type)
	{
		return this.mapObjects[(int)_type].list;
	}

	// Token: 0x04002904 RID: 10500
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DictionaryList<int, MapObject>> mapObjects = new List<DictionaryList<int, MapObject>>();

	// Token: 0x04002906 RID: 10502
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<MapObject> entityList = new List<MapObject>();

	// Token: 0x02000633 RID: 1587
	// (Invoke) Token: 0x06003385 RID: 13189
	public delegate void MapObjectListChangedDelegate(EnumMapObjectType _type, MapObject _mapObject, bool _bAdded);
}
