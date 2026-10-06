using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001993 RID: 6547
	[Preserve]
	public class ActionBaseClientAction : ActionBaseTargetAction
	{
		// Token: 0x0600C961 RID: 51553 RVA: 0x00499B80 File Offset: 0x00497D80
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				this.OnServerPerform(entityPlayer);
				if (entityPlayer is EntityPlayerLocal)
				{
					this.OnClientPerform(entityPlayer);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(base.Owner.Name, entityPlayer.entityId, base.Owner.ExtraData, base.Owner.Tag, NetPackageGameEventResponse.ResponseTypes.ClientSequenceAction, -1, -1, false, base.GetActionKey()), false, entityPlayer.entityId, -1, -1, null, 192, false);
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C962 RID: 51554 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnServerPerform(Entity target)
		{
		}
	}
}
