using System;
using UnityEngine.Scripting;

// Token: 0x020006E2 RID: 1762
[Preserve]
public class MinEventActionAddPartTPV : MinEventActionAddPart
{
	// Token: 0x0600368D RID: 13965 RVA: 0x00167878 File Offset: 0x00165A78
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
