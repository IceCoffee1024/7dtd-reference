using System;
using UnityEngine.Scripting;

// Token: 0x02000599 RID: 1433
[Preserve]
public class ItemActionCancel : ItemAction
{
	// Token: 0x06002F10 RID: 12048 RVA: 0x0012A710 File Offset: 0x00128910
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
