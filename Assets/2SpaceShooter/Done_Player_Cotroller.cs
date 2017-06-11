﻿using System.Security.AccessControl;
using UnityEngine;

namespace _2SpaceShooter
{
    public class Done_Boundary
    {
        public float xMin, xMax, zMin, zMax;
    }

    public class Done_Player_Cotroller : MonoBehaviour
    {
        public float speed;
        public float tilt;
        public Done_Boundary boundary;

        public GameObject shot;
        public Transform shotSpawn;
        public float fireRate;

        private float nextFire;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }
        }

        private void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;

            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

            GetComponent<Rigidbody>().rotation =
                Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }
    }
}