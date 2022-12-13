using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeLoopComponent {

        BTNode node;
        public ushort RepeatCount;
        ushort CurrentCount;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }
            if (node.Children[0].Evaluate()) {
                CurrentCount++;
            }
            if (CurrentCount < RepeatCount) {
                return false;
            }

            return true;

        }

        public void Tick() {
            node.Children[0].Tick();
        }

        public void Reset() {
            CurrentCount = 0;
            node.Children.ForEach(child => child.Reset());
        }

    }
}