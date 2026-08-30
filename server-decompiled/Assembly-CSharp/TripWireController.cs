using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
public class TripWireController : MonoBehaviour
{
	// Token: 0x06001DF0 RID: 7664 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(DynamicProperties _properties)
	{
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000B5FA8 File Offset: 0x000B41A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000B5FA8 File Offset: 0x000B41A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		this.checkIfTriggered(other);
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000B5FB4 File Offset: 0x000B41B4
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

	// Token: 0x040013FC RID: 5116
	public TileEntityPoweredTrigger TileEntityParent;

	// Token: 0x040013FD RID: 5117
	public TileEntityPoweredTrigger TileEntityChild;

	// Token: 0x040013FE RID: 5118
	public IWireNode WireNode;
}
