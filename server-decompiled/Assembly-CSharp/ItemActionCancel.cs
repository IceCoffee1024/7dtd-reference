using System;
using UnityEngine.Scripting;

// Token: 0x0200059A RID: 1434
[Preserve]
public class ItemActionCancel : ItemAction
{
	// Token: 0x06002F52 RID: 12114 RVA: 0x0012B230 File Offset: 0x00129430
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
