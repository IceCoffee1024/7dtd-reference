using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200055D RID: 1373
[global::UnityEngine.Scripting.Preserve]
public class ActionSetManager
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002C82 RID: 11394 RVA: 0x0011AE74 File Offset: 0x00119074
	public bool Empty
	{
		get
		{
			return this.PlayerActions.Count <= 0;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C83 RID: 11395 RVA: 0x0011AE87 File Offset: 0x00119087
	public PlayerActionSet Top
	{
		get
		{
			return this.PlayerActions[this.PlayerActions.Count - 1];
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C84 RID: 11396 RVA: 0x0011AEA1 File Offset: 0x001190A1
	public static ActionSetManager.EDebugLevel DebugLevel
	{
		get
		{
			return ActionSetManager.debug;
		}
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x0011AEA8 File Offset: 0x001190A8
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

	// Token: 0x06002C86 RID: 11398 RVA: 0x0011AEE4 File Offset: 0x001190E4
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

	// Token: 0x06002C87 RID: 11399 RVA: 0x0011AF8C File Offset: 0x0011918C
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

	// Token: 0x06002C88 RID: 11400 RVA: 0x0011B096 File Offset: 0x00119296
	public void Push(PlayerActionSet _playerAction)
	{
		if (_playerAction == null)
		{
			Log.Warning("LocalPlayerInput::Push - Pushing a null input onto stack.");
		}
		this.PushInternal(_playerAction, null);
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x0011B0AD File Offset: 0x001192AD
	public void Push(GUIWindow _window)
	{
		if (((_window != null) ? _window.GetActionSet() : null) == null)
		{
			Log.Warning("LocalPlayerInput::Push - Pushing a null input onto stack.");
		}
		this.PushInternal((_window != null) ? _window.GetActionSet() : null, (_window != null) ? _window.Id : null);
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x0011B0E8 File Offset: 0x001192E8
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

	// Token: 0x06002C8B RID: 11403 RVA: 0x0011B178 File Offset: 0x00119378
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

	// Token: 0x06002C8C RID: 11404 RVA: 0x0011B258 File Offset: 0x00119458
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

	// Token: 0x06002C8D RID: 11405 RVA: 0x0011B3BD File Offset: 0x001195BD
	public void Reset()
	{
		while (!this.Empty)
		{
			this.Pop(null);
		}
	}

	// Token: 0x04002263 RID: 8803
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PlayerActionSet> PlayerActions = new List<PlayerActionSet>();

	// Token: 0x04002264 RID: 8804
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ActionSetManager.EDebugLevel debug = ActionSetManager.EDebugLevel.Off;

	// Token: 0x0200055E RID: 1374
	public enum EDebugLevel
	{
		// Token: 0x04002266 RID: 8806
		Off,
		// Token: 0x04002267 RID: 8807
		Normal,
		// Token: 0x04002268 RID: 8808
		Verbose
	}
}
