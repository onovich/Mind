using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeDelayComponent {

        BTNode node;
        public float dt;
        public float CurrentTime;
        public float DelayTime;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }
            CurrentTime -= dt;
            if (CurrentTime > 0) {
                return false;
            }
            Reset();

            return node.Children[0].Evaluate();

        }

        public void Tick() {
            node.Children[0].Tick();
        }

        public void Reset() {
            CurrentTime = DelayTime;
            node.Children.ForEach(child => child.Reset());
        }

    }
}