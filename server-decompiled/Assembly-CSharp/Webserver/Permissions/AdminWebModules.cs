using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

namespace Webserver.Permissions
{
	// Token: 0x02001AD9 RID: 6873
	public class AdminWebModules : AdminSectionAbs
	{
		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600CE3E RID: 52798 RVA: 0x004B438A File Offset: 0x004B258A
		public static AdminWebModules Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebModules;
			}
		}

		// Token: 0x0600CE3F RID: 52799 RVA: 0x004B439C File Offset: 0x004B259C
		public AdminWebModules(AdminTools _parent)
			: base(_parent, "webmodules")
		{
		}

		// Token: 0x0600CE40 RID: 52800 RVA: 0x004B43E8 File Offset: 0x004B25E8
		public override void Clear()
		{
			this.allModulesList.Clear();
			this.modules.Clear();
		}

		// Token: 0x0600CE41 RID: 52801 RVA: 0x004B4400 File Offset: 0x004B2600
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

		// Token: 0x0600CE42 RID: 52802 RVA: 0x004B4454 File Offset: 0x004B2654
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

		// Token: 0x0600CE43 RID: 52803 RVA: 0x004B44C4 File Offset: 0x004B26C4
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

		// Token: 0x0600CE44 RID: 52804 RVA: 0x004B4528 File Offset: 0x004B2728
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

		// Token: 0x0600CE45 RID: 52805 RVA: 0x004B458C File Offset: 0x004B278C
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

		// Token: 0x0600CE46 RID: 52806 RVA: 0x004B4658 File Offset: 0x004B2858
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

		// Token: 0x0600CE47 RID: 52807 RVA: 0x004B470C File Offset: 0x004B290C
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

		// Token: 0x0600CE48 RID: 52808 RVA: 0x004B4760 File Offset: 0x004B2960
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

		// Token: 0x0600CE49 RID: 52809 RVA: 0x004B47B0 File Offset: 0x004B29B0
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

		// Token: 0x04009CAB RID: 40107
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> modules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009CAC RID: 40108
		public const int MethodLevelInheritGlobal = -2147483648;

		// Token: 0x04009CAD RID: 40109
		public const string MethodLevelInheritKeyword = "inherit";

		// Token: 0x04009CAE RID: 40110
		public const int MethodLevelNotSupported = -2147483647;

		// Token: 0x04009CAF RID: 40111
		public const int PermissionLevelUser = 1000;

		// Token: 0x04009CB0 RID: 40112
		public const int PermissionLevelGuest = 2000;

		// Token: 0x04009CB1 RID: 40113
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> knownModules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009CB2 RID: 40114
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AdminWebModules.WebModule> allModulesList = new List<AdminWebModules.WebModule>();

		// Token: 0x04009CB3 RID: 40115
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AdminWebModules.WebModule defaultModulePermission = new AdminWebModules.WebModule("", 0, true);

		// Token: 0x02001ADA RID: 6874
		public readonly struct WebModule
		{
			// Token: 0x0600CE4A RID: 52810 RVA: 0x004B481C File Offset: 0x004B2A1C
			public WebModule(string _name, int _level, bool _isDefault)
			{
				this.LevelPerMethod = null;
				this.Name = _name;
				this.LevelGlobal = _level;
				this.IsDefault = _isDefault;
			}

			// Token: 0x0600CE4B RID: 52811 RVA: 0x004B483C File Offset: 0x004B2A3C
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

			// Token: 0x0600CE4C RID: 52812 RVA: 0x004B489C File Offset: 0x004B2A9C
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

			// Token: 0x0600CE4D RID: 52813 RVA: 0x004B4950 File Offset: 0x004B2B50
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

			// Token: 0x0600CE4E RID: 52814 RVA: 0x004B4B64 File Offset: 0x004B2D64
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

			// Token: 0x0600CE4F RID: 52815 RVA: 0x004B4BAC File Offset: 0x004B2DAC
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

			// Token: 0x0600CE50 RID: 52816 RVA: 0x004B4BF0 File Offset: 0x004B2DF0
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

			// Token: 0x0600CE51 RID: 52817 RVA: 0x004B4C1C File Offset: 0x004B2E1C
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

			// Token: 0x04009CB4 RID: 40116
			public readonly string Name;

			// Token: 0x04009CB5 RID: 40117
			public readonly int LevelGlobal;

			// Token: 0x04009CB6 RID: 40118
			public readonly int[] LevelPerMethod;

			// Token: 0x04009CB7 RID: 40119
			public readonly bool IsDefault;
		}
	}
}
