using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public readonly struct AIFocusAim : IFocusTarget
{
	// Token: 0x06002153 RID: 8531 RVA: 0x000C95BE File Offset: 0x000C77BE
	public AIFocusAim(Entity target, AIAimFocusOffset targetOffset)
	{
		this.Target = target;
		this.TargetOffset = targetOffset;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000C95DC File Offset: 0x000C77DC
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

	// Token: 0x040016FE RID: 5886
	public readonly Entity Target;

	// Token: 0x040016FF RID: 5887
	public readonly AIAimFocusOffset TargetOffset;

	// Token: 0x04001700 RID: 5888
	public readonly AIFocusConditionDistance ConditionDistance;
}
