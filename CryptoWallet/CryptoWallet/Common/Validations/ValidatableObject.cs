using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryptoWallet.Common.Base;

namespace CryptoWallet.Common.Validations
{
    public class ValidatableObject<T> : ExtendedBindableObject
    {
        public List<IValidationRule<T>> Validations { get; }

        private List<string> errors;

        public List<string> Errors
        {
            get => this.errors;
            set { SetProperty(ref this.errors, value); }
        }

        private T _value;

        public T Value
        {
            get => this._value;
            set { SetProperty(ref this._value, value); }
        }

        private bool isValid;

        public bool IsValid
        {
            get => this.isValid;
            set { SetProperty(ref this.isValid, value); }
        }


        public ValidatableObject()
        {
            this.isValid = true;
            this.errors = new List<string>();
            this.Validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            this.Errors.Clear();

            IEnumerable<string> errors = this.Validations.Where(v => !v.Check(this.Value))
                .Select(v => v.ValidationMessage);

            this.Errors = errors.ToList();
            this.IsValid = !this.Errors.Any();

            return this.IsValid;
        }
    }
}
