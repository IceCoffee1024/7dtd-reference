using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001688 RID: 5768
	public static class Vector2Parser
	{
		// Token: 0x0600B4CB RID: 46283 RVA: 0x0043A138 File Offset: 0x00438338
		public static Vector2 Parse(string _value)
		{
			return StringParsers.ParseVector2(_value);
		}

		// Token: 0x0600B4CC RID: 46284 RVA: 0x0043A140 File Offset: 0x00438340
		public static string Unparse(Vector2 _value)
		{
			return string.Format("{0},{1}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString());
		}
	}
}
