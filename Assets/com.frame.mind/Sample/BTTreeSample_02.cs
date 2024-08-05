using MortiseFrame.Mind;
using UnityEngine;
using System;

namespace MortiseFrame.Sample {

    public class BTTreeSample_02 : MonoBehaviour {

        BTTree tree;
        public Transform target;

        private void Start() {
            Test1();
        }

        private void Update() {
            tree.Tick();
        }

        public void Test1() {

            this.tree = new BTTree();
            var sequence1 = BTNodeFactory.CreatCompositeNode_Sequence();
            var fallback1 = BTNodeFactory.CreatCompositeNode_Fallback();



        }



    }

}