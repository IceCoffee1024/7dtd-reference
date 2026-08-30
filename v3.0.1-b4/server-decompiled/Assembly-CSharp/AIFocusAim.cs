using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public readonly struct AIFocusAim : IFocusTarget
{
	// Token: 0x06002142 RID: 8514 RVA: 0x000C8952 File Offset: 0x000C6B52
	public AIFocusAim(Entity target, AIAimFocusOffset targetOffset)
	{
		this.Target = target;
		this.TargetOffset = targetOffset;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000C8970 File Offset: 0x000C6B70
	public static bool GetActiveFocus(Entity theEntity, AIFocus<AIFocusAim> focus, out Vector3 activeFocus)
	{
		Vector3 position = theEntity.position;
		for (int i = 0; i < focus.FocusTargets.Length; i++)
		{
			Entity target = focus.FocusTargets[i].Target;
			if (target && !focus.FocusTargets[i].ConditionDistance.IsFocusDisabled(theEntity))
			{
				Vector3 vector = target.position;
				switch (focus.FocusTargets[i].TargetOffset)
				{
				case AIAimFocusOffset.Belly:
					vector = target.getBellyPosition();
					goto IL_00A7;
				case AIAimFocusOffset.Chest:
					vector = target.getChestPosition();
					goto IL_00A7;
				case AIAimFocusOffset.Head:
					vector = target.getHeadPosition();
					goto IL_00A7;
				}
				vector = focus.FocusTargets[i].Target.position;
				IL_00A7:
				activeFocus = vector;
				return true;
			}
		}
		activeFocus = default(Vector3);
		return false;
	}

	// Token: 0x040016E2 RID: 5858
	public readonly Entity Target;

	// Token: 0x040016E3 RID: 5859
	public readonly AIAimFocusOffset TargetOffset;

	// Token: 0x040016E4 RID: 5860
	public readonly AIFocusConditionDistance ConditionDistance;
}
