using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DF RID: 6623
	[Preserve]
	public class ActionRespawnEntity : BaseAction
	{
		// Token: 0x0600CAB3 RID: 51891 RVA: 0x004A2EE3 File Offset: 0x004A10E3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600CAB4 RID: 51892 RVA: 0x004A2F00 File Offset: 0x004A1100
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.oldEntityClass == -1 && base.Owner.Target != null && !(base.Owner.Target is EntityPlayer))
			{
				Entity target = base.Owner.Target;
				this.oldEntityClass = target.entityClass;
				this.oldEntityID = target.entityId;
				this.oldPosition = target.position;
				this.oldRotation = target.rotation;
			}
			if (this.delay > 0f)
			{
				this.delay -= Time.deltaTime;
				return BaseAction.ActionCompleteStates.InComplete;
			}
			World world = GameManager.Instance.World;
			GameEventActionSequence gameEventActionSequence = ((base.Owner.OwnerSequence == null) ? base.Owner : base.Owner.OwnerSequence);
			Entity entity = EntityFactory.CreateEntity(this.oldEntityClass, this.oldPosition, this.oldRotation, gameEventActionSequence.Target.entityId, gameEventActionSequence.ExtraData);
			if (entity == null)
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
			world.SpawnEntityInWorld(entity);
			world.RemoveEntity(this.oldEntityID, EnumRemoveEntityReason.Killed);
			base.Owner.Target = entity;
			EntityAlive entityAlive = entity as EntityAlive;
			EntityAlive entityAlive2 = gameEventActionSequence.Target as EntityAlive;
			if (entityAlive2 != null)
			{
				GameEventManager.Current.RegisterSpawnedEntity(entityAlive, entityAlive2, gameEventActionSequence.Requester, gameEventActionSequence, true);
				entityAlive.SetAttackTarget(entityAlive2, 12000);
			}
			if (base.Owner.Requester != null)
			{
				if (gameEventActionSequence.Requester is EntityPlayerLocal)
				{
					GameEventManager.Current.HandleGameEntitySpawned(gameEventActionSequence.Name, entityAlive.entityId, gameEventActionSequence.Tag);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(gameEventActionSequence.Name, gameEventActionSequence.Target.entityId, gameEventActionSequence.ExtraData, gameEventActionSequence.Tag, NetPackageGameEventResponse.ResponseTypes.TwitchSetOwner, entityAlive.entityId, -1, false, ""), false, gameEventActionSequence.Requester.entityId, -1, -1, null, 192, false);
				}
			}
			if (entity != null && this.AddToGroups != null)
			{
				for (int i = 0; i < this.AddToGroups.Length; i++)
				{
					if (this.AddToGroups[i] != "")
					{
						base.Owner.AddEntityToGroup(this.AddToGroups[i], entity);
					}
				}
			}
			if (this.respawnSound != "")
			{
				Manager.BroadcastPlayByLocalPlayer(this.oldPosition, this.respawnSound);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAB5 RID: 51893 RVA: 0x004A3178 File Offset: 0x004A1378
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntity.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntity.PropRespawnSound, ref this.respawnSound);
			properties.ParseString(ActionRespawnEntity.PropAddToGroup, ref this.addToGroup);
			properties.ParseFloat(ActionRespawnEntity.PropDelay, ref this.delay);
		}

		// Token: 0x0600CAB6 RID: 51894 RVA: 0x004A31D0 File Offset: 0x004A13D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRespawnEntity
			{
				targetGroup = this.targetGroup,
				addToGroup = this.addToGroup,
				AddToGroups = this.AddToGroups,
				respawnSound = this.respawnSound,
				delay = this.delay
			};
		}

		// Token: 0x04009966 RID: 39270
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009967 RID: 39271
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x04009968 RID: 39272
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x04009969 RID: 39273
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x0400996A RID: 39274
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x0400996B RID: 39275
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x0400996C RID: 39276
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x0400996D RID: 39277
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDelay = "delay";

		// Token: 0x0400996E RID: 39278
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityClass = -1;

		// Token: 0x0400996F RID: 39279
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityID = -1;

		// Token: 0x04009970 RID: 39280
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldPosition;

		// Token: 0x04009971 RID: 39281
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldRotation;

		// Token: 0x04009972 RID: 39282
		[PublicizedFrom(EAccessModifier.Private)]
		public float delay = 3f;
	}
}
