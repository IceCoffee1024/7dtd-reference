using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001984 RID: 6532
	[Preserve]
	public class ActionFillSafeZone : BaseAction
	{
		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x0600C88B RID: 51339 RVA: 0x00499CE5 File Offset: 0x00497EE5
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600C88C RID: 51340 RVA: 0x00499CF4 File Offset: 0x00497EF4
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

		// Token: 0x0600C88D RID: 51341 RVA: 0x00499D48 File Offset: 0x00497F48
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			base.OnInit();
			string[] array = this.fillTypeNames.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				ActionFillSafeZone.FillTypes fillTypes = (ActionFillSafeZone.FillTypes)Enum.Parse(typeof(ActionFillSafeZone.FillTypes), array[i]);
				this.FillTypeList.Add(fillTypes);
			}
		}

		// Token: 0x0600C88E RID: 51342 RVA: 0x00499D9C File Offset: 0x00497F9C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			switch (this.currentState)
			{
			case ActionFillSafeZone.FillSafeZoneStates.FindClaim:
			{
				if (this.FillTypeList.Count == 0)
				{
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				this.FillType = this.FillTypeList.RandomObject<ActionFillSafeZone.FillTypes>();
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
				if (this.destroyClaim)
				{
					world.SetBlockRPC(this.claimPos, BlockValue.Air);
				}
				this.currentState = ActionFillSafeZone.FillSafeZoneStates.HandleChunks;
				break;
			}
			case ActionFillSafeZone.FillSafeZoneStates.HandleChunks:
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
						}
					}
				}
				List<Entity> entitiesInBounds = world.GetEntitiesInBounds(null, new Bounds(this.claimPos, Vector3.one * (float)num), true);
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
				this.currentState = ActionFillSafeZone.FillSafeZoneStates.SetupChanges;
				break;
			}
			case ActionFillSafeZone.FillSafeZoneStates.SetupChanges:
			{
				this.blockChanges.Clear();
				IChunk chunk2 = null;
				int num7 = this.halfClaimSize;
				BlockValue blockValue = Block.GetBlockValue(this.blockName, false);
				this.TeleportPlayers();
				FastTags<TagGroup.Global> fastTags = ((this.blockTags != null) ? FastTags<TagGroup.Global>.Parse(this.blockTags) : FastTags<TagGroup.Global>.none);
				switch (this.FillType)
				{
				case ActionFillSafeZone.FillTypes.Cube:
				{
					for (int l = -num7; l <= num7; l++)
					{
						for (int m = -num7; m <= num7; m++)
						{
							for (int n = -num7; n <= num7; n++)
							{
								Vector3i vector3i2 = this.claimPos + new Vector3i(n, l, m);
								if (world.GetChunkFromWorldPos(vector3i2, ref chunk2))
								{
									int num8 = World.toBlockXZ(vector3i2.x);
									int num9 = World.toBlockY(vector3i2.y);
									int num10 = World.toBlockXZ(vector3i2.z);
									BlockValue blockNoDamage = chunk2.GetBlockNoDamage(num8, num9, num10);
									if ((blockNoDamage.isair || blockNoDamage.Block.Tags.Test_AnySet(fastTags)) && !this.blockedPositions.Contains(vector3i2))
									{
										Chunk chunk3 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk3))
										{
											this.chunkList.Add(chunk3);
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i2, blockValue));
									}
								}
							}
						}
					}
					break;
				}
				case ActionFillSafeZone.FillTypes.Sphere:
				{
					for (int num11 = -num7; num11 <= num7; num11++)
					{
						for (int num12 = -num7; num12 <= num7; num12++)
						{
							for (int num13 = -num7; num13 <= num7; num13++)
							{
								Vector3i vector3i3 = this.claimPos + new Vector3i(num13, num11, num12);
								if (Vector3.SqrMagnitude(this.claimPos - vector3i3) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i3, ref chunk2))
								{
									int num14 = World.toBlockXZ(vector3i3.x);
									int num15 = World.toBlockY(vector3i3.y);
									int num16 = World.toBlockXZ(vector3i3.z);
									BlockValue blockNoDamage2 = chunk2.GetBlockNoDamage(num14, num15, num16);
									if ((blockNoDamage2.isair || blockNoDamage2.Block.Tags.Test_AnySet(fastTags)) && !this.blockedPositions.Contains(vector3i3))
									{
										Chunk chunk4 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk4))
										{
											this.chunkList.Add(chunk4);
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i3, blockValue));
									}
								}
							}
						}
					}
					break;
				}
				case ActionFillSafeZone.FillTypes.Cylinder:
				{
					for (int num17 = -num7; num17 <= num7; num17++)
					{
						for (int num18 = -num7; num18 <= num7; num18++)
						{
							for (int num19 = -num7; num19 <= num7; num19++)
							{
								Vector3i vector3i4 = this.claimPos + new Vector3i(num19, num17, num18);
								if (Vector3.SqrMagnitude(new Vector3i(this.claimPos.x, vector3i4.y, this.claimPos.z) - vector3i4) < (float)this.distanceSq && world.GetChunkFromWorldPos(vector3i4, ref chunk2))
								{
									int num20 = World.toBlockXZ(vector3i4.x);
									int num21 = World.toBlockY(vector3i4.y);
									int num22 = World.toBlockXZ(vector3i4.z);
									BlockValue blockNoDamage3 = chunk2.GetBlockNoDamage(num20, num21, num22);
									if ((blockNoDamage3.isair || blockNoDamage3.Block.Tags.Test_AnySet(fastTags)) && !this.blockedPositions.Contains(vector3i4))
									{
										Chunk chunk5 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk5))
										{
											this.chunkList.Add(chunk5);
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i4, blockValue));
									}
								}
							}
						}
					}
					break;
				}
				case ActionFillSafeZone.FillTypes.Pyramid:
				{
					int num23 = 0;
					for (int num24 = -num7; num24 <= num7; num24++)
					{
						int num25 = num7 - num23 / 2;
						for (int num26 = -num25; num26 <= num25; num26++)
						{
							for (int num27 = -num25; num27 <= num25; num27++)
							{
								Vector3i vector3i5 = this.claimPos + new Vector3i(num27, num24, num26);
								if (world.GetChunkFromWorldPos(vector3i5, ref chunk2))
								{
									int num28 = World.toBlockXZ(vector3i5.x);
									int num29 = World.toBlockY(vector3i5.y);
									int num30 = World.toBlockXZ(vector3i5.z);
									BlockValue blockNoDamage4 = chunk2.GetBlockNoDamage(num28, num29, num30);
									if ((blockNoDamage4.isair || blockNoDamage4.Block.Tags.Test_AnySet(fastTags)) && !this.blockedPositions.Contains(vector3i5))
									{
										Chunk chunk6 = (Chunk)chunk2;
										if (!this.chunkList.Contains(chunk6))
										{
											this.chunkList.Add(chunk6);
										}
										this.blockChanges.Add(new BlockChangeInfo(vector3i5, blockValue));
									}
								}
							}
						}
						num23++;
					}
					break;
				}
				}
				this.currentState = ActionFillSafeZone.FillSafeZoneStates.AddSigns;
				break;
			}
			case ActionFillSafeZone.FillSafeZoneStates.AddSigns:
			{
				int num31 = this.halfClaimSize + 1;
				BlockValue blockValue2 = Block.GetBlockValue("playerSignWood1x3", false);
				if (this.signPositions == null)
				{
					this.signPositions = new List<Vector3i>();
				}
				Vector3i vector3i6 = this.claimPos + new Vector3i(0, 0, num31);
				vector3i6.y = (int)(world.GetHeight(vector3i6.x, vector3i6.z) + 1);
				blockValue2.rotation = 0;
				this.signPositions.Add(vector3i6);
				this.blockChanges.Add(new BlockChangeInfo(vector3i6, blockValue2));
				vector3i6 = this.claimPos + new Vector3i(0, 0, -num31);
				vector3i6.y = (int)(world.GetHeight(vector3i6.x, vector3i6.z) + 1);
				blockValue2.rotation = 2;
				this.signPositions.Add(vector3i6);
				this.blockChanges.Add(new BlockChangeInfo(vector3i6, blockValue2));
				vector3i6 = this.claimPos + new Vector3i(num31, 0, 0);
				vector3i6.y = (int)(world.GetHeight(vector3i6.x, vector3i6.z) + 1);
				blockValue2.rotation = 1;
				this.signPositions.Add(vector3i6);
				this.blockChanges.Add(new BlockChangeInfo(vector3i6, blockValue2));
				vector3i6 = this.claimPos + new Vector3i(-num31, 0, 0);
				vector3i6.y = (int)(world.GetHeight(vector3i6.x, vector3i6.z) + 1);
				blockValue2.rotation = 3;
				this.signPositions.Add(vector3i6);
				this.blockChanges.Add(new BlockChangeInfo(vector3i6, blockValue2));
				this.currentState = ActionFillSafeZone.FillSafeZoneStates.Action;
				break;
			}
			case ActionFillSafeZone.FillSafeZoneStates.Action:
				GameManager.Instance.ChangeBlocks(null, this.blockChanges);
				this.currentState = ActionFillSafeZone.FillSafeZoneStates.ResetChunks;
				break;
			case ActionFillSafeZone.FillSafeZoneStates.ResetChunks:
			{
				for (int num32 = 0; num32 < this.chunkList.Count; num32++)
				{
					Chunk chunk7 = this.chunkList[num32];
					long num33 = WorldChunkCache.MakeChunkKey(chunk7.X, chunk7.Z);
					this.chunkHash.Add(num33);
				}
				if (base.Owner.Target != null)
				{
					PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
					for (int num34 = 0; num34 < this.signPositions.Count; num34++)
					{
						TEFeatureSignable tefeatureSignable;
						if (world.GetTileEntity(this.signPositions[num34]).TryGetSelfOrFeature<TEFeatureSignable>(out tefeatureSignable))
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

		// Token: 0x0600C88F RID: 51343 RVA: 0x0049A8F8 File Offset: 0x00498AF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void TeleportPlayers()
		{
			foreach (EntityPlayer entityPlayer in this.playerDictionary.Keys)
			{
				GameManager.Instance.StartCoroutine(base.TeleportEntity(entityPlayer, this.playerDictionary[entityPlayer], 0f));
			}
		}

		// Token: 0x0600C890 RID: 51344 RVA: 0x0049A96C File Offset: 0x00498B6C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionFillSafeZone.PropFillType, ref this.fillTypeNames);
			properties.ParseBool(ActionFillSafeZone.PropDestroyClaim, ref this.destroyClaim);
			properties.ParseString(ActionFillSafeZone.PropBlock, ref this.blockName);
			properties.ParseString(ActionFillSafeZone.PropNewName, ref this.newName);
			properties.ParseString(ActionFillSafeZone.PropBlockTags, ref this.blockTags);
		}

		// Token: 0x0600C891 RID: 51345 RVA: 0x0049A9D8 File Offset: 0x00498BD8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFillSafeZone
			{
				fillTypeNames = this.fillTypeNames,
				FillTypeList = this.FillTypeList,
				blockName = this.blockName,
				destroyClaim = this.destroyClaim,
				newName = this.newName,
				blockTags = this.blockTags
			};
		}

		// Token: 0x040097A7 RID: 38823
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionFillSafeZone.FillSafeZoneStates currentState;

		// Token: 0x040097A8 RID: 38824
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionFillSafeZone.FillTypes FillType = ActionFillSafeZone.FillTypes.Sphere;

		// Token: 0x040097A9 RID: 38825
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fillTypeNames = "Sphere";

		// Token: 0x040097AA RID: 38826
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionFillSafeZone.FillTypes> FillTypeList = new List<ActionFillSafeZone.FillTypes>();

		// Token: 0x040097AB RID: 38827
		[PublicizedFrom(EAccessModifier.Private)]
		public bool destroyClaim;

		// Token: 0x040097AC RID: 38828
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x040097AD RID: 38829
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags;

		// Token: 0x040097AE RID: 38830
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFillType = "fill_type";

		// Token: 0x040097AF RID: 38831
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlock = "block";

		// Token: 0x040097B0 RID: 38832
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDestroyClaim = "destroy_claim";

		// Token: 0x040097B1 RID: 38833
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTags = "block_tags";

		// Token: 0x040097B2 RID: 38834
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x040097B3 RID: 38835
		[PublicizedFrom(EAccessModifier.Private)]
		public string blockName = "terrDirtTwitch";

		// Token: 0x040097B4 RID: 38836
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i claimPos = Vector3i.zero;

		// Token: 0x040097B5 RID: 38837
		[PublicizedFrom(EAccessModifier.Private)]
		public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

		// Token: 0x040097B6 RID: 38838
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Chunk> chunkList = new List<Chunk>();

		// Token: 0x040097B7 RID: 38839
		[PublicizedFrom(EAccessModifier.Private)]
		public int claimSize;

		// Token: 0x040097B8 RID: 38840
		[PublicizedFrom(EAccessModifier.Private)]
		public int halfClaimSize;

		// Token: 0x040097B9 RID: 38841
		[PublicizedFrom(EAccessModifier.Private)]
		public int distanceSq;

		// Token: 0x040097BA RID: 38842
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Vector3i> blockedPositions = new HashSet<Vector3i>();

		// Token: 0x040097BB RID: 38843
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetLong chunkHash = new HashSetLong();

		// Token: 0x040097BC RID: 38844
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EntityPlayer, Vector3> playerDictionary = new Dictionary<EntityPlayer, Vector3>();

		// Token: 0x040097BD RID: 38845
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Vector3i> signPositions;

		// Token: 0x02001985 RID: 6533
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FillSafeZoneStates
		{
			// Token: 0x040097BF RID: 38847
			FindClaim,
			// Token: 0x040097C0 RID: 38848
			HandleChunks,
			// Token: 0x040097C1 RID: 38849
			SetupChanges,
			// Token: 0x040097C2 RID: 38850
			AddSigns,
			// Token: 0x040097C3 RID: 38851
			Action,
			// Token: 0x040097C4 RID: 38852
			ResetChunks
		}

		// Token: 0x02001986 RID: 6534
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FillTypes
		{
			// Token: 0x040097C6 RID: 38854
			Cube,
			// Token: 0x040097C7 RID: 38855
			Sphere,
			// Token: 0x040097C8 RID: 38856
			Cylinder,
			// Token: 0x040097C9 RID: 38857
			Pyramid
		}
	}
}
