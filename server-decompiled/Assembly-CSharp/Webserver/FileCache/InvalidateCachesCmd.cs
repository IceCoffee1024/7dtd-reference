using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.FileCache
{
	// Token: 0x02001B67 RID: 7015
	[Preserve]
	public class InvalidateCachesCmd : ConsoleCmdAbstract
	{
		// Token: 0x0600D26B RID: 53867 RVA: 0x004C3748 File Offset: 0x004C1948
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "invalidatecaches" };
		}

		// Token: 0x0600D26C RID: 53868 RVA: 0x004C3758 File Offset: 0x004C1958
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Invalidate contents of web file caches";
		}

		// Token: 0x0600D26D RID: 53869 RVA: 0x004C375F File Offset: 0x004C195F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "TODO";
		}

		// Token: 0x0600D26E RID: 53870 RVA: 0x004C3768 File Offset: 0x004C1968
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			ValueTuple<int, int> valueTuple = AbstractCache.InvalidateAllCaches();
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Caches invalidated, dropped {0} files with {1} Bytes", item, item2));
		}
	}
}
