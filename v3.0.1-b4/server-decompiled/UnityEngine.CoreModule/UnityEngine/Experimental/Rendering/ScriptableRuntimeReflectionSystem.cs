using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004D5 RID: 1237
	public abstract class ScriptableRuntimeReflectionSystem : IScriptableRuntimeReflectionSystem, IDisposable
	{
		// Token: 0x06002B27 RID: 11047 RVA: 0x000490E8 File Offset: 0x000472E8
		public virtual bool TickRealtimeProbes()
		{
			return false;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x00002669 File Offset: 0x00000869
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000490FB File Offset: 0x000472FB
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
