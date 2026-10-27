using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020014FA RID: 5370
public struct UVRectTiling
{
	// Token: 0x0600A810 RID: 43024 RVA: 0x003E8FE4 File Offset: 0x003E71E4
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

	// Token: 0x0600A811 RID: 43025 RVA: 0x003E9128 File Offset: 0x003E7328
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

	// Token: 0x0600A812 RID: 43026 RVA: 0x003E9264 File Offset: 0x003E7464
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

	// Token: 0x04007CBB RID: 31931
	public static UVRectTiling Empty;

	// Token: 0x04007CBC RID: 31932
	public Rect uv;

	// Token: 0x04007CBD RID: 31933
	public int blockW;

	// Token: 0x04007CBE RID: 31934
	public int blockH;

	// Token: 0x04007CBF RID: 31935
	public bool bSwitchUV;

	// Token: 0x04007CC0 RID: 31936
	public bool bGlobalUV;

	// Token: 0x04007CC1 RID: 31937
	public Color color;

	// Token: 0x04007CC2 RID: 31938
	public int index;

	// Token: 0x04007CC3 RID: 31939
	public MaterialBlock material;

	// Token: 0x04007CC4 RID: 31940
	public string textureName;
}
