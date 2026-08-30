using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DFD RID: 3581
[Preserve]
public class XUiC_Credits : XUiController
{
	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06006D92 RID: 28050 RVA: 0x002A8AD9 File Offset: 0x002A6CD9
	// (set) Token: 0x06006D93 RID: 28051 RVA: 0x002A8AE1 File Offset: 0x002A6CE1
	[XuiXmlAttribute("file", false)]
	public string CreditsFile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006D94 RID: 28052 RVA: 0x002A8AEC File Offset: 0x002A6CEC
	public override void Init()
	{
		base.Init();
		XUiC_Credits.ID = base.WindowGroup.Id;
		this.btnBack.OnPress += this.BtnBack_OnPressed;
		this.creditsGridTrans = this.creditsGrid.UiTransform;
		this.initialPosition = this.creditsPanel.Position;
		this.getTemplates("categoryTemplates", this.categoryTemplates, ref this.defaultCategoryTemplate);
		this.getTemplates("creditTemplates", this.creditTemplates, ref this.defaultCreditTemplate);
	}

	// Token: 0x06006D95 RID: 28053 RVA: 0x002A8B78 File Offset: 0x002A6D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void getTemplates(string _parentRectName, Dictionary<string, GameObject> _templatesDict, ref string _defaultTemplateField)
	{
		XUiController childById = base.GetChildById(_parentRectName);
		if (childById == null)
		{
			return;
		}
		for (int i = 0; i < childById.Children.Count; i++)
		{
			XUiView viewComponent = childById.Children[i].ViewComponent;
			string id = viewComponent.ID;
			if (_defaultTemplateField == null)
			{
				_defaultTemplateField = id;
			}
			_templatesDict[id] = viewComponent.UiTransform.gameObject;
		}
	}

