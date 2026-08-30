using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000251 RID: 593
	[NativeHeader("Runtime/Scripting/DelayedCallUtility.h")]
	[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
	[ExtensionOfNativeClass]
	[RequiredByNativeCode]
	public class MonoBehaviour : Behaviour
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x0002A358 File Offset: 0x00028558
		public CancellationToken destroyCancellationToken
		{
			get
			{
				bool flag = this == null;
				if (flag)
				{
					throw new MissingReferenceException("DestroyCancellation token should be called atleast once before destroying the monobehaviour object");
				}
				bool flag2 = this.m_CancellationTokenSource == null;
				if (flag2)
				{
					this.m_CancellationTokenSource = new CancellationTokenSource();
					this.OnCancellationTokenCreated();
				}
				return this.m_CancellationTokenSource.Token;
			}
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0002A3AC File Offset: 0x000285AC
		[RequiredByNativeCode]
		private void RaiseCancellation()
		{
			CancellationTokenSource cancellationTokenSource = this.m_CancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0002A3C4 File Offset: 0x000285C4
		public bool IsInvoking()
		{
			return MonoBehaviour.Internal_IsInvokingAll(this);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0002A3DC File Offset: 0x000285DC
		public void CancelInvoke()
		{
			MonoBehaviour.Internal_CancelInvokeAll(this);
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0002A3E6 File Offset: 0x000285E6
		public void Invoke(string methodName, float time)
		{
			MonoBehaviour.InvokeDelayed(this, methodName, time, 0f);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0002A3F8 File Offset: 0x000285F8
		public void InvokeRepeating(string methodName, float time, float repeatRate)
		{
			bool flag = repeatRate <= 1E-05f && repeatRate != 0f;
			if (flag)
			{
				throw new UnityException("Invoke repeat rate has to be larger than 0.00001F)");
			}
			MonoBehaviour.InvokeDelayed(this, methodName, time, repeatRate);
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0002A435 File Offset: 0x00028635
		public void CancelInvoke(string methodName)
		{
			MonoBehaviour.CancelInvoke(this, methodName);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0002A440 File Offset: 0x00028640
		public bool IsInvoking(string methodName)
		{
			return MonoBehaviour.IsInvoking(this, methodName);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0002A45C File Offset: 0x0002865C
		[ExcludeFromDocs]
		public Coroutine StartCoroutine(string methodName)
		{
			object obj = null;
			return this.StartCoroutine(methodName, obj);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0002A478 File Offset: 0x00028678
		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
		{
			bool flag = string.IsNullOrEmpty(methodName);
			if (flag)
			{
				throw new NullReferenceException("methodName is null or empty");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged(methodName, value);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0002A4C0 File Offset: 0x000286C0
		public Coroutine StartCoroutine(IEnumerator routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged2(routine);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0002A504 File Offset: 0x00028704
		[Obsolete("StartCoroutine_Auto has been deprecated. Use StartCoroutine instead (UnityUpgradable) -> StartCoroutine([mscorlib] System.Collections.IEnumerator)", false)]
		public Coroutine StartCoroutine_Auto(IEnumerator routine)
		{
			return this.StartCoroutine(routine);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0002A520 File Offset: 0x00028720
		public void StopCoroutine(IEnumerator routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineFromEnumeratorManaged(routine);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0002A564 File Offset: 0x00028764
		public void StopCoroutine(Coroutine routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineManaged(routine);
		}

		// Token: 0x06001949 RID: 6473
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopCoroutine(string methodName);

		// Token: 0x0600194A RID: 6474
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopAllCoroutines();

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600194B RID: 6475
		// (set) Token: 0x0600194C RID: 6476
		public extern bool useGUILayout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0002A5A5 File Offset: 0x000287A5
		public static void print(object message)
		{
			Debug.Log(message);
		}

		// Token: 0x0600194E RID: 6478
		[FreeFunction("CancelInvoke")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CancelInvokeAll([NotNull("NullExceptionObject")] MonoBehaviour self);

		// Token: 0x0600194F RID: 6479
		[FreeFunction("IsInvoking")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_IsInvokingAll([NotNull("NullExceptionObject")] MonoBehaviour self);

		// Token: 0x06001950 RID: 6480
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvokeDelayed([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName, float time, float repeatRate);

		// Token: 0x06001951 RID: 6481
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CancelInvoke([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName);

		// Token: 0x06001952 RID: 6482
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInvoking([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName);

		// Token: 0x06001953 RID: 6483
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsObjectMonoBehaviour([NotNull("NullExceptionObject")] Object obj);

		// Token: 0x06001954 RID: 6484
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Coroutine StartCoroutineManaged(string methodName, object value);

		// Token: 0x06001955 RID: 6485
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Coroutine StartCoroutineManaged2(IEnumerator enumerator);

		// Token: 0x06001956 RID: 6486
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StopCoroutineManaged(Coroutine routine);

		// Token: 0x06001957 RID: 6487
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StopCoroutineFromEnumeratorManaged(IEnumerator routine);

		// Token: 0x06001958 RID: 6488
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetScriptClassName();

		// Token: 0x06001959 RID: 6489
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void OnCancellationTokenCreated();

		// Token: 0x040008D6 RID: 2262
		private CancellationTokenSource m_CancellationTokenSource;
	}
}
