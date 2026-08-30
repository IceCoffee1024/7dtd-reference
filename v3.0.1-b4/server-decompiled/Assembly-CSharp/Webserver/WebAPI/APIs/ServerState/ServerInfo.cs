using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AB8 RID: 6840
	[Preserve]
	public class ServerInfo : KeyValueListAbs
	{
		// Token: 0x0600CD7E RID: 52606 RVA: 0x004AFD25 File Offset: 0x004ADF25
		public ServerInfo()
			: base("ServerInfo")
		{
		}

		// Token: 0x0600CD7F RID: 52607 RVA: 0x004AFD34 File Offset: 0x004ADF34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void iterateList(ref JsonWriter _writer, ref bool _first)
		{
			GameServerInfo localServerInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo;
			IList<GameInfoString> list = EnumUtils.Values<GameInfoString>();
			for (int i = 0; i < list.Count; i++)
			{
				GameInfoString gameInfoString = list[i];
				base.addItem(ref _writer, ref _first, gameInfoString.ToStringCached<GameInfoString>(), localServerInfo.GetValue(gameInfoString));
			}
			IList<GameInfoInt> list2 = EnumUtils.Values<GameInfoInt>();
			for (int j = 0; j < list2.Count; j++)
			{
				GameInfoInt gameInfoInt = list2[j];
				base.addItem(ref _writer, ref _first, gameInfoInt.ToStringCached<GameInfoInt>(), localServerInfo.GetValue(gameInfoInt));
			}
			IList<GameInfoBool> list3 = EnumUtils.Values<GameInfoBool>();
			for (int k = 0; k < list3.Count; k++)
			{
				GameInfoBool gameInfoBool = list3[k];
				base.addItem(ref _writer, ref _first, gameInfoBool.ToStringCached<GameInfoBool>(), localServerInfo.GetValue(gameInfoBool));
			}
		}

		// Token: 0x0600CD80 RID: 52608 RVA: 0x004AEB65 File Offset: 0x004ACD65
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
