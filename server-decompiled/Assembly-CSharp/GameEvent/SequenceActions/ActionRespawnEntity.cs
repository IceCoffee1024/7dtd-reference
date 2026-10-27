using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A10 RID: 6672
	[Preserve]
	public class ActionRespawnEntity : BaseAction
	{
		// Token: 0x0600CBC7 RID: 52167 RVA: 0x004A69E3 File Offset: 0x004A4BE3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600CBC8 RID: 52168 RVA: 0x004A6A00 File Offset: 0x004A4C00
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

		// Token: 0x0600CBC9 RID: 52169 RVA: 0x004A6C78 File Offset: 0x004A4E78
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntity.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntity.PropRespawnSound, ref this.respawnSound);
			properties.ParseString(ActionRespawnEntity.PropAddToGroup, ref this.addToGroup);
			properties.ParseFloat(ActionRespawnEntity.PropDelay, ref this.delay);
		}

		// Token: 0x0600CBCA RID: 52170 RVA: 0x004A6CD0 File Offset: 0x004A4ED0
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

		// Token: 0x04009A4F RID: 39503
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009A50 RID: 39504
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x04009A51 RID: 39505
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x04009A52 RID: 39506
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x04009A53 RID: 39507
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009A54 RID: 39508
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x04009A55 RID: 39509
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x04009A56 RID: 39510
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDelay = "delay";

		// Token: 0x04009A57 RID: 39511
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityClass = -1;

		// Token: 0x04009A58 RID: 39512
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityID = -1;

		// Token: 0x04009A59 RID: 39513
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldPosition;

		// Token: 0x04009A5A RID: 39514
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldRotation;

		// Token: 0x04009A5B RID: 39515
		[PublicizedFrom(EAccessModifier.Private)]
		public float delay = 3f;
	}
}
