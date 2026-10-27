using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

namespace GUI_2
{
	// Token: 0x02001709 RID: 5897
	public static class UIUtils
	{
		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x0600B824 RID: 47140 RVA: 0x0044116D File Offset: 0x0043F36D
		public static INGUIAtlas IconAtlas
		{
			get
			{
				return UIUtils.symbolAtlas;
			}
		}

		// Token: 0x0600B825 RID: 47141 RVA: 0x00441174 File Offset: 0x0043F374
		public static string GetSpriteName(UIUtils.ButtonIcon _icon)
		{
			if (PlayerInputManager.InputStyleFromSelectedIconStyle() != PlayerInputManager.InputStyle.PS4)
			{
				return UIUtils.buttonIconMapXB[_icon];
			}
			return UIUtils.buttonIconMapPS[_icon];
		}

		// Token: 0x0600B826 RID: 47142 RVA: 0x00441198 File Offset: 0x0043F398
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

		// Token: 0x0600B827 RID: 47143 RVA: 0x00441229 File Offset: 0x0043F429
		public static void LoadAtlas()
		{
			UIUtils.symbolAtlas = Resources.Load<UIAtlas>("GUI/Prefabs/SymbolAtlas");
		}

		// Token: 0x0400899A RID: 35226
		[PublicizedFrom(EAccessModifier.Private)]
		public static INGUIAtlas symbolAtlas;

		// Token: 0x0400899B RID: 35227
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

		// Token: 0x0400899C RID: 35228
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

		// Token: 0x0400899D RID: 35229
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

		// Token: 0x0400899E RID: 35230
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<PlayerAction> loggedMissingBindingSources = new HashSet<PlayerAction>();

		// Token: 0x0200170A RID: 5898
		public enum ButtonIcon
		{
			// Token: 0x040089A0 RID: 35232
			FaceButtonSouth,
			// Token: 0x040089A1 RID: 35233
			FaceButtonNorth,
			// Token: 0x040089A2 RID: 35234
			FaceButtonEast,
			// Token: 0x040089A3 RID: 35235
			FaceButtonWest,
			// Token: 0x040089A4 RID: 35236
			ConfirmButton,
			// Token: 0x040089A5 RID: 35237
			CancelButton,
			// Token: 0x040089A6 RID: 35238
			LeftBumper,
			// Token: 0x040089A7 RID: 35239
			RightBumper,
			// Token: 0x040089A8 RID: 35240
			LeftTrigger,
			// Token: 0x040089A9 RID: 35241
			RightTrigger,
			// Token: 0x040089AA RID: 35242
			LeftStick,
			// Token: 0x040089AB RID: 35243
			LeftStickUpDown,
			// Token: 0x040089AC RID: 35244
			LeftStickLeftRight,
			// Token: 0x040089AD RID: 35245
			LeftStickButton,
			// Token: 0x040089AE RID: 35246
			LeftStickLeft,
			// Token: 0x040089AF RID: 35247
			LeftStickRight,
			// Token: 0x040089B0 RID: 35248
			LeftStickUp,
			// Token: 0x040089B1 RID: 35249
			LeftStickDown,
			// Token: 0x040089B2 RID: 35250
			RightStick,
			// Token: 0x040089B3 RID: 35251
			RightStickUpDown,
			// Token: 0x040089B4 RID: 35252
			RightStickLeftRight,
			// Token: 0x040089B5 RID: 35253
			RightStickButton,
			// Token: 0x040089B6 RID: 35254
			RightStickLeft,
			// Token: 0x040089B7 RID: 35255
			RightStickRight,
			// Token: 0x040089B8 RID: 35256
			RightStickUp,
			// Token: 0x040089B9 RID: 35257
			RightStickDown,
			// Token: 0x040089BA RID: 35258
			DPadLeft,
			// Token: 0x040089BB RID: 35259
			DPadRight,
			// Token: 0x040089BC RID: 35260
			DPadUp,
			// Token: 0x040089BD RID: 35261
			DPadDown,
			// Token: 0x040089BE RID: 35262
			StartButton,
			// Token: 0x040089BF RID: 35263
			BackButton,
			// Token: 0x040089C0 RID: 35264
			None,
			// Token: 0x040089C1 RID: 35265
			Count
		}
	}
}
