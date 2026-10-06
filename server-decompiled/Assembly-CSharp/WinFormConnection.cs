using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class WinFormConnection : Form, IConsoleConnection
{
	// Token: 0x060014D6 RID: 5334 RVA: 0x0007E374 File Offset: 0x0007C574
	public WinFormConnection(WinFormInstance _owner)
	{
		this.initialize();
		ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(this.OnGameStartDone));
		ModEvents.GameAwake.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameAwakeData>(this.OnGameAwake));
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x0007E406 File Offset: 0x0007C606
	public void CloseTerminal()
	{
		if (base.InvokeRequired)
		{
			base.BeginInvoke(new Action(this.CloseTerminal));
			return;
		}
		this.forceClose = true;
		base.Close();
		global::System.Windows.Forms.Application.Exit();
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x0007E438 File Offset: 0x0007C638
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnClosing(CancelEventArgs _e)
	{
		base.OnClosing(_e);
		if (this.forceClose)
		{
			return;
		}
		_e.Cancel = true;
		if (this.shutdownRequested)
		{
			return;
		}
		if (MessageBox.Show("Really shut down the 7 Days to Die server?", "Shutdown", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
		{
			return;
		}
		this.shutdownRequested = true;
		Log.Out("Shutdown game from Terminal Window");
		ThreadManager.AddSingleTaskMainThread("Shutdown", delegate(object _)
		{
			global::UnityEngine.Application.Quit();
		}, null);
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x0007E4B6 File Offset: 0x0007C6B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnClosed(EventArgs _e)
	{
		base.OnClosed(_e);
		ModEvents.GameStartDone.UnregisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(this.OnGameStartDone));
		ModEvents.GameAwake.UnregisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameAwakeData>(this.OnGameAwake));
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x0007E4EC File Offset: 0x0007C6EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void initialize()
	{
		base.SuspendLayout();
		base.ClientSize = new Size(1000, 600);
		this.Text = "Starting - 7 Days to Die Dedicated Server Console";
		base.Icon = new Icon(this.iconPath);
		this.BackColor = global::System.Drawing.Color.Black;
		this.ForeColor = WinFormConnection.logColorNormal;
		this.consoleOutputBox = new RichTextBox
		{
			Dock = DockStyle.Fill,
			Multiline = true,
			ScrollBars = RichTextBoxScrollBars.Both,
			Font = new global::System.Drawing.Font(FontFamily.GenericMonospace, 10f),
			ReadOnly = true,
			BackColor = this.BackColor,
			ForeColor = this.ForeColor,
			BorderStyle = BorderStyle.None
		};
		base.Controls.Add(this.consoleOutputBox);
		IntPtr handle = this.consoleOutputBox.Handle;
		this.commandInputBox = new TextBox
		{
			Dock = DockStyle.Bottom,
			Multiline = false,
			Text = "",
			Font = new global::System.Drawing.Font(FontFamily.GenericMonospace, 12f),
			Enabled = false,
			AutoCompleteMode = AutoCompleteMode.Append,
			AutoCompleteSource = AutoCompleteSource.CustomSource,
			BackColor = global::System.Drawing.Color.LightGray,
			ForeColor = global::System.Drawing.Color.Black,
			BorderStyle = BorderStyle.FixedSingle
		};
		this.commandInputBox.KeyDown += this.CommandInputBoxOnKeyDown;
		this.commandInputBox.AutoCompleteCustomSource = new AutoCompleteStringCollection();
		base.Controls.Add(this.commandInputBox);
		this.CreateHandle();
		base.ResumeLayout();
		base.PerformLayout();
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x0007E674 File Offset: 0x0007C874
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameAwake(ref ModEvents.SGameAwakeData _data)
	{
		this.Text = string.Format("{0} - Port {1} - Loading - 7 Days to Die Dedicated Server Console", GamePrefs.GetString(EnumGamePrefs.ServerName), GamePrefs.GetInt(EnumGamePrefs.ServerPort));
		foreach (IConsoleCommand consoleCommand in SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands())
		{
			this.commandInputBox.AutoCompleteCustomSource.AddRange(consoleCommand.GetCommands());
		}
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x0007E6F8 File Offset: 0x0007C8F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameStartDone(ref ModEvents.SGameStartDoneData _data)
	{
		this.Text = string.Format("{0} - Port {1} - Running - 7 Days to Die Dedicated Server Console", GamePrefs.GetString(EnumGamePrefs.ServerName), GamePrefs.GetInt(EnumGamePrefs.ServerPort));
		this.commandInputBox.Enabled = true;
		this.commandInputBox.Clear();
		this.commandInputBox.Focus();
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x0007E74B File Offset: 0x0007C94B
	[PublicizedFrom(EAccessModifier.Private)]
	public void CommandInputBoxOnKeyDown(object _sender, KeyEventArgs _keyEventArgs)
	{
		if (_keyEventArgs.KeyCode == Keys.Return)
		{
			this.execCommand();
		}
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x0007E760 File Offset: 0x0007C960
	[PublicizedFrom(EAccessModifier.Private)]
	public void execCommand()
	{
		if (this.commandInputBox.Enabled && this.commandInputBox.Text.Length > 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteAsync(this.commandInputBox.Text, this);
			this.commandInputBox.Clear();
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x0007E7B0 File Offset: 0x0007C9B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddLine(string _text, global::System.Drawing.Color _color)
	{
		if (this.consoleOutputBox.InvokeRequired)
		{
			if (this.addLineDelegate == null)
			{
				this.addLineDelegate = new Action<string, global::System.Drawing.Color>(this.AddLine);
			}
			base.BeginInvoke(this.addLineDelegate, new object[] { _text, _color });
			return;
		}
		this.consoleOutputBox.SelectionStart = this.consoleOutputBox.TextLength;
		this.consoleOutputBox.SelectionLength = 0;
		this.consoleOutputBox.SelectionColor = _color;
		this.consoleOutputBox.AppendText(_text + "\n");
		this.consoleOutputBox.SelectionColor = this.consoleOutputBox.ForeColor;
		if (this.consoleOutputBox.Lines.Length > 1000)
		{
			int num = this.consoleOutputBox.Lines.Length - 500;
			string[] array = new string[500];
			for (int i = 0; i < 500; i++)
			{
				array[i] = this.consoleOutputBox.Lines[i + num];
			}
			this.consoleOutputBox.Lines = array;
		}
		this.consoleOutputBox.SelectionStart = this.consoleOutputBox.TextLength;
		this.consoleOutputBox.ScrollToCaret();
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x0007E8E1 File Offset: 0x0007CAE1
	public void SendLine(string _line)
	{
		this.AddLine(_line, WinFormConnection.logColorCommandReply);
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0007E8F0 File Offset: 0x0007CAF0
	public void SendLines(List<string> _output)
	{
		foreach (string text in _output)
		{
			this.SendLine(text);
		}
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x0007E940 File Offset: 0x0007CB40
	public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (this.IsLogLevelEnabled(_type))
		{
			global::System.Drawing.Color color = WinFormConnection.logColorNormal;
			switch (_type)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				color = WinFormConnection.logColorError;
				break;
			case LogType.Warning:
				color = WinFormConnection.logColorWarning;
				break;
			}
			this.AddLine(_formattedMessage, color);
		}
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x0007E990 File Offset: 0x0007CB90
	public void EnableLogLevel(LogType _type, bool _enable)
	{
		if (_enable)
		{
			this.enabledLogLevels.Add(_type);
			return;
		}
		this.enabledLogLevels.Remove(_type);
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x0007E9B0 File Offset: 0x0007CBB0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsLogLevelEnabled(LogType _type)
	{
		return this.enabledLogLevels.Contains(_type);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x0007E9BE File Offset: 0x0007CBBE
	public string GetDescription()
	{
		return "Terminal Window";
	}

	// Token: 0x04000DD1 RID: 3537
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string iconPath = GameIO.GetGameDir("Data") + "/7dtd_icon.ico";

	// Token: 0x04000DD2 RID: 3538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<LogType> enabledLogLevels = new HashSet<LogType>
	{
		LogType.Log,
		LogType.Warning,
		LogType.Error,
		LogType.Exception,
		LogType.Assert
	};

	// Token: 0x04000DD3 RID: 3539
	[PublicizedFrom(EAccessModifier.Private)]
	public RichTextBox consoleOutputBox;

	// Token: 0x04000DD4 RID: 3540
	[PublicizedFrom(EAccessModifier.Private)]
	public TextBox commandInputBox;

	// Token: 0x04000DD5 RID: 3541
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceClose;

	// Token: 0x04000DD6 RID: 3542
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shutdownRequested;

	// Token: 0x04000DD7 RID: 3543
	[PublicizedFrom(EAccessModifier.Private)]
	public const int lineLimit = 500;

	// Token: 0x04000DD8 RID: 3544
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorCommandReply = global::System.Drawing.Color.LightCyan;

	// Token: 0x04000DD9 RID: 3545
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorNormal = global::System.Drawing.Color.LimeGreen;

	// Token: 0x04000DDA RID: 3546
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorWarning = global::System.Drawing.Color.Yellow;

	// Token: 0x04000DDB RID: 3547
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorError = global::System.Drawing.Color.Red;

	// Token: 0x04000DDC RID: 3548
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string, global::System.Drawing.Color> addLineDelegate;
}
