using System;
using UnityEngine;

// Token: 0x0200117A RID: 4474
public class XUiV_TextList : XUiV_LabelBase
{
	// Token: 0x17001130 RID: 4400
	// (get) Token: 0x06008EAC RID: 36524 RVA: 0x0035D7F3 File Offset: 0x0035B9F3
	// (set) Token: 0x06008EAD RID: 36525 RVA: 0x0035D800 File Offset: 0x0035BA00
	public float ScrollValue
	{
		get
		{
			return this.textList.scrollValue;
		}
		set
		{
			this.textList.scrollValue = value;
		}
	}

	// Token: 0x17001131 RID: 4401
	// (get) Token: 0x06008EAE RID: 36526 RVA: 0x0035D80E File Offset: 0x0035BA0E
	// (set) Token: 0x06008EAF RID: 36527 RVA: 0x0035D816 File Offset: 0x0035BA16
	[XuiXmlAttribute("max_paragraphs", false)]
	public int ParagraphHistory
	{
		get
		{
			return this.paragraphHistory;
		}
		set
		{
			if (value == this.paragraphHistory)
			{
				return;
			}
			this.paragraphHistory = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001132 RID: 4402
	// (get) Token: 0x06008EB0 RID: 36528 RVA: 0x0035D82F File Offset: 0x0035BA2F
	// (set) Token: 0x06008EB1 RID: 36529 RVA: 0x0035D837 File Offset: 0x0035BA37
	[XuiXmlAttribute("list_style", false)]
	public UITextList.Style ListStyle
	{
		get
		{
			return this.listStyle;
		}
		set
		{
			if (value == this.listStyle)
			{
				return;
			}
			this.listStyle = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001133 RID: 4403
	// (get) Token: 0x06008EB2 RID: 36530 RVA: 0x0035D850 File Offset: 0x0035BA50
	// (set) Token: 0x06008EB3 RID: 36531 RVA: 0x0035D858 File Offset: 0x0035BA58
	[XuiXmlAttribute("prefix_first_line", false)]
	public string FirstLinePrefix
	{
		get
		{
			return this.firstLinePrefix;
		}
		set
		{
			if (this.firstLinePrefix == value)
			{
				return;
			}
			this.firstLinePrefix = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008EB4 RID: 36532 RVA: 0x0035D876 File Offset: 0x0035BA76
	public XUiV_TextList(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EB5 RID: 36533 RVA: 0x0035D888 File Offset: 0x0035BA88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITextList>();
	}

	// Token: 0x06008EB6 RID: 36534 RVA: 0x0035D898 File Offset: 0x0035BA98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.textList = this.uiTransform.GetComponent<UITextList>();
	}

	// Token: 0x06008EB7 RID: 36535 RVA: 0x0035D8B1 File Offset: 0x0035BAB1
	public override void InitView()
	{
		base.EventOnDrag = true;
		base.EventOnScroll = true;
		base.InitView();
	}

	// Token: 0x06008EB8 RID: 36536 RVA: 0x0035D8C8 File Offset: 0x0035BAC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.label.width = this.size.x;
		this.label.height = this.size.y;
		this.textList.paragraphHistory = this.paragraphHistory;
		this.textList.style = this.listStyle;
		base.updateData();
	}

	// Token: 0x06008EB9 RID: 36537 RVA: 0x0035D929 File Offset: 0x0035BB29
	public override void Cleanup()
	{
		this.textList.Clear();
		base.Cleanup();
	}

	// Token: 0x06008EBA RID: 36538 RVA: 0x0035D93C File Offset: 0x0035BB3C
	public void AddLine(string _line)
	{
		if (!string.IsNullOrEmpty(this.firstLinePrefix))
		{
			_line = this.firstLinePrefix + _line;
		}
		_line = this.getFormattedText(_line);
		this.textList.Add(_line);
	}

	// Token: 0x040068B2 RID: 26802
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList textList;

	// Token: 0x040068B3 RID: 26803
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList.Style listStyle;

	// Token: 0x040068B4 RID: 26804
	[PublicizedFrom(EAccessModifier.Protected)]
	public string firstLinePrefix;

	// Token: 0x040068B5 RID: 26805
	[PublicizedFrom(EAccessModifier.Protected)]
	public int paragraphHistory = 50;
}
