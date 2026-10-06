using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200131F RID: 4895
public class WaypointCollection
{
	// Token: 0x06009A96 RID: 39574 RVA: 0x003A91F4 File Offset: 0x003A73F4
	public void Read(PooledBinaryReader _br)
	{
		this.Collection.Clear();
		int num = (int)_br.ReadByte();
		int num2 = (int)_br.ReadUInt16();
		for (int i = 0; i < num2; i++)
		{
			Waypoint waypoint = new Waypoint();
			waypoint.Read(_br, num);
			this.Collection.Add(waypoint);
		}
	}

	// Token: 0x06009A97 RID: 39575 RVA: 0x003A9240 File Offset: 0x003A7440
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(7);
		int num = 0;
		for (int i = 0; i < this.Collection.list.Count; i++)
		{
			if (this.Collection.list[i].IsSaved)
			{
				num++;
			}
		}
		_bw.Write((ushort)num);
		for (int j = 0; j < this.Collection.list.Count; j++)
		{
			if (this.Collection.list[j].IsSaved)
			{
				this.Collection.list[j].Write(_bw);
			}
		}
	}

	// Token: 0x06009A98 RID: 39576 RVA: 0x003A92E0 File Offset: 0x003A74E0
	public WaypointCollection Clone()
	{
		WaypointCollection waypointCollection = new WaypointCollection();
		for (int i = 0; i < this.Collection.list.Count; i++)
		{
			waypointCollection.Collection.Add(this.Collection.list[i].Clone());
		}
		return waypointCollection;
	}

	// Token: 0x06009A99 RID: 39577 RVA: 0x003A9330 File Offset: 0x003A7530
	public bool ContainsWaypoint(Waypoint _wp)
	{
		return this.Collection.hashSet.Contains(_wp);
	}

	// Token: 0x06009A9A RID: 39578 RVA: 0x003A9344 File Offset: 0x003A7544
	public bool ContainsLastKnownPositionWaypoint(int _entityId)
	{
		using (List<Waypoint>.Enumerator enumerator = this.Collection.list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.lastKnownPositionEntityId == _entityId)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06009A9B RID: 39579 RVA: 0x003A93A4 File Offset: 0x003A75A4
	public Waypoint GetLastKnownPositionWaypoint(int _entityID)
	{
		foreach (Waypoint waypoint in this.Collection.list)
		{
			if (waypoint.lastKnownPositionEntityId == _entityID)
			{
				return waypoint;
			}
		}
		return null;
	}

	// Token: 0x06009A9C RID: 39580 RVA: 0x003A9408 File Offset: 0x003A7608
	public void UpdateEntityVehicleWayPoint(EntityVehicle vehicle, bool unloaded = false)
	{
		if (!vehicle.LocalPlayerIsOwner())
		{
			return;
		}
		Waypoint waypoint = null;
		foreach (Waypoint waypoint2 in this.Collection.list)
		{
			if (waypoint2.lastKnownPositionEntityId == vehicle.entityId)
			{
				waypoint = waypoint2;
				break;
			}
		}
		if (waypoint != null)
		{
			Vector3i vector3i = Vector3i.FromVector3Rounded(vehicle.position);
			if (waypoint.pos != vector3i)
			{
				this.Collection.Remove(waypoint);
				waypoint.pos = vector3i;
				waypoint.navObject.TrackedPosition = vehicle.position;
				this.Collection.Add(waypoint);
			}
			waypoint.navObject.hiddenOnCompass = !unloaded;
			this.SetWaypointHiddenOnMap(vehicle.entityId, !unloaded);
			return;
		}
		((XUiC_MapArea)GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui.GetWindow("mapArea").Controller).RefreshVehiclePositionWaypoint(vehicle, false);
	}

	// Token: 0x06009A9D RID: 39581 RVA: 0x003A9518 File Offset: 0x003A7718
	public void UpdateEntityAnimalWayPoint(EntityAnimal _animal, bool unloaded = false)
	{
		Waypoint waypoint = null;
		foreach (Waypoint waypoint2 in this.Collection.list)
		{
			if (waypoint2.lastKnownPositionEntityId == _animal.entityId)
			{
				waypoint = waypoint2;
				break;
			}
		}
		if (waypoint != null)
		{
			Vector3i vector3i = Vector3i.FromVector3Rounded(_animal.position);
			if (waypoint.pos != vector3i)
			{
				this.Collection.Remove(waypoint);
				waypoint.pos = vector3i;
				waypoint.navObject.TrackedPosition = _animal.position;
				this.Collection.Add(waypoint);
			}
			waypoint.navObject.hiddenOnCompass = !unloaded;
			this.SetWaypointHiddenOnMap(_animal.entityId, !unloaded);
			return;
		}
		((XUiC_MapArea)GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui.GetWindow("mapArea").Controller).RefreshAnimalPositionWaypoint(_animal, false);
	}

	// Token: 0x06009A9E RID: 39582 RVA: 0x003A9620 File Offset: 0x003A7820
	public void SetEntityVehicleWaypointFromVehicleManager([TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		List<Waypoint> list = new List<Waypoint>();
		for (int i = 0; i < this.Collection.list.Count; i++)
		{
			if (this.Collection.list[i].lastKnownPositionEntityId != -1 && this.Collection.list[i].lastKnownPositionEntityType == eLastKnownPositionEntityType.Vehicle)
			{
				Waypoint waypoint = this.Collection.list[i];
				bool flag = false;
				int j = 0;
				while (j < _positions.Count)
				{
					if (waypoint.lastKnownPositionEntityId == _positions[j].Item1)
					{
						flag = true;
						Vector3i vector3i = Vector3i.FromVector3Rounded(_positions[j].Item2);
						if (waypoint.pos != vector3i)
						{
							this.Collection.Remove(waypoint);
							waypoint.pos = vector3i;
							waypoint.navObject.TrackedPosition = _positions[j].Item2;
							this.Collection.Add(waypoint);
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
				if (!flag)
				{
					list.Add(waypoint);
				}
			}
		}
		foreach (Waypoint waypoint2 in list)
		{
			this.Collection.Remove(waypoint2);
			NavObjectManager.Instance.UnRegisterNavObject(waypoint2.navObject);
		}
	}

	// Token: 0x06009A9F RID: 39583 RVA: 0x003A9790 File Offset: 0x003A7990
	public void SetDroneWaypointsFromDroneManager([TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _drones)
	{
		for (int i = 0; i < _drones.Count; i++)
		{
			Waypoint waypoint = null;
			foreach (Waypoint waypoint2 in this.Collection.list)
			{
				if (waypoint2.lastKnownPositionEntityId == _drones[i].Item1)
				{
					waypoint = waypoint2;
					break;
				}
			}
			if (waypoint != null)
			{
				Vector3i vector3i = Vector3i.FromVector3Rounded(_drones[i].Item2);
				if (waypoint.pos != vector3i)
				{
					this.Collection.Remove(waypoint);
					waypoint.pos = vector3i;
					waypoint.navObject.TrackedPosition = _drones[i].Item2;
					this.Collection.Add(waypoint);
				}
			}
			else
			{
				((XUiC_MapArea)GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui.GetWindow("mapArea").Controller).RefreshDronePositionWaypoint(_drones[i].Item1, Vector3i.FromVector3Rounded(_drones[i].Item2), true);
			}
		}
		List<Waypoint> list = new List<Waypoint>();
		for (int j = 0; j < this.Collection.list.Count; j++)
		{
			if (this.Collection.list[j].lastKnownPositionEntityId != -1 && this.Collection.list[j].lastKnownPositionEntityType == eLastKnownPositionEntityType.Drone)
			{
				Waypoint waypoint3 = this.Collection.list[j];
				bool flag = false;
				for (int k = 0; k < _drones.Count; k++)
				{
					if (waypoint3.lastKnownPositionEntityId == _drones[k].Item1)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(waypoint3);
				}
			}
		}
		foreach (Waypoint waypoint4 in list)
		{
			this.Collection.Remove(waypoint4);
			NavObjectManager.Instance.UnRegisterNavObject(waypoint4.navObject);
		}
	}

	// Token: 0x06009AA0 RID: 39584 RVA: 0x003A99D0 File Offset: 0x003A7BD0
	public void UpdateEntityDroneWayPoint(EntityDrone drone, bool following, bool unloaded = false)
	{
		if (!drone.LocalPlayerIsOwner())
		{
			return;
		}
		Waypoint waypoint = null;
		foreach (Waypoint waypoint2 in this.Collection.list)
		{
			if (waypoint2.lastKnownPositionEntityId == drone.entityId)
			{
				waypoint = waypoint2;
				break;
			}
		}
		if (waypoint != null)
		{
			Vector3i vector3i = Vector3i.FromVector3Rounded(drone.position);
			if (waypoint.pos != vector3i)
			{
				this.Collection.Remove(waypoint);
				waypoint.pos = vector3i;
				waypoint.navObject.TrackedPosition = drone.position;
				this.Collection.Add(waypoint);
			}
			waypoint.navObject.hiddenOnCompass = !unloaded;
			this.SetWaypointHiddenOnMap(drone.entityId, !unloaded);
			return;
		}
		((XUiC_MapArea)GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui.GetWindow("mapArea").Controller).RefreshDronePositionWaypoint(drone, false);
	}

	// Token: 0x06009AA1 RID: 39585 RVA: 0x003A9AE0 File Offset: 0x003A7CE0
	public void SetWaypointHiddenOnMap(int _entityId, bool _hidden)
	{
		foreach (Waypoint waypoint in this.Collection.list)
		{
			if (waypoint.lastKnownPositionEntityId == _entityId)
			{
				waypoint.HiddenOnMap = _hidden;
				waypoint.navObject.hiddenOnMap = _hidden;
				break;
			}
		}
	}

	// Token: 0x06009AA2 RID: 39586 RVA: 0x003A9B50 File Offset: 0x003A7D50
	public bool TryRemoveLastKnownPositionWaypoint(int _entityId)
	{
		Waypoint lastKnownPositionWaypoint = this.GetLastKnownPositionWaypoint(_entityId);
		if (lastKnownPositionWaypoint != null)
		{
			this.Collection.Remove(lastKnownPositionWaypoint);
			NavObjectManager.Instance.UnRegisterNavObject(lastKnownPositionWaypoint.navObject);
			return true;
		}
		return false;
	}

	// Token: 0x06009AA3 RID: 39587 RVA: 0x003A9B88 File Offset: 0x003A7D88
	public Waypoint GetWaypointForNavObject(NavObject nav)
	{
		foreach (Waypoint waypoint in this.Collection.list)
		{
			if (nav == waypoint.navObject)
			{
				return waypoint;
			}
		}
		return null;
	}

	// Token: 0x04007492 RID: 29842
	public const int cCurrentSaveVersion = 7;

	// Token: 0x04007493 RID: 29843
	public HashSetList<Waypoint> Collection = new HashSetList<Waypoint>();
}
