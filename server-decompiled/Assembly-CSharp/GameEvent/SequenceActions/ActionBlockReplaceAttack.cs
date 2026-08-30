using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E7 RID: 6631
	[Preserve]
	public class ActionBlockReplaceAttack : ActionBlockReplace
	{
		// Token: 0x0600CA55 RID: 51797 RVA: 0x004A2A2C File Offset: 0x004A0C2C
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

		// Token: 0x0600CA56 RID: 51798 RVA: 0x004A2AD4 File Offset: 0x004A0CD4
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

		// Token: 0x0600CA57 RID: 51799 RVA: 0x004A2C10 File Offset: 0x004A0E10
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionBlockReplaceAttack.PropTimeAlive, ref this.timeAlive);
			properties.ParseString(ActionBlockReplaceAttack.PropRemoveSound, ref this.removeSound);
			properties.ParseBool(ActionBlockReplaceAttack.PropRefundOnRemove, ref this.refundOnRemove);
		}

		// Token: 0x0600CA58 RID: 51800 RVA: 0x004A2C4C File Offset: 0x004A0E4C
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

		// Token: 0x040099A0 RID: 39328
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<Vector3i> blocksAdded = new List<Vector3i>();

		// Token: 0x040099A1 RID: 39329
		[PublicizedFrom(EAccessModifier.Protected)]
		public float timeAlive = -1f;

		// Token: 0x040099A2 RID: 39330
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removeSound = "";

		// Token: 0x040099A3 RID: 39331
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool refundOnRemove;

		// Token: 0x040099A4 RID: 39332
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTimeAlive = "time_alive";

		// Token: 0x040099A5 RID: 39333
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSound = "remove_sound";

		// Token: 0x040099A6 RID: 39334
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRefundOnRemove = "refund_on_remove";
	}
}
