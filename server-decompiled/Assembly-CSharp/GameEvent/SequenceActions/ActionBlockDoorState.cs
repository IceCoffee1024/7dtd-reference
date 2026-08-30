using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B8 RID: 6584
	[Preserve]
	public class ActionBlockDoorState : ActionBaseBlockAction
	{
		// Token: 0x0600C8AC RID: 51372 RVA: 0x0049BFE4 File Offset: 0x0049A1E4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				TileEntityComposite tileEntityComposite = world.GetTileEntity(currentPos) as TileEntityComposite;
				TEFeatureDoor tefeatureDoor;
				if (tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor))
				{
					tefeatureDoor.SetOpen(this.setOpen, false);
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

		// Token: 0x0600C8AD RID: 51373 RVA: 0x0049C051 File Offset: 0x0049A251
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionBlockDoorState.PropSetOpenState, ref this.setOpen);
			if (properties.Contains(ActionBlockDoorState.PropSetLockState))
			{
				this.handleLock = true;
				properties.ParseBool(ActionBlockDoorState.PropSetLockState, ref this.setLocked);
			}
		}

		// Token: 0x0600C8AE RID: 51374 RVA: 0x0049C090 File Offset: 0x0049A290
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDoorState
			{
				setOpen = this.setOpen,
				setLocked = this.setLocked,
				handleLock = this.handleLock
			};
		}

		// Token: 0x0400985A RID: 39002
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool setOpen = true;

		// Token: 0x0400985B RID: 39003
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool setLocked;

		// Token: 0x0400985C RID: 39004
		[PublicizedFrom(EAccessModifier.Private)]
		public bool handleLock;

		// Token: 0x0400985D RID: 39005
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetOpenState = "set_open";

		// Token: 0x0400985E RID: 39006
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSetLockState = "set_lock";
	}
}
