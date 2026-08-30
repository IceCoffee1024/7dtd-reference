using System;

namespace TriggerEffects
{
	// Token: 0x02001668 RID: 5736
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class TriggerEffectDualsense
	{
		// Token: 0x0600B3B0 RID: 46000 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void InitTriggerEffectManager()
		{
		}

		// Token: 0x0600B3B1 RID: 46001 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ConnectedUpdate(TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
		}

		// Token: 0x0600B3B2 RID: 46002 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void Update(TriggerEffectManager.TriggerEffectDS left, TriggerEffectManager.TriggerEffectDS right)
		{
		}

		// Token: 0x0600B3B3 RID: 46003 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetEffectToOff()
		{
		}

		// Token: 0x0600B3B4 RID: 46004 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ResetControllerIdentification()
		{
		}

		// Token: 0x0600B3B5 RID: 46005 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetControllerIdentification()
		{
		}

		// Token: 0x0600B3B6 RID: 46006 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyImmediate(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect currentEffect)
		{
		}

		// Token: 0x0600B3B7 RID: 46007 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyEffectPS5Input(int slot, TriggerEffectManager.GamepadTrigger triggerGeneric, TriggerEffectManager.TriggerEffectDS effect)
		{
		}

		// Token: 0x0600B3B8 RID: 46008 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
		}

		// Token: 0x0600B3B9 RID: 46009 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
		}

		// Token: 0x0600B3BA RID: 46010 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
		}

		// Token: 0x0600B3BB RID: 46011 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void EnableVibration()
		{
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
		{
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x000027FC File Offset: 0x000009FC
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
		}
	}
}
