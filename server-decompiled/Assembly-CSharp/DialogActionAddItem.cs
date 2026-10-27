using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002E8 RID: 744
[Preserve]
public class DialogActionAddItem : BaseDialogAction
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001556 RID: 5462 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddItem;
		}
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000806F4 File Offset: 0x0007E8F4
	public override void PerformAction(EntityPlayer player)
	{
		ItemValue item = ItemClass.GetItem(base.ID, false);
		ItemValue itemValue = new ItemValue(ItemClass.GetItem(base.ID, false).type, true);
		int num = 1;
		if (base.Value != null && base.Value != "")
		{
			if (int.TryParse(base.Value, out num))
			{
				if (itemValue.HasQuality)
				{
					itemValue = new ItemValue(item.type, num, num, true, null, 1f);
					num = 1;
				}
				else
				{
					itemValue = new ItemValue(item.type, true);
				}
			}
			else if (base.Value.Contains(","))
			{
				string[] array = base.Value.Split(',', StringSplitOptions.None);
				int num2 = Convert.ToInt32(array[0]);
				int num3 = Convert.ToInt32(array[1]);
				if (itemValue.HasQuality)
				{
					itemValue = new ItemValue(item.type, num2, num3, true, null, 1f);
					num = 1;
				}
				else
				{
					itemValue = new ItemValue(item.type, true);
					num = global::UnityEngine.Random.Range(num2, num3);
				}
			}
		}
		LocalPlayerUI.primaryUI.xui.PlayerInventory.AddItem(new ItemStack(itemValue, num));
	}

	// Token: 0x04000E50 RID: 3664
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
