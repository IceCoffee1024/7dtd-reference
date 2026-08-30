using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

namespace Webserver.Permissions
{
	// Token: 0x02001B00 RID: 6912
	public class AdminWebModules : AdminSectionAbs
	{
		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x0600CFC7 RID: 53191 RVA: 0x004BA77E File Offset: 0x004B897E
		public static AdminWebModules Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebModules;
			}
		}

		// Token: 0x0600CFC8 RID: 53192 RVA: 0x004BA790 File Offset: 0x004B8990
		public AdminWebModules(AdminTools _parent)
			: base(_parent, "webmodules")
		{
		}

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x004BA7DC File Offset: 0x004B89DC
		public override void Clear()
		{
			this.allModulesList.Clear();
			this.modules.Clear();
		}

		// Token: 0x0600CFCA RID: 53194 RVA: 0x004BA7F4 File Offset: 0x004B89F4
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

		// Token: 0x0600CFCB RID: 53195 RVA: 0x004BA848 File Offset: 0x004B8A48
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

		// Token: 0x0600CFCC RID: 53196 RVA: 0x004BA8B8 File Offset: 0x004B8AB8
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

		// Token: 0x0600CFCD RID: 53197 RVA: 0x004BA91C File Offset: 0x004B8B1C
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

		// Token: 0x0600CFCE RID: 53198 RVA: 0x004BA980 File Offset: 0x004B8B80
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

		// Token: 0x0600CFCF RID: 53199 RVA: 0x004BAA4C File Offset: 0x004B8C4C
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

		// Token: 0x0600CFD0 RID: 53200 RVA: 0x004BAB00 File Offset: 0x004B8D00
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

		// Token: 0x0600CFD1 RID: 53201 RVA: 0x004BAB54 File Offset: 0x004B8D54
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

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x004BABA4 File Offset: 0x004B8DA4
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

		// Token: 0x04009DDC RID: 40412
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> modules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009DDD RID: 40413
		public const int MethodLevelInheritGlobal = -2147483648;

		// Token: 0x04009DDE RID: 40414
		public const string MethodLevelInheritKeyword = "inherit";

		// Token: 0x04009DDF RID: 40415
		public const int MethodLevelNotSupported = -2147483647;

		// Token: 0x04009DE0 RID: 40416
		public const int PermissionLevelUser = 1000;

		// Token: 0x04009DE1 RID: 40417
		public const int PermissionLevelGuest = 2000;

		// Token: 0x04009DE2 RID: 40418
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> knownModules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009DE3 RID: 40419
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AdminWebModules.WebModule> allModulesList = new List<AdminWebModules.WebModule>();

		// Token: 0x04009DE4 RID: 40420
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AdminWebModules.WebModule defaultModulePermission = new AdminWebModules.WebModule("", 0, true);

		// Token: 0x02001B01 RID: 6913
		public readonly struct WebModule
		{
			// Token: 0x0600CFD3 RID: 53203 RVA: 0x004BAC10 File Offset: 0x004B8E10
			public WebModule(string _name, int _level, bool _isDefault)
			{
				this.LevelPerMethod = null;
				this.Name = _name;
				this.LevelGlobal = _level;
				this.IsDefault = _isDefault;
			}

			// Token: 0x0600CFD4 RID: 53204 RVA: 0x004BAC30 File Offset: 0x004B8E30
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

			// Token: 0x0600CFD5 RID: 53205 RVA: 0x004BAC90 File Offset: 0x004B8E90
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

			// Token: 0x0600CFD6 RID: 53206 RVA: 0x004BAD44 File Offset: 0x004B8F44
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

			// Token: 0x0600CFD7 RID: 53207 RVA: 0x004BAF58 File Offset: 0x004B9158
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

			// Token: 0x0600CFD8 RID: 53208 RVA: 0x004BAFA0 File Offset: 0x004B91A0
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

			// Token: 0x0600CFD9 RID: 53209 RVA: 0x004BAFE4 File Offset: 0x004B91E4
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

			// Token: 0x0600CFDA RID: 53210 RVA: 0x004BB010 File Offset: 0x004B9210
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

			// Token: 0x04009DE5 RID: 40421
			public readonly string Name;

			// Token: 0x04009DE6 RID: 40422
			public readonly int LevelGlobal;

			// Token: 0x04009DE7 RID: 40423
			public readonly int[] LevelPerMethod;

			// Token: 0x04009DE8 RID: 40424
			public readonly bool IsDefault;
		}
	}
}
