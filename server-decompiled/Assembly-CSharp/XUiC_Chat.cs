using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F01 RID: 3841
[Preserve]
public class XUiC_Chat : XUiController
{
	// Token: 0x060077EB RID: 30699 RVA: 0x002D5EA0 File Offset: 0x002D40A0
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

	// Token: 0x060077EC RID: 30700 RVA: 0x002D5F1C File Offset: 0x002D411C
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

	// Token: 0x060077ED RID: 30701 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x060077EE RID: 30702 RVA: 0x002D6024 File Offset: 0x002D4224
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

	// Token: 0x060077EF RID: 30703 RVA: 0x002D6090 File Offset: 0x002D4290
	public override void OnOpen()
	{
		this.cbxTarget.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.txtInput.Enabled = PermissionsManager.IsCommunicationAllowed();
		this.updateTargets();
		base.OnOpen();
	}

	// Token: 0x060077F0 RID: 30704 RVA: 0x002D60C0 File Offset: 0x002D42C0
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

	// Token: 0x060077F1 RID: 30705 RVA: 0x002D6308 File Offset: 0x002D4508
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

	// Token: 0x060077F2 RID: 30706 RVA: 0x002D639C File Offset: 0x002D459C
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

	// Token: 0x060077F3 RID: 30707 RVA: 0x002D63CF File Offset: 0x002D45CF
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x060077F4 RID: 30708 RVA: 0x002D63E0 File Offset: 0x002D45E0
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

	// Token: 0x060077F5 RID: 30709 RVA: 0x002D6497 File Offset: 0x002D4697
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "keep_whisper_targets_for_minutes")
		{
			this.keepWhisperTargetsForMinutes = StringParsers.ParseUInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060077F6 RID: 30710 RVA: 0x002D64C0 File Offset: 0x002D46C0
	public static void RegisterCustomMessagingHandler(EChatType _chatType, XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
	{
		if (_chatType >= (EChatType)XUiC_Chat.messagingHandlers.Length)
		{
			Log.Warning(string.Format("Can not register chat messaging handler for invalid chat type '{0}'", (int)_chatType));
			return;
		}
		XUiC_Chat.messagingHandlers[(int)_chatType] = new XUiC_Chat.ChatMessagingHandler(_isValidTargetDelegate, _getTargetDisplayNameDelegate, _sendMessageDelegate);
	}

	// Token: 0x060077F7 RID: 30711 RVA: 0x002D6500 File Offset: 0x002D4700
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

	// Token: 0x060077F8 RID: 30712 RVA: 0x002D656C File Offset: 0x002D476C
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

	// Token: 0x060077F9 RID: 30713 RVA: 0x002D65EC File Offset: 0x002D47EC
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

	// Token: 0x060077FE RID: 30718 RVA: 0x002D66C8 File Offset: 0x002D48C8
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

	// Token: 0x060077FF RID: 30719 RVA: 0x002D6778 File Offset: 0x002D4978
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

	// Token: 0x06007800 RID: 30720 RVA: 0x002D6810 File Offset: 0x002D4A10
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

	// Token: 0x040058E6 RID: 22758
	public static string ID = "";

	// Token: 0x040058E7 RID: 22759
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_Chat.ChatTarget> cbxTarget;

	// Token: 0x040058E8 RID: 22760
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040058E9 RID: 22761
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsFriends = new List<int>();

	// Token: 0x040058EA RID: 22762
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityIdsParty = new List<int>();

	// Token: 0x040058EB RID: 22763
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_Chat.ChatTarget> allTargets = new List<XUiC_Chat.ChatTarget>();

	// Token: 0x040058EC RID: 22764
	[PublicizedFrom(EAccessModifier.Private)]
	public uint keepWhisperTargetsForMinutes = 15U;

	// Token: 0x040058ED RID: 22765
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly XUiC_Chat.ChatMessagingHandler[] messagingHandlers = new XUiC_Chat.ChatMessagingHandler[(int)(EnumUtils.MaxValue<EChatType>() + 1)];

	// Token: 0x02000F02 RID: 3842
	[UsedImplicitly]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatTarget : IComparable<XUiC_Chat.ChatTarget>
	{
		// Token: 0x06007801 RID: 30721 RVA: 0x002D687C File Offset: 0x002D4A7C
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

		// Token: 0x06007802 RID: 30722 RVA: 0x002D68FA File Offset: 0x002D4AFA
		public override string ToString()
		{
			return this.displayText;
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x002D6902 File Offset: 0x002D4B02
		public void Send(string _message)
		{
			this.messageHandler.SendMessageDelegate(this.ChatType, this.TargetId, _message);
			this.lastUsed = DateTime.Now;
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06007804 RID: 30724 RVA: 0x002D692C File Offset: 0x002D4B2C
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastUsed;
			}
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x002D693E File Offset: 0x002D4B3E
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

		// Token: 0x06007806 RID: 30726 RVA: 0x002D6974 File Offset: 0x002D4B74
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

		// Token: 0x040058EE RID: 22766
		public readonly EChatType ChatType;

		// Token: 0x040058EF RID: 22767
		public readonly string TargetId;

		// Token: 0x040058F0 RID: 22768
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Chat.ChatMessagingHandler messageHandler;

		// Token: 0x040058F1 RID: 22769
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayText;

		// Token: 0x040058F2 RID: 22770
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastUsed;

		// Token: 0x040058F3 RID: 22771
		public readonly bool KeepForever;

		// Token: 0x040058F4 RID: 22772
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<XUiC_Chat.ChatTarget, XUi, bool> ValidCondition;
	}

	// Token: 0x02000F03 RID: 3843
	// (Invoke) Token: 0x06007808 RID: 30728
	public delegate bool IsValidTarget(EChatType _chatType, string _targetId);

	// Token: 0x02000F04 RID: 3844
	// (Invoke) Token: 0x0600780C RID: 30732
	public delegate string GetTargetDisplayName(EChatType _chatType, string _targetId);

	// Token: 0x02000F05 RID: 3845
	// (Invoke) Token: 0x06007810 RID: 30736
	public delegate void SendMessage(EChatType _chatType, string _targetId, string _message);

	// Token: 0x02000F06 RID: 3846
	[PublicizedFrom(EAccessModifier.Private)]
	public class ChatMessagingHandler
	{
		// Token: 0x06007813 RID: 30739 RVA: 0x002D69E7 File Offset: 0x002D4BE7
		public ChatMessagingHandler(XUiC_Chat.IsValidTarget _isValidTargetDelegate, XUiC_Chat.GetTargetDisplayName _getTargetDisplayNameDelegate, XUiC_Chat.SendMessage _sendMessageDelegate)
		{
			this.IsValidTargetDelegate = _isValidTargetDelegate;
			this.GetTargetDisplayNameDelegate = _getTargetDisplayNameDelegate;
			this.SendMessageDelegate = _sendMessageDelegate;
		}

		// Token: 0x040058F5 RID: 22773
		public readonly XUiC_Chat.IsValidTarget IsValidTargetDelegate;

		// Token: 0x040058F6 RID: 22774
		public readonly XUiC_Chat.GetTargetDisplayName GetTargetDisplayNameDelegate;

		// Token: 0x040058F7 RID: 22775
		public readonly XUiC_Chat.SendMessage SendMessageDelegate;
	}
}
