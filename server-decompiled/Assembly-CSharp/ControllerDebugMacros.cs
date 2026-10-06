using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x020011A9 RID: 4521
public class ControllerDebugMacros : MonoBehaviour
{
	// Token: 0x06009062 RID: 36962 RVA: 0x003657DB File Offset: 0x003639DB
	public ControllerDebugMacros()
	{
		this.m_debugMacros = new SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>>();
	}

	// Token: 0x06009063 RID: 36963 RVA: 0x003657F0 File Offset: 0x003639F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_debugLabel = base.GetComponent<ControllerDebugLabel>();
		this.m_debugLabel.AddDebugProvider("Debug Macros", new Action<StringBuilder>(this.BuildDebugMacroStatus));
		this.AddDebugMacro("Do Nothing", delegate
		{
		});
		this.AddDebugMacro("Open Console", new Action(this.MacroOpenConsole));
		this.AddDebugMacro("Toggle God Mode", new Action(this.MacroToggleGodMode));
		this.AddDebugMacro("Toggle Flying", new Action(this.MacroToggleFlying));
	}

	// Token: 0x06009064 RID: 36964 RVA: 0x00365894 File Offset: 0x00363A94
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_debugLabel.RemoveDebugProvider("Debug Macros");
		this.RemoveDebugMacro("Open Console");
		this.RemoveDebugMacro("Toggle God Mode");
		this.RemoveDebugMacro("Toggle Flying");
	}

	// Token: 0x06009065 RID: 36965 RVA: 0x003658C8 File Offset: 0x00363AC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_currentMacro == null || !this.m_debugMacros.ContainsKey(this.m_currentMacro))
		{
			if (this.m_lastIndex >= 0 && this.m_lastIndex < this.m_debugMacros.Keys.Count)
			{
				this.m_currentMacro = this.m_debugMacros.Keys.Skip<string>(this.m_lastIndex).FirstOrDefault<string>();
			}
			else
			{
				this.m_currentMacro = this.m_debugMacros.Keys.LastOrDefault<string>();
				this.m_lastIndex = this.m_debugMacros.Keys.Count - 1;
			}
		}
		if (this.m_currentMacro == null)
		{
			return;
		}
		ControllerDebugMacros.DebugDirection? executeMacroKeybind = this.GetExecuteMacroKeybind();
		if (executeMacroKeybind != null)
		{
			ControllerDebugMacros.DebugDirection valueOrDefault = executeMacroKeybind.GetValueOrDefault();
			if (this.m_keybindPressed)
			{
				return;
			}
			this.m_keybindPressed = true;
			if (this.m_debugMacros.ContainsKey(this.m_currentMacro))
			{
				this.m_debugMacros[this.m_currentMacro](valueOrDefault);
				return;
			}
		}
		else
		{
			if (this.HasNextMacroKeybind())
			{
				if (this.m_keybindPressed)
				{
					return;
				}
				this.m_keybindPressed = true;
				bool flag = false;
				int num = 0;
				using (SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>>.KeyCollection.Enumerator enumerator = this.m_debugMacros.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (flag)
						{
							this.m_currentMacro = text;
							this.m_lastIndex = num;
							break;
						}
						if (text == this.m_currentMacro)
						{
							flag = true;
						}
						num++;
					}
					return;
				}
			}
			if (this.HasPreviousMacroKeybind())
			{
				if (this.m_keybindPressed)
				{
					return;
				}
				this.m_keybindPressed = true;
				string text2 = null;
				int num2 = 0;
				using (SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>>.KeyCollection.Enumerator enumerator = this.m_debugMacros.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text3 = enumerator.Current;
						if (text3 == this.m_currentMacro)
						{
							if (text2 != null)
							{
								this.m_currentMacro = text2;
								this.m_lastIndex = num2 - 1;
								break;
							}
							break;
						}
						else
						{
							text2 = text3;
							num2++;
						}
					}
					return;
				}
			}
			this.m_keybindPressed = false;
		}
	}

	// Token: 0x06009066 RID: 36966 RVA: 0x00365AEC File Offset: 0x00363CEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildDebugMacroStatus(StringBuilder builder)
	{
		foreach (string text in this.m_debugMacros.Keys)
		{
			if (builder.Length > 0)
			{
				builder.Append(' ');
			}
			bool flag = text == this.m_currentMacro;
			if (flag)
			{
				builder.Append('[');
			}
			builder.Append(text);
			if (flag)
			{
				builder.Append(']');
			}
		}
	}

	// Token: 0x06009067 RID: 36967 RVA: 0x00365B7C File Offset: 0x00363D7C
	public void AddDebugMacro(string macroName, Action macro)
	{
		this.m_debugMacros[macroName] = delegate(ControllerDebugMacros.DebugDirection _)
		{
			macro();
		};
	}

	// Token: 0x06009068 RID: 36968 RVA: 0x00365BAE File Offset: 0x00363DAE
	public void AddDebugMacro(string macroName, Action<ControllerDebugMacros.DebugDirection> macro)
	{
		this.m_debugMacros[macroName] = macro;
	}

	// Token: 0x06009069 RID: 36969 RVA: 0x00365BBD File Offset: 0x00363DBD
	public void RemoveDebugMacro(string macroName)
	{
		this.m_debugMacros.Remove(macroName);
	}

	// Token: 0x0600906A RID: 36970 RVA: 0x00365BCC File Offset: 0x00363DCC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasPreviousMacroKeybind()
	{
		return ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0() || ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasControllerKeybind|14_1();
	}

	// Token: 0x0600906B RID: 36971 RVA: 0x00365BDC File Offset: 0x00363DDC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasNextMacroKeybind()
	{
		return ControllerDebugMacros.<HasNextMacroKeybind>g__HasKeyboardKeybind|15_0() || ControllerDebugMacros.<HasNextMacroKeybind>g__HasControllerKeybind|15_1();
	}

	// Token: 0x0600906C RID: 36972 RVA: 0x00365BEC File Offset: 0x00363DEC
	[PublicizedFrom(EAccessModifier.Private)]
	public ControllerDebugMacros.DebugDirection? GetExecuteMacroKeybind()
	{
		ControllerDebugMacros.DebugDirection? debugDirection = ControllerDebugMacros.<GetExecuteMacroKeybind>g__GetKeyboardKeybind|16_0();
		if (debugDirection == null)
		{
			return ControllerDebugMacros.<GetExecuteMacroKeybind>g__GetControllerKeybind|16_1();
		}
		return debugDirection;
	}

	// Token: 0x0600906D RID: 36973 RVA: 0x00365C10 File Offset: 0x00363E10
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroOpenConsole()
	{
		global::UnityEngine.Object @object = global::UnityEngine.Object.FindAnyObjectByType<GUIWindowManager>();
		GameManager instance = GameManager.Instance;
		if (!@object || !instance)
		{
			return;
		}
		GUIWindowConsole.Open();
	}

	// Token: 0x0600906E RID: 36974 RVA: 0x00012E78 File Offset: 0x00011078
	[PublicizedFrom(EAccessModifier.Private)]
	public static EntityPlayerLocal GetPrimaryPlayer()
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return null;
		}
		return world.GetPrimaryPlayer();
	}

	// Token: 0x0600906F RID: 36975 RVA: 0x00365C40 File Offset: 0x00363E40
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroToggleGodMode()
	{
		EntityPlayerLocal primaryPlayer = ControllerDebugMacros.GetPrimaryPlayer();
		if (!primaryPlayer)
		{
			return;
		}
		DataItem<bool> isGodMode = primaryPlayer.IsGodMode;
		if (isGodMode.Value)
		{
			isGodMode.Value = false;
			primaryPlayer.Buffs.RemoveBuff("god", -1, true);
		}
		else
		{
			isGodMode.Value = true;
			primaryPlayer.Buffs.AddBuff("god", -1, true, false, -1f);
		}
		primaryPlayer.bEntityAliveFlagsChanged = true;
	}

	// Token: 0x06009070 RID: 36976 RVA: 0x00365CB0 File Offset: 0x00363EB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroToggleFlying()
	{
		EntityPlayerLocal primaryPlayer = ControllerDebugMacros.GetPrimaryPlayer();
		if (!primaryPlayer)
		{
			return;
		}
		DataItem<bool> isFlyMode = primaryPlayer.IsFlyMode;
		isFlyMode.Value = !isFlyMode.Value;
		primaryPlayer.bEntityAliveFlagsChanged = true;
	}

	// Token: 0x06009071 RID: 36977 RVA: 0x00365CE7 File Offset: 0x00363EE7
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.LeftArrow);
	}

	// Token: 0x06009072 RID: 36978 RVA: 0x00365D14 File Offset: 0x00363F14
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasControllerKeybind|14_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadLeft.IsPressed;
	}

	// Token: 0x06009073 RID: 36979 RVA: 0x00365D64 File Offset: 0x00363F64
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasKeyboardKeybind|15_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.RightArrow);
	}

	// Token: 0x06009074 RID: 36980 RVA: 0x00365D90 File Offset: 0x00363F90
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasControllerKeybind|15_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadRight.IsPressed;
	}

	// Token: 0x06009075 RID: 36981 RVA: 0x00365DE0 File Offset: 0x00363FE0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static ControllerDebugMacros.DebugDirection? <GetExecuteMacroKeybind>g__GetKeyboardKeybind|16_0()
	{
		if (!InputUtils.ControlKeyPressed)
		{
			return null;
		}
		if (!InputUtils.ShiftKeyPressed)
		{
			return null;
		}
		if (!InputUtils.AltKeyPressed)
		{
			return null;
		}
		if (Input.GetKey(KeyCode.Menu))
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Neutral);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Down);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Up);
		}
		return null;
	}

	// Token: 0x06009076 RID: 36982 RVA: 0x00365E64 File Offset: 0x00364064
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static ControllerDebugMacros.DebugDirection? <GetExecuteMacroKeybind>g__GetControllerKeybind|16_1()
	{
		if (!InputManager.Enabled)
		{
			return null;
		}
		if (!InputManager.ActiveDevice.LeftBumper.IsPressed)
		{
			return null;
		}
		if (!InputManager.ActiveDevice.RightBumper.IsPressed)
		{
			return null;
		}
		if (InputManager.ActiveDevice.Action1.IsPressed)
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Neutral);
		}
		if (InputManager.ActiveDevice.DPadDown.IsPressed)
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Down);
		}
		if (InputManager.ActiveDevice.DPadUp.IsPressed)
		{
			return new ControllerDebugMacros.DebugDirection?(ControllerDebugMacros.DebugDirection.Up);
		}
		return null;
	}

	// Token: 0x04006AC2 RID: 27330
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>> m_debugMacros;

	// Token: 0x04006AC3 RID: 27331
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string m_currentMacro;

	// Token: 0x04006AC4 RID: 27332
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_lastIndex;

	// Token: 0x04006AC5 RID: 27333
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_keybindPressed;

	// Token: 0x04006AC6 RID: 27334
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ControllerDebugLabel m_debugLabel;

	// Token: 0x020011AA RID: 4522
	public enum DebugDirection
	{
		// Token: 0x04006AC8 RID: 27336
		Up,
		// Token: 0x04006AC9 RID: 27337
		Down,
		// Token: 0x04006ACA RID: 27338
		Neutral
	}
}
