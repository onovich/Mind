using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeFallbackComponent : IBTNodeComponent {

        BTNode node;
        short activeIndex;
        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (activeIndex >= node.Children.Count) {
                Reset();
                return false;
            }
            var child = node.Children[activeIndex];
            if (!child.CanEnter()) {
                activeIndex++;
                return false;
            }

            var result = node.Children[activeIndex].Evaluate();
            if (!result) {
                child.SetBTNodeState(BTNodeState.Running);
                return false;
            }

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