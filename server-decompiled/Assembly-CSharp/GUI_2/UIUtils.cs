using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

namespace GUI_2
{
	// Token: 0x020016EA RID: 5866
	public static class UIUtils
	{
		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x0600B750 RID: 46928 RVA: 0x0043E6C9 File Offset: 0x0043C8C9
		public static INGUIAtlas IconAtlas
		{
			get
			{
				return UIUtils.symbolAtlas;
			}
		}

		// Token: 0x0600B751 RID: 46929 RVA: 0x0043E6D0 File Offset: 0x0043C8D0
		public static string GetSpriteName(UIUtils.ButtonIcon _icon)
		{
			if (PlayerInputManager.InputStyleFromSelectedIconStyle() != PlayerInputManager.InputStyle.PS4)
			{
				return UIUtils.buttonIconMapXB[_icon];
			}
			return UIUtils.buttonIconMapPS[_icon];
		}

		// Token: 0x0600B752 RID: 46930 RVA: 0x0043E6F4 File Offset: 0x0043C8F4
		public static UIUtils.ButtonIcon GetButtonIconForAction(PlayerAction _action)
		{
			if (_action == null)
			{
				return UIUtils.ButtonIcon.None;
			}
			DeviceBindingSource deviceBindingSource = _action.GetBindingOfType(true) as DeviceBindingSource;
			if (deviceBindingSource == null)
			{
				if (UIUtils.loggedMissingBindingSources.Add(_action))
				{
					Log.Warning("UIUtils: No device binding source could be found for PlayerAction {0}", new object[] { _action.Name });
				}
				return UIUtils.ButtonIcon.None;
			}
			UIUtils.ButtonIcon buttonIcon;
			if (UIUtils.iconControlMap.TryGetValue(deviceBindingSource.Control, out buttonIcon))
			{
				return buttonIcon;
			}
			Log.Warning("UIUtils: Could not assign a ButtonIcon for device control {0}", new object[] { deviceBindingSource.Control.ToString() });
			return UIUtils.ButtonIcon.None;
		}

		// Token: 0x0600B753 RID: 46931 RVA: 0x0043E785 File Offset: 0x0043C985
		public static void LoadAtlas()
		{
			UIUtils.symbolAtlas = Resources.Load<UIAtlas>("GUI/Prefabs/SymbolAtlas");
		}

		// Token: 0x040088E7 RID: 35047
		[PublicizedFrom(EAccessModifier.Private)]
		public static INGUIAtlas symbolAtlas;

