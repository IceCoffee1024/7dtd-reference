using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001700 RID: 5888
	public class StampGroup
	{
		// Token: 0x0600B6DB RID: 46811 RVA: 0x00440C43 File Offset: 0x0043EE43
		public StampGroup(string _name)
		{
			this.Name = _name;
			this.Stamps = new List<Stamp>();
		}

		// Token: 0x040088AA RID: 34986
		public string Name;

		// Token: 0x040088AB RID: 34987
		public List<Stamp> Stamps;
	}
}
