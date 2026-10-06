using System;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x020014E4 RID: 5348
public class TextEllipsisAnimator
{
	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600A7AB RID: 42923 RVA: 0x003E61AE File Offset: 0x003E43AE
	public int animationStates
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.isChinese)
			{
				return 4;
			}
			return 3;
		}
	}

	// Token: 0x0600A7AC RID: 42924 RVA: 0x003E61BC File Offset: 0x003E43BC
	public TextEllipsisAnimator(string _input, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A7AD RID: 42925 RVA: 0x003E623C File Offset: 0x003E443C
	public TextEllipsisAnimator(string _input, XUiV_Label _label, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.xuiLabel = _label;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A7AE RID: 42926 RVA: 0x003E62C4 File Offset: 0x003E44C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init(string _input)
	{
		this.currentEllipsisState = this.animationStates - 1;
		this.sb = new StringBuilder();
		if (TextEllipsisAnimator.ellipsisBreakPatternEastern == null)
		{
			TextEllipsisAnimator.ellipsisBreakPatternEastern = (this.isChinese ? new Regex("([\\u3008-\\u9FFF])(\\u2026\\n\\u2026)") : new Regex("([\\u3008-\\u9FFF])(\\.\\n\\.\\.|\\.\\.\\n\\.)"));
		}
		this.SetBaseString(_input, TextEllipsisAnimator.AnimationMode.All);
	}

	// Token: 0x0600A7AF RID: 42927 RVA: 0x003E631C File Offset: 0x003E451C
	public void GetNextAnimatedString(float _dt)
	{
		if (this.totalEllipsis == 0 || this.baseString == null || this.uiLabel == null)
		{
			return;
		}
		this.ellipsisTimer += _dt;
		if (this.ellipsisTimer >= 0.5f)
		{
			this.currentEllipsisState = (this.currentEllipsisState + 1) % this.animationStates;
			this.ellipsisTimer = 0f;
			this.UpdateLabel();
		}
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x003E638C File Offset: 0x003E458C
	public void SetBaseString(string _input, TextEllipsisAnimator.AnimationMode _mode = TextEllipsisAnimator.AnimationMode.All)
	{
		if (_input == null || this.uiLabel == null || (this.xuiLabel != null && !this.xuiLabel.SupportBbCode) || _mode == TextEllipsisAnimator.AnimationMode.Off)
		{
			this.baseString = null;
			return;
		}
		this.sb.Clear();
		this.mostRecentAlphaBB = "[FF]";
		this.cycles = 0;
		this.totalEllipsis = 0;
		this.sb.Append(_input);
		if (_mode == TextEllipsisAnimator.AnimationMode.Final)
		{
			int i;
			for (i = this.sb.Length - 1; i > 0; i--)
			{
				if (!char.IsWhiteSpace(this.sb[i]))
				{
					break;
				}
				this.sb.Remove(i, 1);
			}
			while (i >= 3 && this.sb[i] == '.' && this.sb[i - 1] == '.')
			{
				if (this.sb[i - 2] != '.')
				{
					break;
				}
				this.sb.Remove(i - 2, 3);
				i -= 3;
				this.totalEllipsis = 1;
			}
			while (i >= 1)
			{
				if (this.sb[i] != '…')
				{
					break;
				}
				this.sb.Remove(i, 1);
				i--;
				this.totalEllipsis = 1;
			}
			while (i > 0 && char.IsWhiteSpace(this.sb[i]))
			{
				this.sb.Remove(i, 1);
				i--;
			}
			if (this.totalEllipsis > 0)
			{
				this.sb.Append(this.lastEllipsisState);
			}
		}
		else if (_mode == TextEllipsisAnimator.AnimationMode.All)
		{
			for (int n = 0; n < this.sb.Length; n++)
			{
				if (this.sb[n] == '[' && n + 3 < this.sb.Length && this.sb[n + 3] == ']' && this.IsHexDigit(this.sb[n + 1]) && this.IsHexDigit(this.sb[n + 2]))
				{
					this.mostRecentAlphaBB = this.sb[n].ToString() + this.sb[n + 1].ToString() + this.sb[n + 2].ToString() + this.sb[n + 3].ToString();
					n += 3;
				}
				if (this.sb[n] == '…')
				{
					int num = 0;
					while (n + num + 1 < this.sb.Length && this.sb[n + num + 1] == '…')
					{
						num++;
					}
					if (num > 0)
					{
						this.sb.Remove(n + 1, num);
					}
					this.sb.Remove(n, 1);
					this.sb.Insert(n, this.lastEllipsisState + this.mostRecentAlphaBB);
					n += this.lastEllipsisState.Length - 1 + 4;
					this.totalEllipsis++;
				}
			}
			for (int k = 0; k < this.sb.Length - 2; k++)
			{
				if (this.sb[k] == '[' && k + 3 < this.sb.Length && this.sb[k + 3] == ']' && this.IsHexDigit(this.sb[k + 1]) && this.IsHexDigit(this.sb[k + 2]))
				{
					this.mostRecentAlphaBB = this.sb[k].ToString() + this.sb[k + 1].ToString() + this.sb[k + 2].ToString() + this.sb[k + 3].ToString();
					k += 3;
				}
				if (this.sb[k] == '.' && this.sb[k + 1] == '.' && this.sb[k + 2] == '.')
				{
					this.sb.Remove(k, 3);
					this.sb.Insert(k, this.lastEllipsisState + this.mostRecentAlphaBB);
					k += 3 - this.lastEllipsisState.Length + 4;
					this.totalEllipsis++;
				}
			}
		}
		this.baseString = this.sb.ToString();
		XUiV_Label xuiV_Label = this.xuiLabel;
		if (xuiV_Label != null)
		{
			xuiV_Label.SetTextImmediately(this.baseString);
		}
		int processedMatches = 0;
		for (int l = 0; l < this.totalEllipsis; l++)
		{
			if (this.xuiLabel != null && this.xuiLabel.Text == this.uiLabel.processedText)
			{
				break;
			}
			bool hadEastern = false;
			int j = 0;
			this.baseString = TextEllipsisAnimator.ellipsisBreakPatternEastern.Replace(this.uiLabel.processedText, delegate(Match m)
			{
				int num2;
				if (j == processedMatches)
				{
					hadEastern = true;
					num2 = processedMatches;
					processedMatches = num2 + 1;
					return m.Groups[1].Value + "\n" + this.lastEllipsisState;
				}
				num2 = j;
				j = num2 + 1;
				return m.Value;
			});
			if (!hadEastern)
			{
				j = 0;
				this.baseString = TextEllipsisAnimator.ellipsisBreakPattern.Replace(this.uiLabel.processedText, delegate(Match m)
				{
					int num3;
					if (j == processedMatches)
					{
						num3 = processedMatches;
						processedMatches = num3 + 1;
						return "\n" + m.Groups[1].Value + this.lastEllipsisState;
					}
					num3 = j;
					j = num3 + 1;
					return m.Value;
				});
				XUiV_Label xuiV_Label2 = this.xuiLabel;
				if (xuiV_Label2 != null)
				{
					xuiV_Label2.SetTextImmediately(this.baseString);
				}
			}
		}
		this.UpdateLabel();
	}

	// Token: 0x0600A7B1 RID: 42929 RVA: 0x003E6950 File Offset: 0x003E4B50
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLabel()
	{
		if (this.totalEllipsis > 0)
		{
			if (this.cycles < this.animationStates)
			{
				this.cycles++;
				if (this.isChinese)
				{
					this.animatedStrings[this.currentEllipsisState] = this.baseString.Replace(this.lastEllipsisState, TextEllipsisAnimator.ellipsisEndingsChinese[this.currentEllipsisState]);
				}
				else
				{
					this.animatedStrings[this.currentEllipsisState] = this.baseString.Replace(this.lastEllipsisState, TextEllipsisAnimator.ellipsisEndings[this.currentEllipsisState]);
				}
			}
			if (this.xuiLabel != null)
			{
				this.xuiLabel.SetTextImmediately(this.animatedStrings[this.currentEllipsisState]);
				return;
			}
			this.uiLabel.text = this.animatedStrings[this.currentEllipsisState];
			return;
		}
		else
		{
			if (this.xuiLabel != null)
			{
				this.xuiLabel.SetTextImmediately(this.baseString);
				return;
			}
			this.uiLabel.text = this.baseString;
			return;
		}
	}

	// Token: 0x0600A7B2 RID: 42930 RVA: 0x003E6A48 File Offset: 0x003E4C48
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsHexDigit(char c)
	{
		return char.IsDigit(c) || (char.ToLower(c) >= 'a' && char.ToLower(c) <= 'f');
	}

	// Token: 0x04007C60 RID: 31840
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndings = new string[] { "[00]...", ".[00]..", "..[00].", "..." };

	// Token: 0x04007C61 RID: 31841
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndingsChinese = new string[] { "[00]……", "…[00]…", "……" };

	// Token: 0x04007C62 RID: 31842
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isChinese;

	// Token: 0x04007C63 RID: 31843
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string lastEllipsisState;

	// Token: 0x04007C64 RID: 31844
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex ellipsisBreakPatternEastern = null;

	// Token: 0x04007C65 RID: 31845
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ellipsisBreakPattern = new Regex("(\\b\\w+\\b)(\\n\\.\\.\\.|\\.\\n\\.\\.|\\.\\.\\n\\.)");

	// Token: 0x04007C66 RID: 31846
	[PublicizedFrom(EAccessModifier.Private)]
	public const char ellipsisChar = '…';

	// Token: 0x04007C67 RID: 31847
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label xuiLabel;

	// Token: 0x04007C68 RID: 31848
	[PublicizedFrom(EAccessModifier.Private)]
	public UILabel uiLabel;

	// Token: 0x04007C69 RID: 31849
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentEllipsisState;

	// Token: 0x04007C6A RID: 31850
	[PublicizedFrom(EAccessModifier.Private)]
	public float ellipsisTimer;

	// Token: 0x04007C6B RID: 31851
	[PublicizedFrom(EAccessModifier.Private)]
	public int cycles;

	// Token: 0x04007C6C RID: 31852
	[PublicizedFrom(EAccessModifier.Private)]
	public string baseString;

	// Token: 0x04007C6D RID: 31853
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalEllipsis;

	// Token: 0x04007C6E RID: 31854
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] animatedStrings = new string[4];

	// Token: 0x04007C6F RID: 31855
	[PublicizedFrom(EAccessModifier.Private)]
	public string mostRecentAlphaBB = "[FF]";

	// Token: 0x04007C70 RID: 31856
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb;

	// Token: 0x020014E5 RID: 5349
	public enum AnimationMode
	{
		// Token: 0x04007C72 RID: 31858
		Off,
		// Token: 0x04007C73 RID: 31859
		Final,
		// Token: 0x04007C74 RID: 31860
		All
	}
}
