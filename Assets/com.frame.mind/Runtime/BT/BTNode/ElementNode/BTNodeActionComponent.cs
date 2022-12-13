using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeActionComponent {

        BTNode node;
        BTActionState actionState;

        public Action OnStart;
        public Func<bool> OnExcute;
        public Action OnExit;

        public BTNodeActionComponent() { }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }
            if (actionState == BTActionState.End) {
                return false;
            }
            return true;

        }

        public void Tick() {

            if (actionState == BTActionState.Ready) {
                OnStart();
                actionState = BTActionState.Running;
                return;
            }
            if (actionState == BTActionState.Running) {
                var result = OnExcute();
                if (result) {
                    OnExit();
                    actionState = BTActionState.End;
                }
                return;

            }

        }

        public void Reset() {
            actionState = BTActionState.Ready;
        }

    }

}