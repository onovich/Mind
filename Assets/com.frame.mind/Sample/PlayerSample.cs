using MortiseFrame.Mind;
using UnityEngine;
using System;

namespace MortiseFrame.Sample {

    public class PlayerSample : MonoBehaviour {

        public byte status;

        private void Start() {
            status = 0;
        }

        void switchStatus() {
            if (status == 0) {
                status = 1;
            } else {
                status = 0;
            }
            Debug.Log("切换状态:"+status);
        }

        void move() {
            if (Input.GetKey(KeyCode.W)) {
                transform.Translate(Vector2.up * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S)) {
                transform.Translate(Vector2.down * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A)) {
                transform.Translate(Vector2.left * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D)) {
                transform.Translate(Vector2.right * Time.deltaTime);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                switchStatus();
            }
            move();
        }

    }
}