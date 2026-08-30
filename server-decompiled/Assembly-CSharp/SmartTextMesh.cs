using System;
using System.Text;
using UnityEngine;

// Token: 0x02001370 RID: 4976
[RequireComponent(typeof(TextMesh))]
public class SmartTextMesh : MonoBehaviour
{
	// Token: 0x17001295 RID: 4757
	// (get) Token: 0x06009CED RID: 40173 RVA: 0x003B450B File Offset: 0x003B270B
	// (set) Token: 0x06009CEE RID: 40174 RVA: 0x003B4513 File Offset: 0x003B2713
	public string UnwrappedText
	{
		get
		{
			return this.unwrappedText;
		}
		set
		{
			if (value != this.unwrappedText)
			{
				this.unwrappedText = value ?? "";
				this.NeedsLayout = true;
			}
		}
	}

	// Token: 0x17001296 RID: 4758
	// (get) Token: 0x06009CEF RID: 40175 RVA: 0x003B453A File Offset: 0x003B273A
	// (set) Token: 0x06009CF0 RID: 40176 RVA: 0x003B4548 File Offset: 0x003B2748
	public float MaxWidthReal
	{
		get
		{
			return this.MaxWidth * 2f;
		}
		set
		{
			this.MaxWidth = value / 2f;
		}
	}

	// Token: 0x06009CF1 RID: 40177 RVA: 0x003B4558 File Offset: 0x003B2758
	public void Start()
	{
		this.TheMesh = base.GetComponent<TextMesh>();
		if (this.ConvertNewLines && this.UnwrappedText != null)
		{
			this.UnwrappedText = this.UnwrappedText.Replace("\\n", "\n");
		}
		if (this.UnwrappedText == null)
		{
			this.UnwrappedText = "";
		}
	}

