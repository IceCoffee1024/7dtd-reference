using System;

namespace TriggerEffects
{
	// Token: 0x020016BA RID: 5818
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class TriggerEffectDualsense
	{
		// Token: 0x0600B68A RID: 46730 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void InitTriggerEffectManager()
		{
		}

		// Token: 0x0600B68B RID: 46731 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ConnectedUpdate(TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
		}

		// Token: 0x0600B68C RID: 46732 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void Update(TriggerEffectManager.TriggerEffectDS left, TriggerEffectManager.TriggerEffectDS right)
		{
		}

		// Token: 0x0600B68D RID: 46733 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetEffectToOff()
		{
		}

		// Token: 0x0600B68E RID: 46734 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ResetControllerIdentification()
		{
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetControllerIdentification()
		{
		}

		// Token: 0x0600B690 RID: 46736 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyImmediate(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect currentEffect)
		{
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyEffectPS5Input(int slot, TriggerEffectManager.GamepadTrigger triggerGeneric, TriggerEffectManager.TriggerEffectDS effect)
		{
		}

		// Token: 0x0600B692 RID: 46738 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
		}

		// Token: 0x0600B693 RID: 46739 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
		}

		// Token: 0x0600B694 RID: 46740 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
		}

		// Token: 0x0600B695 RID: 46741 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void EnableVibration()
		{
		}

		// Token: 0x0600B696 RID: 46742 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
		{
		}

		// Token: 0x0600B697 RID: 46743 RVA: 0x000027FC File Offset: 0x000009FC
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
		}
	}
}
