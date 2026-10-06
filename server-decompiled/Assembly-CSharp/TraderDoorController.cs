using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class TraderDoorController : MonoBehaviour
{
	// Token: 0x0600062D RID: 1581 RVA: 0x0002BD08 File Offset: 0x00029F08
	public static TraderDoorController AddTraderDoorController(Vector3 pos, GameObject go)
	{
		if (!TraderDoorController.ControllerDictionary.ContainsKey(pos))
		{
			TraderDoorController traderDoorController = go.AddComponent<TraderDoorController>();
			TraderDoorController.ControllerDictionary.Add(pos, traderDoorController);
			return traderDoorController;
		}
		return TraderDoorController.ControllerDictionary[pos];
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002BD42 File Offset: 0x00029F42
	public static TraderDoorController GetTraderDoorController(Vector3 pos)
	{
		if (TraderDoorController.ControllerDictionary.ContainsKey(pos))
		{
			return TraderDoorController.ControllerDictionary[pos];
		}
		return null;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0002BD60 File Offset: 0x00029F60
	public static void RemoveTraderDoorController(Vector3 pos)
	{
		if (TraderDoorController.ControllerDictionary.ContainsKey(pos))
		{
			World world = GameManager.Instance.World;
			EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
			if (entityPlayerLocal != null)
			{
				EntityVehicle entityVehicle = entityPlayerLocal.AttachedToEntity as EntityVehicle;
				if (entityVehicle != null)
				{
					TraderDoorController traderDoorController = TraderDoorController.ControllerDictionary[pos];
					if (entityVehicle.HornActivation == traderDoorController)
					{
						entityVehicle.HornActivation = null;
					}
				}
			}
			TraderDoorController.ControllerDictionary.Remove(pos);
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0002BDD8 File Offset: 0x00029FD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		EntityVehicle vehicleFromCollider = this.GetVehicleFromCollider(other);
		if (vehicleFromCollider != null)
		{
			vehicleFromCollider.HornActivation = this;
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002BE00 File Offset: 0x0002A000
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		EntityVehicle vehicleFromCollider = this.GetVehicleFromCollider(other);
		if (vehicleFromCollider != null)
		{
			vehicleFromCollider.HornActivation = null;
		}
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002BE28 File Offset: 0x0002A028
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle GetVehicleFromCollider(Collider collider)
	{
		if (collider == null)
		{
			return null;
		}
		Transform transform = collider.transform;
		if (transform != null)
		{
			EntityVehicle entityVehicle = transform.GetComponent<EntityVehicle>();
			if (entityVehicle == null)
			{
				entityVehicle = transform.GetComponentInParent<EntityVehicle>();
			}
			if (entityVehicle == null && transform.parent != null)
			{
				entityVehicle = transform.parent.GetComponentInChildren<EntityVehicle>();
			}
			if (entityVehicle == null)
			{
				entityVehicle = transform.GetComponentInChildren<EntityVehicle>();
			}
			if (entityVehicle != null && entityVehicle.IsAlive())
			{
				return entityVehicle;
			}
		}
		return null;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
	public void Activate()
	{
		TEFeatureLockable tefeatureLockable = null;
		this.OwnerDoor.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable);
		if (tefeatureLockable != null && tefeatureLockable.IsLocked())
		{
			return;
		}
		this.OwnerDoor.SetOpen(!this.OwnerDoor.IsOpen(), true);
	}

	// Token: 0x040006DB RID: 1755
	public TEFeatureDoor OwnerDoor;

	// Token: 0x040006DC RID: 1756
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Vector3, TraderDoorController> ControllerDictionary = new Dictionary<Vector3, TraderDoorController>();
}
