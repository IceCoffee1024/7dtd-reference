using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

namespace GUI_2
{
	// Token: 0x02001698 RID: 5784
	public static class UIUtils
	{
		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x0600B476 RID: 46198 RVA: 0x00434431 File Offset: 0x00432631
		public static INGUIAtlas IconAtlas
		{
			get
			{
				return UIUtils.symbolAtlas;
			}
		}

		// Token: 0x0600B477 RID: 46199 RVA: 0x00434438 File Offset: 0x00432638
		public static string GetSpriteName(UIUtils.ButtonIcon _icon)
		{
			if (PlayerInputManager.InputStyleFromSelectedIconStyle() != PlayerInputManager.InputStyle.PS4)
			{
				return UIUtils.buttonIconMapXB[_icon];
			}
			return UIUtils.buttonIconMapPS[_icon];
		}

		// Token: 0x0600B478 RID: 46200 RVA: 0x0043445C File Offset: 0x0043265C
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

		// Token: 0x0600B479 RID: 46201 RVA: 0x004344ED File Offset: 0x004326ED
		public static void LoadAtlas()
		{
			UIUtils.symbolAtlas = Resources.Load<UIAtlas>("GUI/Prefabs/SymbolAtlas");
		}

		// Token: 0x04008706 RID: 34566
		[PublicizedFrom(EAccessModifier.Private)]
		public static INGUIAtlas symbolAtlas;

		// Token: 0x04008707 RID: 34567
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

		// Token: 0x04008708 RID: 34568
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

		// Token: 0x04008709 RID: 34569
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

		// Token: 0x0400870A RID: 34570
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<PlayerAction> loggedMissingBindingSources = new HashSet<PlayerAction>();

		// Token: 0x02001699 RID: 5785
		public enum ButtonIcon
		{
			// Token: 0x0400870C RID: 34572
			FaceButtonSouth,
			// Token: 0x0400870D RID: 34573
			FaceButtonNorth,
			// Token: 0x0400870E RID: 34574
			FaceButtonEast,
			// Token: 0x0400870F RID: 34575
			FaceButtonWest,
			// Token: 0x04008710 RID: 34576
			ConfirmButton,
			// Token: 0x04008711 RID: 34577
			CancelButton,
			// Token: 0x04008712 RID: 34578
			LeftBumper,
			// Token: 0x04008713 RID: 34579
			RightBumper,
			// Token: 0x04008714 RID: 34580
			LeftTrigger,
			// Token: 0x04008715 RID: 34581
			RightTrigger,
			// Token: 0x04008716 RID: 34582
			LeftStick,
			// Token: 0x04008717 RID: 34583
			LeftStickUpDown,
			// Token: 0x04008718 RID: 34584
			LeftStickLeftRight,
			// Token: 0x04008719 RID: 34585
			LeftStickButton,
			// Token: 0x0400871A RID: 34586
			LeftStickLeft,
			// Token: 0x0400871B RID: 34587
			LeftStickRight,
			// Token: 0x0400871C RID: 34588
			LeftStickUp,
			// Token: 0x0400871D RID: 34589
			LeftStickDown,
			// Token: 0x0400871E RID: 34590
			RightStick,
			// Token: 0x0400871F RID: 34591
			RightStickUpDown,
			// Token: 0x04008720 RID: 34592
			RightStickLeftRight,
			// Token: 0x04008721 RID: 34593
			RightStickButton,
			// Token: 0x04008722 RID: 34594
			RightStickLeft,
			// Token: 0x04008723 RID: 34595
			RightStickRight,
			// Token: 0x04008724 RID: 34596
			RightStickUp,
			// Token: 0x04008725 RID: 34597
			RightStickDown,
			// Token: 0x04008726 RID: 34598
			DPadLeft,
			// Token: 0x04008727 RID: 34599
			DPadRight,
			// Token: 0x04008728 RID: 34600
			DPadUp,
			// Token: 0x04008729 RID: 34601
			DPadDown,
			// Token: 0x0400872A RID: 34602
			StartButton,
			// Token: 0x0400872B RID: 34603
			BackButton,
			// Token: 0x0400872C RID: 34604
			None,
			// Token: 0x0400872D RID: 34605
			Count
		}
	}
}
