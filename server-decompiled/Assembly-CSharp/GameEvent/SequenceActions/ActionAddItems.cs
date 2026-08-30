using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200195D RID: 6493
	[Preserve]
	public class ActionAddItems : ActionBaseClientAction
	{
		// Token: 0x0600C7D9 RID: 51161 RVA: 0x00494BFC File Offset: 0x00492DFC
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

		// Token: 0x0600C7DA RID: 51162 RVA: 0x00494CE4 File Offset: 0x00492EE4
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

		// Token: 0x0600C7DB RID: 51163 RVA: 0x00494D8D File Offset: 0x00492F8D
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

		// Token: 0x040096A0 RID: 38560
		public string[] AddItems;

		// Token: 0x040096A1 RID: 38561
		public string[] AddItemCounts;

		// Token: 0x040096A2 RID: 38562
		public static string PropAddItems = "added_items";

		// Token: 0x040096A3 RID: 38563
		public static string PropAddItemCounts = "added_item_counts";
	}
}
