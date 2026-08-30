using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B3 RID: 6579
	[Preserve]
	public class ActionRespawnEntity : BaseAction
	{
		// Token: 0x0600C95B RID: 51547 RVA: 0x0049E9F3 File Offset: 0x0049CBF3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.AddToGroups = this.addToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C95C RID: 51548 RVA: 0x0049EA10 File Offset: 0x0049CC10
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

		// Token: 0x0600C95D RID: 51549 RVA: 0x0049EC88 File Offset: 0x0049CE88
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRespawnEntity.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionRespawnEntity.PropRespawnSound, ref this.respawnSound);
			properties.ParseString(ActionRespawnEntity.PropAddToGroup, ref this.addToGroup);
			properties.ParseFloat(ActionRespawnEntity.PropDelay, ref this.delay);
		}

		// Token: 0x0600C95E RID: 51550 RVA: 0x0049ECE0 File Offset: 0x0049CEE0
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

		// Token: 0x040098A0 RID: 39072
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040098A1 RID: 39073
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addToGroup = "";

		// Token: 0x040098A2 RID: 39074
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x040098A3 RID: 39075
		[PublicizedFrom(EAccessModifier.Protected)]
		public string respawnSound = "";

		// Token: 0x040098A4 RID: 39076
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040098A5 RID: 39077
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x040098A6 RID: 39078
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRespawnSound = "respawn_sound";

		// Token: 0x040098A7 RID: 39079
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDelay = "delay";

		// Token: 0x040098A8 RID: 39080
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityClass = -1;

		// Token: 0x040098A9 RID: 39081
		[PublicizedFrom(EAccessModifier.Private)]
		public int oldEntityID = -1;

		// Token: 0x040098AA RID: 39082
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldPosition;

		// Token: 0x040098AB RID: 39083
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 oldRotation;

		// Token: 0x040098AC RID: 39084
		[PublicizedFrom(EAccessModifier.Private)]
		public float delay = 3f;
	}
}
