using System;
using UnityEngine.Scripting;

// Token: 0x020006EA RID: 1770
[Preserve]
public class MinEventActionAddPartTPV : MinEventActionAddPart
{
	// Token: 0x060036A4 RID: 13988 RVA: 0x00168660 File Offset: 0x00166860
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal == null || entityPlayerLocal.emodel.IsFPV)
		{
			EntityPlayer entityPlayer = _params.Self as EntityPlayer;
			if (entityPlayer == null || !entityPlayer.isEntityRemote)
			{
				return false;
			}
		}
		return base.CanExecute(_eventType, _params);
	}
}
