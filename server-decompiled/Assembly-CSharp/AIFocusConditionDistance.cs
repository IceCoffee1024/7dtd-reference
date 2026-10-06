using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public readonly struct AIFocusConditionDistance
{
	// Token: 0x06002145 RID: 8517 RVA: 0x000C9019 File Offset: 0x000C7219
	public AIFocusConditionDistance(float distance, EntityAlive entityTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = entityTransform;
		this.DistanceTransform = null;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x000C9044 File Offset: 0x000C7244
	public AIFocusConditionDistance(float distance, Transform distanceTransform)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = distanceTransform;
		this.DistancePosition = Vector3.zero;
		this.HasDistancePosition = false;
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000C906F File Offset: 0x000C726F
	public AIFocusConditionDistance(float distance, Vector3 distancePosition)
	{
		this.ConditionalDistanceSq = distance * distance;
		this.EntityTransform = null;
		this.DistanceTransform = null;
		this.DistancePosition = distancePosition;
		this.HasDistancePosition = true;
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000C9098 File Offset: 0x000C7298
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

	// Token: 0x040016E0 RID: 5856
	public readonly float ConditionalDistanceSq;

	// Token: 0x040016E1 RID: 5857
	public readonly EntityAlive EntityTransform;

	// Token: 0x040016E2 RID: 5858
	public readonly Transform DistanceTransform;

	// Token: 0x040016E3 RID: 5859
	public readonly Vector3 DistancePosition;

	// Token: 0x040016E4 RID: 5860
	public readonly bool HasDistancePosition;
}
