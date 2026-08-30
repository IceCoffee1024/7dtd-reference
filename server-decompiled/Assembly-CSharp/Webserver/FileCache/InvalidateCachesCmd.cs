using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.FileCache
{
	// Token: 0x02001B0A RID: 6922
	[Preserve]
	public class InvalidateCachesCmd : ConsoleCmdAbstract
	{
		// Token: 0x0600CFFF RID: 53247 RVA: 0x004BB754 File Offset: 0x004B9954
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "invalidatecaches" };
		}

		// Token: 0x0600D000 RID: 53248 RVA: 0x004BB764 File Offset: 0x004B9964
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Invalidate contents of web file caches";
		}

		// Token: 0x0600D001 RID: 53249 RVA: 0x004BB76B File Offset: 0x004B996B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "TODO";
		}

		// Token: 0x0600D002 RID: 53250 RVA: 0x004BB774 File Offset: 0x004B9974
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			ValueTuple<int, int> valueTuple = AbstractCache.InvalidateAllCaches();
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Caches invalidated, dropped {0} files with {1} Bytes", item, item2));
		}
	}
}
