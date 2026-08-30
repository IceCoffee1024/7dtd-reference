using System;
using System.Threading;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020002CE RID: 718
public class WinFormInstance : IConsoleServer
{
	// Token: 0x060014C1 RID: 5313 RVA: 0x0007D8F0 File Offset: 0x0007BAF0
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

	// Token: 0x060014C2 RID: 5314 RVA: 0x0007D984 File Offset: 0x0007BB84
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

	// Token: 0x060014C3 RID: 5315 RVA: 0x0007D9D9 File Offset: 0x0007BBD9
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplicationOnThreadException(object _sender, ThreadExceptionEventArgs _threadExceptionEventArgs)
	{
		Log.Error("TerminalWindow Exeption:");
		Log.Exception(_threadExceptionEventArgs.Exception);
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0007D9F0 File Offset: 0x0007BBF0
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

	// Token: 0x060014C5 RID: 5317 RVA: 0x0007DA2C File Offset: 0x0007BC2C
	public void SendLine(string _line)
	{
		if (_line != null && this.form != null)
		{
			this.form.SendLine(_line);
		}
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0007DA45 File Offset: 0x0007BC45
	public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (_formattedMessage != null && this.form != null)
		{
			this.form.SendLog(_formattedMessage, _plainMessage, _trace, _type, _timestamp, _uptime);
		}
	}

	// Token: 0x04000DC4 RID: 3524
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Thread windowThread;

	// Token: 0x04000DC5 RID: 3525
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopThread;

	// Token: 0x04000DC6 RID: 3526
	[PublicizedFrom(EAccessModifier.Private)]
	public WinFormConnection form;
}