		// Token: 0x040088E8 RID: 35048
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<UIUtils.ButtonIcon, string> buttonIconMapPS = new EnumDictionary<UIUtils.ButtonIcon, string>
		{
			{
				UIUtils.ButtonIcon.FaceButtonSouth,
				"PS5_Button_Action1"
			},
			{
				UIUtils.ButtonIcon.FaceButtonNorth,
				"PS5_Button_Action4"
			},
			{
				UIUtils.ButtonIcon.FaceButtonEast,
				"PS5_Button_Action2"
			},
			{
				UIUtils.ButtonIcon.FaceButtonWest,
				"PS5_Button_Action3"
			},
			{
				UIUtils.ButtonIcon.ConfirmButton,
				"PS5_Button_Action1"
			},
			{
				UIUtils.ButtonIcon.CancelButton,
				"PS5_Button_Action2"
			},
			{
				UIUtils.ButtonIcon.LeftBumper,
				"PS5_Button_LeftBumper"
			},
			{
				UIUtils.ButtonIcon.LeftTrigger,
				"PS5_Button_LeftTrigger"
			},
			{
				UIUtils.ButtonIcon.RightBumper,
				"PS5_Button_RightBumper"
			},
			{
				UIUtils.ButtonIcon.RightTrigger,
				"PS5_Button_RightTrigger"
			},
			{
				UIUtils.ButtonIcon.LeftStick,
				"PS5_Button_LeftStick"
			},
			{
				UIUtils.ButtonIcon.LeftStickUpDown,
				"PS5_Button_LeftStickUpDown"
			},
			{
				UIUtils.ButtonIcon.LeftStickLeftRight,
				"PS5_Button_LeftStickLeftRight"
			},
			{
				UIUtils.ButtonIcon.LeftStickButton,
				"PS5_Button_LeftStickButton"
			},
			{
				UIUtils.ButtonIcon.LeftStickLeft,
				"PS5_Button_LeftStickLeft"
			},
			{
				UIUtils.ButtonIcon.LeftStickRight,
				"PS5_Button_LeftStickRight"
			},
			{
				UIUtils.ButtonIcon.LeftStickDown,
				"PS5_Button_LeftStickDown"
			},
			{
				UIUtils.ButtonIcon.LeftStickUp,
				"PS5_Button_LeftStickUp"
			},
			{
				UIUtils.ButtonIcon.RightStick,
				"PS5_Button_RightStick"
			},
			{
				UIUtils.ButtonIcon.RightStickUpDown,
				"PS5_Button_RightStickUpDown"
			},
			{
				UIUtils.ButtonIcon.RightStickLeftRight,
				"PS5_Button_RightStickLeftRight"
			},
			{
				UIUtils.ButtonIcon.RightStickButton,
				"PS5_Button_RightStickButton"
			},
			{
				UIUtils.ButtonIcon.RightStickLeft,
				"PS5_Button_RightStickLeft"
			},
			{
				UIUtils.ButtonIcon.RightStickRight,
				"PS5_Button_RightStickRight"
			},
			{
				UIUtils.ButtonIcon.RightStickDown,
				"PS5_Button_RightStickDown"
			},
			{
				UIUtils.ButtonIcon.RightStickUp,
				"PS5_Button_RightStickUp"
			},
			{
				UIUtils.ButtonIcon.DPadLeft,
				"PS5_Button_DPadLeft"
			},
			{
				UIUtils.ButtonIcon.DPadRight,
				"PS5_Button_DPadRight"
			},
			{
				UIUtils.ButtonIcon.DPadUp,
				"PS5_Button_DPadUp"
			},
			{
				UIUtils.ButtonIcon.DPadDown,
				"PS5_Button_DPadDown"
			},
			{
				UIUtils.ButtonIcon.StartButton,
				"PS5_Button_Start"
			},
			{
				UIUtils.ButtonIcon.BackButton,
				"PS5_Button_Back"
			},
			{
				UIUtils.ButtonIcon.None,
				""
			}
		};

		// Token: 0x040088E9 RID: 35049
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<UIUtils.ButtonIcon, string> buttonIconMapXB = new EnumDictionary<UIUtils.ButtonIcon, string>
		{
			{
				UIUtils.ButtonIcon.FaceButtonSouth,
				"XB_Button_Action1"
			},
			{
				UIUtils.ButtonIcon.FaceButtonNorth,
				"XB_Button_Action4"
			},
			{
				UIUtils.ButtonIcon.FaceButtonEast,
				"XB_Button_Action2"
			},
			{
				UIUtils.ButtonIcon.FaceButtonWest,
				"XB_Button_Action3"
			},
			{
				UIUtils.ButtonIcon.ConfirmButton,
				"XB_Button_Action1"
			},
			{
				UIUtils.ButtonIcon.CancelButton,
				"XB_Button_Action2"
			},
			{
				UIUtils.ButtonIcon.LeftBumper,
				"XB_Button_LeftBumper"
			},
			{
				UIUtils.ButtonIcon.LeftTrigger,
				"XB_Button_LeftTrigger"
			},
			{
				UIUtils.ButtonIcon.RightBumper,
				"XB_Button_RightBumper"
			},
			{
				UIUtils.ButtonIcon.RightTrigger,
				"XB_Button_RightTrigger"
			},
			{
				UIUtils.ButtonIcon.LeftStick,
				"XB_Button_LeftStick"
			},
			{
				UIUtils.ButtonIcon.LeftStickUpDown,
				"XB_Button_LeftStickUpDown"
			},
			{
				UIUtils.ButtonIcon.LeftStickLeftRight,
				"XB_Button_LeftStickLeftRight"
			},
			{
				UIUtils.ButtonIcon.LeftStickButton,
				"XB_Button_LeftStickButton"
			},
			{
				UIUtils.ButtonIcon.LeftStickLeft,
				"XB_Button_LeftStickLeft"
			},
			{
				UIUtils.ButtonIcon.LeftStickRight,
				"XB_Button_LeftStickRight"
			},
			{
				UIUtils.ButtonIcon.LeftStickDown,
				"XB_Button_LeftStickDown"
			},
			{
				UIUtils.ButtonIcon.LeftStickUp,
				"XB_Button_LeftStickUp"
			},
			{
				UIUtils.ButtonIcon.RightStick,
				"XB_Button_RightStick"
			},
			{
				UIUtils.ButtonIcon.RightStickUpDown,
				"XB_Button_RightStickUpDown"
			},
			{
				UIUtils.ButtonIcon.RightStickLeftRight,
				"XB_Button_RightStickLeftRight"
			},
			{
				UIUtils.ButtonIcon.RightStickButton,
				"XB_Button_RightStickButton"
			},
			{
				UIUtils.ButtonIcon.RightStickLeft,
				"XB_Button_RightStickLeft"
			},
			{
				UIUtils.ButtonIcon.RightStickRight,
				"XB_Button_RightStickRight"
			},
			{
				UIUtils.ButtonIcon.RightStickDown,
				"XB_Button_RightStickDown"
			},
			{
				UIUtils.ButtonIcon.RightStickUp,
				"XB_Button_RightStickUp"
			},
			{
				UIUtils.ButtonIcon.DPadLeft,
				"XB_Button_DPadLeft"
			},
			{
				UIUtils.ButtonIcon.DPadRight,
				"XB_Button_DPadRight"
			},
			{
				UIUtils.ButtonIcon.DPadUp,
				"XB_Button_DPadUp"
			},
			{
				UIUtils.ButtonIcon.DPadDown,
				"XB_Button_DPadDown"
			},
			{
				UIUtils.ButtonIcon.StartButton,
				"XB_Button_Menu"
			},
			{
				UIUtils.ButtonIcon.BackButton,
				"XB_Button_Back"
			},
			{
				UIUtils.ButtonIcon.None,
				""
			}
		};

