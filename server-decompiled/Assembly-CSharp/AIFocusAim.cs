using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public readonly struct AIFocusAim : IFocusTarget
{
	// Token: 0x0600214E RID: 8526 RVA: 0x000C9212 File Offset: 0x000C7412
	public AIFocusAim(Entity target, AIAimFocusOffset targetOffset)
	{
		this.Target = target;
		this.TargetOffset = targetOffset;
		this.ConditionDistance = default(AIFocusConditionDistance);
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000C9230 File Offset: 0x000C7430
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

	// Token: 0x040016EE RID: 5870
	public readonly Entity Target;

	// Token: 0x040016EF RID: 5871
	public readonly AIAimFocusOffset TargetOffset;

	// Token: 0x040016F0 RID: 5872
	public readonly AIFocusConditionDistance ConditionDistance;
}
