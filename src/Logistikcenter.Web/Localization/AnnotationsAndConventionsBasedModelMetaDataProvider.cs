using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Resources;

namespace Logistikcenter.Web.Localization
{
    public class AnnotationsAndConventionsBasedModelMetaDataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var result = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (string.IsNullOrEmpty(result.DisplayName) && containerType != null && propertyName != null)
            {
                var keyForDisplayValue = string.Format("{0}_{1}", containerType.Name, propertyName);
                var translatedValue = ViewModels.ResourceManager.GetObject(keyForDisplayValue) as string;
                
                if (!string.IsNullOrEmpty(translatedValue))
                {
                    result.DisplayName = translatedValue;
                }
            }

            return result;
        }
    }
}