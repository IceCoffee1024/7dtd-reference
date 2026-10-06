using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AD RID: 6573
	[Preserve]
	public class ActionFillArea : BaseAction
	{
		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600C9DA RID: 51674 RVA: 0x0049D339 File Offset: 0x0049B539
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600C9DB RID: 51675 RVA: 0x0049D348 File Offset: 0x0049B548
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

		// Token: 0x0600C9DC RID: 51676 RVA: 0x0049D39C File Offset: 0x0049B59C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			string[] array = this.fillTypeNames.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				ActionFillArea.FillTypes fillTypes = (ActionFillArea.FillTypes)Enum.Parse(typeof(ActionFillArea.FillTypes), array[i]);
				this.FillTypeList.Add(fillTypes);
			}
		}

		// Token: 0x0600C9DD RID: 51677 RVA: 0x0049D3F0 File Offset: 0x0049B5F0
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			switch (this.currentState)
			{
			case ActionFillArea.FillSafeZoneStates.HandleChunks:
			{
				this.FillType = this.FillTypeList.RandomObject<ActionFillArea.FillTypes>();
				if (base.Owner.TargetPosition != Vector3.zero)
				{
					this.claimPos = new Vector3i(base.Owner.TargetPosition);
				}
				else
				{
					if (!(base.Owner.Target.position != Vector3.zero))
					{
						return BaseAction.ActionCompleteStates.InCompleteRefund;
					}
					this.claimPos = new Vector3i(base.Owner.Target.position);
				}
				this.chunkList.Clear();
				int num = this.claimSize / 16 + 1;
				int num2 = this.claimSize / 16 + 1;
				for (int i = -num; i <= num; i++)
				{
					int num3 = this.claimPos.x + i * 16;
					for (int j = -num2; j <= num2; j++)
					{
						int num4 = this.claimPos.z + j * 16;
						Chunk chunk = (Chunk)world.GetChunkFromWorldPos(new Vector3i(num3, this.claimPos.y, num4));
						if (chunk != null && !this.chunkList.Contains(chunk))
						{
							this.chunkList.Add(chunk);
						}
					}
				}
				List<Entity> entitiesInBounds = world.GetEntitiesInBounds(null, new Bounds(this.claimPos, Vector3.one * (float)this.claimSize), true);
				for (int k = 0; k < entitiesInBounds.Count; k++)
				{
					Entity entity = entitiesInBounds[k];
					bool flag = entity is EntityPlayer;
					Vector3i vector3i = new Vector3i(Utils.Fastfloor(entity.position.x), Utils.Fastfloor(entity.position.y), Utils.Fastfloor(entity.position.z));
					if (flag)
					{
						EntityPlayer entityPlayer = entity as EntityPlayer;
						Vector3 vector = vector3i + new Vector3(0.5f, 0.5f, 0.5f);
						this.playerDictionary.Add(entityPlayer, vector);
					}
					this.blockedPositions.Add(vector3i);
					this.blockedPositions.Add(vector3i + Vector3i.up);
					if (flag)
					{
						this.blockedPositions.Add(vector3i + Vector3i.up * 2);
					}
					vector3i += Vector3i.right;
					this.blockedPositions.Add(vector3i);
					this.blockedPositions.Add(vector3i + Vector3i.up);
					if (flag)
					{
						this.blockedPositions.Add(vector3i + Vector3i.up * 2);
					}
				}
				this.currentState = ActionFillArea.FillSafeZoneStates.SetupChanges;
				break;
			}
			case ActionFillArea.FillSafeZoneStates.SetupChanges:
			{
				this.blockChanges.Clear();
				IChunk chunk2 = null;
				int num5 = this.halfClaimSize;
				BlockValue blockValue = Block.GetBlockValue(this.blockName, false);
				this.TeleportPlayers();
				FastTags<TagGroup.Global> fastTags = ((this.blockTags != null) ? FastTags<TagGroup.Global>.Parse(this.blockTags) : FastTags<TagGroup.Global>.none);
				FastTags<TagGroup.Global> fastTags2 = ((this.excludeBlockTags != null) ? FastTags<TagGroup.Global>.Parse(this.excludeBlockTags) : FastTags<TagGroup.Global>.none);
				switch (this.FillType)
				{
				case ActionFillArea.FillTypes.Cube:
				{
					for (int l = -num5; l <= num5; l++)
					{
						for (int m = -num5; m <= num5; m++)
						{
							for (int n = -num5; n <= num5; n++)
							{
								Vector3i vector3i2 = this.claimPos + new Vector3i(n, l, m);
								if (world.GetChunkFromWorldPos(vector3i2, ref chunk2))
								{
									int num6 = World.toBlockXZ(vector3i2.x);
									int num7 = World.toBlockY(vector3i2.y);
									int num8 = World.toBlockXZ(vector3i2.z);
									BlockValue blockNoDamage = chunk2.GetBlockNoDamage(num6, num7, num8);
									if ((blockNoDamage.isair || blockNoDamage.Block.Tags.Test_AnySet(fastTags) || this.replaceAll) && !blockNoDamage.Block.Tags.Test_AnySet(fastTags2) && world.GetTraderAreaAt(vector3i2) == null && !this.blockedPositions.Contains(vector3i2))
									{
										Chunk chunk3 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk3))
										{
											this.chunkList.Add(chunk3);
										}
										if (this.maxDensity)
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i2, blockValue, sbyte.MaxValue));
										}
										else
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i2, blockValue));
										}
									}
								}
							}
						}
					}
					break;
				}
				case ActionFillArea.FillTypes.Sphere:
				{
					for (int num9 = -num5; num9 <= num5; num9++)
					{
						for (int num10 = -num5; num10 <= num5; num10++)
						{
							for (int num11 = -num5; num11 <= num5; num11++)
							{
								Vector3i vector3i3 = this.claimPos + new Vector3i(num11, num9, num10);
								if (Vector3.SqrMagnitude(this.claimPos - vector3i3) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i3, ref chunk2))
								{
									int num12 = World.toBlockXZ(vector3i3.x);
									int num13 = World.toBlockY(vector3i3.y);
									int num14 = World.toBlockXZ(vector3i3.z);
									BlockValue blockNoDamage2 = chunk2.GetBlockNoDamage(num12, num13, num14);
									if ((blockNoDamage2.isair || blockNoDamage2.Block.Tags.Test_AnySet(fastTags) || this.replaceAll) && !blockNoDamage2.Block.Tags.Test_AnySet(fastTags2) && world.GetTraderAreaAt(vector3i3) == null && !this.blockedPositions.Contains(vector3i3))
									{
										Chunk chunk4 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk4))
										{
											this.chunkList.Add(chunk4);
										}
										if (this.maxDensity)
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i3, blockValue, sbyte.MaxValue));
										}
										else
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i3, blockValue));
										}
									}
								}
							}
						}
					}
					break;
				}
				case ActionFillArea.FillTypes.Cylinder:
				{
					for (int num15 = -num5; num15 <= num5; num15++)
					{
						for (int num16 = -num5; num16 <= num5; num16++)
						{
							for (int num17 = -num5; num17 <= num5; num17++)
							{
								Vector3i vector3i4 = this.claimPos + new Vector3i(num17, num15, num16);
								if (Vector3.SqrMagnitude(new Vector3i(this.claimPos.x, vector3i4.y, this.claimPos.z) - vector3i4) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i4, ref chunk2))
								{
									int num18 = World.toBlockXZ(vector3i4.x);
									int num19 = World.toBlockY(vector3i4.y);
									int num20 = World.toBlockXZ(vector3i4.z);
									BlockValue blockNoDamage3 = chunk2.GetBlockNoDamage(num18, num19, num20);
									if ((blockNoDamage3.isair || blockNoDamage3.Block.Tags.Test_AnySet(fastTags) || this.replaceAll) && !blockNoDamage3.Block.Tags.Test_AnySet(fastTags2) && world.GetTraderAreaAt(vector3i4) == null && !this.blockedPositions.Contains(vector3i4))
									{
										Chunk chunk5 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk5))
										{
											this.chunkList.Add(chunk5);
										}
										if (this.maxDensity)
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue, sbyte.MaxValue));
										}
										else
										{
											this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
										}
									}
								}
							}
						}
					}
					break;
				}
				}
				this.currentState = ActionFillArea.FillSafeZoneStates.AddSigns;
				break;
			}
			case ActionFillArea.FillSafeZoneStates.AddSigns:
				if (this.newName == "")
				{
					this.currentState = ActionFillArea.FillSafeZoneStates.Action;
				}
				else
				{
					int num21 = this.halfClaimSize + 1;
					BlockValue blockValue2 = Block.GetBlockValue("playerSignWood1x3", false);
					if (this.signPositions == null)
					{
						this.signPositions = new List<Vector3i>();
					}
					Vector3i vector3i5 = this.claimPos + new Vector3i(0, 0, num21);
					vector3i5.y = (int)(world.GetHeight(vector3i5.x, vector3i5.z) + 1);
					blockValue2.rotation = 0;
					if (world.GetTraderAreaAt(vector3i5) == null)
					{
						this.signPositions.Add(vector3i5);
						this.blockChanges.Add(new BlockChangeInfo(vector3i5, blockValue2));
					}
					vector3i5 = this.claimPos + new Vector3i(0, 0, -num21);
					vector3i5.y = (int)(world.GetHeight(vector3i5.x, vector3i5.z) + 1);
					blockValue2.rotation = 2;
					if (world.GetTraderAreaAt(vector3i5) == null)
					{
						this.signPositions.Add(vector3i5);
						this.blockChanges.Add(new BlockChangeInfo(vector3i5, blockValue2));
					}
					vector3i5 = this.claimPos + new Vector3i(num21, 0, 0);
					vector3i5.y = (int)(world.GetHeight(vector3i5.x, vector3i5.z) + 1);
					blockValue2.rotation = 1;
					if (world.GetTraderAreaAt(vector3i5) == null)
					{
						this.signPositions.Add(vector3i5);
						this.blockChanges.Add(new BlockChangeInfo(vector3i5, blockValue2));
					}
					vector3i5 = this.claimPos + new Vector3i(-num21, 0, 0);
					vector3i5.y = (int)(world.GetHeight(vector3i5.x, vector3i5.z) + 1);
					blockValue2.rotation = 3;
					if (world.GetTraderAreaAt(vector3i5) == null)
					{
						this.signPositions.Add(vector3i5);
						this.blockChanges.Add(new BlockChangeInfo(vector3i5, blockValue2));
					}
					this.currentState = ActionFillArea.FillSafeZoneStates.Action;
				}
				break;
			case ActionFillArea.FillSafeZoneStates.Action:
				GameManager.Instance.ChangeBlocks(null, this.blockChanges);
				this.currentState = ActionFillArea.FillSafeZoneStates.ResetChunks;
				break;
			case ActionFillArea.FillSafeZoneStates.ResetChunks:
			{
				for (int num22 = 0; num22 < this.chunkList.Count; num22++)
				{
					Chunk chunk6 = this.chunkList[num22];
					long num23 = WorldChunkCache.MakeChunkKey(chunk6.X, chunk6.Z);
					this.chunkHash.Add(num23);
				}
				if (base.Owner.Target != null && this.newName != "")
				{
					PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
					for (int num24 = 0; num24 < this.signPositions.Count; num24++)
					{
						TEFeatureSignable tefeatureSignable;
						if (world.GetTileEntity(this.signPositions[num24]).TryGetSelfOrFeature<TEFeatureSignable>(out tefeatureSignable))
						{
							tefeatureSignable.SetText(this.ModifiedName, true, (playerDataFromEntityID != null) ? playerDataFromEntityID.PrimaryId : null);
						}
					}
				}
				world.m_ChunkManager.ResendChunksToClients(this.chunkHash);
				this.TeleportPlayers();
				return BaseAction.ActionCompleteStates.Complete;
			}
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C9DE RID: 51678 RVA: 0x0049DF1C File Offset: 0x0049C11C
		[PublicizedFrom(EAccessModifier.Private)]
		public void TeleportPlayers()
		{
			foreach (EntityPlayer entityPlayer in this.playerDictionary.Keys)
			{
				GameManager.Instance.StartCoroutine(base.TeleportEntity(entityPlayer, this.playerDictionary[entityPlayer], 0f));
			}
		}

		// Token: 0x0600C9DF RID: 51679 RVA: 0x0049DF90 File Offset: 0x0049C190
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionFillArea.PropFillType, ref this.fillTypeNames);
			properties.ParseBool(ActionFillArea.PropDestroyClaim, ref this.destroyClaim);
			properties.ParseString(ActionFillArea.PropBlock, ref this.blockName);
			properties.ParseString(ActionFillArea.PropNewName, ref this.newName);
			properties.ParseString(ActionFillArea.PropBlockTags, ref this.blockTags);
			properties.ParseString(ActionFillArea.PropExcludeBlockTags, ref this.excludeBlockTags);
			properties.ParseBool(ActionFillArea.PropReplaceAll, ref this.replaceAll);
			properties.ParseBool(ActionFillArea.PropMaxDensity, ref this.maxDensity);
		}

		// Token: 0x0600C9E0 RID: 51680 RVA: 0x0049E02C File Offset: 0x0049C22C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFillArea
			{
				fillTypeNames = this.fillTypeNames,
				FillTypeList = this.FillTypeList,
				blockName = this.blockName,
				destroyClaim = this.destroyClaim,
				newName = this.newName,
				blockTags = this.blockTags,
				excludeBlockTags = this.excludeBlockTags,
				replaceAll = this.replaceAll,
				maxDensity = this.maxDensity
			};
		}

		// Token: 0x04009846 RID: 38982
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionFillArea.FillSafeZoneStates currentState;

		// Token: 0x04009847 RID: 38983
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionFillArea.FillTypes FillType = ActionFillArea.FillTypes.Sphere;

		// Token: 0x04009848 RID: 38984
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fillTypeNames = "Sphere";

		// Token: 0x04009849 RID: 38985
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionFillArea.FillTypes> FillTypeList = new List<ActionFillArea.FillTypes>();

		// Token: 0x0400984A RID: 38986
		[PublicizedFrom(EAccessModifier.Private)]
		public bool destroyClaim;

		// Token: 0x0400984B RID: 38987
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x0400984C RID: 38988
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags;

		// Token: 0x0400984D RID: 38989
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeBlockTags;

		// Token: 0x0400984E RID: 38990
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool replaceAll;

		// Token: 0x0400984F RID: 38991
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool maxDensity;

		// Token: 0x04009850 RID: 38992
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFillType = "fill_type";

		// Token: 0x04009851 RID: 38993
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlock = "block";

		// Token: 0x04009852 RID: 38994
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDestroyClaim = "destroy_claim";

		// Token: 0x04009853 RID: 38995
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009854 RID: 38996
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeBlockTags = "exclude_block_tags";

		// Token: 0x04009855 RID: 38997
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceAll = "replace_all";

		// Token: 0x04009856 RID: 38998
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x04009857 RID: 38999
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDensity = "max_density";

		// Token: 0x04009858 RID: 39000
		[PublicizedFrom(EAccessModifier.Private)]
		public string blockName = "terrDirtTwitch";

		// Token: 0x04009859 RID: 39001
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i claimPos = Vector3i.zero;

		// Token: 0x0400985A RID: 39002
		[PublicizedFrom(EAccessModifier.Private)]
		public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

		// Token: 0x0400985B RID: 39003
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Chunk> chunkList = new List<Chunk>();

		// Token: 0x0400985C RID: 39004
		[PublicizedFrom(EAccessModifier.Private)]
		public int claimSize = 10;

		// Token: 0x0400985D RID: 39005
		[PublicizedFrom(EAccessModifier.Private)]
		public int halfClaimSize = 5;

		// Token: 0x0400985E RID: 39006
		[PublicizedFrom(EAccessModifier.Private)]
		public int distanceSq = 25;

		// Token: 0x0400985F RID: 39007
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Vector3i> blockedPositions = new HashSet<Vector3i>();

		// Token: 0x04009860 RID: 39008
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetLong chunkHash = new HashSetLong();

		// Token: 0x04009861 RID: 39009
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EntityPlayer, Vector3> playerDictionary = new Dictionary<EntityPlayer, Vector3>();

		// Token: 0x04009862 RID: 39010
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3i> signPositions;

		// Token: 0x020019AE RID: 6574
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FillSafeZoneStates
		{
			// Token: 0x04009864 RID: 39012
			HandleChunks,
			// Token: 0x04009865 RID: 39013
			SetupChanges,
			// Token: 0x04009866 RID: 39014
			AddSigns,
			// Token: 0x04009867 RID: 39015
			Action,
			// Token: 0x04009868 RID: 39016
			ResetChunks
		}

		// Token: 0x020019AF RID: 6575
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FillTypes
		{
			// Token: 0x0400986A RID: 39018
			Cube,
			// Token: 0x0400986B RID: 39019
			Sphere,
			// Token: 0x0400986C RID: 39020
			Cylinder
		}
	}
}
