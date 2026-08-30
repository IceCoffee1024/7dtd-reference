using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200196B RID: 6507
	[Preserve]
	public class ActionBaseSpawn : BaseAction
	{
		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x0600C817 RID: 51223 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool UseRepeating
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600C818 RID: 51224 RVA: 0x00495BA4 File Offset: 0x00493DA4
		[PublicizedFrom(EAccessModifier.Protected)]
		public int ModifiedCount(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive == null)
			{
				return this.count;
			}
			if (this.count == -1)
			{
				this.count = GameEventManager.GetIntValue(entityAlive, this.countText, 1);
			}
			EntityPlayer entityPlayer = entityAlive as EntityPlayer;
			if (entityPlayer != null && entityPlayer.Party != null)
			{
				int num = this.count;
				if (!this.UseRepeating)
				{
					num += this.GetPartyAdditionCount(entityPlayer);
				}
				if (base.Owner.ActionType != GameEventActionSequence.ActionTypes.Game && !this.ignoreMultiplier)
				{
					num = (int)EffectManager.GetValue(PassiveEffects.TwitchSpawnMultiplier, null, (float)num, entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				}
				return num;
			}
			if (base.Owner.ActionType != GameEventActionSequence.ActionTypes.Game && !this.ignoreMultiplier)
			{
				return (int)EffectManager.GetValue(PassiveEffects.TwitchSpawnMultiplier, null, (float)this.count, entityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
			return this.count;
		}

		// Token: 0x0600C819 RID: 51225 RVA: 0x00495C8C File Offset: 0x00493E8C
		[PublicizedFrom(EAccessModifier.Protected)]
		public int GetPartyAdditionCount(EntityPlayer player)
		{
			if (player.Party != null)
			{
				int intValue = GameEventManager.GetIntValue(player, this.partyAdditionText, 0);
				return (player.Party.MemberList.Count - 1) * intValue;
			}
			return 0;
		}

		// Token: 0x0600C81A RID: 51226 RVA: 0x00495CC8 File Offset: 0x00493EC8
		public override bool CanPerform(Entity player)
		{
			this.count = GameEventManager.GetIntValue(player as EntityAlive, this.countText, 1);
			if (!this.useEntityGroup && this.entityIDs.Count == 0)
			{
				Debug.LogWarning("Error: GameEventSequence missing spawn type: " + base.Owner.Name);
				return false;
			}
			if (player != null && player.IsDead())
			{
				return false;
			}
			if (GameEventManager.Current.CurrentCount + this.count > GameEventManager.Current.MaxSpawnCount)
			{
				return false;
			}
			if (!this.safeSpawn)
			{
				if (player != null && !GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(player.position), null, false))
				{
					return false;
				}
				if (player == null && !GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(base.Owner.TargetPosition), null, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C81B RID: 51227 RVA: 0x00495DB0 File Offset: 0x00493FB0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			this.SetupEntityIDs();
			this.AddToGroups = this.AddToGroup.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C81C RID: 51228 RVA: 0x00495DD4 File Offset: 0x00493FD4
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SetupEntityIDs()
		{
			if (this.useEntityGroup)
			{
				this.entityIDs.Clear();
				return;
			}
			string[] array = this.entityNames.Split(',', StringSplitOptions.None);
			this.entityIDs.Clear();
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
		}

		// Token: 0x0600C81D RID: 51229 RVA: 0x00495E9C File Offset: 0x0049409C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (!base.Owner.HasTarget())
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			this.HandleExtraAction();
			switch (this.CurrentState)
			{
			case ActionBaseSpawn.SpawnUpdateTypes.NeedSpawnEntries:
				if (this.SpawnEntries == null)
				{
					if (!this.useEntityGroup && this.entityIDs.Count == 0)
					{
						this.SetupEntityIDs();
						return BaseAction.ActionCompleteStates.InComplete;
					}
					this.SpawnEntries = new List<ActionBaseSpawn.SpawnEntry>();
					if (this.singleChoice && this.selectedEntityIndex == -1)
					{
						this.selectedEntityIndex = global::UnityEngine.Random.Range(0, this.entityIDs.Count);
					}
					GameStageDefinition gameStageDefinition = null;
					int num = -1;
					if (this.useEntityGroup)
					{
						gameStageDefinition = GameStageDefinition.GetGameStage(this.entityNames);
						if (gameStageDefinition == null)
						{
							return BaseAction.ActionCompleteStates.InCompleteRefund;
						}
					}
					if (this.targetGroup != "")
					{
						List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
						if (entityGroup != null)
						{
							for (int i = 0; i < entityGroup.Count; i++)
							{
								EntityPlayer entityPlayer = entityGroup[i] as EntityPlayer;
								if (entityPlayer != null && (base.Owner.ActionType != GameEventActionSequence.ActionTypes.TwitchAction || entityPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Enabled))
								{
									int num2 = this.ModifiedCount(entityPlayer);
									World world = GameManager.Instance.World;
									for (int j = 0; j < num2; j++)
									{
										if (this.useEntityGroup)
										{
											int randomFromGroup = EntityGroups.GetRandomFromGroup(gameStageDefinition.GetStage(entityPlayer.PartyGameStage).GetSpawnGroup(0).groupName, ref num, null);
											this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
											{
												EntityTypeID = randomFromGroup,
												Target = entityPlayer
											});
										}
										else
										{
											int num3 = ((this.selectedEntityIndex == -1) ? global::UnityEngine.Random.Range(0, this.entityIDs.Count) : this.selectedEntityIndex);
											this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
											{
												EntityTypeID = this.entityIDs[num3],
												Target = entityPlayer
											});
										}
									}
									if (this.attackTarget)
									{
										base.Owner.ReservedSpawnCount += num2;
										GameEventManager.Current.ReservedCount += num2;
									}
								}
							}
						}
						else
						{
							int num4 = this.ModifiedCount(base.Owner.Target);
							for (int k = 0; k < num4; k++)
							{
								if (this.useEntityGroup)
								{
									EntityPlayer entityPlayer2 = base.Owner.Target as EntityPlayer;
									if (entityPlayer2 != null)
									{
										int randomFromGroup2 = EntityGroups.GetRandomFromGroup(gameStageDefinition.GetStage(entityPlayer2.PartyGameStage).GetSpawnGroup(0).groupName, ref num, null);
										this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
										{
											EntityTypeID = randomFromGroup2,
											Target = entityPlayer2
										});
									}
								}
								else
								{
									int num5 = ((this.selectedEntityIndex == -1) ? global::UnityEngine.Random.Range(0, this.entityIDs.Count) : this.selectedEntityIndex);
									this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
									{
										EntityTypeID = this.entityIDs[num5],
										Target = base.Owner.Target
									});
								}
							}
						}
					}
					else
					{
						int num6 = this.ModifiedCount(base.Owner.Target);
						for (int l = 0; l < num6; l++)
						{
							if (this.useEntityGroup)
							{
								EntityPlayer entityPlayer3 = base.Owner.Target as EntityPlayer;
								if (entityPlayer3 == null)
								{
									Debug.LogWarning("ActionBaseSpawn: Use EntityGroup requires a player target.");
									return BaseAction.ActionCompleteStates.InCompleteRefund;
								}
								int randomFromGroup3 = EntityGroups.GetRandomFromGroup(gameStageDefinition.GetStage(entityPlayer3.PartyGameStage).GetSpawnGroup(0).groupName, ref num, null);
								this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
								{
									EntityTypeID = randomFromGroup3,
									Target = entityPlayer3
								});
							}
							else
							{
								int num7 = ((this.selectedEntityIndex == -1) ? global::UnityEngine.Random.Range(0, this.entityIDs.Count) : this.selectedEntityIndex);
								this.SpawnEntries.Add(new ActionBaseSpawn.SpawnEntry
								{
									EntityTypeID = this.entityIDs[num7],
									Target = base.Owner.Target
								});
							}
						}
					}
					this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.NeedPosition;
				}
				break;
			case ActionBaseSpawn.SpawnUpdateTypes.NeedPosition:
				if (this.spawnType == ActionBaseSpawn.SpawnTypes.NearTarget && base.Owner.Target == null && base.Owner.TargetPosition.y != 0f)
				{
					this.spawnType = ActionBaseSpawn.SpawnTypes.NearPosition;
				}
				switch (this.spawnType)
				{
				case ActionBaseSpawn.SpawnTypes.NearTarget:
					if (base.Owner.Target == null)
					{
						return BaseAction.ActionCompleteStates.InCompleteRefund;
					}
					this.position = base.Owner.Target.position;
					this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
					break;
				case ActionBaseSpawn.SpawnTypes.Position:
					if (base.Owner.TargetPosition.y != 0f)
					{
						this.position = base.Owner.TargetPosition;
						this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
						this.resetTime = 3f;
					}
					else if (base.Owner.Target != null)
					{
						if (!ActionBaseSpawn.FindValidPosition(out this.position, base.Owner.Target, this.minDistance, this.maxDistance, this.safeSpawn, this.yOffset, this.airSpawn))
						{
							return BaseAction.ActionCompleteStates.InComplete;
						}
						this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
						this.resetTime = 3f;
					}
					else
					{
						this.spawnType = ActionBaseSpawn.SpawnTypes.NearTarget;
						this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
					}
					break;
				case ActionBaseSpawn.SpawnTypes.NearPosition:
					if (base.Owner.TargetPosition.y != 0f)
					{
						this.position = base.Owner.TargetPosition;
					}
					else if (base.Owner.Target != null)
					{
						this.position = base.Owner.Target.position;
					}
					this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
					break;
				case ActionBaseSpawn.SpawnTypes.WanderingHorde:
					this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities;
					if (base.Owner.TargetPosition == Vector3.zero && base.Owner.Target != null)
					{
						base.Owner.TargetPosition = base.Owner.Target.position;
					}
					break;
				}
				break;
			case ActionBaseSpawn.SpawnUpdateTypes.SpawnEntities:
				if (this.SpawnEntries.Count == 0)
				{
					if (this.UseRepeating)
					{
						if (this.HandleRepeat())
						{
							this.SpawnEntries = null;
							this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.NeedSpawnEntries;
						}
						return BaseAction.ActionCompleteStates.InComplete;
					}
					if (this.clearPositionOnComplete)
					{
						base.Owner.TargetPosition = Vector3.zero;
					}
					if (!this.hasSpawned)
					{
						return BaseAction.ActionCompleteStates.InCompleteRefund;
					}
					return BaseAction.ActionCompleteStates.Complete;
				}
				else
				{
					if (this.spawnType == ActionBaseSpawn.SpawnTypes.Position)
					{
						this.resetTime -= Time.deltaTime;
						if (this.resetTime <= 0f)
						{
							this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.NeedPosition;
							return BaseAction.ActionCompleteStates.InComplete;
						}
					}
					for (int m = 0; m < this.SpawnEntries.Count; m++)
					{
						ActionBaseSpawn.SpawnEntry spawnEntry = this.SpawnEntries[m];
						if (spawnEntry.Target == null && this.spawnType != ActionBaseSpawn.SpawnTypes.Position)
						{
							this.SpawnEntries.RemoveAt(m);
							break;
						}
						Entity entity = null;
						switch (this.spawnType)
						{
						case ActionBaseSpawn.SpawnTypes.NearTarget:
							entity = this.SpawnEntity(spawnEntry.EntityTypeID, spawnEntry.Target, spawnEntry.Target.position, this.minDistance, this.maxDistance, this.safeSpawn, this.yOffset);
							break;
						case ActionBaseSpawn.SpawnTypes.Position:
							entity = this.SpawnEntity(spawnEntry.EntityTypeID, spawnEntry.Target, this.position, 1f, 4f, this.safeSpawn, this.yOffset);
							break;
						case ActionBaseSpawn.SpawnTypes.NearPosition:
							entity = this.SpawnEntity(spawnEntry.EntityTypeID, spawnEntry.Target, this.position, this.minDistance, this.maxDistance, this.safeSpawn, this.yOffset);
							break;
						case ActionBaseSpawn.SpawnTypes.WanderingHorde:
							if (!GameManager.Instance.World.GetMobRandomSpawnPosWithWater(base.Owner.TargetPosition, (int)this.minDistance, (int)this.maxDistance, 15, false, out this.position))
							{
								return BaseAction.ActionCompleteStates.InComplete;
							}
							entity = this.SpawnEntity(spawnEntry.EntityTypeID, spawnEntry.Target, this.position, 1f, 1f, this.safeSpawn, this.yOffset);
							break;
						}
						if (entity != null)
						{
							this.resetTime = 60f;
							this.AddPropertiesToSpawnedEntity(entity);
							base.Owner.TargetPosition = this.position;
							if (this.AddToGroups != null)
							{
								for (int n = 0; n < this.AddToGroups.Length; n++)
								{
									if (this.AddToGroups[n] != "")
									{
										base.Owner.AddEntityToGroup(this.AddToGroups[n], entity);
									}
								}
							}
							if (this.attackTarget)
							{
								EntityAlive entityAlive = entity as EntityAlive;
								if (entityAlive != null)
								{
									EntityAlive entityAlive2 = base.Owner.Target as EntityAlive;
									if (entityAlive2 != null)
									{
										this.HandleTargeting(entityAlive, entityAlive2);
										GameEventManager.Current.RegisterSpawnedEntity(entity, entityAlive2, base.Owner.Requester, base.Owner, this.isAggressive);
										base.Owner.ReservedSpawnCount--;
										GameEventManager.Current.ReservedCount--;
									}
								}
							}
							if (base.Owner.Requester != null)
							{
								GameEventActionSequence gameEventActionSequence = ((base.Owner.OwnerSequence == null) ? base.Owner : base.Owner.OwnerSequence);
								if (base.Owner.Requester is EntityPlayerLocal)
								{
									GameEventManager.Current.HandleGameEntitySpawned(gameEventActionSequence.Name, entity.entityId, gameEventActionSequence.Tag);
								}
								else
								{
									SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(gameEventActionSequence.Name, -1, string.Empty, gameEventActionSequence.Tag, NetPackageGameEventResponse.ResponseTypes.EntitySpawned, entity.entityId, -1, false, ""), false, gameEventActionSequence.Requester.entityId, -1, -1, null, 192, false);
								}
							}
							this.hasSpawned = true;
							this.SpawnEntries.RemoveAt(m);
							break;
						}
					}
				}
				break;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C81E RID: 51230 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleExtraAction()
		{
		}

		// Token: 0x0600C81F RID: 51231 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleRepeat()
		{
			return false;
		}

		// Token: 0x0600C820 RID: 51232 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleTargeting(EntityAlive attacker, EntityAlive targetAlive)
		{
		}

		// Token: 0x0600C821 RID: 51233 RVA: 0x00496894 File Offset: 0x00494A94
		public static bool FindValidPosition(out Vector3 newPoint, Entity entity, float minDistance, float maxDistance, bool spawnInSafe, float yOffset = 0f, bool spawnInAir = false)
		{
			return ActionBaseSpawn.FindValidPosition(out newPoint, entity.position, minDistance, maxDistance, spawnInSafe, yOffset, spawnInAir, 0f);
		}

		// Token: 0x0600C822 RID: 51234 RVA: 0x004968B0 File Offset: 0x00494AB0
		public static bool FindValidPosition(out Vector3 newPoint, Vector3 startPoint, float minDistance, float maxDistance, bool spawnInSafe, float yOffset = 0f, bool spawnInAir = false, float raycastOffset = 0f)
		{
			World world = GameManager.Instance.World;
			if (world == null)
			{
				newPoint = Vector3.zero;
				return false;
			}
			Vector3 vector = new Vector3(world.GetGameRandom().RandomFloat * 2f + -1f, 0f, world.GetGameRandom().RandomFloat * 2f + -1f);
			vector.Normalize();
			float num = world.GetGameRandom().RandomFloat * (maxDistance - minDistance) + minDistance;
			newPoint = startPoint + vector * num;
			newPoint.y = startPoint.y + 1.5f;
			if (yOffset != 0f)
			{
				newPoint += Vector3.up * yOffset;
			}
			startPoint += vector * raycastOffset;
			Ray ray = new Ray(startPoint, (newPoint - startPoint).normalized);
			if (Voxel.Raycast(world, ray, num, -538750989, 67, 0f))
			{
				return false;
			}
			BlockValue block = world.GetBlock(new Vector3i(newPoint - ray.direction * 0.5f));
			if (block.Block.IsCollideMovement || block.Block.IsCollideArrows)
			{
				return false;
			}
			Vector3i vector3i = new Vector3i(startPoint);
			if (!spawnInSafe && !world.CanPlaceBlockAt(vector3i, null, false))
			{
				return false;
			}
			if (!spawnInAir)
			{
				if (!Voxel.Raycast(world, new Ray(newPoint, Vector3.down), 3f + yOffset, false, false))
				{
					return false;
				}
				newPoint = Voxel.voxelRayHitInfo.hit.pos;
			}
			return true;
		}

		// Token: 0x0600C823 RID: 51235 RVA: 0x00496A60 File Offset: 0x00494C60
		public Entity SpawnEntity(int spawnedEntityID, Entity target, Vector3 startPoint, float minDistance, float maxDistance, bool spawnInSafe, float yOffset = 0f)
		{
			World world = GameManager.Instance.World;
			Vector3 vector = ((target != null) ? new Vector3(0f, target.transform.eulerAngles.y + 180f, 0f) : Vector3.zero);
			Vector3 zero = Vector3.zero;
			Entity entity = null;
			if (ActionBaseSpawn.FindValidPosition(out zero, startPoint, minDistance, maxDistance, spawnInSafe, yOffset, this.airSpawn, this.raycastOffset))
			{
				int num = -1;
				if (base.Owner.TwitchActivated && target != null)
				{
					num = target.entityId;
				}
				entity = EntityFactory.CreateEntity(spawnedEntityID, zero + new Vector3(0f, 0.5f, 0f), vector, num, base.Owner.ExtraData);
				entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
				world.SpawnEntityInWorld(entity);
				if (target != null && this.spawnSound != "")
				{
					Manager.BroadcastPlayByLocalPlayer(entity.position, this.spawnSound);
				}
			}
			return entity;
		}

		// Token: 0x0600C824 RID: 51236 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void AddPropertiesToSpawnedEntity(Entity entity)
		{
		}

		// Token: 0x0600C825 RID: 51237 RVA: 0x00496B62 File Offset: 0x00494D62
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.CurrentState = ActionBaseSpawn.SpawnUpdateTypes.NeedSpawnEntries;
			this.SpawnEntries = null;
			this.selectedEntityIndex = -1;
		}

		// Token: 0x0600C826 RID: 51238 RVA: 0x00496B7C File Offset: 0x00494D7C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseSpawn.PropEntityNames, ref this.entityNames);
			properties.ParseBool(ActionBaseSpawn.PropSingleChoice, ref this.singleChoice);
			properties.ParseString(ActionBaseSpawn.PropSpawnCount, ref this.countText);
			properties.ParseString(ActionBaseSpawn.PropPartyAddition, ref this.partyAdditionText);
			properties.ParseFloat(ActionBaseSpawn.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionBaseSpawn.PropMaxDistance, ref this.maxDistance);
			properties.ParseBool(ActionBaseSpawn.PropSpawnInSafe, ref this.safeSpawn);
			properties.ParseBool(ActionBaseSpawn.PropAttackTarget, ref this.attackTarget);
			properties.ParseBool(ActionBaseSpawn.PropSpawnInAir, ref this.airSpawn);
			properties.ParseString(ActionBaseSpawn.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionBaseSpawn.PropAddToGroup, ref this.AddToGroup);
			properties.ParseFloat(ActionBaseSpawn.PropYOffset, ref this.yOffset);
			properties.ParseBool(ActionBaseSpawn.PropClearPositionOnComplete, ref this.clearPositionOnComplete);
			properties.ParseBool(ActionBaseSpawn.PropIgnoreSpawnMultiplier, ref this.ignoreMultiplier);
			properties.ParseEnum<ActionBaseSpawn.SpawnTypes>(ActionBaseSpawn.PropSpawnType, ref this.spawnType);
			properties.ParseFloat(ActionBaseSpawn.PropRaycastOffset, ref this.raycastOffset);
			properties.ParseBool(ActionBaseSpawn.PropIsAggressive, ref this.isAggressive);
			properties.ParseString(ActionBaseSpawn.PropSpawnSound, ref this.spawnSound);
			if (properties.Contains(ActionBaseSpawn.PropEntityGroup))
			{
				this.useEntityGroup = true;
				properties.ParseString(ActionBaseSpawn.PropEntityGroup, ref this.entityNames);
			}
		}

		// Token: 0x040096E0 RID: 38624
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityNames = "";

		// Token: 0x040096E1 RID: 38625
		[PublicizedFrom(EAccessModifier.Protected)]
		public string countText = "";

		// Token: 0x040096E2 RID: 38626
		[PublicizedFrom(EAccessModifier.Protected)]
		public int count = -1;

		// Token: 0x040096E3 RID: 38627
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool singleChoice;

		// Token: 0x040096E4 RID: 38628
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 8f;

		// Token: 0x040096E5 RID: 38629
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 12f;

		// Token: 0x040096E6 RID: 38630
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeSpawn;

		// Token: 0x040096E7 RID: 38631
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040096E8 RID: 38632
		[PublicizedFrom(EAccessModifier.Protected)]
		public string AddToGroup = "";

		// Token: 0x040096E9 RID: 38633
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddToGroups;

		// Token: 0x040096EA RID: 38634
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool attackTarget = true;

		// Token: 0x040096EB RID: 38635
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool airSpawn;

		// Token: 0x040096EC RID: 38636
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool hasSpawned;

		// Token: 0x040096ED RID: 38637
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool clearPositionOnComplete;

		// Token: 0x040096EE RID: 38638
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseSpawn.SpawnTypes spawnType;

		// Token: 0x040096EF RID: 38639
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yOffset;

		// Token: 0x040096F0 RID: 38640
		[PublicizedFrom(EAccessModifier.Protected)]
		public string partyAdditionText = "";

		// Token: 0x040096F1 RID: 38641
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool useEntityGroup;

		// Token: 0x040096F2 RID: 38642
		[PublicizedFrom(EAccessModifier.Protected)]
		public string spawnSound = "";

		// Token: 0x040096F3 RID: 38643
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityNames = "entity_names";

		// Token: 0x040096F4 RID: 38644
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityGroup = "entity_group";

		// Token: 0x040096F5 RID: 38645
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSingleChoice = "single_choice";

		// Token: 0x040096F6 RID: 38646
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnCount = "spawn_count";

		// Token: 0x040096F7 RID: 38647
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040096F8 RID: 38648
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040096F9 RID: 38649
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInSafe = "safe_spawn";

		// Token: 0x040096FA RID: 38650
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040096FB RID: 38651
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddToGroup = "add_to_group";

		// Token: 0x040096FC RID: 38652
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAttackTarget = "attack_target";

		// Token: 0x040096FD RID: 38653
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnInAir = "air_spawn";

		// Token: 0x040096FE RID: 38654
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnType = "spawn_type";

		// Token: 0x040096FF RID: 38655
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPartyAddition = "party_addition";

		// Token: 0x04009700 RID: 38656
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropClearPositionOnComplete = "clear_position_on_complete";

		// Token: 0x04009701 RID: 38657
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIgnoreSpawnMultiplier = "ignore_multiplier";

		// Token: 0x04009702 RID: 38658
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYOffset = "yoffset";

		// Token: 0x04009703 RID: 38659
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRaycastOffset = "raycast_offset";

		// Token: 0x04009704 RID: 38660
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsAggressive = "is_aggressive";

		// Token: 0x04009705 RID: 38661
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnSound = "spawn_sound";

		// Token: 0x04009706 RID: 38662
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<int> entityIDs = new List<int>();

		// Token: 0x04009707 RID: 38663
		[PublicizedFrom(EAccessModifier.Protected)]
		public int selectedEntityIndex = -1;

		// Token: 0x04009708 RID: 38664
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentCount = 1;

		// Token: 0x04009709 RID: 38665
		[PublicizedFrom(EAccessModifier.Private)]
		public float resetTime = 1f;

		// Token: 0x0400970A RID: 38666
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreMultiplier;

		// Token: 0x0400970B RID: 38667
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 position = Vector3.zero;

		// Token: 0x0400970C RID: 38668
		[PublicizedFrom(EAccessModifier.Protected)]
		public float raycastOffset;

		// Token: 0x0400970D RID: 38669
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isAggressive = true;

		// Token: 0x0400970E RID: 38670
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseSpawn.SpawnUpdateTypes CurrentState;

		// Token: 0x0400970F RID: 38671
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionBaseSpawn.SpawnEntry> SpawnEntries;

		// Token: 0x0200196C RID: 6508
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpawnTypes
		{
			// Token: 0x04009711 RID: 38673
			NearTarget,
			// Token: 0x04009712 RID: 38674
			Position,
			// Token: 0x04009713 RID: 38675
			NearPosition,
			// Token: 0x04009714 RID: 38676
			WanderingHorde
		}

		// Token: 0x0200196D RID: 6509
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpawnUpdateTypes
		{
			// Token: 0x04009716 RID: 38678
			NeedSpawnEntries,
			// Token: 0x04009717 RID: 38679
			NeedPosition,
			// Token: 0x04009718 RID: 38680
			SpawnEntities
		}

		// Token: 0x0200196E RID: 6510
		[PublicizedFrom(EAccessModifier.Protected)]
		public class SpawnEntry
		{
			// Token: 0x04009719 RID: 38681
			public Entity Target;

			// Token: 0x0400971A RID: 38682
			public int EntityTypeID;
		}
	}
}
