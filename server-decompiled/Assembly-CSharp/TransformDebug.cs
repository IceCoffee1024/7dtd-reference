using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200117C RID: 4476
public sealed class TransformDebug : MonoBehaviour
{
	// Token: 0x06008F02 RID: 36610 RVA: 0x0035D0CC File Offset: 0x0035B2CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		try
		{
			if (this.m_stack.Count <= 0)
			{
				GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
				for (int i = rootGameObjects.Length - 1; i >= 0; i--)
				{
					GameObject gameObject = rootGameObjects[i];
					if (gameObject)
					{
						this.PushTransform(gameObject.transform);
					}
				}
			}
			int num = 0;
			while (num < 256 && this.m_stack.Count > 0)
			{
				this.DoOperation();
				num++;
			}
		}
		finally
		{
		}
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x0035D158 File Offset: 0x0035B358
	[PublicizedFrom(EAccessModifier.Private)]
	public void PushTransform(Transform t)
	{
		if (!t)
		{
			return;
		}
		this.m_stack.Push(t);
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x0035D170 File Offset: 0x0035B370
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoOperation()
	{
		Transform transform = this.m_stack.Pop();
		if (!transform)
		{
			return;
		}
		TransformDebug.TransformState orCreateValue = this.m_states.GetOrCreateValue(transform);
		orCreateValue.Update(transform);
		string text = null;
		if (transform.localPosition != orCreateValue.Position)
		{
			text = this.DescribeTransformHierarchy(transform);
			Log.Error(string.Format("[{0}] Invalid local position {1} correcting to {2}: {3}", new object[] { "TransformDebug", transform.localPosition, orCreateValue.Position, text }));
			transform.localPosition = orCreateValue.Position;
		}
		if (transform.localRotation != orCreateValue.Rotation)
		{
			if (text == null)
			{
				text = this.DescribeTransformHierarchy(transform);
			}
			Log.Error(string.Format("[{0}] Invalid local rotation {1} correcting to {2}: {3}", new object[] { "TransformDebug", transform.localRotation, orCreateValue.Rotation, text }));
			transform.localRotation = orCreateValue.Rotation;
		}
		if (transform.localScale != orCreateValue.Scale)
		{
			if (text == null)
			{
				text = this.DescribeTransformHierarchy(transform);
			}
			Log.Error(string.Format("[{0}] Invalid local scale {1} correcting to {2}: {3}", new object[] { "TransformDebug", transform.localScale, orCreateValue.Scale, text }));
			transform.localScale = orCreateValue.Scale;
		}
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			this.PushTransform(child);
		}
	}

	// Token: 0x06008F05 RID: 36613 RVA: 0x0035D304 File Offset: 0x0035B504
	[PublicizedFrom(EAccessModifier.Private)]
	public string DescribeTransformHierarchy(Transform t)
	{
		string text;
		try
		{
			this.m_describeStack.Push(t.name);
			Transform transform = t;
			while (transform.parent)
			{
				transform = transform.parent;
				this.m_describeStack.Push(transform.name);
			}
			while (this.m_describeStack.Count > 0)
			{
				if (this.m_describeBuilder.Length > 0)
				{
					this.m_describeBuilder.Append('/');
				}
				this.m_describeBuilder.Append(this.m_describeStack.Pop());
			}
			text = this.m_describeBuilder.ToString();
		}
		finally
		{
			this.m_describeStack.Clear();
			this.m_describeBuilder.Clear();
		}
		return text;
	}

	// Token: 0x06008F06 RID: 36614 RVA: 0x0035D3C8 File Offset: 0x0035B5C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsValid(float f)
	{
		return float.IsFinite(f);
	}

