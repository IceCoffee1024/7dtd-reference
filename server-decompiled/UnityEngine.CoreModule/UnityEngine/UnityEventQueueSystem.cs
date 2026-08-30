using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000297 RID: 663
	[NativeHeader("Runtime/Export/UnityEvent/UnityEventQueueSystem.bindings.h")]
	public class UnityEventQueueSystem
	{
		// Token: 0x06001C1B RID: 7195 RVA: 0x0002E968 File Offset: 0x0002CB68
		public static string GenerateEventIdForPayload(string eventPayloadName)
		{
			byte[] array = Guid.NewGuid().ToByteArray();
			return string.Format("REGISTER_EVENT_ID(0x{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}ULL,0x{8:X2}{9:X2}{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}ULL,{16})", new object[]
			{
				array[0],
				array[1],
				array[2],
				array[3],
				array[4],
				array[5],
				array[6],
				array[7],
				array[8],
				array[9],
				array[10],
				array[11],
				array[12],
				array[13],
				array[14],
				array[15],
				eventPayloadName
			});
		}

		// Token: 0x06001C1C RID: 7196
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetGlobalEventQueue();
	}
}
