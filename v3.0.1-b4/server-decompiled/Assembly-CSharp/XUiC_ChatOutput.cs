using System;
using System.Globalization;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDA RID: 3802
[Preserve]
public class XUiC_ChatOutput : XUiController
{
	// Token: 0x06007754 RID: 30548 RVA: 0x002CEAD8 File Offset: 0x002CCCD8
	public override void Init()
	{
		XUiC_ChatOutput.ID = this.windowGroup.Id;
		base.Init();
		this.txtOutput = (XUiV_TextList)base.GetChildById("txtOutput").ViewComponent;
		this.collider = this.txtOutput.UiTransform.GetComponent<BoxCollider>();
	}

	// Token: 0x06007755 RID: 30549 RVA: 0x002CEB2C File Offset: 0x002CCD2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addMessage(EnumGameMessages _messageType, EChatType _chatType, EChatDirection _chatDirection, string _message, string _senderDisplayName, string _senderHandlerId)
	{
		if (this.txtOutput == null)
		{
			return;
		}
		if (!ThreadManager.IsMainThread())
		{
			ThreadManager.AddSingleTaskMainThread("AddTextListLine", delegate(object _)
			{
				this.addMessage(_messageType, _chatType, _chatDirection, _message, _senderDisplayName, _senderHandlerId);
			}, null);
			return;
		}
		if (_messageType == EnumGameMessages.Chat)
		{
			if (_senderDisplayName == null)
			{
				_senderDisplayName = "";
			}
			if (_chatType == EChatType.Discord && !string.IsNullOrEmpty(_senderDisplayName))
			{
				_senderDisplayName += " [discord] ";
			}
			string text = (string.IsNullOrEmpty(_senderDisplayName) ? "" : (LabelUrlUtils.BuildUrlFunctionString("Chat", new ValueTuple<string, string>("ChatType", _chatType.ToStringCached<EChatType>()), new ValueTuple<string, string>("Sender", _senderHandlerId)) + _senderDisplayName + "[/url]: "));
			string text2 = (this.useChatTypeColorForMessage ? "" : ("[-][" + this.textColorMessage + "]"));
			string text3 = Localization.Get((_chatDirection == EChatDirection.Outbound) ? "xuiChatDirectionTo" : "xuiChatDirectionFrom", false, null);
			string text4;
			switch (_chatType)
			{
			case EChatType.Global:
				text4 = string.Concat(new string[]
				{
					"[",
					this.textColorGlobal,
					"]",
					text,
					text2,
					LabelUrlUtils.BuildUrlFunctionString("Chat", new ValueTuple<string, string>("ChatType", _chatType.ToStringCached<EChatType>())),
					_message,
					"[/url]"
				});
				break;
			case EChatType.Friends:
				text4 = string.Concat(new string[]
				{
					"[",
					this.textColorFriends,
					"]",
					LabelUrlUtils.BuildUrlFunctionString("Chat", new ValueTuple<string, string>("ChatType", _chatType.ToStringCached<EChatType>())),
					"(Friends)[/url] ",
					text,
					text2,
					_message
				});
				break;
			case EChatType.Party:
				text4 = string.Concat(new string[]
				{
					"[",
					this.textColorParty,
					"]",
					LabelUrlUtils.BuildUrlFunctionString("Chat", new ValueTuple<string, string>("ChatType", _chatType.ToStringCached<EChatType>())),
					"(Party)[/url] ",
					text,
					text2,
					_message
				});
				break;
			case EChatType.Whisper:
				text4 = string.Concat(new string[] { "[", this.textColorWhisper, "]", text3, " ", text, text2, _message });
				break;
			case EChatType.Discord:
				text4 = string.Concat(new string[] { "[5865f2]", text3, " ", text, text2, _message });
				break;
			default:
				throw new ArgumentOutOfRangeException("_chatType", _chatType, null);
			}
			_message = text4;
		}
		_message = _message.Replace('\n', ' ');
		this.txtOutput.AddLine(_message);
		this.txtOutput.Alpha = 1f;
		this.currentWaitTime = this.fadeoutWaitTime + this.fadeoutDuration;
		this.txtOutput.ScrollValue = 1f;
	}

	// Token: 0x06007756 RID: 30550 RVA: 0x002CEEC8 File Offset: 0x002CD0C8
	public static void AddMessage(XUi _xuiInstance, EnumGameMessages _messageType, string _message, EChatType _chatType = EChatType.Global, EChatDirection _chatDirection = EChatDirection.Inbound, int _senderId = -1, string _senderDisplayName = null, string _senderHandlerId = null, EMessageSender _messageSenderType = EMessageSender.None, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode _bbMode = GeneratedTextManager.BbCodeSupportMode.Supported)
	{
		if (_messageType == EnumGameMessages.Chat && !PermissionsManager.IsCommunicationAllowed())
		{
			return;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
		if (_senderId != -1 || _messageSenderType == EMessageSender.SenderIdAsPlayer)
		{
			PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_senderId);
			if (playerDataFromEntityID == null)
			{
				Log.Warning(string.Format("Could not find player name corresponding to entity id {0}, discarding message", _senderId));
				return;
			}
			if (playerDataFromEntityID.PlatformData.Blocked[EBlockType.TextChat].IsBlocked())
			{
				return;
			}
			platformUserIdentifierAbs = playerDataFromEntityID.PrimaryId;
			_senderDisplayName = playerDataFromEntityID.PlayerName.DisplayName;
		}
		if (_messageType == EnumGameMessages.Chat && _messageSenderType == EMessageSender.SenderIdAsPlayer)
		{
			_bbMode = GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes;
		}
		GeneratedTextManager.GetDisplayText(_message, platformUserIdentifierAbs, delegate(string _filteredMessage)
		{
			XUiController xuiController = _xuiInstance.FindWindowGroupByName(XUiC_ChatOutput.ID);
			if (xuiController != null)
			{
				_filteredMessage += "[ffffffff][/url][/b][/i][/u][/s][/sub][/sup]";
				XUiC_ChatOutput childByType = xuiController.GetChildByType<XUiC_ChatOutput>();
				string text;
				if (_messageSenderType == EMessageSender.Server)
				{
					text = Localization.Get("xuiChatServer", false, null);
				}
				else
				{
					text = _senderDisplayName;
				}
				_senderDisplayName = text;
				childByType.addMessage(_messageType, _chatType, _chatDirection, _filteredMessage, _senderDisplayName, _senderHandlerId);
			}
		}, false, _filteringMode, _bbMode);
	}

