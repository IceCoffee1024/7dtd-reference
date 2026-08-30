using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D8E RID: 3470
[UnityEngine.Scripting.Preserve]
public class XUiC_OptionsController : XUiC_OptionsControlsBase
{
	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x060069DF RID: 27103 RVA: 0x00297CCE File Offset: 0x00295ECE
	[XuiXmlBinding("isds4")]
	public bool ShowDs4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DeviceFlag.PS5.IsCurrent() || (this.comboShowDS4 != null && this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x060069E0 RID: 27104 RVA: 0x00297CF0 File Offset: 0x00295EF0
	[XuiXmlBinding("isxb1")]
	public bool ShowXb1
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent() || (this.comboShowDS4 != null && !this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x060069E1 RID: 27105 RVA: 0x00297D15 File Offset: 0x00295F15
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

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x060069E2 RID: 27106 RVA: 0x00297D2A File Offset: 0x00295F2A
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

	// Token: 0x060069E3 RID: 27107 RVA: 0x00297D3F File Offset: 0x00295F3F
	[XuiBindEvent("OnValueChanged", "comboShowDS4")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowDs4_OnOnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x060069E4 RID: 27108 RVA: 0x00297D3F File Offset: 0x00295F3F
	[XuiBindEvent("OnValueChanged", "comboShowBindingsFor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowMenuBindings_OnOnValueChanged(XUiController _sender, string _s, string _newValue1)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x060069E5 RID: 27109 RVA: 0x00297D50 File Offset: 0x00295F50
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

	// Token: 0x060069E6 RID: 27110 RVA: 0x00297F18 File Offset: 0x00296118
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

	// Token: 0x060069E7 RID: 27111 RVA: 0x00297F58 File Offset: 0x00296158
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

	// Token: 0x060069E8 RID: 27112 RVA: 0x00297F90 File Offset: 0x00296190
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

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x060069E9 RID: 27113 RVA: 0x0026E35F File Offset: 0x0026C55F
	[XuiXmlBinding("cursor_sensitivity_min")]
	public float CursorSensitivityMin
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x060069EA RID: 27114 RVA: 0x00040D24 File Offset: 0x0003EF24
	[XuiXmlBinding("cursor_sensitivity_max")]
	public float CursorSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x060069EB RID: 27115 RVA: 0x002982D8 File Offset: 0x002964D8
	[XuiXmlBinding("controller_sensitivity_min")]
	public float ControllerSensitivityMin
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x060069EC RID: 27116 RVA: 0x00040D24 File Offset: 0x0003EF24
	[XuiXmlBinding("controller_sensitivity_max")]
	public float ControllerSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x060069ED RID: 27117 RVA: 0x002982DF File Offset: 0x002964DF
	[XuiXmlBinding("controller_modifier_sensitivity_max")]
	public float ControllerModifierSensitivityMax
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x060069EE RID: 27118 RVA: 0x002982E6 File Offset: 0x002964E6
	public override void Init()
	{
		base.Init();
		XUiC_OptionsController.ID = base.WindowGroup.Id;
		this.initControllerLayout();
	}

	// Token: 0x060069EF RID: 27119 RVA: 0x00298304 File Offset: 0x00296504
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		base.doResetToDefaultsInternal();
		if (this.comboIconStyle != null)
		{
			this.comboIconStyle.Value = ((PlayerInputManager.ControllerIconStyle)GamePrefs.GetDefault(EnumGamePrefs.OptionsControllerIconStyle)).ToString();
		}
		this.updateControllerMappingLabels();
	}

	// Token: 0x060069F0 RID: 27120 RVA: 0x00298350 File Offset: 0x00296550
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

	// Token: 0x060069F1 RID: 27121 RVA: 0x002985A4 File Offset: 0x002967A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateOptions()
	{
		if (this.comboIconStyle != null)
		{
			this.comboIconStyle.Value = ((PlayerInputManager.ControllerIconStyle)GamePrefs.GetInt(EnumGamePrefs.OptionsControllerIconStyle)).ToString();
		}
	}

	// Token: 0x060069F2 RID: 27122 RVA: 0x002985DC File Offset: 0x002967DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		if (this.comboIconStyle != null)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsControllerIconStyle, this.comboIconStyle.SelectedIndex);
		}
	}

	// Token: 0x060069F3 RID: 27123 RVA: 0x00298601 File Offset: 0x00296801
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.updateControllerMappingLabels();
		PlatformManager.NativePlatform.Input.ForceInputStyleChange();
		this.xui.CalloutWindow.ForceInputStyleChange(base.CurrentInputStyle, base.CurrentInputStyle);
	}

	// Token: 0x060069F4 RID: 27124 RVA: 0x0029863C File Offset: 0x0029683C
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateOptions();
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

	// Token: 0x040050A9 RID: 20649
	[XuiBindComponent("ShowDS4", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboShowDS4;

	// Token: 0x040050AA RID: 20650
	[XuiBindComponent("ShowBindingsFor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboShowBindingsFor;

	// Token: 0x040050AB RID: 20651
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetInGame;

	// Token: 0x040050AC RID: 20652
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetVehicles;

	// Token: 0x040050AD RID: 20653
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetMenu;

	// Token: 0x040050AE RID: 20654
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetPermanent;

	// Token: 0x040050AF RID: 20655
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_OptionsController.ControllerLabelMapping> labelsForControllers = new List<XUiC_OptionsController.ControllerLabelMapping>();

	// Token: 0x040050B0 RID: 20656
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping leftStickLabel;

	// Token: 0x040050B1 RID: 20657
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping rightStickLabel;

	// Token: 0x040050B2 RID: 20658
	public static string ID = "";

	// Token: 0x040050B3 RID: 20659
	[XuiBindComponent("ControllerIconStyle", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboIconStyle;

	// Token: 0x02000D8F RID: 3471
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ControllerLabelMapping
	{
		// Token: 0x060069F7 RID: 27127 RVA: 0x002986B8 File Offset: 0x002968B8
		public ControllerLabelMapping(string[] _controlTypeNames, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[_controlTypeNames.Length];
			for (int i = 0; i < _controlTypeNames.Length; i++)
			{
				this.ControlTypes[i] = EnumUtils.Parse<InputControlType>(_controlTypeNames[i], true);
			}
			this.Labels = _labels;
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x002986F9 File Offset: 0x002968F9
		public ControllerLabelMapping(string _controlTypeName, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[] { EnumUtils.Parse<InputControlType>(_controlTypeName, true) };
			this.Labels = _labels;
		}

		// Token: 0x040050B4 RID: 20660
		public readonly InputControlType[] ControlTypes;

		// Token: 0x040050B5 RID: 20661
		public readonly XUiV_Label[] Labels;
	}
}
