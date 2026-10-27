using System;
using UnityEngine.Scripting;

// Token: 0x020006E5 RID: 1765
[Preserve]
public class MinEventActionLogItemData : MinEventActionBase
{
	// Token: 0x06003695 RID: 13973 RVA: 0x00167C04 File Offset: 0x00165E04
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

	// Token: 0x04002BE2 RID: 11234
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
