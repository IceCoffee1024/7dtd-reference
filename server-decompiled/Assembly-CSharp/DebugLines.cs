using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011AD RID: 4525
public class DebugLines : MonoBehaviour
{
	// Token: 0x0600907C RID: 36988 RVA: 0x00365F22 File Offset: 0x00364122
	public static DebugLines Create(string _name, Transform _parentT, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		return debugLines;
	}

	// Token: 0x0600907D RID: 36989 RVA: 0x00365F58 File Offset: 0x00364158
	public static DebugLines Create(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, _pos1 - Origin.position);
		lineRenderer.SetPosition(1, _pos2 - Origin.position);
		return debugLines;
	}

	// Token: 0x0600907E RID: 36990 RVA: 0x00365FC4 File Offset: 0x003641C4
	public static DebugLines CreateAttached(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.useWorldSpace = false;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		Vector3 vector = _parentT.InverseTransformPoint(_pos1 - Origin.position);
		lineRenderer.SetPosition(0, vector);
		Vector3 vector2 = _parentT.InverseTransformPoint(_pos2 - Origin.position);
		lineRenderer.SetPosition(1, vector2);
		return debugLines;
	}

	// Token: 0x0600907F RID: 36991 RVA: 0x00366048 File Offset: 0x00364248
	[PublicizedFrom(EAccessModifier.Private)]
	public static DebugLines Create(string _name, Transform _parentT)
	{
		DebugLines debugLines = null;
		string text = "DebugLines";
		if (_name != null)
		{
			text += _name;
			DebugLines.lines.TryGetValue(_name, out debugLines);
		}
		if (!debugLines)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load("Prefabs/Debug/DebugLines"), _parentT);
			gameObject.name = text;
			debugLines = gameObject.transform.GetComponent<DebugLines>();
			if (_name != null)
			{
				debugLines.keyName = _name;
				DebugLines.lines[_name] = debugLines;
			}
		}
		else
		{
			debugLines.transform.SetParent(_parentT, false);
		}
		debugLines.line = debugLines.GetComponent<LineRenderer>();
		debugLines.line.positionCount = 0;
		return debugLines;
	}

	// Token: 0x06009080 RID: 36992 RVA: 0x003660E4 File Offset: 0x003642E4
	public void AddPoint(Vector3 _pos)
	{
		int positionCount = this.line.positionCount;
		this.line.positionCount = positionCount + 1;
		Vector3 vector = _pos - Origin.position;
		if (!this.line.useWorldSpace)
		{
			vector = base.transform.InverseTransformPoint(vector);
		}
		this.line.SetPosition(positionCount, vector);
	}

	// Token: 0x06009081 RID: 36993 RVA: 0x00366140 File Offset: 0x00364340
	public void AddCube(Vector3 _cornerPos1, Vector3 _cornerPos2)
	{
		Vector3 vector = _cornerPos1;
		Vector3 vector2 = _cornerPos2 - _cornerPos1;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
	}

	// Token: 0x06009082 RID: 36994 RVA: 0x003662C6 File Offset: 0x003644C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.duration -= Time.deltaTime;
		if (this.duration <= 0f)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06009083 RID: 36995 RVA: 0x003662F2 File Offset: 0x003644F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (this.keyName != null)
		{
			DebugLines.lines.Remove(this.keyName);
		}
	}

	// Token: 0x04006ACE RID: 27342
	public static Vector3 InsideOffsetV = new Vector3(0.05f, 0.05f, 0.05f);

	// Token: 0x04006ACF RID: 27343
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cName = "DebugLines";

	// Token: 0x04006AD0 RID: 27344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<string, DebugLines> lines = new Dictionary<string, DebugLines>();

	// Token: 0x04006AD1 RID: 27345
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string keyName;

	// Token: 0x04006AD2 RID: 27346
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float duration;

	// Token: 0x04006AD3 RID: 27347
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer line;
}
