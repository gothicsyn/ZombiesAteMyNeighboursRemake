// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("AI/Waypoint AI/Waypoint")]
public class AIWaypoint : MonoBehaviour
{
	public AIWaypoint nextWaypoint;
	public Color pathColor = Color.grey;
	public bool wayPointIsStar = false;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") {
			if (nextWaypoint != null) {
				other.GetComponent<WaypointAI>().target = nextWaypoint;
				other.GetComponent<WaypointAI>().waypointIsStar = wayPointIsStar;
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (nextWaypoint != null) {
			Gizmos.color = pathColor;
			Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
		}
	}
	public void OnDrawGizmosSelected()
	{
		if (nextWaypoint != null) {
			Gizmos.DrawIcon(nextWaypoint.transform.position, "Waypoint AI/Waypoint_Target.png", true);
		}
	}
}