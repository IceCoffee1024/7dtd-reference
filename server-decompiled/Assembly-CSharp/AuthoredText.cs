using System;
using System.IO;

// Token: 0x02000C66 RID: 3174
public class AuthoredText
{
	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x0600602C RID: 24620 RVA: 0x00263ECB File Offset: 0x002620CB
	// (set) Token: 0x0600602D RID: 24621 RVA: 0x00263ED3 File Offset: 0x002620D3
	public string Text
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x0600602E RID: 24622 RVA: 0x00263EDC File Offset: 0x002620DC
	// (set) Token: 0x0600602F RID: 24623 RVA: 0x00263EE4 File Offset: 0x002620E4
	public PlatformUserIdentifierAbs Author
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x00263EED File Offset: 0x002620ED
	public AuthoredText()
	{
		this.Update(string.Empty, null);
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x00263F01 File Offset: 0x00262101
	public AuthoredText(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Update(_text, _author);
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x00263F11 File Offset: 0x00262111
	public void Update(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Author = _author;
		this.Text = _text;
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x00263F24 File Offset: 0x00262124
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

	// Token: 0x06006034 RID: 24628 RVA: 0x00263F5D File Offset: 0x0026215D
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

	// Token: 0x06006035 RID: 24629 RVA: 0x00263F8E File Offset: 0x0026218E
	public AuthoredText Clone()
	{
		return new AuthoredText(this.Text, this.Author);
	}
}
