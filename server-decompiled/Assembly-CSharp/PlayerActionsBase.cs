using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using InControl;

// Token: 0x02000567 RID: 1383
public abstract class PlayerActionsBase : PlayerActionSet
{
	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x0011C235 File Offset: 0x0011A435
	// (set) Token: 0x06002CB2 RID: 11442 RVA: 0x0011C22C File Offset: 0x0011A42C
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x0011C240 File Offset: 0x0011A440
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

	// Token: 0x06002CB5 RID: 11445 RVA: 0x0011C36C File Offset: 0x0011A56C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitActionSet()
	{
		this.CreateActions();
		this.CreateDefaultKeyboardBindings();
		this.CreateDefaultJoystickBindings();
	}

	// Token: 0x06002CB6 RID: 11446
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateActions();

	// Token: 0x06002CB7 RID: 11447
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultKeyboardBindings();

	// Token: 0x06002CB8 RID: 11448
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultJoystickBindings();

	// Token: 0x06002CB9 RID: 11449 RVA: 0x0011C380 File Offset: 0x0011A580
	public void ResetControllerBindings()
	{
		this.AsyncResetControllerBindings();
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0011C388 File Offset: 0x0011A588
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

	// Token: 0x04002296 RID: 8854
	public List<PlayerAction> ControllerRebindableActions = new List<PlayerAction>();
}
