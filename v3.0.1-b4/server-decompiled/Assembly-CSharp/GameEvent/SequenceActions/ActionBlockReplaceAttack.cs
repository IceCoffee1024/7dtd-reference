using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C0 RID: 6592
	[Preserve]
	public class ActionBlockReplaceAttack : ActionBlockReplace
	{
		// Token: 0x0600C8CC RID: 51404 RVA: 0x0049C638 File Offset: 0x0049A838
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (this.blockTo == null)
			{
				return null;
			}
			if (this.emptyOnly && !blockValue.isair)
			{
				return null;
			}
			if (!blockValue.Block.blockMaterial.CanDestroy)
			{
				return null;
			}
			BlockValue blockValue2 = Block.GetBlockValue(this.blockTo[this.random.RandomRange(0, this.blockTo.Length)], false);
			if (blockValue.type != blockValue2.type)
			{
				if (!blockValue2.isair)
				{
					if (this.blocksAdded == null)
					{
						this.blocksAdded = new List<Vector3i>();
					}
					this.blocksAdded.Add(currentPos);
				}
				return new BlockChangeInfo(currentPos, blockValue2, true);
			}
			return null;
		}

		// Token: 0x0600C8CD RID: 51405 RVA: 0x0049C6E0 File Offset: 0x0049A8E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ChangesComplete()
		{
			base.ChangesComplete();
			if (this.blocksAdded != null)
			{
				GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = GameEventManager.Current.RegisterSpawnedBlocks(this.blocksAdded, base.Owner.Target, base.Owner.Requester, base.Owner, this.timeAlive, this.removeSound, (base.Owner.Target != null) ? base.Owner.Target.position : base.Owner.TargetPosition, this.refundOnRemove);
				if (base.Owner.Requester != null)
				{
					if (base.Owner.Requester is EntityPlayerLocal)
					{
						GameEventManager.Current.HandleGameBlocksAdded(base.Owner.Name, spawnedBlocksEntry.BlockGroupID, this.blocksAdded, base.Owner.Tag);
						return;
					}
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.BlocksAdded, base.Owner.Name, spawnedBlocksEntry.BlockGroupID, this.blocksAdded, base.Owner.Tag, false), false, base.Owner.Requester.entityId, -1, -1, null, 192, false);
				}
			}
		}

		// Token: 0x0600C8CE RID: 51406 RVA: 0x0049C81C File Offset: 0x0049AA1C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionBlockReplaceAttack.PropTimeAlive, ref this.timeAlive);
			properties.ParseString(ActionBlockReplaceAttack.PropRemoveSound, ref this.removeSound);
			properties.ParseBool(ActionBlockReplaceAttack.PropRefundOnRemove, ref this.refundOnRemove);
		}

		// Token: 0x0600C8CF RID: 51407 RVA: 0x0049C858 File Offset: 0x0049AA58
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockReplaceAttack
			{
				blockTo = this.blockTo,
				emptyOnly = this.emptyOnly,
				timeAlive = this.timeAlive,
				removeSound = this.removeSound,
				refundOnRemove = this.refundOnRemove
			};
		}

		// Token: 0x0400986F RID: 39023
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Vector3i> blocksAdded = new List<Vector3i>();

		// Token: 0x04009870 RID: 39024
		[PublicizedFrom(EAccessModifier.Protected)]
		public float timeAlive = -1f;

		// Token: 0x04009871 RID: 39025
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removeSound = "";

		// Token: 0x04009872 RID: 39026
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool refundOnRemove;

		// Token: 0x04009873 RID: 39027
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTimeAlive = "time_alive";

		// Token: 0x04009874 RID: 39028
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSound = "remove_sound";

		// Token: 0x04009875 RID: 39029
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRefundOnRemove = "refund_on_remove";
	}
}
