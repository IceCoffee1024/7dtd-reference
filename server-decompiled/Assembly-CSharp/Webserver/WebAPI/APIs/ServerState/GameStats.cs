using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B06 RID: 6918
	[Preserve]
	public class GameStats : KeyValueListAbs
	{
		// Token: 0x0600D047 RID: 53319 RVA: 0x004B9EBC File Offset: 0x004B80BC
		public GameStats()
			: base("GameStats")
		{
		}

		// Token: 0x0600D048 RID: 53320 RVA: 0x004B9ECC File Offset: 0x004B80CC
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
