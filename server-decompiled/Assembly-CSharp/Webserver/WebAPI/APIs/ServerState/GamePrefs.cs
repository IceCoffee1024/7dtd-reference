using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B36 RID: 6966
	[Preserve]
	public class GamePrefs : KeyValueListAbs
	{
		// Token: 0x0600D159 RID: 53593 RVA: 0x004BD88A File Offset: 0x004BBA8A
		public GamePrefs()
			: base("GamePrefs")
		{
		}

		// Token: 0x0600D15A RID: 53594 RVA: 0x004BD898 File Offset: 0x004BBA98
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void iterateList(ref JsonWriter _writer, ref bool _first)
		{
			foreach (EnumGamePrefs enumGamePrefs in EnumUtils.Values<EnumGamePrefs>())
			{
				string text = enumGamePrefs.ToStringCached<EnumGamePrefs>();
				if (!text.Contains("Password", StringComparison.Ordinal))
				{
					GamePrefs.EnumType? prefType = GamePrefs.GetPrefType(enumGamePrefs);
					object @default = GamePrefs.GetDefault(enumGamePrefs);
					if (prefType != null)
					{
						switch (prefType.GetValueOrDefault())
						{
						case GamePrefs.EnumType.Int:
						{
							int? num = @default as int?;
							base.addItem(ref _writer, ref _first, text, GamePrefs.GetInt(enumGamePrefs), num);
							break;
						}
						case GamePrefs.EnumType.Float:
						{
							float? num2 = @default as float?;
							base.addItem(ref _writer, ref _first, text, GamePrefs.GetFloat(enumGamePrefs), num2);
							break;
						}
						case GamePrefs.EnumType.String:
						{
							string text2 = @default as string;
							base.addItem(ref _writer, ref _first, text, GamePrefs.GetString(enumGamePrefs), text2);
							break;
						}
						case GamePrefs.EnumType.Bool:
						{
							bool? flag = @default as bool?;
							base.addItem(ref _writer, ref _first, text, GamePrefs.GetBool(enumGamePrefs), flag);
							break;
						}
						}
					}
				}
			}
		}
	}
}
