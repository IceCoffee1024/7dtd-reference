using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001677 RID: 5751
	public static class Vector3Parser
	{
		// Token: 0x0600B475 RID: 46197 RVA: 0x00437D5E File Offset: 0x00435F5E
		public static Vector3 Parse(string _value)
		{
			return StringParsers.ParseVector3(_value, 0, -1);
		}

		// Token: 0x0600B476 RID: 46198 RVA: 0x00437D68 File Offset: 0x00435F68
		public static string Unparse(Vector3 _value)
		{
			return string.Format("{0},{1},{2}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString(), _value.z.ToCultureInvariantString());
		}
	}
}
