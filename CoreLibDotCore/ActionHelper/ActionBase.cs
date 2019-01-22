using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibDotCore.ActionHelper
{
    public class ActionBase
    {
        private Action _action;

        public Type ParaType;
        protected ActionBase()
        {

        }
        public ActionBase(Action action, Type paraType)
        {
            _action = action;
            ParaType = paraType;
        }
        public virtual void Execute()
        {
            _action?.Invoke();
        }
    }
    public class ActionBase<T> : ActionBase, IExecuteWithObject
    {
        private Action<T> _action;

        public ActionBase(Action<T> action, Type paraType)
        {
            _action = action;
            ParaType = paraType;
        }

        public void ExecuteWithObject(object parameter)
        {

            if (parameter.GetType() == ParaType)
            {
                ExecuteWithPara((T)parameter);
            }

        }

        public void ExecuteWithPara(T obj)
        {
            _action?.Invoke(obj);
        }
    }
}
