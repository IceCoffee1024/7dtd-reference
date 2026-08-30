using System;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class TripWireController : MonoBehaviour
{
	// Token: 0x06001DD3 RID: 7635 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(DynamicProperties _properties)
	{
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000B5864 File Offset: 0x000B3A64
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000B5864 File Offset: 0x000B3A64
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000B5870 File Offset: 0x000B3A70
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkIfTriggered(Collider other)
	{
		if (this.TileEntityParent == null || this.WireNode == null)
		{
			return;
		}
		EntityAlive entityAlive = other.transform.GetComponent<EntityAlive>();
		if (entityAlive == null)
		{
			entityAlive = other.transform.GetComponentInParent<EntityAlive>();
		}
		if (entityAlive == null)
		{
			entityAlive = other.transform.parent.GetComponentInChildren<EntityAlive>();
		}
		if (entityAlive == null)
		{
			entityAlive = other.transform.GetComponentInChildren<EntityAlive>();
		}
		if (entityAlive != null && entityAlive as EntityVehicle != null && !(entityAlive as EntityVehicle).HasDriver)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.TileEntityParent.IsPowered)
		{
			this.TileEntityChild.IsTriggered = true;
		}
	}

	// Token: 0x040013E7 RID: 5095
	public TileEntityPoweredTrigger TileEntityParent;

	// Token: 0x040013E8 RID: 5096
	public TileEntityPoweredTrigger TileEntityChild;

	// Token: 0x040013E9 RID: 5097
	public IWireNode WireNode;
}
