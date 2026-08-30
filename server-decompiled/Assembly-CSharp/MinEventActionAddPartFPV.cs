using System;
using UnityEngine.Scripting;

// Token: 0x020006DF RID: 1759
[Preserve]
public class MinEventActionAddPartFPV : MinEventActionAddPart
{
	// Token: 0x06003648 RID: 13896 RVA: 0x00166ECC File Offset: 0x001650CC
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		return entityPlayerLocal != null && entityPlayerLocal.emodel.IsFPV && base.CanExecute(_eventType, _params);
	}
}
