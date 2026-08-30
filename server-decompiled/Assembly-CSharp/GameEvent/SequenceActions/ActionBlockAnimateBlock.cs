using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DC RID: 6620
	[Preserve]
	public class ActionBlockAnimateBlock : ActionBaseBlockAction
	{
		// Token: 0x0600CA26 RID: 51750 RVA: 0x004A1FBA File Offset: 0x004A01BA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CA27 RID: 51751 RVA: 0x004A1FD4 File Offset: 0x004A01D4
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

		// Token: 0x0600CA28 RID: 51752 RVA: 0x004A2055 File Offset: 0x004A0255
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

		// Token: 0x0600CA29 RID: 51753 RVA: 0x004A206C File Offset: 0x004A026C
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

		// Token: 0x0600CA2A RID: 51754 RVA: 0x004A219C File Offset: 0x004A039C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationBool, ref this.animationBool);
			properties.ParseBool(ActionBlockAnimateBlock.PropAnimationBoolValue, ref this.animationBoolValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationInteger, ref this.animationInteger);
			properties.ParseInt(ActionBlockAnimateBlock.PropAnimationIntegerValue, ref this.animationIntegerValue);
			properties.ParseString(ActionBlockAnimateBlock.PropAnimationTrigger, ref this.animationTrigger);
		}

		// Token: 0x0600CA2B RID: 51755 RVA: 0x004A2208 File Offset: 0x004A0408
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

		// Token: 0x04009974 RID: 39284
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationBool;

		// Token: 0x04009975 RID: 39285
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationInteger;

		// Token: 0x04009976 RID: 39286
		[PublicizedFrom(EAccessModifier.Protected)]
		public string animationTrigger;

		// Token: 0x04009977 RID: 39287
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animationBoolValue = true;

		// Token: 0x04009978 RID: 39288
		[PublicizedFrom(EAccessModifier.Protected)]
		public int animationIntegerValue;

		// Token: 0x04009979 RID: 39289
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBool = "animation_bool";

		// Token: 0x0400997A RID: 39290
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationBoolValue = "animation_bool_value";

		// Token: 0x0400997B RID: 39291
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationInteger = "animation_integer";

		// Token: 0x0400997C RID: 39292
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationIntegerValue = "animation_integer_value";

		// Token: 0x0400997D RID: 39293
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimationTrigger = "animation_trigger";
	}
}
