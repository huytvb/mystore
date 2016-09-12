using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace MyStore.Framework
{
    public class MyStoreValidatorFactory : AttributedValidatorFactory
    {
        #pragma warning disable 1591
        public override FluentValidation.IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
                if ((attribute != null) && (attribute.ValidatorType != null))
                {
                    var instance = Activator.CreateInstance(attribute.ValidatorType);
                    return instance as IValidator;
                }
            }
            return null;
        }
    }
}