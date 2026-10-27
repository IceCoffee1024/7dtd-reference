using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D7 RID: 6615
	[Preserve]
	public class ActionDropHeldItem : ActionBaseItemAction
	{
		// Token: 0x0600CACC RID: 51916 RVA: 0x004A0218 File Offset: 0x0049E418
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600CACD RID: 51917 RVA: 0x004A0248 File Offset: 0x0049E448
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				Inventory inventory = entityAlive.inventory;
				if (inventory.holdingItem != inventory.Hand.BareHandItem)
				{
					Vector3 dropPosition = entityAlive.GetDropPosition();
					ItemValue holdingItemItemValue = inventory.holdingItemItemValue;
					int count = inventory.holdingItemStack.count;
					GameManager.Instance.DropContentInLootContainerServer(entityAlive.entityId, "DroppedLootContainerTwitch", dropPosition, new ItemStack[] { inventory.holdingItemStack.Clone() }, false, null);
					entityAlive.AddUIHarvestingItem(new ItemStack(holdingItemItemValue.Clone(), -count), false);
					Manager.BroadcastPlay(entityAlive, this.DropSound, false, 1f);
					inventory.DecHoldingItem(count);
				}
			}
		}

		// Token: 0x0600CACE RID: 51918 RVA: 0x004A0305 File Offset: 0x0049E505
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropHeldItem.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600CACF RID: 51919 RVA: 0x004A031F File Offset: 0x0049E51F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropHeldItem
			{
				targetGroup = this.targetGroup,
				DropSound = this.DropSound
			};
		}

		// Token: 0x04009903 RID: 39171
		public string DropSound = "";

		// Token: 0x04009904 RID: 39172
		public static string PropDropSound = "drop_sound";
	}
}
