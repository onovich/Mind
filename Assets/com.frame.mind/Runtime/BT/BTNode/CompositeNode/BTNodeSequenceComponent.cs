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

            if (activeIndex >= node.Children.Count) {
                Reset();
                return false;
            }

            var child = node.Children[activeIndex];
            if (child.CanEnter()) {
                child.SetBTNodeState(BTNodeState.Running);
                activeIndex++;
                return true;
            }

            var result = child.Evaluate();
            if (!result) {
                child.SetBTNodeState(BTNodeState.End);
            }

            return true;

        }

        public void Tick() {
            if (activeIndex >= node.Children.Count) {
                return;
            }
            node.Children[activeIndex].Tick();
        }

        public void Reset() {
            activeIndex = 0;
            node.Children.ForEach(child => child.Reset());
        }

    }
}