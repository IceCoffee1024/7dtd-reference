using System;
using UnityEngine;

namespace XMLData.Parsers
{
	// Token: 0x0200163C RID: 5692
	public static class Vector2Parser
	{
		// Token: 0x0600B2E4 RID: 45796 RVA: 0x0042ED18 File Offset: 0x0042CF18
		public static Vector2 Parse(string _value)
		{
			return StringParsers.ParseVector2(_value);
		}

		// Token: 0x0600B2E5 RID: 45797 RVA: 0x0042ED20 File Offset: 0x0042CF20
		public static string Unparse(Vector2 _value)
		{
			return string.Format("{0},{1}", _value.x.ToCultureInvariantString(), _value.y.ToCultureInvariantString());
		}
	}
}
