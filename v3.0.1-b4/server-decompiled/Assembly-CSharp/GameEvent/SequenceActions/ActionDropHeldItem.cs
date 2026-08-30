using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001954 RID: 6484
	[Preserve]
	public class ActionDropHeldItem : ActionBaseItemAction
	{
		// Token: 0x0600C6D9 RID: 50905 RVA: 0x00491EF4 File Offset: 0x004900F4
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !((entityPlayer.saveInventory != null) ? entityPlayer.saveInventory : entityPlayer.inventory).holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C6DA RID: 50906 RVA: 0x00491F34 File Offset: 0x00490134
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

		// Token: 0x0600C6DB RID: 50907 RVA: 0x00491FF9 File Offset: 0x004901F9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropHeldItem.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C6DC RID: 50908 RVA: 0x00492013 File Offset: 0x00490213
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropHeldItem
			{
				targetGroup = this.targetGroup,
				DropSound = this.DropSound
			};
		}

		// Token: 0x0400962B RID: 38443
		public string DropSound = "";

		// Token: 0x0400962C RID: 38444
		public static string PropDropSound = "drop_sound";
	}
}
