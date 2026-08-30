using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class TraderDoorController : MonoBehaviour
{
	// Token: 0x06000627 RID: 1575 RVA: 0x0002BC10 File Offset: 0x00029E10
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

	// Token: 0x06000628 RID: 1576 RVA: 0x0002BC4A File Offset: 0x00029E4A
	public static TraderDoorController GetTraderDoorController(Vector3 pos)
	{
		if (TraderDoorController.ControllerDictionary.ContainsKey(pos))
		{
			return TraderDoorController.ControllerDictionary[pos];
		}
		return null;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002BC68 File Offset: 0x00029E68
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

	// Token: 0x0600062A RID: 1578 RVA: 0x0002BCE0 File Offset: 0x00029EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		EntityVehicle vehicleFromCollider = this.GetVehicleFromCollider(other);
		if (vehicleFromCollider != null)
		{
			vehicleFromCollider.HornActivation = this;
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002BD08 File Offset: 0x00029F08
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		EntityVehicle vehicleFromCollider = this.GetVehicleFromCollider(other);
		if (vehicleFromCollider != null)
		{
			vehicleFromCollider.HornActivation = null;
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0002BD30 File Offset: 0x00029F30
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

	// Token: 0x0600062D RID: 1581 RVA: 0x0002BDB8 File Offset: 0x00029FB8
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

	// Token: 0x040006D8 RID: 1752
	public TEFeatureDoor OwnerDoor;

	// Token: 0x040006D9 RID: 1753
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Vector3, TraderDoorController> ControllerDictionary = new Dictionary<Vector3, TraderDoorController>();
}
