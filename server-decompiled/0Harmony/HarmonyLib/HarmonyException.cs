using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using HarmonyLib.Internal.Patching;
using Mono.Cecil.Cil;

namespace HarmonyLib
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class HarmonyException : Exception
	{
		// Token: 0x060000FA RID: 250 RVA: 0x0000590F File Offset: 0x00003B0F
		internal HarmonyException()
		{
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005929 File Offset: 0x00003B29
		internal HarmonyException(string message)
			: base(message)
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005944 File Offset: 0x00003B44
		internal HarmonyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005960 File Offset: 0x00003B60
		protected HarmonyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000597F File Offset: 0x00003B7F
		internal HarmonyException(Exception innerException, Dictionary<int, CodeInstruction> instructions, int errorOffset)
			: base("IL Compile Error", innerException)
		{
			this.instructions = instructions;
			this.errorOffset = errorOffset;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000059B0 File Offset: 0x00003BB0
		internal static Exception Create(Exception ex, MethodBody body)
		{
			HarmonyException ex2 = ex as HarmonyException;
			if (ex2 != null)
			{
				Dictionary<int, CodeInstruction> dictionary = ex2.instructions;
				if (dictionary != null && dictionary.Count > 0 && ex2.errorOffset >= 0)
				{
					return ex;
				}
			}
			Match match = Regex.Match(ex.Message.TrimEnd(new char[0]), "(?:Reason: )?Invalid IL code in.+: IL_(\\d{4}): (.+)$");
			if (!match.Success)
			{
				return new HarmonyException("IL Compile Error (unknown location)", ex);
			}
			Dictionary<int, CodeInstruction> dictionary2 = ILManipulator.GetInstructions(body) ?? new Dictionary<int, CodeInstruction>();
			int num = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
			Regex.Replace(match.Groups[2].Value, " {2,}", " ");
			HarmonyException ex3 = ex as HarmonyException;
			if (ex3 != null)
			{
				if (dictionary2.Count != 0)
				{
					ex3.instructions = dictionary2;
					ex3.errorOffset = num;
				}
				return ex3;
			}
			return new HarmonyException(ex, dictionary2, num);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005A93 File Offset: 0x00003C93
		public List<KeyValuePair<int, CodeInstruction>> GetInstructionsWithOffsets()
		{
			List<KeyValuePair<int, CodeInstruction>> list = new List<KeyValuePair<int, CodeInstruction>>();
			list.AddRange(this.instructions.OrderBy<KeyValuePair<int, CodeInstruction>, int>((KeyValuePair<int, CodeInstruction> ins) => ins.Key));
			return list;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005ACC File Offset: 0x00003CCC
		public List<CodeInstruction> GetInstructions()
		{
			return (from ins in this.instructions
				orderby ins.Key
				select ins.Value).ToList<CodeInstruction>();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005B2C File Offset: 0x00003D2C
		public int GetErrorOffset()
		{
			return this.errorOffset;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005B34 File Offset: 0x00003D34
		public int GetErrorIndex()
		{
			CodeInstruction codeInstruction;
			if (this.instructions.TryGetValue(this.errorOffset, out codeInstruction))
			{
				return this.GetInstructions().IndexOf(codeInstruction);
			}
			return -1;
		}

		// Token: 0x04000061 RID: 97
		private Dictionary<int, CodeInstruction> instructions = new Dictionary<int, CodeInstruction>();

		// Token: 0x04000062 RID: 98
		private int errorOffset = -1;
	}
}
