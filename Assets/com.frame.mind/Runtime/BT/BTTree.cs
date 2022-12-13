using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public abstract class BTTree {

        public BTNode root;

        public BTTree() {
        }

        public void SetRoot(BTNode root) {
            this.root = root;
        }

        public void Tick() {
            var res = root.Evaluate();
            if (res) {
                root.Tick();
            }
        }
    }
}