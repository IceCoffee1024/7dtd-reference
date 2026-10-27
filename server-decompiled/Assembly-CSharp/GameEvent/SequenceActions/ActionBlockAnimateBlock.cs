using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A39 RID: 6713
	[Preserve]
	public class ActionBlockAnimateBlock : ActionBaseBlockAction
	{
		// Token: 0x0600CC92 RID: 52370 RVA: 0x004A9F5A File Offset: 0x004A815A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CC93 RID: 52371 RVA: 0x004A9F74 File Offset: 0x004A8174
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

		// Token: 0x0600CC94 RID: 52372 RVA: 0x004A9FF5 File Offset: 0x004A81F5
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

		// Token: 0x0600CC95 RID: 52373 RVA: 0x004AA00C File Offset: 0x004A820C
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

		// Token: 0x0600CC96 RID: 52374 RVA: 0x004AA13C File Offset: 0x004A833C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationBool, ref this.animationBool);
			properties.ParseBool(ActionBlockAnimateBlock.PropAnimationBoolValue, ref this.animationBoolValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationInteger, ref this.animationInteger);
			properties.ParseInt(ActionBlockAnimateBlock.PropAnimationIntegerValue, ref this.animationIntegerValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationTrigger, ref this.animationTrigger);
		}

		// Token: 0x0600CC97 RID: 52375 RVA: 0x004AA1A8 File Offset: 0x004A83A8
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

		// Token: 0x04009B23 RID: 39715
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationBool;

		// Token: 0x04009B24 RID: 39716
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationInteger;

		// Token: 0x04009B25 RID: 39717
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationTrigger;

		// Token: 0x04009B26 RID: 39718
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animationBoolValue = true;

		// Token: 0x04009B27 RID: 39719
		[PublicizedFrom(EAccessModifier.Protected)]
		public int animationIntegerValue;

		// Token: 0x04009B28 RID: 39720
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBool = "animation_bool";

		// Token: 0x04009B29 RID: 39721
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBoolValue = "animation_bool_value";

		// Token: 0x04009B2A RID: 39722
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationInteger = "animation_integer";

		// Token: 0x04009B2B RID: 39723
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationIntegerValue = "animation_integer_value";

		// Token: 0x04009B2C RID: 39724
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationTrigger = "animation_trigger";
	}
}
