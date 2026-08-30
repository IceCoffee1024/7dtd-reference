using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B6 RID: 6582
	[Preserve]
	public class ActionBlockAnimateBlock : ActionBaseBlockAction
	{
		// Token: 0x0600C89E RID: 51358 RVA: 0x0049BC56 File Offset: 0x00499E56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600C89F RID: 51359 RVA: 0x0049BC70 File Offset: 0x00499E70
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockChangeInfo blockChangeInfo = blockChanges[i];
				if ((Chunk)world.GetChunkFromWorldPos(blockChangeInfo.blockValueRef) != null)
				{
					BlockEntityData blockEntity = world.ChunkCache.GetBlockEntity(blockChangeInfo.blockValueRef);
					if (blockEntity != null)
					{
						if (blockEntity.transform == null)
						{
							GameManager.Instance.StartCoroutine(this.WaitForBEDTransform(blockEntity));
						}
						else
						{
							this.AnimateBlock(blockEntity);
						}
					}
				}
			}
		}

		// Token: 0x0600C8A0 RID: 51360 RVA: 0x0049BCF1 File Offset: 0x00499EF1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator WaitForBEDTransform(BlockEntityData bed)
		{
			int num;
			for (int frames = 0; frames < 10; frames = num + 1)
			{
				yield return 0;
				if (bed == null)
				{
					yield break;
				}
				if (bed.transform != null)
				{
					this.AnimateBlock(bed);
					yield break;
				}
				num = frames;
			}
			yield break;
		}

		// Token: 0x0600C8A1 RID: 51361 RVA: 0x0049BD08 File Offset: 0x00499F08
		[PublicizedFrom(EAccessModifier.Private)]
		public void AnimateBlock(BlockEntityData bed)
		{
			Animator[] componentsInChildren = bed.transform.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				for (int i = componentsInChildren.Length - 1; i >= 0; i--)
				{
					Animator animator = componentsInChildren[i];
					animator.enabled = true;
					if (this.animationBool != null)
					{
						animator.SetBool(this.animationBool, this.animationBoolValue);
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAnimateBlock>().Setup(bed.pos, this.animationBool, this.animationBoolValue), false, -1, -1, -1, null, 192, false);
					}
					if (this.animationInteger != null)
					{
						animator.SetInteger(this.animationInteger, this.animationIntegerValue);
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAnimateBlock>().Setup(bed.pos, this.animationInteger, this.animationIntegerValue), false, -1, -1, -1, null, 192, false);
					}
					if (this.animationTrigger != null)
					{
						animator.SetTrigger(this.animationTrigger);
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAnimateBlock>().Setup(bed.pos, this.animationTrigger), false, -1, -1, -1, null, 192, false);
					}
				}
			}
		}

		// Token: 0x0600C8A2 RID: 51362 RVA: 0x0049BE38 File Offset: 0x0049A038
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationBool, ref this.animationBool);
			properties.ParseBool(ActionBlockAnimateBlock.PropAnimationBoolValue, ref this.animationBoolValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationInteger, ref this.animationInteger);
			properties.ParseInt(ActionBlockAnimateBlock.PropAnimationIntegerValue, ref this.animationIntegerValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationTrigger, ref this.animationTrigger);
		}

		// Token: 0x0600C8A3 RID: 51363 RVA: 0x0049BEA4 File Offset: 0x0049A0A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockAnimateBlock
			{
				animationBool = this.animationBool,
				animationBoolValue = this.animationBoolValue,
				animationInteger = this.animationInteger,
				animationIntegerValue = this.animationIntegerValue,
				animationTrigger = this.animationTrigger
			};
		}

		// Token: 0x0400984B RID: 38987
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationBool;

		// Token: 0x0400984C RID: 38988
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationInteger;

		// Token: 0x0400984D RID: 38989
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationTrigger;

		// Token: 0x0400984E RID: 38990
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animationBoolValue = true;

		// Token: 0x0400984F RID: 38991
		[PublicizedFrom(EAccessModifier.Protected)]
		public int animationIntegerValue;

		// Token: 0x04009850 RID: 38992
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBool = "animation_bool";

		// Token: 0x04009851 RID: 38993
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBoolValue = "animation_bool_value";

		// Token: 0x04009852 RID: 38994
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationInteger = "animation_integer";

		// Token: 0x04009853 RID: 38995
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationIntegerValue = "animation_integer_value";

		// Token: 0x04009854 RID: 38996
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationTrigger = "animation_trigger";
	}
}
