using System;
using UnityEngine.Scripting;

// Token: 0x020006D2 RID: 1746
[Preserve]
public class MinEventActionAddOrRemoveBuff : MinEventActionAddBuff
{
	// Token: 0x06003622 RID: 13858 RVA: 0x00165D59 File Offset: 0x00163F59
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		this.isAdd = base.CanExecute(_eventType, _params);
		return true;
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00165D6A File Offset: 0x00163F6A
	public override void Execute(MinEventParams _params)
	{
		if (this.isAdd)
		{
			base.Execute(_params);
			return;
		}
		base.Remove(_params);
	}

	// Token: 0x04002BB5 RID: 11189
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;
}
