using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x02000327 RID: 807
	[Serializable]
	public struct CreateSceneParameters
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x00036550 File Offset: 0x00034750
		// (set) Token: 0x060020C8 RID: 8392 RVA: 0x00036568 File Offset: 0x00034768
		public LocalPhysicsMode localPhysicsMode
		{
			get
			{
				return this.m_LocalPhysicsMode;
			}
			set
			{
				this.m_LocalPhysicsMode = value;
			}
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00036568 File Offset: 0x00034768
		public CreateSceneParameters(LocalPhysicsMode physicsMode)
		{
			this.m_LocalPhysicsMode = physicsMode;
		}

		// Token: 0x04000ACF RID: 2767
		[SerializeField]
		private LocalPhysicsMode m_LocalPhysicsMode;
	}
}
