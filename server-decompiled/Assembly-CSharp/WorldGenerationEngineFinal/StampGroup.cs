using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001752 RID: 5970
	public class StampGroup
	{
		// Token: 0x0600B9B5 RID: 47541 RVA: 0x0044AEDB File Offset: 0x004490DB
		public StampGroup(string _name)
		{
			this.Name = _name;
			this.Stamps = new List<Stamp>();
		}

		// Token: 0x04008A8B RID: 35467
		public string Name;

		// Token: 0x04008A8C RID: 35468
		public List<Stamp> Stamps;
	}
}
