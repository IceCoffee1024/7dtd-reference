using System;
using Platform;
using UnityEngine;

// Token: 0x020012BF RID: 4799
public class POIWaypoint : Waypoint
{
	// Token: 0x0600984B RID: 38987 RVA: 0x0039B056 File Offset: 0x00399256
	public POIWaypoint()
	{
		this.IsSaved = false;
	}

	// Token: 0x0600984C RID: 38988 RVA: 0x0039B068 File Offset: 0x00399268
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

	// Token: 0x0600984D RID: 38989 RVA: 0x0039B1E4 File Offset: 0x003993E4
	public static void Remove(int prefabInstanceId)
	{
		foreach (EntityPlayer entityPlayer in GameManager.Instance.World.Players.list)
		{
			POIWaypoint.Remove(entityPlayer, prefabInstanceId);
		}
	}

	// Token: 0x0600984E RID: 38990 RVA: 0x0039B244 File Offset: 0x00399444
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

	// Token: 0x0600984F RID: 38991 RVA: 0x0039B308 File Offset: 0x00399508
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

	// Token: 0x0400726A RID: 29290
	public int prefabInstanceId;
}
