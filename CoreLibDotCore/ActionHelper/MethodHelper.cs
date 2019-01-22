using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibDotCore.ActionHelper
{
    public class MethodHelper
    {
        private static Dictionary<string, ActionObj> _funcDictionary = new Dictionary<string, ActionObj>();

        #region 公开


        public static void AddAction(string token, Action action)
        {
            lock (_funcDictionary)
            {
                //已存在此token，不做处理,处理了，删除重添
                if (_funcDictionary.ContainsKey(token))
                {
                    _funcDictionary.Remove(token);
                }
                var weakeAction = new ActionBase(action, null);
                var actionObj = new ActionObj() { Action = weakeAction };
                _funcDictionary.Add(token, actionObj);
            }
        }
        public static void AddAction<T>(string token, Action<T> action)
        {
            lock (_funcDictionary)
            {
                //已存在此token，不做处理,处理了，删除重添
                if (_funcDictionary.ContainsKey(token))
                {
                    _funcDictionary.Remove(token);
                }


                var weakeAction = new ActionBase<T>(action, typeof(T));
                var actionObj = new ActionObj() { Action = weakeAction };
                _funcDictionary.Add(token, actionObj);
            }

        }

        public static void InvokeAction(string token)
        {
            ActionObj action;
            if (_funcDictionary.TryGetValue(token, out action))
            {
                (action.Action)?.Execute();
            }
        }
        public static void InvokeAction(string token, object para)
        {
            ActionObj action;
            if (_funcDictionary.TryGetValue(token, out action))
            {
                (action.Action as IExecuteWithObject)?.ExecuteWithObject(para);
            }
        }

        #endregion
    }
}
