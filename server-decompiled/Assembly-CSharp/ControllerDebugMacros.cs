using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x02001191 RID: 4497
public class ControllerDebugMacros : MonoBehaviour
{
	// Token: 0x06008FD6 RID: 36822 RVA: 0x0036100F File Offset: 0x0035F20F
	public ControllerDebugMacros()
	{
		this.m_debugMacros = new SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>>();
	}

	// Token: 0x06008FD7 RID: 36823 RVA: 0x00361024 File Offset: 0x0035F224
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

	// Token: 0x06008FD8 RID: 36824 RVA: 0x003610C8 File Offset: 0x0035F2C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.m_debugLabel.RemoveDebugProvider("Debug Macros");
		this.RemoveDebugMacro("Open Console");
		this.RemoveDebugMacro("Toggle God Mode");
		this.RemoveDebugMacro("Toggle Flying");
	}

	// Token: 0x06008FD9 RID: 36825 RVA: 0x003610FC File Offset: 0x0035F2FC
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

	// Token: 0x06008FDA RID: 36826 RVA: 0x00361320 File Offset: 0x0035F520
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

	// Token: 0x06008FDB RID: 36827 RVA: 0x003613B0 File Offset: 0x0035F5B0
	public void AddDebugMacro(string macroName, Action macro)
	{
		this.m_debugMacros[macroName] = delegate(ControllerDebugMacros.DebugDirection _)
		{
			macro();
		};
	}

	// Token: 0x06008FDC RID: 36828 RVA: 0x003613E2 File Offset: 0x0035F5E2
	public void AddDebugMacro(string macroName, Action<ControllerDebugMacros.DebugDirection> macro)
	{
		this.m_debugMacros[macroName] = macro;
	}

	// Token: 0x06008FDD RID: 36829 RVA: 0x003613F1 File Offset: 0x0035F5F1
	public void RemoveDebugMacro(string macroName)
	{
		this.m_debugMacros.Remove(macroName);
	}

	// Token: 0x06008FDE RID: 36830 RVA: 0x00361400 File Offset: 0x0035F600
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasPreviousMacroKeybind()
	{
		return ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0() || ControllerDebugMacros.<HasPreviousMacroKeybind>g__HasControllerKeybind|14_1();
	}

	// Token: 0x06008FDF RID: 36831 RVA: 0x00361410 File Offset: 0x0035F610
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HasNextMacroKeybind()
	{
		return ControllerDebugMacros.<HasNextMacroKeybind>g__HasKeyboardKeybind|15_0() || ControllerDebugMacros.<HasNextMacroKeybind>g__HasControllerKeybind|15_1();
	}

	// Token: 0x06008FE0 RID: 36832 RVA: 0x00361420 File Offset: 0x0035F620
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

	// Token: 0x06008FE1 RID: 36833 RVA: 0x00361444 File Offset: 0x0035F644
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

	// Token: 0x06008FE2 RID: 36834 RVA: 0x00012E78 File Offset: 0x00011078
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

	// Token: 0x06008FE3 RID: 36835 RVA: 0x00361474 File Offset: 0x0035F674
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

	// Token: 0x06008FE4 RID: 36836 RVA: 0x003614E4 File Offset: 0x0035F6E4
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

	// Token: 0x06008FE5 RID: 36837 RVA: 0x0036151B File Offset: 0x0035F71B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasKeyboardKeybind|14_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.LeftArrow);
	}

	// Token: 0x06008FE6 RID: 36838 RVA: 0x00361548 File Offset: 0x0035F748
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasPreviousMacroKeybind>g__HasControllerKeybind|14_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadLeft.IsPressed;
	}

	// Token: 0x06008FE7 RID: 36839 RVA: 0x00361598 File Offset: 0x0035F798
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasKeyboardKeybind|15_0()
	{
		return InputUtils.ControlKeyPressed && InputUtils.ShiftKeyPressed && InputUtils.AltKeyPressed && Input.GetKey(KeyCode.RightArrow);
	}

	// Token: 0x06008FE8 RID: 36840 RVA: 0x003615C4 File Offset: 0x0035F7C4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <HasNextMacroKeybind>g__HasControllerKeybind|15_1()
	{
		return InputManager.Enabled && InputManager.ActiveDevice.LeftBumper.IsPressed && InputManager.ActiveDevice.RightBumper.IsPressed && InputManager.ActiveDevice.DPadRight.IsPressed;
	}

	// Token: 0x06008FE9 RID: 36841 RVA: 0x00361614 File Offset: 0x0035F814
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

	// Token: 0x06008FEA RID: 36842 RVA: 0x00361698 File Offset: 0x0035F898
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

	// Token: 0x04006A5C RID: 27228
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<ControllerDebugMacros.DebugDirection>> m_debugMacros;

	// Token: 0x04006A5D RID: 27229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string m_currentMacro;

	// Token: 0x04006A5E RID: 27230
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_lastIndex;

	// Token: 0x04006A5F RID: 27231
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_keybindPressed;

	// Token: 0x04006A60 RID: 27232
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ControllerDebugLabel m_debugLabel;

	// Token: 0x02001192 RID: 4498
	public enum DebugDirection
	{
		// Token: 0x04006A62 RID: 27234
		Up,
		// Token: 0x04006A63 RID: 27235
		Down,
		// Token: 0x04006A64 RID: 27236
		Neutral
	}
}
