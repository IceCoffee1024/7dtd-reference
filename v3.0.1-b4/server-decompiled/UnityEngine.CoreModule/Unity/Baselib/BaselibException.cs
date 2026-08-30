using System;
using Unity.Baselib.LowLevel;

namespace Unity.Baselib
{
	// Token: 0x02000014 RID: 20
	internal class BaselibException : Exception
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002200 File Offset: 0x00000400
		internal BaselibException(ErrorState errorState)
			: base(errorState.Explain(Binding.Baselib_ErrorState_ExplainVerbosity.ErrorType_SourceLocation_Explanation))
		{
			this.errorState = errorState;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000221C File Offset: 0x0000041C
		public Binding.Baselib_ErrorCode ErrorCode
		{
			get
			{
				return this.errorState.ErrorCode;
			}
		}

		// Token: 0x04000026 RID: 38
		private readonly ErrorState errorState;
	}
}
