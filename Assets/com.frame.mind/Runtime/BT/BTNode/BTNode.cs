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
        public BTNodeState State;
        public bool isActive;

        // - Composite Node 
        public BTNodeFallbackComponent FallbackComponent;
        public BTNodeParallelComponent ParallelComponent;
        public BTNodeSequenceComponent SequenceComponent;

        // - Decorator Node
        public BTNodeDelayComponent DelayComponent;
        public BTNodeLoopComponent LoopComponent;

        // - Element Node
        public BTNodeActionComponent ActionComponent;

        public BTNode() {
            this.Children = new List<BTNode>();
            this.State = BTNodeState.None;
            this.isActive = false;
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

        public bool Evaluate() {

            // Composite Node

            if (this.NodeType == BTNodeType.Sequence) {
                if (this.SequenceComponent != null) {
                    return this.SequenceComponent.Evaluate();
                }
            }
            if (this.NodeType == BTNodeType.Fallback) {
                if (this.FallbackComponent != null) {
                    return this.FallbackComponent.Evaluate();
                }
            }
            if (this.NodeType == BTNodeType.Parallel) {
                if (this.ParallelComponent != null) {
                    return this.ParallelComponent.Evaluate();
                }
            }

            // Decorator Node

            if (this.NodeType == BTNodeType.Delay) {
                if (this.DelayComponent != null) {
                    return this.DelayComponent.Evaluate();
                }
            }
            if (this.NodeType == BTNodeType.Repeat) {
                if (this.LoopComponent != null) {
                    return this.LoopComponent.Evaluate();
                }
            }

            // Element Node

            if (this.NodeType == BTNodeType.Action) {
                if (this.ActionComponent != null) {
                    return this.ActionComponent.Evaluate();
                }
            }
            return false;
        }

        public void Tick() {

            // Composite Node

            if (this.NodeType == BTNodeType.Sequence) {
                if (this.SequenceComponent != null) {
                    this.SequenceComponent.Tick();
                }
            }

            if (this.NodeType == BTNodeType.Fallback) {
                if (this.FallbackComponent != null) {
                    this.FallbackComponent.Tick();
                }
            }

            if (this.NodeType == BTNodeType.Parallel) {
                if (this.ParallelComponent != null) {
                    this.ParallelComponent.Tick();
                }
            }

            // Element Node

            if (this.NodeType == BTNodeType.Action) {
                if (this.ActionComponent != null) {
                    this.ActionComponent.Tick();
                }
            }

        }

        public void Reset() {

            Children.ForEach(child => child.Reset());
            FallbackComponent?.Reset();
            ParallelComponent?.Reset();
            SequenceComponent?.Reset();
            DelayComponent?.Reset();
            LoopComponent?.Reset();
            ActionComponent?.Reset();
            State = BTNodeState.End;

        }

    }
}