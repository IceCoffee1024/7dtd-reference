using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BA RID: 6586
	[Preserve]
	public class ActionAddItems : ActionBaseClientAction
	{
		// Token: 0x0600CA45 RID: 51781 RVA: 0x0049CADC File Offset: 0x0049ACDC
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

		// Token: 0x0600CA46 RID: 51782 RVA: 0x0049CBC4 File Offset: 0x0049ADC4
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

		// Token: 0x0600CA47 RID: 51783 RVA: 0x0049CC6D File Offset: 0x0049AE6D
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

		// Token: 0x0400984F RID: 38991
		public string[] AddItems;

		// Token: 0x04009850 RID: 38992
		public string[] AddItemCounts;

		// Token: 0x04009851 RID: 38993
		public static string PropAddItems = "added_items";

		// Token: 0x04009852 RID: 38994
		public static string PropAddItemCounts = "added_item_counts";
	}
}
