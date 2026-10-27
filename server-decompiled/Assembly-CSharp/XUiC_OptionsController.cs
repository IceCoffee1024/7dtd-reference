using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DBC RID: 3516
[UnityEngine.Scripting.Preserve]
public class XUiC_OptionsController : XUiC_OptionsControlsBase
{
	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x06006A98 RID: 27288 RVA: 0x0029F6DA File Offset: 0x0029D8DA
	[XuiXmlBinding("isds4")]
	public bool ShowDs4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DeviceFlag.PS5.IsCurrent() || (this.comboShowDS4 != null && this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x06006A99 RID: 27289 RVA: 0x0029F6FC File Offset: 0x0029D8FC
	[XuiXmlBinding("isxb1")]
	public bool ShowXb1
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent() || (this.comboShowDS4 != null && !this.comboShowDS4.Value);
		}
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06006A9A RID: 27290 RVA: 0x0029F721 File Offset: 0x0029D921
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

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06006A9B RID: 27291 RVA: 0x0029F736 File Offset: 0x0029D936
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

	// Token: 0x06006A9C RID: 27292 RVA: 0x0029F74B File Offset: 0x0029D94B
	[XuiBindEvent("OnValueChanged", "comboShowDS4")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowDs4_OnOnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A9D RID: 27293 RVA: 0x0029F74B File Offset: 0x0029D94B
	[XuiBindEvent("OnValueChanged", "comboShowBindingsFor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboShowMenuBindings_OnOnValueChanged(XUiController _sender, string _s, string _newValue1)
	{
		this.IsDirty = true;
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006A9E RID: 27294 RVA: 0x0029F75C File Offset: 0x0029D95C
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

	// Token: 0x06006A9F RID: 27295 RVA: 0x0029F924 File Offset: 0x0029DB24
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

	// Token: 0x06006AA0 RID: 27296 RVA: 0x0029F964 File Offset: 0x0029DB64
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

	// Token: 0x06006AA1 RID: 27297 RVA: 0x0029F99C File Offset: 0x0029DB9C
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

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x00275B87 File Offset: 0x00273D87
	[XuiXmlBinding("cursor_sensitivity_min")]
	public float CursorSensitivityMin
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x00041024 File Offset: 0x0003F224
	[XuiXmlBinding("cursor_sensitivity_max")]
	public float CursorSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x0029FCE4 File Offset: 0x0029DEE4
	[XuiXmlBinding("controller_sensitivity_min")]
	public float ControllerSensitivityMin
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x00041024 File Offset: 0x0003F224
	[XuiXmlBinding("controller_sensitivity_max")]
	public float ControllerSensitivityMax
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x0029FCEB File Offset: 0x0029DEEB
	[XuiXmlBinding("controller_modifier_sensitivity_max")]
	public float ControllerModifierSensitivityMax
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06006AA7 RID: 27303 RVA: 0x0029FCF2 File Offset: 0x0029DEF2
	public override void Init()
	{
		base.Init();
		XUiC_OptionsController.ID = base.WindowGroup.Id;
		this.initControllerLayout();
	}

	// Token: 0x06006AA8 RID: 27304 RVA: 0x0029FD10 File Offset: 0x0029DF10
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		base.doResetToDefaultsInternal();
		this.updateControllerMappingLabels();
	}

	// Token: 0x06006AA9 RID: 27305 RVA: 0x0029FD20 File Offset: 0x0029DF20
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

	// Token: 0x06006AAA RID: 27306 RVA: 0x0029FF74 File Offset: 0x0029E174
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.updateControllerMappingLabels();
		PlatformManager.NativePlatform.Input.ForceInputStyleChange();
		this.xui.CalloutWindow.ForceInputStyleChange(base.CurrentInputStyle, base.CurrentInputStyle);
	}

	// Token: 0x06006AAB RID: 27307 RVA: 0x0029FFB0 File Offset: 0x0029E1B0
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

	// Token: 0x040051A7 RID: 20903
	[XuiBindComponent("ShowDS4", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool comboShowDS4;

	// Token: 0x040051A8 RID: 20904
	[XuiBindComponent("ShowBindingsFor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboShowBindingsFor;

	// Token: 0x040051A9 RID: 20905
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetInGame;

	// Token: 0x040051AA RID: 20906
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetVehicles;

	// Token: 0x040051AB RID: 20907
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetMenu;

	// Token: 0x040051AC RID: 20908
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerActionsBase actionSetPermanent;

	// Token: 0x040051AD RID: 20909
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_OptionsController.ControllerLabelMapping> labelsForControllers = new List<XUiC_OptionsController.ControllerLabelMapping>();

	// Token: 0x040051AE RID: 20910
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping leftStickLabel;

	// Token: 0x040051AF RID: 20911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsController.ControllerLabelMapping rightStickLabel;

	// Token: 0x040051B0 RID: 20912
	public static string ID = "";

	// Token: 0x02000DBD RID: 3517
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ControllerLabelMapping
	{
		// Token: 0x06006AAE RID: 27310 RVA: 0x002A0024 File Offset: 0x0029E224
		public ControllerLabelMapping(string[] _controlTypeNames, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[_controlTypeNames.Length];
			for (int i = 0; i < _controlTypeNames.Length; i++)
			{
				this.ControlTypes[i] = EnumUtils.Parse<InputControlType>(_controlTypeNames[i], true);
			}
			this.Labels = _labels;
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x002A0065 File Offset: 0x0029E265
		public ControllerLabelMapping(string _controlTypeName, XUiV_Label[] _labels)
		{
			this.ControlTypes = new InputControlType[] { EnumUtils.Parse<InputControlType>(_controlTypeName, true) };
			this.Labels = _labels;
		}

		// Token: 0x040051B1 RID: 20913
		public readonly InputControlType[] ControlTypes;

		// Token: 0x040051B2 RID: 20914
		public readonly XUiV_Label[] Labels;
	}
}
