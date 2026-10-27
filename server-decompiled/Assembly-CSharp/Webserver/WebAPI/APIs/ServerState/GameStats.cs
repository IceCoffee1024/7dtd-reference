using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B37 RID: 6967
	[Preserve]
	public class GameStats : KeyValueListAbs
	{
		// Token: 0x0600D15B RID: 53595 RVA: 0x004BD9B8 File Offset: 0x004BBBB8
		public GameStats()
			: base("GameStats")
		{
		}

		// Token: 0x0600D15C RID: 53596 RVA: 0x004BD9C8 File Offset: 0x004BBBC8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void iterateList(ref JsonWriter _writer, ref bool _first)
		{
			foreach (EnumGameStats enumGameStats in EnumUtils.Values<EnumGameStats>())
			{
				string text = enumGameStats.ToStringCached<EnumGameStats>();
				GameStats.EnumType? statType = GameStats.GetStatType(enumGameStats);
				if (statType != null)
				{
					switch (statType.GetValueOrDefault())
					{
					case GameStats.EnumType.Int:
						base.addItem(ref _writer, ref _first, text, GameStats.GetInt(enumGameStats));
						break;
					case GameStats.EnumType.Float:
						base.addItem(ref _writer, ref _first, text, GameStats.GetFloat(enumGameStats));
						break;
					case GameStats.EnumType.String:
						base.addItem(ref _writer, ref _first, text, GameStats.GetString(enumGameStats));
						break;
					case GameStats.EnumType.Bool:
						base.addItem(ref _writer, ref _first, text, GameStats.GetBool(enumGameStats));
						break;
					}
				}
			}
		}
	}
}
