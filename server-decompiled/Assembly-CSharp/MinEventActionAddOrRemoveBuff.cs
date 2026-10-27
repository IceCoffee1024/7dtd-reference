using System;
using UnityEngine.Scripting;

// Token: 0x020006DC RID: 1756
[Preserve]
public class MinEventActionAddOrRemoveBuff : MinEventActionAddBuff
{
	// Token: 0x06003676 RID: 13942 RVA: 0x0016747A File Offset: 0x0016567A
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		this.isAdd = base.CanExecute(_eventType, _params);
		return true;
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x0016748B File Offset: 0x0016568B
	public override void Execute(MinEventParams _params)
	{
		if (this.isAdd)
		{
			base.Execute(_params);
			return;
		}
		base.Remove(_params);
	}

	// Token: 0x04002BCA RID: 11210
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;
}
