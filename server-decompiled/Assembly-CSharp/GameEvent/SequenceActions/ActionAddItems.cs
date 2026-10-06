using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001989 RID: 6537
	[Preserve]
	public class ActionAddItems : ActionBaseClientAction
	{
		// Token: 0x0600C931 RID: 51505 RVA: 0x004990E8 File Offset: 0x004972E8
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

		// Token: 0x0600C932 RID: 51506 RVA: 0x004991D0 File Offset: 0x004973D0
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

		// Token: 0x0600C933 RID: 51507 RVA: 0x00499279 File Offset: 0x00497479
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

		// Token: 0x04009766 RID: 38758
		public string[] AddItems;

		// Token: 0x04009767 RID: 38759
		public string[] AddItemCounts;

		// Token: 0x04009768 RID: 38760
		public static string PropAddItems = "added_items";

		// Token: 0x04009769 RID: 38761
		public static string PropAddItemCounts = "added_item_counts";
	}
}
