using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public readonly struct AIFocusConditionDistance
{
	// Token: 0x06002139 RID: 8505 RVA: 0x000C8759 File Offset: 0x000C6959
	public AIFocusConditionDistance(float distance, EntityAlive entityTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = entityTransform;
		this.DistanceTransform = null;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000C8784 File Offset: 0x000C6984
	public AIFocusConditionDistance(float distance, Transform distanceTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = distanceTransform;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000C87AF File Offset: 0x000C69AF
	public AIFocusConditionDistance(float distance, Vector3 distancePosition)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = null;
		this.DistancePosition = distancePosition;
		this.HasDistancePosition = true;
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000C87D8 File Offset: 0x000C69D8
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

	// Token: 0x040016D4 RID: 5844
	public readonly float ConditionalDistanceSq;

	// Token: 0x040016D5 RID: 5845
	public readonly EntityAlive EntityTransform;

	// Token: 0x040016D6 RID: 5846
	public readonly Transform DistanceTransform;

	// Token: 0x040016D7 RID: 5847
	public readonly Vector3 DistancePosition;

	// Token: 0x040016D8 RID: 5848
	public readonly bool HasDistancePosition;
}
