using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001937 RID: 6455
	[Preserve]
	public class ActionAddItems : ActionBaseClientAction
	{
		// Token: 0x0600C652 RID: 50770 RVA: 0x0048E8B0 File Offset: 0x0048CAB0
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				for (int i = 0; i < this.AddItems.Length; i++)
				{
					string text = ((this.AddItemCounts != null && this.AddItemCounts.Length > i) ? this.AddItemCounts[i] : "1");
					int num = 1;
					ItemClass itemClass = ItemClass.GetItemClass(this.AddItems[i], false);
					num = GameEventManager.GetIntValue(entityPlayerLocal, text, num);
					ItemValue itemValue;
					if (itemClass.HasQuality)
					{
						itemValue = new ItemValue(itemClass.Id, num, num, false, null, 1f);
						num = 1;
					}
					else
					{
						itemValue = new ItemValue(itemClass.Id, false);
					}
					ItemStack itemStack = new ItemStack(itemValue, num);
					if (!LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).xui.PlayerInventory.AddItem(itemStack))
					{
						GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), Vector3.zero, -1, 60f, false);
					}
				}
			}
		}

		// Token: 0x0600C653 RID: 50771 RVA: 0x0048E998 File Offset: 0x0048CB98
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (!properties.Values.ContainsKey(ActionAddItems.PropAddItems))
			{
				this.AddItems = null;
				this.AddItemCounts = null;
				return;
			}
			this.AddItems = properties.Values[ActionAddItems.PropAddItems].Replace(" ", "").Split(',', StringSplitOptions.None);
			if (properties.Values.ContainsKey(ActionAddItems.PropAddItemCounts))
			{
				this.AddItemCounts = properties.Values[ActionAddItems.PropAddItemCounts].Replace(" ", "").Split(',', StringSplitOptions.None);
				return;
			}
			this.AddItemCounts = null;
		}

		// Token: 0x0600C654 RID: 50772 RVA: 0x0048EA41 File Offset: 0x0048CC41
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddItems
			{
				AddItems = this.AddItems,
				AddItemCounts = this.AddItemCounts,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009577 RID: 38263
		public string[] AddItems;

		// Token: 0x04009578 RID: 38264
		public string[] AddItemCounts;

		// Token: 0x04009579 RID: 38265
		public static string PropAddItems = "added_items";

		// Token: 0x0400957A RID: 38266
		public static string PropAddItemCounts = "added_item_counts";
	}
}
