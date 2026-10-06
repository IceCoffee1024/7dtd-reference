using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

// Token: 0x02001521 RID: 5409
public class XmlPatcher
{
	// Token: 0x0600A9F2 RID: 43506 RVA: 0x003F3584 File Offset: 0x003F1784
	public static IEnumerator LoadAndPatchConfig(string _configName, Action<XmlFile> _callback)
	{
		if (!_configName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
		{
			_configName += ".xml";
		}
		Exception xmlLoadException = null;
		XmlFile xmlFile = new XmlFile(GameIO.GetGameDir("Data/Config"), _configName, delegate(Exception _exception)
		{
			if (_exception != null)
			{
				xmlLoadException = _exception;
			}
		});
		while (!xmlFile.Loaded && xmlLoadException == null)
		{
			yield return null;
		}
		if (xmlLoadException != null)
		{
			Log.Error("XML loader: Loading base XML '" + xmlFile.Filename + "' failed:");
			Log.Exception(xmlLoadException);
			yield break;
		}
		MicroStopwatch msw = new MicroStopwatch(true);
		foreach (Mod mod in ModManager.GetLoadedMods())
		{
			string text = mod.Path + "/Config/" + _configName;
			if (SdFile.Exists(text))
			{
				try
				{
					XmlFile xmlFile2 = XmlPatcher.ReadPatchXmlWithFixedModFolders(mod, text);
					if (xmlFile2 == null)
					{
						continue;
					}
					XElement root = xmlFile2.XmlDoc.Root;
					XmlPatcher.PatchXml(xmlFile, root, xmlFile2, mod);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new string[] { "XML loader: Patching '", xmlFile.Filename, "' from mod '", mod.Name, "' failed:" }));
					Log.Exception(ex);
				}
				if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
				{
					yield return null;
					msw.ResetAndRestart();
				}
			}
		}
		List<Mod>.Enumerator enumerator = default(List<Mod>.Enumerator);
		_callback(xmlFile);
		yield break;
		yield break;
	}

	// Token: 0x0600A9F3 RID: 43507 RVA: 0x003F359C File Offset: 0x003F179C
	public static XmlFile ReadPatchXmlWithFixedModFolders(Mod _parentMod, string _file)
	{
		string text = SdFile.ReadAllText(_file, Encoding.UTF8);
		text = text.Replace("@modfolder:", "@modfolder(" + _parentMod.Name + "):");
		string fileName = Path.GetFileName(_file);
		try
		{
			return new XmlFile(text, Path.GetDirectoryName(_file), fileName, true);
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "XML loader: Loading XML patch file '", fileName, "' from mod '", _parentMod.Name, "' failed:" }));
			Log.Exception(ex);
		}
		return null;
	}

	// Token: 0x0600A9F4 RID: 43508 RVA: 0x003F3640 File Offset: 0x003F1840
	public static bool PatchXml(XmlFile _xmlFile, XElement _containerElement, XmlFile _patchFile, Mod _patchingMod)
	{
		if (_containerElement == null)
		{
			return false;
		}
		bool flag = true;
		foreach (XElement xelement in _containerElement.Elements())
		{
			bool flag2 = XmlPatcher.singlePatch(_xmlFile, xelement, _patchFile, _patchingMod);
			if (!flag2)
			{
				IXmlLineInfo xmlLineInfo = xelement;
				Log.Warning(string.Format("XML patch for \"{0}\" from mod \"{1}\" did not apply: {2} (line {3} at pos {4})", new object[]
				{
					_xmlFile.Filename,
					_patchingMod.Name,
					xelement.GetElementString(),
					xmlLineInfo.LineNumber,
					xmlLineInfo.LinePosition
				}));
			}
			flag = flag && flag2;
		}
		return flag;
	}

	// Token: 0x0600A9F5 RID: 43509 RVA: 0x003F36F0 File Offset: 0x003F18F0
	public static IEnumerator ApplyConditionalXmlBlocks(string _xmlName, XmlFile _xmlFile, MicroStopwatch _timer, XmlPatcher.EEvaluator _evaluator, Action _errorCallback)
	{
		if (_timer != null)
		{
			_timer.ResetAndRestart();
		}
		string text = ((_evaluator == XmlPatcher.EEvaluator.Host) ? "//conditional[(@evaluator='host' or not(@evaluator)) and not(ancestor::conditional[@evaluator='host' or not(@evaluator)])]" : "//conditional[@evaluator='client' and not(ancestor::conditional)]");
		List<XObject> list;
		while (_xmlFile.GetXpathResults(text, out list))
		{
			using (List<XObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XObject xobject = enumerator.Current;
					XElement xelement = xobject as XElement;
					if (xelement != null)
					{
						XElement xelement2 = XmlPatchConditionEvaluator.FindActiveConditionalBranchElement(_xmlFile, xelement);
						if (xelement2 != null)
						{
							xelement.AddAfterSelf(xelement2.Nodes());
						}
						xelement.Remove();
					}
				}
				continue;
			}
			break;
		}
		_xmlFile.ClearXpathResults();
		if (_timer != null)
		{
			_timer.Stop();
		}
		if (((_timer != null) ? _timer.ElapsedMilliseconds : 0L) > 50L)
		{
			Log.Out(string.Format("Conditionals handling in {0} for {1} took {2} ms", _xmlName, _evaluator, _timer.ElapsedMilliseconds));
		}
		if (_timer != null)
		{
			_timer.Start();
		}
		yield break;
	}

	// Token: 0x0600A9F6 RID: 43510 RVA: 0x003F3714 File Offset: 0x003F1914
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool singlePatch(XmlFile _targetFile, XElement _patchElement, XmlFile _patchFile, Mod _patchingMod)
	{
		string localName = _patchElement.Name.LocalName;
		XmlPatcher.PatchMethodDefinition patchMethodDefinition;
		if (!XmlPatcher.XpathPatchMethods.TryGetValue(localName, out patchMethodDefinition))
		{
			Log.Warning(string.Format("XML.Patch ({0}, line {1} at pos {2}): Patch type ({3}) unknown", new object[]
			{
				_patchElement.GetXPath(),
				((IXmlLineInfo)_patchElement).LineNumber,
				((IXmlLineInfo)_patchElement).LinePosition,
				localName
			}));
			return false;
		}
		if (patchMethodDefinition.RequiresXpath && !_patchElement.HasAttribute("xpath"))
		{
			throw new Exception(string.Format("XML.Patch ({0}, line {1} at pos {2}): Patch element does not have an 'xpath' attribute", _patchElement.GetXPath(), ((IXmlLineInfo)_patchElement).LineNumber, ((IXmlLineInfo)_patchElement).LinePosition));
		}
		string attribute = _patchElement.GetAttribute("xpath");
		bool flag;
		try
		{
			flag = patchMethodDefinition.Delegate(_targetFile, attribute, _patchElement, _patchFile, _patchingMod) > 0;
		}
		catch (XPathException ex)
		{
			throw new XPathException(string.Format("XML.Patch ({0}, line {1} at pos {2}): XPath evaluation failed: {3}", new object[]
			{
				_patchElement.GetXPath(),
				((IXmlLineInfo)_patchElement).LineNumber,
				((IXmlLineInfo)_patchElement).LinePosition,
				ex.Message
			}));
		}
		catch (XmlException)
		{
			Log.Error(string.Format("XML.Patch ({0}, line {1} at pos {2}): Unknown XML exception while applying patch:", _patchElement.GetXPath(), ((IXmlLineInfo)_patchElement).LineNumber, ((IXmlLineInfo)_patchElement).LinePosition));
			throw;
		}
		return flag;
	}

	// Token: 0x0600A9F7 RID: 43511 RVA: 0x003F3880 File Offset: 0x003F1A80
	[PublicizedFrom(EAccessModifier.Private)]
	static XmlPatcher()
	{
		ReflectionHelpers.FindTypesWithAttribute<XmlPatchMethodsClassAttribute>(new Action<Type, bool, XmlPatchMethodsClassAttribute>(XmlPatcher.<.cctor>g__TypeFoundCallback|9_0), true);
	}

	// Token: 0x0600A9F8 RID: 43512 RVA: 0x003F38A0 File Offset: 0x003F1AA0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addXmlFilePatchMethod(string _patchName, string _methodName, bool _requiresXpath = true)
	{
		MethodInfo method = typeof(XmlFile).GetMethod(_methodName);
		XmlPatcher.XpathDelegate xpathDelegate = (XmlPatcher.XpathDelegate)Delegate.CreateDelegate(typeof(XmlPatcher.XpathDelegate), method);
		if (!XmlPatcher.XpathPatchMethods.TryAdd(_patchName, new XmlPatcher.PatchMethodDefinition(xpathDelegate, _requiresXpath)))
		{
			XmlPatcher.redeclarationLog(_patchName, method);
		}
	}

	// Token: 0x0600A9F9 RID: 43513 RVA: 0x003F38F0 File Offset: 0x003F1AF0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addXmlFilePatchMethod(string _patchName, MethodInfo _method, bool _requiresXpath = true)
	{
		XmlPatcher.XpathDelegate xpathDelegate = (XmlPatcher.XpathDelegate)Delegate.CreateDelegate(typeof(XmlPatcher.XpathDelegate), _method);
		if (!XmlPatcher.XpathPatchMethods.TryAdd(_patchName, new XmlPatcher.PatchMethodDefinition(xpathDelegate, _requiresXpath)))
		{
			XmlPatcher.redeclarationLog(_patchName, _method);
		}
	}

	// Token: 0x0600A9FA RID: 43514 RVA: 0x003F3930 File Offset: 0x003F1B30
	[PublicizedFrom(EAccessModifier.Private)]
	public static void redeclarationLog(string _patchName, MethodInfo _newMethod)
	{
		MethodInfo method = XmlPatcher.XpathPatchMethods[_patchName].Delegate.Method;
		Log.Warning(string.Concat(new string[]
		{
			"XML patch method '",
			_patchName,
			"' already defined in ",
			method.DeclaringType.FullName,
			".",
			method.Name,
			". Redeclaration in ",
			_newMethod.DeclaringType.FullName,
			".",
			_newMethod.Name
		}));
	}

	// Token: 0x0600A9FC RID: 43516 RVA: 0x003F39BF File Offset: 0x003F1BBF
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <.cctor>g__TypeFoundCallback|9_0(Type _type, bool _hasMultiple, XmlPatchMethodsClassAttribute _xmlPatchMethodsClassAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XmlPatchMethodAttribute>(_type, new Action<MethodInfo, bool, XmlPatchMethodAttribute>(XmlPatcher.<.cctor>g__MethodFoundCallback|9_1), true, false, true);
	}

	// Token: 0x0600A9FD RID: 43517 RVA: 0x003F39D8 File Offset: 0x003F1BD8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <.cctor>g__MethodFoundCallback|9_1(MethodInfo _method, bool _hasMultiple, XmlPatchMethodAttribute _xmlPatchMethodAttribute)
	{
		if (!ReflectionHelpers.MethodCompatibleWithDelegate<XmlPatcher.XpathDelegate>(_method, false))
		{
			Log.Error(string.Concat(new string[]
			{
				"XML patch method ",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				" does not have the expected signature"
			}));
			return;
		}
		foreach (Attribute attribute in _method.GetCustomAttributes(typeof(XmlPatchMethodAttribute)))
		{
			XmlPatchMethodAttribute xmlPatchMethodAttribute = attribute as XmlPatchMethodAttribute;
			if (xmlPatchMethodAttribute != null)
			{
				string patchName = xmlPatchMethodAttribute.PatchName;
				bool requiresXpath = xmlPatchMethodAttribute.RequiresXpath;
				XmlPatcher.addXmlFilePatchMethod(patchName, _method, requiresXpath);
				break;
			}
		}
	}

	// Token: 0x04007DA0 RID: 32160
	public static readonly Dictionary<string, XmlPatcher.PatchMethodDefinition> XpathPatchMethods = new CaseInsensitiveStringDictionary<XmlPatcher.PatchMethodDefinition>();

	// Token: 0x02001522 RID: 5410
	public enum EEvaluator
	{
		// Token: 0x04007DA2 RID: 32162
		Host,
		// Token: 0x04007DA3 RID: 32163
		Client
	}

	// Token: 0x02001523 RID: 5411
	public struct PatchMethodDefinition
	{
		// Token: 0x0600A9FE RID: 43518 RVA: 0x003F3A94 File Offset: 0x003F1C94
		public PatchMethodDefinition(XmlPatcher.XpathDelegate _delegate, bool _requiresXpath)
		{
			this.Delegate = _delegate;
			this.RequiresXpath = _requiresXpath;
		}

		// Token: 0x04007DA4 RID: 32164
		public readonly XmlPatcher.XpathDelegate Delegate;

		// Token: 0x04007DA5 RID: 32165
		public readonly bool RequiresXpath;
	}

	// Token: 0x02001524 RID: 5412
	// (Invoke) Token: 0x0600AA00 RID: 43520
	public delegate int XpathDelegate(XmlFile _targetFile, string _xpath, XElement _patchSourceElement, XmlFile _patchFile, Mod _patchingMod);
}
