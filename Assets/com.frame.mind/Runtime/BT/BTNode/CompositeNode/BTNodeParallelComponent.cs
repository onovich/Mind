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

            var endCount = 0;
            for (int i = 0; i < node.Children.Count; i++) {
                var child = node.Children[i];
                var state = child.State;
                if (state == BTNodeState.Ready) {
                    if (!child.CanEnter()) {
                        continue;
                    }
                    child.SetBTNodeState(BTNodeState.Running);
                } else if (state == BTNodeState.Running) {
                    var result = child.Evaluate();
                    if (!result) {
                        child.SetBTNodeState(BTNodeState.End);
                        endCount++;
                        break;
                    }
                }

                if (endCount > 0) {
                    Reset();
                    return false;
                }
                return true;

            }

            Reset();
            return true;

        }

        public bool EvaluateAnd() {

            var endCount = 0;
            for (int i = 0; i < node.Children.Count; i++) {
                var child = node.Children[i];
                var state = child.State;
                if (state == BTNodeState.Ready) {
                    if (!child.CanEnter()) {
                        continue;
                    }
                    child.SetBTNodeState(BTNodeState.Running);
                } else if (state == BTNodeState.Running) {
                    var result = child.Evaluate();
                    if (!result) {
                        child.SetBTNodeState(BTNodeState.End);
                        endCount++;
                    }
                } else if (state == BTNodeState.End) {
                    endCount++;
                }
            }

            if (endCount >= node.Children.Count) {
                Reset();
                return false;
            }
            return true;

        }

        public void Tick() {
            node.Children.ForEach(child => child.Tick());
        }

        public void Reset() {
            node.Children.ForEach(child => child.Reset());
        }

    }
}