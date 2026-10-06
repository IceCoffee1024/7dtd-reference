using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020014E8 RID: 5352
public struct UVRectTiling
{
	// Token: 0x0600A7B8 RID: 42936 RVA: 0x003E6BE0 File Offset: 0x003E4DE0
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"x=\"",
			this.uv.x.ToCultureInvariantString(),
			"\" y=\"",
			this.uv.y.ToCultureInvariantString(),
			"\" w=\"",
			this.uv.width.ToCultureInvariantString(),
			"\" h=\"",
			this.uv.height.ToCultureInvariantString(),
			"\" blockw=\"",
			this.blockW.ToString(),
			"\" blockh=\"",
			this.blockH.ToString(),
			"\" color=\"",
			this.color.r.ToCultureInvariantString(),
			",",
			this.color.g.ToCultureInvariantString(),
			",",
			this.color.b.ToCultureInvariantString(),
			"\" globaluv=\"",
			this.bGlobalUV.ToString(),
			"\" index=\"",
			this.index.ToString(),
			"\""
		});
	}

	// Token: 0x0600A7B9 RID: 42937 RVA: 0x003E6D24 File Offset: 0x003E4F24
	public void ToXML(XmlElement _elem)
	{
		_elem.SetAttrib("x", this.uv.x.ToCultureInvariantString());
		_elem.SetAttrib("y", this.uv.y.ToCultureInvariantString());
		_elem.SetAttrib("w", this.uv.width.ToCultureInvariantString());
		_elem.SetAttrib("h", this.uv.height.ToCultureInvariantString());
		_elem.SetAttrib("blockw", this.blockW.ToString());
		_elem.SetAttrib("blockh", this.blockH.ToString());
		_elem.SetAttrib("color", string.Concat(new string[]
		{
			this.color.r.ToCultureInvariantString(),
			",",
			this.color.g.ToCultureInvariantString(),
			",",
			this.color.b.ToCultureInvariantString()
		}));
		_elem.SetAttrib("globaluv", this.bGlobalUV.ToString());
		_elem.SetAttrib("index", this.index.ToString());
	}

	// Token: 0x0600A7BA RID: 42938 RVA: 0x003E6E60 File Offset: 0x003E5060
	public void FromXML(XElement _element)
	{
		this.uv.x = StringParsers.ParseFloat(_element.GetAttribute("x"), 0, -1, NumberStyles.Any);
		this.uv.y = StringParsers.ParseFloat(_element.GetAttribute("y"), 0, -1, NumberStyles.Any);
		this.uv.width = StringParsers.ParseFloat(_element.GetAttribute("w"), 0, -1, NumberStyles.Any);
		this.uv.height = StringParsers.ParseFloat(_element.GetAttribute("h"), 0, -1, NumberStyles.Any);
		this.blockW = int.Parse(_element.GetAttribute("blockw"));
		this.blockH = int.Parse(_element.GetAttribute("blockh"));
		this.bSwitchUV = _element.HasAttribute("switchuv") && StringParsers.ParseBool(_element.GetAttribute("switchuv"), 0, -1, true);
		this.bGlobalUV = _element.HasAttribute("globaluv") && StringParsers.ParseBool(_element.GetAttribute("globaluv"), 0, -1, true);
		this.material = MaterialBlock.fromString(_element.GetAttribute("material"));
		this.textureName = _element.GetAttribute("texture");
		string[] array = _element.GetAttribute("color").Split(',', StringSplitOptions.None);
		this.color = new Color(StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[2], 0, -1, NumberStyles.Any));
		this.index = (_element.HasAttribute("index") ? int.Parse(_element.GetAttribute("index")) : 0);
	}

	// Token: 0x04007C7A RID: 31866
	public static UVRectTiling Empty;

	// Token: 0x04007C7B RID: 31867
	public Rect uv;

	// Token: 0x04007C7C RID: 31868
	public int blockW;

	// Token: 0x04007C7D RID: 31869
	public int blockH;

	// Token: 0x04007C7E RID: 31870
	public bool bSwitchUV;

	// Token: 0x04007C7F RID: 31871
	public bool bGlobalUV;

	// Token: 0x04007C80 RID: 31872
	public Color color;

	// Token: 0x04007C81 RID: 31873
	public int index;

	// Token: 0x04007C82 RID: 31874
	public MaterialBlock material;

	// Token: 0x04007C83 RID: 31875
	public string textureName;
}
