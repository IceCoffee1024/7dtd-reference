using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class MapObjectManager
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060033BB RID: 13243 RVA: 0x0014D844 File Offset: 0x0014BA44
	// (remove) Token: 0x060033BC RID: 13244 RVA: 0x0014D87C File Offset: 0x0014BA7C
	public event MapObjectManager.MapObjectListChangedDelegate ChangedDelegates;

	// Token: 0x060033BD RID: 13245 RVA: 0x0014D8B1 File Offset: 0x0014BAB1
	public static void Reset()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x0014D8C0 File Offset: 0x0014BAC0
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

	// Token: 0x060033BF RID: 13247 RVA: 0x0014D8B1 File Offset: 0x0014BAB1
	public static void ClearEntityList()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x0014D960 File Offset: 0x0014BB60
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

	// Token: 0x060033C1 RID: 13249 RVA: 0x0014DA00 File Offset: 0x0014BC00
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

	// Token: 0x060033C2 RID: 13250 RVA: 0x0014DA88 File Offset: 0x0014BC88
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

	// Token: 0x060033C3 RID: 13251 RVA: 0x0014DB08 File Offset: 0x0014BD08
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

	// Token: 0x060033C4 RID: 13252 RVA: 0x0014DB6E File Offset: 0x0014BD6E
	public void Clear()
	{
		this.mapObjects.Clear();
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x0014DB7B File Offset: 0x0014BD7B
	public List<MapObject> GetList(EnumMapObjectType _type)
	{
		return this.mapObjects[(int)_type].list;
	}

	// Token: 0x040028FD RID: 10493
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DictionaryList<int, MapObject>> mapObjects = new List<DictionaryList<int, MapObject>>();

	// Token: 0x040028FF RID: 10495
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<MapObject> entityList = new List<MapObject>();

	// Token: 0x02000635 RID: 1589
	// (Invoke) Token: 0x060033C8 RID: 13256
	public delegate void MapObjectListChangedDelegate(EnumMapObjectType _type, MapObject _mapObject, bool _bAdded);
}
