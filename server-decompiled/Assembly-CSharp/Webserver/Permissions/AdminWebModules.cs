using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

namespace Webserver.Permissions
{
	// Token: 0x02001B5D RID: 7005
	public class AdminWebModules : AdminSectionAbs
	{
		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600D233 RID: 53811 RVA: 0x004C2746 File Offset: 0x004C0946
		public static AdminWebModules Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebModules;
			}
		}

		// Token: 0x0600D234 RID: 53812 RVA: 0x004C2758 File Offset: 0x004C0958
		public AdminWebModules(AdminTools _parent)
			: base(_parent, "webmodules")
		{
		}

		// Token: 0x0600D235 RID: 53813 RVA: 0x004C27A4 File Offset: 0x004C09A4
		public override void Clear()
		{
			this.allModulesList.Clear();
			this.modules.Clear();
		}

		// Token: 0x0600D236 RID: 53814 RVA: 0x004C27BC File Offset: 0x004C09BC
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

		// Token: 0x0600D237 RID: 53815 RVA: 0x004C2810 File Offset: 0x004C0A10
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

		// Token: 0x0600D238 RID: 53816 RVA: 0x004C2880 File Offset: 0x004C0A80
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

		// Token: 0x0600D239 RID: 53817 RVA: 0x004C28E4 File Offset: 0x004C0AE4
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

		// Token: 0x0600D23A RID: 53818 RVA: 0x004C2948 File Offset: 0x004C0B48
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

		// Token: 0x0600D23B RID: 53819 RVA: 0x004C2A14 File Offset: 0x004C0C14
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

		// Token: 0x0600D23C RID: 53820 RVA: 0x004C2AC8 File Offset: 0x004C0CC8
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

		// Token: 0x0600D23D RID: 53821 RVA: 0x004C2B1C File Offset: 0x004C0D1C
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

		// Token: 0x0600D23E RID: 53822 RVA: 0x004C2B6C File Offset: 0x004C0D6C
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

		// Token: 0x04009F8B RID: 40843
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> modules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009F8C RID: 40844
		public const int MethodLevelInheritGlobal = -2147483648;

		// Token: 0x04009F8D RID: 40845
		public const string MethodLevelInheritKeyword = "inherit";

		// Token: 0x04009F8E RID: 40846
		public const int MethodLevelNotSupported = -2147483647;

		// Token: 0x04009F8F RID: 40847
		public const int PermissionLevelUser = 1000;

		// Token: 0x04009F90 RID: 40848
		public const int PermissionLevelGuest = 2000;

		// Token: 0x04009F91 RID: 40849
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebModules.WebModule> knownModules = new CaseInsensitiveStringDictionary<AdminWebModules.WebModule>();

		// Token: 0x04009F92 RID: 40850
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AdminWebModules.WebModule> allModulesList = new List<AdminWebModules.WebModule>();

		// Token: 0x04009F93 RID: 40851
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AdminWebModules.WebModule defaultModulePermission = new AdminWebModules.WebModule("", 0, true);

		// Token: 0x02001B5E RID: 7006
		public readonly struct WebModule
		{
			// Token: 0x0600D23F RID: 53823 RVA: 0x004C2BD8 File Offset: 0x004C0DD8
			public WebModule(string _name, int _level, bool _isDefault)
			{
				this.LevelPerMethod = null;
				this.Name = _name;
				this.LevelGlobal = _level;
				this.IsDefault = _isDefault;
			}

			// Token: 0x0600D240 RID: 53824 RVA: 0x004C2BF8 File Offset: 0x004C0DF8
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

			// Token: 0x0600D241 RID: 53825 RVA: 0x004C2C58 File Offset: 0x004C0E58
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

			// Token: 0x0600D242 RID: 53826 RVA: 0x004C2D0C File Offset: 0x004C0F0C
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

			// Token: 0x0600D243 RID: 53827 RVA: 0x004C2F20 File Offset: 0x004C1120
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

			// Token: 0x0600D244 RID: 53828 RVA: 0x004C2F68 File Offset: 0x004C1168
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

			// Token: 0x0600D245 RID: 53829 RVA: 0x004C2FAC File Offset: 0x004C11AC
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

			// Token: 0x0600D246 RID: 53830 RVA: 0x004C2FD8 File Offset: 0x004C11D8
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

			// Token: 0x04009F94 RID: 40852
			public readonly string Name;

			// Token: 0x04009F95 RID: 40853
			public readonly int LevelGlobal;

			// Token: 0x04009F96 RID: 40854
			public readonly int[] LevelPerMethod;

			// Token: 0x04009F97 RID: 40855
			public readonly bool IsDefault;
		}
	}
}
