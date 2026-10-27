using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Webserver.Permissions
{
	// Token: 0x02001B5F RID: 7007
	public class AdminWebUsers : AdminSectionAbs
	{
		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x0600D247 RID: 53831 RVA: 0x004C3052 File Offset: 0x004C1252
		public static AdminWebUsers Instance
		{
			get
			{
				return GameManager.Instance.adminTools.WebUsers;
			}
		}

		// Token: 0x0600D248 RID: 53832 RVA: 0x004C3063 File Offset: 0x004C1263
		public AdminWebUsers(AdminTools _parent)
			: base(_parent, "webusers")
		{
		}

		// Token: 0x0600D249 RID: 53833 RVA: 0x004C307C File Offset: 0x004C127C
		public override void Clear()
		{
			this.users.Clear();
		}

		// Token: 0x0600D24A RID: 53834 RVA: 0x004C308C File Offset: 0x004C128C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ParseElement(XmlElement _childElement)
		{
			AdminWebUsers.WebUser webUser;
			if (AdminWebUsers.WebUser.TryParse(_childElement, out webUser))
			{
				this.users[webUser.Name] = webUser;
			}
		}

		// Token: 0x0600D24B RID: 53835 RVA: 0x004C30B8 File Offset: 0x004C12B8
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

		// Token: 0x0600D24C RID: 53836 RVA: 0x004C3128 File Offset: 0x004C1328
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

		// Token: 0x0600D24D RID: 53837 RVA: 0x004C3188 File Offset: 0x004C1388
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

		// Token: 0x0600D24E RID: 53838 RVA: 0x004C31E0 File Offset: 0x004C13E0
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

		// Token: 0x0600D24F RID: 53839 RVA: 0x004C3224 File Offset: 0x004C1424
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] Hash(string _input)
		{
			byte[] array;
			using (MD5 md = MD5.Create())
			{
				array = md.ComputeHash(Encoding.UTF8.GetBytes(_input));
			}
			return array;
		}

		// Token: 0x0600D250 RID: 53840 RVA: 0x004C3268 File Offset: 0x004C1468
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

		// Token: 0x0600D251 RID: 53841 RVA: 0x004C32C8 File Offset: 0x004C14C8
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

		// Token: 0x04009F98 RID: 40856
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AdminWebUsers.WebUser> users = new CaseInsensitiveStringDictionary<AdminWebUsers.WebUser>();

		// Token: 0x02001B60 RID: 7008
		public readonly struct WebUser
		{
			// Token: 0x0600D252 RID: 53842 RVA: 0x004C3380 File Offset: 0x004C1580
			public WebUser(string _name, byte[] _passwordHash, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = _passwordHash;
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600D253 RID: 53843 RVA: 0x004C339F File Offset: 0x004C159F
			public WebUser(string _name, string _password, PlatformUserIdentifierAbs _platformUser, PlatformUserIdentifierAbs _crossPlatformUser)
			{
				this.Name = _name;
				this.PasswordHash = AdminWebUsers.Hash(_password);
				this.PlatformUser = _platformUser;
				this.CrossPlatformUser = _crossPlatformUser;
			}

			// Token: 0x0600D254 RID: 53844 RVA: 0x004C33C4 File Offset: 0x004C15C4
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

			// Token: 0x0600D255 RID: 53845 RVA: 0x004C342C File Offset: 0x004C162C
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

			// Token: 0x0600D256 RID: 53846 RVA: 0x004C34D5 File Offset: 0x004C16D5
			public bool ValidatePassword(string _password)
			{
				return Utils.ArrayEquals(AdminWebUsers.Hash(_password), this.PasswordHash);
			}

			// Token: 0x04009F99 RID: 40857
			public readonly string Name;

			// Token: 0x04009F9A RID: 40858
			public readonly byte[] PasswordHash;

			// Token: 0x04009F9B RID: 40859
			public readonly PlatformUserIdentifierAbs PlatformUser;

			// Token: 0x04009F9C RID: 40860
			public readonly PlatformUserIdentifierAbs CrossPlatformUser;
		}
	}
}
