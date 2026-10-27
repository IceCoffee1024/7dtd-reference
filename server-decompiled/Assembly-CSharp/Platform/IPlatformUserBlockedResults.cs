using System;

namespace Platform
{
	// Token: 0x02001C13 RID: 7187
	public interface IPlatformUserBlockedResults
	{
		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x0600D5F5 RID: 54773
		IPlatformUser User { get; }

		// Token: 0x0600D5F6 RID: 54774
		void Block(EBlockType blockType);

		// Token: 0x0600D5F7 RID: 54775 RVA: 0x004CFE64 File Offset: 0x004CE064
		void BlockAll()
		{
			foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
			{
				this.Block(eblockType);
			}
		}

		// Token: 0x0600D5F8 RID: 54776
		void Error();
	}
}
