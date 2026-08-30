using System;
using UnityEngine.Scripting;

// Token: 0x020006DB RID: 1755
[Preserve]
public class MinEventActionLogItemData : MinEventActionBase
{
	// Token: 0x0600363B RID: 13883 RVA: 0x001664AC File Offset: 0x001646AC
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

	// Token: 0x04002BCD RID: 11213
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
