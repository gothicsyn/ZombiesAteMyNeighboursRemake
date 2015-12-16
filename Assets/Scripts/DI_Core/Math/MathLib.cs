/*
*
* 	Devils Inc Studios
* 	// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015, 2015
*	
*	TODO: Include a description of the file here.
*
*/

static public class MathLib {
	// Helper to wrap angles greater than 360
	static public float wrapAngle(float angle) {
		if (angle < 0) {
			angle += 360;
		}
		if (angle < -360F) {
			while (angle < -360F) {
				angle += 360F;
			}
		}
		if (angle > 360F) {
			while (angle > 360F) {
				angle -= 360F;
			}
		}
		if (angle == 360f) {
			angle = 0f;
		}
		return angle;
	}

	static public string convertToReadableTime(float time)
	{
		float hours = 0;
		float minutes = 0;
		float seconds = 0;

		while (time > 3600) {
			time -= 3600;
			hours++;
		}
		while (time > 60) {
			time -= 60;
			minutes++;
		}
		seconds = time;
		if (hours > 0) {
			return (hours + ":" + minutes + ":" + seconds);
		}
		else if (minutes > 0) {
			return (minutes + ":" + seconds);
		}
		else {
			return seconds + "";
		}
	}
}