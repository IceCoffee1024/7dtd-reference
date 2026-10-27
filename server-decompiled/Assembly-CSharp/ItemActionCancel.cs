using System;
using UnityEngine.Scripting;

// Token: 0x020005A1 RID: 1441
[Preserve]
public class ItemActionCancel : ItemAction
{
	// Token: 0x06002F60 RID: 12128 RVA: 0x0012BE24 File Offset: 0x0012A024
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
