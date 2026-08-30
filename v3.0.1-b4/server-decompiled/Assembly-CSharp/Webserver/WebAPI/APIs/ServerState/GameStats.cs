using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AB3 RID: 6835
	[Preserve]
	public class GameStats : KeyValueListAbs
	{
		// Token: 0x0600CD66 RID: 52582 RVA: 0x004AF5FC File Offset: 0x004AD7FC
		public GameStats()
			: base("GameStats")
		{
		}

		// Token: 0x0600CD67 RID: 52583 RVA: 0x004AF60C File Offset: 0x004AD80C
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
