using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Windows
{
	// Token: 0x020002C6 RID: 710
	[NativeHeader("PlatformDependent/Win/Bindings/InputBindings.h")]
	public static class Input
	{
		// Token: 0x06001E60 RID: 7776
		[NativeName("ForwardRawInput")]
		[StaticAccessor("", StaticAccessorType.DoubleColon)]
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void ForwardRawInputImpl(uint* rawInputHeaderIndices, uint* rawInputDataIndices, uint indicesCount, byte* rawInputData, uint rawInputDataSize);

		// Token: 0x06001E61 RID: 7777 RVA: 0x00031F15 File Offset: 0x00030115
		public unsafe static void ForwardRawInput(IntPtr rawInputHeaderIndices, IntPtr rawInputDataIndices, uint indicesCount, IntPtr rawInputData, uint rawInputDataSize)
		{
			Input.ForwardRawInput((uint*)(void*)rawInputHeaderIndices, (uint*)(void*)rawInputDataIndices, indicesCount, (byte*)(void*)rawInputData, rawInputDataSize);
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x00031F38 File Offset: 0x00030138
		public unsafe static void ForwardRawInput(uint* rawInputHeaderIndices, uint* rawInputDataIndices, uint indicesCount, byte* rawInputData, uint rawInputDataSize)
		{
			bool flag = rawInputHeaderIndices == null;
			if (flag)
			{
				throw new ArgumentNullException("rawInputHeaderIndices");
			}
			bool flag2 = rawInputDataIndices == null;
			if (flag2)
			{
				throw new ArgumentNullException("rawInputDataIndices");
			}
			bool flag3 = rawInputData == null;
			if (flag3)
			{
				throw new ArgumentNullException("rawInputData");
			}
			Input.ForwardRawInputImpl(rawInputHeaderIndices, rawInputDataIndices, indicesCount, rawInputData, rawInputDataSize);
		}
	}
}
