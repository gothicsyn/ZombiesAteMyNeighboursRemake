// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2013, 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using System;

namespace DI_Game.Attachments
{
	[AddComponentMenu("Attachments/Laser Sight")]
	public class LaserSight : MonoBehaviour
	{
		public LineRenderer lineRenderer;
		public RaycastHit hit;
		public bool hasTarget;
		public GameObject target;
		public Transform anchorPoint;

		public void OnEnable()
		{
			//lineRenderer = GetComponent<LineRenderer>();
		}

		public void LateUpdate()
		{
			if (lineRenderer != null) {
				lineRenderer.SetPosition(0, anchorPoint.position);
				if (hasTarget) {
					if (target != null) {
						if (Physics.Raycast(anchorPoint.position, target.transform.position, out hit, Mathf.Infinity)) {
							lineRenderer.SetPosition(1, hit.point);
						}
						else {
							Debug.DrawLine(anchorPoint.position, target.transform.position, Color.blue);
							lineRenderer.SetPosition(1, target.transform.position);
						}
					}
				}
				else {
					if (Physics.Raycast(anchorPoint.position, anchorPoint.forward, out hit, Mathf.Infinity)) {
						lineRenderer.SetPosition(1, hit.point);
					}
				}
			}
		}
	}
}
