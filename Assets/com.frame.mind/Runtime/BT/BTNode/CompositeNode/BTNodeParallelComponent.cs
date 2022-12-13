using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNodeParallelComponent : IBTNodeComponent {

        BTNode node;
        BTNodeChildrenRelationship childrenRelationship;


        public void Inject(BTNode node) {
            this.node = node;
        }

        public bool Evaluate() {

            if (childrenRelationship == BTNodeChildrenRelationship.And) {
                return EvaluateAnd();
            } else if (childrenRelationship == BTNodeChildrenRelationship.Or) {
                return EvaluateOr();
            }
            return true;

        }

        public bool EvaluateOr() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }

            for (int i = 0; i < node.Children.Count; i++) {
                var result = node.Children[i].Evaluate();
                if (!result) {
                    return false;
                }
            }

            Reset();
            return true;

        }

        public bool EvaluateAnd() {

            if (node.Precondition != null && !node.Precondition()) {
                return false;
            }

            for (int i = 0; i < node.Children.Count; i++) {
                var result = node.Children[i].Evaluate();
                if (result) {
                    return true;
                }
            }

            Reset();
            return false;

        }

        public void Tick() {
            node.Children.ForEach(child => child.Tick());
        }

        public void Reset() {
            node.Children.ForEach(child => child.Reset());
        }

    }
}