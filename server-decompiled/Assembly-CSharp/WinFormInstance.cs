using System;
using System.Threading;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020002D2 RID: 722
public class WinFormInstance : IConsoleServer
{
	// Token: 0x060014D3 RID: 5331 RVA: 0x0007DF48 File Offset: 0x0007C148
	public WinFormInstance()
	{
		try
		{
			this.windowThread = new Thread(new ThreadStart(this.windowThreadMain))
			{
				Name = "WinFormInstance"
			};
			this.windowThread.SetApartmentState(ApartmentState.STA);
			this.windowThread.Start();
			Thread.Sleep(250);
			Log.Out("Started Terminal Window");
		}
		catch (Exception ex)
		{
			string text = "Error in WinFormInstance.ctor: ";
			Exception ex2 = ex;
			Log.Out(text + ((ex2 != null) ? ex2.ToString() : null));
		}
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x0007DFDC File Offset: 0x0007C1DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void windowThreadMain()
	{
		this.form = new WinFormConnection(this);
		Log.Out("WinThread started");
		global::System.Windows.Forms.Application.ThreadException += this.ApplicationOnThreadException;
		global::System.Windows.Forms.Application.Run(this.form);
		Profiler.EndThreadProfiling();
		this.form = null;
		Log.Out("WinThread ended");
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0007E031 File Offset: 0x0007C231
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplicationOnThreadException(object _sender, ThreadExceptionEventArgs _threadExceptionEventArgs)
	{
		Log.Error("TerminalWindow Exeption:");
		Log.Exception(_threadExceptionEventArgs.Exception);
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x0007E048 File Offset: 0x0007C248
	public void Disconnect()
	{
		if (this.form == null)
		{
			return;
		}
		Log.Out("Closing Terminal Window");
		WinFormConnection winFormConnection = this.form;
		this.form = null;
		winFormConnection.CloseTerminal();
		this.windowThread.Join();
		Log.Out("Ended Terminal Window");
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x0007E084 File Offset: 0x0007C284
	public void SendLine(string _line)
	{
		if (_line != null && this.form != null)
		{
			this.form.SendLine(_line);
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x0007E09D File Offset: 0x0007C29D
	public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (_formattedMessage != null && this.form != null)
		{
			this.form.SendLog(_formattedMessage, _plainMessage, _trace, _type, _timestamp, _uptime);
		}
	}

	// Token: 0x04000DD5 RID: 3541
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Thread windowThread;

	// Token: 0x04000DD6 RID: 3542
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopThread;

	// Token: 0x04000DD7 RID: 3543
	[PublicizedFrom(EAccessModifier.Private)]
	public WinFormConnection form;
}
