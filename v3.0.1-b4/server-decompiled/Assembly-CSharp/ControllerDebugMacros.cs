using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x02001177 RID: 4471
public class ControllerDebugMacros : MonoBehaviour
{
	// Token: 0x06008EDE RID: 36574 RVA: 0x0035C573 File Offset: 0x0035A773
	public ControllerDebugMacros()
	{
		this.m_debugMacros = new SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>>();
	}

	// Token: 0x06008EDF RID: 36575 RVA: 0x0035C588 File Offset: 0x0035A788
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

	// Token: 0x06008EE0 RID: 36576 RVA: 0x0035C62C File Offset: 0x0035A82C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_debugLabel.RemoveDebugProvider("Debug Macros");
		this.RemoveDebugMacro("Open Console");
		this.RemoveDebugMacro("Toggle God Mode");
		this.RemoveDebugMacro("Toggle Flying");
	}

	// Token: 0x06008EE1 RID: 36577 RVA: 0x0035C660 File Offset: 0x0035A860
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

	// Token: 0x06008EE2 RID: 36578 RVA: 0x0035C884 File Offset: 0x0035AA84
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

	// Token: 0x06008EE3 RID: 36579 RVA: 0x0035C914 File Offset: 0x0035AB14
	public void AddDebugMacro(string macroName, Action macro)
	{
		this.m_debugMacros[macroName] = delegate(ControllerDebugMacros.DebugDirection _)
		{
			macro();
		};
	}

	// Token: 0x06008EE4 RID: 36580 RVA: 0x0035C946 File Offset: 0x0035AB46
	public void AddDebugMacro(string macroName, Action<ControllerDebugMacros.DebugDirection> macro)
	{
		this.m_debugMacros[macroName] = macro;
	}

	// Token: 0x06008EE5 RID: 36581 RVA: 0x0035C955 File Offset: 0x0035AB55
	public void RemoveDebugMacro(string macroName)
	{
		this.m_debugMacros.Remove(macroName);
	}

	// Token: 0x06008EE6 RID: 36582 RVA: 0x0035C964 File Offset: 0x0035AB64
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasPreviousMacroKeybind()
	{
		return ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0() || ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasControllerKeybind|14_1();
	}

	// Token: 0x06008EE7 RID: 36583 RVA: 0x0035C974 File Offset: 0x0035AB74
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasNextMacroKeybind()
	{
		return ControllerDebugMacros.<HasNextMacroKeybind>g__HasKeyboardKeybind|15_0() || ControllerDebugMacros.<HasNextMacroKeybind>g__HasControllerKeybind|15_1();
	}

	// Token: 0x06008EE8 RID: 36584 RVA: 0x0035C984 File Offset: 0x0035AB84
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

	// Token: 0x06008EE9 RID: 36585 RVA: 0x0035C9A8 File Offset: 0x0035ABA8
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

	// Token: 0x06008EEA RID: 36586 RVA: 0x00012E78 File Offset: 0x00011078
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

	// Token: 0x06008EEB RID: 36587 RVA: 0x0035C9D8 File Offset: 0x0035ABD8
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

	// Token: 0x06008EEC RID: 36588 RVA: 0x0035CA48 File Offset: 0x0035AC48
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

	// Token: 0x06008EED RID: 36589 RVA: 0x0035CA7F File Offset: 0x0035AC7F
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.LeftArrow);
	}

	// Token: 0x06008EEE RID: 36590 RVA: 0x0035CAAC File Offset: 0x0035ACAC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasControllerKeybind|14_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadLeft.IsPressed;
	}

	// Token: 0x06008EEF RID: 36591 RVA: 0x0035CAFC File Offset: 0x0035ACFC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasKeyboardKeybind|15_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.RightArrow);
	}

	// Token: 0x06008EF0 RID: 36592 RVA: 0x0035CB28 File Offset: 0x0035AD28
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasControllerKeybind|15_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadRight.IsPressed;
	}

	// Token: 0x06008EF1 RID: 36593 RVA: 0x0035CB78 File Offset: 0x0035AD78
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

	// Token: 0x06008EF2 RID: 36594 RVA: 0x0035CBFC File Offset: 0x0035ADFC
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

	// Token: 0x0400698D RID: 27021
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>> m_debugMacros;

	// Token: 0x0400698E RID: 27022
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string m_currentMacro;

	// Token: 0x0400698F RID: 27023
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_lastIndex;

	// Token: 0x04006990 RID: 27024
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_keybindPressed;

	// Token: 0x04006991 RID: 27025
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ControllerDebugLabel m_debugLabel;

	// Token: 0x02001178 RID: 4472
	public enum DebugDirection
	{
		// Token: 0x04006993 RID: 27027
		Up,
		// Token: 0x04006994 RID: 27028
		Down,
		// Token: 0x04006995 RID: 27029
		Neutral
	}
}
