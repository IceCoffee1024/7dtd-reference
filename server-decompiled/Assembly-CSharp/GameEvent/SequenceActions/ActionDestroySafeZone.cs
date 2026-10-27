using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D4 RID: 6612
	[Preserve]
	public class ActionDestroySafeZone : BaseAction
	{
		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x0600CAC4 RID: 51908 RVA: 0x0049F685 File Offset: 0x0049D885
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600CAC5 RID: 51909 RVA: 0x0049F694 File Offset: 0x0049D894
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

		// Token: 0x0600CAC6 RID: 51910 RVA: 0x0049F6E8 File Offset: 0x0049D8E8
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

		// Token: 0x0600CAC7 RID: 51911 RVA: 0x0049F73C File Offset: 0x0049D93C
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
				TEFeatureLandClaim selfOrFeature = world.GetTileEntity(this.claimPos).GetSelfOrFeature<TEFeatureLandClaim>();
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
										TEFeatureStorage tefeatureStorage;
										if (chunk3.GetTileEntity(World.toBlock(vector3i)).TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
										{
											tefeatureStorage.SetEmpty();
											tefeatureStorage.SetModified();
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
										TEFeatureStorage tefeatureStorage2;
										if (chunk4.GetTileEntity(World.toBlock(vector3i2)).TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage2))
										{
											tefeatureStorage2.SetEmpty();
											tefeatureStorage2.SetModified();
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
										TEFeatureStorage tefeatureStorage3;
										if (chunk5.GetTileEntity(World.toBlock(vector3i3)).TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage3))
										{
											tefeatureStorage3.SetEmpty();
											tefeatureStorage3.SetModified();
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

		// Token: 0x0600CAC8 RID: 51912 RVA: 0x004A0137 File Offset: 0x0049E337
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDestroySafeZone.PropDestructionType, ref this.destroyTypeNames);
			properties.ParseString(ActionDestroySafeZone.PropNewName, ref this.newName);
		}

		// Token: 0x0600CAC9 RID: 51913 RVA: 0x004A0162 File Offset: 0x0049E362
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

		// Token: 0x040098E6 RID: 39142
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionDestroySafeZone.DestroySafeZoneStates currentState;

		// Token: 0x040098E7 RID: 39143
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionDestroySafeZone.DestructionTypes DestructionType = ActionDestroySafeZone.DestructionTypes.Sphere;

		// Token: 0x040098E8 RID: 39144
		[PublicizedFrom(EAccessModifier.Protected)]
		public string destroyTypeNames = "Sphere";

		// Token: 0x040098E9 RID: 39145
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x040098EA RID: 39146
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionDestroySafeZone.DestructionTypes> DestroyTypeList = new List<ActionDestroySafeZone.DestructionTypes>();

		// Token: 0x040098EB RID: 39147
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDestructionType = "destruction_type";

		// Token: 0x040098EC RID: 39148
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x040098ED RID: 39149
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i claimPos = Vector3i.zero;

		// Token: 0x040098EE RID: 39150
		[PublicizedFrom(EAccessModifier.Private)]
		public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

		// Token: 0x040098EF RID: 39151
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Chunk> chunkList = new List<Chunk>();

		// Token: 0x040098F0 RID: 39152
		[PublicizedFrom(EAccessModifier.Private)]
		public int claimSize;

		// Token: 0x040098F1 RID: 39153
		[PublicizedFrom(EAccessModifier.Private)]
		public int halfClaimSize;

		// Token: 0x040098F2 RID: 39154
		[PublicizedFrom(EAccessModifier.Private)]
		public int distanceSq;

		// Token: 0x040098F3 RID: 39155
		[PublicizedFrom(EAccessModifier.Private)]
		public float delay = 1f;

		// Token: 0x040098F4 RID: 39156
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetLong chunkHash = new HashSetLong();

		// Token: 0x040098F5 RID: 39157
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3i> signPositions;

		// Token: 0x020019D5 RID: 6613
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DestroySafeZoneStates
		{
			// Token: 0x040098F7 RID: 39159
			FindClaim,
			// Token: 0x040098F8 RID: 39160
			HandleChunks,
			// Token: 0x040098F9 RID: 39161
			SetupChanges,
			// Token: 0x040098FA RID: 39162
			AddSigns,
			// Token: 0x040098FB RID: 39163
			Action,
			// Token: 0x040098FC RID: 39164
			ResetChunks,
			// Token: 0x040098FD RID: 39165
			ClientResets
		}

		// Token: 0x020019D6 RID: 6614
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DestructionTypes
		{
			// Token: 0x040098FF RID: 39167
			Cube,
			// Token: 0x04009900 RID: 39168
			Sphere,
			// Token: 0x04009901 RID: 39169
			Cylinder,
			// Token: 0x04009902 RID: 39170
			LandClaimOnly
		}
	}
}
