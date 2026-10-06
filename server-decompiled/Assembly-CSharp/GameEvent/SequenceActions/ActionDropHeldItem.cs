using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A6 RID: 6566
	[Preserve]
	public class ActionDropHeldItem : ActionBaseItemAction
	{
		// Token: 0x0600C9B8 RID: 51640 RVA: 0x0049C718 File Offset: 0x0049A918
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C9B9 RID: 51641 RVA: 0x0049C748 File Offset: 0x0049A948
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

		// Token: 0x0600C9BA RID: 51642 RVA: 0x0049C805 File Offset: 0x0049AA05
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropHeldItem.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x0049C81F File Offset: 0x0049AA1F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropHeldItem
			{
				targetGroup = this.targetGroup,
				DropSound = this.DropSound
			};
		}

		// Token: 0x0400981A RID: 38938
		public string DropSound = "";

		// Token: 0x0400981B RID: 38939
		public static string PropDropSound = "drop_sound";
	}
}
