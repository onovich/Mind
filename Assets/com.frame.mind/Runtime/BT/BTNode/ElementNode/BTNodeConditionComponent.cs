using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNodeConditionComponent {

        BTNode node;
        public Func<bool> Condition;

        public BTNodeConditionComponent() { }

        public void Inject(BTNode node) {
            this.node = node;
        }

        public BTNodeResult Evaluate() {

            Debug.Log("BTNodeConditionComponent.Evaluate");

            if (!Condition.Invoke()) {
                return BTNodeResult.False;
            }

            return BTNodeResult.True;

        }

        public void Tick() {
        }

        public void Reset() { }

    }

}