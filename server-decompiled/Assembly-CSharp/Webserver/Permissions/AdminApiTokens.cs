using System;
using System.Collections.Generic;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001B5B RID: 7003
	public class AdminApiTokens : AdminSectionAbs
	{
		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x0600D226 RID: 53798 RVA: 0x004C235F File Offset: 0x004C055F
		public static AdminApiTokens Instance
		{
			get
			{
				return GameManager.Instance.adminTools.ApiTokens;
			}
		}

		// Token: 0x0600D227 RID: 53799 RVA: 0x004C2370 File Offset: 0x004C0570
		public AdminApiTokens(AdminTools _parent)
			: base(_parent, "apitokens")
		{
		}

		// Token: 0x0600D228 RID: 53800 RVA: 0x004C2389 File Offset: 0x004C0589
		public override void Clear()
		{
			this.tokens.Clear();
		}

		// Token: 0x0600D229 RID: 53801 RVA: 0x004C2398 File Offset: 0x004C0598
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminApiTokens.ApiToken apiToken;
			if (AdminApiTokens.ApiToken.TryParse(_childElement, out apiToken))
			{
				this.tokens[apiToken.Name] = apiToken;
			}
		}

		// Token: 0x0600D22A RID: 53802 RVA: 0x004C23C4 File Offset: 0x004C05C4
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

		// Token: 0x0600D22B RID: 53803 RVA: 0x004C2440 File Offset: 0x004C0640
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

		// Token: 0x0600D22C RID: 53804 RVA: 0x004C249C File Offset: 0x004C069C
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

		// Token: 0x0600D22D RID: 53805 RVA: 0x004C24F4 File Offset: 0x004C06F4
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

		// Token: 0x0600D22E RID: 53806 RVA: 0x004C2538 File Offset: 0x004C0738
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

		// Token: 0x0600D22F RID: 53807 RVA: 0x004C25B0 File Offset: 0x004C07B0
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

		// Token: 0x04009F84 RID: 40836
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminApiTokens.ApiToken> tokens = new CaseInsensitiveStringDictionary<AdminApiTokens.ApiToken>();

		// Token: 0x04009F85 RID: 40837
		[PublicizedFrom(EAccessModifier.Private)]
		public bool commandlineChecked;

		// Token: 0x04009F86 RID: 40838
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenName;

		// Token: 0x04009F87 RID: 40839
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenSecret;

		// Token: 0x02001B5C RID: 7004
		public readonly struct ApiToken
		{
			// Token: 0x0600D230 RID: 53808 RVA: 0x004C2624 File Offset: 0x004C0824
			public ApiToken(string _name, string _secret, int _permissionLevel)
			{
				this.Name = _name;
				this.Secret = _secret;
				this.PermissionLevel = _permissionLevel;
			}

			// Token: 0x0600D231 RID: 53809 RVA: 0x004C263C File Offset: 0x004C083C
			public void ToXml(XmlElement _parent)
			{
				_parent.AddXmlElement("token").SetAttrib("name", this.Name).SetAttrib("secret", this.Secret)
					.SetAttrib("permission_level", this.PermissionLevel.ToString());
			}

			// Token: 0x0600D232 RID: 53810 RVA: 0x004C2690 File Offset: 0x004C0890
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

			// Token: 0x04009F88 RID: 40840
			public readonly string Name;

			// Token: 0x04009F89 RID: 40841
			public readonly string Secret;

			// Token: 0x04009F8A RID: 40842
			public readonly int PermissionLevel;
		}
	}
}
