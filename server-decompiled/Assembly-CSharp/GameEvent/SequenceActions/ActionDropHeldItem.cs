using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197A RID: 6522
	[Preserve]
	public class ActionDropHeldItem : ActionBaseItemAction
	{
		// Token: 0x0600C860 RID: 51296 RVA: 0x0049823C File Offset: 0x0049643C
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !((entityPlayer.saveInventory != null) ? entityPlayer.saveInventory : entityPlayer.inventory).holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C861 RID: 51297 RVA: 0x0049827C File Offset: 0x0049647C
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				Inventory inventory = ((entityAlive.saveInventory != null) ? entityAlive.saveInventory : entityAlive.inventory);
				if (inventory.holdingItem != entityAlive.inventory.GetBareHandItem())
				{
					Vector3 dropPosition = entityAlive.GetDropPosition();
					ItemValue holdingItemItemValue = inventory.holdingItemItemValue;
					int count = inventory.holdingItemStack.count;
					GameManager.Instance.DropContentInLootContainerServer(entityAlive.entityId, "DroppedLootContainerTwitch", dropPosition, new ItemStack[] { inventory.holdingItemStack.Clone() }, false, null);
					entityAlive.AddUIHarvestingItem(new ItemStack(holdingItemItemValue, -count), false);
					Manager.BroadcastPlay(entityAlive, this.DropSound, false, 1f);
					inventory.DecHoldingItem(count);
				}
			}
		}

		// Token: 0x0600C862 RID: 51298 RVA: 0x00498341 File Offset: 0x00496541
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropHeldItem.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C863 RID: 51299 RVA: 0x0049835B File Offset: 0x0049655B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropHeldItem
			{
				targetGroup = this.targetGroup,
				DropSound = this.DropSound
			};
		}

		// Token: 0x04009754 RID: 38740
		public string DropSound = "";

		// Token: 0x04009755 RID: 38741
		public static string PropDropSound = "drop_sound";
	}
}
