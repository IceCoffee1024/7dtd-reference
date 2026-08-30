using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000209 RID: 521
	[NativeHeader("Runtime/Utilities/PlayerPrefs.h")]
	public class PlayerPrefs
	{
		// Token: 0x06001782 RID: 6018
		[NativeMethod("SetInt")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TrySetInt(string key, int value);

		// Token: 0x06001783 RID: 6019
		[NativeMethod("SetFloat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TrySetFloat(string key, float value);

		// Token: 0x06001784 RID: 6020
		[NativeMethod("SetString")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TrySetSetString(string key, string value);

		// Token: 0x06001785 RID: 6021 RVA: 0x0002731C File Offset: 0x0002551C
		public static void SetInt(string key, int value)
		{
			bool flag = !PlayerPrefs.TrySetInt(key, value);
			if (flag)
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		// Token: 0x06001786 RID: 6022
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetInt(string key, int defaultValue);

		// Token: 0x06001787 RID: 6023 RVA: 0x00027344 File Offset: 0x00025544
		public static int GetInt(string key)
		{
			return PlayerPrefs.GetInt(key, 0);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00027360 File Offset: 0x00025560
		public static void SetFloat(string key, float value)
		{
			bool flag = !PlayerPrefs.TrySetFloat(key, value);
			if (flag)
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		// Token: 0x06001789 RID: 6025
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloat(string key, float defaultValue);

		// Token: 0x0600178A RID: 6026 RVA: 0x00027388 File Offset: 0x00025588
		public static float GetFloat(string key)
		{
			return PlayerPrefs.GetFloat(key, 0f);
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000273A8 File Offset: 0x000255A8
		public static void SetString(string key, string value)
		{
			bool flag = !PlayerPrefs.TrySetSetString(key, value);
			if (flag)
			{
				throw new PlayerPrefsException("Could not store preference value");
			}
		}

		// Token: 0x0600178C RID: 6028
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetString(string key, string defaultValue);

		// Token: 0x0600178D RID: 6029 RVA: 0x000273D0 File Offset: 0x000255D0
		public static string GetString(string key)
		{
			return PlayerPrefs.GetString(key, "");
		}

		// Token: 0x0600178E RID: 6030
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasKey(string key);

		// Token: 0x0600178F RID: 6031
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteKey(string key);

		// Token: 0x06001790 RID: 6032
		[NativeMethod("DeleteAllWithCallback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteAll();

		// Token: 0x06001791 RID: 6033
		[NativeMethod("Sync")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Save();
	}
}
