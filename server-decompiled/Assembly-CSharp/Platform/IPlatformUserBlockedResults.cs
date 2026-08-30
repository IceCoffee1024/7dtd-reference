using System;

namespace Platform
{
	// Token: 0x02001B89 RID: 7049
	public interface IPlatformUserBlockedResults
	{
		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600D1DD RID: 53725
		IPlatformUser User { get; }

		// Token: 0x0600D1DE RID: 53726
		void Block(EBlockType blockType);

		// Token: 0x0600D1DF RID: 53727 RVA: 0x004C1460 File Offset: 0x004BF660
		void BlockAll()
		{
			foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
			{
				this.Block(eblockType);
			}
		}

		// Token: 0x0600D1E0 RID: 53728
		void Error();
	}
}
