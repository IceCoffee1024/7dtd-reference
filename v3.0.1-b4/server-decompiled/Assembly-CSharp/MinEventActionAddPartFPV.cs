using System;
using UnityEngine.Scripting;

// Token: 0x020006D5 RID: 1749
[Preserve]
public class MinEventActionAddPartFPV : MinEventActionAddPart
{
	// Token: 0x060035F2 RID: 13810 RVA: 0x00164CF4 File Offset: 0x00162EF4
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		return entityPlayerLocal != null && entityPlayerLocal.emodel.IsFPV && base.CanExecute(_eventType, _params);
	}
}
