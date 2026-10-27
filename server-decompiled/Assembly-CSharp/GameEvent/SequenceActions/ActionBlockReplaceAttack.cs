using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A44 RID: 6724
	[Preserve]
	public class ActionBlockReplaceAttack : ActionBlockReplace
	{
		// Token: 0x0600CCC1 RID: 52417 RVA: 0x004AA9CC File Offset: 0x004A8BCC
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

		// Token: 0x0600CCC2 RID: 52418 RVA: 0x004AAA74 File Offset: 0x004A8C74
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

		// Token: 0x0600CCC3 RID: 52419 RVA: 0x004AABB0 File Offset: 0x004A8DB0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionBlockReplaceAttack.PropTimeAlive, ref this.timeAlive);
			properties.ParseString(ActionBlockReplaceAttack.PropRemoveSound, ref this.removeSound);
			properties.ParseBool(ActionBlockReplaceAttack.PropRefundOnRemove, ref this.refundOnRemove);
		}

		// Token: 0x0600CCC4 RID: 52420 RVA: 0x004AABEC File Offset: 0x004A8DEC
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

		// Token: 0x04009B4F RID: 39759
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Vector3i> blocksAdded = new List<Vector3i>();

		// Token: 0x04009B50 RID: 39760
		[PublicizedFrom(EAccessModifier.Protected)]
		public float timeAlive = -1f;

		// Token: 0x04009B51 RID: 39761
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removeSound = "";

		// Token: 0x04009B52 RID: 39762
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool refundOnRemove;

		// Token: 0x04009B53 RID: 39763
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTimeAlive = "time_alive";

		// Token: 0x04009B54 RID: 39764
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSound = "remove_sound";

		// Token: 0x04009B55 RID: 39765
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRefundOnRemove = "refund_on_remove";
	}
}
