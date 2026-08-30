using System;
using System.IO;

// Token: 0x02000C42 RID: 3138
public class AuthoredText
{
	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x06005F89 RID: 24457 RVA: 0x0025DD2F File Offset: 0x0025BF2F
	// (set) Token: 0x06005F8A RID: 24458 RVA: 0x0025DD37 File Offset: 0x0025BF37
	public string Text
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06005F8B RID: 24459 RVA: 0x0025DD40 File Offset: 0x0025BF40
	// (set) Token: 0x06005F8C RID: 24460 RVA: 0x0025DD48 File Offset: 0x0025BF48
	public PlatformUserIdentifierAbs Author
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x0025DD51 File Offset: 0x0025BF51
	public AuthoredText()
	{
		this.Update(string.Empty, null);
	}

	// Token: 0x06005F8E RID: 24462 RVA: 0x0025DD65 File Offset: 0x0025BF65
	public AuthoredText(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Update(_text, _author);
	}

	// Token: 0x06005F8F RID: 24463 RVA: 0x0025DD75 File Offset: 0x0025BF75
	public void Update(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Author = _author;
		this.Text = _text;
	}

	// Token: 0x06005F90 RID: 24464 RVA: 0x0025DD88 File Offset: 0x0025BF88
	public static AuthoredText FromStream(BinaryReader _reader)
	{
		if (_reader == null)
		{
			return null;
		}
		if (!_reader.ReadBoolean())
		{
			return null;
		}
		string text = _reader.ReadString();
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
		AuthoredText authoredText = new AuthoredText();
		authoredText.Update(text, platformUserIdentifierAbs);
		return authoredText;
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x0025DDC1 File Offset: 0x0025BFC1
	public static void ToStream(AuthoredText _instance, BinaryWriter _writer)
	{
		if (_writer == null)
		{
			return;
		}
		if (_instance == null)
		{
			_writer.Write(0);
			return;
		}
		_writer.Write(1);
		_writer.Write(_instance.Text);
		_instance.Author.ToStream(_writer, false);
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x0025DDF2 File Offset: 0x0025BFF2
	public AuthoredText Clone()
	{
		return new AuthoredText(this.Text, this.Author);
	}
}
