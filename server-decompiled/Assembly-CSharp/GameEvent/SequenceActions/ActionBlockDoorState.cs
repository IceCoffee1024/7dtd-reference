using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0A RID: 6666
	[Preserve]
	public class ActionBlockDoorState : ActionBaseBlockAction
	{
		// Token: 0x0600CB8C RID: 52108 RVA: 0x004A67EA File Offset: 0x004A49EA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool AllowInTrader()
		{
			return this.traderOnly;
		}

		// Token: 0x0600CB8D RID: 52109 RVA: 0x004A67F4 File Offset: 0x004A49F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				if (this.traderOnly && !world.IsWithinTraderArea(currentPos))
				{
					return null;
				}
				TileEntityComposite tileEntityComposite = world.GetTileEntity(currentPos) as TileEntityComposite;
				TEFeatureDoor tefeatureDoor;
				if (tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor))
				{
					ActionBlockDoorState.OpenDoorStates openDoorStates = this.setOpen;
					bool flag = openDoorStates != ActionBlockDoorState.OpenDoorStates.Close && (openDoorStates != ActionBlockDoorState.OpenDoorStates.Toggle || !tefeatureDoor.IsOpen());
					tefeatureDoor.SetOpen(flag, this.animate);
					TEFeatureLockable tefeatureLockable;
					if (this.handleLock && tileEntityComposite.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable))
					{
						tefeatureLockable.SetLocked(this.setLocked);
					}
					tefeatureDoor.HandleOpenCloseSound(currentPos);
					return new BlockChangeInfo(currentPos, blockValue);
				}
			}
			return null;
		}

		// Token: 0x0600CB8E RID: 52110 RVA: 0x004A689C File Offset: 0x004A4A9C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionBlockDoorState.OpenDoorStates>(ActionBlockDoorState.PropSetOpenState, ref this.setOpen);
			properties.ParseBool(ActionBlockDoorState.PropTraderOnly, ref this.traderOnly);
			properties.ParseBool(ActionBlockDoorState.PropAnimate, ref this.animate);
			if (properties.Contains(ActionBlockDoorState.PropSetLockState))
			{
				this.handleLock = true;
				properties.ParseBool(ActionBlockDoorState.PropSetLockState, ref this.setLocked);
			}
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x004A6908 File Offset: 0x004A4B08
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDoorState
			{
				setOpen = this.setOpen,
				setLocked = this.setLocked,
				handleLock = this.handleLock,
				traderOnly = this.traderOnly
			};
		}

		// Token: 0x04009A49 RID: 39497
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockDoorState.OpenDoorStates setOpen;

		// Token: 0x04009A4A RID: 39498
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool setLocked;

		// Token: 0x04009A4B RID: 39499
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool traderOnly;

		// Token: 0x04009A4C RID: 39500
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animate = true;

		// Token: 0x04009A4D RID: 39501
		[PublicizedFrom(EAccessModifier.Private)]
		public bool handleLock;

		// Token: 0x04009A4E RID: 39502
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetOpenState = "set_open";

		// Token: 0x04009A4F RID: 39503
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetLockState = "set_lock";

		// Token: 0x04009A50 RID: 39504
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimate = "animate";

		// Token: 0x04009A51 RID: 39505
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTraderOnly = "trader_only";

		// Token: 0x02001A0B RID: 6667
		public enum OpenDoorStates
		{
			// Token: 0x04009A53 RID: 39507
			Open,
			// Token: 0x04009A54 RID: 39508
			Close,
			// Token: 0x04009A55 RID: 39509
			Toggle
		}
	}
}
