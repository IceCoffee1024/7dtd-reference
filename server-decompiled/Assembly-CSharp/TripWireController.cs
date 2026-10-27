using System;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class TripWireController : MonoBehaviour
{
	// Token: 0x06001DEC RID: 7660 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(DynamicProperties _properties)
	{
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000B63C4 File Offset: 0x000B45C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x000B63C4 File Offset: 0x000B45C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000B63D0 File Offset: 0x000B45D0
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

	// Token: 0x04001405 RID: 5125
	public TileEntityPoweredTrigger TileEntityParent;

	// Token: 0x04001406 RID: 5126
	public TileEntityPoweredTrigger TileEntityChild;

	// Token: 0x04001407 RID: 5127
	public IWireNode WireNode;
}
