using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001983 RID: 6531
	[Preserve]
	public class ActionReplaceEntities : ActionBaseTargetAction
	{
		// Token: 0x0600C79A RID: 51098 RVA: 0x00496F58 File Offset: 0x00495158
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			string[] array = this.entityNames.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
				{
					if (keyValuePair.Value.entityClassName == array[i])
					{
						this.entityIDs.Add(keyValuePair.Key);
						if (this.entityIDs.Count == array.Length)
						{
							break;
						}
					}
				}
			}
			if (this.singleChoice && this.selectedEntityIndex == -1)
			{
				this.selectedEntityIndex = global::UnityEngine.Random.Range(0, this.entityIDs.Count);
			}
		}

		// Token: 0x0600C79B RID: 51099 RVA: 0x00497030 File Offset: 0x00495230
		public override void StartTargetAction()
		{
			this.newList = new List<Entity>();
		}

		// Token: 0x0600C79C RID: 51100 RVA: 0x0049703D File Offset: 0x0049523D
		public override void EndTargetAction()
		{
			if (this.targetGroup != "")
			{
				base.Owner.AddEntitiesToGroup(this.targetGroup, this.newList, false);
			}
		}

		// Token: 0x0600C79D RID: 51101 RVA: 0x0049706C File Offset: 0x0049526C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			World world = GameManager.Instance.World;
			if (target != null && !(target is EntityPlayer))
			{
				int num = ((this.selectedEntityIndex == -1) ? global::UnityEngine.Random.Range(0, this.entityIDs.Count) : this.selectedEntityIndex);
				Entity entity = EntityFactory.CreateEntity(this.entityIDs[num], target.position, target.rotation, (base.Owner.Target != null) ? base.Owner.Target.entityId : (-1), base.Owner.ExtraData);
				entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
				world.SpawnEntityInWorld(entity);
				this.newList.Add(entity);
				if (this.attackTarget)
				{
					EntityAlive entityAlive = entity as EntityAlive;
					if (entityAlive != null)
					{
						EntityAlive entityAlive2 = base.Owner.Target as EntityAlive;
						if (entityAlive2 != null)
						{
							GameEventManager.Current.RegisterSpawnedEntity(entityAlive, entityAlive2, base.Owner.Requester, base.Owner, true);
							entityAlive.SetAttackTarget(entityAlive2, 12000);
							entityAlive.aiManager.SetTargetOnlyPlayers(100f);
							if (base.Owner.Requester != null)
							{
								GameEventActionSequence gameEventActionSequence = ((base.Owner.OwnerSequence == null) ? base.Owner : base.Owner.OwnerSequence);
								if (base.Owner.Requester is EntityPlayerLocal)
								{
									GameEventManager.Current.HandleGameEntitySpawned(gameEventActionSequence.Name, entity.entityId, gameEventActionSequence.Tag);
								}
								else
								{
									SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(gameEventActionSequence.Name, gameEventActionSequence.Target.entityId, gameEventActionSequence.ExtraData, gameEventActionSequence.Tag, NetPackageGameEventResponse.ResponseTypes.EntitySpawned, entity.entityId, -1, false, ""), false, gameEventActionSequence.Requester.entityId, -1, -1, null, 192, false);
								}
							}
						}
					}
				}
				this.HandleRemoveData(target);
				GameManager.Instance.StartCoroutine(this.removeLater(target));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C79E RID: 51102 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRemoveData(Entity ent)
		{
		}

		// Token: 0x0600C79F RID: 51103 RVA: 0x0049727A File Offset: 0x0049547A
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator removeLater(Entity e)
		{
			yield return new WaitForSeconds(0.25f);
			if (e is EntityVehicle)
			{
				(e as EntityVehicle).Kill();
			}
			GameManager.Instance.World.RemoveEntity(e.entityId, EnumRemoveEntityReason.Killed);
			yield break;
		}

		// Token: 0x0600C7A0 RID: 51104 RVA: 0x0049728C File Offset: 0x0049548C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionReplaceEntities.PropEntityNames))
			{
				this.entityNames = properties.Values[ActionReplaceEntities.PropEntityNames];
			}
			if (properties.Values.ContainsKey(ActionReplaceEntities.PropSingleChoice))
			{
				this.singleChoice = StringParsers.ParseBool(properties.Values[ActionReplaceEntities.PropSingleChoice], 0, -1, true);
			}
			properties.ParseBool(ActionReplaceEntities.PropAttackTarget, ref this.attackTarget);
		}

		// Token: 0x0600C7A1 RID: 51105 RVA: 0x0049730C File Offset: 0x0049550C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceEntities
			{
				entityNames = this.entityNames,
				entityIDs = this.entityIDs,
				singleChoice = this.singleChoice,
				targetGroup = this.targetGroup,
				selectedEntityIndex = this.selectedEntityIndex,
				attackTarget = this.attackTarget
			};
		}

		// Token: 0x04009737 RID: 38711
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityNames = "";

		// Token: 0x04009738 RID: 38712
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool singleChoice;

		// Token: 0x04009739 RID: 38713
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityNames = "entity_names";

		// Token: 0x0400973A RID: 38714
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSingleChoice = "single_choice";

		// Token: 0x0400973B RID: 38715
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAttackTarget = "attack_target";

		// Token: 0x0400973C RID: 38716
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<int> entityIDs = new List<int>();

		// Token: 0x0400973D RID: 38717
		[PublicizedFrom(EAccessModifier.Protected)]
		public int selectedEntityIndex = -1;

		// Token: 0x0400973E RID: 38718
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Entity> newList;

		// Token: 0x0400973F RID: 38719
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool attackTarget = true;
	}
}
