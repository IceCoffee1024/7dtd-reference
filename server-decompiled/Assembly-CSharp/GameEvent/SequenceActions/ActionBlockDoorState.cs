using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DE RID: 6622
	[Preserve]
	public class ActionBlockDoorState : ActionBaseBlockAction
	{
		// Token: 0x0600CA34 RID: 51764 RVA: 0x004A2346 File Offset: 0x004A0546
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool AllowInTrader()
		{
			return this.traderOnly;
		}

		// Token: 0x0600CA35 RID: 51765 RVA: 0x004A2350 File Offset: 0x004A0550
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

		// Token: 0x0600CA36 RID: 51766 RVA: 0x004A23F8 File Offset: 0x004A05F8
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

		// Token: 0x0600CA37 RID: 51767 RVA: 0x004A2464 File Offset: 0x004A0664
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

		// Token: 0x04009983 RID: 39299
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockDoorState.OpenDoorStates setOpen;

		// Token: 0x04009984 RID: 39300
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool setLocked;

		// Token: 0x04009985 RID: 39301
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool traderOnly;

		// Token: 0x04009986 RID: 39302
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool animate = true;

		// Token: 0x04009987 RID: 39303
		[PublicizedFrom(EAccessModifier.Private)]
		public bool handleLock;

		// Token: 0x04009988 RID: 39304
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetOpenState = "set_open";

		// Token: 0x04009989 RID: 39305
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetLockState = "set_lock";

		// Token: 0x0400998A RID: 39306
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAnimate = "animate";

		// Token: 0x0400998B RID: 39307
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTraderOnly = "trader_only";

		// Token: 0x020019DF RID: 6623
		public enum OpenDoorStates
		{
			// Token: 0x0400998D RID: 39309
			Open,
			// Token: 0x0400998E RID: 39310
			Close,
			// Token: 0x0400998F RID: 39311
			Toggle
		}
	}
}
