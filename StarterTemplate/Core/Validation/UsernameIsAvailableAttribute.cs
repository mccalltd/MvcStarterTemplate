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
    public class UsernameIsAvailableAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "That username is already in use.";

        public UsernameIsAvailableAttribute()
        {
            ErrorMessage = DefaultErrorMessage;
        }

        public IRepository Repository { get; set; }

        public override bool IsValid(object value)
        {
            var username = value as string;
            if (!username.HasValue())
                return true;

            var repository = Repository ?? ObjectFactory.GetInstance<IRepository>();

            return repository.First<Member>(m => m.Username == username) == null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRemoteRule(
                FormatErrorMessage(metadata.GetDisplayName()),
                "/validate/username-is-available",
                "POST",
                null);
        }
    }
}