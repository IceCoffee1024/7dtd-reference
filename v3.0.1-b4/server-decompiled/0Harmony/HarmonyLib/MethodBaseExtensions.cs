using System;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000052 RID: 82
	public static class MethodBaseExtensions
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000E88C File Offset: 0x0000CA8C
		public static bool HasMethodBody(this MethodBase member)
		{
			MethodBody methodBody = member.GetMethodBody();
			int? num;
			if (methodBody == null)
			{
				num = null;
			}
			else
			{
				byte[] ilasByteArray = methodBody.GetILAsByteArray();
				num = ((ilasByteArray != null) ? new int?(ilasByteArray.Length) : null);
			}
			int? num2 = num;
			return num2.GetValueOrDefault() > 0;
		}
	}
}
