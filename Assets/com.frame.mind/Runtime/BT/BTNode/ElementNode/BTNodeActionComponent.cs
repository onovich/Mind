using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNodeActionComponent {


        enum BTActionResult : byte {
            Ready,
            Running,
            End,
        }


        BTNode node;
        public Action OnEnter;
        public Action OnExit;
        public Func<bool> OnExcute;
        BTActionResult actionResult;
        public Func<bool> ExitCondition;

        public BTNodeActionComponent() {
            actionResult = BTActionResult.Ready;
        }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public BTNodeResult Evaluate() {

            if (ExitCondition() && actionResult == BTActionResult.End) {
                return node.Result = BTNodeResult.True;
            }
            return node.Result = BTNodeResult.Continue;

        }

        public void Tick() {

            if (actionResult == BTActionResult.End) {
                return;
            }

            if (actionResult == BTActionResult.Running) {
                bool res = OnExcute.Invoke();
                if (res) {
                    actionResult = BTActionResult.End;
                    OnExit?.Invoke();
                    Debug.Log("OnExit");
                }
                return;
            }

            if (actionResult == BTActionResult.Ready) {
                OnEnter?.Invoke();
                Debug.Log("OnEnter");
                actionResult = BTActionResult.Running;
                return;
            }

        }

        public void Reset() {
            actionResult = BTActionResult.Ready;
        }

    }

}