using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A13 RID: 6675
	[Preserve]
	public class ActionBlockReplaceAttack : ActionBlockReplace
	{
		// Token: 0x0600CBAD RID: 52141 RVA: 0x004A6ED0 File Offset: 0x004A50D0
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

		// Token: 0x0600CBAE RID: 52142 RVA: 0x004A6F78 File Offset: 0x004A5178
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

		// Token: 0x0600CBAF RID: 52143 RVA: 0x004A70B4 File Offset: 0x004A52B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionBlockReplaceAttack.PropTimeAlive, ref this.timeAlive);
			properties.ParseString(ActionBlockReplaceAttack.PropRemoveSound, ref this.removeSound);
			properties.ParseBool(ActionBlockReplaceAttack.PropRefundOnRemove, ref this.refundOnRemove);
		}

		// Token: 0x0600CBB0 RID: 52144 RVA: 0x004A70F0 File Offset: 0x004A52F0
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

		// Token: 0x04009A66 RID: 39526
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Vector3i> blocksAdded = new List<Vector3i>();

		// Token: 0x04009A67 RID: 39527
		[PublicizedFrom(EAccessModifier.Protected)]
		public float timeAlive = -1f;

		// Token: 0x04009A68 RID: 39528
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removeSound = "";

		// Token: 0x04009A69 RID: 39529
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool refundOnRemove;

		// Token: 0x04009A6A RID: 39530
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTimeAlive = "time_alive";

		// Token: 0x04009A6B RID: 39531
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSound = "remove_sound";

		// Token: 0x04009A6C RID: 39532
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRefundOnRemove = "refund_on_remove";
	}
}
