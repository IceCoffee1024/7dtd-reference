using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002DF RID: 735
[Preserve]
public class DialogActionAddItem : BaseDialogAction
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001542 RID: 5442 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddItem;
		}
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x0007FBB4 File Offset: 0x0007DDB4
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

	// Token: 0x04000E45 RID: 3653
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
