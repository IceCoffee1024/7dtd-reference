using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
public class TripWireController : MonoBehaviour
{
	// Token: 0x06001DE8 RID: 7656 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(DynamicProperties _properties)
	{
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000B61B4 File Offset: 0x000B43B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x000B61B4 File Offset: 0x000B43B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000B61C0 File Offset: 0x000B43C0
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

	// Token: 0x040013F5 RID: 5109
	public TileEntityPoweredTrigger TileEntityParent;

	// Token: 0x040013F6 RID: 5110
	public TileEntityPoweredTrigger TileEntityChild;

	// Token: 0x040013F7 RID: 5111
	public IWireNode WireNode;
}
