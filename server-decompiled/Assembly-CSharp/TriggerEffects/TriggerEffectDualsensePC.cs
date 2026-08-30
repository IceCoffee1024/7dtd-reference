using System;
using UnityEngine;

namespace TriggerEffects
{
	// Token: 0x0200168F RID: 5775
	public static class TriggerEffectDualsensePC
	{
		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x0600B541 RID: 46401 RVA: 0x00437BD8 File Offset: 0x00435DD8
		// (set) Token: 0x0600B542 RID: 46402 RVA: 0x00437BDF File Offset: 0x00435DDF
		public static TriggerEffectDualsensePC.APIState _apiState
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get;
			[PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B543 RID: 46403 RVA: 0x00437BE7 File Offset: 0x00435DE7
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectVibration(userID, (int)trigger, position, amplitude, frequency);
			}
		}

		// Token: 0x0600B544 RID: 46404 RVA: 0x00437C0D File Offset: 0x00435E0D
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

		// Token: 0x0600B545 RID: 46405 RVA: 0x00437C44 File Offset: 0x00435E44
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

		// Token: 0x0600B546 RID: 46406 RVA: 0x00437CA2 File Offset: 0x00435EA2
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetLightbar(userId, colorR, colorG, colorB);
			}
		}

		// Token: 0x0600B547 RID: 46407 RVA: 0x00437CC6 File Offset: 0x00435EC6
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectWeapon(userID, (int)trigger, startPosition, endPosition, strength);
			}
		}

		// Token: 0x0600B548 RID: 46408 RVA: 0x00437CEC File Offset: 0x00435EEC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectMultiVibration(userID, (int)trigger, amplitudes, amplitudes.Length, frequency);
			}
		}

		// Token: 0x0600B549 RID: 46409 RVA: 0x00437D14 File Offset: 0x00435F14
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

		// Token: 0x0600B54A RID: 46410 RVA: 0x00437DC8 File Offset: 0x00435FC8
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

		// Token: 0x0600B54B RID: 46411 RVA: 0x00437E9C File Offset: 0x0043609C
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

		// Token: 0x0600B54C RID: 46412 RVA: 0x00437F60 File Offset: 0x00436160
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

		// Token: 0x0600B54D RID: 46413 RVA: 0x00438010 File Offset: 0x00436210
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

		// Token: 0x0600B54E RID: 46414 RVA: 0x0043813C File Offset: 0x0043633C
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

		// Token: 0x0600B54F RID: 46415 RVA: 0x0043817C File Offset: 0x0043637C
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

		// Token: 0x0600B550 RID: 46416 RVA: 0x00438200 File Offset: 0x00436400
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

		// Token: 0x0600B551 RID: 46417 RVA: 0x00438238 File Offset: 0x00436438
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

		// Token: 0x02001690 RID: 5776
		public enum APIState
		{
			// Token: 0x040087B6 RID: 34742
			UnInit,
			// Token: 0x040087B7 RID: 34743
			OK,
			// Token: 0x040087B8 RID: 34744
			Error
		}
	}
}
