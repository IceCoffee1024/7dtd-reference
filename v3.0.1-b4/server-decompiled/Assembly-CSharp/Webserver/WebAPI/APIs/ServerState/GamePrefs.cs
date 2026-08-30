using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AB2 RID: 6834
	[Preserve]
	public class GamePrefs : KeyValueListAbs
	{
		// Token: 0x0600CD64 RID: 52580 RVA: 0x004AF4CE File Offset: 0x004AD6CE
		public GamePrefs()
			: base("GamePrefs")
		{
		}

		// Token: 0x0600CD65 RID: 52581 RVA: 0x004AF4DC File Offset: 0x004AD6DC
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
