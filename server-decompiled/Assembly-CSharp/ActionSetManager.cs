using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000559 RID: 1369
[global::UnityEngine.Scripting.Preserve]
public class ActionSetManager
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002C7A RID: 11386 RVA: 0x0011A8E8 File Offset: 0x00118AE8
	public bool Empty
	{
		get
		{
			return this.PlayerActions.Count <= 0;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C7B RID: 11387 RVA: 0x0011A8FB File Offset: 0x00118AFB
	public PlayerActionSet Top
	{
		get
		{
			return this.PlayerActions[this.PlayerActions.Count - 1];
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C7C RID: 11388 RVA: 0x0011A915 File Offset: 0x00118B15
	public static ActionSetManager.EDebugLevel DebugLevel
	{
		get
		{
			return ActionSetManager.debug;
		}
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x0011A91C File Offset: 0x00118B1C
	[PublicizedFrom(EAccessModifier.Private)]
	static ActionSetManager()
	{
		string launchArgument = GameUtils.GetLaunchArgument("debuginput");
		if (launchArgument != null)
		{
			if (launchArgument == "verbose")
			{
				ActionSetManager.debug = ActionSetManager.EDebugLevel.Verbose;
				return;
			}
			ActionSetManager.debug = ActionSetManager.EDebugLevel.Normal;
		}
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x0011A958 File Offset: 0x00118B58
	public void Insert(PlayerActionSet _playerAction, int _index, string _windowName = null)
	{
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			Log.Out("LocalPlayerInput.Insert ({2} - {0}):{1}", new object[]
			{
				_playerAction.GetType().FullName,
				(ActionSetManager.debug == ActionSetManager.EDebugLevel.Verbose) ? ("\n" + StackTraceUtility.ExtractStackTrace()) : "",
				_windowName
			});
		}
		if (_playerAction == null)
		{
			Log.Warning("LocalPlayerInput::Insert - Inserting a null input onto stack.");
		}
		if (!this.Empty)
		{
			this.Top.Enabled = false;
		}
		_playerAction.Enabled = false;
		this.PlayerActions.Insert(_index, _playerAction);
		this.Top.Enabled = true;
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			this.LogActionSets();
		}
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x0011AA00 File Offset: 0x00118C00
	public void Remove(PlayerActionSet _playerAction, int _minIndex, string _windowName = null)
	{
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			Log.Out("LocalPlayerInput.Remove ({2} - {0}):{1}", new object[]
			{
				_playerAction.GetType().FullName,
				(ActionSetManager.debug == ActionSetManager.EDebugLevel.Verbose) ? ("\n" + StackTraceUtility.ExtractStackTrace()) : "",
				_windowName
			});
		}
		if (_playerAction == null)
		{
			Log.Warning("LocalPlayerInput::Remove - Trying to remove a null input from the stack.");
		}
		if (this.Empty)
		{
			Log.Warning("LocalPlayerInput::Remove - Removing input from an empty stack.");
			return;
		}
		this.Top.Enabled = false;
		_playerAction.Enabled = false;
		int num = -1;
		for (int i = this.PlayerActions.Count - 1; i >= _minIndex; i--)
		{
			if (this.PlayerActions[i] == _playerAction)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			this.PlayerActions.RemoveAt(num);
		}
		else
		{
			Log.Warning(string.Format("LocalPlayerInput::Remove - Failed to find action set of type '{0}' with a min index of {1} to remove.", _playerAction.GetType().FullName, _minIndex));
		}
		if (!this.Empty)
		{
			this.Top.Enabled = true;
		}
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			this.LogActionSets();
		}
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x0011AB0A File Offset: 0x00118D0A
	public void Push(PlayerActionSet _playerAction)
	{
		if (_playerAction == null)
		{
			Log.Warning("LocalPlayerInput::Push - Pushing a null input onto stack.");
		}
		this.PushInternal(_playerAction, null);
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x0011AB21 File Offset: 0x00118D21
	public void Push(GUIWindow _window)
	{
		if (((_window != null) ? _window.GetActionSet() : null) == null)
		{
			Log.Warning("LocalPlayerInput::Push - Pushing a null input onto stack.");
		}
		this.PushInternal((_window != null) ? _window.GetActionSet() : null, (_window != null) ? _window.Id : null);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x0011AB5C File Offset: 0x00118D5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PushInternal(PlayerActionSet _playerAction, string _windowName = null)
	{
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			Log.Out("LocalPlayerInput.Push ({2} - {0}):{1}", new object[]
			{
				_playerAction.GetType().FullName,
				(ActionSetManager.debug == ActionSetManager.EDebugLevel.Verbose) ? ("\n" + StackTraceUtility.ExtractStackTrace()) : "",
				_windowName
			});
		}
		if (!this.Empty)
		{
			this.Top.Enabled = false;
		}
		this.PlayerActions.Add(_playerAction);
		this.Top.Enabled = true;
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			this.LogActionSets();
		}
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x0011ABEC File Offset: 0x00118DEC
	public void Pop(GUIWindow _window = null)
	{
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			Log.Out("LocalPlayerInput.Pop ({1}):{0}", new object[]
			{
				(ActionSetManager.debug == ActionSetManager.EDebugLevel.Verbose) ? ("\n" + StackTraceUtility.ExtractStackTrace()) : "",
				(_window != null) ? _window.Id : null
			});
		}
		if (this.Empty)
		{
			Log.Warning("LocalPlayerInput::Pop - Popping input from an empty stack.");
			return;
		}
		int num = this.PlayerActions.Count - 1;
		if (_window != null)
		{
			PlayerActionsBase actionSet = _window.GetActionSet();
			if (actionSet != null && actionSet != this.PlayerActions[num])
			{
				Log.Warning("LocalPlayerInput::Pop - Tried to pop a different action set from what belongs to window " + _window.Id);
				return;
			}
		}
		this.Top.Enabled = false;
		this.PlayerActions.RemoveAt(num);
		if (!this.Empty)
		{
			this.Top.Enabled = true;
		}
		if (ActionSetManager.debug != ActionSetManager.EDebugLevel.Off)
		{
			this.LogActionSets();
		}
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x0011ACCC File Offset: 0x00118ECC
	public void LogActionSets()
	{
		string text = "";
		for (int i = 0; i < this.PlayerActions.Count; i++)
		{
			text = string.Concat(new string[]
			{
				text,
				this.PlayerActions[i].GetType().Name,
				" (",
				this.PlayerActions[i].Enabled.ToString(),
				"), "
			});
		}
		string text2 = "";
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		int? num;
		if (nativePlatform == null)
		{
			num = null;
		}
		else
		{
			PlayerInputManager input = nativePlatform.Input;
			if (input == null)
			{
				num = null;
			}
			else
			{
				ReadOnlyCollection<PlayerActionsBase> actionSets = input.ActionSets;
				num = ((actionSets != null) ? new int?(actionSets.Count) : null);
			}
		}
		int? num2 = num;
		if (num2.GetValueOrDefault() > 0)
		{
			for (int j = 0; j < PlatformManager.NativePlatform.Input.ActionSets.Count; j++)
			{
				text2 += string.Format("{0} ({1}), ", PlatformManager.NativePlatform.Input.ActionSets[j].GetType().Name, PlatformManager.NativePlatform.Input.ActionSets[j].Enabled);
			}
		}
		Log.Out("ActionSets: Stack: {0} --- All: {1}", new object[] { text, text2 });
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x0011AE31 File Offset: 0x00119031
	public void Reset()
	{
		while (!this.Empty)
		{
			this.Pop(null);
		}
	}

	// Token: 0x0400224C RID: 8780
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PlayerActionSet> PlayerActions = new List<PlayerActionSet>();

	// Token: 0x0400224D RID: 8781
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ActionSetManager.EDebugLevel debug = ActionSetManager.EDebugLevel.Off;

	// Token: 0x0200055A RID: 1370
	public enum EDebugLevel
	{
		// Token: 0x0400224F RID: 8783
		Off,
		// Token: 0x04002250 RID: 8784
		Normal,
		// Token: 0x04002251 RID: 8785
		Verbose
	}
}
