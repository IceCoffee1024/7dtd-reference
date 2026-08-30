using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000275 RID: 629
	public sealed class Security
	{
		// Token: 0x06001A2C RID: 6700 RVA: 0x0002C28C File Offset: 0x0002A48C
		[Obsolete("This was an internal method which is no longer used", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
		{
			return null;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0002C2A0 File Offset: 0x0002A4A0
		[Obsolete("This was an internal method which is no longer used", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
		{
			return null;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0002C2B4 File Offset: 0x0002A4B4
		[ExcludeFromDocs]
		[Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
		public static bool PrefetchSocketPolicy(string ip, int atPort)
		{
			int num = 3000;
			return Security.PrefetchSocketPolicy(ip, atPort, num);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
		[Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
		public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
		{
			return false;
		}
	}
}
