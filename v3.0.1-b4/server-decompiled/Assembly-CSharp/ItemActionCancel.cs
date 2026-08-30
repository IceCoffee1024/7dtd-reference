using System;
using UnityEngine.Scripting;

// Token: 0x02000594 RID: 1428
[Preserve]
public class ItemActionCancel : ItemAction
{
	// Token: 0x06002EE2 RID: 12002 RVA: 0x001296A0 File Offset: 0x001278A0
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (_bReleased)
		{
			int num = 1 - _actionData.indexInEntityOfAction;
			if (_actionData.invData.actionData[num] != null)
			{
				_actionData.invData.item.Actions[num].CancelAction(_actionData.invData.actionData[num]);
			}
		}
	}
}
