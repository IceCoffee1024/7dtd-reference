using System;
using Unity.Collections;

namespace Unity.Jobs.LowLevel.Unsafe
{
	// Token: 0x0200004C RID: 76
	public struct BatchQueryJob<CommandT, ResultT> where CommandT : struct where ResultT : struct
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public BatchQueryJob(NativeArray<CommandT> commands, NativeArray<ResultT> results)
		{
			this.commands = commands;
			this.results = results;
		}

		// Token: 0x040000FD RID: 253
		[ReadOnly]
		internal NativeArray<CommandT> commands;

		// Token: 0x040000FE RID: 254
		internal NativeArray<ResultT> results;
	}
}
