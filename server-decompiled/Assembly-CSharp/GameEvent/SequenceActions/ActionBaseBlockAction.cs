using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DA RID: 6618
	[Preserve]
	public class ActionBaseBlockAction : BaseAction
	{
		// Token: 0x0600CA14 RID: 51732 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool NeedsDamage()
		{
			return false;
		}

		// Token: 0x0600CA15 RID: 51733 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool AllowInTrader()
		{
			return false;
		}

		// Token: 0x0600CA16 RID: 51734 RVA: 0x004A1988 File Offset: 0x0049FB88
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			List<BlockChangeInfo> list = new List<BlockChangeInfo>();
			this.startPoint = ((base.Owner.TargetPosition.y != 0f) ? base.Owner.TargetPosition : base.Owner.Target.position);
			World world = GameManager.Instance.World;
			this.random = world.GetGameRandom();
			FastTags<TagGroup.Global> fastTags = ((this.blockTags != null) ? FastTags<TagGroup.Global>.Parse(this.blockTags) : FastTags<TagGroup.Global>.none);
			FastTags<TagGroup.Global> fastTags2 = ((this.excludeTags != null) ? FastTags<TagGroup.Global>.Parse(this.excludeTags) : FastTags<TagGroup.Global>.none);
			IChunk chunk = null;
			bool flag = this.AllowInTrader();
			if (base.Owner.Target != null && !base.Owner.Target.onGround)
			{
				return BaseAction.ActionCompleteStates.InComplete;
			}
			for (int i = this.minOffset.y; i <= this.maxOffset.y; i++)
			{
				for (int j = this.minOffset.z; j <= this.maxOffset.z; j += this.spacing + 1)
				{
					int num = (int)Utils.FastAbs((float)j);
					for (int k = this.minOffset.x; k <= this.maxOffset.x; k += this.spacing + 1)
					{
						if ((this.innerOffset == -1 || Utils.FastAbs((float)k) > (float)this.innerOffset || num > this.innerOffset) && (this.randomChance <= 0f || this.random.RandomFloat <= this.randomChance))
						{
							Vector3i vector3i = new Vector3i(Utils.Fastfloor(this.startPoint.x + (float)k), Utils.Fastfloor(this.startPoint.y + (float)i), Utils.Fastfloor(this.startPoint.z + (float)j));
							if (vector3i.y >= 0 && world.GetChunkFromWorldPos(vector3i, ref chunk) && (flag || world.GetTraderAreaAt(vector3i) == null) && (!this.checkSafe || (!this.safeAllowed && world.CanPlaceBlockAt(vector3i, null, false))))
							{
								int num2 = World.toBlockXZ(vector3i.x);
								int num3 = World.toBlockXZ(vector3i.z);
								BlockValue blockValue = (this.NeedsDamage() ? chunk.GetBlock(num2, vector3i.y, num3) : chunk.GetBlockNoDamage(num2, vector3i.y, num3));
								if (!blockValue.ischild && (this.allowTerrain || !blockValue.Block.shape.IsTerrain()) && (this.blockTags == null || blockValue.Block.Tags.Test_AnySet(fastTags)) && (this.excludeTags == null || !blockValue.Block.Tags.Test_AnySet(fastTags2)) && this.CheckValid(world, vector3i))
								{
									BlockChangeInfo blockChangeInfo = this.UpdateBlock(world, vector3i, blockValue);
									if (blockChangeInfo != null)
									{
										list.Add(blockChangeInfo);
									}
								}
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				if (this.maxCount != -1 && this.maxCount < list.Count)
				{
					int num4 = list.Count - this.maxCount;
					for (int l = 0; l < num4; l++)
					{
						list.RemoveAt(this.random.RandomRange(list.Count));
					}
				}
				this.ChangesComplete();
				this.ProcessChanges(world, list);
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600CA17 RID: 51735 RVA: 0x004A1CFD File Offset: 0x0049FEFD
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			world.SetBlocksRPC(blockChanges);
		}

		// Token: 0x0600CA18 RID: 51736 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ChangesComplete()
		{
		}

		// Token: 0x0600CA19 RID: 51737 RVA: 0x00020021 File Offset: 0x0001E221
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckValid(World world, Vector3i currentPos)
		{
			return true;
		}

		// Token: 0x0600CA1A RID: 51738 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			return null;
		}

		// Token: 0x0600CA1B RID: 51739 RVA: 0x004A1D06 File Offset: 0x0049FF06
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(0.5f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600CA1C RID: 51740 RVA: 0x004A1D18 File Offset: 0x0049FF18
		public override BaseAction Clone()
		{
			ActionBaseBlockAction actionBaseBlockAction = base.Clone() as ActionBaseBlockAction;
			actionBaseBlockAction.minOffset = this.minOffset;
			actionBaseBlockAction.maxOffset = this.maxOffset;
			actionBaseBlockAction.spacing = this.spacing;
			actionBaseBlockAction.randomChance = this.randomChance;
			actionBaseBlockAction.safeAllowed = this.safeAllowed;
			actionBaseBlockAction.checkSafe = this.checkSafe;
			actionBaseBlockAction.blockTags = this.blockTags;
			actionBaseBlockAction.excludeTags = this.excludeTags;
			actionBaseBlockAction.innerOffset = this.innerOffset;
			actionBaseBlockAction.allowTerrain = this.allowTerrain;
			actionBaseBlockAction.maxCount = this.maxCount;
			return actionBaseBlockAction;
		}

		// Token: 0x0600CA1D RID: 51741 RVA: 0x004A1DB4 File Offset: 0x0049FFB4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(ActionBaseBlockAction.PropMinOffset, ref this.minOffset);
			properties.ParseVec(ActionBaseBlockAction.PropMaxOffset, ref this.maxOffset);
			properties.ParseInt(ActionBaseBlockAction.PropSpacing, ref this.spacing);
			properties.ParseInt(ActionBaseBlockAction.PropInnerOffset, ref this.innerOffset);
			properties.ParseFloat(ActionBaseBlockAction.PropRandomChance, ref this.randomChance);
			if (properties.Contains(ActionBaseBlockAction.PropSafeAllowed))
			{
				properties.ParseBool(ActionBaseBlockAction.PropSafeAllowed, ref this.safeAllowed);
				this.checkSafe = true;
			}
			properties.ParseString(ActionBaseBlockAction.PropBlockTags, ref this.blockTags);
			properties.ParseString(ActionBaseBlockAction.PropExcludeTags, ref this.excludeTags);
			properties.ParseBool(ActionBaseBlockAction.PropAllowTerrain, ref this.allowTerrain);
			properties.ParseInt(ActionBaseBlockAction.PropMaxCount, ref this.maxCount);
		}

		// Token: 0x0400995A RID: 39258
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i minOffset = Vector3i.zero;

		// Token: 0x0400995B RID: 39259
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i maxOffset = Vector3i.zero;

		// Token: 0x0400995C RID: 39260
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags;

		// Token: 0x0400995D RID: 39261
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags;

		// Token: 0x0400995E RID: 39262
		[PublicizedFrom(EAccessModifier.Protected)]
		public int spacing;

		// Token: 0x0400995F RID: 39263
		[PublicizedFrom(EAccessModifier.Protected)]
		public int innerOffset = -1;

		// Token: 0x04009960 RID: 39264
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeAllowed;

		// Token: 0x04009961 RID: 39265
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkSafe;

		// Token: 0x04009962 RID: 39266
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowTerrain;

		// Token: 0x04009963 RID: 39267
		[PublicizedFrom(EAccessModifier.Protected)]
		public float randomChance = -1f;

		// Token: 0x04009964 RID: 39268
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxCount = -1;

		// Token: 0x04009965 RID: 39269
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009966 RID: 39270
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";

		// Token: 0x04009967 RID: 39271
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinOffset = "min_offset";

		// Token: 0x04009968 RID: 39272
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxOffset = "max_offset";

		// Token: 0x04009969 RID: 39273
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpacing = "spacing";

		// Token: 0x0400996A RID: 39274
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRandomChance = "random_chance";

		// Token: 0x0400996B RID: 39275
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSafeAllowed = "safe_allowed";

		// Token: 0x0400996C RID: 39276
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropInnerOffset = "inner_offset";

		// Token: 0x0400996D RID: 39277
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowTerrain = "allow_terrain";

		// Token: 0x0400996E RID: 39278
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxCount = "max_count";

		// Token: 0x0400996F RID: 39279
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom random;

		// Token: 0x04009970 RID: 39280
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 startPoint = Vector3.zero;
	}
}
