using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace CoreLibFrame4.Command
{
    public class RelayCommand<T> : ICommand

    {

        private readonly Action<T> _execute;



        private readonly Func<T, bool> _canExecute;



        /// <summary>

        /// Initializes a new instance of the RelayCommand class that 

        /// can always execute.

        /// </summary>

        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure,

        /// you must set keepTargetAlive to true to avoid side effects. </param>

        /// <param name="keepTargetAlive">If true, the target of the Action will

        /// be kept as a hard reference, which might cause a memory leak. You should only set this

        /// parameter to true if the action is causing a closure. See

        /// http://galasoft.ch/s/mvvmweakaction. </param>

        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>

        public RelayCommand(Action<T> execute)

            : this(execute, null)

        {

        }



        /// <summary>

        /// Initializes a new instance of the RelayCommand class.

        /// </summary>

        /// <param name="execute">The execution logic. IMPORTANT: If the action causes a closure,

        /// you must set keepTargetAlive to true to avoid side effects. </param>

        /// <param name="canExecute">The execution status logic.  IMPORTANT: If the func causes a closure,

        /// you must set keepTargetAlive to true to avoid side effects. </param>

        /// <param name="keepTargetAlive">If true, the target of the Action will

        /// be kept as a hard reference, which might cause a memory leak. You should only set this

        /// parameter to true if the action is causing a closure. See

        /// http://galasoft.ch/s/mvvmweakaction. </param>

        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)

        {

            if (execute == null)

            {

                throw new ArgumentNullException("execute");

            }



            _execute = new Action<T>(execute);



            if (canExecute != null)

            {

                _canExecute = new Func<T, bool>(canExecute);

            }

        }



#if SILVERLIGHT

        /// <summary>

        /// Occurs when changes occur that affect whether the command should execute.

        /// </summary>

        public event EventHandler CanExecuteChanged;

#elif NETFX_CORE

        /// <summary>

        /// Occurs when changes occur that affect whether the command should execute.

        /// </summary>

        public event EventHandler CanExecuteChanged;

#elif XAMARIN

        /// <summary>

        /// Occurs when changes occur that affect whether the command should execute.

        /// </summary>

        public event EventHandler CanExecuteChanged;

#else

        /// <summary>

        /// Occurs when changes occur that affect whether the command should execute.

        /// </summary>

        public event EventHandler CanExecuteChanged

        {

            add

            {

                if (_canExecute != null)

                {

                    CommandManager.RequerySuggested += value;

                }

            }



            remove

            {

                if (_canExecute != null)

                {

                    CommandManager.RequerySuggested -= value;

                }

            }

        }

#endif



        /// <summary>

        /// Raises the <see cref="CanExecuteChanged" /> event.

        /// </summary>

        [SuppressMessage(

            "Microsoft.Performance",

            "CA1822:MarkMembersAsStatic",

            Justification = "The this keyword is used in the Silverlight version")]

        [SuppressMessage(

            "Microsoft.Design",

            "CA1030:UseEventsWhereAppropriate",

            Justification = "This cannot be an event")]

        public void RaiseCanExecuteChanged()

        {

#if SILVERLIGHT

            var handler = CanExecuteChanged;

            if (handler != null)

            {

                handler(this, EventArgs.Empty);

            }

#elif NETFX_CORE

            var handler = CanExecuteChanged;

            if (handler != null)

            {

                handler(this, EventArgs.Empty);

            }

#elif XAMARIN

            var handler = CanExecuteChanged;

            if (handler != null)

            {

                handler(this, EventArgs.Empty);

            }

#else

            CommandManager.InvalidateRequerySuggested();

#endif

        }



        /// <summary>

        /// Defines the method that determines whether the command can execute in its current state.

        /// </summary>

        /// <param name="parameter">Data used by the command. If the command does not require data 

        /// to be passed, this object can be set to a null reference</param>

        /// <returns>true if this command can be executed; otherwise, false.</returns>

        public bool CanExecute(object parameter)

        {

            if (_canExecute == null)

            {

                return true;

            }




            if (parameter == null

#if NETFX_CORE

                    && typeof(T).GetTypeInfo().IsValueType)

#else

                && typeof(T).IsValueType)

#endif

            {

                return _canExecute(default(T));

            }



            if (parameter == null || parameter is T)

            {

                return _canExecute((T)parameter);

            }





            return false;

        }



        /// <summary>

        /// Defines the method to be called when the command is invoked. 

        /// </summary>

        /// <param name="parameter">Data used by the command. If the command does not require data 

        /// to be passed, this object can be set to a null reference</param>

        public virtual void Execute(object parameter)

        {

            var val = parameter;



#if !NETFX_CORE

            if (parameter != null

                && parameter.GetType() != typeof(T))

            {

                if (parameter is IConvertible)

                {

                    val = Convert.ChangeType(parameter, typeof(T), null);

                }

            }

#endif



            if (CanExecute(val)

                && _execute != null)
            {

                if (val == null)

                {

#if NETFX_CORE

                    if (typeof(T).GetTypeInfo().IsValueType)

#else

                    if (typeof(T).IsValueType)

#endif

                    {

                        _execute(default(T));

                    }

                    else

                    {

                        // ReSharper disable ExpressionIsAlwaysNull

                        _execute((T)val);

                        // ReSharper restore ExpressionIsAlwaysNull

                    }

                }

                else

                {

                    _execute((T)val);

                }

            }

        }

    }
}
