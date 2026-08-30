using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000EE7 RID: 3815
[Preserve]
public class XUiC_Chat : XUiController
{
	// Token: 0x060077CA RID: 30666 RVA: 0x002D15D4 File Offset: 0x002CF7D4
	public override void Init()
	{
		XUiC_Chat.ID = this.windowGroup.Id;
		base.Init();
		this.cbxTarget = base.GetChildByType<XUiC_ComboBoxList<XUiC_Chat.ChatTarget>>();
		this.txtInput = base.GetChildByType<XUiC_TextInput>();
		this.txtInput.OnSubmitHandler += this.TextInput_OnSubmitHandler;
		this.txtInput.OnInputAbortedHandler += this.TextInput_OnInputAbortedHandler;
		this.txtInput.SupportBbCode = false;
		this.registerRegularChatMessageHandlers();
	}

	// Token: 0x060077CB RID: 30667 RVA: 0x002D1650 File Offset: 0x002CF850
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerRegularChatMessageHandlers()
	{
		XUiC_Chat.RegisterCustomMessagingHandler(EChatType.Global, new XUiC_Chat.IsValidTarget(XUiC_Chat.<registerRegularChatMessageHandlers>g__IsValidTargetRegular|9_4), new XUiC_Chat.GetTargetDisplayName(XUiC_Chat.<registerRegularChatMessageHandlers>g__GetTargetDisplayNameRegular|9_3), new XUiC_Chat.SendMessage(this.<registerRegularChatMessageHandlers>g__SendMessageRegular|9_2));
		XUiC_Chat.RegisterCustomMessagingHandler(EChatType.Friends, new XUiC_Chat.IsValidTarget(XUiC_Chat.<registerRegularChatMessageHandlers>g__IsValidTargetRegular|9_4), new XUiC_Chat.GetTargetDisplayName(XUiC_Chat.<registerRegularChatMessageHandlers>g__GetTargetDisplayNameRegular|9_3), new XUiC_Chat.SendMessage(this.<registerRegularChatMessageHandlers>g__SendMessageRegular|9_2));
		XUiC_Chat.RegisterCustomMessagingHandler(EChatType.Party, new XUiC_Chat.IsValidTarget(XUiC_Chat.<registerRegularChatMessageHandlers>g__IsValidTargetRegular|9_4), new XUiC_Chat.GetTargetDisplayName(XUiC_Chat.<registerRegularChatMessageHandlers>g__GetTargetDisplayNameRegular|9_3), new XUiC_Chat.SendMessage(this.<registerRegularChatMessageHandlers>g__SendMessageRegular|9_2));
		XUiC_Chat.RegisterCustomMessagingHandler(EChatType.Whisper, new XUiC_Chat.IsValidTarget(XUiC_Chat.<registerRegularChatMessageHandlers>g__IsValidTargetRegular|9_4), new XUiC_Chat.GetTargetDisplayName(XUiC_Chat.<registerRegularChatMessageHandlers>g__GetTargetDisplayNameRegular|9_3), new XUiC_Chat.SendMessage(this.<registerRegularChatMessageHandlers>g__SendMessageRegular|9_2));
		this.allTargets.Add(new XUiC_Chat.ChatTarget(EChatType.Global, null, null, true));
		this.allTargets.Add(new XUiC_Chat.ChatTarget(EChatType.Friends, null, (XUiC_Chat.ChatTarget _, XUi _) => this.entityIdsFriends.Count > 1, true));
		this.allTargets.Add(new XUiC_Chat.ChatTarget(EChatType.Party, null, (XUiC_Chat.ChatTarget _, XUi _) => this.entityIdsParty.Count > 1, true));
	}

