using System;
using UnityEngine.Scripting;

// Token: 0x020006E1 RID: 1761
[Preserve]
public class MinEventActionAddPartFPV : MinEventActionAddPart
{
	// Token: 0x0600368B RID: 13963 RVA: 0x0016783C File Offset: 0x00165A3C
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		return entityPlayerLocal != null && entityPlayerLocal.emodel.IsFPV && base.CanExecute(_eventType, _params);
	}
}
