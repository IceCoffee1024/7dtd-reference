using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020014C6 RID: 5318
public struct UVRectTiling
{
	// Token: 0x0600A705 RID: 42757 RVA: 0x003E1D8C File Offset: 0x003DFF8C
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

	// Token: 0x0600A706 RID: 42758 RVA: 0x003E1ED0 File Offset: 0x003E00D0
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

	// Token: 0x0600A707 RID: 42759 RVA: 0x003E200C File Offset: 0x003E020C
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

	// Token: 0x04007BF7 RID: 31735
	public static UVRectTiling Empty;

	// Token: 0x04007BF8 RID: 31736
	public Rect uv;

	// Token: 0x04007BF9 RID: 31737
	public int blockW;

	// Token: 0x04007BFA RID: 31738
	public int blockH;

	// Token: 0x04007BFB RID: 31739
	public bool bSwitchUV;

	// Token: 0x04007BFC RID: 31740
	public bool bGlobalUV;

	// Token: 0x04007BFD RID: 31741
	public Color color;

	// Token: 0x04007BFE RID: 31742
	public int index;

	// Token: 0x04007BFF RID: 31743
	public MaterialBlock material;

	// Token: 0x04007C00 RID: 31744
	public string textureName;
}
