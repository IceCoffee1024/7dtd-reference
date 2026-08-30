using System;
using Platform;
using UnityEngine;

// Token: 0x020012A5 RID: 4773
public class POIWaypoint : Waypoint
{
	// Token: 0x06009750 RID: 38736 RVA: 0x00395E12 File Offset: 0x00394012
	public POIWaypoint()
	{
		this.IsSaved = false;
	}

	// Token: 0x06009751 RID: 38737 RVA: 0x00395E24 File Offset: 0x00394024
	public static bool TrySet(EntityPlayer player, int prefabInstanceId, bool hiddenOnCompass)
	{
		if (player.isEntityRemote)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(player.entityId);
				if (clientInfo != null)
				{
					clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackagePOIWaypoint>().Setup(NetPackagePOIWaypoint.OperationType.TrySet, player.entityId, prefabInstanceId, hiddenOnCompass));
					return true;
				}
			}
			return false;
		}
		PrefabInstance prefab = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetPrefab(prefabInstanceId);
		if (prefab == null)
		{
			Log.Error(string.Format("{0} not set: could not find prefab instance with id {1}", "POIWaypoint", prefabInstanceId));
			return false;
		}
		POIWaypoint poiwaypoint = new POIWaypoint();
		poiwaypoint.prefabInstanceId = prefab.id;
		poiwaypoint.pos = prefab.boundingBoxPosition + prefab.boundingBoxSize / 2;
		poiwaypoint.name.Update(prefab.prefab.PrefabName, PlatformManager.MultiPlatform.User.PlatformUserId);
		poiwaypoint.icon = "ui_game_symbol_map_trader";
		poiwaypoint.ownerId = null;
		poiwaypoint.lastKnownPositionEntityId = -1;
		if (!player.Waypoints.ContainsWaypoint(poiwaypoint))
		{
			NavObject navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", poiwaypoint.pos, poiwaypoint.icon, hiddenOnCompass, -1, null);
			navObject.UseOverrideColor = true;
			navObject.OverrideColor = Color.cyan;
			navObject.IsActive = true;
			navObject.name = poiwaypoint.name.Text;
			poiwaypoint.navObject = navObject;
			player.Waypoints.Collection.Add(poiwaypoint);
			return true;
		}
		return false;
	}

	// Token: 0x06009752 RID: 38738 RVA: 0x00395FA0 File Offset: 0x003941A0
	public static void Remove(int prefabInstanceId)
	{
		foreach (EntityPlayer entityPlayer in GameManager.Instance.World.Players.list)
		{
			POIWaypoint.Remove(entityPlayer, prefabInstanceId);
		}
	}

	// Token: 0x06009753 RID: 38739 RVA: 0x00396000 File Offset: 0x00394200
	public static void Remove(EntityPlayer player, int prefabInstanceId)
	{
		if (player.isEntityRemote)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(player.entityId);
				if (clientInfo != null)
				{
					clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackagePOIWaypoint>().Setup(NetPackagePOIWaypoint.OperationType.Remove, player.entityId, prefabInstanceId, true));
				}
			}
			return;
		}
		for (int i = player.Waypoints.Collection.list.Count - 1; i >= 0; i--)
		{
			Waypoint waypoint = player.Waypoints.Collection.list[i];
			POIWaypoint poiwaypoint = waypoint as POIWaypoint;
			if (poiwaypoint != null && poiwaypoint.prefabInstanceId == prefabInstanceId)
			{
				NavObjectManager.Instance.UnRegisterNavObject(poiwaypoint.navObject);
				player.Waypoints.Collection.Remove(waypoint);
			}
		}
	}

	// Token: 0x06009754 RID: 38740 RVA: 0x003960C4 File Offset: 0x003942C4
	public static void ClearAll(EntityPlayer localPlayer)
	{
		if (localPlayer.isEntityRemote)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(localPlayer.entityId);
				if (clientInfo != null)
				{
					clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackagePOIWaypoint>().Setup(NetPackagePOIWaypoint.OperationType.ClearAll, localPlayer.entityId, -1, true));
				}
			}
			return;
		}
		NavObjectManager instance = NavObjectManager.Instance;
		for (int i = localPlayer.Waypoints.Collection.list.Count - 1; i >= 0; i--)
		{
			Waypoint waypoint = localPlayer.Waypoints.Collection.list[i];
			POIWaypoint poiwaypoint = waypoint as POIWaypoint;
			if (poiwaypoint != null)
			{
				instance.UnRegisterNavObject(poiwaypoint.navObject);
				localPlayer.Waypoints.Collection.Remove(waypoint);
			}
		}
	}

	// Token: 0x04007193 RID: 29075
	public int prefabInstanceId;
}
