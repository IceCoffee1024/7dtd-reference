using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D4 RID: 6612
	[Preserve]
	public class ActionTwitchAddEntitiesToSpawned : ActionBaseTargetAction
	{
		// Token: 0x0600C929 RID: 51497 RVA: 0x0049E2C4 File Offset: 0x0049C4C4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityPlayer entityPlayer = base.Owner.Target as EntityPlayer;
			if (entityPlayer != null && !entityPlayer.TwitchEnabled)
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				if (target is EntityPlayer)
				{
					return BaseAction.ActionCompleteStates.Complete;
				}
				if (!TwitchManager.Current.LiveListContains(entityAlive.entityId))
				{
					entityAlive.SetSpawnByData(base.Owner.Target.entityId, base.Owner.ExtraData);
					GameEventManager.Current.RegisterSpawnedEntity(entityAlive, target, base.Owner.Requester, base.Owner, true);
					if (base.Owner.Requester != null)
					{
						if (base.Owner.Requester is EntityPlayerLocal)
						{
							GameEventManager.Current.HandleGameEntitySpawned(base.Owner.Name, entityAlive.entityId, base.Owner.Tag);
						}
						else
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(base.Owner.Name, base.Owner.Target.entityId, base.Owner.ExtraData, base.Owner.Tag, NetPackageGameEventResponse.ResponseTypes.TwitchSetOwner, entityAlive.entityId, -1, false, ""), false, base.Owner.Requester.entityId, -1, -1, null, 192, false);
						}
					}
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C92A RID: 51498 RVA: 0x0049E420 File Offset: 0x0049C620
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchAddEntitiesToSpawned
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
