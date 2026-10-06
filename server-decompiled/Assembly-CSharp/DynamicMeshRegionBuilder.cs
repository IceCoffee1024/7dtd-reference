using System;
using System.Threading;

// Token: 0x0200039C RID: 924
public class DynamicMeshRegionBuilder
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001B7B RID: 7035 RVA: 0x000A32B7 File Offset: 0x000A14B7
	public static World world
	{
		get
		{
			return GameManager.Instance.World;
		}
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000A32C3 File Offset: 0x000A14C3
	public bool AddNewItem(DynamicMeshRegion region)
	{
		if (this.Status != DynamicMeshBuilderStatus.Ready)
		{
			Log.Warning("Builder thread tried to start when not ready. Current Status: " + this.Status.ToString());
			return false;
		}
		this.Region = region;
		this.Status = DynamicMeshBuilderStatus.StartingExport;
		return true;
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000A3300 File Offset: 0x000A1500
	public void RequestStop(bool forceStop = false)
	{
		this.StopRequested = true;
		if (forceStop)
		{
			try
			{
				Thread thread = this.thread;
				if (thread != null)
				{
					thread.Abort();
				}
			}
			catch (Exception)
			{
			}
			this.Status = DynamicMeshBuilderStatus.Stopped;
		}
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x000A3344 File Offset: 0x000A1544
	public void StartThread()
	{
		this.thread = new Thread(delegate
		{
			try
			{
				while (!this.StopRequested)
				{
					if (GameManager.Instance == null)
					{
						return;
					}
					if (GameManager.Instance.World == null)
					{
						return;
					}
					if (this.Status == DynamicMeshBuilderStatus.Ready || this.Status == DynamicMeshBuilderStatus.Complete)
					{
						Thread.Sleep(100);
					}
					else
					{
						if (this.Status != DynamicMeshBuilderStatus.StartingExport)
						{
							Log.Error("Builder thread and wrong state: " + this.Status.ToString());
							this.Status = DynamicMeshBuilderStatus.Error;
							return;
						}
						throw new NotImplementedException("No build method");
					}
				}
			}
			catch (Exception ex)
			{
				this.Error = "Builder error: " + ex.Message;
				Log.Error(this.Error);
			}
			this.Status = DynamicMeshBuilderStatus.Stopped;
		});
		this.thread.Start();
	}

	// Token: 0x040011AA RID: 4522
	public string Error;

	// Token: 0x040011AB RID: 4523
	public bool StopRequested;

	// Token: 0x040011AC RID: 4524
	public DynamicMeshBuilderStatus Status;

	// Token: 0x040011AD RID: 4525
	public ExportMeshResult Result = ExportMeshResult.Missing;

	// Token: 0x040011AE RID: 4526
	public DynamicMeshRegion Region;

	// Token: 0x040011AF RID: 4527
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread thread;
}
