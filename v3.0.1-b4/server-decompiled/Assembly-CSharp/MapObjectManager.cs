using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000629 RID: 1577
public class MapObjectManager
{
	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06003325 RID: 13093 RVA: 0x0014ADB8 File Offset: 0x00148FB8
	// (remove) Token: 0x06003326 RID: 13094 RVA: 0x0014ADF0 File Offset: 0x00148FF0
	public event MapObjectManager.MapObjectListChangedDelegate ChangedDelegates;

	// Token: 0x06003327 RID: 13095 RVA: 0x0014AE25 File Offset: 0x00149025
	public static void Reset()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x0014AE34 File Offset: 0x00149034
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

	// Token: 0x06003329 RID: 13097 RVA: 0x0014AE25 File Offset: 0x00149025
	public static void ClearEntityList()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x0014AED4 File Offset: 0x001490D4
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

	// Token: 0x0600332B RID: 13099 RVA: 0x0014AF74 File Offset: 0x00149174
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

	// Token: 0x0600332C RID: 13100 RVA: 0x0014AFFC File Offset: 0x001491FC
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

	// Token: 0x0600332D RID: 13101 RVA: 0x0014B07C File Offset: 0x0014927C
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

	// Token: 0x0600332E RID: 13102 RVA: 0x0014B0E2 File Offset: 0x001492E2
	public void Clear()
	{
		this.mapObjects.Clear();
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x0014B0EF File Offset: 0x001492EF
	public List<MapObject> GetList(EnumMapObjectType _type)
	{
		return this.mapObjects[(int)_type].list;
	}

	// Token: 0x04002891 RID: 10385
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DictionaryList<int, MapObject>> mapObjects = new List<DictionaryList<int, MapObject>>();

	// Token: 0x04002893 RID: 10387
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<MapObject> entityList = new List<MapObject>();

	// Token: 0x0200062A RID: 1578
	// (Invoke) Token: 0x06003332 RID: 13106
	public delegate void MapObjectListChangedDelegate(EnumMapObjectType _type, MapObject _mapObject, bool _bAdded);
}
