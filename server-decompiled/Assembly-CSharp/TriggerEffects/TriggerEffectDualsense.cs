using System;

namespace TriggerEffects
{
	// Token: 0x0200168E RID: 5774
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class TriggerEffectDualsense
	{
		// Token: 0x0600B533 RID: 46387 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void InitTriggerEffectManager()
		{
		}

		// Token: 0x0600B534 RID: 46388 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ConnectedUpdate(TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
		}

		// Token: 0x0600B535 RID: 46389 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void Update(TriggerEffectManager.TriggerEffectDS left, TriggerEffectManager.TriggerEffectDS right)
		{
		}

		// Token: 0x0600B536 RID: 46390 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetEffectToOff()
		{
		}

		// Token: 0x0600B537 RID: 46391 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ResetControllerIdentification()
		{
		}

		// Token: 0x0600B538 RID: 46392 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetControllerIdentification()
		{
		}

		// Token: 0x0600B539 RID: 46393 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyImmediate(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect currentEffect)
		{
		}

		// Token: 0x0600B53A RID: 46394 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyEffectPS5Input(int slot, TriggerEffectManager.GamepadTrigger triggerGeneric, TriggerEffectManager.TriggerEffectDS effect)
		{
		}

		// Token: 0x0600B53B RID: 46395 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
		}

		// Token: 0x0600B53C RID: 46396 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
		}

		// Token: 0x0600B53D RID: 46397 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
		}

		// Token: 0x0600B53E RID: 46398 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void EnableVibration()
		{
		}

		// Token: 0x0600B53F RID: 46399 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
		{
		}

		// Token: 0x0600B540 RID: 46400 RVA: 0x000027FC File Offset: 0x000009FC
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
		}
	}
}
