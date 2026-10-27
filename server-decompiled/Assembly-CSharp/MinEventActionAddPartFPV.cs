using System;
using UnityEngine.Scripting;

// Token: 0x020006E9 RID: 1769
[Preserve]
public class MinEventActionAddPartFPV : MinEventActionAddPart
{
	// Token: 0x060036A2 RID: 13986 RVA: 0x00168624 File Offset: 0x00166824
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		return entityPlayerLocal != null && entityPlayerLocal.emodel.IsFPV && base.CanExecute(_eventType, _params);
	}
}
