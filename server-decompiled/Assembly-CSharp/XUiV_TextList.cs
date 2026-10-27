using System;
using UnityEngine;

// Token: 0x0200118A RID: 4490
public class XUiV_TextList : XUiV_LabelBase
{
	// Token: 0x1700113B RID: 4411
	// (get) Token: 0x06008F00 RID: 36608 RVA: 0x0035F8E3 File Offset: 0x0035DAE3
	// (set) Token: 0x06008F01 RID: 36609 RVA: 0x0035F8F0 File Offset: 0x0035DAF0
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

	// Token: 0x1700113C RID: 4412
	// (get) Token: 0x06008F02 RID: 36610 RVA: 0x0035F8FE File Offset: 0x0035DAFE
	// (set) Token: 0x06008F03 RID: 36611 RVA: 0x0035F906 File Offset: 0x0035DB06
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

	// Token: 0x1700113D RID: 4413
	// (get) Token: 0x06008F04 RID: 36612 RVA: 0x0035F91F File Offset: 0x0035DB1F
	// (set) Token: 0x06008F05 RID: 36613 RVA: 0x0035F927 File Offset: 0x0035DB27
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

	// Token: 0x1700113E RID: 4414
	// (get) Token: 0x06008F06 RID: 36614 RVA: 0x0035F940 File Offset: 0x0035DB40
	// (set) Token: 0x06008F07 RID: 36615 RVA: 0x0035F948 File Offset: 0x0035DB48
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

	// Token: 0x06008F08 RID: 36616 RVA: 0x0035F966 File Offset: 0x0035DB66
	public XUiV_TextList(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008F09 RID: 36617 RVA: 0x0035F978 File Offset: 0x0035DB78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITextList>();
	}

	// Token: 0x06008F0A RID: 36618 RVA: 0x0035F988 File Offset: 0x0035DB88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.textList = this.uiTransform.GetComponent<UITextList>();
	}

	// Token: 0x06008F0B RID: 36619 RVA: 0x0035F9A1 File Offset: 0x0035DBA1
	public override void InitView()
	{
		base.EventOnDrag = true;
		base.EventOnScroll = true;
		base.InitView();
	}

	// Token: 0x06008F0C RID: 36620 RVA: 0x0035F9B8 File Offset: 0x0035DBB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.label.width = this.size.x;
		this.label.height = this.size.y;
		this.textList.paragraphHistory = this.paragraphHistory;
		this.textList.style = this.listStyle;
		base.updateData();
	}

	// Token: 0x06008F0D RID: 36621 RVA: 0x0035FA19 File Offset: 0x0035DC19
	public override void Cleanup()
	{
		this.textList.Clear();
		base.Cleanup();
	}

	// Token: 0x06008F0E RID: 36622 RVA: 0x0035FA2C File Offset: 0x0035DC2C
	public void AddLine(string _line)
	{
		if (!string.IsNullOrEmpty(this.firstLinePrefix))
		{
			_line = this.firstLinePrefix + _line;
		}
		_line = this.getFormattedText(_line);
		this.textList.Add(_line);
	}

	// Token: 0x040068F2 RID: 26866
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList textList;

	// Token: 0x040068F3 RID: 26867
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList.Style listStyle;

	// Token: 0x040068F4 RID: 26868
	[PublicizedFrom(EAccessModifier.Protected)]
	public string firstLinePrefix;

	// Token: 0x040068F5 RID: 26869
	[PublicizedFrom(EAccessModifier.Protected)]
	public int paragraphHistory = 50;
}
