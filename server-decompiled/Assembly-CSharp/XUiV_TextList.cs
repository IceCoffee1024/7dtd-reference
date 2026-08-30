using System;
using UnityEngine;

// Token: 0x02001162 RID: 4450
public class XUiV_TextList : XUiV_LabelBase
{
	// Token: 0x170010EC RID: 4332
	// (get) Token: 0x06008E21 RID: 36385 RVA: 0x0035904F File Offset: 0x0035724F
	// (set) Token: 0x06008E22 RID: 36386 RVA: 0x0035905C File Offset: 0x0035725C
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

	// Token: 0x170010ED RID: 4333
	// (get) Token: 0x06008E23 RID: 36387 RVA: 0x0035906A File Offset: 0x0035726A
	// (set) Token: 0x06008E24 RID: 36388 RVA: 0x00359072 File Offset: 0x00357272
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

	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06008E25 RID: 36389 RVA: 0x0035908B File Offset: 0x0035728B
	// (set) Token: 0x06008E26 RID: 36390 RVA: 0x00359093 File Offset: 0x00357293
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

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06008E27 RID: 36391 RVA: 0x003590AC File Offset: 0x003572AC
	// (set) Token: 0x06008E28 RID: 36392 RVA: 0x003590B4 File Offset: 0x003572B4
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

	// Token: 0x06008E29 RID: 36393 RVA: 0x003590D2 File Offset: 0x003572D2
	public XUiV_TextList(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E2A RID: 36394 RVA: 0x003590E4 File Offset: 0x003572E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITextList>();
	}

	// Token: 0x06008E2B RID: 36395 RVA: 0x003590F4 File Offset: 0x003572F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.textList = this.uiTransform.GetComponent<UITextList>();
	}

	// Token: 0x06008E2C RID: 36396 RVA: 0x0035910D File Offset: 0x0035730D
	public override void InitView()
	{
		base.EventOnDrag = true;
		base.EventOnScroll = true;
		base.InitView();
	}

	// Token: 0x06008E2D RID: 36397 RVA: 0x00359124 File Offset: 0x00357324
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.label.width = this.size.x;
		this.label.height = this.size.y;
		this.textList.paragraphHistory = this.paragraphHistory;
		this.textList.style = this.listStyle;
		base.updateData();
	}

	// Token: 0x06008E2E RID: 36398 RVA: 0x00359185 File Offset: 0x00357385
	public override void Cleanup()
	{
		this.textList.Clear();
		base.Cleanup();
	}

	// Token: 0x06008E2F RID: 36399 RVA: 0x00359198 File Offset: 0x00357398
	public void AddLine(string _line)
	{
		if (!string.IsNullOrEmpty(this.firstLinePrefix))
		{
			_line = this.firstLinePrefix + _line;
		}
		_line = this.getFormattedText(_line);
		this.textList.Add(_line);
	}

	// Token: 0x0400684D RID: 26701
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList textList;

	// Token: 0x0400684E RID: 26702
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITextList.Style listStyle;

	// Token: 0x0400684F RID: 26703
	[PublicizedFrom(EAccessModifier.Protected)]
	public string firstLinePrefix;

	// Token: 0x04006850 RID: 26704
	[PublicizedFrom(EAccessModifier.Protected)]
	public int paragraphHistory = 50;
}
