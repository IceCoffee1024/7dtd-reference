using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A08 RID: 6664
	[Preserve]
	public class ActionBlockAnimateBlock : ActionBaseBlockAction
	{
		// Token: 0x0600CB7E RID: 52094 RVA: 0x004A645E File Offset: 0x004A465E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CB7F RID: 52095 RVA: 0x004A6478 File Offset: 0x004A4678
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

		// Token: 0x0600CB80 RID: 52096 RVA: 0x004A64F9 File Offset: 0x004A46F9
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

		// Token: 0x0600CB81 RID: 52097 RVA: 0x004A6510 File Offset: 0x004A4710
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

		// Token: 0x0600CB82 RID: 52098 RVA: 0x004A6640 File Offset: 0x004A4840
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationBool, ref this.animationBool);
			properties.ParseBool(ActionBlockAnimateBlock.PropAnimationBoolValue, ref this.animationBoolValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationInteger, ref this.animationInteger);
			properties.ParseInt(ActionBlockAnimateBlock.PropAnimationIntegerValue, ref this.animationIntegerValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationTrigger, ref this.animationTrigger);
		}

		// Token: 0x0600CB83 RID: 52099 RVA: 0x004A66AC File Offset: 0x004A48AC
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

		// Token: 0x04009A3A RID: 39482
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationBool;

		// Token: 0x04009A3B RID: 39483
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationInteger;

		// Token: 0x04009A3C RID: 39484
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationTrigger;

		// Token: 0x04009A3D RID: 39485
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animationBoolValue = true;

		// Token: 0x04009A3E RID: 39486
		[PublicizedFrom(EAccessModifier.Protected)]
		public int animationIntegerValue;

		// Token: 0x04009A3F RID: 39487
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBool = "animation_bool";

		// Token: 0x04009A40 RID: 39488
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBoolValue = "animation_bool_value";

		// Token: 0x04009A41 RID: 39489
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationInteger = "animation_integer";

		// Token: 0x04009A42 RID: 39490
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationIntegerValue = "animation_integer_value";

		// Token: 0x04009A43 RID: 39491
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationTrigger = "animation_trigger";
	}
}
