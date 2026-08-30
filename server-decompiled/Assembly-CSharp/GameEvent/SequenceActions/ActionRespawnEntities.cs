using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198C RID: 6540
	[Preserve]
	public class ActionRespawnEntities : BaseAction
	{
		// Token: 0x0600C7CD RID: 51149 RVA: 0x0049829D File Offset: 0x0049649D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C7CE RID: 51150 RVA: 0x004982BC File Offset: 0x004964BC
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.entityList == null)
			{
				this.entityList = new List<EntityAlive>();
				List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
				if (entityGroup == null)
				{
					Debug.LogWarning("ActionReviveEntities: Target Group " + this.targetGroup + " Does not exist!");
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				for (int i = 0; i < entityGroup.Count; i++)
				{
					EntityAlive entityAlive = entityGroup[i] as EntityAlive;
					if (entityAlive != null)
					{
						this.entityList.Add(entityAlive);
					}
				}
			}
			else if (this.entityList.Count > 0)
			{
				this.checkTime -= Time.deltaTime;
				if (this.checkTime > 0f)
				{
					return BaseAction.ActionCompleteStates.Complete;
				}
				World world = GameManager.Instance.World;
				for (int j = 0; j < this.entityList.Count; j++)
				{
					if (this.entityList[j] != null && !this.entityList[j].IsAlive())
					{
						Entity entity = this.entityList[j];
						Entity entity2 = EntityFactory.CreateEntity(this.entityList[j].entityClass, entity.position, entity.rotation, base.Owner.Target.entityId, base.Owner.ExtraData);
						entity2.SetSpawnerSource(EnumSpawnerSource.Dynamic);
						world.SpawnEntityInWorld(entity2);
						world.RemoveEntity(entity.entityId, EnumRemoveEntityReason.Killed);
						EntityAlive entityAlive2 = entity2 as EntityAlive;
						GameEventManager.Current.RegisterSpawnedEntity(entity2 as EntityAlive, base.Owner.Target, base.Owner.Requester, base.Owner, true);
						if (base.Owner.Requester != null)
						{
							if (base.Owner.Requester is EntityPlayerLocal)
							{
								GameEventManager.Current.HandleGameEntitySpawned(base.Owner.Name, entityAlive2.entityId, base.Owner.Tag);
							}
							else
							{
								SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(base.Owner.Name, base.Owner.Target.entityId, base.Owner.ExtraData, base.Owner.Tag, NetPackageGameEventResponse.ResponseTypes.TwitchSetOwner, entityAlive2.entityId, -1, false, ""), false, base.Owner.Requester.entityId, -1, -1, null, 192, false);
							}
						}
						if (this.respawnSound != "")
						{
							Manager.BroadcastPlayByLocalPlayer(entity.position, this.respawnSound);
						}
						if (entity2 != null && this.AddToGroups != null)
						{
							for (int k = 0; k < this.AddToGroups.Length; k++)
							{
								if (this.AddToGroups[k] != "")
								{
									base.Owner.AddEntityToGroup(this.AddToGroups[k], entity2);
								}
							}
						}
						this.entityList.RemoveAt(j);
						return BaseAction.ActionCompleteStates.InComplete;
					}
				}
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C7CF RID: 51151 RVA: 0x004985CD File Offset: 0x004967CD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600C7D0 RID: 51152 RVA: 0x004985D6 File Offset: 0x004967D6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntities.PropAddToGroup, ref this.addToGroup);
			properties.ParseString(ActionRespawnEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntities.PropRespawnSound, ref this.respawnSound);
		}

		// Token: 0x0600C7D1 RID: 51153 RVA: 0x00498612 File Offset: 0x00496812
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRespawnEntities
			{
				targetGroup = this.targetGroup,
				addToGroup = this.addToGroup,
				respawnSound = this.respawnSound
			};
		}

		// Token: 0x0400976D RID: 38765
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400976E RID: 38766
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x0400976F RID: 38767
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x04009770 RID: 38768
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x04009771 RID: 38769
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009772 RID: 38770
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x04009773 RID: 38771
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsMulti = "is_multi";

		// Token: 0x04009774 RID: 38772
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x04009775 RID: 38773
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009776 RID: 38774
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
