using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DE RID: 6622
	[Preserve]
	public class ActionRespawnEntities : BaseAction
	{
		// Token: 0x0600CAAC RID: 51884 RVA: 0x004A2AE5 File Offset: 0x004A0CE5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600CAAD RID: 51885 RVA: 0x004A2B04 File Offset: 0x004A0D04
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

		// Token: 0x0600CAAE RID: 51886 RVA: 0x004A2E15 File Offset: 0x004A1015
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600CAAF RID: 51887 RVA: 0x004A2E1E File Offset: 0x004A101E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntities.PropAddToGroup, ref this.addToGroup);
			properties.ParseString(ActionRespawnEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntities.PropRespawnSound, ref this.respawnSound);
		}

		// Token: 0x0600CAB0 RID: 51888 RVA: 0x004A2E5A File Offset: 0x004A105A
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

		// Token: 0x0400995C RID: 39260
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400995D RID: 39261
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x0400995E RID: 39262
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x0400995F RID: 39263
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x04009960 RID: 39264
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009961 RID: 39265
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x04009962 RID: 39266
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsMulti = "is_multi";

		// Token: 0x04009963 RID: 39267
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x04009964 RID: 39268
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009965 RID: 39269
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
