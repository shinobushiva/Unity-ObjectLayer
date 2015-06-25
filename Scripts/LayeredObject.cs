using UnityEngine;
using System.Collections;

namespace ObjectLayer {
	public class LayeredObject : MonoBehaviour {

		public string layerName;

		private ObjectLayerMaster master;

		private int orgLayer;

		public bool visible = true;

		private int invisibleLayer;

		void Awake(){
//			orgLayer = gameObject.layer;
			invisibleLayer = LayerMask.NameToLayer ("Invisible");
		}

		// Use this for initialization
		void Start () {
			master = FindObjectOfType<ObjectLayerMaster> ();
			master.AddObject (this);
			 
		}

		public void SetVisible(bool b){
			visible = b;
			if (b) {
				gameObject.transform.SetLayer(orgLayer);
			} else {
				if(gameObject.layer != invisibleLayer){
					orgLayer = gameObject.layer;
					gameObject.transform.SetLayer(invisibleLayer);
				}
			}
		}
	}
}
