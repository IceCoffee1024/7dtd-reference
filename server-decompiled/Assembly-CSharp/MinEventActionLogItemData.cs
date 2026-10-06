using System;
using UnityEngine.Scripting;

// Token: 0x020006DD RID: 1757
[Preserve]
public class MinEventActionLogItemData : MinEventActionBase
{
	// Token: 0x0600367E RID: 13950 RVA: 0x00166E1C File Offset: 0x0016501C
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self.inventory.holdingItem == null)
		{
			return;
		}
		Log.Out("Debug Item: '{0}' Tags: {1}", new object[]
		{
			_params.Self.inventory.holdingItem.GetItemName(),
			_params.Self.inventory.holdingItem.ItemTags.ToString()
		});
	}

	// Token: 0x04002BC6 RID: 11206
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
