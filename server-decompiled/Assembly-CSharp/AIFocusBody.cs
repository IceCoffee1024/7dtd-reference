using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public struct AIFocusBody : IFocusTarget
{
	// Token: 0x06002149 RID: 8521 RVA: 0x000C911F File Offset: 0x000C731F
	public AIFocusBody(float target)
	{
		this.TargetYaw = target;
		this.TargetYawEntity = null;
		this.HasTargetYaw = true;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000C9142 File Offset: 0x000C7342
	public AIFocusBody(EntityAlive target)
	{
		this.TargetYaw = 0f;
		this.TargetYawEntity = target;
		this.HasTargetYaw = false;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000C9169 File Offset: 0x000C7369
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

	// Token: 0x0600214C RID: 8524 RVA: 0x000C91A3 File Offset: 0x000C73A3
	public static bool GetActiveFocusForPriority(EntityAlive theEntity, FocusPriority priority, AIFocus<AIFocusBody> focus, out float focusForPriority)
	{
		return focus.FocusTargets[(int)priority].TryGetValue(theEntity, out focusForPriority);
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000C91B8 File Offset: 0x000C73B8
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

	// Token: 0x040016E5 RID: 5861
	public readonly EntityAlive TargetYawEntity;

	// Token: 0x040016E6 RID: 5862
	public readonly float TargetYaw;

	// Token: 0x040016E7 RID: 5863
	public readonly bool HasTargetYaw;

	// Token: 0x040016E8 RID: 5864
	public AIFocusConditionDistance ConditionDistance;
}
