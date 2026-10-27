using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000B0B RID: 2827
public sealed class GameStageGroup
{
	// Token: 0x060053AA RID: 21418 RVA: 0x00202484 File Offset: 0x00200684
	public GameStageGroup(GameStageDefinition _spawner)
	{
		this.spawner = _spawner;
	}

	// Token: 0x060053AB RID: 21419 RVA: 0x00202494 File Offset: 0x00200694
	public static void AddGameStageGroup(string _fullName, GameStageGroup _group)
	{
		string text = GameStageGroup.CleanName(_fullName);
		GameStageGroup.groups.Add(text, _group);
		GameStageGroup.groupsFullName.Add(_fullName, _group);
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x002024C0 File Offset: 0x002006C0
	public static GameStageGroup TryGet(string _name)
	{
		GameStageGroup gameStageGroup;
		if (GameStageGroup.groups.TryGetValue(_name, out gameStageGroup))
		{
			return gameStageGroup;
		}
		return null;
	}

	// Token: 0x060053AD RID: 21421 RVA: 0x002024DF File Offset: 0x002006DF
	public static void Clear()
	{
		GameStageGroup.groups.Clear();
		GameStageGroup.groupsFullName.Clear();
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x060053AE RID: 21422 RVA: 0x002024F5 File Offset: 0x002006F5
	public static Dictionary<string, GameStageGroup> Groups
	{
		get
		{
			return GameStageGroup.groupsFullName;
		}
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x002024FC File Offset: 0x002006FC
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

	// Token: 0x060053B0 RID: 21424 RVA: 0x00202564 File Offset: 0x00200764
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

	// Token: 0x04004195 RID: 16789
	public const string cDefaultGroupName = "GroupGenericZombie";

	// Token: 0x04004196 RID: 16790
	public readonly GameStageDefinition spawner;

	// Token: 0x04004197 RID: 16791
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groups = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004198 RID: 16792
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groupsFullName = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004199 RID: 16793
	[PublicizedFrom(EAccessModifier.Private)]
	public static StringBuilder stringBuilder = new StringBuilder();
}
