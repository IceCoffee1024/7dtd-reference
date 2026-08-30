using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AE RID: 6574
	[Preserve]
	public class ActionUnloadItems : ActionBaseItemAction
	{
		// Token: 0x0600C85F RID: 51295 RVA: 0x0049AB94 File Offset: 0x00498D94
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

		// Token: 0x0600C860 RID: 51296 RVA: 0x0049AC4C File Offset: 0x00498E4C
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

		// Token: 0x0600C861 RID: 51297 RVA: 0x0049AD0E File Offset: 0x00498F0E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUnloadItems();
		}

		// Token: 0x04009810 RID: 38928
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> ItemStacks = new List<ItemStack>();
	}
}
