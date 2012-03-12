using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Configuration
{
    public class ApplicationModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            // Apply convention-based naming schemes
            if (metadata.DisplayName.HasNoValue() && propertyName.HasValue())
            {
                var displayName = propertyName.ToWords();

                if (displayName.EndsWith(" Id"))
                    displayName = displayName.Substring(0, displayName.Length - 3);

                if (typeof(bool) == metadata.ModelType)
                    displayName += "?";

                displayName = displayName.Replace(" Or ", " or ");

                metadata.DisplayName = displayName;
            }

            // Set the default placeholder text for the input
            if (metadata.Watermark.HasNoValue())
            {
                metadata.Watermark = metadata.DisplayName;
            }

            return metadata;
        }
    }
}