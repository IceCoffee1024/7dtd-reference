using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public struct AIFocusBody : IFocusTarget
{
	// Token: 0x0600214E RID: 8526 RVA: 0x000C94CB File Offset: 0x000C76CB
	public AIFocusBody(float target)
	{
		this.TargetYaw = target;
		this.TargetYawEntity = null;
		this.HasTargetYaw = true;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000C94EE File Offset: 0x000C76EE
	public AIFocusBody(EntityAlive target)
	{
		this.TargetYaw = 0f;
		this.TargetYawEntity = target;
		this.HasTargetYaw = false;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000C9515 File Offset: 0x000C7715
	public bool TryGetValue(EntityAlive theEntity, out float value)
	{
		if (this.HasTargetYaw)
		{
			value = this.TargetYaw;
			return true;
		}
		if (this.TargetYawEntity != null)
		{
			value = theEntity.YawForTarget(this.TargetYawEntity);
			return true;
		}
		value = 0f;
		return false;
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x000C954F File Offset: 0x000C774F
	public static bool GetActiveFocusForPriority(EntityAlive theEntity, FocusPriority priority, AIFocus<AIFocusBody> focus, out float focusForPriority)
	{
		return focus.FocusTargets[(int)priority].TryGetValue(theEntity, out focusForPriority);
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x000C9564 File Offset: 0x000C7764
	public static bool GetActiveFocus(EntityAlive theEntity, AIFocus<AIFocusBody> focus, out float activeFocus)
	{
		Vector3 position = theEntity.position;
		for (int i = 0; i < focus.FocusTargets.Length; i++)
		{
			ref AIFocusBody ptr = ref focus.FocusTargets[i];
			float num;
			if (ptr.TryGetValue(theEntity, out num) && !ptr.ConditionDistance.IsFocusDisabled(theEntity))
			{
				activeFocus = num;
				return true;
			}
		}
		activeFocus = 0f;
		return false;
	}

	// Token: 0x040016F5 RID: 5877
	public readonly EntityAlive TargetYawEntity;

	// Token: 0x040016F6 RID: 5878
	public readonly float TargetYaw;

	// Token: 0x040016F7 RID: 5879
	public readonly bool HasTargetYaw;

	// Token: 0x040016F8 RID: 5880
	public AIFocusConditionDistance ConditionDistance;
}
