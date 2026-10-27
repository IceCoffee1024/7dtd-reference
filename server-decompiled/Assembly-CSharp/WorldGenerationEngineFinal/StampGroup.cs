using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001783 RID: 6019
	public class StampGroup
	{
		// Token: 0x0600BAC9 RID: 47817 RVA: 0x0044E887 File Offset: 0x0044CA87
		public StampGroup(string _name)
		{
			this.Name = _name;
			this.Stamps = new List<Stamp>();
		}

		// Token: 0x04008B74 RID: 35700
		public string Name;

		// Token: 0x04008B75 RID: 35701
		public List<Stamp> Stamps;
	}
}
