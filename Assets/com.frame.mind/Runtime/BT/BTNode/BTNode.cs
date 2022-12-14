using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public class BTNode {

        // - Node
        public ushort ID;
        public BTNodeType NodeType;
        public List<BTNode> Children;

        // - Evaluate
        public Func<bool> Precondition;
        BTNodeState state;
        public BTNodeState State => state;
        public void SetBTNodeState(BTNodeState state) => this.state = state;

        public bool isActive;

        // - Composite Node 
        public BTNodeFallbackComponent FallbackComponent;
        public BTNodeParallelComponent ParallelComponent;
        public BTNodeSequenceComponent SequenceComponent;

        // - Element Node
        public BTNodeActionComponent ActionComponent;

        public BTNode() {
            this.Children = new List<BTNode>();
            this.state = BTNodeState.None;
            this.isActive = true;
        }

        public void Attach(BTNode node) {
            if (this.NodeType == BTNodeType.Action || this.NodeType == BTNodeType.Condition) {
                throw new System.Exception("Action Node can not have children");
            }
            if ((this.NodeType == BTNodeType.Delay || node.NodeType == BTNodeType.Repeat) && (Children.Count > 0)) {
                throw new System.Exception("Delay Node can not have more than one child");
            }

            this.Children.Add(node);
        }

        public void DetachAll() {
            this.Children.Clear();
        }

        public bool CanEnter() {

            if (!isActive) {
                return false;
            }
            if (Precondition != null && !Precondition()) {
                return false;
            }
            return true;

        }

        public bool Evaluate() {

            if (!isActive) {
                return false;
            }

            return FallbackComponent?.Evaluate() ?? ParallelComponent?.Evaluate() ?? SequenceComponent?.Evaluate() ?? ActionComponent?.Evaluate() ?? false;

        }

        public void Tick() {

            if (!isActive) {
                return;
            }

            FallbackComponent?.Tick();
            ParallelComponent?.Tick();
            SequenceComponent?.Tick();
            ActionComponent?.Tick();

        }

        public void Reset() {

            Children.ForEach(child => child.Reset());
            FallbackComponent?.Reset();
            ParallelComponent?.Reset();
            SequenceComponent?.Reset();
            ActionComponent?.Reset();
            state = BTNodeState.End;

        }

    }
}