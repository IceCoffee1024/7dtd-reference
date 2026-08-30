using System;
using UnityEngine.Scripting;

// Token: 0x020006C8 RID: 1736
[Preserve]
public class MinEventActionAddOrRemoveBuff : MinEventActionAddBuff
{
	// Token: 0x060035CC RID: 13772 RVA: 0x00163B81 File Offset: 0x00161D81
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		this.isAdd = base.CanExecute(_eventType, _params);
		return true;
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x00163B92 File Offset: 0x00161D92
	public override void Execute(MinEventParams _params)
	{
		if (this.isAdd)
		{
			base.Execute(_params);
			return;
		}
		base.Remove(_params);
	}

	// Token: 0x04002B41 RID: 11073
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;
}
