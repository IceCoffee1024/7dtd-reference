using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class MapObjectManager
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060033C9 RID: 13257 RVA: 0x0014E438 File Offset: 0x0014C638
	// (remove) Token: 0x060033CA RID: 13258 RVA: 0x0014E470 File Offset: 0x0014C670
	public event MapObjectManager.MapObjectListChangedDelegate ChangedDelegates;

	// Token: 0x060033CB RID: 13259 RVA: 0x0014E4A5 File Offset: 0x0014C6A5
	public static void Reset()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0014E4B4 File Offset: 0x0014C6B4
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

	// Token: 0x060033CD RID: 13261 RVA: 0x0014E4A5 File Offset: 0x0014C6A5
	public static void ClearEntityList()
	{
		MapObjectManager.entityList.Clear();
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0014E554 File Offset: 0x0014C754
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

	// Token: 0x060033CF RID: 13263 RVA: 0x0014E5F4 File Offset: 0x0014C7F4
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

	// Token: 0x060033D0 RID: 13264 RVA: 0x0014E67C File Offset: 0x0014C87C
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

	// Token: 0x060033D1 RID: 13265 RVA: 0x0014E6FC File Offset: 0x0014C8FC
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

	// Token: 0x060033D2 RID: 13266 RVA: 0x0014E762 File Offset: 0x0014C962
	public void Clear()
	{
		this.mapObjects.Clear();
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x0014E76F File Offset: 0x0014C96F
	public List<MapObject> GetList(EnumMapObjectType _type)
	{
		return this.mapObjects[(int)_type].list;
	}

	// Token: 0x04002918 RID: 10520
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DictionaryList<int, MapObject>> mapObjects = new List<DictionaryList<int, MapObject>>();

	// Token: 0x0400291A RID: 10522
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<MapObject> entityList = new List<MapObject>();

	// Token: 0x0200063C RID: 1596
	// (Invoke) Token: 0x060033D6 RID: 13270
	public delegate void MapObjectListChangedDelegate(EnumMapObjectType _type, MapObject _mapObject, bool _bAdded);
}
