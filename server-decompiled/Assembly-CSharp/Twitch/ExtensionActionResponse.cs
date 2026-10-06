using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F9 RID: 6137
	[Serializable]
	public class ExtensionActionResponse
	{
		// Token: 0x04008E50 RID: 36432
		public List<ExtensionAction> standardActions;

		// Token: 0x04008E51 RID: 36433
		public List<ExtensionBitAction> bitActions;
	}
}
