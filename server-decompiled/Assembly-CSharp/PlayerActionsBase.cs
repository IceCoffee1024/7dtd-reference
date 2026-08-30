using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using InControl;

// Token: 0x0200055D RID: 1373
public abstract class PlayerActionsBase : PlayerActionSet
{
	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x0011AED9 File Offset: 0x001190D9
	// (set) Token: 0x06002CB4 RID: 11444 RVA: 0x0011AED0 File Offset: 0x001190D0
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x0011AEE4 File Offset: 0x001190E4
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

	// Token: 0x06002CB7 RID: 11447 RVA: 0x0011B010 File Offset: 0x00119210
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitActionSet()
	{
		this.CreateActions();
		this.CreateDefaultKeyboardBindings();
		this.CreateDefaultJoystickBindings();
	}

	// Token: 0x06002CB8 RID: 11448
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateActions();

	// Token: 0x06002CB9 RID: 11449
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultKeyboardBindings();

	// Token: 0x06002CBA RID: 11450
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultJoystickBindings();

	// Token: 0x06002CBB RID: 11451 RVA: 0x0011B024 File Offset: 0x00119224
	public void ResetControllerBindings()
	{
		this.AsyncResetControllerBindings();
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x0011B02C File Offset: 0x0011922C
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

	// Token: 0x04002277 RID: 8823
	public List<PlayerAction> ControllerRebindableActions = new List<PlayerAction>();
}
