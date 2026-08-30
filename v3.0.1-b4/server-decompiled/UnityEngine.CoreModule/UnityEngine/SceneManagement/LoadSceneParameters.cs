using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x02000326 RID: 806
	[Serializable]
	public struct LoadSceneParameters
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x000364E8 File Offset: 0x000346E8
		// (set) Token: 0x060020C2 RID: 8386 RVA: 0x00036500 File Offset: 0x00034700
		public LoadSceneMode loadSceneMode
		{
			get
			{
				return this.m_LoadSceneMode;
			}
			set
			{
				this.m_LoadSceneMode = value;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x0003650C File Offset: 0x0003470C
		// (set) Token: 0x060020C4 RID: 8388 RVA: 0x00036524 File Offset: 0x00034724
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

		// Token: 0x060020C5 RID: 8389 RVA: 0x0003652E File Offset: 0x0003472E
		public LoadSceneParameters(LoadSceneMode mode)
		{
			this.m_LoadSceneMode = mode;
			this.m_LocalPhysicsMode = LocalPhysicsMode.None;
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0003653F File Offset: 0x0003473F
		public LoadSceneParameters(LoadSceneMode mode, LocalPhysicsMode physicsMode)
		{
			this.m_LoadSceneMode = mode;
			this.m_LocalPhysicsMode = physicsMode;
		}

		// Token: 0x04000ACD RID: 2765
		[SerializeField]
		private LoadSceneMode m_LoadSceneMode;

		// Token: 0x04000ACE RID: 2766
		[SerializeField]
		private LocalPhysicsMode m_LocalPhysicsMode;
	}
}
