using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeConditionComponent {

        BTNode node;
        public Func<bool> Condition;

        public BTNodeConditionComponent() {
        }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }
            if (!Condition()) {
                return false;
            }
            return true;

        }

        public void Tick() {

        }

        public void Reset() {

        }
        
    }

}