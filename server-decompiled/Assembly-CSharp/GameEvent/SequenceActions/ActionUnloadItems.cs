using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D4 RID: 6612
	[Preserve]
	public class ActionUnloadItems : ActionBaseItemAction
	{
		// Token: 0x0600C9E6 RID: 51686 RVA: 0x004A0EEC File Offset: 0x0049F0EC
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

		// Token: 0x0600C9E7 RID: 51687 RVA: 0x004A0FA4 File Offset: 0x0049F1A4
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
					entityLootContainer.SetContent(ItemStack.Clone(this.ItemStacks));
				}
				GameManager.Instance.World.SpawnEntityInWorld(entityLootContainer);
			}
		}

		// Token: 0x0600C9E8 RID: 51688 RVA: 0x004A1066 File Offset: 0x0049F266
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUnloadItems();
		}

		// Token: 0x04009939 RID: 39225
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> ItemStacks = new List<ItemStack>();
	}
}
