using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A00 RID: 6656
	[Preserve]
	public class ActionUnloadItems : ActionBaseItemAction
	{
		// Token: 0x0600CB3E RID: 52030 RVA: 0x004A5390 File Offset: 0x004A3590
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			if (!stack.IsEmpty() && (this.itemTags == "" || stack.itemValue.ItemClass.HasAnyTags(this.fastItemTags)))
			{
				ItemClass itemClass = stack.itemValue.ItemClass;
				if (itemClass != null)
				{
					ItemActionAttack itemActionAttack = itemClass.Actions[0] as ItemActionAttack;
					if (itemActionAttack != null && !itemActionAttack.IsEditingTool())
					{
						int meta = stack.itemValue.Meta;
						string text = itemActionAttack.MagazineItemNames[(int)stack.itemValue.SelectedAmmoTypeIndex];
						stack.itemValue.Meta = 0;
						this.ItemStacks.Add(new ItemStack(ItemClass.GetItem(text, false), meta));
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CB3F RID: 52031 RVA: 0x004A5448 File Offset: 0x004A3648
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionEnded(EntityPlayer player)
		{
			base.OnClientActionEnded(player);
			for (int i = this.ItemStacks.Count - 1; i >= 0; i--)
			{
				ItemStack itemStack = this.ItemStacks[i];
				if (LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal).xui.PlayerInventory.AddItem(itemStack))
				{
					this.ItemStacks.RemoveAt(i);
				}
			}
			if (this.ItemStacks.Count > 0)
			{
				string text = "DroppedLootContainerTwitch";
				EntityLootContainer entityLootContainer = EntityFactory.CreateEntity(player.entityId, text.GetHashCode(), player.position, Vector3.zero) as EntityLootContainer;
				if (entityLootContainer != null)
				{
					entityLootContainer.SetContent(ItemStack.Clone(this.ItemStacks), 0);
				}
				GameManager.Instance.World.SpawnEntityInWorld(entityLootContainer);
			}
		}

		// Token: 0x0600CB40 RID: 52032 RVA: 0x004A550B File Offset: 0x004A370B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUnloadItems();
		}

		// Token: 0x040099FF RID: 39423
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> ItemStacks = new List<ItemStack>();
	}
}
