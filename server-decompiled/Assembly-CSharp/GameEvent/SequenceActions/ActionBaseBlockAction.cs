using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A37 RID: 6711
	[Preserve]
	public class ActionBaseBlockAction : BaseAction
	{
		// Token: 0x0600CC80 RID: 52352 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool NeedsDamage()
		{
			return false;
		}

		// Token: 0x0600CC81 RID: 52353 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool AllowInTrader()
		{
			return false;
		}

		// Token: 0x0600CC82 RID: 52354 RVA: 0x004A9928 File Offset: 0x004A7B28
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

		// Token: 0x0600CC83 RID: 52355 RVA: 0x004A9C9D File Offset: 0x004A7E9D
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			world.SetBlocksRPC(blockChanges);
		}

		// Token: 0x0600CC84 RID: 52356 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ChangesComplete()
		{
		}

		// Token: 0x0600CC85 RID: 52357 RVA: 0x00020119 File Offset: 0x0001E319
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckValid(World world, Vector3i currentPos)
		{
			return true;
		}

		// Token: 0x0600CC86 RID: 52358 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			return null;
		}

		// Token: 0x0600CC87 RID: 52359 RVA: 0x004A9CA6 File Offset: 0x004A7EA6
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(0.5f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600CC88 RID: 52360 RVA: 0x004A9CB8 File Offset: 0x004A7EB8
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

		// Token: 0x0600CC89 RID: 52361 RVA: 0x004A9D54 File Offset: 0x004A7F54
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

		// Token: 0x04009B09 RID: 39689
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i minOffset = Vector3i.zero;

		// Token: 0x04009B0A RID: 39690
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i maxOffset = Vector3i.zero;

		// Token: 0x04009B0B RID: 39691
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags;

		// Token: 0x04009B0C RID: 39692
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags;

		// Token: 0x04009B0D RID: 39693
		[PublicizedFrom(EAccessModifier.Protected)]
		public int spacing;

		// Token: 0x04009B0E RID: 39694
		[PublicizedFrom(EAccessModifier.Protected)]
		public int innerOffset = -1;

		// Token: 0x04009B0F RID: 39695
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeAllowed;

		// Token: 0x04009B10 RID: 39696
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkSafe;

		// Token: 0x04009B11 RID: 39697
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowTerrain;

		// Token: 0x04009B12 RID: 39698
		[PublicizedFrom(EAccessModifier.Protected)]
		public float randomChance = -1f;

		// Token: 0x04009B13 RID: 39699
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxCount = -1;

		// Token: 0x04009B14 RID: 39700
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009B15 RID: 39701
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";

		// Token: 0x04009B16 RID: 39702
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinOffset = "min_offset";

		// Token: 0x04009B17 RID: 39703
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxOffset = "max_offset";

		// Token: 0x04009B18 RID: 39704
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpacing = "spacing";

		// Token: 0x04009B19 RID: 39705
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRandomChance = "random_chance";

		// Token: 0x04009B1A RID: 39706
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSafeAllowed = "safe_allowed";

		// Token: 0x04009B1B RID: 39707
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropInnerOffset = "inner_offset";

		// Token: 0x04009B1C RID: 39708
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowTerrain = "allow_terrain";

		// Token: 0x04009B1D RID: 39709
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxCount = "max_count";

		// Token: 0x04009B1E RID: 39710
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom random;

		// Token: 0x04009B1F RID: 39711
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 startPoint = Vector3.zero;
	}
}
