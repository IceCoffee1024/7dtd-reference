using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200182A RID: 6186
	[Serializable]
	public class ExtensionActionResponse
	{
		// Token: 0x04008F39 RID: 36665
		public List<ExtensionAction> standardActions;

		// Token: 0x04008F3A RID: 36666
		public List<ExtensionBitAction> bitActions;
	}
}
