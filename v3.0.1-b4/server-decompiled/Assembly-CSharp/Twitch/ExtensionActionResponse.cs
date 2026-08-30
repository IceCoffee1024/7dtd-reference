using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017A7 RID: 6055
	[Serializable]
	public class ExtensionActionResponse
	{
		// Token: 0x04008C6E RID: 35950
		public List<ExtensionAction> standardActions;

		// Token: 0x04008C6F RID: 35951
		public List<ExtensionBitAction> bitActions;
	}
}