	// Token: 0x06009CF2 RID: 40178 RVA: 0x003B45B4 File Offset: 0x003B27B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (!this.NeedsLayout)
		{
			return;
		}
		this.NeedsLayout = false;
		if (this.MaxWidth == 0f)
		{
			this.TheMesh.text = this.UnwrappedText;
			return;
		}
		if (this.SeperatedLinesMode)
		{
			this.FormatSeparateLines();
			return;
		}
		this.WrapTextToWidth();
	}

	// Token: 0x06009CF3 RID: 40179 RVA: 0x003B4608 File Offset: 0x003B2808
	public bool CanRenderString(string _text)
	{
		foreach (char c in _text.Trim())
		{
			if (c != '\n' && !this.TheMesh.font.HasCharacter(c))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06009CF4 RID: 40180 RVA: 0x003B4650 File Offset: 0x003B2850
	public unsafe void WrapTextToWidth()
	{
		this.TheMesh.font.RequestCharactersInTexture(this.unwrappedText, this.TheMesh.fontSize, this.TheMesh.fontStyle);
		float textWidth = this.GetTextWidth(this.TheMesh, " ");
		ReadOnlySpan<char> readOnlySpan = this.unwrappedText;
		readOnlySpan = readOnlySpan.Trim();
		int num = 0;
		int num2 = 1;
		int num3 = 0;
		float num4 = 0f;
		this.wrappedText.Clear();
		while (num < readOnlySpan.Length && num2 <= this.MaxLines)
		{
			int num5 = readOnlySpan.Slice(num).IndexOfAny(' ', '\n');
			if (num5 < 0)
			{
				num5 = readOnlySpan.Length;
			}
			else
			{
				num5 += num;
			}
			ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(num, num5 - num);
			float textWidth2 = this.GetTextWidth(this.TheMesh, readOnlySpan2);
			float num6 = ((num4 > 0f) ? (num4 + textWidth + textWidth2) : textWidth2);
			if (num6 > this.MaxWidthReal)
			{
				if (num4 > 0f)
				{
					this.wrappedText.Append(readOnlySpan.Slice(num3, num - 1 - num3));
					this.wrappedText.Append('\n');
					num3 = (num = num);
					num2++;
					num4 = 0f;
				}
				else
				{
					float num7 = 1.2f * textWidth2 / this.MaxWidthReal;
					int num8 = (int)((float)readOnlySpan2.Length / num7);
					this.wrappedText.Append(readOnlySpan2.Slice(0, num8));
					this.wrappedText.Append('…');
					this.wrappedText.Append('\n');
					num3 = (num = num5 + 1);
					num2++;
					num4 = 0f;
				}
			}
			else if (num5 >= readOnlySpan.Length || *readOnlySpan[num5] == 10)
			{
				this.wrappedText.Append(readOnlySpan.Slice(num3, num5 - num3));
				this.wrappedText.Append('\n');
				num2++;
				num3 = (num = num5 + 1);
				num4 = 0f;
			}
			else
			{
				num4 = num6;
				num = num5 + 1;
			}
		}
		if (this.wrappedText.Length > 0 && this.wrappedText[this.wrappedText.Length - 1] == '\n')
		{
			num2--;
			this.wrappedText.Length--;
			if (num < readOnlySpan.Length)
			{
				this.wrappedText.Append('…');
			}
		}
		this.TheMesh.text = this.wrappedText.ToString();
	}

	// Token: 0x06009CF5 RID: 40181 RVA: 0x003B48DC File Offset: 0x003B2ADC
	[PublicizedFrom(EAccessModifier.Private)]
	public void FormatSeparateLines()
	{
		this.TheMesh.font.RequestCharactersInTexture(this.unwrappedText, this.TheMesh.fontSize, this.TheMesh.fontStyle);
		float textWidth = this.GetTextWidth(this.TheMesh, " ");
		ReadOnlySpan<char> readOnlySpan = this.unwrappedText;
		readOnlySpan = readOnlySpan.Trim();
		this.wrappedText.Clear();
		string[] array = readOnlySpan.ToString().Split('\n', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(' ', StringSplitOptions.None);
			float num = 0f;
			for (int j = 0; j < array2.Length; j++)
			{
				ReadOnlySpan<char> readOnlySpan2 = array2[j];
				float textWidth2 = this.GetTextWidth(this.TheMesh, readOnlySpan2);
				float num2 = ((num > 0f) ? (num + textWidth + textWidth2) : textWidth2);
				if (num2 > this.MaxWidthReal)
				{
					this.wrappedText.Append('…');
					break;
				}
				if (j != 0)
				{
					this.wrappedText.Append(' ');
				}
				this.wrappedText.Append(readOnlySpan2);
				num = num2;
			}
			if (i < array.Length - 1)
			{
				this.wrappedText.Append('\n');
			}
		}
		this.TheMesh.text = this.wrappedText.ToString();
	}

	// Token: 0x06009CF6 RID: 40182 RVA: 0x003B4A40 File Offset: 0x003B2C40
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe float GetTextWidth(TextMesh _textMesh, ReadOnlySpan<char> _s)
	{
		Font font = _textMesh.font;
		int fontSize = _textMesh.fontSize;
		FontStyle fontStyle = _textMesh.fontStyle;
		int num = 0;
		ReadOnlySpan<char> readOnlySpan = _s;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			char c = (char)(*readOnlySpan[i]);
			CharacterInfo characterInfo;
			if (font.GetCharacterInfo(c, out characterInfo, fontSize, fontStyle))
			{
				num += characterInfo.advance;
			}
			else
			{
				Log.Warning(string.Format("No character info for symbol '{0}'", c));
			}
		}
		return (float)num * _textMesh.characterSize * Mathf.Abs(_textMesh.transform.lossyScale.x) * 0.1f;
	}

	// Token: 0x040076C8 RID: 30408
	public TextMesh TheMesh;

	// Token: 0x040076C9 RID: 30409
	[TextArea]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public string unwrappedText;

	// Token: 0x040076CA RID: 30410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder wrappedText = new StringBuilder();

	// Token: 0x040076CB RID: 30411
	public float MaxWidth;

	// Token: 0x040076CC RID: 30412
	public int MaxLines;

	// Token: 0x040076CD RID: 30413
	public bool NeedsLayout = true;

	// Token: 0x040076CE RID: 30414
	public bool ConvertNewLines;

	// Token: 0x040076CF RID: 30415
	public bool SeperatedLinesMode;
}
