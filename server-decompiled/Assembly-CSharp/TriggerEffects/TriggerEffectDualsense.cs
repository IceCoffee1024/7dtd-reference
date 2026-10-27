using System;

namespace TriggerEffects
{
	// Token: 0x020016D9 RID: 5849
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class TriggerEffectDualsense
	{
		// Token: 0x0600B75E RID: 46942 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void InitTriggerEffectManager()
		{
		}

		// Token: 0x0600B75F RID: 46943 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ConnectedUpdate(TriggerEffectManager.TriggerEffectDS currentEffectLeft, TriggerEffectManager.TriggerEffectDS currentEffectRight)
		{
		}

		// Token: 0x0600B760 RID: 46944 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void Update(TriggerEffectManager.TriggerEffectDS left, TriggerEffectManager.TriggerEffectDS right)
		{
		}

		// Token: 0x0600B761 RID: 46945 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetEffectToOff()
		{
		}

		// Token: 0x0600B762 RID: 46946 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ResetControllerIdentification()
		{
		}

		// Token: 0x0600B763 RID: 46947 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetControllerIdentification()
		{
		}

		// Token: 0x0600B764 RID: 46948 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyImmediate(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect currentEffect)
		{
		}

		// Token: 0x0600B765 RID: 46949 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void ApplyEffectPS5Input(int slot, TriggerEffectManager.GamepadTrigger triggerGeneric, TriggerEffectManager.TriggerEffectDS effect)
		{
		}

		// Token: 0x0600B766 RID: 46950 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
		{
		}

		// Token: 0x0600B767 RID: 46951 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
		{
		}

		// Token: 0x0600B768 RID: 46952 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
		{
		}

		// Token: 0x0600B769 RID: 46953 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void EnableVibration()
		{
		}

		// Token: 0x0600B76A RID: 46954 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
		{
		}

		// Token: 0x0600B76B RID: 46955 RVA: 0x000027FC File Offset: 0x000009FC
		public static void SetLightbar(int userId, byte colorR, byte colorG, byte colorB)
		{
		}
	}
}