	// Token: 0x06007757 RID: 30551 RVA: 0x002CEFBC File Offset: 0x002CD1BC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = this.xui.playerUI.windowManager.IsWindowOpen(XUiC_Chat.ID);
		this.collider.enabled = flag;
		if (flag)
		{
			this.currentWaitTime = this.fadeoutWaitTime + this.fadeoutDuration;
		}
		this.txtOutput.Alpha = Mathf.Lerp(0f, 1f, this.currentWaitTime / this.fadeoutDuration);
		this.currentWaitTime -= Time.deltaTime;
		if (GameManager.Instance == null || GameManager.Instance.World == null || this.xui.playerUI.entityPlayer == null || this.xui.playerUI.entityPlayer.IsDead())
		{
			this.txtOutput.Alpha = 0f;
		}
	}

	// Token: 0x06007758 RID: 30552 RVA: 0x002CF0A0 File Offset: 0x002CD2A0
	public override bool ParseAttribute(string _name, string _value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
		if (num <= 1054300425U)
		{
			if (num <= 484394924U)
			{
				if (num != 395109698U)
				{
					if (num == 484394924U)
					{
						if (_name == "text_color_global")
						{
							this.textColorGlobal = _value;
							return true;
						}
					}
				}
				else if (_name == "fadeout_duration")
				{
					this.fadeoutDuration = StringParsers.ParseFloat(_value, 0, -1, NumberStyles.Any);
					return true;
				}
			}
			else if (num != 750825111U)
			{
				if (num == 1054300425U)
				{
					if (_name == "text_color_party")
					{
						this.textColorParty = _value;
						return true;
					}
				}
			}
			else if (_name == "text_color_whisper")
			{
				this.textColorWhisper = _value;
				return true;
			}
		}
		else if (num <= 3890498550U)
		{
			if (num != 3023599480U)
			{
				if (num == 3890498550U)
				{
					if (_name == "text_color_message")
					{
						this.textColorMessage = _value;
						return true;
					}
				}
			}
			else if (_name == "text_color_friends")
			{
				this.textColorFriends = _value;
				return true;
			}
		}
		else if (num != 4020806997U)
		{
			if (num == 4076460375U)
			{
				if (_name == "use_chattype_color_for_message")
				{
					this.useChatTypeColorForMessage = StringParsers.ParseBool(_value, 0, -1, true);
					return true;
				}
			}
		}
		else if (_name == "fadeout_wait_time")
		{
			this.fadeoutWaitTime = StringParsers.ParseFloat(_value, 0, -1, NumberStyles.Any);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x040057FB RID: 22523
	public static string ID = "";

	// Token: 0x040057FC RID: 22524
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_TextList txtOutput;

	// Token: 0x040057FD RID: 22525
	[PublicizedFrom(EAccessModifier.Private)]
	public BoxCollider collider;

	// Token: 0x040057FE RID: 22526
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeoutWaitTime = 5f;

	// Token: 0x040057FF RID: 22527
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeoutDuration = 2f;

	// Token: 0x04005800 RID: 22528
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentWaitTime;

	// Token: 0x04005801 RID: 22529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useChatTypeColorForMessage;

	// Token: 0x04005802 RID: 22530
	[PublicizedFrom(EAccessModifier.Private)]
	public string textColorMessage = Color.white.ToHexCode(false);

	// Token: 0x04005803 RID: 22531
	[PublicizedFrom(EAccessModifier.Private)]
	public string textColorGlobal = Color.white.ToHexCode(false);

	// Token: 0x04005804 RID: 22532
	[PublicizedFrom(EAccessModifier.Private)]
	public string textColorFriends = new Color(0f, 0.75f, 0f).ToHexCode(false);

	// Token: 0x04005805 RID: 22533
	[PublicizedFrom(EAccessModifier.Private)]
	public string textColorParty = new Color(1f, 0.8f, 0f).ToHexCode(false);

	// Token: 0x04005806 RID: 22534
	[PublicizedFrom(EAccessModifier.Private)]
	public string textColorWhisper = new Color(0.8f, 0f, 0f).ToHexCode(false);

	// Token: 0x04005807 RID: 22535
	[PublicizedFrom(EAccessModifier.Private)]
	public const string TextColorDiscord = "5865f2";
}
