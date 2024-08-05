using System;

namespace MortiseFrame.Mind {

    public static class BTNodeFactory {

        static ushort NodeIDRecord;

        public static ushort GetNodeID() {
            return NodeIDRecord++;
        }

        // - Composite Node

        public static BTNode CreatCompositeNode_Sequence(IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Sequence;
            var sequenceComponent = new BTNodeSequenceComponent();
            sequenceComponent.Inject(node);

            node.SequenceComponent = sequenceComponent;
            node.ID = GetNodeID();

            return node;

        }

        public static BTNode CreatCompositeNode_Fallback(IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Fallback;
            var fallbackComponent = new BTNodeFallbackComponent();
            fallbackComponent.Inject(node);

            node.FallbackComponent = fallbackComponent;
            node.ID = GetNodeID();

            return node;

        }

        public static BTNode CreatCompositeNode_ParallelAnd(IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Parallel;
            var parallelComponent = new BTNodeParallelComponent(true);
            parallelComponent.Inject(node);

            node.ParallelComponent = parallelComponent;
            node.ID = GetNodeID();

            return node;

        }

        public static BTNode CreatCompositeNode_ParallelOr(IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Parallel;
            var parallelComponent = new BTNodeParallelComponent(false);
            parallelComponent.Inject(node);

            node.ParallelComponent = parallelComponent;
            node.ID = GetNodeID();

            return node;

        }

        // - Element Node

        public static BTNode CreatElementNode_Action(IBTAction Action, IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Action;
            var actionComponent = new BTNodeActionComponent();
            actionComponent.OnEnter = Action.OnEnter;
            actionComponent.OnExcute = Action.OnExcute;
            actionComponent.OnExit = Action.OnExit;
            actionComponent.Inject(node);

            node.ActionComponent = actionComponent;
            node.ID = GetNodeID();

            return node;

        }


        public static BTNode CreatElementNode_Condition(Func<bool> Condition, IBTPrecondition precondition = null) {

            var node = new BTNode(precondition);
            node.NodeType = BTNodeType.Condition;
            var conditionComponent = new BTNodeConditionComponent();
            conditionComponent.Condition = Condition;
            conditionComponent.Inject(node);

            node.ConditionComponent = conditionComponent;
            node.ID = GetNodeID();

            return node;

        }


    }
}
