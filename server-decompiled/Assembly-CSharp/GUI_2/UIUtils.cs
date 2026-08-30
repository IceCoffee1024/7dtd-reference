using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

namespace GUI_2
{
	// Token: 0x020016BE RID: 5822
	public static class UIUtils
	{
		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x0600B5F9 RID: 46585 RVA: 0x0043A1A9 File Offset: 0x004383A9
		public static INGUIAtlas IconAtlas
		{
			get
			{
				return UIUtils.symbolAtlas;
			}
		}

		// Token: 0x0600B5FA RID: 46586 RVA: 0x0043A1B0 File Offset: 0x004383B0
		public static string GetSpriteName(UIUtils.ButtonIcon _icon)
		{
			if (PlayerInputManager.InputStyleFromSelectedIconStyle() != PlayerInputManager.InputStyle.PS4)
			{
				return UIUtils.buttonIconMapXB[_icon];
			}
			return UIUtils.buttonIconMapPS[_icon];
		}

		// Token: 0x0600B5FB RID: 46587 RVA: 0x0043A1D4 File Offset: 0x004383D4
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

		// Token: 0x0600B5FC RID: 46588 RVA: 0x0043A265 File Offset: 0x00438465
		public static void LoadAtlas()
		{
			UIUtils.symbolAtlas = Resources.Load<UIAtlas>("GUI/Prefabs/SymbolAtlas");
		}

		// Token: 0x04008822 RID: 34850
		[PublicizedFrom(EAccessModifier.Private)]
		public static INGUIAtlas symbolAtlas;

		// Token: 0x04008823 RID: 34851
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

		// Token: 0x04008824 RID: 34852
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

		// Token: 0x04008825 RID: 34853
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

		// Token: 0x04008826 RID: 34854
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<PlayerAction> loggedMissingBindingSources = new HashSet<PlayerAction>();

		// Token: 0x020016BF RID: 5823
		public enum ButtonIcon
		{
			// Token: 0x04008828 RID: 34856
			FaceButtonSouth,
			// Token: 0x04008829 RID: 34857
			FaceButtonNorth,
			// Token: 0x0400882A RID: 34858
			FaceButtonEast,
			// Token: 0x0400882B RID: 34859
			FaceButtonWest,
			// Token: 0x0400882C RID: 34860
			ConfirmButton,
			// Token: 0x0400882D RID: 34861
			CancelButton,
			// Token: 0x0400882E RID: 34862
			LeftBumper,
			// Token: 0x0400882F RID: 34863
			RightBumper,
			// Token: 0x04008830 RID: 34864
			LeftTrigger,
			// Token: 0x04008831 RID: 34865
			RightTrigger,
			// Token: 0x04008832 RID: 34866
			LeftStick,
			// Token: 0x04008833 RID: 34867
			LeftStickUpDown,
			// Token: 0x04008834 RID: 34868
			LeftStickLeftRight,
			// Token: 0x04008835 RID: 34869
			LeftStickButton,
			// Token: 0x04008836 RID: 34870
			LeftStickLeft,
			// Token: 0x04008837 RID: 34871
			LeftStickRight,
			// Token: 0x04008838 RID: 34872
			LeftStickUp,
			// Token: 0x04008839 RID: 34873
			LeftStickDown,
			// Token: 0x0400883A RID: 34874
			RightStick,
			// Token: 0x0400883B RID: 34875
			RightStickUpDown,
			// Token: 0x0400883C RID: 34876
			RightStickLeftRight,
			// Token: 0x0400883D RID: 34877
			RightStickButton,
			// Token: 0x0400883E RID: 34878
			RightStickLeft,
			// Token: 0x0400883F RID: 34879
			RightStickRight,
			// Token: 0x04008840 RID: 34880
			RightStickUp,
			// Token: 0x04008841 RID: 34881
			RightStickDown,
			// Token: 0x04008842 RID: 34882
			DPadLeft,
			// Token: 0x04008843 RID: 34883
			DPadRight,
			// Token: 0x04008844 RID: 34884
			DPadUp,
			// Token: 0x04008845 RID: 34885
			DPadDown,
			// Token: 0x04008846 RID: 34886
			StartButton,
			// Token: 0x04008847 RID: 34887
			BackButton,
			// Token: 0x04008848 RID: 34888
			None,
			// Token: 0x04008849 RID: 34889
			Count
		}
	}
}
