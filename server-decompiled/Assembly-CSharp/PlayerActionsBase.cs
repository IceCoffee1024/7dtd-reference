using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using InControl;

// Token: 0x02000563 RID: 1379
public abstract class PlayerActionsBase : PlayerActionSet
{
	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002CAB RID: 11435 RVA: 0x0011BCA9 File Offset: 0x00119EA9
	// (set) Token: 0x06002CAA RID: 11434 RVA: 0x0011BCA0 File Offset: 0x00119EA0
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x0011BCB4 File Offset: 0x00119EB4
	public PlayerActionsBase()
	{
		FieldInfo field = typeof(PlayerActionSet).GetField("actionsByName", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			field.SetValue(this, new CaseInsensitiveStringDictionary<PlayerAction>());
		}
		base.ListenOptions = new BindingListenOptions
		{
			UnsetDuplicateBindingsOnSet = false,
			MaxAllowedBindings = 0U,
			MaxAllowedBindingsPerType = 1U,
			AllowDuplicateBindingsPerSet = true,
			IncludeKeys = false,
			IncludeMouseButtons = false,
			IncludeControllers = false,
			IncludeModifiersAsFirstClassKeys = true
		};
		base.ListenOptions.OnBindingFound = delegate(PlayerAction _action, BindingSource _binding)
		{
			if (!_action.HasBinding(_binding))
			{
				return true;
			}
			Log.Out("Binding already bound.");
			_action.StopListeningForBinding();
			return false;
		};
		BindingListenOptions listenOptions = base.ListenOptions;
		listenOptions.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(listenOptions.OnBindingAdded, new Action<PlayerAction, BindingSource>(delegate(PlayerAction _action, BindingSource _binding)
		{
			Log.Out("Binding added for action {0} on device {1}: {2}", new object[] { _action.Name, _binding.DeviceName, _binding.Name });
		}));
		BindingListenOptions listenOptions2 = base.ListenOptions;
		listenOptions2.OnBindingRejected = (Action<PlayerAction, BindingSource, BindingSourceRejectionType>)Delegate.Combine(listenOptions2.OnBindingRejected, new Action<PlayerAction, BindingSource, BindingSourceRejectionType>(delegate(PlayerAction _action, BindingSource _binding, BindingSourceRejectionType _reason)
		{
			Log.Out("Binding rejected for action {0}: {1}", new object[]
			{
				_action.Name,
				_reason.ToStringCached<BindingSourceRejectionType>()
			});
		}));
		this.InitActionSet();
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x0011BDE0 File Offset: 0x00119FE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitActionSet()
	{
		this.CreateActions();
		this.CreateDefaultKeyboardBindings();
		this.CreateDefaultJoystickBindings();
	}

	// Token: 0x06002CAE RID: 11438
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateActions();

	// Token: 0x06002CAF RID: 11439
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultKeyboardBindings();

	// Token: 0x06002CB0 RID: 11440
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultJoystickBindings();

	// Token: 0x06002CB1 RID: 11441 RVA: 0x0011BDF4 File Offset: 0x00119FF4
	public void ResetControllerBindings()
	{
		this.AsyncResetControllerBindings();
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x0011BDFC File Offset: 0x00119FFC
	[PublicizedFrom(EAccessModifier.Private)]
	public async void AsyncResetControllerBindings()
	{
		foreach (PlayerAction playerAction in base.Actions)
		{
			PlayerActionData.ActionUserData actionUserData = playerAction.UserData as PlayerActionData.ActionUserData;
			if (actionUserData != null && actionUserData.defaultOnStartup)
			{
				foreach (BindingSource bindingSource in playerAction.Bindings)
				{
					if (bindingSource.BindingSourceType == BindingSourceType.DeviceBindingSource)
					{
						playerAction.RemoveBinding(bindingSource);
					}
				}
			}
		}
		await Task.Yield();
		this.CreateDefaultJoystickBindings();
	}

	// Token: 0x0400227F RID: 8831
	public List<PlayerAction> ControllerRebindableActions = new List<PlayerAction>();
}
