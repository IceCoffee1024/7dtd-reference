using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001726 RID: 5926
	public class StampGroup
	{
		// Token: 0x0600B85E RID: 47198 RVA: 0x004469BB File Offset: 0x00444BBB
		public StampGroup(string _name)
		{
			this.Name = _name;
			this.Stamps = new List<Stamp>();
		}

		// Token: 0x040089C6 RID: 35270
		public string Name;

		// Token: 0x040089C7 RID: 35271
		public List<Stamp> Stamps;
	}
}
