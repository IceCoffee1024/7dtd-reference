using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.FileCache
{
	// Token: 0x02001B36 RID: 6966
	[Preserve]
	public class InvalidateCachesCmd : ConsoleCmdAbstract
	{
		// Token: 0x0600D157 RID: 53591 RVA: 0x004BFC20 File Offset: 0x004BDE20
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "invalidatecaches" };
		}

		// Token: 0x0600D158 RID: 53592 RVA: 0x004BFC30 File Offset: 0x004BDE30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Invalidate contents of web file caches";
		}

		// Token: 0x0600D159 RID: 53593 RVA: 0x004BFC37 File Offset: 0x004BDE37
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "TODO";
		}

		// Token: 0x0600D15A RID: 53594 RVA: 0x004BFC40 File Offset: 0x004BDE40
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			ValueTuple<int, int> valueTuple = AbstractCache.InvalidateAllCaches();
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Caches invalidated, dropped {0} files with {1} Bytes", item, item2));
		}
	}
}
