using System;
using UnityEngine.Scripting;

// Token: 0x020006D4 RID: 1748
[Preserve]
public class MinEventActionAddOrRemoveBuff : MinEventActionAddBuff
{
	// Token: 0x06003665 RID: 13925 RVA: 0x001666C9 File Offset: 0x001648C9
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		this.isAdd = base.CanExecute(_eventType, _params);
		return true;
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x001666DA File Offset: 0x001648DA
	public override void Execute(MinEventParams _params)
	{
		if (this.isAdd)
		{
			base.Execute(_params);
			return;
		}
		base.Remove(_params);
	}

	// Token: 0x04002BAE RID: 11182
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;
}