		// Token: 0x040088EA RID: 35050
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<InputControlType, UIUtils.ButtonIcon> iconControlMap = new EnumDictionary<InputControlType, UIUtils.ButtonIcon>
		{
			{
				InputControlType.Action1,
				UIUtils.ButtonIcon.FaceButtonSouth
			},
			{
				InputControlType.Action2,
				UIUtils.ButtonIcon.FaceButtonEast
			},
			{
				InputControlType.Action3,
				UIUtils.ButtonIcon.FaceButtonWest
			},
			{
				InputControlType.Action4,
				UIUtils.ButtonIcon.FaceButtonNorth
			},
			{
				InputControlType.LeftBumper,
				UIUtils.ButtonIcon.LeftBumper
			},
			{
				InputControlType.RightBumper,
				UIUtils.ButtonIcon.RightBumper
			},
			{
				InputControlType.LeftTrigger,
				UIUtils.ButtonIcon.LeftTrigger
			},
			{
				InputControlType.RightTrigger,
				UIUtils.ButtonIcon.RightTrigger
			},
			{
				InputControlType.LeftStickButton,
				UIUtils.ButtonIcon.LeftStickButton
			},
			{
				InputControlType.LeftStickX,
				UIUtils.ButtonIcon.LeftStickLeftRight
			},
			{
				InputControlType.LeftStickY,
				UIUtils.ButtonIcon.LeftStickUpDown
			},
			{
				InputControlType.LeftStickLeft,
				UIUtils.ButtonIcon.LeftStickLeft
			},
			{
				InputControlType.LeftStickRight,
				UIUtils.ButtonIcon.LeftStickRight
			},
			{
				InputControlType.LeftStickDown,
				UIUtils.ButtonIcon.LeftStickDown
			},
			{
				InputControlType.LeftStickUp,
				UIUtils.ButtonIcon.LeftStickUp
			},
			{
				InputControlType.RightStickButton,
				UIUtils.ButtonIcon.RightStickButton
			},
			{
				InputControlType.RightStickX,
				UIUtils.ButtonIcon.RightStickLeftRight
			},
			{
				InputControlType.RightStickY,
				UIUtils.ButtonIcon.RightStickUpDown
			},
			{
				InputControlType.RightStickLeft,
				UIUtils.ButtonIcon.RightStickLeft
			},
			{
				InputControlType.RightStickRight,
				UIUtils.ButtonIcon.RightStickRight
			},
			{
				InputControlType.RightStickDown,
				UIUtils.ButtonIcon.RightStickDown
			},
			{
				InputControlType.RightStickUp,
				UIUtils.ButtonIcon.RightStickUp
			},
			{
				InputControlType.DPadUp,
				UIUtils.ButtonIcon.DPadUp
			},
			{
				InputControlType.DPadDown,
				UIUtils.ButtonIcon.DPadDown
			},
			{
				InputControlType.DPadLeft,
				UIUtils.ButtonIcon.DPadLeft
			},
			{
				InputControlType.DPadRight,
				UIUtils.ButtonIcon.DPadRight
			},
			{
				InputControlType.Start,
				UIUtils.ButtonIcon.StartButton
			},
			{
				InputControlType.Menu,
				UIUtils.ButtonIcon.StartButton
			},
			{
				InputControlType.Options,
				UIUtils.ButtonIcon.StartButton
			},
			{
				InputControlType.Plus,
				UIUtils.ButtonIcon.StartButton
			},
			{
				InputControlType.Select,
				UIUtils.ButtonIcon.BackButton
			},
			{
				InputControlType.View,
				UIUtils.ButtonIcon.BackButton
			},
			{
				InputControlType.TouchPadButton,
				UIUtils.ButtonIcon.BackButton
			},
			{
				InputControlType.Minus,
				UIUtils.ButtonIcon.BackButton
			},
			{
				InputControlType.None,
				UIUtils.ButtonIcon.None
			}
		};

