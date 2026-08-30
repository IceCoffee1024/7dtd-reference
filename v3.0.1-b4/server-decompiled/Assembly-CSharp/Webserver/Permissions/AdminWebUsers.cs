using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001ADB RID: 6875
	public class AdminWebUsers : AdminSectionAbs
	{
		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x0600CE52 RID: 52818 RVA: 0x004B4C96 File Offset: 0x004B2E96
		public static AdminWebUsers Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebUsers;
			}
		}

		// Token: 0x0600CE53 RID: 52819 RVA: 0x004B4CA7 File Offset: 0x004B2EA7
		public AdminWebUsers(AdminTools _parent)
			: base(_parent, "webusers")
		{
		}

		// Token: 0x0600CE54 RID: 52820 RVA: 0x004B4CC0 File Offset: 0x004B2EC0
		public override void Clear()
		{
			this.users.Clear();
		}

		// Token: 0x0600CE55 RID: 52821 RVA: 0x004B4CD0 File Offset: 0x004B2ED0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminWebUsers.WebUser webUser;
			if (AdminWebUsers.WebUser.TryParse(_childElement, out webUser))
			{
				this.users[webUser.Name] = webUser;
			}
		}

		// Token: 0x0600CE56 RID: 52822 RVA: 0x004B4CFC File Offset: 0x004B2EFC
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

		// Token: 0x0600CE57 RID: 52823 RVA: 0x004B4D6C File Offset: 0x004B2F6C
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

		// Token: 0x0600CE58 RID: 52824 RVA: 0x004B4DCC File Offset: 0x004B2FCC
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

		// Token: 0x0600CE59 RID: 52825 RVA: 0x004B4E24 File Offset: 0x004B3024
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

		// Token: 0x0600CE5A RID: 52826 RVA: 0x004B4E68 File Offset: 0x004B3068
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] Hash(string _input)
		{
			return new MD5Cng().ComputeHash(Encoding.UTF8.GetBytes(_input));
		}

		// Token: 0x0600CE5B RID: 52827 RVA: 0x004B4E80 File Offset: 0x004B3080
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

		// Token: 0x0600CE5C RID: 52828 RVA: 0x004B4EE0 File Offset: 0x004B30E0
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

		// Token: 0x04009CB8 RID: 40120
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebUsers.WebUser> users = new CaseInsensitiveStringDictionary<AdminWebUsers.WebUser>();

		// Token: 0x02001ADC RID: 6876
		public readonly struct WebUser
		{
			// Token: 0x0600CE5D RID: 52829 RVA: 0x004B4F98 File Offset: 0x004B3198
			public WebUser(string _name, byte[] _passwordHash, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = _passwordHash;
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600CE5E RID: 52830 RVA: 0x004B4FB7 File Offset: 0x004B31B7
			public WebUser(string _name, string _password, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = AdminWebUsers.Hash(_password);
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600CE5F RID: 52831 RVA: 0x004B4FDC File Offset: 0x004B31DC
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

			// Token: 0x0600CE60 RID: 52832 RVA: 0x004B5044 File Offset: 0x004B3244
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

			// Token: 0x0600CE61 RID: 52833 RVA: 0x004B50ED File Offset: 0x004B32ED
			public bool ValidatePassword(string _password)
			{
				return Utils.ArrayEquals(AdminWebUsers.Hash(_password), this.PasswordHash);
			}

			// Token: 0x04009CB9 RID: 40121
			public readonly string Name;

			// Token: 0x04009CBA RID: 40122
			public readonly byte[] PasswordHash;

			// Token: 0x04009CBB RID: 40123
			public readonly PlatformUserIdentifierAbs PlatformUser;

			// Token: 0x04009CBC RID: 40124
			public readonly PlatformUserIdentifierAbs CrossPlatformUser;
		}
	}
}
