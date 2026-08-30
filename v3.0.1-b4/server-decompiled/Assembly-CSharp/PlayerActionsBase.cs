using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using InControl;

// Token: 0x02000558 RID: 1368
public abstract class PlayerActionsBase : PlayerActionSet
{
	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06002C8B RID: 11403 RVA: 0x00119E65 File Offset: 0x00118065
	// (set) Token: 0x06002C8A RID: 11402 RVA: 0x00119E5C File Offset: 0x0011805C
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x00119E70 File Offset: 0x00118070
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

	// Token: 0x06002C8D RID: 11405 RVA: 0x00119F9C File Offset: 0x0011819C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitActionSet()
	{
		this.CreateActions();
		this.CreateDefaultKeyboardBindings();
		this.CreateDefaultJoystickBindings();
	}

	// Token: 0x06002C8E RID: 11406
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateActions();

	// Token: 0x06002C8F RID: 11407
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultKeyboardBindings();

	// Token: 0x06002C90 RID: 11408
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CreateDefaultJoystickBindings();

	// Token: 0x06002C91 RID: 11409 RVA: 0x00119FB0 File Offset: 0x001181B0
	public void ResetControllerBindings()
	{
		this.AsyncResetControllerBindings();
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x00119FB8 File Offset: 0x001181B8
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

	// Token: 0x04002245 RID: 8773
	public List<PlayerAction> ControllerRebindableActions = new List<PlayerAction>();
}
