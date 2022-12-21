using System;
using System.Collections.Generic;

namespace MortiseFrame.Mind {

    public interface IBTNodeComponent {

        BTNodeResult Evaluate();
        void Tick();
        void Reset();

    }
}