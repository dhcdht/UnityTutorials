using UnityEngine;

namespace _2SpaceShooter
{
	public class Done_BGScroller : MonoBehaviour {

		public float ScrollSpeed;
		public float TileSizeZ;

		private Vector3 _startPosition;

		// Use this for initialization
		void Start () {
			_startPosition = transform.position;
		}

		// Update is called once per frame
		void Update () {
			float newPosition = Mathf.Repeat(Time.time * ScrollSpeed, TileSizeZ);
			transform.position = _startPosition + Vector3.forward * newPosition;
		}
	}
}
