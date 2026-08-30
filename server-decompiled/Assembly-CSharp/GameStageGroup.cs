using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000AEE RID: 2798
public sealed class GameStageGroup
{
	// Token: 0x0600536C RID: 21356 RVA: 0x001FDCA0 File Offset: 0x001FBEA0
	public GameStageGroup(GameStageDefinition _spawner)
	{
		this.spawner = _spawner;
	}

	// Token: 0x0600536D RID: 21357 RVA: 0x001FDCB0 File Offset: 0x001FBEB0
	public static void AddGameStageGroup(string _fullName, GameStageGroup _group)
	{
		string text = GameStageGroup.CleanName(_fullName);
		GameStageGroup.groups.Add(text, _group);
		GameStageGroup.groupsFullName.Add(_fullName, _group);
	}

	// Token: 0x0600536E RID: 21358 RVA: 0x001FDCDC File Offset: 0x001FBEDC
	public static GameStageGroup TryGet(string _name)
	{
		GameStageGroup gameStageGroup;
		if (GameStageGroup.groups.TryGetValue(_name, out gameStageGroup))
		{
			return gameStageGroup;
		}
		return null;
	}

	// Token: 0x0600536F RID: 21359 RVA: 0x001FDCFB File Offset: 0x001FBEFB
	public static void Clear()
	{
		GameStageGroup.groups.Clear();
		GameStageGroup.groupsFullName.Clear();
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x06005370 RID: 21360 RVA: 0x001FDD11 File Offset: 0x001FBF11
	public static Dictionary<string, GameStageGroup> Groups
	{
		get
		{
			return GameStageGroup.groupsFullName;
		}
	}

	// Token: 0x06005371 RID: 21361 RVA: 0x001FDD18 File Offset: 0x001FBF18
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

	// Token: 0x06005372 RID: 21362 RVA: 0x001FDD80 File Offset: 0x001FBF80
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

	// Token: 0x04004133 RID: 16691
	public const string cDefaultGroupName = "GroupGenericZombie";

	// Token: 0x04004134 RID: 16692
	public readonly GameStageDefinition spawner;

	// Token: 0x04004135 RID: 16693
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groups = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004136 RID: 16694
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, GameStageGroup> groupsFullName = new Dictionary<string, GameStageGroup>();

	// Token: 0x04004137 RID: 16695
	[PublicizedFrom(EAccessModifier.Private)]
	public static StringBuilder stringBuilder = new StringBuilder();
}
