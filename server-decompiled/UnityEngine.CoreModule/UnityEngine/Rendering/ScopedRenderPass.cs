using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046F RID: 1135
	public struct ScopedRenderPass : IDisposable
	{
		// Token: 0x06002641 RID: 9793 RVA: 0x00041ADD File Offset: 0x0003FCDD
		internal ScopedRenderPass(ScriptableRenderContext context)
		{
			this.m_Context = context;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00041AE8 File Offset: 0x0003FCE8
		public void Dispose()
		{
			try
			{
				this.m_Context.EndRenderPass();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("The ScopedRenderPass instance is not valid. This can happen if it was constructed using the default constructor.", ex);
			}
		}

		// Token: 0x04000E93 RID: 3731
		private ScriptableRenderContext m_Context;
	}
}
