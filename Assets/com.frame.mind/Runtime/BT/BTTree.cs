using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTTree {

        public BTNode root;

        public BTTree() { }

        public void SetRoot(BTNode root) {
            this.root = root;
        }

        public void Activate() {
            this.root.Activate();
        }

        public void Tick() {
            var res = root.Evaluate();

            //Debug.Log("BTTree.Tick() " + res.ToString());
            if (root == null) {
                throw new System.Exception("BTTree.Tick() Error");
            }
            if (!root.IsActive) {
                return;
            }
            if (res == BTNodeResult.True || res == BTNodeResult.Continue) {
                root.Tick();
            }
            if (res == BTNodeResult.False || res == BTNodeResult.True) {
                root.Reset();
            }
        }
    }
}