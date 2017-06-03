using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public float Speed;
	public Text CountText;
	public Text WinText;

	private Rigidbody _rb;
	private int _count;

	// Use this for initialization
	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_count = 0;
		WinText.text = "";
		
		SetCountText();
	}

	private void FixedUpdate()
	{
		var moveHorizontal = Input.GetAxis("Horizontal");
		var moveVertical = Input.GetAxis("Vertical");

		var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		_rb.AddForce(movement * Speed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Pick Up")) return;
		
		other.gameObject.SetActive(false);
		_count += 1;
		SetCountText();
	}

	private void SetCountText()
	{
		CountText.text = string.Format("Count: {0}", _count);
		if (_count >= 3)
		{
			WinText.text = "You win!";
		}
	}
}
