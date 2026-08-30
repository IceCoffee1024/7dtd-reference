using System;
using UnityEngine.Scripting;

// Token: 0x020006D6 RID: 1750
[Preserve]
public class MinEventActionAddPartTPV : MinEventActionAddPart
{
	// Token: 0x060035F4 RID: 13812 RVA: 0x00164D30 File Offset: 0x00162F30
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
