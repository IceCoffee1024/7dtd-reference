using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000ED2 RID: 3794
[Preserve]
public class XUiC_Chat : XUiController
{
	// Token: 0x06007728 RID: 30504 RVA: 0x002CDF50 File Offset: 0x002CC150
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

	// Token: 0x06007729 RID: 30505 RVA: 0x002CDFCC File Offset: 0x002CC1CC
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

	// Token: 0x0600772A RID: 30506 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600772B RID: 30507 RVA: 0x002CE0D4 File Offset: 0x002CC2D4
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

	// Token: 0x0600772C RID: 30508 RVA: 0x002CE140 File Offset: 0x002CC340
	public override void OnOpen()
	{
		this.cbxTarget.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.txtInput.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.updateTargets();
		base.OnOpen();
	}

	// Token: 0x0600772D RID: 30509 RVA: 0x002CE170 File Offset: 0x002CC370
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

	// Token: 0x0600772E RID: 30510 RVA: 0x002CE3B8 File Offset: 0x002CC5B8
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

	// Token: 0x0600772F RID: 30511 RVA: 0x002CE44C File Offset: 0x002CC64C
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

	// Token: 0x06007730 RID: 30512 RVA: 0x002CE47F File Offset: 0x002CC67F
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06007731 RID: 30513 RVA: 0x002CE490 File Offset: 0x002CC690
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

	// Token: 0x06007732 RID: 30514 RVA: 0x002CE547 File Offset: 0x002CC747
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "keep_whisper_targets_for_minutes")
		{
			this.keepWhisperTargetsForMinutes = StringParsers.ParseUInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007733 RID: 30515 RVA: 0x002CE570 File Offset: 0x002CC770
	public static void RegisterCustomMessagingHandler(EChatType _chatType, XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
	{
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length)
		{
			Log.Warning(string.Format("Can not register chat messaging handler for invalid chat type '{0}'", (int)_chatType));
			return;
		}
		XUiC_Chat.messagingHandlers[(int)_chatType] = new XUiC_Chat.ChatMessagingHandler(_isValidTargetDelegate, _getTargetDisplayNameDelegate, _sendMessageDelegate);
	}

	// Token: 0x06007734 RID: 30516 RVA: 0x002CE5B0 File Offset: 0x002CC7B0
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

	// Token: 0x06007735 RID: 30517 RVA: 0x002CE61C File Offset: 0x002CC81C
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

	// Token: 0x06007736 RID: 30518 RVA: 0x002CE69C File Offset: 0x002CC89C
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

	// Token: 0x0600773B RID: 30523 RVA: 0x002CE778 File Offset: 0x002CC978
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

	// Token: 0x0600773C RID: 30524 RVA: 0x002CE828 File Offset: 0x002CCA28
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

	// Token: 0x0600773D RID: 30525 RVA: 0x002CE8C0 File Offset: 0x002CCAC0
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

	// Token: 0x040057E3 RID: 22499
	public static string ID = "";

	// Token: 0x040057E4 RID: 22500
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_Chat.ChatTarget> cbxTarget;

	// Token: 0x040057E5 RID: 22501
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040057E6 RID: 22502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsFriends = new List<int>();

	// Token: 0x040057E7 RID: 22503
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsParty = new List<int>();

	// Token: 0x040057E8 RID: 22504
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_Chat.ChatTarget> allTargets = new List<XUiC_Chat.ChatTarget>();

	// Token: 0x040057E9 RID: 22505
	[PublicizedFrom(EAccessModifier.Private)]
	public uint keepWhisperTargetsForMinutes = 15U;

	// Token: 0x040057EA RID: 22506
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XUiC_Chat.ChatMessagingHandler[] messagingHandlers = new XUiC_Chat.ChatMessagingHandler[(int)(EnumUtils.MaxValue<EChatType>() + 1)];

	// Token: 0x02000ED3 RID: 3795
	[UsedImplicitly]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatTarget : IComparable<XUiC_Chat.ChatTarget>
	{
		// Token: 0x0600773E RID: 30526 RVA: 0x002CE92C File Offset: 0x002CCB2C
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

		// Token: 0x0600773F RID: 30527 RVA: 0x002CE9AA File Offset: 0x002CCBAA
		public override string ToString()
		{
			return this.displayText;
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x002CE9B2 File Offset: 0x002CCBB2
		public void Send(string _message)
		{
			this.messageHandler.SendMessageDelegate(this.ChatType, this.TargetId, _message);
			this.lastUsed = DateTime.Now;
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06007741 RID: 30529 RVA: 0x002CE9DC File Offset: 0x002CCBDC
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastUsed;
			}
		}

		// Token: 0x06007742 RID: 30530 RVA: 0x002CE9EE File Offset: 0x002CCBEE
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

		// Token: 0x06007743 RID: 30531 RVA: 0x002CEA24 File Offset: 0x002CCC24
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

		// Token: 0x040057EB RID: 22507
		public readonly EChatType ChatType;

		// Token: 0x040057EC RID: 22508
		public readonly string TargetId;

		// Token: 0x040057ED RID: 22509
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Chat.ChatMessagingHandler messageHandler;

		// Token: 0x040057EE RID: 22510
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayText;

		// Token: 0x040057EF RID: 22511
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastUsed;

		// Token: 0x040057F0 RID: 22512
		public readonly bool KeepForever;

		// Token: 0x040057F1 RID: 22513
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<XUiC_Chat.ChatTarget, XUi, bool> ValidCondition;
	}

	// Token: 0x02000ED4 RID: 3796
	// (Invoke) Token: 0x06007745 RID: 30533
	public delegate bool IsValidTarget(EChatType _chatType, string _targetId);

	// Token: 0x02000ED5 RID: 3797
	// (Invoke) Token: 0x06007749 RID: 30537
	public delegate string GetTargetDisplayName(EChatType _chatType, string _targetId);

	// Token: 0x02000ED6 RID: 3798
	// (Invoke) Token: 0x0600774D RID: 30541
	public delegate void SendMessage(EChatType _chatType, string _targetId, string _message);

	// Token: 0x02000ED7 RID: 3799
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatMessagingHandler
	{
		// Token: 0x06007750 RID: 30544 RVA: 0x002CEA97 File Offset: 0x002CCC97
		public ChatMessagingHandler(XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
		{
			this.IsValidTargetDelegate = _isValidTargetDelegate;
			this.GetTargetDisplayNameDelegate = _getTargetDisplayNameDelegate;
			this.SendMessageDelegate = _sendMessageDelegate;
		}

		// Token: 0x040057F2 RID: 22514
		public readonly XUiC_Chat.IsValidTarget IsValidTargetDelegate;

		// Token: 0x040057F3 RID: 22515
		public readonly XUiC_Chat.GetTargetDisplayName GetTargetDisplayNameDelegate;

		// Token: 0x040057F4 RID: 22516
		public readonly XUiC_Chat.SendMessage SendMessageDelegate;
	}
}
