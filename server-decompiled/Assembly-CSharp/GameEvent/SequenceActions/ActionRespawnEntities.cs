using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B2 RID: 6578
	[Preserve]
	public class ActionRespawnEntities : BaseAction
	{
		// Token: 0x0600C954 RID: 51540 RVA: 0x0049E5F5 File Offset: 0x0049C7F5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C955 RID: 51541 RVA: 0x0049E614 File Offset: 0x0049C814
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

		// Token: 0x0600C956 RID: 51542 RVA: 0x0049E925 File Offset: 0x0049CB25
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600C957 RID: 51543 RVA: 0x0049E92E File Offset: 0x0049CB2E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntities.PropAddToGroup, ref this.addToGroup);
			properties.ParseString(ActionRespawnEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntities.PropRespawnSound, ref this.respawnSound);
		}

		// Token: 0x0600C958 RID: 51544 RVA: 0x0049E96A File Offset: 0x0049CB6A
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

		// Token: 0x04009896 RID: 39062
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009897 RID: 39063
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x04009898 RID: 39064
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x04009899 RID: 39065
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x0400989A RID: 39066
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x0400989B RID: 39067
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x0400989C RID: 39068
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsMulti = "is_multi";

		// Token: 0x0400989D RID: 39069
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x0400989E RID: 39070
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x0400989F RID: 39071
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
