using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DA2 RID: 3490
[UnityEngine.Scripting.Preserve]
public class XUiC_OptionsController : XUiC_OptionsControlsBase
{
	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x06006A7B RID: 27259 RVA: 0x0029AE22 File Offset: 0x00299022
	[XuiXmlBinding("isds4")]
	public bool ShowDs4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DeviceFlag.PS5.IsCurrent() || (this.comboShowDS4 != null && this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x06006A7C RID: 27260 RVA: 0x0029AE44 File Offset: 0x00299044
	[XuiXmlBinding("isxb1")]
	public bool ShowXb1
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent() || (this.comboShowDS4 != null && !this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x06006A7D RID: 27261 RVA: 0x0029AE69 File Offset: 0x00299069
	[XuiXmlBinding("controller_art")]
	public string ControllerArtSprite
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.ShowDs4)
			{
				return "Controller_Art_XB";
			}
			return "Controller_Art_PS5";
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06006A7E RID: 27262 RVA: 0x0029AE7E File Offset: 0x0029907E
	[XuiXmlBinding("controller_lines")]
	public string ControllerLinesSprite
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.ShowDs4)
			{
				return "Controller_Lines_XB";
			}
			return "Controller_Lines_PS5";
		}
	}

	// Token: 0x06006A7F RID: 27263 RVA: 0x0029AE93 File Offset: 0x00299093
	[XuiBindEvent("OnValueChanged", "comboShowDS4")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowDs4_OnOnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A80 RID: 27264 RVA: 0x0029AE93 File Offset: 0x00299093
	[XuiBindEvent("OnValueChanged", "comboShowBindingsFor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowMenuBindings_OnOnValueChanged(XUiController _sender, string _s, string _newValue1)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A81 RID: 27265 RVA: 0x0029AEA4 File Offset: 0x002990A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void initControllerLayout()
	{
		this.actionSetInGame = this.xui.playerUI.playerInput;
		this.actionSetVehicles = this.xui.playerUI.playerInput.VehicleActions;
		this.actionSetMenu = this.xui.playerUI.playerInput.GUIActions;
		this.actionSetPermanent = this.xui.playerUI.playerInput.PermanentActions;
		XUiController xuiController;
		base.TryGetChildController<XUiController>("controllerlayout", out xuiController);
		this.addControllerLabelMappingsForButton(xuiController, "Menu", null);
		this.addControllerLabelMappingsForButton(xuiController, "RightTrigger", null);
		this.addControllerLabelMappingsForButton(xuiController, "RightBumper", null);
		this.addControllerLabelMappingsForButton(xuiController, "Action4", null);
		this.addControllerLabelMappingsForButton(xuiController, "Action3", null);
		this.addControllerLabelMappingsForButton(xuiController, "Action2", null);
		this.addControllerLabelMappingsForButton(xuiController, "Action1", null);
		this.addControllerLabelMappingsForButton(xuiController, "RightStickButton", null);
		this.addControllerLabelMappingsForButton(xuiController, "View", null);
		this.addControllerLabelMappingsForButton(xuiController, "LeftTrigger", null);
		this.addControllerLabelMappingsForButton(xuiController, "LeftBumper", null);
		this.addControllerLabelMappingsForButton(xuiController, "LeftStickButton", null);
		this.addControllerLabelMappingsForButton(xuiController, "DPadUp", null);
		this.addControllerLabelMappingsForButton(xuiController, "DPadLeft", null);
		this.addControllerLabelMappingsForButton(xuiController, "DPadDown", null);
		this.addControllerLabelMappingsForButton(xuiController, "DPadRight", null);
		this.assignControllerLabelMappingsForButton(xuiController, "LeftStick", out this.leftStickLabel, new string[] { "LeftStickLeft", "LeftStickRight", "LeftStickUp", "LeftStickDown" });
		this.assignControllerLabelMappingsForButton(xuiController, "RightStick", out this.rightStickLabel, new string[] { "RightStickLeft", "RightStickRight", "RightStickUp", "RightStickDown" });
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A82 RID: 27266 RVA: 0x0029B06C File Offset: 0x0029926C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addControllerLabelMappingsForButton(XUiController _layoutParent, string _uiName, string[] _controlTypeNames = null)
	{
		XUiV_Label[] childViews = _layoutParent.GetChildViews<XUiV_Label>(_uiName, null);
		if (_controlTypeNames == null)
		{
			this.labelsForControllers.Add(new XUiC_OptionsController.ControllerLabelMapping(_uiName, childViews));
			return;
		}
		this.labelsForControllers.Add(new XUiC_OptionsController.ControllerLabelMapping(_controlTypeNames, childViews));
	}

	// Token: 0x06006A83 RID: 27267 RVA: 0x0029B0AC File Offset: 0x002992AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void assignControllerLabelMappingsForButton(XUiController _layoutParent, string _uiName, out XUiC_OptionsController.ControllerLabelMapping _assignTo, string[] _controlTypeNames = null)
	{
		XUiV_Label[] childViews = _layoutParent.GetChildViews<XUiV_Label>(_uiName, null);
		if (_controlTypeNames == null)
		{
			_assignTo = new XUiC_OptionsController.ControllerLabelMapping(_uiName, childViews);
			return;
		}
		_assignTo = new XUiC_OptionsController.ControllerLabelMapping(_controlTypeNames, childViews);
	}

	// Token: 0x06006A84 RID: 27268 RVA: 0x0029B0E4 File Offset: 0x002992E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateControllerMappingLabels()
	{
		List<PlayerAction> list = new List<PlayerAction>();
		int i = this.comboShowBindingsFor.SelectedIndex;
		PlayerActionsBase playerActionsBase;
		if (i > 0)
		{
			if (i != 1)
			{
				playerActionsBase = this.actionSetMenu;
			}
			else
			{
				playerActionsBase = this.actionSetVehicles;
			}
		}
		else
		{
			playerActionsBase = this.actionSetInGame;
		}
		PlayerActionsBase playerActionsBase2 = playerActionsBase;
		int num = ((this.comboShowDS4 == null || !this.comboShowDS4.Value) ? 0 : 1);
		foreach (XUiC_OptionsController.ControllerLabelMapping controllerLabelMapping in this.labelsForControllers)
		{
			string text = "";
			string text2 = "";
			InputControlType[] controlTypes = controllerLabelMapping.ControlTypes;
			for (i = 0; i < controlTypes.Length; i++)
			{
				controlTypes[i].GetBoundAction(new PlayerActionSet[] { playerActionsBase2, this.actionSetPermanent }, list);
				if (list.Count > 0)
				{
					foreach (PlayerAction playerAction in list)
					{
						PlayerActionData.ActionUserData actionUserData = (PlayerActionData.ActionUserData)playerAction.UserData;
						if (actionUserData != null)
						{
							if ((actionUserData.appliesToInputType == PlayerActionData.EAppliesToInputType.Both || actionUserData.appliesToInputType == PlayerActionData.EAppliesToInputType.ControllerOnly) && !actionUserData.doNotDisplay)
							{
								if (text.Length > 0)
								{
									text += ", ";
									text2 += ", ";
								}
								text += actionUserData.LocalizedName;
								text2 += actionUserData.LocalizedDescription;
							}
						}
						else if (ActionSetManager.DebugLevel > ActionSetManager.EDebugLevel.Off)
						{
							text += " !NULL! ";
							text2 += " !NULL! ";
						}
					}
				}
			}
			if (text.Length == 0)
			{
				text = Localization.Get("inpUnboundControllerKey", false, null);
				text2 = Localization.Get("inpUnboundControllerKeyTooltip", false, null);
			}
			int num2 = 0;
			if (controllerLabelMapping.Labels.Length > 1)
			{
				num2 = num;
			}
			controllerLabelMapping.Labels[num2].Text = text;
			controllerLabelMapping.Labels[num2].ToolTip = text2;
		}
		string text3 = "";
		string text4 = "";
		switch (GamePrefs.GetInt(EnumGamePrefs.OptionsControllerJoystickLayout))
		{
		case 0:
			text3 = Localization.Get("inpStandardLeftStick", false, null);
			text4 = Localization.Get("inpStandardRightStick", false, null);
			break;
		case 1:
			text3 = Localization.Get("inpStandardRightStick", false, null);
			text4 = Localization.Get("inpStandardLeftStick", false, null);
			break;
		case 2:
			text3 = Localization.Get("inpLegacyLeftStick", false, null);
			text4 = Localization.Get("inpLegacyRightStick", false, null);
			break;
		case 3:
			text3 = Localization.Get("inpLegacyRightStick", false, null);
			text4 = Localization.Get("inpLegacyLeftStick", false, null);
			break;
		}
		XUiV_Label[] array = this.leftStickLabel.Labels;
		for (i = 0; i < array.Length; i++)
		{
			array[i].Text = text3;
		}
		array = this.rightStickLabel.Labels;
		for (i = 0; i < array.Length; i++)
		{
			array[i].Text = text4;
		}
	}

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06006A85 RID: 27269 RVA: 0x0027125B File Offset: 0x0026F45B
	[XuiXmlBinding("cursor_sensitivity_min")]
	public float CursorSensitivityMin
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06006A86 RID: 27270 RVA: 0x00040F84 File Offset: 0x0003F184
	[XuiXmlBinding("cursor_sensitivity_max")]
	public float CursorSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06006A87 RID: 27271 RVA: 0x0029B42C File Offset: 0x0029962C
	[XuiXmlBinding("controller_sensitivity_min")]
	public float ControllerSensitivityMin
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06006A88 RID: 27272 RVA: 0x00040F84 File Offset: 0x0003F184
	[XuiXmlBinding("controller_sensitivity_max")]
	public float ControllerSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06006A89 RID: 27273 RVA: 0x0029B433 File Offset: 0x00299633
	[XuiXmlBinding("controller_modifier_sensitivity_max")]
	public float ControllerModifierSensitivityMax
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06006A8A RID: 27274 RVA: 0x0029B43A File Offset: 0x0029963A
	public override void Init()
	{
		base.Init();
		XUiC_OptionsController.ID = base.WindowGroup.Id;
		this.initControllerLayout();
	}

	// Token: 0x06006A8B RID: 27275 RVA: 0x0029B458 File Offset: 0x00299658
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		base.doResetToDefaultsInternal();
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A8C RID: 27276 RVA: 0x0029B468 File Offset: 0x00299668
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createControlsEntries()
	{
		Dictionary<string, List<PlayerAction>> dictionary = new Dictionary<string, List<PlayerAction>>();
		PlayerActionsBase[] array = new PlayerActionsBase[]
		{
			this.xui.playerUI.playerInput,
			this.xui.playerUI.playerInput.VehicleActions,
			this.xui.playerUI.playerInput.PermanentActions
		};
		dictionary.Add("inpTabPlayerOnFoot", new List<PlayerAction>());
		PlayerActionsBase[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			foreach (PlayerAction playerAction in array2[i].ControllerRebindableActions)
			{
				PlayerActionData.ActionUserData actionUserData = playerAction.UserData as PlayerActionData.ActionUserData;
				if (actionUserData != null && !playerAction.Equals(this.xui.playerUI.playerInput.PermanentActions.PushToTalk))
				{
					if (actionUserData.actionGroup.actionTab.tabNameKey == "inpTabPlayerControl")
					{
						dictionary["inpTabPlayerOnFoot"].Add(playerAction);
					}
					else
					{
						List<PlayerAction> list;
						if (!dictionary.TryGetValue(actionUserData.actionGroup.actionTab.tabNameKey, out list))
						{
							list = new List<PlayerAction>();
							dictionary.Add(actionUserData.actionGroup.actionTab.tabNameKey, list);
						}
						list.Add(playerAction);
					}
				}
			}
		}
		dictionary["inpTabPlayerOnFoot"].Add(this.xui.playerUI.playerInput.PermanentActions.PushToTalk);
		int num = 1;
		foreach (KeyValuePair<string, List<PlayerAction>> keyValuePair in dictionary)
		{
			this.TabSelector.SetTabCaption(num, Localization.Get(keyValuePair.Key, false, null));
			XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.GetTab(num).GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
			for (int j = 0; j < childControllers.Length; j++)
			{
				childControllers[j].Action = ((j < keyValuePair.Value.Count) ? keyValuePair.Value[j] : null);
			}
			num++;
		}
	}

	// Token: 0x06006A8D RID: 27277 RVA: 0x0029B6BC File Offset: 0x002998BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.updateControllerMappingLabels();
		PlatformManager.NativePlatform.Input.ForceInputStyleChange();
		this.xui.CalloutWindow.ForceInputStyleChange(base.CurrentInputStyle, base.CurrentInputStyle);
	}

	// Token: 0x06006A8E RID: 27278 RVA: 0x0029B6F8 File Offset: 0x002998F8
	public override void OnOpen()
	{
		base.OnOpen();
		PlayerInputManager.InputStyle currentControllerInputStyle = PlatformManager.NativePlatform.Input.CurrentControllerInputStyle;
		if (currentControllerInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			bool flag = currentControllerInputStyle == PlayerInputManager.InputStyle.PS4;
			if (this.comboShowDS4 != null && flag != this.comboShowDS4.Value)
			{
				this.comboShowDS4.Value = flag;
				this.updateControllerMappingLabels();
			}
		}
	}

	// Token: 0x0400514B RID: 20811
	[XuiBindComponent("ShowDS4", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboShowDS4;

	// Token: 0x0400514C RID: 20812
	[XuiBindComponent("ShowBindingsFor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboShowBindingsFor;

	// Token: 0x0400514D RID: 20813
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetInGame;

	// Token: 0x0400514E RID: 20814
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetVehicles;

	// Token: 0x0400514F RID: 20815
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetMenu;

	// Token: 0x04005150 RID: 20816
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetPermanent;

	// Token: 0x04005151 RID: 20817
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_OptionsController.ControllerLabelMapping> labelsForControllers = new List<XUiC_OptionsController.ControllerLabelMapping>();

	// Token: 0x04005152 RID: 20818
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping leftStickLabel;

	// Token: 0x04005153 RID: 20819
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping rightStickLabel;

	// Token: 0x04005154 RID: 20820
	public static string ID = "";

	// Token: 0x02000DA3 RID: 3491
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ControllerLabelMapping
	{
		// Token: 0x06006A91 RID: 27281 RVA: 0x0029B76C File Offset: 0x0029996C
		public ControllerLabelMapping(string[] _controlTypeNames, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[_controlTypeNames.Length];
			for (int i = 0; i < _controlTypeNames.Length; i++)
			{
				this.ControlTypes[i] = EnumUtils.Parse<InputControlType>(_controlTypeNames[i], true);
			}
			this.Labels = _labels;
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x0029B7AD File Offset: 0x002999AD
		public ControllerLabelMapping(string _controlTypeName, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[] { EnumUtils.Parse<InputControlType>(_controlTypeName, true) };
			this.Labels = _labels;
		}

		// Token: 0x04005155 RID: 20821
		public readonly InputControlType[] ControlTypes;

		// Token: 0x04005156 RID: 20822
		public readonly XUiV_Label[] Labels;
	}
}
