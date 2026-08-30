using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02001508 RID: 5384
[Preserve]
[XmlPatchMethodsClass]
public static class XmlPatchMethods
{
	// Token: 0x0600A962 RID: 43362 RVA: 0x003EF0EC File Offset: 0x003ED2EC
	[XmlPatchMethod("append")]
	public static int AppendByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XObject xobject2 = xobject;
			XElement xelement = xobject2 as XElement;
			XAttribute xattribute;
			if (xelement == null)
			{
				xattribute = xobject2 as XAttribute;
				if (xattribute == null)
				{
					IXmlLineInfo xmlLineInfo = xobject;
					throw new XmlPatchException(_patchSourceElement, "AppendByXPath", string.Format("Matched node type ({0}, line {1} at pos {2}) can not be appended to", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
				}
			}
			else
			{
				using (IEnumerator<XElement> enumerator2 = _patchSourceElement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement2 = enumerator2.Current;
						XElement xelement3 = new XElement(xelement2);
						if (_patchingMod != null)
						{
							XComment xcomment = new XComment("Element appended by: \"" + _patchingMod.Name + "\"");
							xelement3.AddFirst(xcomment);
						}
						xelement.Add(xelement3);
					}
					continue;
				}
			}
			XText xtext = _patchSourceElement.FirstNode as XText;
			if (xtext == null)
			{
				XAttribute xattribute2 = xattribute;
				IXmlLineInfo xmlLineInfo2 = xattribute2;
				throw new XmlPatchException(_patchSourceElement, "AppendByXPath", string.Format("Appending to attribute ({0}, line {1} at pos {2}) from a non-text source: {3}", new object[]
				{
					xattribute2.GetXPath(),
					xmlLineInfo2.LineNumber,
					xmlLineInfo2.LinePosition,
					_patchSourceElement.FirstNode.NodeType.ToStringCached<XmlNodeType>()
				}));
			}
			XAttribute xattribute3 = xattribute;
			xattribute3.Value += xtext.Value.Trim();
			if (_patchingMod != null)
			{
				XComment xcomment2 = new XComment(string.Format("Attribute \"{0}\" appended by: \"{1}\"", xattribute.Name, _patchingMod.Name));
				XElement parent = xattribute.Parent;
				if (parent != null)
				{
					parent.AddFirst(xcomment2);
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A963 RID: 43363 RVA: 0x003EF308 File Offset: 0x003ED508
	[XmlPatchMethod("prepend")]
	public static int PrependByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XObject xobject2 = xobject;
			XElement xelement = xobject2 as XElement;
			XAttribute xattribute;
			if (xelement == null)
			{
				xattribute = xobject2 as XAttribute;
				if (xattribute == null)
				{
					IXmlLineInfo xmlLineInfo = xobject;
					throw new XmlPatchException(_patchSourceElement, "PrependByXPath", string.Format("Matched node type ({0}, line {1} at pos {2}) can not be prepended to", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
				}
			}
			else
			{
				using (IEnumerator<XElement> enumerator2 = _patchSourceElement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement2 = enumerator2.Current;
						XElement xelement3 = new XElement(xelement2);
						if (_patchingMod != null)
						{
							XComment xcomment = new XComment("Element prepended by: \"" + _patchingMod.Name + "\"");
							xelement3.AddFirst(xcomment);
						}
						xelement.AddFirst(xelement3);
					}
					continue;
				}
			}
			XText xtext = _patchSourceElement.FirstNode as XText;
			if (xtext == null)
			{
				XAttribute xattribute2 = xattribute;
				IXmlLineInfo xmlLineInfo2 = xattribute2;
				throw new XmlPatchException(_patchSourceElement, "PrependByXPath", string.Format("Prepending to attribute ({0}, line {1} at pos {2}) from a non-text source: {3}", new object[]
				{
					xattribute2.GetXPath(),
					xmlLineInfo2.LineNumber,
					xmlLineInfo2.LinePosition,
					_patchSourceElement.FirstNode.NodeType.ToStringCached<XmlNodeType>()
				}));
			}
			xattribute.Value = xtext.Value.Trim() + xattribute.Value;
			if (_patchingMod != null)
			{
				XComment xcomment2 = new XComment(string.Format("Attribute \"{0}\" prepended by: \"{1}\"", xattribute.Name, _patchingMod.Name));
				XElement parent = xattribute.Parent;
				if (parent != null)
				{
					parent.AddFirst(xcomment2);
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A964 RID: 43364 RVA: 0x003EF524 File Offset: 0x003ED724
	[XmlPatchMethod("insertAfter")]
	public static int InsertAfterByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XElement xelement = xobject as XElement;
			if (xelement != null)
			{
				foreach (XElement xelement2 in _patchSourceElement.Elements().Reverse<XElement>())
				{
					XElement xelement3 = new XElement(xelement2);
					if (_patchingMod != null)
					{
						XComment xcomment = new XComment("Element inserted by: \"" + _patchingMod.Name + "\"");
						xelement3.AddFirst(xcomment);
					}
					xelement.AddAfterSelf(xelement3);
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A965 RID: 43365 RVA: 0x003EF5F8 File Offset: 0x003ED7F8
	[XmlPatchMethod("insertBefore")]
	public static int InsertBeforeByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XElement xelement = xobject as XElement;
			if (xelement != null)
			{
				foreach (XElement xelement2 in _patchSourceElement.Elements())
				{
					XElement xelement3 = new XElement(xelement2);
					if (_patchingMod != null)
					{
						XComment xcomment = new XComment("Element inserted by: \"" + _patchingMod.Name + "\"");
						xelement3.AddFirst(xcomment);
					}
					xelement.AddBeforeSelf(xelement3);
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A966 RID: 43366 RVA: 0x003EF6C8 File Offset: 0x003ED8C8
	[XmlPatchMethod("remove")]
	public static int RemoveByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XAttribute xattribute = xobject as XAttribute;
			if (xattribute != null)
			{
				IXmlLineInfo xmlLineInfo = xattribute;
				throw new XmlPatchException(_patchSourceElement, "RemoveByXPath", string.Format("Can not remove matched Attribute ({0}, line {1} at pos {2}), use removeattribute instead", xattribute.GetXPath(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
			}
			XElement xelement = xobject as XElement;
			if (xelement == null)
			{
				IXmlLineInfo xmlLineInfo2 = xobject;
				throw new XmlPatchException(_patchSourceElement, "RemoveByXPath", string.Format("Matched node type ({0}, line {1} at pos {2}) can not be removed", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo2.LineNumber, xmlLineInfo2.LinePosition));
			}
			if (_patchingMod != null)
			{
				XComment xcomment = new XComment(string.Concat(new string[] { "Element removed by: \"", _patchingMod.Name, "\" (XPath: \"", _xpath, "\")" }));
				xelement.AddAfterSelf(xcomment);
			}
			xelement.Remove();
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A967 RID: 43367 RVA: 0x003EF804 File Offset: 0x003EDA04
	[XmlPatchMethod("set")]
	public static int SetByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XObject xobject2 = xobject;
			XElement xelement = xobject2 as XElement;
			if (xelement == null)
			{
				XAttribute xattribute = xobject2 as XAttribute;
				if (xattribute == null)
				{
					IXmlLineInfo xmlLineInfo = xobject;
					throw new XmlPatchException(_patchSourceElement, "SetByXPath", string.Format("Matched node type ({0}, line {1} at pos {2}) can not be set", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
				}
				if (!_patchSourceElement.Nodes().Any<XNode>())
				{
					IXmlLineInfo xmlLineInfo2 = xobject;
					throw new XmlPatchException(_patchSourceElement, "SetByXPath", string.Format("Setting attribute ({0}, line {1} at pos {2}) without any replacement text given as child element", xattribute.GetXPath(), xmlLineInfo2.LineNumber, xmlLineInfo2.LinePosition));
				}
				XAttribute xattribute2 = xattribute;
				XText xtext = _patchSourceElement.FirstNode as XText;
				if (xtext == null)
				{
					XAttribute xattribute3 = xattribute;
					IXmlLineInfo xmlLineInfo3 = xobject;
					throw new XmlPatchException(_patchSourceElement, "SetByXPath", string.Format("Setting attribute ({0}, line {1} at pos {2}) from a non-text source: {3}", new object[]
					{
						xattribute3.GetXPath(),
						xmlLineInfo3.LineNumber,
						xmlLineInfo3.LinePosition,
						_patchSourceElement.FirstNode.NodeType.ToStringCached<XmlNodeType>()
					}));
				}
				xattribute2.Value = xtext.Value.Trim();
				if (_patchingMod != null)
				{
					XComment xcomment = new XComment(string.Format("Attribute \"{0}\" replaced by: \"{1}\"", xattribute2.Name, _patchingMod.Name));
					XElement parent = xattribute2.Parent;
					if (parent != null)
					{
						parent.AddFirst(xcomment);
					}
				}
			}
			else
			{
				xelement.ReplaceNodes(_patchSourceElement.Nodes());
				if (_patchingMod != null)
				{
					XComment xcomment2 = new XComment("Element contents replaced by: \"" + _patchingMod.Name + "\"");
					xelement.AddFirst(xcomment2);
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A968 RID: 43368 RVA: 0x003EFA18 File Offset: 0x003EDC18
	[XmlPatchMethod("setattribute")]
	public static int SetAttributeByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		if (!_patchSourceElement.HasAttribute("name"))
		{
			throw new XmlPatchException(_patchSourceElement, "SetAttributeByXPath", "Patch element does not have a 'name' attribute");
		}
		XName xname = _patchSourceElement.GetAttribute("name");
		XText xtext = _patchSourceElement.FirstNode as XText;
		if (xtext == null)
		{
			throw new XmlPatchException(_patchSourceElement, "SetAttributeByXPath", string.Format("Setting attribute ({0}) from a non-text source: {1}", xname, _patchSourceElement.FirstNode.NodeType.ToStringCached<XmlNodeType>()));
		}
		string text = xtext.Value.Trim();
		foreach (XObject xobject in list)
		{
			XElement xelement = xobject as XElement;
			if (xelement == null)
			{
				IXmlLineInfo xmlLineInfo = xobject;
				throw new XmlPatchException(_patchSourceElement, "SetAttributeByXPath", string.Format("Matched node (line {0} at pos {1}) is not an XML element but {2}", xmlLineInfo.LineNumber, xmlLineInfo.LinePosition, xobject.NodeType.ToStringCached<XmlNodeType>()));
			}
			xelement.SetAttributeValue(xname, text);
			if (_patchingMod != null)
			{
				XComment xcomment = new XComment(string.Format("Attribute \"{0}\" added/overwritten by: \"{1}\"", xname, _patchingMod.Name));
				xelement.AddFirst(xcomment);
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A969 RID: 43369 RVA: 0x003EFB70 File Offset: 0x003EDD70
	[XmlPatchMethod("removeattribute")]
	public static int RemoveAttributeByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		foreach (XObject xobject in list)
		{
			XAttribute xattribute = xobject as XAttribute;
			if (xattribute == null)
			{
				IXmlLineInfo xmlLineInfo = xobject;
				throw new XmlPatchException(_patchSourceElement, "RemoveAttributeByXPath", string.Format("Can only remove Attributes (matched {0}, line {1} at pos {2}), use remove instead", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
			}
			if (_patchingMod != null)
			{
				XComment xcomment = new XComment(string.Format("Attribute \"{0}\" removed by: \"{1}\"", xattribute.Name, _patchingMod.Name));
				XElement parent = xattribute.Parent;
				if (parent != null)
				{
					parent.AddFirst(xcomment);
				}
			}
			xattribute.Remove();
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A96A RID: 43370 RVA: 0x003EFC54 File Offset: 0x003EDE54
	[XmlPatchMethod("csv")]
	public static int CsvOperationsByXPath(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		List<XObject> list;
		if (!_targetFile.GetXpathResults(_xpath, out list))
		{
			return 0;
		}
		XAttribute xattribute = _patchSourceElement.Attribute("op");
		if (xattribute == null)
		{
			throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "Patch element does not have an 'op' attribute");
		}
		XmlPatchMethods.ECsvOperation ecsvOperation;
		if (!EnumUtils.TryParse<XmlPatchMethods.ECsvOperation>(xattribute.Value, out ecsvOperation, true))
		{
			throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "Unsupported 'op' attribute value (only supports 'add', 'remove')");
		}
		XAttribute xattribute2 = _patchSourceElement.Attribute("delim");
		if (xattribute2 != null && xattribute2.Value.Length != 1 && xattribute2.Value != "\\n")
		{
			throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "Patch element 'delim' attribute needs to be exactly 1 character");
		}
		char c = ((xattribute2 != null) ? xattribute2.Value[0] : ',');
		if (((xattribute2 != null) ? xattribute2.Value : null) == "\\n")
		{
			c = '\n';
		}
		bool flag = false;
		XAttribute xattribute3 = _patchSourceElement.Attribute("keep_whitespace");
		if (xattribute3 != null && !StringParsers.TryParseBool(xattribute3.Value, out flag))
		{
			throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "Patch element 'keep_whitespace' attribute needs to be a valid boolean value");
		}
		XText xtext = _patchSourceElement.FirstNode as XText;
		if (xtext == null)
		{
			throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "CSV operations require a text value: " + _patchSourceElement.FirstNode.NodeType.ToStringCached<XmlNodeType>());
		}
		List<string> list2 = new List<string>(xtext.Value.Split(c, int.MaxValue, StringSplitOptions.RemoveEmptyEntries));
		for (int i = list2.Count - 1; i >= 0; i--)
		{
			list2[i] = list2[i].Trim();
			if (list2[i].Length == 0)
			{
				list2.RemoveAt(i);
			}
		}
		XmlPatchMethods.EWildcardPositions[] array = null;
		if (ecsvOperation == XmlPatchMethods.ECsvOperation.Remove)
		{
			array = new XmlPatchMethods.EWildcardPositions[list2.Count];
		}
		for (int j = 0; j < list2.Count; j++)
		{
			bool flag2 = list2[j].StartsWith('*');
			bool flag3 = list2[j].EndsWith('*');
			if (ecsvOperation == XmlPatchMethods.ECsvOperation.Add && (flag2 || flag3))
			{
				throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", "Only 'remove' operation supports wildcards");
			}
			if (ecsvOperation == XmlPatchMethods.ECsvOperation.Remove)
			{
				array[j] = ((flag2 && flag3) ? XmlPatchMethods.EWildcardPositions.Both : (flag2 ? XmlPatchMethods.EWildcardPositions.Start : (flag3 ? XmlPatchMethods.EWildcardPositions.End : XmlPatchMethods.EWildcardPositions.None)));
				if (flag2)
				{
					list2[j] = list2[j].Substring(1);
				}
				if (flag3)
				{
					list2[j] = list2[j].Substring(0, list2[j].Length - 1);
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (XObject xobject in list)
		{
			XAttribute xattribute4 = xobject as XAttribute;
			XText xtext2 = xobject as XText;
			if (xattribute4 == null && xtext2 == null)
			{
				IXmlLineInfo xmlLineInfo = xobject;
				throw new XmlPatchException(_patchSourceElement, "CsvOperationsByXPath", string.Format("Can only operate on Attributes or Text (matched {0}, line {1} at pos {2})", xobject.NodeType.ToStringCached<XmlNodeType>(), xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
			}
			string text = ((xattribute4 != null) ? xattribute4.Value : xtext2.Value);
			List<string> list3 = new List<string>(text.Split(c, int.MaxValue, StringSplitOptions.RemoveEmptyEntries));
			if (!flag)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					list3[k] = list3[k].Trim();
				}
			}
			for (int l = 0; l < list2.Count; l++)
			{
				string text2 = list2[l];
				if (ecsvOperation != XmlPatchMethods.ECsvOperation.Add)
				{
					if (ecsvOperation == XmlPatchMethods.ECsvOperation.Remove)
					{
						XmlPatchMethods.EWildcardPositions ewildcardPositions = array[l];
						for (int m = list3.Count - 1; m >= 0; m--)
						{
							bool flag4;
							switch (ewildcardPositions)
							{
							case XmlPatchMethods.EWildcardPositions.None:
								flag4 = list3[m].Trim().EqualsCaseInsensitive(text2);
								break;
							case XmlPatchMethods.EWildcardPositions.Start:
								flag4 = list3[m].TrimEnd().EndsWith(text2, StringComparison.OrdinalIgnoreCase);
								break;
							case XmlPatchMethods.EWildcardPositions.End:
								flag4 = list3[m].TrimStart().StartsWith(text2, StringComparison.OrdinalIgnoreCase);
								break;
							case XmlPatchMethods.EWildcardPositions.Both:
								flag4 = list3[m].ContainsCaseInsensitive(text2);
								break;
							default:
								flag4 = false;
								break;
							}
							if (flag4)
							{
								list3.RemoveAt(m);
							}
						}
					}
				}
				else if (!list3.ContainsCaseInsensitive(text2))
				{
					list3.Add(text2);
				}
			}
			for (int n = 0; n < list3.Count; n++)
			{
				if (n > 0)
				{
					stringBuilder.Append(c);
				}
				stringBuilder.Append(list3[n]);
			}
			text = stringBuilder.ToString();
			stringBuilder.Clear();
			if (xattribute4 != null)
			{
				xattribute4.Value = text;
			}
			else
			{
				xtext2.Value = text;
			}
			if (_patchingMod != null)
			{
				if (xattribute4 != null)
				{
					XComment xcomment = new XComment(string.Format("Attribute \"{0}\" CSV manipulated by: \"{1}\"", xattribute4.Name, _patchingMod.Name));
					XElement parent = xattribute4.Parent;
					if (parent != null)
					{
						parent.AddFirst(xcomment);
					}
				}
				else
				{
					XComment xcomment2 = new XComment("Content CSV manipulated by: \"" + _patchingMod.Name + "\"");
					XElement parent2 = xtext2.Parent;
					if (parent2 != null)
					{
						parent2.Add(xcomment2);
					}
				}
			}
		}
		return _targetFile.ClearXpathResults();
	}

	// Token: 0x0600A96B RID: 43371 RVA: 0x003F01A8 File Offset: 0x003EE3A8
	[XmlPatchMethod("conditional", false)]
	public static int Conditional(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		if (_patchSourceElement.HasAttribute("evaluator"))
		{
			Log.Warning(string.Concat(new string[] { "XML loader: Patching '", _targetFile.Filename, "' from mod '", _patchingMod.Name, "': Conditional patch element ignores 'evaluator' attribute!" }));
		}
		XElement xelement = XmlPatchConditionEvaluator.FindActiveConditionalBranchElement(_targetFile, _patchSourceElement);
		if (xelement == null)
		{
			return 1;
		}
		if (!XmlPatcher.PatchXml(_targetFile, xelement, _patchFile, _patchingMod))
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x0600A96C RID: 43372 RVA: 0x003F0220 File Offset: 0x003EE420
	[XmlPatchMethod("include", false)]
	public static int Include(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod = null)
	{
		XAttribute xattribute = _patchSourceElement.Attribute("filename");
		if (xattribute == null)
		{
			throw new XmlPatchException(_patchSourceElement, "Include", "Patch element does not have an 'filename' attribute");
		}
		string value = xattribute.Value;
		string text = Path.Combine(Path.GetDirectoryName(Path.Combine(_patchFile.Directory, _patchFile.Filename)), value);
		if (!SdFile.Exists(text))
		{
			throw new XmlPatchException(_patchSourceElement, "Include", "Given file not found: " + value);
		}
		try
		{
			XmlFile xmlFile = XmlPatcher.ReadPatchXmlWithFixedModFolders(_patchingMod, text);
			if (xmlFile == null)
			{
				throw new XmlPatchException(_patchSourceElement, "Include", "Error loading included patch file: " + value);
			}
			XElement root = xmlFile.XmlDoc.Root;
			return XmlPatcher.PatchXml(_targetFile, root, xmlFile, _patchingMod) ? 1 : 0;
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "XML loader: Patching '", _targetFile.Filename, "' from mod '", _patchingMod.Name, "' failed:" }));
			Log.Exception(ex);
		}
		return 0;
	}

	// Token: 0x02001509 RID: 5385
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ECsvOperation
	{
		// Token: 0x04007D35 RID: 32053
		Add,
		// Token: 0x04007D36 RID: 32054
		Remove
	}

	// Token: 0x0200150A RID: 5386
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EWildcardPositions
	{
		// Token: 0x04007D38 RID: 32056
		None,
		// Token: 0x04007D39 RID: 32057
		Start,
		// Token: 0x04007D3A RID: 32058
		End,
		// Token: 0x04007D3B RID: 32059
		Both
	}
}
