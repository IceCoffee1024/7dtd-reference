using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A8 RID: 6568
	[Preserve]
	public class ActionEjectFromVehicle : ActionBaseTargetAction
	{
		// Token: 0x0600C9C5 RID: 51653 RVA: 0x0049CBA4 File Offset: 0x0049ADA4
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

		// Token: 0x0600C9C6 RID: 51654 RVA: 0x0049CC2A File Offset: 0x0049AE2A
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
