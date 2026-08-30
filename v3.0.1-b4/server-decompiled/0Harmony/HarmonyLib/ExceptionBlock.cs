using System;

namespace HarmonyLib
{
	// Token: 0x02000038 RID: 56
	public class ExceptionBlock
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000519E File Offset: 0x0000339E
		public ExceptionBlock(ExceptionBlockType blockType, Type catchType = null)
		{
		}

		// Token: 0x0400005A RID: 90
		public ExceptionBlockType blockType = blockType;

		// Token: 0x0400005B RID: 91
		public Type catchType = catchType ?? typeof(object);
	}
}