	// Token: 0x06006D96 RID: 28054 RVA: 0x0008A567 File Offset: 0x00088767
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006D97 RID: 28055 RVA: 0x002A8BDC File Offset: 0x002A6DDC
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.windowGroup.isModal)
		{
			this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.categoryTemplates)
		{
			string text;
			GameObject gameObject;
			keyValuePair.Deconstruct(out text, out gameObject);
			gameObject.SetActive(false);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.creditTemplates)
		{
			string text;
			GameObject gameObject;
			keyValuePair.Deconstruct(out text, out gameObject);
			gameObject.SetActive(false);
		}
		this.loadCredits();
		this.firstUpdate = true;
		this.creditsPanel.StaticWidgets = true;
		this.creditsPanel.Position = this.initialPosition;
		this.creditsPanel.ClipOffset = Vector2.zero;
	}

	// Token: 0x06006D98 RID: 28056 RVA: 0x002A8CE4 File Offset: 0x002A6EE4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.firstUpdate)
		{
			this.firstUpdate = false;
			return;
		}
		Vector2 vector = this.creditsPanel.ClipOffset - new Vector2(0f, 50f * _dt);
		this.creditsPanel.Position = new Vector2i(this.initialPosition.x, this.initialPosition.y - Mathf.RoundToInt(vector.y));
		this.creditsPanel.ClipOffset = vector;
	}

	// Token: 0x06006D99 RID: 28057 RVA: 0x002A8D68 File Offset: 0x002A6F68
	[PublicizedFrom(EAccessModifier.Private)]
	public XmlFile loadXml()
	{
		if (this.CreditsFile == null)
		{
			return new XmlFile(Resources.Load("Data/Credits/Credits") as TextAsset);
		}
		string text = ModManager.PatchModPathString(this.CreditsFile);
		if (text != null)
		{
			return new XmlFile(Path.GetDirectoryName(text), Path.GetFileName(text), false, false);
		}
		return null;
	}

	// Token: 0x06006D9A RID: 28058 RVA: 0x002A8DB6 File Offset: 0x002A6FB6
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject getTemplate(string _name, string _defaultTemplateName, Dictionary<string, GameObject> _templatesDict)
	{
		if (string.IsNullOrEmpty(_name))
		{
			_name = _defaultTemplateName;
		}
		return _templatesDict[_name];
	}

	// Token: 0x06006D9B RID: 28059 RVA: 0x002A8DCC File Offset: 0x002A6FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadCredits()
	{
		if (this.loaded || this.defaultCategoryTemplate == null || this.defaultCreditTemplate == null)
		{
			return;
		}
		this.loaded = true;
		XmlFile xmlFile = this.loadXml();
		XElement xelement = ((xmlFile != null) ? xmlFile.XmlDoc.Root : null);
		if (xelement == null)
		{
			Log.Error("Credits.xml not found or no XML root");
			return;
		}
		int num = 10000;
		foreach (XElement xelement2 in xelement.Elements("category"))
		{
			XAttribute xattribute = xelement2.Attribute("name");
			string text = ((xattribute != null) ? xattribute.Value : null) ?? "";
			XAttribute xattribute2 = xelement2.Attribute("template");
			string text2 = ((xattribute2 != null) ? xattribute2.Value : null) ?? this.defaultCategoryTemplate;
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.getTemplate(text2, this.defaultCategoryTemplate, this.categoryTemplates), this.creditsGridTrans.transform);
			gameObject.name = num++.ToString() + gameObject.name;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.Find("caption").GetComponent<UILabel>().text = text;
			gameObject.SetActive(true);
			foreach (XElement xelement3 in xelement2.Elements())
			{
				XAttribute xattribute3 = xelement3.Attribute("name");
				string text3 = ((xattribute3 != null) ? xattribute3.Value : null) ?? "";
				XAttribute xattribute4 = xelement3.Attribute("center_text");
				string text4 = ((xattribute4 != null) ? xattribute4.Value : null) ?? "";
				XAttribute xattribute5 = xelement3.Attribute("contribution");
				string text5 = ((xattribute5 != null) ? xattribute5.Value : null) ?? "";
				XAttribute xattribute6 = xelement3.Attribute("template");
				text2 = ((xattribute6 != null) ? xattribute6.Value : null) ?? this.defaultCreditTemplate;
				GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.getTemplate(text2, this.defaultCreditTemplate, this.creditTemplates), this.creditsGridTrans.transform);
				gameObject2.name = num++.ToString() + gameObject2.name;
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.Find("name").GetComponent<UILabel>().text = text3;
				gameObject2.transform.Find("contribution").GetComponent<UILabel>().text = text5;
				gameObject2.transform.Find("centertext").GetComponent<UILabel>().text = text4;
				gameObject2.transform.Find("line").gameObject.SetActive(text3.Length > 0 && text5.Length > 0);
				gameObject2.SetActive(true);
			}
			GameObject gameObject3 = global::UnityEngine.Object.Instantiate<GameObject>(this.getTemplate("", this.defaultCreditTemplate, this.creditTemplates), this.creditsGridTrans.transform);
			gameObject3.name = num++.ToString() + gameObject3.name;
			gameObject3.transform.localScale = Vector3.one;
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.transform.Find("name").GetComponent<UILabel>().text = "";
			gameObject3.transform.Find("contribution").GetComponent<UILabel>().text = "";
			gameObject3.transform.Find("centertext").GetComponent<UILabel>().text = "";
			gameObject3.transform.Find("line").gameObject.SetActive(false);
			gameObject3.SetActive(true);
		}
		UIGrid component = this.creditsGridTrans.GetComponent<UIGrid>();
		if (component != null)
		{
			component.Reposition();
		}
		UITable component2 = this.creditsGridTrans.GetComponent<UITable>();
		if (component2 == null)
		{
			return;
		}
		component2.Reposition();
	}

	// Token: 0x04005371 RID: 21361
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScrollSpeed = 50;

	// Token: 0x04005372 RID: 21362
	public static string ID = "";

	// Token: 0x04005373 RID: 21363
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005374 RID: 21364
	[XuiBindComponent("creditsGrid", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView creditsGrid;

	// Token: 0x04005375 RID: 21365
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Panel creditsPanel;

	// Token: 0x04005376 RID: 21366
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform creditsGridTrans;

	// Token: 0x04005377 RID: 21367
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i initialPosition;

	// Token: 0x04005379 RID: 21369
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCategoryTemplate;

	// Token: 0x0400537A RID: 21370
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCreditTemplate;

	// Token: 0x0400537B RID: 21371
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, GameObject> categoryTemplates = new CaseInsensitiveStringDictionary<GameObject>();

	// Token: 0x0400537C RID: 21372
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, GameObject> creditTemplates = new CaseInsensitiveStringDictionary<GameObject>();

	// Token: 0x0400537D RID: 21373
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate;

	// Token: 0x0400537E RID: 21374
	[PublicizedFrom(EAccessModifier.Private)]
	public bool loaded;
}
