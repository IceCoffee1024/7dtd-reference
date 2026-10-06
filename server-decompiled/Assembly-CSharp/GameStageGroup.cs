using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000B00 RID: 2816
public sealed class GameStageGroup
{
	// Token: 0x06005385 RID: 21381 RVA: 0x00200F00 File Offset: 0x001FF100
	public GameStageGroup(GameStageDefinition _spawner)
	{
		this.spawner = _spawner;
	}

	// Token: 0x06005386 RID: 21382 RVA: 0x00200F10 File Offset: 0x001FF110
	public static void AddGameStageGroup(string _fullName, GameStageGroup _group)
	{
		string text = GameStageGroup.CleanName(_fullName);
		GameStageGroup.groups.Add(text, _group);
		GameStageGroup.groupsFullName.Add(_fullName, _group);
	}

	// Token: 0x06005387 RID: 21383 RVA: 0x00200F3C File Offset: 0x001FF13C
	public static GameStageGroup TryGet(string _name)
	{
		GameStageGroup gameStageGroup;
		if (GameStageGroup.groups.TryGetValue(_name, out gameStageGroup))
		{
			return gameStageGroup;
		}
		return null;
	}

	// Token: 0x06005388 RID: 21384 RVA: 0x00200F5B File Offset: 0x001FF15B
	public static void Clear()
	{
		GameStageGroup.groups.Clear();
		GameStageGroup.groupsFullName.Clear();
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06005389 RID: 21385 RVA: 0x00200F71 File Offset: 0x001FF171
	public static Dictionary<string, GameStageGroup> Groups
	{
		get
		{
			return GameStageGroup.groupsFullName;
		}
	}

	// Token: 0x0600538A RID: 21386 RVA: 0x00200F78 File Offset: 0x001FF178
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

	// Token: 0x0600538B RID: 21387 RVA: 0x00200FE0 File Offset: 0x001FF1E0
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

	// Token: 0x04004170 RID: 16752
	public const string cDefaultGroupName = "GroupGenericZombie";

	// Token: 0x04004171 RID: 16753
	public readonly GameStageDefinition spawner;

	// Token: 0x04004172 RID: 16754
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groups = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004173 RID: 16755
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groupsFullName = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004174 RID: 16756
	[PublicizedFrom(EAccessModifier.Private)]
	public static StringBuilder stringBuilder = new StringBuilder();
}
