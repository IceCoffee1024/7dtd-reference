using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public struct AIFocusBody : IFocusTarget
{
	// Token: 0x0600213D RID: 8509 RVA: 0x000C885F File Offset: 0x000C6A5F
	public AIFocusBody(float target)
	{
		this.TargetYaw = target;
		this.TargetYawEntity = null;
		this.HasTargetYaw = true;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000C8882 File Offset: 0x000C6A82
	public AIFocusBody(EntityAlive target)
	{
		this.TargetYaw = 0f;
		this.TargetYawEntity = target;
		this.HasTargetYaw = false;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000C88A9 File Offset: 0x000C6AA9
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

	// Token: 0x06002140 RID: 8512 RVA: 0x000C88E3 File Offset: 0x000C6AE3
	public static bool GetActiveFocusForPriority(EntityAlive theEntity, FocusPriority priority, AIFocus<AIFocusBody> focus, out float focusForPriority)
	{
		return focus.FocusTargets[(int)priority].TryGetValue(theEntity, out focusForPriority);
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000C88F8 File Offset: 0x000C6AF8
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

	// Token: 0x040016D9 RID: 5849
	public readonly EntityAlive TargetYawEntity;

	// Token: 0x040016DA RID: 5850
	public readonly float TargetYaw;

	// Token: 0x040016DB RID: 5851
	public readonly bool HasTargetYaw;

	// Token: 0x040016DC RID: 5852
	public AIFocusConditionDistance ConditionDistance;
}