	// Token: 0x060077CC RID: 30668 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060077CD RID: 30669 RVA: 0x002D1758 File Offset: 0x002CF958
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnSubmitHandler(XUiController _sender, string _text)
	{
		if (_text.Length > 0 && _text != " ")
		{
			_text = _text.Replace('\n', ' ');
			this.cbxTarget.Value.Send(_text);
			this.txtInput.Text = "";
		}
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060077CE RID: 30670 RVA: 0x002D17C4 File Offset: 0x002CF9C4
	public override void OnOpen()
	{
		this.cbxTarget.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.txtInput.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.updateTargets();
		base.OnOpen();
	}

	// Token: 0x060077CF RID: 30671 RVA: 0x002D17F4 File Offset: 0x002CF9F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTargets()
	{
		this.entityIdsFriends.Clear();
		this.entityIdsFriends.Add(this.xui.playerUI.entityPlayer.entityId);
		foreach (EntityPlayer entityPlayer in GameManager.Instance.World.Players.list)
		{
			if (entityPlayer.IsFriendOfLocalPlayer)
			{
				this.entityIdsFriends.Add(entityPlayer.entityId);
			}
		}
		this.entityIdsParty.Clear();
		this.entityIdsParty.Add(this.xui.playerUI.entityPlayer.entityId);
		if (this.xui.playerUI.entityPlayer.Party != null)
		{
			foreach (EntityPlayer entityPlayer2 in this.xui.playerUI.entityPlayer.Party.MemberList)
			{
				if (entityPlayer2 != this.xui.playerUI.entityPlayer)
				{
					this.entityIdsParty.Add(entityPlayer2.entityId);
				}
			}
		}
		this.allTargets.Sort();
		XUiC_Chat.ChatTarget value = this.cbxTarget.Value;
		this.cbxTarget.Elements.Clear();
		for (int i = this.allTargets.Count - 1; i >= 0; i--)
		{
			XUiC_Chat.ChatTarget chatTarget = this.allTargets[i];
			if (!chatTarget.KeepForever && chatTarget.Age.TotalMinutes > this.keepWhisperTargetsForMinutes)
			{
				this.allTargets.RemoveAt(i);
			}
			else if (!chatTarget.IsValid(this.xui))
			{
				if (!chatTarget.KeepForever)
				{
					this.allTargets.RemoveAt(i);
				}
			}
			else
			{
				this.cbxTarget.Elements.Insert(0, chatTarget);
			}
		}
		if (value != null)
		{
			int num = this.cbxTarget.Elements.IndexOf(value);
			this.cbxTarget.SelectedIndex = ((num < 0) ? 0 : num);
		}
	}

	// Token: 0x060077D0 RID: 30672 RVA: 0x002D1A3C File Offset: 0x002CFC3C
	[PublicizedFrom(EAccessModifier.Private)]
	public int enforceTarget(EChatType _chatType, string _targetId)
	{
		int num = this.cbxTarget.Elements.FindIndex((XUiC_Chat.ChatTarget _target) => _target.ChatType == _chatType && _targetId == _target.TargetId);
		if (num >= 0)
		{
			return num;
		}
		if (string.IsNullOrEmpty(_targetId))
		{
			return -1;
		}
		this.allTargets.Add(new XUiC_Chat.ChatTarget(_chatType, _targetId, null, false));
		this.updateTargets();
		return this.cbxTarget.Elements.FindIndex((XUiC_Chat.ChatTarget _target) => _target.ChatType == _chatType && _targetId == _target.TargetId);
	}

	// Token: 0x060077D1 RID: 30673 RVA: 0x002D1AD0 File Offset: 0x002CFCD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void findOrAddTarget(EChatType _chatType, string _targetId)
	{
		int num = this.enforceTarget(_chatType, _targetId);
		if (num >= 0)
		{
			this.cbxTarget.SelectedIndex = num;
			this.txtInput.SetSelected(true, false);
		}
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x002D1B03 File Offset: 0x002CFD03
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x002D1B14 File Offset: 0x002CFD14
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if (this.xui.playerUI.playerInput != null)
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
				if (guiactions.Up.WasPressed)
				{
					this.cbxTarget.ChangeIndex(-1);
				}
				if (guiactions.Down.WasPressed)
				{
					this.cbxTarget.ChangeIndex(1);
				}
			}
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed)
			{
				this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			}
		}
	}

	// Token: 0x060077D4 RID: 30676 RVA: 0x002D1BCB File Offset: 0x002CFDCB
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "keep_whisper_targets_for_minutes")
		{
			this.keepWhisperTargetsForMinutes = StringParsers.ParseUInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060077D5 RID: 30677 RVA: 0x002D1BF4 File Offset: 0x002CFDF4
	public static void RegisterCustomMessagingHandler(EChatType _chatType, XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
	{
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length)
		{
			Log.Warning(string.Format("Can not register chat messaging handler for invalid chat type '{0}'", (int)_chatType));
			return;
		}
		XUiC_Chat.messagingHandlers[(int)_chatType] = new XUiC_Chat.ChatMessagingHandler(_isValidTargetDelegate, _getTargetDisplayNameDelegate, _sendMessageDelegate);
	}

	// Token: 0x060077D6 RID: 30678 RVA: 0x002D1C34 File Offset: 0x002CFE34
	public static void SetChatTarget(XUi _xuiInstance, EChatType _chatType, string _targetId)
	{
		XUiC_Chat windowByType = _xuiInstance.GetWindowByType<XUiC_Chat>();
		if (windowByType == null)
		{
			Log.Error("No chat window found!");
			return;
		}
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length || XUiC_Chat.messagingHandlers[(int)_chatType] == null)
		{
			Log.Warning(string.Format("Can not handle chat messaging, invalid chat type '{0}' or no handler defined", (int)_chatType));
			return;
		}
		if (!XUiC_Chat.messagingHandlers[(int)_chatType].IsValidTargetDelegate(_chatType, _targetId))
		{
			return;
		}
		windowByType.findOrAddTarget(_chatType, _targetId);
	}

	// Token: 0x060077D7 RID: 30679 RVA: 0x002D1CA0 File Offset: 0x002CFEA0
	public static bool GetChatTargetTooltip(XUi _xuiInstance, EChatType _chatType, string _targetId, out string _targetTooltip)
	{
		_targetTooltip = null;
		if (_xuiInstance.GetWindowByType<XUiC_Chat>() == null)
		{
			Log.Error("No chat window found!");
			return false;
		}
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length || XUiC_Chat.messagingHandlers[(int)_chatType] == null)
		{
			Log.Warning(string.Format("Can not handle chat messaging, invalid chat type '{0}' or no handler defined", (int)_chatType));
			return false;
		}
		if (!XUiC_Chat.messagingHandlers[(int)_chatType].IsValidTargetDelegate(_chatType, _targetId))
		{
			return false;
		}
		_targetTooltip = XUiC_Chat.messagingHandlers[(int)_chatType].GetTargetDisplayNameDelegate(_chatType, _targetId);
		return true;
	}

	// Token: 0x060077D8 RID: 30680 RVA: 0x002D1D20 File Offset: 0x002CFF20
	public static void EnforceTargetExists(XUi _xuiInstance, EChatType _chatType, string _targetId)
	{
		XUiC_Chat windowByType = _xuiInstance.GetWindowByType<XUiC_Chat>();
		if (windowByType == null)
		{
			Log.Error("No chat window found!");
			return;
		}
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length || XUiC_Chat.messagingHandlers[(int)_chatType] == null)
		{
			Log.Warning(string.Format("Can not handle chat messaging, invalid chat type '{0}' or no handler defined", (int)_chatType));
			return;
		}
		if (!XUiC_Chat.messagingHandlers[(int)_chatType].IsValidTargetDelegate(_chatType, _targetId))
		{
			return;
		}
		windowByType.enforceTarget(_chatType, _targetId);
	}

	// Token: 0x060077DD RID: 30685 RVA: 0x002D1DFC File Offset: 0x002CFFFC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <registerRegularChatMessageHandlers>g__SendMessageRegular|9_2(EChatType _chatType, string _targetId, string _message)
	{
		List<int> list2;
		if (_targetId == null)
		{
			List<int> list;
			switch (_chatType)
			{
			case EChatType.Global:
				list = null;
				break;
			case EChatType.Friends:
				list = this.entityIdsFriends;
				break;
			case EChatType.Party:
				list = this.entityIdsParty;
				break;
			case EChatType.Whisper:
				throw new ArgumentException("Whisper without ID not supported");
			default:
				throw new ArgumentException("Invalid chat type without ID");
			}
			list2 = list;
		}
		else
		{
			int num;
			if (!int.TryParse(_targetId, out num))
			{
				throw new ArgumentException("Could not parse chat entity id '" + _targetId + "'");
			}
			_chatType = EChatType.Whisper;
			list2 = new List<int> { num };
		}
		GameManager.Instance.ChatMessageServer(null, _chatType, this.xui.playerUI.entityPlayer.entityId, _message, list2, EMessageSender.SenderIdAsPlayer, GeneratedTextManager.BbCodeSupportMode.Supported);
	}

	// Token: 0x060077DE RID: 30686 RVA: 0x002D1EAC File Offset: 0x002D00AC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <registerRegularChatMessageHandlers>g__GetTargetDisplayNameRegular|9_3(EChatType _chatType, string _targetId)
	{
		if (_targetId == null)
		{
			return Localization.Get("xuiChatTarget" + _chatType.ToStringCached<EChatType>(), false, null);
		}
		int num;
		if (!int.TryParse(_targetId, out num))
		{
			throw new ArgumentException("Could not parse chat entity id '" + _targetId + "'");
		}
		EntityPlayer entityPlayer;
		if (!GameManager.Instance.World.Players.dict.TryGetValue(num, out entityPlayer))
		{
			return Localization.Get("xuiChatTarget" + _chatType.ToStringCached<EChatType>(), false, null);
		}
		return string.Format(Localization.Get("xuiChatTargetWhisper", false, null), entityPlayer.PlayerDisplayName);
	}

	// Token: 0x060077DF RID: 30687 RVA: 0x002D1F44 File Offset: 0x002D0144
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <registerRegularChatMessageHandlers>g__IsValidTargetRegular|9_4(EChatType _chatType, string _targetId)
	{
		if (_targetId == null)
		{
			return true;
		}
		int num;
		if (!int.TryParse(_targetId, out num))
		{
			throw new ArgumentException("Could not parse chat entity id '" + _targetId + "' for target validation");
		}
		EntityPlayer entityPlayer;
		return GameManager.Instance.World.Players.dict.TryGetValue(num, out entityPlayer) && num != GameManager.Instance.World.GetPrimaryPlayer().entityId;
	}

	// Token: 0x04005889 RID: 22665
	public static string ID = "";

	// Token: 0x0400588A RID: 22666
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_Chat.ChatTarget> cbxTarget;

	// Token: 0x0400588B RID: 22667
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400588C RID: 22668
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsFriends = new List<int>();

	// Token: 0x0400588D RID: 22669
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsParty = new List<int>();

	// Token: 0x0400588E RID: 22670
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_Chat.ChatTarget> allTargets = new List<XUiC_Chat.ChatTarget>();

	// Token: 0x0400588F RID: 22671
	[PublicizedFrom(EAccessModifier.Private)]
	public uint keepWhisperTargetsForMinutes = 15U;

	// Token: 0x04005890 RID: 22672
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XUiC_Chat.ChatMessagingHandler[] messagingHandlers = new XUiC_Chat.ChatMessagingHandler[(int)(EnumUtils.MaxValue<EChatType>() + 1)];

	// Token: 0x02000EE8 RID: 3816
	[UsedImplicitly]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatTarget : IComparable<XUiC_Chat.ChatTarget>
	{
		// Token: 0x060077E0 RID: 30688 RVA: 0x002D1FB0 File Offset: 0x002D01B0
		public ChatTarget(EChatType _chatType, string _targetId = null, Func<XUiC_Chat.ChatTarget, XUi, bool> _validCondition = null, bool _keepForever = false)
		{
			this.ChatType = _chatType;
			this.TargetId = _targetId;
			this.ValidCondition = _validCondition;
			this.messageHandler = XUiC_Chat.messagingHandlers[(int)_chatType];
			if (this.messageHandler == null)
			{
				throw new ArgumentException(string.Format("Missing handler for chat type {0}", _chatType));
			}
			this.displayText = this.messageHandler.GetTargetDisplayNameDelegate(_chatType, _targetId);
			this.KeepForever = _keepForever;
			this.lastUsed = DateTime.Now;
		}

		// Token: 0x060077E1 RID: 30689 RVA: 0x002D202E File Offset: 0x002D022E
		public override string ToString()
		{
			return this.displayText;
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x002D2036 File Offset: 0x002D0236
		public void Send(string _message)
		{
			this.messageHandler.SendMessageDelegate(this.ChatType, this.TargetId, _message);
			this.lastUsed = DateTime.Now;
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x060077E3 RID: 30691 RVA: 0x002D2060 File Offset: 0x002D0260
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastUsed;
			}
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x002D2072 File Offset: 0x002D0272
		public bool IsValid(XUi _xuiInstance)
		{
			Func<XUiC_Chat.ChatTarget, XUi, bool> validCondition = this.ValidCondition;
			if (validCondition == null)
			{
				XUiC_Chat.ChatMessagingHandler chatMessagingHandler = this.messageHandler;
				return chatMessagingHandler == null || chatMessagingHandler.IsValidTargetDelegate(this.ChatType, this.TargetId);
			}
			return validCondition(this, _xuiInstance);
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x002D20A8 File Offset: 0x002D02A8
		public int CompareTo(XUiC_Chat.ChatTarget _other)
		{
			if (this == _other)
			{
				return 0;
			}
			if (_other == null)
			{
				return -1;
			}
			int num = -this.KeepForever.CompareTo(_other.KeepForever);
			if (num != 0)
			{
				return num;
			}
			if (this.KeepForever)
			{
				num = this.ChatType.CompareTo(_other.ChatType);
				if (num != 0)
				{
					return num;
				}
			}
			return this.Age.CompareTo(_other.Age);
		}

		// Token: 0x04005891 RID: 22673
		public readonly EChatType ChatType;

		// Token: 0x04005892 RID: 22674
		public readonly string TargetId;

		// Token: 0x04005893 RID: 22675
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Chat.ChatMessagingHandler messageHandler;

		// Token: 0x04005894 RID: 22676
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayText;

		// Token: 0x04005895 RID: 22677
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastUsed;

		// Token: 0x04005896 RID: 22678
		public readonly bool KeepForever;

		// Token: 0x04005897 RID: 22679
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<XUiC_Chat.ChatTarget, XUi, bool> ValidCondition;
	}

	// Token: 0x02000EE9 RID: 3817
	// (Invoke) Token: 0x060077E7 RID: 30695
	public delegate bool IsValidTarget(EChatType _chatType, string _targetId);

	// Token: 0x02000EEA RID: 3818
	// (Invoke) Token: 0x060077EB RID: 30699
	public delegate string GetTargetDisplayName(EChatType _chatType, string _targetId);

	// Token: 0x02000EEB RID: 3819
	// (Invoke) Token: 0x060077EF RID: 30703
	public delegate void SendMessage(EChatType _chatType, string _targetId, string _message);

	// Token: 0x02000EEC RID: 3820
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatMessagingHandler
	{
		// Token: 0x060077F2 RID: 30706 RVA: 0x002D211B File Offset: 0x002D031B
		public ChatMessagingHandler(XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
		{
			this.IsValidTargetDelegate = _isValidTargetDelegate;
			this.GetTargetDisplayNameDelegate = _getTargetDisplayNameDelegate;
			this.SendMessageDelegate = _sendMessageDelegate;
		}

		// Token: 0x04005898 RID: 22680
		public readonly XUiC_Chat.IsValidTarget IsValidTargetDelegate;

		// Token: 0x04005899 RID: 22681
		public readonly XUiC_Chat.GetTargetDisplayName GetTargetDisplayNameDelegate;

		// Token: 0x0400589A RID: 22682
		public readonly XUiC_Chat.SendMessage SendMessageDelegate;
	}
}
