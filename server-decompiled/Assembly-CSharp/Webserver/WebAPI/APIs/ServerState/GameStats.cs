using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001ADA RID: 6874
	[Preserve]
	public class GameStats : KeyValueListAbs
	{
		// Token: 0x0600CEEF RID: 52975 RVA: 0x004B59F0 File Offset: 0x004B3BF0
		public GameStats()
			: base("GameStats")
		{
		}

		// Token: 0x0600CEF0 RID: 52976 RVA: 0x004B5A00 File Offset: 0x004B3C00
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
