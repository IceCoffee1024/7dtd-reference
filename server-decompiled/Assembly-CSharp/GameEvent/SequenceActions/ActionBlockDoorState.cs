using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A3B RID: 6715
	[Preserve]
	public class ActionBlockDoorState : ActionBaseBlockAction
	{
		// Token: 0x0600CCA0 RID: 52384 RVA: 0x004AA2E6 File Offset: 0x004A84E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool AllowInTrader()
		{
			return this.traderOnly;
		}

		// Token: 0x0600CCA1 RID: 52385 RVA: 0x004AA2F0 File Offset: 0x004A84F0
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

		// Token: 0x0600CCA2 RID: 52386 RVA: 0x004AA398 File Offset: 0x004A8598
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

		// Token: 0x0600CCA3 RID: 52387 RVA: 0x004AA404 File Offset: 0x004A8604
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

		// Token: 0x04009B32 RID: 39730
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockDoorState.OpenDoorStates setOpen;

		// Token: 0x04009B33 RID: 39731
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool setLocked;

		// Token: 0x04009B34 RID: 39732
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool traderOnly;

		// Token: 0x04009B35 RID: 39733
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animate = true;

		// Token: 0x04009B36 RID: 39734
		[PublicizedFrom(EAccessModifier.Private)]
		public bool handleLock;

		// Token: 0x04009B37 RID: 39735
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetOpenState = "set_open";

		// Token: 0x04009B38 RID: 39736
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetLockState = "set_lock";

		// Token: 0x04009B39 RID: 39737
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimate = "animate";

		// Token: 0x04009B3A RID: 39738
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTraderOnly = "trader_only";

		// Token: 0x02001A3C RID: 6716
		public enum OpenDoorStates
		{
			// Token: 0x04009B3C RID: 39740
			Open,
			// Token: 0x04009B3D RID: 39741
			Close,
			// Token: 0x04009B3E RID: 39742
			Toggle
		}
	}
}
