using System;
using System.Collections.Generic;

namespace HarmonyLib
{
	// Token: 0x02000050 RID: 80
	public static class CodeInstructionsExtensions
	{
		// Token: 0x06000385 RID: 901 RVA: 0x0000E6BB File Offset: 0x0000C8BB
		public static bool Matches(this IEnumerable<CodeInstruction> instructions, CodeMatch[] matches)
		{
			return new CodeMatcher(instructions, null).MatchStartForward(matches).IsValid;
		}
	}
}
