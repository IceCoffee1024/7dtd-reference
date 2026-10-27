using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A06 RID: 6662
	[Preserve]
	public class ActionReplaceEntities : ActionBaseTargetAction
	{
		// Token: 0x0600CB8D RID: 52109 RVA: 0x004A528C File Offset: 0x004A348C
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

		// Token: 0x0600CB8E RID: 52110 RVA: 0x004A5364 File Offset: 0x004A3564
		public override void StartTargetAction()
		{
			this.newList = new List<Entity>();
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x004A5371 File Offset: 0x004A3571
		public override void EndTargetAction()
		{
			if (this.targetGroup != "")
			{
				base.Owner.AddEntitiesToGroup(this.targetGroup, this.newList, false);
			}
		}

		// Token: 0x0600CB90 RID: 52112 RVA: 0x004A53A0 File Offset: 0x004A35A0
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

		// Token: 0x0600CB91 RID: 52113 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRemoveData(Entity ent)
		{
		}

		// Token: 0x0600CB92 RID: 52114 RVA: 0x004A55AE File Offset: 0x004A37AE
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

		// Token: 0x0600CB93 RID: 52115 RVA: 0x004A55C0 File Offset: 0x004A37C0
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

		// Token: 0x0600CB94 RID: 52116 RVA: 0x004A5640 File Offset: 0x004A3840
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

		// Token: 0x04009A0F RID: 39439
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityNames = "";

		// Token: 0x04009A10 RID: 39440
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool singleChoice;

		// Token: 0x04009A11 RID: 39441
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityNames = "entity_names";

		// Token: 0x04009A12 RID: 39442
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSingleChoice = "single_choice";

		// Token: 0x04009A13 RID: 39443
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAttackTarget = "attack_target";

		// Token: 0x04009A14 RID: 39444
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<int> entityIDs = new List<int>();

		// Token: 0x04009A15 RID: 39445
		[PublicizedFrom(EAccessModifier.Protected)]
		public int selectedEntityIndex = -1;

		// Token: 0x04009A16 RID: 39446
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Entity> newList;

		// Token: 0x04009A17 RID: 39447
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool attackTarget = true;
	}
}
