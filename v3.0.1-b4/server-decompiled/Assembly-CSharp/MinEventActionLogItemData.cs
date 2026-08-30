using System;
using UnityEngine.Scripting;

// Token: 0x020006D1 RID: 1745
[Preserve]
public class MinEventActionLogItemData : MinEventActionBase
{
	// Token: 0x060035E5 RID: 13797 RVA: 0x001642D4 File Offset: 0x001624D4
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

	// Token: 0x04002B59 RID: 11097
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
