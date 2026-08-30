using System;
using UnityEngine;

// Token: 0x02001149 RID: 4425
public class XUiV_TextList : XUiV_LabelBase
{
	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06008D2F RID: 36143 RVA: 0x00354813 File Offset: 0x00352A13
	// (set) Token: 0x06008D30 RID: 36144 RVA: 0x00354820 File Offset: 0x00352A20
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

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06008D31 RID: 36145 RVA: 0x0035482E File Offset: 0x00352A2E
	// (set) Token: 0x06008D32 RID: 36146 RVA: 0x00354836 File Offset: 0x00352A36
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

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06008D33 RID: 36147 RVA: 0x0035484F File Offset: 0x00352A4F
	// (set) Token: 0x06008D34 RID: 36148 RVA: 0x00354857 File Offset: 0x00352A57
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

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06008D35 RID: 36149 RVA: 0x00354870 File Offset: 0x00352A70
	// (set) Token: 0x06008D36 RID: 36150 RVA: 0x00354878 File Offset: 0x00352A78
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

	// Token: 0x06008D37 RID: 36151 RVA: 0x00354896 File Offset: 0x00352A96
	public XUiV_TextList(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D38 RID: 36152 RVA: 0x003548A8 File Offset: 0x00352AA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITextList>();
	}

	// Token: 0x06008D39 RID: 36153 RVA: 0x003548B8 File Offset: 0x00352AB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.textList = this.uiTransform.GetComponent<UITextList>();
	}

	// Token: 0x06008D3A RID: 36154 RVA: 0x003548D1 File Offset: 0x00352AD1
	public override void InitView()
	{
		base.EventOnDrag = true;
		base.EventOnScroll = true;
		base.InitView();
	}

	// Token: 0x06008D3B RID: 36155 RVA: 0x003548E8 File Offset: 0x00352AE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.label.width = this.size.x;
		this.label.height = this.size.y;
		this.textList.paragraphHistory = this.paragraphHistory;
		this.textList.style = this.listStyle;
		base.updateData();
	}

	// Token: 0x06008D3C RID: 36156 RVA: 0x00354949 File Offset: 0x00352B49
	public override void Cleanup()
	{
		this.textList.Clear();
		base.Cleanup();
	}

	// Token: 0x06008D3D RID: 36157 RVA: 0x0035495C File Offset: 0x00352B5C
	public void AddLine(string _line)
	{
		if (!string.IsNullOrEmpty(this.firstLinePrefix))
		{
			_line = this.firstLinePrefix + _line;
		}
		_line = this.getFormattedText(_line);
		this.textList.Add(_line);
	}

	// Token: 0x04006783 RID: 26499
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList textList;

	// Token: 0x04006784 RID: 26500
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList.Style listStyle;

	// Token: 0x04006785 RID: 26501
	[PublicizedFrom(EAccessModifier.Protected)]
	public string firstLinePrefix;

	// Token: 0x04006786 RID: 26502
	[PublicizedFrom(EAccessModifier.Protected)]
	public int paragraphHistory = 50;
}
