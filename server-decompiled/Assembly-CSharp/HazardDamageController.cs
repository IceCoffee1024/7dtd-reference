using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class HazardDamageController : MonoBehaviour
{
	// Token: 0x06000621 RID: 1569 RVA: 0x0002B9CC File Offset: 0x00029BCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (!this.IsActive)
		{
			if (this.CollidersThisFrame != null && this.CollidersThisFrame.Count > 0)
			{
				this.CollidersThisFrame.Clear();
			}
			return;
		}
		if (this.CollidersThisFrame == null || this.CollidersThisFrame.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.CollidersThisFrame.Count; i++)
		{
			this.touched(this.CollidersThisFrame[i]);
		}
		this.CollidersThisFrame.Clear();
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002BA4C File Offset: 0x00029C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (!this.IsActive)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002BA4C File Offset: 0x00029C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		if (!this.IsActive)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002BA4C File Offset: 0x00029C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (!this.IsActive)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002BA84 File Offset: 0x00029C84
	[PublicizedFrom(EAccessModifier.Private)]
	public void touched(Collider collider)
	{
		if (!this.IsActive || collider == null)
		{
			return;
		}
		Transform transform = collider.transform;
		if (transform != null)
		{
			EntityAlive entityAlive = transform.GetComponent<EntityAlive>();
			if (entityAlive == null)
			{
				entityAlive = transform.GetComponentInParent<EntityAlive>();
			}
			if (entityAlive == null && transform.parent != null)
			{
				entityAlive = transform.parent.GetComponentInChildren<EntityAlive>();
			}
			if (entityAlive == null)
			{
				entityAlive = transform.GetComponentInChildren<EntityAlive>();
			}
			if (entityAlive != null && entityAlive.IsAlive() && this.buffActions != null)
			{
				for (int i = 0; i < this.buffActions.Count; i++)
				{
					if (entityAlive.emodel != null && entityAlive.emodel.transform != null && !entityAlive.Buffs.HasBuff(this.buffActions[i]))
					{
						entityAlive.Buffs.AddBuff(this.buffActions[i], -1, true, false, -1f);
					}
				}
			}
		}
	}

	// Token: 0x040006CB RID: 1739
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Collider> CollidersThisFrame;

	// Token: 0x040006CC RID: 1740
	public bool IsActive;

	// Token: 0x040006CD RID: 1741
	public List<string> buffActions;
}
