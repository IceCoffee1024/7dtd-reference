using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public readonly struct AIFocusConditionDistance
{
	// Token: 0x0600214A RID: 8522 RVA: 0x000C93C5 File Offset: 0x000C75C5
	public AIFocusConditionDistance(float distance, EntityAlive entityTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = entityTransform;
		this.DistanceTransform = null;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000C93F0 File Offset: 0x000C75F0
	public AIFocusConditionDistance(float distance, Transform distanceTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = distanceTransform;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000C941B File Offset: 0x000C761B
	public AIFocusConditionDistance(float distance, Vector3 distancePosition)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = null;
		this.DistancePosition = distancePosition;
		this.HasDistancePosition = true;
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000C9444 File Offset: 0x000C7644
	public bool IsFocusDisabled(Entity theEntity)
	{
		if (this.ConditionalDistanceSq > 0f)
		{
			Vector3 vector = Vector3.zero;
			if (this.HasDistancePosition)
			{
				vector = this.DistancePosition;
			}
			else if (this.EntityTransform)
			{
				vector = this.EntityTransform.position;
			}
			else
			{
				if (!this.DistanceTransform)
				{
					return false;
				}
				vector = this.DistanceTransform.position;
			}
			Vector3 position = theEntity.position;
			if (Vector3.SqrMagnitude(vector - position) > this.ConditionalDistanceSq)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040016F0 RID: 5872
	public readonly float ConditionalDistanceSq;

	// Token: 0x040016F1 RID: 5873
	public readonly EntityAlive EntityTransform;

	// Token: 0x040016F2 RID: 5874
	public readonly Transform DistanceTransform;

	// Token: 0x040016F3 RID: 5875
	public readonly Vector3 DistancePosition;

	// Token: 0x040016F4 RID: 5876
	public readonly bool HasDistancePosition;
}
