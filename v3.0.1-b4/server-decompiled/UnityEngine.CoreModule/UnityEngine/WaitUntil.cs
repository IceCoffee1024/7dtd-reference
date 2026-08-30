using System;

namespace UnityEngine
{
	// Token: 0x02000272 RID: 626
	public sealed class WaitUntil : CustomYieldInstruction
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0002C228 File Offset: 0x0002A428
		public override bool keepWaiting
		{
			get
			{
				return !this.m_Predicate();
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0002C248 File Offset: 0x0002A448
		public WaitUntil(Func<bool> predicate)
		{
			this.m_Predicate = predicate;
		}

		// Token: 0x04000916 RID: 2326
		private Func<bool> m_Predicate;
	}
}
