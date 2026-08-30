using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public readonly struct AIFocusConditionDistance
{
	// Token: 0x06002156 RID: 8534 RVA: 0x000C8F65 File Offset: 0x000C7165
	public AIFocusConditionDistance(float distance, EntityAlive entityTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = entityTransform;
		this.DistanceTransform = null;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000C8F90 File Offset: 0x000C7190
	public AIFocusConditionDistance(float distance, Transform distanceTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = distanceTransform;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000C8FBB File Offset: 0x000C71BB
	public AIFocusConditionDistance(float distance, Vector3 distancePosition)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = null;
		this.DistancePosition = distancePosition;
		this.HasDistancePosition = true;
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000C8FE4 File Offset: 0x000C71E4
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

	// Token: 0x040016E9 RID: 5865
	public readonly float ConditionalDistanceSq;

	// Token: 0x040016EA RID: 5866
	public readonly EntityAlive EntityTransform;

	// Token: 0x040016EB RID: 5867
	public readonly Transform DistanceTransform;

	// Token: 0x040016EC RID: 5868
	public readonly Vector3 DistancePosition;

	// Token: 0x040016ED RID: 5869
	public readonly bool HasDistancePosition;
}
