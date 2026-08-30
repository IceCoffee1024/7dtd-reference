using System;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x020014A7 RID: 5287
public class TextEllipsisAnimator
{
	// Token: 0x1700135E RID: 4958
	// (get) Token: 0x0600A5FB RID: 42491 RVA: 0x003DC01A File Offset: 0x003DA21A
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

	// Token: 0x0600A5FC RID: 42492 RVA: 0x003DC028 File Offset: 0x003DA228
	public TextEllipsisAnimator(string _input, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A5FD RID: 42493 RVA: 0x003DC0A8 File Offset: 0x003DA2A8
	public TextEllipsisAnimator(string _input, XUiV_Label _label, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.xuiLabel = _label;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A5FE RID: 42494 RVA: 0x003DC130 File Offset: 0x003DA330
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

	// Token: 0x0600A5FF RID: 42495 RVA: 0x003DC188 File Offset: 0x003DA388
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

	// Token: 0x0600A600 RID: 42496 RVA: 0x003DC1F8 File Offset: 0x003DA3F8
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

	// Token: 0x0600A601 RID: 42497 RVA: 0x003DC7BC File Offset: 0x003DA9BC
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

	// Token: 0x0600A602 RID: 42498 RVA: 0x003DC8B4 File Offset: 0x003DAAB4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsHexDigit(char c)
	{
		return char.IsDigit(c) || (char.ToLower(c) >= 'a' && char.ToLower(c) <= 'f');
	}

	// Token: 0x04007B04 RID: 31492
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndings = new string[] { "[00]...", ".[00]..", "..[00].", "..." };

	// Token: 0x04007B05 RID: 31493
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndingsChinese = new string[] { "[00]……", "…[00]…", "……" };

	// Token: 0x04007B06 RID: 31494
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isChinese;

	// Token: 0x04007B07 RID: 31495
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string lastEllipsisState;

	// Token: 0x04007B08 RID: 31496
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex ellipsisBreakPatternEastern = null;

	// Token: 0x04007B09 RID: 31497
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ellipsisBreakPattern = new Regex("(\\b\\w+\\b)(\\n\\.\\.\\.|\\.\\n\\.\\.|\\.\\.\\n\\.)");

	// Token: 0x04007B0A RID: 31498
	[PublicizedFrom(EAccessModifier.Private)]
	public const char ellipsisChar = '…';

	// Token: 0x04007B0B RID: 31499
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label xuiLabel;

	// Token: 0x04007B0C RID: 31500
	[PublicizedFrom(EAccessModifier.Private)]
	public UILabel uiLabel;

	// Token: 0x04007B0D RID: 31501
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentEllipsisState;

	// Token: 0x04007B0E RID: 31502
	[PublicizedFrom(EAccessModifier.Private)]
	public float ellipsisTimer;

	// Token: 0x04007B0F RID: 31503
	[PublicizedFrom(EAccessModifier.Private)]
	public int cycles;

	// Token: 0x04007B10 RID: 31504
	[PublicizedFrom(EAccessModifier.Private)]
	public string baseString;

	// Token: 0x04007B11 RID: 31505
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalEllipsis;

	// Token: 0x04007B12 RID: 31506
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] animatedStrings = new string[4];

	// Token: 0x04007B13 RID: 31507
	[PublicizedFrom(EAccessModifier.Private)]
	public string mostRecentAlphaBB = "[FF]";

	// Token: 0x04007B14 RID: 31508
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb;

	// Token: 0x020014A8 RID: 5288
	public enum AnimationMode
	{
		// Token: 0x04007B16 RID: 31510
		Off,
		// Token: 0x04007B17 RID: 31511
		Final,
		// Token: 0x04007B18 RID: 31512
		All
	}
}
