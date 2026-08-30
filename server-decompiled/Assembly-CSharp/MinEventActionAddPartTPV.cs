using System;
using UnityEngine.Scripting;

// Token: 0x020006E0 RID: 1760
[Preserve]
public class MinEventActionAddPartTPV : MinEventActionAddPart
{
	// Token: 0x0600364A RID: 13898 RVA: 0x00166F08 File Offset: 0x00165108
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
