using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001951 RID: 6481
	[Preserve]
	public class ActionDestroySafeZone : BaseAction
	{
		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x0600C6D1 RID: 50897 RVA: 0x0049135D File Offset: 0x0048F55D
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600C6D2 RID: 50898 RVA: 0x0049136C File Offset: 0x0048F56C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string ParseTextElement(string element)
		{
			if (!(element == "viewer"))
			{
				return element;
			}
			if (base.Owner.ExtraData.Length <= 12)
			{
				return base.Owner.ExtraData;
			}
			return base.Owner.ExtraData.Insert(12, "\n");
		}

		// Token: 0x0600C6D3 RID: 50899 RVA: 0x004913C0 File Offset: 0x0048F5C0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			string[] array = this.destroyTypeNames.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				ActionDestroySafeZone.DestructionTypes destructionTypes = (ActionDestroySafeZone.DestructionTypes)Enum.Parse(typeof(ActionDestroySafeZone.DestructionTypes), array[i]);
				this.DestroyTypeList.Add(destructionTypes);
			}
		}

		// Token: 0x0600C6D4 RID: 50900 RVA: 0x00491414 File Offset: 0x0048F614
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			switch (this.currentState)
			{
			case ActionDestroySafeZone.DestroySafeZoneStates.FindClaim:
			{
				if (this.DestroyTypeList.Count == 0)
				{
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				this.DestructionType = this.DestroyTypeList.RandomObject<ActionDestroySafeZone.DestructionTypes>();
				if (base.Owner.TargetPosition == Vector3.zero)
				{
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				this.claimSize = Mathf.Min(GameStats.GetInt(EnumGameStats.LandClaimSize), 41);
				this.halfClaimSize = (this.claimSize - 1) / 2;
				this.distanceSq = this.halfClaimSize * this.halfClaimSize;
				this.claimPos = new Vector3i(base.Owner.TargetPosition);
				TEFeatureLandClaim selfOrFeature = world.GetTileEntity(World.toBlock(this.claimPos)).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature == null || !selfOrFeature.IsPrimary())
				{
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				world.SetBlockRPC(this.claimPos, BlockValue.Air);
				this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.HandleChunks;
				break;
			}
			case ActionDestroySafeZone.DestroySafeZoneStates.HandleChunks:
			{
				this.chunkList.Clear();
				int num = GameStats.GetInt(EnumGameStats.LandClaimSize) - 1;
				int num2 = GameStats.GetInt(EnumGameStats.LandClaimDeadZone) + num;
				int num3 = num2 / 16 + 1;
				int num4 = num2 / 16 + 1;
				for (int i = -num3; i <= num3; i++)
				{
					int num5 = this.claimPos.x + i * 16;
					for (int j = -num4; j <= num4; j++)
					{
						int num6 = this.claimPos.z + j * 16;
						Chunk chunk = (Chunk)world.GetChunkFromWorldPos(new Vector3i(num5, this.claimPos.y, num6));
						if (chunk != null && !this.chunkList.Contains(chunk))
						{
							this.chunkList.Add(chunk);
							chunk.StopStabilityCalculation = true;
						}
					}
				}
				List<Entity> entitiesInBounds = world.GetEntitiesInBounds(null, new Bounds(this.claimPos, Vector3.one * (float)num), true);
				for (int k = 0; k < entitiesInBounds.Count; k++)
				{
					EntityAlive entityAlive = entitiesInBounds[k] as EntityAlive;
					if (entityAlive != null)
					{
						entityAlive.Buffs.AddBuff("buffTwitchDontBreakLeg", -1, true, false, -1f);
					}
				}
				this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.SetupChanges;
				break;
			}
			case ActionDestroySafeZone.DestroySafeZoneStates.SetupChanges:
			{
				this.blockChanges.Clear();
				IChunk chunk2 = null;
				int num7 = this.halfClaimSize;
				switch (this.DestructionType)
				{
				case ActionDestroySafeZone.DestructionTypes.Cube:
				{
					for (int l = -num7; l <= num7; l++)
					{
						for (int m = -num7; m <= num7; m++)
						{
							for (int n = -num7; n <= num7; n++)
							{
								Vector3i vector3i = this.claimPos + new Vector3i(n, l, m);
								if (world.GetChunkFromWorldPos(vector3i, ref chunk2))
								{
									int num8 = World.toBlockXZ(vector3i.x);
									int num9 = World.toBlockY(vector3i.y);
									int num10 = World.toBlockXZ(vector3i.z);
									if (!chunk2.IsAir(num8, num9, num10) && chunk2.GetBlock(num8, num9, num10).Block.blockMaterial.CanDestroy)
									{
										Chunk chunk3 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk3))
										{
											this.chunkList.Add(chunk3);
											chunk3.StopStabilityCalculation = true;
										}
										ITileEntityLootable tileEntityLootable;
										if (chunk3.GetTileEntity(World.toBlock(vector3i)).TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
										{
											tileEntityLootable.SetEmpty();
											tileEntityLootable.SetModified();
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i, BlockValue.Air));
									}
								}
							}
						}
					}
					break;
				}
				case ActionDestroySafeZone.DestructionTypes.Sphere:
				{
					for (int num11 = -num7; num11 <= num7; num11++)
					{
						for (int num12 = -num7; num12 <= num7; num12++)
						{
							for (int num13 = -num7; num13 <= num7; num13++)
							{
								Vector3i vector3i2 = this.claimPos + new Vector3i(num13, num11, num12);
								if (Vector3.SqrMagnitude(this.claimPos - vector3i2) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i2, ref chunk2))
								{
									int num14 = World.toBlockXZ(vector3i2.x);
									int num15 = World.toBlockY(vector3i2.y);
									int num16 = World.toBlockXZ(vector3i2.z);
									if (!chunk2.IsAir(num14, num15, num16) && chunk2.GetBlock(num14, num15, num16).Block.blockMaterial.CanDestroy)
									{
										Chunk chunk4 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk4))
										{
											this.chunkList.Add(chunk4);
											chunk4.StopStabilityCalculation = true;
										}
										ITileEntityLootable tileEntityLootable2;
										if (chunk4.GetTileEntity(World.toBlock(vector3i2)).TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable2))
										{
											tileEntityLootable2.SetEmpty();
											tileEntityLootable2.SetModified();
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i2, BlockValue.Air));
									}
								}
							}
						}
					}
					break;
				}
				case ActionDestroySafeZone.DestructionTypes.Cylinder:
				{
					for (int num17 = -num7; num17 <= num7; num17++)
					{
						for (int num18 = -num7; num18 <= num7; num18++)
						{
							for (int num19 = -num7; num19 <= num7; num19++)
							{
								Vector3i vector3i3 = this.claimPos + new Vector3i(num19, num17, num18);
								if (Vector3.SqrMagnitude(new Vector3i(this.claimPos.x, vector3i3.y, this.claimPos.z) - vector3i3) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i3, ref chunk2))
								{
									int num20 = World.toBlockXZ(vector3i3.x);
									int num21 = World.toBlockY(vector3i3.y);
									int num22 = World.toBlockXZ(vector3i3.z);
									if (!chunk2.IsAir(num20, num21, num22) && chunk2.GetBlock(num20, num21, num22).Block.blockMaterial.CanDestroy)
									{
										Chunk chunk5 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk5))
										{
											this.chunkList.Add(chunk5);
											chunk5.StopStabilityCalculation = true;
										}
										ITileEntityLootable tileEntityLootable3;
										if (chunk5.GetTileEntity(World.toBlock(vector3i3)).TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable3))
										{
											tileEntityLootable3.SetEmpty();
											tileEntityLootable3.SetModified();
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i3, BlockValue.Air));
									}
								}
							}
						}
					}
					break;
				}
				case ActionDestroySafeZone.DestructionTypes.LandClaimOnly:
					this.blockChanges.Add(new BlockChangeInfo(this.claimPos, BlockValue.Air));
					break;
				}
				if (this.DestructionType == ActionDestroySafeZone.DestructionTypes.LandClaimOnly)
				{
					this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.Action;
				}
				else
				{
					this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.AddSigns;
				}
				break;
			}
			case ActionDestroySafeZone.DestroySafeZoneStates.AddSigns:
			{
				int num23 = this.halfClaimSize + 1;
				BlockValue blockValue = Block.GetBlockValue("playerSignWood1x3", false);
				if (this.signPositions == null)
				{
					this.signPositions = new List<Vector3i>();
				}
				Vector3i vector3i4 = this.claimPos + new Vector3i(0, 0, num23);
				vector3i4.y = (int)(world.GetHeight(vector3i4.x, vector3i4.z) + 1);
				blockValue.rotation = 0;
				this.signPositions.Add(vector3i4);
				this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
				vector3i4 = this.claimPos + new Vector3i(0, 0, -num23);
				vector3i4.y = (int)(world.GetHeight(vector3i4.x, vector3i4.z) + 1);
				blockValue.rotation = 2;
				this.signPositions.Add(vector3i4);
				this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
				vector3i4 = this.claimPos + new Vector3i(num23, 0, 0);
				vector3i4.y = (int)(world.GetHeight(vector3i4.x, vector3i4.z) + 1);
				blockValue.rotation = 1;
				this.signPositions.Add(vector3i4);
				this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
				vector3i4 = this.claimPos + new Vector3i(-num23, 0, 0);
				vector3i4.y = (int)(world.GetHeight(vector3i4.x, vector3i4.z) + 1);
				blockValue.rotation = 3;
				this.signPositions.Add(vector3i4);
				this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
				this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.Action;
				break;
			}
			case ActionDestroySafeZone.DestroySafeZoneStates.Action:
				GameManager.Instance.ChangeBlocks(null, this.blockChanges);
				this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.ResetChunks;
				break;
			case ActionDestroySafeZone.DestroySafeZoneStates.ResetChunks:
			{
				for (int num24 = 0; num24 < this.chunkList.Count; num24++)
				{
					Chunk chunk6 = this.chunkList[num24];
					long num25 = WorldChunkCache.MakeChunkKey(chunk6.X, chunk6.Z);
					chunk6.StopStabilityCalculation = false;
					this.chunkHash.Add(num25);
				}
				if (this.signPositions != null && base.Owner.Target != null)
				{
					PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
					for (int num26 = 0; num26 < this.signPositions.Count; num26++)
					{
						TEFeatureSignable tefeatureSignable;
						if (world.GetTileEntity(this.signPositions[num26]).TryGetSelfOrFeature<TEFeatureSignable>(out tefeatureSignable))
						{
							tefeatureSignable.SetText(this.ModifiedName, true, (playerDataFromEntityID != null) ? playerDataFromEntityID.PrimaryId : null);
						}
					}
				}
				world.m_ChunkManager.ResendChunksToClients(this.chunkHash);
				this.currentState = ActionDestroySafeZone.DestroySafeZoneStates.ClientResets;
				break;
			}
			case ActionDestroySafeZone.DestroySafeZoneStates.ClientResets:
				if (this.delay <= 0f)
				{
					world.m_ChunkManager.ResendChunksToClients(this.chunkHash);
					return BaseAction.ActionCompleteStates.Complete;
				}
				this.delay -= Time.deltaTime;
				break;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C6D5 RID: 50901 RVA: 0x00491E14 File Offset: 0x00490014
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDestroySafeZone.PropDestructionType, ref this.destroyTypeNames);
			properties.ParseString(ActionDestroySafeZone.PropNewName, ref this.newName);
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x00491E3F File Offset: 0x0049003F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDestroySafeZone
			{
				destroyTypeNames = this.destroyTypeNames,
				DestroyTypeList = this.DestroyTypeList,
				newName = this.newName
			};
		}

		// Token: 0x0400960E RID: 38414
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionDestroySafeZone.DestroySafeZoneStates currentState;

		// Token: 0x0400960F RID: 38415
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionDestroySafeZone.DestructionTypes DestructionType = ActionDestroySafeZone.DestructionTypes.Sphere;

		// Token: 0x04009610 RID: 38416
		[PublicizedFrom(EAccessModifier.Protected)]
		public string destroyTypeNames = "Sphere";

		// Token: 0x04009611 RID: 38417
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x04009612 RID: 38418
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionDestroySafeZone.DestructionTypes> DestroyTypeList = new List<ActionDestroySafeZone.DestructionTypes>();

		// Token: 0x04009613 RID: 38419
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDestructionType = "destruction_type";

		// Token: 0x04009614 RID: 38420
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x04009615 RID: 38421
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i claimPos = Vector3i.zero;

		// Token: 0x04009616 RID: 38422
		[PublicizedFrom(EAccessModifier.Private)]
		public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

		// Token: 0x04009617 RID: 38423
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Chunk> chunkList = new List<Chunk>();

		// Token: 0x04009618 RID: 38424
		[PublicizedFrom(EAccessModifier.Private)]
		public int claimSize;

		// Token: 0x04009619 RID: 38425
		[PublicizedFrom(EAccessModifier.Private)]
		public int halfClaimSize;

		// Token: 0x0400961A RID: 38426
		[PublicizedFrom(EAccessModifier.Private)]
		public int distanceSq;

		// Token: 0x0400961B RID: 38427
		[PublicizedFrom(EAccessModifier.Private)]
		public float delay = 1f;

		// Token: 0x0400961C RID: 38428
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetLong chunkHash = new HashSetLong();

		// Token: 0x0400961D RID: 38429
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3i> signPositions;

		// Token: 0x02001952 RID: 6482
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DestroySafeZoneStates
		{
			// Token: 0x0400961F RID: 38431
			FindClaim,
			// Token: 0x04009620 RID: 38432
			HandleChunks,
			// Token: 0x04009621 RID: 38433
			SetupChanges,
			// Token: 0x04009622 RID: 38434
			AddSigns,
			// Token: 0x04009623 RID: 38435
			Action,
			// Token: 0x04009624 RID: 38436
			ResetChunks,
			// Token: 0x04009625 RID: 38437
			ClientResets
		}

		// Token: 0x02001953 RID: 6483
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DestructionTypes
		{
			// Token: 0x04009627 RID: 38439
			Cube,
			// Token: 0x04009628 RID: 38440
			Sphere,
			// Token: 0x04009629 RID: 38441
			Cylinder,
			// Token: 0x0400962A RID: 38442
			LandClaimOnly
		}
	}
}
