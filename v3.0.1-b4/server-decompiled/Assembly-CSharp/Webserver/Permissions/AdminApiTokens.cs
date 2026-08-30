using System;
using System.Collections.Generic;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001AD7 RID: 6871
	public class AdminApiTokens : AdminSectionAbs
	{
		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600CE31 RID: 52785 RVA: 0x004B3FA3 File Offset: 0x004B21A3
		public static AdminApiTokens Instance
		{
			get
			{
				return GameManager.Instance.adminTools.ApiTokens;
			}
		}

		// Token: 0x0600CE32 RID: 52786 RVA: 0x004B3FB4 File Offset: 0x004B21B4
		public AdminApiTokens(AdminTools _parent)
			: base(_parent, "apitokens")
		{
		}

		// Token: 0x0600CE33 RID: 52787 RVA: 0x004B3FCD File Offset: 0x004B21CD
		public override void Clear()
		{
			this.tokens.Clear();
		}

		// Token: 0x0600CE34 RID: 52788 RVA: 0x004B3FDC File Offset: 0x004B21DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminApiTokens.ApiToken apiToken;
			if (AdminApiTokens.ApiToken.TryParse(_childElement, out apiToken))
			{
				this.tokens[apiToken.Name] = apiToken;
			}
		}

		// Token: 0x0600CE35 RID: 52789 RVA: 0x004B4008 File Offset: 0x004B2208
		public override void Save(XmlElement _root)
		{
			XmlElement xmlElement = _root.AddXmlElement(this.SectionTypeName);
			xmlElement.AddXmlComment(" <token name=\"adminuser1\" secret=\"supersecrettoken\" permission_level=\"0\" /> ");
			foreach (KeyValuePair<string, AdminApiTokens.ApiToken> keyValuePair in this.tokens)
			{
				string text;
				AdminApiTokens.ApiToken apiToken;
				keyValuePair.Deconstruct(out text, out apiToken);
				AdminApiTokens.ApiToken apiToken2 = apiToken;
				apiToken2.ToXml(xmlElement);
			}
		}

		// Token: 0x0600CE36 RID: 52790 RVA: 0x004B4084 File Offset: 0x004B2284
		public void AddToken(string _name, string _secret, int _permissionLevel)
		{
			AdminTools parent = this.Parent;
			lock (parent)
			{
				AdminApiTokens.ApiToken apiToken = new AdminApiTokens.ApiToken(_name, _secret, _permissionLevel);
				this.tokens[_name] = apiToken;
				this.Parent.Save();
			}
		}

		// Token: 0x0600CE37 RID: 52791 RVA: 0x004B40E0 File Offset: 0x004B22E0
		public bool RemoveToken(string _name)
		{
			AdminTools parent = this.Parent;
			bool flag3;
			lock (parent)
			{
				bool flag2 = this.tokens.Remove(_name);
				if (flag2)
				{
					this.Parent.Save();
				}
				flag3 = flag2;
			}
			return flag3;
		}

		// Token: 0x0600CE38 RID: 52792 RVA: 0x004B4138 File Offset: 0x004B2338
		public Dictionary<string, AdminApiTokens.ApiToken> GetTokens()
		{
			AdminTools parent = this.Parent;
			Dictionary<string, AdminApiTokens.ApiToken> dictionary;
			lock (parent)
			{
				dictionary = this.tokens;
			}
			return dictionary;
		}

		// Token: 0x0600CE39 RID: 52793 RVA: 0x004B417C File Offset: 0x004B237C
		public int GetPermissionLevel(string _name, string _secret)
		{
			AdminTools parent = this.Parent;
			int num;
			lock (parent)
			{
				AdminApiTokens.ApiToken apiToken;
				if (this.tokens.TryGetValue(_name, out apiToken) && apiToken.Secret == _secret)
				{
					num = apiToken.PermissionLevel;
				}
				else if (this.IsCommandlineToken(_name, _secret))
				{
					num = 0;
				}
				else
				{
					num = int.MaxValue;
				}
			}
			return num;
		}

		// Token: 0x0600CE3A RID: 52794 RVA: 0x004B41F4 File Offset: 0x004B23F4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsCommandlineToken(string _name, string _secret)
		{
			if (!this.commandlineChecked)
			{
				this.commandlineTokenName = GameUtils.GetLaunchArgument("webapitokenname");
				this.commandlineTokenSecret = GameUtils.GetLaunchArgument("webapitokensecret");
				this.commandlineChecked = true;
			}
			return !string.IsNullOrEmpty(this.commandlineTokenName) && !string.IsNullOrEmpty(this.commandlineTokenSecret) && _name == this.commandlineTokenName && _secret == this.commandlineTokenSecret;
		}

		// Token: 0x04009CA4 RID: 40100
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminApiTokens.ApiToken> tokens = new CaseInsensitiveStringDictionary<AdminApiTokens.ApiToken>();

		// Token: 0x04009CA5 RID: 40101
		[PublicizedFrom(EAccessModifier.Private)]
		public bool commandlineChecked;

		// Token: 0x04009CA6 RID: 40102
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenName;

		// Token: 0x04009CA7 RID: 40103
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenSecret;

		// Token: 0x02001AD8 RID: 6872
		public readonly struct ApiToken
		{
			// Token: 0x0600CE3B RID: 52795 RVA: 0x004B4268 File Offset: 0x004B2468
			public ApiToken(string _name, string _secret, int _permissionLevel)
			{
				this.Name = _name;
				this.Secret = _secret;
				this.PermissionLevel = _permissionLevel;
			}

			// Token: 0x0600CE3C RID: 52796 RVA: 0x004B4280 File Offset: 0x004B2480
			public void ToXml(XmlElement _parent)
			{
				_parent.AddXmlElement("token").SetAttrib("name", this.Name).SetAttrib("secret", this.Secret)
					.SetAttrib("permission_level", this.PermissionLevel.ToString());
			}

			// Token: 0x0600CE3D RID: 52797 RVA: 0x004B42D4 File Offset: 0x004B24D4
			public static bool TryParse(XmlElement _element, out AdminApiTokens.ApiToken _result)
			{
				_result = default(AdminApiTokens.ApiToken);
				string text;
				if (!_element.TryGetAttribute("name", out text))
				{
					Log.Warning("[Web] [Perms] Ignoring apitoken-entry because of missing 'name' attribute: " + _element.OuterXml);
					return false;
				}
				string text2;
				if (!_element.TryGetAttribute("secret", out text2))
				{
					Log.Warning("[Web] [Perms] Ignoring apitoken-entry because of missing 'secret' attribute: " + _element.OuterXml);
					return false;
				}
				string text3;
				if (!_element.TryGetAttribute("permission_level", out text3))
				{
					Log.Warning("[Web] [Perms] Ignoring apitoken-entry because of missing 'permission_level' attribute: " + _element.OuterXml);
					return false;
				}
				int num;
				if (!int.TryParse(text3, out num))
				{
					Log.Warning("[Web] [Perms] Ignoring apitoken-entry because of invalid (non-numeric) value for 'permission_level' attribute: " + _element.OuterXml);
					return false;
				}
				_result = new AdminApiTokens.ApiToken(text, text2, num);
				return true;
			}

			// Token: 0x04009CA8 RID: 40104
			public readonly string Name;

			// Token: 0x04009CA9 RID: 40105
			public readonly string Secret;

			// Token: 0x04009CAA RID: 40106
			public readonly int PermissionLevel;
		}
	}
}
