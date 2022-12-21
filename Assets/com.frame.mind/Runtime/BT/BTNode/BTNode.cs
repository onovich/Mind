using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Mind {

    public class BTNode {

        // - Node
        public ushort ID;
        public BTNodeType NodeType;
        public List<BTNode> Children;

        // - Evaluate
        public bool IsActive;
        public BTNodeResult Result;

        // - Composite Node 
        public BTNodeFallbackComponent FallbackComponent;
        public BTNodeParallelComponent ParallelComponent;
        public BTNodeSequenceComponent SequenceComponent;

        // - Element Node
        public BTNodeActionComponent ActionComponent;
        public BTNodeConditionComponent ConditionComponent;

        public BTNode() {
            this.Children = new List<BTNode>();
            this.IsActive = false;
            //this.Decision = BTNodeDecision.DoNothing;
            //this.Result = BTNodeResult.False;
            //this.State = BTNodeState.Idle;
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

            if (!IsActive) {
                return false;
            }

            return true;

        }

        public void Activate() {
            this.IsActive = true;
            Children?.ForEach(child => child.Activate());
        }

        public void Deactivate() {
            this.IsActive = false;
            Children?.ForEach(child => child.Deactivate());
        }

        public BTNodeResult Evaluate() {

            if (!IsActive) {
                return BTNodeResult.False;
            }

            return FallbackComponent?.Evaluate() ??
            ParallelComponent?.Evaluate() ??
            SequenceComponent?.Evaluate() ??
            ActionComponent?.Evaluate() ??
            ConditionComponent?.Evaluate() ??
            BTNodeResult.False;

        }

        public void Tick() {

            if (!IsActive) {
                return;
            }

            FallbackComponent?.Tick();
            ParallelComponent?.Tick();
            SequenceComponent?.Tick();
            ActionComponent?.Tick();
            ConditionComponent?.Tick();

        }



        public void Reset() {

            Children.ForEach(child => child.Reset());
            FallbackComponent?.Reset();
            ParallelComponent?.Reset();
            SequenceComponent?.Reset();
            ActionComponent?.Reset();
            ConditionComponent?.Reset();

        }

    }
}