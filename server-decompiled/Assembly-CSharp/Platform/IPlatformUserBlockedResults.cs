using System;

namespace Platform
{
	// Token: 0x02001BB0 RID: 7088
	public interface IPlatformUserBlockedResults
	{
		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x0600D369 RID: 54121
		IPlatformUser User { get; }

		// Token: 0x0600D36A RID: 54122
		void Block(EBlockType blockType);

		// Token: 0x0600D36B RID: 54123 RVA: 0x004C7854 File Offset: 0x004C5A54
		void BlockAll()
		{
			foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
			{
				this.Block(eblockType);
			}
		}

		// Token: 0x0600D36C RID: 54124
		void Error();
	}
}
