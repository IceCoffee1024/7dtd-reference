using System;
using UnityEngine;

namespace TriggerEffects
{
	// Token: 0x020016BB RID: 5819
	public static class TriggerEffectDualsensePC
	{
		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x0600B698 RID: 46744 RVA: 0x0043C0F9 File Offset: 0x0043A2F9
		// (set) Token: 0x0600B699 RID: 46745 RVA: 0x0043C100 File Offset: 0x0043A300
		public static TriggerEffectDualsensePC.APIState _apiState
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get;
			[PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B69A RID: 46746 RVA: 0x0043C108 File Offset: 0x0043A308
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectVibration(userID, (int)trigger, position, amplitude, frequency);
			}
		}

		// Token: 0x0600B69B RID: 46747 RVA: 0x0043C12E File Offset: 0x0043A32E
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ResetControllerIdentification()
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtResetLightbar(1);
				ControllerExt.ControllerExtResetLightbar(2);
				ControllerExt.ControllerExtResetLightbar(3);
				ControllerExt.ControllerExtResetLightbar(4);
			}
		}

		// Token: 0x0600B69C RID: 46748 RVA: 0x0043C164 File Offset: 0x0043A364
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetControllerIdentification()
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetLightbar(1, 180, 180, 0);
				ControllerExt.ControllerExtSetLightbar(2, 80, 80, 0);
				ControllerExt.ControllerExtSetLightbar(3, 0, 180, 80);
				ControllerExt.ControllerExtSetLightbar(4, 0, 80, 80);
			}
		}

		// Token: 0x0600B69D RID: 46749 RVA: 0x0043C1C2 File Offset: 0x0043A3C2
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetLightbar(userId, colorR, colorG, colorB);
			}
		}

		// Token: 0x0600B69E RID: 46750 RVA: 0x0043C1E6 File Offset: 0x0043A3E6
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectWeapon(userID, (int)trigger, startPosition, endPosition, strength);
			}
		}

		// Token: 0x0600B69F RID: 46751 RVA: 0x0043C20C File Offset: 0x0043A40C
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectMultiVibration(userID, (int)trigger, amplitudes, amplitudes.Length, frequency);
			}
		}

		// Token: 0x0600B6A0 RID: 46752 RVA: 0x0043C234 File Offset: 0x0043A434
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void LibShutdown()
		{
			try
			{
				if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
				{
					for (int i = 1; i < 5; i++)
					{
						ControllerExt.ControllerExtResetLightbar(i);
						ControllerExt.ControllerExtSetTriggerEffectOff(i, 0);
						ControllerExt.ControllerExtSetTriggerEffectOff(i, 1);
						if (!ControllerExt.ControllerExtClosePad(i))
						{
							Log.Warning(string.Format("ControllerExtClosePad failed for user {0}", i));
						}
					}
					if (!ControllerExt.ControllerExtShutdown())
					{
						Log.Warning("ControllerExtShutdown failed");
					}
					Application.quitting -= TriggerEffectDualsensePC.LibShutdown;
					TriggerEffectDualsensePC._apiState = TriggerEffectDualsensePC.APIState.UnInit;
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("[ControllerExt] Shutdown failed {0}", ex));
			}
		}

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x0043C2E8 File Offset: 0x0043A4E8
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void InitTriggerEffectManager(ref bool[] controllersConnected)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor || (Application.platform == RuntimePlatform.WindowsPlayer && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.UnInit))
			{
				Application.quitting += TriggerEffectDualsensePC.LibShutdown;
				try
				{
					if (!ControllerExt.ControllerExtInit() && Application.platform == RuntimePlatform.WindowsEditor)
					{
						string text;
						if (ControllerExt.GetErrorString(out text))
						{
							Log.Warning("TriggerEffectManager: ControllerExtInit failed: " + text);
						}
						ControllerExt.ControllerExtUpdate();
						for (int i = 1; i < 5; i++)
						{
							ControllerExt.ControllerExtClosePad(i);
							controllersConnected[i - 1] = false;
						}
					}
					else
					{
						ControllerExt.ControllerExtUpdate();
						for (int j = 1; j < 5; j++)
						{
							if (ControllerExt.ControllerExtOpenPad(j))
							{
								controllersConnected[j - 1] = true;
							}
						}
						ControllerExt.ControllerExtUpdate();
						TriggerEffectDualsensePC._apiState = TriggerEffectDualsensePC.APIState.OK;
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Format("[ControllerExt] Failed to load Library, disabiling: {0}", ex));
				}
			}
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x0043C3BC File Offset: 0x0043A5BC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void PCTriggerUpdate(bool stateChanged, TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				for (int i = 1; i < 5; i++)
				{
					if (GameManager.Instance.IsPaused())
					{
						TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.LeftTrigger, TriggerEffectManager.NoneEffectDs);
						TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.RightTrigger, TriggerEffectManager.NoneEffectDs);
					}
					else
					{
						string text;
						if (!TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.LeftTrigger, currentEffectLeft) && stateChanged && ControllerExt.GetErrorString(out text))
						{
							Log.Warning(string.Format("Controller {0} ControllerExtTriggerEffectApply returned false ({1})", i, text));
						}
						string text2;
						if (!TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.RightTrigger, currentEffectRight) && stateChanged && ControllerExt.GetErrorString(out text2))
						{
							Log.Warning(string.Format("Controller {0} ControllerExtTriggerEffectApply returned false ({1})", i, text2));
						}
					}
				}
			}
		}

		// Token: 0x0600B6A3 RID: 46755 RVA: 0x0043C480 File Offset: 0x0043A680
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void PCConnectedUpdate(ref bool[] controllersConnected, TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtUpdate();
				for (int i = 1; i < 5; i++)
				{
					if (controllersConnected[i - 1] && !ControllerExt.ControllerExtIsConnected(i))
					{
						ControllerExt.ControllerExtClosePad(i);
						controllersConnected[i - 1] = false;
					}
					else if (!controllersConnected[i - 1] && ControllerExt.ControllerExtOpenPad(i))
					{
						controllersConnected[i - 1] = true;
						if (GameManager.Instance.IsPaused())
						{
							TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.LeftTrigger, TriggerEffectManager.NoneEffectDs);
							TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.RightTrigger, TriggerEffectManager.NoneEffectDs);
						}
						else
						{
							TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.LeftTrigger, currentEffectLeft);
							TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, TriggerEffectManager.GamepadTrigger.RightTrigger, currentEffectRight);
						}
					}
				}
			}
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x0043C530 File Offset: 0x0043A730
		[PublicizedFrom(EAccessModifier.Internal)]
		public static bool ApplyEffectDualsenseOnPC(int userId, TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.TriggerEffectDS effect)
		{
			bool flag = true;
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				switch (effect.Effect)
				{
				case TriggerEffectManager.EffectDualsense.Off:
					flag = ControllerExt.ControllerExtSetTriggerEffectOff(userId, (int)trigger);
					break;
				case TriggerEffectManager.EffectDualsense.WeaponSingle:
					flag = ControllerExt.ControllerExtSetTriggerEffectWeapon(userId, (int)trigger, effect.Position, effect.EndPosition, effect.Strength);
					break;
				case TriggerEffectManager.EffectDualsense.WeaponMultipoint:
				case TriggerEffectManager.EffectDualsense.VibrationSlope:
					throw new NotSupportedException();
				case TriggerEffectManager.EffectDualsense.FeedbackSingle:
					flag = ControllerExt.ControllerExtSetTriggerEffectFeedback(userId, (int)trigger, effect.Position, effect.Strength);
					break;
				case TriggerEffectManager.EffectDualsense.VibrationSingle:
					flag = ControllerExt.ControllerExtSetTriggerEffectVibration(userId, (int)trigger, effect.Position, effect.AmplitudeEndStrength, effect.Frequency);
					break;
				case TriggerEffectManager.EffectDualsense.FeedbackSlope:
					flag = ControllerExt.ControllerExtSetTriggerEffectSlopeFeedback(userId, (int)trigger, effect.Position, effect.EndPosition, effect.Strength, effect.AmplitudeEndStrength);
					break;
				case TriggerEffectManager.EffectDualsense.FeedbackMultipoint:
					flag = ControllerExt.ControllerExtSetTriggerEffectMultiFeedback(userId, (int)trigger, effect.Strengths, effect.Strengths.Length);
					break;
				case TriggerEffectManager.EffectDualsense.VibrationMultipoint:
					flag = ControllerExt.ControllerExtSetTriggerEffectMultiVibration(userId, (int)trigger, effect.Strengths, effect.Strengths.Length, effect.Frequency);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return flag;
		}

		// Token: 0x0600B6A5 RID: 46757 RVA: 0x0043C65C File Offset: 0x0043A85C
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyImmediate(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect currentEffect)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				for (int i = 1; i < 5; i++)
				{
					TriggerEffectDualsensePC.ApplyEffectDualsenseOnPC(i, trigger, currentEffect.DualsenseEffect);
				}
			}
		}

		// Token: 0x0600B6A6 RID: 46758 RVA: 0x0043C69C File Offset: 0x0043A89C
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void DualsensePCSetEffectToOff()
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				for (int i = 1; i < 5; i++)
				{
					string text;
					if (!ControllerExt.ControllerExtSetTriggerEffectOff(i, 0) && ControllerExt.GetErrorString(out text))
					{
						Log.Warning(string.Format("Controller {0} ControllerExtTriggerEffectApply returned false {1}", i, text));
					}
					if (!ControllerExt.ControllerExtSetTriggerEffectOff(i, 1) && ControllerExt.GetErrorString(out text))
					{
						Log.Warning(string.Format("Controller {0} ControllerExtTriggerEffectApply returned false {1}", i, text));
					}
				}
			}
		}

		// Token: 0x0600B6A7 RID: 46759 RVA: 0x0043C720 File Offset: 0x0043A920
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void EnableVibration()
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				for (int i = 1; i < 5; i++)
				{
					ControllerExt.ControllerExtEnableCompatibleVibration(i);
				}
			}
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x0043C758 File Offset: 0x0043A958
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				for (int i = 1; i < 5; i++)
				{
					ControllerExt.ControllerExtSetMotorValue(i, _smallMotor, _largeMotor);
				}
			}
		}

		// Token: 0x020016BC RID: 5820
		public enum APIState
		{
			// Token: 0x0400887B RID: 34939
			UnInit,
			// Token: 0x0400887C RID: 34940
			OK,
			// Token: 0x0400887D RID: 34941
			Error
		}
	}
}
