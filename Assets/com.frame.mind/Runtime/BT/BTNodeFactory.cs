using System;

namespace MortiseFrame.Mind {

    public abstract class BTNodeFactory<T> {

        static ushort NodeIDRecord;
        public BTNodeFactory() {
            NodeIDRecord = 0;
        }

        public ushort GetNodeID() {
            return NodeIDRecord++;
        }

        // - Composite Node

        public BTNode CreatCompositeNode_Sequence(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Sequence;
            var sequenceComponent = new BTNodeSequenceComponent();
            sequenceComponent.Inject(node);

            node.SequenceComponent = sequenceComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatCompositeNode_Fallback(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Fallback;
            node.Precondition = precondition;
            var fallbackComponent = new BTNodeFallbackComponent();
            fallbackComponent.Inject(node);

            node.FallbackComponent = fallbackComponent;
            node.ID = GetNodeID();

            return node;

        }

        public BTNode CreatCompositeNode_Parallel(Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Parallel;
            node.Precondition = precondition;
            var parallelComponent = new BTNodeParallelComponent();
            parallelComponent.Inject(node);

            node.ParallelComponent = parallelComponent;
            node.ID = GetNodeID();

            return node;

        }

        // - Element Node

        public BTNode CreatElementNode_Action(Action OnStart, Func<bool> OnExcute, Action OnExit, Func<bool> precondition) {

            var node = new BTNode();
            node.NodeType = BTNodeType.Action;
            node.Precondition = precondition;
            var actionComponent = new BTNodeActionComponent();
            actionComponent.OnStart = OnStart;
            actionComponent.OnExcute = OnExcute;
            actionComponent.OnExit = OnExit;
            actionComponent.Inject(node);

            node.ActionComponent = actionComponent;
            node.ID = GetNodeID();

            return node;

        }

    }
}
