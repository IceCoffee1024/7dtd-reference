using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.FileCache
{
	// Token: 0x02001AE3 RID: 6883
	[Preserve]
	public class InvalidateCachesCmd : ConsoleCmdAbstract
	{
		// Token: 0x0600CE76 RID: 52854 RVA: 0x004B5360 File Offset: 0x004B3560
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "invalidatecaches" };
		}

		// Token: 0x0600CE77 RID: 52855 RVA: 0x004B5370 File Offset: 0x004B3570
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Invalidate contents of web file caches";
		}

		// Token: 0x0600CE78 RID: 52856 RVA: 0x004B5377 File Offset: 0x004B3577
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "TODO";
		}

		// Token: 0x0600CE79 RID: 52857 RVA: 0x004B5380 File Offset: 0x004B3580
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			ValueTuple<int, int> valueTuple = AbstractCache.InvalidateAllCaches();
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Caches invalidated, dropped {0} files with {1} Bytes", item, item2));
		}
	}
}
