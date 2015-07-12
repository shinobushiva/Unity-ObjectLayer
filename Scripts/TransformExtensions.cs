using UnityEngine;
using System.Collections;

public static class TransformExtensions
{
	public static void SetLayer(this Transform trans, int layer) 
	{
		trans.gameObject.layer = layer;
		foreach(Transform child in trans)
			child.SetLayer( layer);
	}

	public static void SetCollision(this Transform trans, bool col) 
	{
		Collider[] cs = trans.GetComponentsInChildren<Collider> ();
		foreach (Collider c in cs)
			c.enabled = col;

		Collider cc = trans.GetComponent<Collider>();
		if(cc)
			cc.enabled = col;
	}
}
