using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017CD RID: 6093
	[Serializable]
	public class ExtensionActionResponse
	{
		// Token: 0x04008D8A RID: 36234
		public List<ExtensionAction> standardActions;

		// Token: 0x04008D8B RID: 36235
		public List<ExtensionBitAction> bitActions;
	}
}
