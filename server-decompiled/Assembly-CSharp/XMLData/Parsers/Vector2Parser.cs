using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x02001676 RID: 5750
	public static class Vector2Parser
	{
		// Token: 0x0600B473 RID: 46195 RVA: 0x00437D34 File Offset: 0x00435F34
		public static Vector2 Parse(string _value)
		{
			return StringParsers.ParseVector2(_value);
		}

		// Token: 0x0600B474 RID: 46196 RVA: 0x00437D3C File Offset: 0x00435F3C
		public static string Unparse(Vector2 _value)
		{
			return string.Format("{0},{1}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString());
		}
	}
}
