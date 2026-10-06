using System;

namespace Platform
{
	// Token: 0x02001BE2 RID: 7138
	public interface IPlatformUserBlockedResults
	{
		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600D4E1 RID: 54497
		IPlatformUser User { get; }

		// Token: 0x0600D4E2 RID: 54498
		void Block(EBlockType blockType);

		// Token: 0x0600D4E3 RID: 54499 RVA: 0x004CC33C File Offset: 0x004CA53C
		void BlockAll()
		{
			foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
			{
				this.Block(eblockType);
			}
		}

		// Token: 0x0600D4E4 RID: 54500
		void Error();
	}
}
