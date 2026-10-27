using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001689 RID: 5769
	public static class Vector3Parser
	{
		// Token: 0x0600B4CD RID: 46285 RVA: 0x0043A162 File Offset: 0x00438362
		public static Vector3 Parse(string _value)
		{
			return StringParsers.ParseVector3(_value, 0, -1);
		}

		// Token: 0x0600B4CE RID: 46286 RVA: 0x0043A16C File Offset: 0x0043836C
		public static string Unparse(Vector3 _value)
		{
			return string.Format("{0},{1},{2}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString(), _value.z.ToCultureInvariantString());
		}
	}
}
