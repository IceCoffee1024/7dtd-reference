using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000ADC RID: 2780
public sealed class GameStageGroup
{
	// Token: 0x060052E4 RID: 21220 RVA: 0x001FB030 File Offset: 0x001F9230
	public GameStageGroup(GameStageDefinition _spawner)
	{
		this.spawner = _spawner;
	}

	// Token: 0x060052E5 RID: 21221 RVA: 0x001FB040 File Offset: 0x001F9240
	public static void AddGameStageGroup(string _fullName, GameStageGroup _group)
	{
		string text = GameStageGroup.CleanName(_fullName);
		GameStageGroup.groups.Add(text, _group);
		GameStageGroup.groupsFullName.Add(_fullName, _group);
	}

	// Token: 0x060052E6 RID: 21222 RVA: 0x001FB06C File Offset: 0x001F926C
	public static GameStageGroup TryGet(string _name)
	{
		GameStageGroup gameStageGroup;
		if (GameStageGroup.groups.TryGetValue(_name, out gameStageGroup))
		{
			return gameStageGroup;
		}
		return null;
	}

	// Token: 0x060052E7 RID: 21223 RVA: 0x001FB08B File Offset: 0x001F928B
	public static void Clear()
	{
		GameStageGroup.groups.Clear();
		GameStageGroup.groupsFullName.Clear();
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x060052E8 RID: 21224 RVA: 0x001FB0A1 File Offset: 0x001F92A1
	public static Dictionary<string, GameStageGroup> Groups
	{
		get
		{
			return GameStageGroup.groupsFullName;
		}
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001FB0A8 File Offset: 0x001F92A8
	public static string CleanName(string _name)
	{
		if (_name.Length > 0 && char.IsDigit(_name[0]))
		{
			_name = _name.Substring(1);
		}
		else if (_name.StartsWith("S_"))
		{
			int num = 2;
			if (_name.StartsWith("S_-"))
			{
				num = 3;
			}
			return _name.Substring(num).Replace("_", "");
		}
		return _name;
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001FB110 File Offset: 0x001F9310
	public static string MakeDisplayName(string _name)
	{
		bool flag = false;
		foreach (char c in _name)
		{
			if (!char.IsDigit(c))
			{
				if (char.IsUpper(c) && flag)
				{
					GameStageGroup.stringBuilder.Append(' ');
				}
				GameStageGroup.stringBuilder.Append(c);
				flag = true;
			}
		}
		_name = GameStageGroup.stringBuilder.ToString();
		GameStageGroup.stringBuilder.Clear();
		return _name;
	}

	// Token: 0x0400409D RID: 16541
	public const string cDefaultGroupName = "GroupGenericZombie";

	// Token: 0x0400409E RID: 16542
	public readonly GameStageDefinition spawner;

	// Token: 0x0400409F RID: 16543
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groups = new Dictionary<string, GameStageGroup>();

	// Token: 0x040040A0 RID: 16544
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groupsFullName = new Dictionary<string, GameStageGroup>();

	// Token: 0x040040A1 RID: 16545
	[PublicizedFrom(EAccessModifier.Private)]
	public static StringBuilder stringBuilder = new StringBuilder();
}
