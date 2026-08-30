using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B4 RID: 6580
	[Preserve]
	public class ActionBaseBlockAction : BaseAction
	{
		// Token: 0x0600C88D RID: 51341 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool NeedsDamage()
		{
			return false;
		}

		// Token: 0x0600C88E RID: 51342 RVA: 0x0049B630 File Offset: 0x00499830
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			List<BlockChangeInfo> list = new List<BlockChangeInfo>();
			this.startPoint = ((base.Owner.TargetPosition.y != 0f) ? base.Owner.TargetPosition : base.Owner.Target.position);
			World world = GameManager.Instance.World;
			this.random = world.GetGameRandom();
			FastTags<TagGroup.Global> fastTags = ((this.blockTags != null) ? FastTags<TagGroup.Global>.Parse(this.blockTags) : FastTags<TagGroup.Global>.none);
			FastTags<TagGroup.Global> fastTags2 = ((this.excludeTags != null) ? FastTags<TagGroup.Global>.Parse(this.excludeTags) : FastTags<TagGroup.Global>.none);
			IChunk chunk = null;
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
							if (vector3i.y >= 0 && world.GetChunkFromWorldPos(vector3i, ref chunk) && world.GetTraderAreaAt(vector3i) == null && (!this.checkSafe || (!this.safeAllowed && world.CanPlaceBlockAt(vector3i, null, false))))
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

		// Token: 0x0600C88F RID: 51343 RVA: 0x0049B999 File Offset: 0x00499B99
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			world.SetBlocksRPC(blockChanges);
		}

		// Token: 0x0600C890 RID: 51344 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ChangesComplete()
		{
		}

		// Token: 0x0600C891 RID: 51345 RVA: 0x0001FF5D File Offset: 0x0001E15D
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckValid(World world, Vector3i currentPos)
		{
			return true;
		}

		// Token: 0x0600C892 RID: 51346 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			return null;
		}

		// Token: 0x0600C893 RID: 51347 RVA: 0x0049B9A2 File Offset: 0x00499BA2
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(0.5f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600C894 RID: 51348 RVA: 0x0049B9B4 File Offset: 0x00499BB4
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

		// Token: 0x0600C895 RID: 51349 RVA: 0x0049BA50 File Offset: 0x00499C50
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

		// Token: 0x04009831 RID: 38961
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i minOffset = Vector3i.zero;

		// Token: 0x04009832 RID: 38962
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3i maxOffset = Vector3i.zero;

		// Token: 0x04009833 RID: 38963
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags;

		// Token: 0x04009834 RID: 38964
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags;

		// Token: 0x04009835 RID: 38965
		[PublicizedFrom(EAccessModifier.Protected)]
		public int spacing;

		// Token: 0x04009836 RID: 38966
		[PublicizedFrom(EAccessModifier.Protected)]
		public int innerOffset = -1;

		// Token: 0x04009837 RID: 38967
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool safeAllowed;

		// Token: 0x04009838 RID: 38968
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkSafe;

		// Token: 0x04009839 RID: 38969
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowTerrain;

		// Token: 0x0400983A RID: 38970
		[PublicizedFrom(EAccessModifier.Protected)]
		public float randomChance = -1f;

		// Token: 0x0400983B RID: 38971
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxCount = -1;

		// Token: 0x0400983C RID: 38972
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTags = "block_tags";

		// Token: 0x0400983D RID: 38973
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";

		// Token: 0x0400983E RID: 38974
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinOffset = "min_offset";

		// Token: 0x0400983F RID: 38975
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxOffset = "max_offset";

		// Token: 0x04009840 RID: 38976
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpacing = "spacing";

		// Token: 0x04009841 RID: 38977
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRandomChance = "random_chance";

		// Token: 0x04009842 RID: 38978
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSafeAllowed = "safe_allowed";

		// Token: 0x04009843 RID: 38979
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropInnerOffset = "inner_offset";

		// Token: 0x04009844 RID: 38980
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowTerrain = "allow_terrain";

		// Token: 0x04009845 RID: 38981
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxCount = "max_count";

		// Token: 0x04009846 RID: 38982
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameRandom random;

		// Token: 0x04009847 RID: 38983
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 startPoint = Vector3.zero;
	}
}
