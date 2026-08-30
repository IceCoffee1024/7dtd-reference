using System;
using UnityEngine;

namespace TriggerEffects
{
	// Token: 0x02001669 RID: 5737
	public static class TriggerEffectDualsensePC
	{
		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x0600B3BE RID: 46014 RVA: 0x00431E5F File Offset: 0x0043005F
		// (set) Token: 0x0600B3BF RID: 46015 RVA: 0x00431E66 File Offset: 0x00430066
		public static TriggerEffectDualsensePC.APIState _apiState
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get;
			[PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B3C0 RID: 46016 RVA: 0x00431E6E File Offset: 0x0043006E
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectVibration(userID, (int)trigger, position, amplitude, frequency);
			}
		}

		// Token: 0x0600B3C1 RID: 46017 RVA: 0x00431E94 File Offset: 0x00430094
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

		// Token: 0x0600B3C2 RID: 46018 RVA: 0x00431ECC File Offset: 0x004300CC
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

		// Token: 0x0600B3C3 RID: 46019 RVA: 0x00431F2A File Offset: 0x0043012A
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetLightbar(userId, colorR, colorG, colorB);
			}
		}

		// Token: 0x0600B3C4 RID: 46020 RVA: 0x00431F4E File Offset: 0x0043014E
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectWeapon(userID, (int)trigger, startPosition, endPosition, strength);
			}
		}

		// Token: 0x0600B3C5 RID: 46021 RVA: 0x00431F74 File Offset: 0x00430174
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
			if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && TriggerEffectDualsensePC._apiState == TriggerEffectDualsensePC.APIState.OK)
			{
				ControllerExt.ControllerExtSetTriggerEffectMultiVibration(userID, (int)trigger, amplitudes, amplitudes.Length, frequency);
			}
		}

		// Token: 0x0600B3C6 RID: 46022 RVA: 0x00431F9C File Offset: 0x0043019C
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

		// Token: 0x0600B3C7 RID: 46023 RVA: 0x00432050 File Offset: 0x00430250
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

		// Token: 0x0600B3C8 RID: 46024 RVA: 0x00432124 File Offset: 0x00430324
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

		// Token: 0x0600B3C9 RID: 46025 RVA: 0x004321E8 File Offset: 0x004303E8
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

		// Token: 0x0600B3CA RID: 46026 RVA: 0x00432298 File Offset: 0x00430498
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

		// Token: 0x0600B3CB RID: 46027 RVA: 0x004323C4 File Offset: 0x004305C4
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

		// Token: 0x0600B3CC RID: 46028 RVA: 0x00432404 File Offset: 0x00430604
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

		// Token: 0x0600B3CD RID: 46029 RVA: 0x00432488 File Offset: 0x00430688
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

		// Token: 0x0600B3CE RID: 46030 RVA: 0x004324C0 File Offset: 0x004306C0
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

		// Token: 0x0200166A RID: 5738
		public enum APIState
		{
			// Token: 0x0400869A RID: 34458
			UnInit,
			// Token: 0x0400869B RID: 34459
			OK,
			// Token: 0x0400869C RID: 34460
			Error
		}
	}
}
