using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001B02 RID: 6914
	public class AdminWebUsers : AdminSectionAbs
	{
		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x0600CFDB RID: 53211 RVA: 0x004BB08A File Offset: 0x004B928A
		public static AdminWebUsers Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebUsers;
			}
		}

		// Token: 0x0600CFDC RID: 53212 RVA: 0x004BB09B File Offset: 0x004B929B
		public AdminWebUsers(AdminTools _parent)
			: base(_parent, "webusers")
		{
		}

		// Token: 0x0600CFDD RID: 53213 RVA: 0x004BB0B4 File Offset: 0x004B92B4
		public override void Clear()
		{
			this.users.Clear();
		}

		// Token: 0x0600CFDE RID: 53214 RVA: 0x004BB0C4 File Offset: 0x004B92C4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminWebUsers.WebUser webUser;
			if (AdminWebUsers.WebUser.TryParse(_childElement, out webUser))
			{
				this.users[webUser.Name] = webUser;
			}
		}

		// Token: 0x0600CFDF RID: 53215 RVA: 0x004BB0F0 File Offset: 0x004B92F0
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

		// Token: 0x0600CFE0 RID: 53216 RVA: 0x004BB160 File Offset: 0x004B9360
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

		// Token: 0x0600CFE1 RID: 53217 RVA: 0x004BB1C0 File Offset: 0x004B93C0
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

		// Token: 0x0600CFE2 RID: 53218 RVA: 0x004BB218 File Offset: 0x004B9418
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

		// Token: 0x0600CFE3 RID: 53219 RVA: 0x004BB25C File Offset: 0x004B945C
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] Hash(string _input)
		{
			return new MD5Cng().ComputeHash(Encoding.UTF8.GetBytes(_input));
		}

		// Token: 0x0600CFE4 RID: 53220 RVA: 0x004BB274 File Offset: 0x004B9474
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

		// Token: 0x0600CFE5 RID: 53221 RVA: 0x004BB2D4 File Offset: 0x004B94D4
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

		// Token: 0x04009DE9 RID: 40425
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebUsers.WebUser> users = new CaseInsensitiveStringDictionary<AdminWebUsers.WebUser>();

		// Token: 0x02001B03 RID: 6915
		public readonly struct WebUser
		{
			// Token: 0x0600CFE6 RID: 53222 RVA: 0x004BB38C File Offset: 0x004B958C
			public WebUser(string _name, byte[] _passwordHash, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = _passwordHash;
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600CFE7 RID: 53223 RVA: 0x004BB3AB File Offset: 0x004B95AB
			public WebUser(string _name, string _password, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = AdminWebUsers.Hash(_password);
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600CFE8 RID: 53224 RVA: 0x004BB3D0 File Offset: 0x004B95D0
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

			// Token: 0x0600CFE9 RID: 53225 RVA: 0x004BB438 File Offset: 0x004B9638
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

			// Token: 0x0600CFEA RID: 53226 RVA: 0x004BB4E1 File Offset: 0x004B96E1
			public bool ValidatePassword(string _password)
			{
				return Utils.ArrayEquals(AdminWebUsers.Hash(_password), this.PasswordHash);
			}

			// Token: 0x04009DEA RID: 40426
			public readonly string Name;

			// Token: 0x04009DEB RID: 40427
			public readonly byte[] PasswordHash;

			// Token: 0x04009DEC RID: 40428
			public readonly PlatformUserIdentifierAbs PlatformUser;

			// Token: 0x04009DED RID: 40429
			public readonly PlatformUserIdentifierAbs CrossPlatformUser;
		}
	}
}
