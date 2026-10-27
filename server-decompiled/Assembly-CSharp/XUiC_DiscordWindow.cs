using System;
using UnityEngine.Scripting;

// Token: 0x0200035B RID: 859
[Preserve]
public class XUiC_DiscordWindow : XUiController
{
	// Token: 0x060018B6 RID: 6326 RVA: 0x0008C03C File Offset: 0x0008A23C
	public override void Init()
	{
		base.Init();
		XUiC_DiscordWindow.ID = base.WindowGroup.Id;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnLoginAccount") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += delegate(XUiController _, int _)
			{
				DiscordManager.Instance.AuthManager.LoginDiscordUser();
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnLoginProvisional") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += delegate(XUiController _, int _)
			{
				DiscordManager.Instance.AuthManager.LoginProvisionalAccount();
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton3 = base.GetChildById("btnDisconnect") as XUiC_SimpleButton;
		if (xuiC_SimpleButton3 != null)
		{
			xuiC_SimpleButton3.OnPressed += delegate(XUiController _, int _)
			{
				DiscordManager.Instance.AuthManager.Disconnect();
			};
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x0008C10A File Offset: 0x0008A30A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x0008C11C File Offset: 0x0008A31C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		DiscordManager instance = DiscordManager.Instance;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1339365942U)
		{
			if (num <= 927036493U)
			{
				if (num != 296438015U)
				{
					if (num == 927036493U)
					{
						if (_bindingName == "supports_provisional_accounts")
						{
							_value = DiscordManager.SupportsProvisionalAccounts.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "userid")
				{
					DiscordManager.DiscordUser localUser = instance.LocalUser;
					_value = ((localUser != null) ? localUser.ID.ToString() : null) ?? "";
					return true;
				}
			}
			else if (num != 1108639912U)
			{
				if (num == 1339365942U)
				{
					if (_bindingName == "displayname")
					{
						DiscordManager.DiscordUser localUser2 = instance.LocalUser;
						_value = ((localUser2 != null) ? localUser2.DisplayName : null) ?? "";
						return true;
					}
				}
			}
			else if (_bindingName == "supports_full_accounts")
			{
				_value = DiscordManager.SupportsFullAccounts.ToString();
				return true;
			}
		}
		else if (num <= 3125508079U)
		{
			if (num != 2529602836U)
			{
				if (num == 3125508079U)
				{
					if (_bindingName == "status")
					{
						_value = instance.Status.ToStringCached<DiscordManager.EDiscordStatus>();
						return true;
					}
				}
			}
			else if (_bindingName == "discorddisplayname")
			{
				DiscordManager.DiscordUser localUser3 = instance.LocalUser;
				_value = ((localUser3 != null) ? localUser3.DiscordDisplayName : null) ?? "";
				return true;
			}
		}
		else if (num != 4013119319U)
		{
			if (num == 4074759933U)
			{
				if (_bindingName == "is_ready")
				{
					_value = instance.IsReady.ToString();
					return true;
				}
			}
		}
		else if (_bindingName == "discordusername")
		{
			DiscordManager.DiscordUser localUser4 = instance.LocalUser;
			_value = ((localUser4 != null) ? localUser4.DiscordUserName : null) ?? "";
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04000FD0 RID: 4048
	public static string ID = "";
}
