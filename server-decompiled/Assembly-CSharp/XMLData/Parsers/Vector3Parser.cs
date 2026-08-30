using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001658 RID: 5720
	public static class Vector3Parser
	{
		// Token: 0x0600B3E3 RID: 46051 RVA: 0x00434082 File Offset: 0x00432282
		public static Vector3 Parse(string _value)
		{
			return StringParsers.ParseVector3(_value, 0, -1);
		}

		// Token: 0x0600B3E4 RID: 46052 RVA: 0x0043408C File Offset: 0x0043228C
		public static string Unparse(Vector3 _value)
		{
			return string.Format("{0},{1},{2}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString(), _value.z.ToCultureInvariantString());
		}
	}
}