		// Token: 0x040088EB RID: 35051
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<PlayerAction> loggedMissingBindingSources = new HashSet<PlayerAction>();

		// Token: 0x020016EB RID: 5867
		public enum ButtonIcon
		{
			// Token: 0x040088ED RID: 35053
			FaceButtonSouth,
			// Token: 0x040088EE RID: 35054
			FaceButtonNorth,
			// Token: 0x040088EF RID: 35055
			FaceButtonEast,
			// Token: 0x040088F0 RID: 35056
			FaceButtonWest,
			// Token: 0x040088F1 RID: 35057
			ConfirmButton,
			// Token: 0x040088F2 RID: 35058
			CancelButton,
			// Token: 0x040088F3 RID: 35059
			LeftBumper,
			// Token: 0x040088F4 RID: 35060
			RightBumper,
			// Token: 0x040088F5 RID: 35061
			LeftTrigger,
			// Token: 0x040088F6 RID: 35062
			RightTrigger,
			// Token: 0x040088F7 RID: 35063
			LeftStick,
			// Token: 0x040088F8 RID: 35064
			LeftStickUpDown,
			// Token: 0x040088F9 RID: 35065
			LeftStickLeftRight,
			// Token: 0x040088FA RID: 35066
			LeftStickButton,
			// Token: 0x040088FB RID: 35067
			LeftStickLeft,
			// Token: 0x040088FC RID: 35068
			LeftStickRight,
			// Token: 0x040088FD RID: 35069
			LeftStickUp,
			// Token: 0x040088FE RID: 35070
			LeftStickDown,
			// Token: 0x040088FF RID: 35071
			RightStick,
			// Token: 0x04008900 RID: 35072
			RightStickUpDown,
			// Token: 0x04008901 RID: 35073
			RightStickLeftRight,
			// Token: 0x04008902 RID: 35074
			RightStickButton,
			// Token: 0x04008903 RID: 35075
			RightStickLeft,
			// Token: 0x04008904 RID: 35076
			RightStickRight,
			// Token: 0x04008905 RID: 35077
			RightStickUp,
			// Token: 0x04008906 RID: 35078
			RightStickDown,
			// Token: 0x04008907 RID: 35079
			DPadLeft,
			// Token: 0x04008908 RID: 35080
			DPadRight,
			// Token: 0x04008909 RID: 35081
			DPadUp,
			// Token: 0x0400890A RID: 35082
			DPadDown,
			// Token: 0x0400890B RID: 35083
			StartButton,
			// Token: 0x0400890C RID: 35084
			BackButton,
			// Token: 0x0400890D RID: 35085
			None,
			// Token: 0x0400890E RID: 35086
			Count
		}
	}
}
