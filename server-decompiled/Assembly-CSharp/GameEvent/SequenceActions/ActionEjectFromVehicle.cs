using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197C RID: 6524
	[Preserve]
	public class ActionEjectFromVehicle : ActionBaseTargetAction
	{
		// Token: 0x0600C86D RID: 51309 RVA: 0x004986E0 File Offset: 0x004968E0
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.AttachedToEntity != null)
			{
				if (entityPlayer.isEntityRemote)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(entityPlayer.entityId), false, entityPlayer.entityId, -1, -1, null, 192, false);
				}
				else
				{
					(entityPlayer as EntityPlayerLocal).PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
				}
				entityPlayer.SendDetach();
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C86E RID: 51310 RVA: 0x00498766 File Offset: 0x00496966
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionEjectFromVehicle
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
