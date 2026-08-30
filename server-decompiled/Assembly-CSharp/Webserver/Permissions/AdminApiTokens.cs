using System;
using System.Collections.Generic;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001AFE RID: 6910
	public class AdminApiTokens : AdminSectionAbs
	{
		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x0600CFBA RID: 53178 RVA: 0x004BA397 File Offset: 0x004B8597
		public static AdminApiTokens Instance
		{
			get
			{
				return GameManager.Instance.adminTools.ApiTokens;
			}
		}

		// Token: 0x0600CFBB RID: 53179 RVA: 0x004BA3A8 File Offset: 0x004B85A8
		public AdminApiTokens(AdminTools _parent)
			: base(_parent, "apitokens")
		{
		}

		// Token: 0x0600CFBC RID: 53180 RVA: 0x004BA3C1 File Offset: 0x004B85C1
		public override void Clear()
		{
			this.tokens.Clear();
		}

		// Token: 0x0600CFBD RID: 53181 RVA: 0x004BA3D0 File Offset: 0x004B85D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminApiTokens.ApiToken apiToken;
			if (AdminApiTokens.ApiToken.TryParse(_childElement, out apiToken))
			{
				this.tokens[apiToken.Name] = apiToken;
			}
		}

		// Token: 0x0600CFBE RID: 53182 RVA: 0x004BA3FC File Offset: 0x004B85FC
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

		// Token: 0x0600CFBF RID: 53183 RVA: 0x004BA478 File Offset: 0x004B8678
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

		// Token: 0x0600CFC0 RID: 53184 RVA: 0x004BA4D4 File Offset: 0x004B86D4
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

		// Token: 0x0600CFC1 RID: 53185 RVA: 0x004BA52C File Offset: 0x004B872C
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

		// Token: 0x0600CFC2 RID: 53186 RVA: 0x004BA570 File Offset: 0x004B8770
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

		// Token: 0x0600CFC3 RID: 53187 RVA: 0x004BA5E8 File Offset: 0x004B87E8
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

		// Token: 0x04009DD5 RID: 40405
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminApiTokens.ApiToken> tokens = new CaseInsensitiveStringDictionary<AdminApiTokens.ApiToken>();

		// Token: 0x04009DD6 RID: 40406
		[PublicizedFrom(EAccessModifier.Private)]
		public bool commandlineChecked;

		// Token: 0x04009DD7 RID: 40407
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenName;

		// Token: 0x04009DD8 RID: 40408
		[PublicizedFrom(EAccessModifier.Private)]
		public string commandlineTokenSecret;

		// Token: 0x02001AFF RID: 6911
		public readonly struct ApiToken
		{
			// Token: 0x0600CFC4 RID: 53188 RVA: 0x004BA65C File Offset: 0x004B885C
			public ApiToken(string _name, string _secret, int _permissionLevel)
			{
				this.Name = _name;
				this.Secret = _secret;
				this.PermissionLevel = _permissionLevel;
			}

			// Token: 0x0600CFC5 RID: 53189 RVA: 0x004BA674 File Offset: 0x004B8874
			public void ToXml(XmlElement _parent)
			{
				_parent.AddXmlElement("token").SetAttrib("name", this.Name).SetAttrib("secret", this.Secret)
					.SetAttrib("permission_level", this.PermissionLevel.ToString());
			}

			// Token: 0x0600CFC6 RID: 53190 RVA: 0x004BA6C8 File Offset: 0x004B88C8
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

			// Token: 0x04009DD9 RID: 40409
			public readonly string Name;

			// Token: 0x04009DDA RID: 40410
			public readonly string Secret;

			// Token: 0x04009DDB RID: 40411
			public readonly int PermissionLevel;
		}
	}
}
