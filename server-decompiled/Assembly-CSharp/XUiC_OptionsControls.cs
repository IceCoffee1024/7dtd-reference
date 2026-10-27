using System;
using System.Collections.Generic;
using InControl;
using UnityEngine.Scripting;

// Token: 0x02000DBE RID: 3518
[UnityEngine.Scripting.Preserve]
public class XUiC_OptionsControls : XUiC_OptionsControlsBase
{
	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x002A0084 File Offset: 0x0029E284
	[XuiXmlBinding("mouse_sensitivity_min")]
	public float MouseSensitivityMin
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x000D786A File Offset: 0x000D5A6A
	[XuiXmlBinding("mouse_sensitivity_max")]
	public float MouseSensitivityMax
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x06006AB2 RID: 27314 RVA: 0x002A008B File Offset: 0x0029E28B
	public override void Init()
	{
		base.Init();
		XUiC_OptionsControls.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006AB3 RID: 27315 RVA: 0x002A00A4 File Offset: 0x0029E2A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createControlsEntries()
	{
		SortedDictionary<PlayerActionData.ActionTab, SortedDictionary<PlayerActionData.ActionGroup, List<PlayerAction>>> sortedDictionary = new SortedDictionary<PlayerActionData.ActionTab, SortedDictionary<PlayerActionData.ActionGroup, List<PlayerAction>>>();
		PlayerActionsBase[] array = new PlayerActionsBase[]
		{
			this.xui.playerUI.playerInput,
			this.xui.playerUI.playerInput.VehicleActions,
			this.xui.playerUI.playerInput.PermanentActions,
			this.xui.playerUI.playerInput.GUIActions,
			PlayerActionsGlobal.Instance
		};
		for (int i = 0; i < array.Length; i++)
		{
			foreach (PlayerAction playerAction in array[i].Actions)
			{
				PlayerActionData.ActionUserData actionUserData = playerAction.UserData as PlayerActionData.ActionUserData;
				if (actionUserData != null && !actionUserData.doNotDisplay)
				{
					PlayerActionData.EAppliesToInputType appliesToInputType = actionUserData.appliesToInputType;
					if (appliesToInputType != PlayerActionData.EAppliesToInputType.None && appliesToInputType != PlayerActionData.EAppliesToInputType.ControllerOnly)
					{
						SortedDictionary<PlayerActionData.ActionGroup, List<PlayerAction>> sortedDictionary2;
						if (!sortedDictionary.TryGetValue(actionUserData.actionGroup.actionTab, out sortedDictionary2))
						{
							sortedDictionary2 = new SortedDictionary<PlayerActionData.ActionGroup, List<PlayerAction>>();
							sortedDictionary.Add(actionUserData.actionGroup.actionTab, sortedDictionary2);
						}
						List<PlayerAction> list;
						if (!sortedDictionary2.TryGetValue(actionUserData.actionGroup, out list))
						{
							list = new List<PlayerAction>();
							sortedDictionary2.Add(actionUserData.actionGroup, list);
						}
						list.Add(playerAction);
					}
				}
			}
		}
		int num = 1;
		foreach (KeyValuePair<PlayerActionData.ActionTab, SortedDictionary<PlayerActionData.ActionGroup, List<PlayerAction>>> keyValuePair in sortedDictionary)
		{
			this.TabSelector.SetTabCaption(num, Localization.Get(keyValuePair.Key.tabNameKey, false, null));
			XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.GetTab(num).GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
			int num2 = 0;
			int num3 = 0;
			foreach (KeyValuePair<PlayerActionData.ActionGroup, List<PlayerAction>> keyValuePair2 in keyValuePair.Value)
			{
				if (num2 > 0)
				{
					childControllers[num3].Action = null;
					num3++;
				}
				num2++;
				foreach (PlayerAction playerAction2 in keyValuePair2.Value)
				{
					childControllers[num3].Action = playerAction2;
					num3++;
				}
			}
			num++;
		}
		this.IsDirty = true;
	}

	// Token: 0x040051B3 RID: 20915
	public static string ID = "";
}
