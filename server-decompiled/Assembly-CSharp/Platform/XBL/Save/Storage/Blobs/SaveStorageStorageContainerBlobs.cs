using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL.Save.Storage.Blobs
{
	// Token: 0x02001CB7 RID: 7351
	public sealed class SaveStorageStorageContainerBlobs : ISaveStorageContainer, IDisposable
	{
		// Token: 0x0600D974 RID: 55668 RVA: 0x004DDB6E File Offset: 0x004DBD6E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveStorageStorageContainerBlobs] " + text);
		}

		// Token: 0x0600D975 RID: 55669 RVA: 0x004DDB80 File Offset: 0x004DBD80
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageContainerBlobs] " + text);
		}

		// Token: 0x0600D976 RID: 55670 RVA: 0x004DDB92 File Offset: 0x004DBD92
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerBlobs] " + text);
		}

		// Token: 0x0600D977 RID: 55671 RVA: 0x004DDBA4 File Offset: 0x004DBDA4
		public SaveStorageStorageContainerBlobs(string containerName, XGameSaveProviderHandle gameSaveProviderHandle, SingleThreadTaskScheduler taskScheduler)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				bool flag = false;
				try
				{
					this.m_containerName = containerName;
					this.m_taskScheduler = taskScheduler;
					this.m_gameSaveProviderHandle = gameSaveProviderHandle;
					this.m_operationsInput = new ConcurrentQueue<SaveStorageStorageContainerBlobs.BlobOperation>();
					this.m_operationsInputLatest = new ConcurrentDictionary<string, SaveStorageStorageContainerBlobs.BlobOperation>();
					this.m_processOperationsToDo = new List<SaveStorageStorageContainerBlobs.BlobOperation>();
					this.m_processOperationsCurrent = new Dictionary<string, SaveStorageStorageContainerBlobs.BlobOperation>();
					this.m_processOperationsTaskWaitSkipTaskSource = new TaskCompletionSource<bool>();
					this.m_processOperationsTaskCancellationTokenProvider = new CancellationTokenSource();
					if (!Unity.XGamingRuntime.Interop.HR.FAILED(this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveCreateContainer(this.m_gameSaveProviderHandle, this.m_containerName, out this.m_gameSaveContainerHandle))))
					{
						flag = true;
					}
				}
				finally
				{
					if (!flag)
					{
						this.Dispose();
					}
				}
			}
		}

		// Token: 0x0600D978 RID: 55672 RVA: 0x004DDC8C File Offset: 0x004DBE8C
		public void Dispose()
		{
			this.m_disposed = true;
			try
			{
				CancellationTokenSource processOperationsTaskCancellationTokenProvider = this.m_processOperationsTaskCancellationTokenProvider;
				if (processOperationsTaskCancellationTokenProvider != null)
				{
					processOperationsTaskCancellationTokenProvider.Cancel();
				}
			}
			catch (AggregateException ex)
			{
				SaveStorageStorageContainerBlobs.LogError(string.Format("Error while cancelling process operations task: {0}", ex));
			}
			TaskCompletionSource<bool> processOperationsTaskWaitSkipTaskSource = this.m_processOperationsTaskWaitSkipTaskSource;
			if (processOperationsTaskWaitSkipTaskSource != null)
			{
				processOperationsTaskWaitSkipTaskSource.SetResult(true);
			}
			this.m_processOperationsTaskWaitSkipTaskSource = null;
			object processOperationsTaskLock = this.m_processOperationsTaskLock;
			lock (processOperationsTaskLock)
			{
				if (this.m_processOperationsTask != null)
				{
					try
					{
						this.m_processOperationsTask.Wait();
					}
					catch (AggregateException ex2)
					{
						SaveStorageStorageContainerBlobs.LogError(string.Format("Error while waiting for process operations task to complete: {0}", ex2));
					}
					this.m_processOperationsTask = null;
				}
			}
			CancellationTokenSource processOperationsTaskCancellationTokenProvider2 = this.m_processOperationsTaskCancellationTokenProvider;
			if (processOperationsTaskCancellationTokenProvider2 != null)
			{
				processOperationsTaskCancellationTokenProvider2.Dispose();
			}
			this.m_processOperationsTaskCancellationTokenProvider = null;
			this.m_processOperationsCurrent = null;
			this.m_processOperationsToDo = null;
			this.m_operationsInputLatest = null;
			this.m_operationsInput = null;
			if (this.m_gameSaveContainerHandle != null)
			{
				this.m_taskScheduler.ExecuteAndWait(delegate
				{
					SDK.XGameSaveCloseContainer(this.m_gameSaveContainerHandle);
				});
				this.m_gameSaveContainerHandle = null;
			}
			this.m_gameSaveProviderHandle = null;
			this.m_taskScheduler = null;
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x0600D979 RID: 55673 RVA: 0x004DDDC4 File Offset: 0x004DBFC4
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x0600D97A RID: 55674 RVA: 0x004DDDCC File Offset: 0x004DBFCC
		public string Name
		{
			get
			{
				return this.m_containerName;
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x0600D97B RID: 55675 RVA: 0x004DDDD4 File Offset: 0x004DBFD4
		public DateTime LastAccessed
		{
			get
			{
				return this.m_lastAccessHelper.Time;
			}
		}

		// Token: 0x0600D97C RID: 55676 RVA: 0x004DDDE4 File Offset: 0x004DBFE4
		public void Flush(bool waitForFlush)
		{
			Task task = this.StartProcessOperationsTask();
			if (task.IsCompleted)
			{
				return;
			}
			this.m_processOperationsTaskWaitSkipTaskSource.SetResult(true);
			this.m_processOperationsTaskWaitSkipTaskSource = new TaskCompletionSource<bool>();
			if (waitForFlush)
			{
				try
				{
					task.Wait();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600D97D RID: 55677 RVA: 0x004DDE38 File Offset: 0x004DC038
		public bool TryEnumerateBlobInfos(out Unity.XGamingRuntime.XGameSaveBlobInfo[] blobInfos)
		{
			bool flag;
			using (this.m_lastAccessHelper.CreateScope())
			{
				Unity.XGamingRuntime.XGameSaveBlobInfo[] blobInfosTemp = null;
				int num = this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveEnumerateBlobInfo(this.m_gameSaveContainerHandle, out blobInfosTemp));
				if (this.m_operationsInputLatest.Count <= 0)
				{
					blobInfos = blobInfosTemp;
				}
				else
				{
					Dictionary<string, Unity.XGamingRuntime.XGameSaveBlobInfo> dictionary = blobInfosTemp.ToDictionary<Unity.XGamingRuntime.XGameSaveBlobInfo, string>((Unity.XGamingRuntime.XGameSaveBlobInfo blobInfo) => blobInfo.Name);
					foreach (KeyValuePair<string, SaveStorageStorageContainerBlobs.BlobOperation> keyValuePair in this.m_operationsInputLatest)
					{
						string text;
						SaveStorageStorageContainerBlobs.BlobOperation blobOperation;
						keyValuePair.Deconstruct(out text, out blobOperation);
						string text2 = text;
						SaveStorageStorageContainerBlobs.BlobOperation blobOperation2 = blobOperation;
						using (RefCountedBuffer refCountedBuffer = blobOperation2.CreateRef())
						{
							if (refCountedBuffer == null)
							{
								if (!blobOperation2.IsDisposed)
								{
									dictionary.Remove(text2);
								}
							}
							else
							{
								uint length = (uint)refCountedBuffer.Length;
								if (!refCountedBuffer.IsDisposed)
								{
									dictionary[text2] = new Unity.XGamingRuntime.XGameSaveBlobInfo
									{
										Name = text2,
										Size = length
									};
								}
							}
						}
					}
					blobInfos = dictionary.Values.ToArray<Unity.XGamingRuntime.XGameSaveBlobInfo>();
				}
				flag = Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num);
			}
			return flag;
		}

		// Token: 0x0600D97E RID: 55678 RVA: 0x004DDFE0 File Offset: 0x004DC1E0
		public Unity.XGamingRuntime.XGameSaveBlobInfo GetBlobInfo(string blobName)
		{
			Unity.XGamingRuntime.XGameSaveBlobInfo xgameSaveBlobInfo;
			using (this.m_lastAccessHelper.CreateScope())
			{
				SaveStorageStorageContainerBlobs.BlobOperation blobOperation;
				if (this.m_operationsInputLatest.TryGetValue(blobName, out blobOperation))
				{
					using (RefCountedBuffer refCountedBuffer = blobOperation.CreateRef())
					{
						if (refCountedBuffer == null)
						{
							if (blobOperation.IsDisposed)
							{
								return null;
							}
						}
						else
						{
							uint length = (uint)refCountedBuffer.Length;
							if (!refCountedBuffer.IsDisposed)
							{
								return new Unity.XGamingRuntime.XGameSaveBlobInfo
								{
									Name = blobName,
									Size = length
								};
							}
						}
					}
				}
				Unity.XGamingRuntime.XGameSaveBlobInfo[] blobInfos = null;
				int num = this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveEnumerateBlobInfoByName(this.m_gameSaveContainerHandle, blobName, out blobInfos));
				if (Unity.XGamingRuntime.Interop.HR.FAILED(num))
				{
					GameCoreSaveHelpers.NonTraceLogHR(num, string.Concat(new string[] { "Enumerate Blobs with prefix '", blobName, "' in Container '", this.m_containerName, "'." }));
					xgameSaveBlobInfo = null;
				}
				else
				{
					foreach (Unity.XGamingRuntime.XGameSaveBlobInfo xgameSaveBlobInfo2 in blobInfos)
					{
						if (xgameSaveBlobInfo2.Name == blobName)
						{
							return xgameSaveBlobInfo2;
						}
					}
					xgameSaveBlobInfo = null;
				}
			}
			return xgameSaveBlobInfo;
		}

		// Token: 0x0600D97F RID: 55679 RVA: 0x004DE16C File Offset: 0x004DC36C
		public RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier)
		{
			RefCountedBuffer[] array4;
			using (this.m_lastAccessHelper.CreateScope())
			{
				string text = null;
				string text2 = null;
				int num = 0;
				Dictionary<string, RefCountedBuffer> blobNamesToBuffer = new Dictionary<string, RefCountedBuffer>();
				bool flag = false;
				try
				{
					if (this.m_operationsInputLatest.Count > 0)
					{
						foreach (string text3 in blobNames)
						{
							SaveStorageStorageContainerBlobs.BlobOperation blobOperation;
							if (this.m_operationsInputLatest.TryGetValue(text3, out blobOperation))
							{
								if (blobOperation.Delete)
								{
									throw new IOException("Failed to read deleted blob named '" + text3 + "'.");
								}
								RefCountedBuffer refCountedBuffer = blobOperation.CreateRef();
								if (refCountedBuffer != null && !refCountedBuffer.IsDisposed)
								{
									blobNamesToBuffer[text3] = refCountedBuffer;
								}
							}
						}
					}
					int count = blobNamesToBuffer.Count;
					string[] blobNamesToRead = blobNames.Where<string>((string blobName) => !blobNamesToBuffer.ContainsKey(blobName)).ToArray<string>();
					if (blobNamesToRead.Length != 0)
					{
						Stopwatch stopwatch = new Stopwatch();
						XGameSaveBlob[] blobs = null;
						for (;;)
						{
							using (ManualResetEventSlim done = new ManualResetEventSlim())
							{
								int hr = -1;
								XGameSaveReadBlobDataCompleted <>9__3;
								this.m_taskScheduler.ExecuteAndWait<int>(delegate
								{
									XGameSaveContainerHandle gameSaveContainerHandle = this.m_gameSaveContainerHandle;
									string[] blobNamesToRead2 = blobNamesToRead;
									XGameSaveReadBlobDataCompleted xgameSaveReadBlobDataCompleted;
									if ((xgameSaveReadBlobDataCompleted = <>9__3) == null)
									{
										xgameSaveReadBlobDataCompleted = (<>9__3 = delegate(int hresult, XGameSaveBlob[] saveBlobs)
										{
											hr = hresult;
											blobs = saveBlobs;
											done.Set();
										});
									}
									return SDK.XGameSaveReadBlobDataAsync(gameSaveContainerHandle, blobNamesToRead2, xgameSaveReadBlobDataCompleted);
								});
								stopwatch.Start();
								while (!done.IsSet)
								{
									done.Wait(SaveStorageStorageContainerBlobs.GetBlobsWaitReportingInterval);
									TimeSpan elapsed = stopwatch.Elapsed;
									if (elapsed >= SaveStorageStorageContainerBlobs.GetBlobsWaitReportingInterval)
									{
										string text4 = "Read Blob(s) named '{0}' for '{1}' in Container '{2}' has been waiting for {3:F3} s.";
										object[] array = new object[4];
										int num2 = 0;
										string text5;
										if ((text5 = text) == null)
										{
											text5 = (text = string.Join("', '", blobNamesToRead));
										}
										array[num2] = text5;
										int num3 = 1;
										string text6;
										if ((text6 = text2) == null)
										{
											text6 = (text2 = debugIdentifier.ToString());
										}
										array[num3] = text6;
										array[2] = this.m_containerName;
										array[3] = elapsed.TotalSeconds;
										SaveStorageStorageContainerBlobs.LogWarning(string.Format(text4, array));
									}
								}
								stopwatch.Stop();
								if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hr))
								{
									if (num >= 10 || (hr != -2147024638 && hr != -2147024882))
									{
										int hr2 = hr;
										string[] array2 = new string[7];
										array2[0] = "Read Blob(s) named '";
										int num4 = 1;
										string text7;
										if ((text7 = text) == null)
										{
											text7 = (text = string.Join("', '", blobNamesToRead));
										}
										array2[num4] = text7;
										array2[2] = "' for '";
										int num5 = 3;
										string text8;
										if ((text8 = text2) == null)
										{
											text8 = (text2 = debugIdentifier.ToString());
										}
										array2[num5] = text8;
										array2[4] = "' in Container '";
										array2[5] = this.m_containerName;
										array2[6] = "'.";
										GameCoreSaveHelpers.NonTraceLogHR(hr2, string.Concat(array2));
										throw new IOException("Failed to read blob(s).");
									}
									num++;
									continue;
								}
							}
							break;
						}
						foreach (XGameSaveBlob xgameSaveBlob in blobs)
						{
							blobNamesToBuffer[xgameSaveBlob.Info.Name] = RefCountedBuffer.CreateFromExisting(xgameSaveBlob.Data);
						}
					}
					RefCountedBuffer[] array3 = blobNames.Select<string, RefCountedBuffer>((string blobName) => blobNamesToBuffer[blobName]).ToArray<RefCountedBuffer>();
					flag = true;
					array4 = array3;
				}
				finally
				{
					if (!flag)
					{
						foreach (RefCountedBuffer refCountedBuffer2 in blobNamesToBuffer.Values)
						{
							if (refCountedBuffer2 != null)
							{
								refCountedBuffer2.Dispose();
							}
						}
					}
				}
			}
			return array4;
		}

		// Token: 0x0600D980 RID: 55680 RVA: 0x004DE588 File Offset: 0x004DC788
		public void SetBlob(string blobName, RefCountedBuffer blobData)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				this.EnqueueBlobOperation(new SaveStorageStorageContainerBlobs.BlobOperation(blobName, blobData));
			}
		}

		// Token: 0x0600D981 RID: 55681 RVA: 0x004DE5D0 File Offset: 0x004DC7D0
		public void DeleteBlob(string blobName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				this.EnqueueBlobOperation(new SaveStorageStorageContainerBlobs.BlobOperation(blobName, null));
			}
		}

		// Token: 0x0600D982 RID: 55682 RVA: 0x004DE618 File Offset: 0x004DC818
		public int GetQueuedUsed()
		{
			int num = 0;
			foreach (SaveStorageStorageContainerBlobs.BlobOperation blobOperation in this.m_operationsInputLatest.Values)
			{
				using (RefCountedBuffer refCountedBuffer = blobOperation.CreateRef())
				{
					if (refCountedBuffer != null)
					{
						num += refCountedBuffer.Length;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D983 RID: 55683 RVA: 0x004DE690 File Offset: 0x004DC890
		[PublicizedFrom(EAccessModifier.Private)]
		public Task StartProcessOperationsTask()
		{
			object processOperationsTaskLock = this.m_processOperationsTaskLock;
			Task task;
			lock (processOperationsTaskLock)
			{
				Task processOperationsTask = this.m_processOperationsTask;
				if (processOperationsTask != null)
				{
					task = processOperationsTask;
				}
				else if (this.m_operationsInput.Count <= 0)
				{
					task = Task.CompletedTask;
				}
				else
				{
					CancellationToken cancellationToken = this.m_processOperationsTaskCancellationTokenProvider.Token;
					Task waitSkipTask = this.m_processOperationsTaskWaitSkipTaskSource.Task;
					Task task2 = this.m_taskScheduler.Factory.StartNew<Task>(() => this.ProcessOperations(cancellationToken, waitSkipTask), cancellationToken).Unwrap();
					this.m_processOperationsTask = task2;
					task = task2;
				}
			}
			return task;
		}

		// Token: 0x0600D984 RID: 55684 RVA: 0x004DE758 File Offset: 0x004DC958
		[PublicizedFrom(EAccessModifier.Private)]
		public void EnqueueBlobOperation(SaveStorageStorageContainerBlobs.BlobOperation operation)
		{
			if (LaunchPrefs.GameCoreBlobOperationQueueMaxTotalSize.Value > 0 && this.m_operationsTotalSize > LaunchPrefs.GameCoreBlobOperationQueueMaxTotalSize.Value)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				TimeSpan timeSpan = TimeSpan.Zero;
				while (this.m_operationsTotalSize > LaunchPrefs.GameCoreBlobOperationQueueMaxTotalSize.Value)
				{
					Thread.Sleep(50);
					if (stopwatch.Elapsed > timeSpan + SaveStorageStorageContainerBlobs.EnqueueBlobOperationWaitReportingInterval)
					{
						timeSpan += SaveStorageStorageContainerBlobs.EnqueueBlobOperationWaitReportingInterval;
						SaveStorageStorageContainerBlobs.LogWarning(string.Format("Waiting for blob operations to be processed to free up space for {0:F3}s. Total size: {1}.", timeSpan.TotalSeconds, this.m_operationsTotalSize.FormatSize(true)));
					}
				}
			}
			Interlocked.Add(ref this.m_operationsTotalSize, operation.Size);
			this.m_operationsInputLatest[operation.Name] = operation;
			this.m_operationsInput.Enqueue(operation);
			this.StartProcessOperationsTask();
		}

		// Token: 0x0600D985 RID: 55685 RVA: 0x004DE838 File Offset: 0x004DCA38
		[PublicizedFrom(EAccessModifier.Private)]
		public async Task ProcessOperations(CancellationToken cancellationToken, Task waitSkipTask)
		{
			await Task.WhenAny(new Task[]
			{
				Task.Delay(SaveStorageStorageContainerBlobs.BlobQueueDelay, cancellationToken),
				waitSkipTask
			});
			try
			{
				while (this.m_operationsInput.Count > 0 || this.m_processOperationsToDo.Count > 0)
				{
					this.<ProcessOperations>g__DoSingleUpdate|40_0();
				}
			}
			catch (Exception ex)
			{
				SaveStorageStorageContainerBlobs.LogError(string.Format("Error processing blob operations: {0}", ex));
			}
			finally
			{
				this.m_processOperationsCurrent.Clear();
				object processOperationsTaskLock = this.m_processOperationsTaskLock;
				bool flag = false;
				try
				{
					Monitor.Enter(processOperationsTaskLock, ref flag);
					this.m_processOperationsTask = null;
				}
				finally
				{
					int num;
					if (num < 0 && flag)
					{
						Monitor.Exit(processOperationsTaskLock);
					}
				}
			}
		}

		// Token: 0x0600D989 RID: 55689 RVA: 0x004DE8EC File Offset: 0x004DCAEC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <ProcessOperations>g__DoSingleUpdate|40_0()
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				SaveStorageStorageContainerBlobs.BlobOperation blobOperation;
				while (this.m_operationsInput.TryDequeue(out blobOperation))
				{
					this.m_processOperationsToDo.Add(blobOperation);
				}
				this.m_processOperationsCurrent.Clear();
				int num = 0;
				int num2 = -1;
				for (int i = 0; i < this.m_processOperationsToDo.Count; i++)
				{
					SaveStorageStorageContainerBlobs.BlobOperation blobOperation2 = this.m_processOperationsToDo[i];
					using (RefCountedBuffer refCountedBuffer = blobOperation2.CreateRef())
					{
						SaveStorageStorageContainerBlobs.BlobOperation valueOrDefault = this.m_processOperationsCurrent.GetValueOrDefault(blobOperation2.Name);
						using (RefCountedBuffer refCountedBuffer2 = ((valueOrDefault != null) ? valueOrDefault.CreateRef() : null))
						{
							if (refCountedBuffer2 != null)
							{
								num -= refCountedBuffer2.Length;
							}
							if (refCountedBuffer != null)
							{
								num += refCountedBuffer.Length;
							}
							if (num <= 16777216)
							{
								num2 = i;
							}
							this.m_processOperationsCurrent[blobOperation2.Name] = blobOperation2;
						}
					}
				}
				if (num2 >= 0)
				{
					this.m_processOperationsCurrent.Clear();
					for (int j = 0; j <= num2; j++)
					{
						SaveStorageStorageContainerBlobs.BlobOperation blobOperation3 = this.m_processOperationsToDo[j];
						this.m_processOperationsCurrent[blobOperation3.Name] = blobOperation3;
					}
					XGameSaveUpdateHandle updateHandle = null;
					int num3 = this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveCreateUpdate(this.m_gameSaveContainerHandle, this.m_containerName, out updateHandle));
					if (Unity.XGamingRuntime.Interop.HR.FAILED(num3))
					{
						GameCoreSaveHelpers.NonTraceLogHR(num3, string.Format("Create Update Handle for Container '{0}' for {1} operation(s).", this.m_containerName, this.m_processOperationsCurrent.Count));
						throw new IOException(string.Format("Failed to create update handle. {0} (0x{1:X8})", XblHelpers.GetHRName(num3), num3));
					}
					try
					{
						using (Dictionary<string, SaveStorageStorageContainerBlobs.BlobOperation>.ValueCollection.Enumerator enumerator = this.m_processOperationsCurrent.Values.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SaveStorageStorageContainerBlobs.BlobOperation op = enumerator.Current;
								if (op.Delete)
								{
									int num4 = this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveSubmitBlobDelete(updateHandle, op.Name));
									if (Unity.XGamingRuntime.Interop.HR.FAILED(num4))
									{
										GameCoreSaveHelpers.NonTraceLogHR(num4, string.Concat(new string[] { "Delete Blob Data named '", op.Name, "' for Container '", this.m_containerName, "'." }));
										throw new IOException(string.Format("Failed to delete blob. {0} (0x{1:X8})", XblHelpers.GetHRName(num4), num4));
									}
								}
							}
						}
						foreach (SaveStorageStorageContainerBlobs.BlobOperation blobOperation4 in this.m_processOperationsCurrent.Values)
						{
							if (!blobOperation4.Delete)
							{
								using (RefCountedBuffer refCountedBuffer3 = blobOperation4.CreateRef())
								{
									if (refCountedBuffer3.Offset != 0)
									{
										throw new NotSupportedException("Non-zero offsets are currently not supported.");
									}
									int num5 = SDK.XGameSaveSubmitBlobWrite(updateHandle, blobOperation4.Name, refCountedBuffer3.BufferRaw, (ulong)((long)refCountedBuffer3.Length));
									if (Unity.XGamingRuntime.Interop.HR.FAILED(num5))
									{
										GameCoreSaveHelpers.NonTraceLogHR(num5, string.Concat(new string[] { "Write Blob Data named '", blobOperation4.Name, "' for Container '", this.m_containerName, "'." }));
										throw new IOException(string.Format("Failed to write blob data. {0} (0x{1:X8})", XblHelpers.GetHRName(num5), num5));
									}
								}
							}
						}
						int num6 = this.m_taskScheduler.ExecuteAndWait<int>(() => SDK.XGameSaveSubmitUpdate(updateHandle));
						if (Unity.XGamingRuntime.Interop.HR.FAILED(num6))
						{
							GameCoreSaveHelpers.NonTraceLogHR(num6, string.Format("Submit Update for Container '{0}' for {1} operation(s).", this.m_containerName, this.m_processOperationsCurrent.Count));
							throw new IOException(string.Format("Failed to submit update. {0} (0x{1:X8})", XblHelpers.GetHRName(num6), num6));
						}
					}
					finally
					{
						this.m_taskScheduler.ExecuteAndWait(delegate
						{
							SDK.XGameSaveCloseUpdate(updateHandle);
						});
					}
					for (int k = 0; k <= num2; k++)
					{
						SaveStorageStorageContainerBlobs.BlobOperation blobOperation5 = this.m_processOperationsToDo[k];
						Interlocked.Add(ref this.m_operationsTotalSize, -blobOperation5.Size);
						blobOperation5.Dispose();
						((ICollection<KeyValuePair<string, SaveStorageStorageContainerBlobs.BlobOperation>>)this.m_operationsInputLatest).Remove(new KeyValuePair<string, SaveStorageStorageContainerBlobs.BlobOperation>(blobOperation5.Name, blobOperation5));
					}
					this.m_processOperationsToDo.RemoveRange(0, num2 + 1);
				}
			}
		}

		// Token: 0x0400A551 RID: 42321
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxBlobSize = 16777216;

		// Token: 0x0400A552 RID: 42322
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxUpdateSize = 16777216;

		// Token: 0x0400A553 RID: 42323
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan GetBlobsWaitReportingInterval = TimeSpan.FromSeconds(1.0);

		// Token: 0x0400A554 RID: 42324
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan EnqueueBlobOperationWaitReportingInterval = TimeSpan.FromSeconds(1.0);

		// Token: 0x0400A555 RID: 42325
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan BlobQueueDelay = TimeSpan.FromSeconds(5.0);

		// Token: 0x0400A556 RID: 42326
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LastAccessHelper m_lastAccessHelper = new LastAccessHelper();

		// Token: 0x0400A557 RID: 42327
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x0400A558 RID: 42328
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerName;

		// Token: 0x0400A559 RID: 42329
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleThreadTaskScheduler m_taskScheduler;

		// Token: 0x0400A55A RID: 42330
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveProviderHandle m_gameSaveProviderHandle;

		// Token: 0x0400A55B RID: 42331
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveContainerHandle m_gameSaveContainerHandle;

		// Token: 0x0400A55C RID: 42332
		[PublicizedFrom(EAccessModifier.Private)]
		public ConcurrentQueue<SaveStorageStorageContainerBlobs.BlobOperation> m_operationsInput;

		// Token: 0x0400A55D RID: 42333
		[PublicizedFrom(EAccessModifier.Private)]
		public ConcurrentDictionary<string, SaveStorageStorageContainerBlobs.BlobOperation> m_operationsInputLatest;

		// Token: 0x0400A55E RID: 42334
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_operationsTotalSize;

		// Token: 0x0400A55F RID: 42335
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_processOperationsTaskLock = new object();

		// Token: 0x0400A560 RID: 42336
		[PublicizedFrom(EAccessModifier.Private)]
		public List<SaveStorageStorageContainerBlobs.BlobOperation> m_processOperationsToDo;

		// Token: 0x0400A561 RID: 42337
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, SaveStorageStorageContainerBlobs.BlobOperation> m_processOperationsCurrent;

		// Token: 0x0400A562 RID: 42338
		[PublicizedFrom(EAccessModifier.Private)]
		public TaskCompletionSource<bool> m_processOperationsTaskWaitSkipTaskSource;

		// Token: 0x0400A563 RID: 42339
		[PublicizedFrom(EAccessModifier.Private)]
		public CancellationTokenSource m_processOperationsTaskCancellationTokenProvider;

		// Token: 0x0400A564 RID: 42340
		[PublicizedFrom(EAccessModifier.Private)]
		public Task m_processOperationsTask;

		// Token: 0x02001CB8 RID: 7352
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class BlobOperation : IDisposable
		{
			// Token: 0x0600D98A RID: 55690 RVA: 0x004DEE20 File Offset: 0x004DD020
			public BlobOperation(string name, RefCountedBuffer buffer)
			{
				this.m_name = name;
				this.m_delete = buffer == null;
				this.m_buffer = ((buffer != null) ? buffer.CreateRef() : null);
				if (this.m_buffer != null && this.m_buffer.Offset != 0)
				{
					this.Dispose();
					throw new ArgumentException("Non-zero offsets are currently not supported.", "buffer");
				}
			}

			// Token: 0x0600D98B RID: 55691 RVA: 0x004DEE81 File Offset: 0x004DD081
			[PublicizedFrom(EAccessModifier.Private)]
			public void Dispose(bool disposing)
			{
				if (!disposing)
				{
					Log.Error("BlobOperation is being finalized. It should be disposed properly.");
					return;
				}
				this.m_disposed = true;
				RefCountedBuffer buffer = this.m_buffer;
				if (buffer != null)
				{
					buffer.Dispose();
				}
				this.m_buffer = null;
			}

			// Token: 0x0600D98C RID: 55692 RVA: 0x004DEEB0 File Offset: 0x004DD0B0
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x0600D98D RID: 55693 RVA: 0x004DEEC0 File Offset: 0x004DD0C0
			[PublicizedFrom(EAccessModifier.Protected)]
			public ~BlobOperation()
			{
				this.Dispose(false);
			}

			// Token: 0x17001B91 RID: 7057
			// (get) Token: 0x0600D98E RID: 55694 RVA: 0x004DEEF0 File Offset: 0x004DD0F0
			public bool IsDisposed
			{
				get
				{
					return this.m_disposed;
				}
			}

			// Token: 0x17001B92 RID: 7058
			// (get) Token: 0x0600D98F RID: 55695 RVA: 0x004DEEF8 File Offset: 0x004DD0F8
			public string Name
			{
				get
				{
					return this.m_name;
				}
			}

			// Token: 0x17001B93 RID: 7059
			// (get) Token: 0x0600D990 RID: 55696 RVA: 0x004DEF00 File Offset: 0x004DD100
			public int Size
			{
				get
				{
					RefCountedBuffer buffer = this.m_buffer;
					if (buffer == null)
					{
						return 0;
					}
					return buffer.Length;
				}
			}

			// Token: 0x0600D991 RID: 55697 RVA: 0x004DEF13 File Offset: 0x004DD113
			public RefCountedBuffer CreateRef()
			{
				RefCountedBuffer buffer = this.m_buffer;
				if (buffer == null)
				{
					return null;
				}
				return buffer.CreateRef();
			}

			// Token: 0x17001B94 RID: 7060
			// (get) Token: 0x0600D992 RID: 55698 RVA: 0x004DEF26 File Offset: 0x004DD126
			public bool Delete
			{
				get
				{
					return this.m_delete;
				}
			}

			// Token: 0x0400A565 RID: 42341
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly string m_name;

			// Token: 0x0400A566 RID: 42342
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly bool m_delete;

			// Token: 0x0400A567 RID: 42343
			[PublicizedFrom(EAccessModifier.Private)]
			public RefCountedBuffer m_buffer;

			// Token: 0x0400A568 RID: 42344
			[PublicizedFrom(EAccessModifier.Private)]
			public bool m_disposed;
		}
	}
}