	// Token: 0x06008F07 RID: 36615 RVA: 0x0035D3D0 File Offset: 0x0035B5D0
	public static void Test()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		Stack<Transform> stack = new Stack<Transform>();
		foreach (GameObject gameObject in activeScene.GetRootGameObjects())
		{
			if (gameObject && gameObject.name.EqualsCaseInsensitive("Entities"))
			{
				foreach (object obj in gameObject.transform)
				{
					Transform transform = (Transform)obj;
					stack.Push(transform);
				}
			}
		}
		if (stack.Count == 0)
		{
			Log.Error("No entities found (try testing while in-game).");
		}
		while (stack.Count > 0)
		{
			Transform transform2 = stack.Pop();
			if (transform2)
			{
				transform2.localPosition = new Vector3(TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localPosition.x), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localPosition.y), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localPosition.z));
				transform2.localRotation = new Quaternion(TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localRotation.x), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localRotation.y), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localRotation.z), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localRotation.w));
				transform2.localScale = new Vector3(TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localScale.x), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localScale.y), TransformDebug.<Test>g__MaybeCorruptFloat|10_0(transform2.localScale.z));
				foreach (object obj2 in transform2)
				{
					Transform transform3 = (Transform)obj2;
					stack.Push(transform3);
				}
			}
		}
	}

	// Token: 0x06008F09 RID: 36617 RVA: 0x0035D600 File Offset: 0x0035B800
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static float <Test>g__MaybeCorruptFloat|10_0(float originalValue)
	{
		if (global::UnityEngine.Random.value >= 0.05f)
		{
			return originalValue;
		}
		float value = global::UnityEngine.Random.value;
		float num;
		if (value >= 0.33333334f)
		{
			if (value >= 0.6666667f)
			{
				num = float.NaN;
			}
			else
			{
				num = float.PositiveInfinity;
			}
		}
		else
		{
			num = float.NegativeInfinity;
		}
		return num;
	}

	// Token: 0x0400699F RID: 27039
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int MAX_OPERATIONS_PER_FRAME = 256;

	// Token: 0x040069A0 RID: 27040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly ConditionalWeakTable<Transform, TransformDebug.TransformState> m_states = new ConditionalWeakTable<Transform, TransformDebug.TransformState>();

	// Token: 0x040069A1 RID: 27041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Stack<Transform> m_stack = new Stack<Transform>();

	// Token: 0x040069A2 RID: 27042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Stack<string> m_describeStack = new Stack<string>();

	// Token: 0x040069A3 RID: 27043
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_describeBuilder = new StringBuilder();

	// Token: 0x0200117D RID: 4477
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class TransformState
	{
		// Token: 0x06008F0A RID: 36618 RVA: 0x0035D64C File Offset: 0x0035B84C
		public void Update(Transform t)
		{
			Vector3 localPosition = t.localPosition;
			this.Position = new Vector3(TransformDebug.IsValid(localPosition.x) ? localPosition.x : this.Position.x, TransformDebug.IsValid(localPosition.y) ? localPosition.y : this.Position.y, TransformDebug.IsValid(localPosition.z) ? localPosition.z : this.Position.z);
			Quaternion localRotation = t.localRotation;
			this.Rotation = new Quaternion(TransformDebug.IsValid(localRotation.x) ? localRotation.x : this.Rotation.x, TransformDebug.IsValid(localRotation.y) ? localRotation.y : this.Rotation.y, TransformDebug.IsValid(localRotation.z) ? localRotation.z : this.Rotation.z, TransformDebug.IsValid(localRotation.w) ? localRotation.w : this.Rotation.w);
			if (this.Rotation == TransformDebug.TransformState.zeroQuaternion)
			{
				this.Rotation = Quaternion.identity;
			}
			Vector3 localScale = t.localScale;
			this.Scale = new Vector3(TransformDebug.IsValid(localScale.x) ? localScale.x : this.Scale.x, TransformDebug.IsValid(localScale.y) ? localScale.y : this.Scale.y, TransformDebug.IsValid(localScale.z) ? localScale.z : this.Scale.z);
		}

		// Token: 0x040069A4 RID: 27044
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Quaternion zeroQuaternion = new Quaternion(0f, 0f, 0f, 0f);

		// Token: 0x040069A5 RID: 27045
		public Vector3 Position = Vector3.zero;

		// Token: 0x040069A6 RID: 27046
		public Quaternion Rotation = Quaternion.identity;

		// Token: 0x040069A7 RID: 27047
		public Vector3 Scale = Vector3.one;
	}
}
