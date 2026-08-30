using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class WinFormConnection : Form, IConsoleConnection
{
	// Token: 0x060014C7 RID: 5319 RVA: 0x0007DA68 File Offset: 0x0007BC68
	public WinFormConnection(WinFormInstance _owner)
	{
		this.initialize();
		ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(this.OnGameStartDone));
		ModEvents.GameAwake.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameAwakeData>(this.OnGameAwake));
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0007DAFA File Offset: 0x0007BCFA
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

	// Token: 0x060014C9 RID: 5321 RVA: 0x0007DB2C File Offset: 0x0007BD2C
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

	// Token: 0x060014CA RID: 5322 RVA: 0x0007DBAA File Offset: 0x0007BDAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnClosed(EventArgs _e)
	{
		base.OnClosed(_e);
		ModEvents.GameStartDone.UnregisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(this.OnGameStartDone));
		ModEvents.GameAwake.UnregisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameAwakeData>(this.OnGameAwake));
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x0007DBE0 File Offset: 0x0007BDE0
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

	// Token: 0x060014CC RID: 5324 RVA: 0x0007DD68 File Offset: 0x0007BF68
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameAwake(ref ModEvents.SGameAwakeData _data)
	{
		this.Text = string.Format("{0} - Port {1} - Loading - 7 Days to Die Dedicated Server Console", GamePrefs.GetString(EnumGamePrefs.ServerName), GamePrefs.GetInt(EnumGamePrefs.ServerPort));
		foreach (IConsoleCommand consoleCommand in SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands())
		{
			this.commandInputBox.AutoCompleteCustomSource.AddRange(consoleCommand.GetCommands());
		}
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x0007DDEC File Offset: 0x0007BFEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameStartDone(ref ModEvents.SGameStartDoneData _data)
	{
		this.Text = string.Format("{0} - Port {1} - Running - 7 Days to Die Dedicated Server Console", GamePrefs.GetString(EnumGamePrefs.ServerName), GamePrefs.GetInt(EnumGamePrefs.ServerPort));
		this.commandInputBox.Enabled = true;
		this.commandInputBox.Clear();
		this.commandInputBox.Focus();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0007DE3F File Offset: 0x0007C03F
	[PublicizedFrom(EAccessModifier.Private)]
	public void CommandInputBoxOnKeyDown(object _sender, KeyEventArgs _keyEventArgs)
	{
		if (_keyEventArgs.KeyCode == Keys.Return)
		{
			this.execCommand();
		}
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x0007DE54 File Offset: 0x0007C054
	[PublicizedFrom(EAccessModifier.Private)]
	public void execCommand()
	{
		if (this.commandInputBox.Enabled && this.commandInputBox.Text.Length > 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteAsync(this.commandInputBox.Text, this);
			this.commandInputBox.Clear();
		}
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x0007DEA4 File Offset: 0x0007C0A4
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

	// Token: 0x060014D1 RID: 5329 RVA: 0x0007DFD5 File Offset: 0x0007C1D5
	public void SendLine(string _line)
	{
		this.AddLine(_line, WinFormConnection.logColorCommandReply);
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x0007DFE4 File Offset: 0x0007C1E4
	public void SendLines(List<string> _output)
	{
		foreach (string text in _output)
		{
			this.SendLine(text);
		}
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x0007E034 File Offset: 0x0007C234
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

	// Token: 0x060014D4 RID: 5332 RVA: 0x0007E084 File Offset: 0x0007C284
	public void EnableLogLevel(LogType _type, bool _enable)
	{
		if (_enable)
		{
			this.enabledLogLevels.Add(_type);
			return;
		}
		this.enabledLogLevels.Remove(_type);
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0007E0A4 File Offset: 0x0007C2A4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsLogLevelEnabled(LogType _type)
	{
		return this.enabledLogLevels.Contains(_type);
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x0007E0B2 File Offset: 0x0007C2B2
	public string GetDescription()
	{
		return "Terminal Window";
	}

	// Token: 0x04000DC7 RID: 3527
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string iconPath = GameIO.GetGameDir("Data") + "/7dtd_icon.ico";

	// Token: 0x04000DC8 RID: 3528
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<LogType> enabledLogLevels = new HashSet<LogType>
	{
		LogType.Log,
		LogType.Warning,
		LogType.Error,
		LogType.Exception,
		LogType.Assert
	};

	// Token: 0x04000DC9 RID: 3529
	[PublicizedFrom(EAccessModifier.Private)]
	public RichTextBox consoleOutputBox;

	// Token: 0x04000DCA RID: 3530
	[PublicizedFrom(EAccessModifier.Private)]
	public TextBox commandInputBox;

	// Token: 0x04000DCB RID: 3531
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceClose;

	// Token: 0x04000DCC RID: 3532
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shutdownRequested;

	// Token: 0x04000DCD RID: 3533
	[PublicizedFrom(EAccessModifier.Private)]
	public const int lineLimit = 500;

	// Token: 0x04000DCE RID: 3534
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorCommandReply = global::System.Drawing.Color.LightCyan;

	// Token: 0x04000DCF RID: 3535
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorNormal = global::System.Drawing.Color.LimeGreen;

	// Token: 0x04000DD0 RID: 3536
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorWarning = global::System.Drawing.Color.Yellow;

	// Token: 0x04000DD1 RID: 3537
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly global::System.Drawing.Color logColorError = global::System.Drawing.Color.Red;

	// Token: 0x04000DD2 RID: 3538
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string, global::System.Drawing.Color> addLineDelegate;
}
