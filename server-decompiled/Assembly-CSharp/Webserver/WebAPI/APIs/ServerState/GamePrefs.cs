using System;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AD9 RID: 6873
	[Preserve]
	public class GamePrefs : KeyValueListAbs
	{
		// Token: 0x0600CEED RID: 52973 RVA: 0x004B58C2 File Offset: 0x004B3AC2
		public GamePrefs()
			: base("GamePrefs")
		{
		}

		// Token: 0x0600CEEE RID: 52974 RVA: 0x004B58D0 File Offset: 0x004B3AD0
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
