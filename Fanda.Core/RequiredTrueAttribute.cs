﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core
{
    public class RequiredTrueAttribute : ValidationAttribute
    {
        #region Overrides of ValidationAttribute

        /// <summary>
        ///     Determines whether the specified value of the object is valid.
        /// </summary>
        /// <returns>
        ///     true if the specified value is valid; otherwise, false.
        /// </returns>
        /// <param name="value">
        ///     The value of the specified validation object on which the
        ///     <see cref="T:System.ComponentModel.DataAnnotations.ValidationAttribute" /> is declared.
        /// </param>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.GetType() != typeof(bool))
            {
                throw new InvalidOperationException("Can only be used on boolean properties");
            }

            return (bool)value;
        }

        #endregion Overrides of ValidationAttribute
    }
}