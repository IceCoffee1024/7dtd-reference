using System;
using UnityEngine.Scripting;

namespace Unity.Collections
{
	// Token: 0x02000090 RID: 144
	[AttributeUsage(AttributeTargets.Field)]
	[RequiredByNativeCode]
	public sealed class NativeFixedLengthAttribute : Attribute
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00004EFC File Offset: 0x000030FC
		public NativeFixedLengthAttribute(int fixedLength)
		{
			this.FixedLength = fixedLength;
		}

		// Token: 0x04000212 RID: 530
		public int FixedLength;
	}
}
