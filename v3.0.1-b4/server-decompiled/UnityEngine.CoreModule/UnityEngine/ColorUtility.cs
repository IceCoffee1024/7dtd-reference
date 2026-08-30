using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001EA RID: 490
	[NativeHeader("Runtime/Math/ColorUtility.h")]
	public class ColorUtility
	{
		// Token: 0x06001521 RID: 5409
		[FreeFunction("TryParseHtmlColor", true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DoTryParseHtmlColor(string htmlString, out Color32 color);

		// Token: 0x06001522 RID: 5410 RVA: 0x0001F980 File Offset: 0x0001DB80
		public static bool TryParseHtmlString(string htmlString, out Color color)
		{
			Color32 color2;
			bool flag = ColorUtility.DoTryParseHtmlColor(htmlString, out color2);
			color = color2;
			return flag;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
		public static string ToHtmlStringRGB(Color color)
		{
			Color32 color2 = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255f), 0, 255), 1);
			return UnityString.Format("{0:X2}{1:X2}{2:X2}", new object[] { color2.r, color2.g, color2.b });
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0001FA54 File Offset: 0x0001DC54
		public static string ToHtmlStringRGBA(Color color)
		{
			Color32 color2 = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(color.a * 255f), 0, 255));
			return UnityString.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { color2.r, color2.g, color2.b, color2.a });
		}
	}
}
