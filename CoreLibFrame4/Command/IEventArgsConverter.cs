using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLibFrame4.Command
{
    public interface IEventArgsConverter
    {
        /// <summary>
        /// The method used to convert the EventArgs instance.
        /// </summary>
        /// <param name="value">An instance of EventArgs passed by the
        /// event that the EventToCommand instance is handling.</param>
        /// <param name="parameter">An optional parameter used for the conversion. Use
        /// the <see cref="EventToCommand.EventArgsConverterParameter"/> property
        /// to set this value. This may be null.</param>
        /// <returns>The converted value.</returns>
        object Convert(object value, object parameter);
    }
}
