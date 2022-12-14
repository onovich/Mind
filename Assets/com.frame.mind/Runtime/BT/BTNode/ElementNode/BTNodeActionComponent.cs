using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeActionComponent {

        BTNode node;
        BTNodeState actionState;

        public Action OnStart;
        public Func<bool> OnExcute;
        public Action OnExit;

        public BTNodeActionComponent() { }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (actionState == BTNodeState.End) {
                return false;
            }
            return true;

        }

        public void Tick() {

            if (actionState == BTNodeState.Ready) {
                OnStart();
                actionState = BTNodeState.Running;
                return;
            }
            if (actionState == BTNodeState.Running) {
                var result = OnExcute();
                if (result) {
                    OnExit();
                    actionState = BTNodeState.End;
                }
                return;

            }

        }

        public void Reset() {
            actionState = BTNodeState.Ready;
        }

    }

}