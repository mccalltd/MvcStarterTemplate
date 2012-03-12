using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StarterTemplate.Core.Data;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;
using StructureMap;

namespace StarterTemplate.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EmailIsRegisteredAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "That email address is not registered.";

        public EmailIsRegisteredAttribute()
        {
            ErrorMessage = DefaultErrorMessage;
        }

        public IRepository Repository { get; set; }

        public override bool IsValid(object value)
        {
            var emailAddress = value as string;
            if (!emailAddress.HasValue())
                return true;

            var repository = Repository ?? ObjectFactory.GetInstance<IRepository>();

            return repository.First<Member>(m => m.EmailAddress == emailAddress) != null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRemoteRule(
                FormatErrorMessage(metadata.GetDisplayName()),
                "/validate/email-is-registered",
                "POST",
                null);
        }
    }
}