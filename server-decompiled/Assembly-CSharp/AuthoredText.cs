using System;
using System.IO;

// Token: 0x02000C71 RID: 3185
public class AuthoredText
{
	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06006051 RID: 24657 RVA: 0x002653F3 File Offset: 0x002635F3
	// (set) Token: 0x06006052 RID: 24658 RVA: 0x002653FB File Offset: 0x002635FB
	public string Text
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06006053 RID: 24659 RVA: 0x00265404 File Offset: 0x00263604
	// (set) Token: 0x06006054 RID: 24660 RVA: 0x0026540C File Offset: 0x0026360C
	public PlatformUserIdentifierAbs Author
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x00265415 File Offset: 0x00263615
	public AuthoredText()
	{
		this.Update(string.Empty, null);
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x00265429 File Offset: 0x00263629
	public AuthoredText(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Update(_text, _author);
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x00265439 File Offset: 0x00263639
	public void Update(string _text, PlatformUserIdentifierAbs _author)
	{
		this.Author = _author;
		this.Text = _text;
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x0026544C File Offset: 0x0026364C
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

	// Token: 0x06006059 RID: 24665 RVA: 0x00265485 File Offset: 0x00263685
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

	// Token: 0x0600605A RID: 24666 RVA: 0x002654B6 File Offset: 0x002636B6
	public AuthoredText Clone()
	{
		return new AuthoredText(this.Text, this.Author);
	}
}
