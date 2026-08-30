using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001ADF RID: 6879
	[Preserve]
	public class ServerInfo : KeyValueListAbs
	{
		// Token: 0x0600CF07 RID: 52999 RVA: 0x004B6119 File Offset: 0x004B4319
		public ServerInfo()
			: base("ServerInfo")
		{
		}

		// Token: 0x0600CF08 RID: 53000 RVA: 0x004B6128 File Offset: 0x004B4328
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

		// Token: 0x0600CF09 RID: 53001 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
