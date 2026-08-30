using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x0200163D RID: 5693
	public static class Vector3Parser
	{
		// Token: 0x0600B2E6 RID: 45798 RVA: 0x0042ED42 File Offset: 0x0042CF42
		public static Vector3 Parse(string _value)
		{
			return StringParsers.ParseVector3(_value, 0, -1);
		}

		// Token: 0x0600B2E7 RID: 45799 RVA: 0x0042ED4C File Offset: 0x0042CF4C
		public static string Unparse(Vector3 _value)
		{
			return string.Format("{0},{1},{2}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString(), _value.z.ToCultureInvariantString());
		}
	}
}
