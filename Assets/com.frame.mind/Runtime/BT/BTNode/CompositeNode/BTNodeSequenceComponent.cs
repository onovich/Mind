using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeSequenceComponent : IBTNodeComponent {

        BTNode node;
        short activeIndex;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }
            if (activeIndex >= node.Children.Count) {
                Reset();
                return false;
            }
            var result = node.Children[activeIndex].Evaluate();
            if (!result) {
                return false;
            }
            activeIndex++;
            return true;

        }

        public void Tick() {
            node.Children[activeIndex].Tick();
        }

        public void Reset() {
            activeIndex = 0;
            node.Children.ForEach(child => child.Reset());
        }

    }
}