using System;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x020014C2 RID: 5314
public class TextEllipsisAnimator
{
	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x0600A6F8 RID: 42744 RVA: 0x003E135A File Offset: 0x003DF55A
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

	// Token: 0x0600A6F9 RID: 42745 RVA: 0x003E1368 File Offset: 0x003DF568
	public TextEllipsisAnimator(string _input, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A6FA RID: 42746 RVA: 0x003E13E8 File Offset: 0x003DF5E8
	public TextEllipsisAnimator(string _input, XUiV_Label _label, UILabel _uiLabel)
	{
		this.uiLabel = _uiLabel;
		this.xuiLabel = _label;
		this.isChinese = Localization.ActiveLanguage == "schinese" || Localization.ActiveLanguage == "tchinese";
		this.lastEllipsisState = (this.isChinese ? TextEllipsisAnimator.ellipsisEndingsChinese[2] : TextEllipsisAnimator.ellipsisEndings[3]);
		this.Init(_input);
	}

	// Token: 0x0600A6FB RID: 42747 RVA: 0x003E1470 File Offset: 0x003DF670
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

	// Token: 0x0600A6FC RID: 42748 RVA: 0x003E14C8 File Offset: 0x003DF6C8
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

	// Token: 0x0600A6FD RID: 42749 RVA: 0x003E1538 File Offset: 0x003DF738
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

	// Token: 0x0600A6FE RID: 42750 RVA: 0x003E1AFC File Offset: 0x003DFCFC
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

	// Token: 0x0600A6FF RID: 42751 RVA: 0x003E1BF4 File Offset: 0x003DFDF4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsHexDigit(char c)
	{
		return char.IsDigit(c) || (char.ToLower(c) >= 'a' && char.ToLower(c) <= 'f');
	}

	// Token: 0x04007BDD RID: 31709
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndings = new string[] { "[00]...", ".[00]..", "..[00].", "..." };

	// Token: 0x04007BDE RID: 31710
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] ellipsisEndingsChinese = new string[] { "[00]……", "…[00]…", "……" };

	// Token: 0x04007BDF RID: 31711
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isChinese;

	// Token: 0x04007BE0 RID: 31712
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string lastEllipsisState;

	// Token: 0x04007BE1 RID: 31713
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex ellipsisBreakPatternEastern = null;

	// Token: 0x04007BE2 RID: 31714
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ellipsisBreakPattern = new Regex("(\\b\\w+\\b)(\\n\\.\\.\\.|\\.\\n\\.\\.|\\.\\.\\n\\.)");

	// Token: 0x04007BE3 RID: 31715
	[PublicizedFrom(EAccessModifier.Private)]
	public const char ellipsisChar = '…';

	// Token: 0x04007BE4 RID: 31716
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label xuiLabel;

	// Token: 0x04007BE5 RID: 31717
	[PublicizedFrom(EAccessModifier.Private)]
	public UILabel uiLabel;

	// Token: 0x04007BE6 RID: 31718
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentEllipsisState;

	// Token: 0x04007BE7 RID: 31719
	[PublicizedFrom(EAccessModifier.Private)]
	public float ellipsisTimer;

	// Token: 0x04007BE8 RID: 31720
	[PublicizedFrom(EAccessModifier.Private)]
	public int cycles;

	// Token: 0x04007BE9 RID: 31721
	[PublicizedFrom(EAccessModifier.Private)]
	public string baseString;

	// Token: 0x04007BEA RID: 31722
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalEllipsis;

	// Token: 0x04007BEB RID: 31723
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] animatedStrings = new string[4];

	// Token: 0x04007BEC RID: 31724
	[PublicizedFrom(EAccessModifier.Private)]
	public string mostRecentAlphaBB = "[FF]";

	// Token: 0x04007BED RID: 31725
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb;

	// Token: 0x020014C3 RID: 5315
	public enum AnimationMode
	{
		// Token: 0x04007BEF RID: 31727
		Off,
		// Token: 0x04007BF0 RID: 31728
		Final,
		// Token: 0x04007BF1 RID: 31729
		All
	}
}
