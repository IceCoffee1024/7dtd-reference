using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

namespace Webserver.Permissions
{
	// Token: 0x02001B2C RID: 6956
	public class AdminWebModules : AdminSectionAbs
	{
		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x0600D11F RID: 53535 RVA: 0x004BEC4A File Offset: 0x004BCE4A
		public static AdminWebModules Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebModules;
			}
		}

		// Token: 0x0600D120 RID: 53536 RVA: 0x004BEC5C File Offset: 0x004BCE5C
		public AdminWebModules(AdminTools _parent)
			: base(_parent, "webmodules")
		{
		}

		// Token: 0x0600D121 RID: 53537 RVA: 0x004BECA8 File Offset: 0x004BCEA8
		public override void Clear()
		{
			this.allModulesList.Clear();
			this.modules.Clear();
		}

		// Token: 0x0600D122 RID: 53538 RVA: 0x004BECC0 File Offset: 0x004BCEC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			this.allModulesList.Clear();
			AdminWebModules.WebModule webModule;
			if (!AdminWebModules.WebModule.TryParse(_childElement, out webModule))
			{
				return;
			}
			AdminWebModules.WebModule webModule2;
			if (this.knownModules.TryGetValue(webModule.Name, out webModule2))
			{
				webModule = webModule.FixPermissionLevelsFromKnownModule(webModule2);
			}
			this.modules[webModule.Name] = webModule;
		}

		// Token: 0x0600D123 RID: 53539 RVA: 0x004BED14 File Offset: 0x004BCF14
		public override void Save(XmlElement _root)
		{
			XmlElement xmlElement = _root.AddXmlElement(this.SectionTypeName);
			foreach (KeyValuePair<string, AdminWebModules.WebModule> keyValuePair in this.modules)
			{
				string text;
				AdminWebModules.WebModule webModule;
				keyValuePair.Deconstruct(out text, out webModule);
				AdminWebModules.WebModule webModule2 = webModule;
				webModule2.ToXml(xmlElement);
			}
		}

		// Token: 0x0600D124 RID: 53540 RVA: 0x004BED84 File Offset: 0x004BCF84
		public void AddModule(AdminWebModules.WebModule _module)
		{
			AdminTools parent = this.Parent;
			lock (parent)
			{
				this.allModulesList.Clear();
				this.modules[_module.Name] = _module;
				this.Parent.Save();
			}
		}

		// Token: 0x0600D125 RID: 53541 RVA: 0x004BEDE8 File Offset: 0x004BCFE8
		public bool RemoveModule(string _module)
		{
			AdminTools parent = this.Parent;
			bool flag3;
			lock (parent)
			{
				this.allModulesList.Clear();
				bool flag2 = this.modules.Remove(_module);
				if (flag2)
				{
					this.Parent.Save();
				}
				flag3 = flag2;
			}
			return flag3;
		}

		// Token: 0x0600D126 RID: 53542 RVA: 0x004BEE4C File Offset: 0x004BD04C
		public List<AdminWebModules.WebModule> GetModules()
		{
			AdminTools parent = this.Parent;
			List<AdminWebModules.WebModule> list;
			lock (parent)
			{
				if (this.allModulesList.Count != 0)
				{
					list = this.allModulesList;
				}
				else
				{
					foreach (KeyValuePair<string, AdminWebModules.WebModule> keyValuePair in this.knownModules)
					{
						string text;
						AdminWebModules.WebModule webModule;
						keyValuePair.Deconstruct(out text, out webModule);
						string text2 = text;
						AdminWebModules.WebModule webModule2 = webModule;
						AdminWebModules.WebModule webModule3;
						this.allModulesList.Add(this.modules.TryGetValue(text2, out webModule3) ? webModule3 : webModule2);
					}
					list = this.allModulesList;
				}
			}
			return list;
		}

		// Token: 0x0600D127 RID: 53543 RVA: 0x004BEF18 File Offset: 0x004BD118
		public void AddKnownModule(AdminWebModules.WebModule _module)
		{
			if (!_module.IsDefault)
			{
				Log.Warning("Call to AddKnownModule with IsDefault==false! From:\n" + StackTraceUtility.ExtractStackTrace());
			}
			if (string.IsNullOrEmpty(_module.Name))
			{
				return;
			}
			AdminTools parent = this.Parent;
			lock (parent)
			{
				this.allModulesList.Clear();
				this.knownModules[_module.Name] = _module;
				AdminWebModules.WebModule webModule;
				if (this.modules.TryGetValue(_module.Name, out webModule))
				{
					webModule = webModule.FixPermissionLevelsFromKnownModule(_module);
					this.modules[_module.Name] = webModule;
				}
			}
		}

		// Token: 0x0600D128 RID: 53544 RVA: 0x004BEFCC File Offset: 0x004BD1CC
		public bool IsKnownModule(string _module)
		{
			if (string.IsNullOrEmpty(_module))
			{
				return false;
			}
			AdminTools parent = this.Parent;
			bool flag2;
			lock (parent)
			{
				flag2 = this.knownModules.ContainsKey(_module);
			}
			return flag2;
		}

		// Token: 0x0600D129 RID: 53545 RVA: 0x004BF020 File Offset: 0x004BD220
		public bool ModuleAllowedWithLevel(string _module, int _level)
		{
			AdminTools parent = this.Parent;
			bool flag2;
			lock (parent)
			{
				flag2 = this.GetModule(_module).LevelGlobal >= _level;
			}
			return flag2;
		}

		// Token: 0x0600D12A RID: 53546 RVA: 0x004BF070 File Offset: 0x004BD270
		public AdminWebModules.WebModule GetModule(string _module)
		{
			AdminTools parent = this.Parent;
			AdminWebModules.WebModule webModule2;
			lock (parent)
			{
				AdminWebModules.WebModule webModule;
				if (this.modules.TryGetValue(_module, out webModule))
				{
					webModule2 = webModule;
				}
				else
				{
					webModule2 = (this.knownModules.TryGetValue(_module, out webModule) ? webModule : this.defaultModulePermission);
				}
			}
			return webModule2;
		}

		// Token: 0x04009EA2 RID: 40610
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> modules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009EA3 RID: 40611
		public const int MethodLevelInheritGlobal = -2147483648;

		// Token: 0x04009EA4 RID: 40612
		public const string MethodLevelInheritKeyword = "inherit";

		// Token: 0x04009EA5 RID: 40613
		public const int MethodLevelNotSupported = -2147483647;

		// Token: 0x04009EA6 RID: 40614
		public const int PermissionLevelUser = 1000;

		// Token: 0x04009EA7 RID: 40615
		public const int PermissionLevelGuest = 2000;

		// Token: 0x04009EA8 RID: 40616
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> knownModules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009EA9 RID: 40617
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AdminWebModules.WebModule> allModulesList = new List<AdminWebModules.WebModule>();

		// Token: 0x04009EAA RID: 40618
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AdminWebModules.WebModule defaultModulePermission = new AdminWebModules.WebModule("", 0, true);

		// Token: 0x02001B2D RID: 6957
		public readonly struct WebModule
		{
			// Token: 0x0600D12B RID: 53547 RVA: 0x004BF0DC File Offset: 0x004BD2DC
			public WebModule(string _name, int _level, bool _isDefault)
			{
				this.LevelPerMethod = null;
				this.Name = _name;
				this.LevelGlobal = _level;
				this.IsDefault = _isDefault;
			}

			// Token: 0x0600D12C RID: 53548 RVA: 0x004BF0FC File Offset: 0x004BD2FC
			public WebModule(string _name, int _levelGlobal, int[] _levelPerMethod, bool _isDefault)
			{
				if (_levelPerMethod != null && _levelPerMethod.Length != 7)
				{
					this.LevelPerMethod = AdminWebModules.WebModule.createDefaultPerMethodArray();
					for (int i = 0; i < 7; i++)
					{
						if (_levelPerMethod != null && i < _levelPerMethod.Length)
						{
							this.LevelPerMethod[i] = _levelPerMethod[i];
						}
					}
				}
				else
				{
					this.LevelPerMethod = _levelPerMethod;
				}
				this.Name = _name;
				this.LevelGlobal = _levelGlobal;
				this.IsDefault = _isDefault;
			}

			// Token: 0x0600D12D RID: 53549 RVA: 0x004BF15C File Offset: 0x004BD35C
			public void ToXml(XmlElement _parent)
			{
				bool flag = this.LevelPerMethod != null;
				XmlElement xmlElement = _parent.AddXmlElement("module").SetAttrib("name", this.Name).SetAttrib("permission_level", this.LevelGlobal.ToString());
				if (!flag)
				{
					return;
				}
				for (int i = 0; i < this.LevelPerMethod.Length; i++)
				{
					ERequestMethod erequestMethod = (ERequestMethod)i;
					int num = this.LevelPerMethod[i];
					if (num != -2147483647)
					{
						xmlElement.AddXmlElement("method").SetAttrib("name", erequestMethod.ToStringCached<ERequestMethod>()).SetAttrib("permission_level", (num == int.MinValue) ? "inherit" : num.ToString());
					}
				}
			}

			// Token: 0x0600D12E RID: 53550 RVA: 0x004BF210 File Offset: 0x004BD410
			public static bool TryParse(XmlElement _element, out AdminWebModules.WebModule _result)
			{
				_result = default(AdminWebModules.WebModule);
				string text;
				if (!_element.TryGetAttribute("name", out text))
				{
					Log.Warning("[Web] [Perms] Ignoring module-entry because of missing 'name' attribute: " + _element.OuterXml);
					return false;
				}
				string text2;
				if (!_element.TryGetAttribute("permission_level", out text2))
				{
					Log.Warning("[Web] [Perms] Ignoring module-entry because of missing 'permission_level' attribute: " + _element.OuterXml);
					return false;
				}
				int num;
				if (!int.TryParse(text2, out num))
				{
					Log.Warning("[Web] [Perms] Ignoring module-entry because of invalid (non-numeric) value for 'permission_level' attribute: " + _element.OuterXml);
					return false;
				}
				int[] array = null;
				foreach (object obj in _element.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						string text3;
						ERequestMethod erequestMethod;
						if (xmlElement.Name != "method")
						{
							Log.Warning("[Web] [Perms] Ignoring module child element, invalid element name: " + xmlElement.OuterXml);
						}
						else if (!xmlElement.TryGetAttribute("name", out text3))
						{
							Log.Warning("[Web] [Perms] Ignoring module child element, missing 'name' attribute: " + xmlElement.OuterXml);
						}
						else if (!EnumUtils.TryParse<ERequestMethod>(text3, out erequestMethod, true))
						{
							Log.Warning("[Web] [Perms] Ignoring module child element, unknown method name in 'name' attribute: " + xmlElement.OuterXml);
						}
						else if (erequestMethod >= ERequestMethod.Count)
						{
							Log.Warning("[Web] [Perms] Ignoring module child element, invalid method name in 'name' attribute: " + xmlElement.OuterXml);
						}
						else if (!xmlElement.TryGetAttribute("permission_level", out text2))
						{
							Log.Warning("[Web] [Perms] Ignoring module child element, missing 'permission_level' attribute: " + xmlElement.OuterXml);
						}
						else
						{
							int minValue;
							if (text2.EqualsCaseInsensitive("inherit"))
							{
								minValue = int.MinValue;
							}
							else if (!int.TryParse(text2, out minValue))
							{
								Log.Warning("[Web] [Perms] Ignoring module child element, invalid (non-numeric) value for 'permission_level' attribute: " + xmlElement.OuterXml);
								continue;
							}
							if (array == null)
							{
								array = AdminWebModules.WebModule.createDefaultPerMethodArray();
							}
							array[(int)erequestMethod] = minValue;
						}
					}
				}
				_result = new AdminWebModules.WebModule(text, num, array, false);
				return true;
			}

			// Token: 0x0600D12F RID: 53551 RVA: 0x004BF424 File Offset: 0x004BD624
			[MustUseReturnValue]
			public AdminWebModules.WebModule SetLevelGlobal(int _level)
			{
				int[] array = ((this.LevelPerMethod == null) ? null : new int[this.LevelPerMethod.Length]);
				if (array != null)
				{
					Array.Copy(this.LevelPerMethod, array, array.Length);
				}
				return new AdminWebModules.WebModule(this.Name, _level, array, false);
			}

			// Token: 0x0600D130 RID: 53552 RVA: 0x004BF46C File Offset: 0x004BD66C
			[MustUseReturnValue]
			public AdminWebModules.WebModule SetLevelForMethod(ERequestMethod _method, int _level)
			{
				int[] array = AdminWebModules.WebModule.createDefaultPerMethodArray();
				if (this.LevelPerMethod != null)
				{
					Array.Copy(this.LevelPerMethod, array, array.Length);
				}
				array[(int)_method] = _level;
				return new AdminWebModules.WebModule(this.Name, this.LevelGlobal, array, false);
			}

			// Token: 0x0600D131 RID: 53553 RVA: 0x004BF4B0 File Offset: 0x004BD6B0
			[PublicizedFrom(EAccessModifier.Private)]
			public static int[] createDefaultPerMethodArray()
			{
				int[] array = new int[7];
				for (int i = 0; i < 7; i++)
				{
					array[i] = -2147483647;
				}
				return array;
			}

			// Token: 0x0600D132 RID: 53554 RVA: 0x004BF4DC File Offset: 0x004BD6DC
			[MustUseReturnValue]
			public AdminWebModules.WebModule FixPermissionLevelsFromKnownModule(AdminWebModules.WebModule _knownModule)
			{
				if (_knownModule.LevelPerMethod != null)
				{
					AdminWebModules.WebModule webModule = this;
					for (int i = 0; i < _knownModule.LevelPerMethod.Length; i++)
					{
						if (webModule.LevelPerMethod == null || webModule.LevelPerMethod[i] == -2147483647)
						{
							webModule = webModule.SetLevelForMethod((ERequestMethod)i, _knownModule.LevelPerMethod[i]);
						}
					}
					return webModule;
				}
				if (this.LevelPerMethod != null)
				{
					return new AdminWebModules.WebModule(this.Name, this.LevelGlobal, false);
				}
				return this;
			}

			// Token: 0x04009EAB RID: 40619
			public readonly string Name;

			// Token: 0x04009EAC RID: 40620
			public readonly int LevelGlobal;

			// Token: 0x04009EAD RID: 40621
			public readonly int[] LevelPerMethod;

			// Token: 0x04009EAE RID: 40622
			public readonly bool IsDefault;
		}
	}
}
