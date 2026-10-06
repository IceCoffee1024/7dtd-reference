using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001B2E RID: 6958
	public class AdminWebUsers : AdminSectionAbs
	{
		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x0600D133 RID: 53555 RVA: 0x004BF556 File Offset: 0x004BD756
		public static AdminWebUsers Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebUsers;
			}
		}

		// Token: 0x0600D134 RID: 53556 RVA: 0x004BF567 File Offset: 0x004BD767
		public AdminWebUsers(AdminTools _parent)
			: base(_parent, "webusers")
		{
		}

		// Token: 0x0600D135 RID: 53557 RVA: 0x004BF580 File Offset: 0x004BD780
		public override void Clear()
		{
			this.users.Clear();
		}

		// Token: 0x0600D136 RID: 53558 RVA: 0x004BF590 File Offset: 0x004BD790
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminWebUsers.WebUser webUser;
			if (AdminWebUsers.WebUser.TryParse(_childElement, out webUser))
			{
				this.users[webUser.Name] = webUser;
			}
		}

		// Token: 0x0600D137 RID: 53559 RVA: 0x004BF5BC File Offset: 0x004BD7BC
		public override void Save(XmlElement _root)
		{
			XmlElement xmlElement = _root.AddXmlElement(this.SectionTypeName);
			foreach (KeyValuePair<string, AdminWebUsers.WebUser> keyValuePair in this.users)
			{
				string text;
				AdminWebUsers.WebUser webUser;
				keyValuePair.Deconstruct(out text, out webUser);
				AdminWebUsers.WebUser webUser2 = webUser;
				webUser2.ToXml(xmlElement);
			}
		}

		// Token: 0x0600D138 RID: 53560 RVA: 0x004BF62C File Offset: 0x004BD82C
		public void AddUser(string _name, string _password, PlatformUserIdentifierAbs _userIdentifier, PlatformUserIdentifierAbs _crossPlatformIdentifier)
		{
			AdminTools parent = this.Parent;
			lock (parent)
			{
				AdminWebUsers.WebUser webUser = new AdminWebUsers.WebUser(_name, _password, _userIdentifier, _crossPlatformIdentifier);
				this.users[_name] = webUser;
				this.Parent.Save();
			}
		}

		// Token: 0x0600D139 RID: 53561 RVA: 0x004BF68C File Offset: 0x004BD88C
		public bool RemoveUser(string _name)
		{
			AdminTools parent = this.Parent;
			bool flag3;
			lock (parent)
			{
				bool flag2 = this.users.Remove(_name);
				if (flag2)
				{
					this.Parent.Save();
				}
				flag3 = flag2;
			}
			return flag3;
		}

		// Token: 0x0600D13A RID: 53562 RVA: 0x004BF6E4 File Offset: 0x004BD8E4
		public Dictionary<string, AdminWebUsers.WebUser> GetUsers()
		{
			AdminTools parent = this.Parent;
			Dictionary<string, AdminWebUsers.WebUser> dictionary;
			lock (parent)
			{
				dictionary = this.users;
			}
			return dictionary;
		}

		// Token: 0x0600D13B RID: 53563 RVA: 0x004BF728 File Offset: 0x004BD928
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] Hash(string _input)
		{
			return new MD5Cng().ComputeHash(Encoding.UTF8.GetBytes(_input));
		}

		// Token: 0x0600D13C RID: 53564 RVA: 0x004BF740 File Offset: 0x004BD940
		public bool TryGetUser(string _name, string _password, out AdminWebUsers.WebUser _result)
		{
			AdminTools parent = this.Parent;
			bool flag2;
			lock (parent)
			{
				if (this.users.TryGetValue(_name, out _result) && _result.ValidatePassword(_password))
				{
					flag2 = true;
				}
				else
				{
					_result = default(AdminWebUsers.WebUser);
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600D13D RID: 53565 RVA: 0x004BF7A0 File Offset: 0x004BD9A0
		public bool HasUser(PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser, out AdminWebUsers.WebUser _result)
		{
			AdminTools parent = this.Parent;
			bool flag2;
			lock (parent)
			{
				_result = default(AdminWebUsers.WebUser);
				foreach (KeyValuePair<string, AdminWebUsers.WebUser> keyValuePair in this.users)
				{
					string text;
					AdminWebUsers.WebUser webUser;
					keyValuePair.Deconstruct(out text, out webUser);
					AdminWebUsers.WebUser webUser2 = webUser;
					if (object.Equals(webUser2.PlatformUser, _platformUser) && object.Equals(webUser2.CrossPlatformUser, _crossPlatformUser))
					{
						_result = webUser2;
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04009EAF RID: 40623
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebUsers.WebUser> users = new CaseInsensitiveStringDictionary<AdminWebUsers.WebUser>();

		// Token: 0x02001B2F RID: 6959
		public readonly struct WebUser
		{
			// Token: 0x0600D13E RID: 53566 RVA: 0x004BF858 File Offset: 0x004BDA58
			public WebUser(string _name, byte[] _passwordHash, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = _passwordHash;
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600D13F RID: 53567 RVA: 0x004BF877 File Offset: 0x004BDA77
			public WebUser(string _name, string _password, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = AdminWebUsers.Hash(_password);
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600D140 RID: 53568 RVA: 0x004BF89C File Offset: 0x004BDA9C
			public void ToXml(XmlElement _parent)
			{
				XmlElement xmlElement = _parent.AddXmlElement("user");
				xmlElement.SetAttrib("name", this.Name).SetAttrib("pass", Convert.ToBase64String(this.PasswordHash));
				this.PlatformUser.ToXml(xmlElement, "");
				PlatformUserIdentifierAbs crossPlatformUser = this.CrossPlatformUser;
				if (crossPlatformUser == null)
				{
					return;
				}
				crossPlatformUser.ToXml(xmlElement, "cross");
			}

			// Token: 0x0600D141 RID: 53569 RVA: 0x004BF904 File Offset: 0x004BDB04
			public static bool TryParse(XmlElement _element, out AdminWebUsers.WebUser _result)
			{
				_result = default(AdminWebUsers.WebUser);
				string text;
				if (!_element.TryGetAttribute("name", out text))
				{
					Log.Warning("[Web] [Perms] Ignoring user-entry because of missing 'name' attribute: " + _element.OuterXml);
					return false;
				}
				string text2;
				if (!_element.TryGetAttribute("pass", out text2))
				{
					Log.Warning("[Web] [Perms] Ignoring user-entry because of missing 'pass' attribute: " + _element.OuterXml);
					return false;
				}
				PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromXml(_element, false, null);
				if (platformUserIdentifierAbs == null)
				{
					Log.Warning("[Web] [Perms] Ignoring user-entry because of missing 'platform' or 'userid' attribute: " + _element.OuterXml);
					return false;
				}
				PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromXml(_element, false, "cross");
				byte[] array = Convert.FromBase64String(text2);
				_result = new AdminWebUsers.WebUser(text, array, platformUserIdentifierAbs, platformUserIdentifierAbs2);
				return true;
			}

			// Token: 0x0600D142 RID: 53570 RVA: 0x004BF9AD File Offset: 0x004BDBAD
			public bool ValidatePassword(string _password)
			{
				return Utils.ArrayEquals(AdminWebUsers.Hash(_password), this.PasswordHash);
			}

			// Token: 0x04009EB0 RID: 40624
			public readonly string Name;

			// Token: 0x04009EB1 RID: 40625
			public readonly byte[] PasswordHash;

			// Token: 0x04009EB2 RID: 40626
			public readonly PlatformUserIdentifierAbs PlatformUser;

			// Token: 0x04009EB3 RID: 40627
			public readonly PlatformUserIdentifierAbs CrossPlatformUser;
		}
	}
}
