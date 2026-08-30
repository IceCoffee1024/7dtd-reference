using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000470 RID: 1136
	public struct ScopedSubPass : IDisposable
	{
		// Token: 0x06002643 RID: 9795 RVA: 0x00041B24 File Offset: 0x0003FD24
		internal ScopedSubPass(ScriptableRenderContext context)
		{
			this.m_Context = context;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00041B30 File Offset: 0x0003FD30
		public void Dispose()
		{
			try
			{
				this.m_Context.EndSubPass();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("The ScopedSubPass instance is not valid. This can happen if it was constructed using the default constructor.", ex);
			}
		}

		// Token: 0x04000E94 RID: 3732
		private ScriptableRenderContext m_Context;
	}
}
