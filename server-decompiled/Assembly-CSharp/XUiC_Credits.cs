using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E0C RID: 3596
[Preserve]
public class XUiC_Credits : XUiController
{
	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06006D8A RID: 28042 RVA: 0x002ABE69 File Offset: 0x002AA069
	// (set) Token: 0x06006D8B RID: 28043 RVA: 0x002ABE71 File Offset: 0x002AA071
	[XuiXmlAttribute("file", false)]
	public string CreditsFile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006D8C RID: 28044 RVA: 0x002ABE7C File Offset: 0x002AA07C
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

	// Token: 0x06006D8D RID: 28045 RVA: 0x002ABF08 File Offset: 0x002AA108
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

	// Token: 0x06006D8E RID: 28046 RVA: 0x0008A7E7 File Offset: 0x000889E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006D8F RID: 28047 RVA: 0x002ABF6C File Offset: 0x002AA16C
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

	// Token: 0x06006D90 RID: 28048 RVA: 0x002AC074 File Offset: 0x002AA274
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

	// Token: 0x06006D91 RID: 28049 RVA: 0x002AC0F8 File Offset: 0x002AA2F8
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

	// Token: 0x06006D92 RID: 28050 RVA: 0x002AC146 File Offset: 0x002AA346
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject getTemplate(string _name, string _defaultTemplateName, Dictionary<string, GameObject> _templatesDict)
	{
		if (string.IsNullOrEmpty(_name))
		{
			_name = _defaultTemplateName;
		}
		return _templatesDict[_name];
	}

	// Token: 0x06006D93 RID: 28051 RVA: 0x002AC15C File Offset: 0x002AA35C
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

	// Token: 0x040053A8 RID: 21416
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScrollSpeed = 50;

	// Token: 0x040053A9 RID: 21417
	public static string ID = "";

	// Token: 0x040053AA RID: 21418
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040053AB RID: 21419
	[XuiBindComponent("creditsGrid", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView creditsGrid;

	// Token: 0x040053AC RID: 21420
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Panel creditsPanel;

	// Token: 0x040053AD RID: 21421
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform creditsGridTrans;

	// Token: 0x040053AE RID: 21422
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i initialPosition;

	// Token: 0x040053B0 RID: 21424
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCategoryTemplate;

	// Token: 0x040053B1 RID: 21425
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCreditTemplate;

	// Token: 0x040053B2 RID: 21426
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, GameObject> categoryTemplates = new CaseInsensitiveStringDictionary<GameObject>();

	// Token: 0x040053B3 RID: 21427
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, GameObject> creditTemplates = new CaseInsensitiveStringDictionary<GameObject>();

	// Token: 0x040053B4 RID: 21428
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate;

	// Token: 0x040053B5 RID: 21429
	[PublicizedFrom(EAccessModifier.Private)]
	public bool loaded;
}
