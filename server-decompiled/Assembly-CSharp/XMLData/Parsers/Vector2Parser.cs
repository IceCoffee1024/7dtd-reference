using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001657 RID: 5719
	public static class Vector2Parser
	{
		// Token: 0x0600B3E1 RID: 46049 RVA: 0x00434058 File Offset: 0x00432258
		public static Vector2 Parse(string _value)
		{
			return StringParsers.ParseVector2(_value);
		}

		// Token: 0x0600B3E2 RID: 46050 RVA: 0x00434060 File Offset: 0x00432260
		public static string Unparse(Vector2 _value)
		{
			return string.Format("{0},{1}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString());
		}
	}
}
