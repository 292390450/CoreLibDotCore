using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace CoreLibFrame4.Command
{
    public class RelayCommand : ICommand

    {

        private readonly Action _execute;



        private readonly Func<bool> _canExecute;



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

        public RelayCommand(Action execute)

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

        /// parameter to true if the action is causing a closures. See

        /// http://galasoft.ch/s/mvvmweakaction. </param>

        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>

        public RelayCommand(Action execute, Func<bool> canExecute)

        {

            if (execute == null)

            {

                throw new ArgumentNullException("execute");

            }



            _execute = new Action(execute);



            if (canExecute != null)

            {

                _canExecute = new Func<bool>(canExecute);

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

        private EventHandler _requerySuggestedLocal;



        /// <summary>

        /// Occurs when changes occur that affect whether the command should execute.

        /// </summary>

        public event EventHandler CanExecuteChanged

        {

            add

            {

                if (_canExecute != null)

                {

                    // add event handler to local handler backing field in a thread safe manner

                    EventHandler handler2;

                    EventHandler canExecuteChanged = _requerySuggestedLocal;



                    do

                    {

                        handler2 = canExecuteChanged;

                        EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);

                        canExecuteChanged = System.Threading.Interlocked.CompareExchange(

                            ref _requerySuggestedLocal,

                            handler3,

                            handler2);

                    }

                    while (canExecuteChanged != handler2);



                    CommandManager.RequerySuggested += value;

                }

            }



            remove

            {

                if (_canExecute != null)

                {

                    // removes an event handler from local backing field in a thread safe manner

                    EventHandler handler2;

                    EventHandler canExecuteChanged = _requerySuggestedLocal;



                    do

                    {

                        handler2 = canExecuteChanged;

                        EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);

                        canExecuteChanged = System.Threading.Interlocked.CompareExchange(

                            ref _requerySuggestedLocal,

                            handler3,

                            handler2);

                    }

                    while (canExecuteChanged != handler2);



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

        /// <param name="parameter">This parameter will always be ignored.</param>

        /// <returns>true if this command can be executed; otherwise, false.</returns>

        public bool CanExecute(object parameter)

        {

            return _canExecute == null



                    || _canExecute();

        }



        /// <summary>

        /// Defines the method to be called when the command is invoked. 

        /// </summary>

        /// <param name="parameter">This parameter will always be ignored.</param>

        public virtual void Execute(object parameter)

        {

            if (CanExecute(parameter)

                && _execute != null

                )

            {

                _execute();

            }

        }

    }
}
